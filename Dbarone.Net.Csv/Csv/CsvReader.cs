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
        /// Creates a new configured CsvReader instance from a stream and configueration.
        /// </summary>
        /// <param name="stream">The csv stream to read.</param>
        /// <param name="configuration">The csv configuration.</param>
        public CsvReader(Stream stream, CsvConfiguration configuration)
        {
            this.Stream = stream;
            this.Configuration = configuration;
        }

        /// <summary>
        /// Creates a new configured CsvReader instance from a stream.
        /// </summary>
        /// <param name="stream">The csv stream to read.</param>
        public CsvReader(Stream stream)
        {
            this.Stream = stream;
            this.Configuration = new CsvConfiguration();
        }

        /// <summary>
        /// Reads a csv file.
        /// </summary>
        /// <returns>Parses the csv file and returns a dictionary object for each row.</returns>
        /// <exception cref="CsvException">Throws an exception under various error conditions.</exception>
        public IEnumerable<IDictionary<string, object>> Read()
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

                        // process data
                        // where record > 1 or file doesn't have header
                        if (tokens is not null && !(record == 1 && this.Configuration.HasHeader))
                        {
                            // For data rows, check field count matches header count
                            object[]? values = new object[tokens.Length];
                            Array.Copy(tokens, values, tokens.Length);
                            if (values is not null)
                            {
                                if (tokens.Length != headers!.Length)
                                {
                                    var handler = this.Configuration.ProcessRowErrorHandler ?? CsvConfiguration.RowProcessError;

                                    if (!handler(record, headers, ref values))
                                    {
                                        throw new CsvException($"Column mismatch at record {record}. Fields = {tokens.Length}, Headers = {headers.Length}.");
                                    }
                                }

                                // return a StringDictionary
                                var processRowHandler = this.Configuration.ProcessRowHandler ?? CsvConfiguration.RowProcessDefault;

                                if (!processRowHandler(record, headers, ref values))
                                {
                                    throw new CsvException($"Exception thrown at record: {record}.");
                                }

                                if (values != null)
                                {
                                    if (headers.Count() != values.Length)
                                    {
                                        throw new CsvException($"Values count mismatch at record {record}.");
                                    }

                                    Dictionary<string, object> dict = new Dictionary<string, object>();
                                    for (int i = 0; i < values.Length; i++)
                                    {
                                        dict[headers[i]] = values[i];
                                    }

                                    // Allow text values to be transformed
                                    yield return dict;
                                }
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