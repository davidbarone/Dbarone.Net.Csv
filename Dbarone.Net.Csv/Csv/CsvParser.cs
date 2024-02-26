using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Dbarone.Net.Csv
{
    /// <summary>
    /// Parses a csv file per https://datatracker.ietf.org/doc/html/rfc4180.
    /// </summary>
    public class CsvParser
    {
        CsvConfiguration Configuration { get; set; }

        /// <summary>
        /// Creates a new configured CsvParser instance.
        /// </summary>
        /// <param name="configuration">The csv configuration.</param>
        public CsvParser(CsvConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Creates a new CsvParser instance with default configuration.
        /// </summary>
        public CsvParser()
        {
            this.Configuration = new CsvConfiguration();
        }

        /// <summary>
        /// Parses a csv file.
        /// </summary>
        /// <param name="stream">The input csv stream object.</param>
        /// <param name="headers">Optional array of header names if the csv file does not contain headers.</param>
        /// <returns>Parses the CSV and returns a StringDictionary object for each row.</returns>
        /// <exception cref="CsvException">Throws an exception under various error conditions.</exception>
        public IEnumerable<StringDictionary> Parse(Stream stream, string[]? headers = null)
        {
            StreamReader sr = new StreamReader(stream);
            int line = 0;                       // line in file
            int record = 0;                     // records processed
            int fieldCount = 0;

            try
            {
                var tokeniser = new Tokeniser(this.Configuration);
                string[]? tokens;

                while (!tokeniser.IsEOF)
                {
                    tokens = tokeniser.Tokenise(sr);
                    line += tokeniser.LinesLastProcessed;
                    record++;

                    if (tokens == null)
                    {
                        // ignore blank records
                    }
                    else
                    {
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
                                throw new CsvException($"Column mismatch at line {line}. Fields = {tokens.Length}, Headers = {headers.Length}.");
                            }

                            // return a StringDictionary
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