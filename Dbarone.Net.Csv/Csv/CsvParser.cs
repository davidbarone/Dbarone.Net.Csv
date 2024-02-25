using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
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
        /// <returns>Parses the CSV and returns a StringDictionary object for each row.</returns>
        public IEnumerable<StringDictionary> Parse(Stream stream)
        {
            StreamReader sr = new StreamReader(stream);
            int line = 0;                       // line in file
            int record = 0;                     // records processed
            string[] headers = new string[0];    // headers
            int fieldCount = 0;

            try
            {
                var tokeniser = new Tokeniser(this.Configuration);
                string[] tokens;

                for (tokens = tokeniser.Tokenise(sr); !tokeniser.IsEOF; tokens = tokeniser.Tokenise(sr))
                {
                    line += tokeniser.LinesLastProcessed;
                    record++;

                    if (Configuration.HasHeader && record == 1)
                    {
                        // first record, headers
                        headers = tokens;
                        fieldCount = headers.Length;
                    }
                    else if (record == 1)
                    {
                        // first record, no headers
                        fieldCount = tokens.Length;
                        List<string> tempHeaders = new List<string>();
                        for (int h = 1; h <= fieldCount; h++)
                        {
                            tempHeaders.Add($"Column{h}");
                        }
                        headers = tempHeaders.ToArray();
                    }
                    else
                    {
                        // For data rows, check field count matches header count
                        if (tokens.Length != headers.Length)
                        {
                            throw new CsvException($"Column mismatch at line {line}.");
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
            finally
            {
                sr.Close();
            }
        }
    }
}