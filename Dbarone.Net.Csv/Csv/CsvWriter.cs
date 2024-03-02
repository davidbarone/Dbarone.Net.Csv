using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
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
    /// Writes a csv file per https://datatracker.ietf.org/doc/html/rfc4180.
    /// </summary>
    public class CsvWriter
    {
        CsvConfiguration Configuration { get; set; }
        Stream Stream { get; set; }

        /// <summary>
        /// Creates a new configured CsvWriter instance.
        /// </summary>
        /// <param name="configuration">The csv configuration.</param>
        public CsvWriter(Stream stream, CsvConfiguration configuration)
        {
            this.Stream = stream;
            this.Configuration = configuration;
        }

        /// <summary>
        /// Creates a new CsvWriter instance with default configuration.
        /// </summary>
        public CsvWriter(Stream stream)
        {
            this.Stream = stream;
            this.Configuration = new CsvConfiguration();
        }

        /// <summary>
        /// Writes a csv file.
        /// </summary>
        /// <param name="data">The data to write.</param>
        /// <exception cref="CsvException">Throws an exception under various error conditions.</exception>
        public void Write(IEnumerable<StringDictionary> data)
        {
            using (StreamWriter sw = new StreamWriter(this.Stream))
            {
                int record = 0;                     // records processed
                var headers = this.Configuration.Headers;

                try
                {
                    foreach (var row in data)
                    {
                        record++;
                        if (record == 1)
                        {
                            if (this.Configuration.HasHeader)
                            {
                                if (headers == null)
                                {
                                    headers = row.Keys.Cast<string>().ToArray();
                                }
                                sw.Write(string.Join(this.Configuration.FieldSeparator, headers));
                            }
                            sw.Write(this.Configuration.LineSeparator);
                        }

                        if (headers == null)
                        {
                            throw new CsvException("Headers not set!");
                        }

                        // Check row keys are the same
                        var rowKeys = row.Keys.Cast<string>().ToArray();
                        if (rowKeys.Count() != headers.Count() || rowKeys.Except(headers).Any())
                        {
                            throw new CsvException($"Row [$record] contains invalid key.");
                        }

                        char? delimiter = null;
                        // Write data
                        foreach (var key in headers)
                        {
                            if (delimiter.HasValue)
                            {
                                sw.Write(delimiter);
                            }
                            delimiter = this.Configuration.FieldSeparator;
                            string value = row[key] ?? "";
                            if (
                                value.Contains(this.Configuration.LineSeparator) ||
                                (value.Contains(this.Configuration.FieldEscape)) ||
                                value.Contains(this.Configuration.FieldSeparator))
                            {
                                value = $"{this.Configuration.FieldEscape}{value.Replace($"{this.Configuration.FieldEscape}", $"{this.Configuration.FieldEscape}{this.Configuration.FieldEscape}")}{this.Configuration.FieldEscape}";
                            }
                            sw.Write(value);
                        }
                        sw.Write(this.Configuration.LineSeparator);
                    }
                }
                finally
                {
                    sw.Flush();
                    sw.Close();
                }
            }
        }
    }
}