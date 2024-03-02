using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Dbarone.Net.Csv
{
    /// <summary>
    /// Reads a csv file per https://datatracker.ietf.org/doc/html/rfc4180.
    /// </summary>
    public class CsvReader
    {
        CsvConfiguration Configuration { get; set; }
        Stream Stream { get; set; }


        /// <summary>
        /// Creates a new configured CsvReader instance.
        /// </summary>
        /// <param name="configuration">The csv configuration.</param>
        public CsvReader(Stream stream, CsvConfiguration configuration)
        {
            this.Stream = stream;
            this.Configuration = configuration;
        }

        /// <summary>
        /// Creates a new CsvReader instance with default configuration.
        /// </summary>
        public CsvReader(Stream stream)
        {
            this.Stream = stream;
            this.Configuration = new CsvConfiguration();
        }

        /// <summary>
        /// Reads a csv file.
        /// </summary>
        /// <returns>Parses the csv file and returns a StringDictionary object for each row.</returns>
        /// <exception cref="CsvException">Throws an exception under various error conditions.</exception>
        public IEnumerable<StringDictionary> Read()
        {
            using (StreamReader sr = new StreamReader(this.Stream))
            {
                int line = 0;                       // line in file
                int record = 0;                     // records processed
                int fieldCount = 0;
                var headers = this.Configuration.Headers;

                try
                {
                    var tokeniser = new Tokeniser(this.Configuration);
                    string[]? tokens;

                    while (!tokeniser.IsEOF)
                    {
                        tokens = tokeniser.Tokenise(sr);
                        line += tokeniser.LinesLastProcessed;
                        record++;

                        if (record == 1)
                        {
                            // first record, headers in csv file. Header array can be provided by caller.
                            if (headers == null)
                            {
                                if (Configuration.HasHeader)
                                {
                                    // use headers in first record of csv file
                                    headers = tokens;
                                }
                                else
                                {
                                    // generate new headers for each field in 1st data record
                                    fieldCount = tokens.Length;
                                    List<string> tempHeaders = new List<string>();
                                    for (int h = 1; h <= fieldCount; h++)
                                    {
                                        tempHeaders.Add($"Column{h}");
                                    }
                                    headers = tempHeaders.ToArray();
                                }
                            }

                            // Check for duplicate headers
                            if (headers.Distinct().Count() < headers.Count())
                            {
                                throw new CsvException("Header fields must be unique.");
                            }
                            fieldCount = headers.Length;
                        }

                        // process data: where record > 1 or file doesn't have header
                        if (!(record == 1 && this.Configuration.HasHeader))
                        {
                            // For data rows, check field count matches header count
                            if (tokens.Length != headers!.Length)
                            {
                                if (this.Configuration.InvalidRowHandler != null)
                                {
                                    try
                                    {
                                        if (!this.Configuration.InvalidRowHandler(record, headers, ref tokens))
                                        {
                                            throw new CsvException($"Column mismatch at record {record}. Fields = {tokens.Length}, Headers = {headers.Length}.");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        throw new CsvException($"Column mismatch at record {record}. Fields = {tokens.Length}, Headers = {headers.Length}.");
                                    }
                                }
                                else
                                {
                                    throw new CsvException($"Column mismatch at record {record}. Fields = {tokens.Length}, Headers = {headers.Length}.");
                                }
                            }

                            // return a StringDictionary
                            if (tokens != null)
                            {
                                StringDictionary sd = new StringDictionary();

                                for (int f = 0; f < tokens.Length; f++)
                                {
                                    sd[headers[f]] = tokens[f];
                                }
                                yield return sd;
                            }
                        }
                    }
                }
                finally
                {
                    sr.Close();
                }
            }
        }
    }
}