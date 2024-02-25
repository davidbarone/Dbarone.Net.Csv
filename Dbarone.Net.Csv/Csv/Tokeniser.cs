using System;
using System.Collections.Generic;
using System.Text;

namespace Dbarone.Net.Csv
{
    /// <summary>
    /// Gets the csv tokens in a string.
    /// </summary>
    public class Tokeniser
    {
        CsvConfiguration configuration;

        public bool IsEOF { get; set; }
        public int LinesLastProcessed { get; set; }

        /// <summary>
        /// Initialises a new Tokeniser instance from a string input and configuration.
        /// </summary>
        /// <param name="str">The input string.</param>
        public Tokeniser(CsvConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Tokeniser()
        {
            this.configuration = new CsvConfiguration();
        }

        /// <summary>
        /// Gets the tokens on the next record. Note that this may read more than 1 line of the file.
        /// </summary>
        /// <returns>Returns a string array of tokens</returns>
        public string[] Tokenise(StreamReader sr)
        {
            if (sr == null)
            {
                throw new CsvException("StreamReader is null!");
            }

            this.LinesLastProcessed = 0;
            bool IsEscapedFieldStarted = false;
            string field = "";  // the current field value
            List<string> tokens = new List<string>();

            do
            {
                // If we're processing multiple lines, need to add CRLF to the current field value.
                if (this.LinesLastProcessed > 0)
                {
                    field += Environment.NewLine;
                }

                // Read next line from stream
                var str = sr.ReadLine();

                if (str == null)
                {
                    throw new CsvException("Unexpected EOF!");
                }

                this.LinesLastProcessed++;
                var sp = new StringParser(str);

                do
                {
                    if (IsEscapedFieldStarted && sp.Match(configuration.FieldEscapeCharacter))
                    {
                        // match another text delimiter immediately after previous text delimiter, i.e. "" - treat as escaped text delimiter.
                        if (sp.Match(configuration.FieldEscapeCharacter))
                        {
                            // another text delimiter means treat as text in value
                            field += configuration.FieldEscapeCharacter;
                        }
                        else
                        {
                            // closing text delimiter
                            IsEscapedFieldStarted = false;
                        }
                    }
                    else if (!IsEscapedFieldStarted && sp.Match(configuration.FieldEscapeCharacter))
                    {
                        if (string.IsNullOrEmpty(field))
                        {
                            // text delimiter denoting start of value
                            IsEscapedFieldStarted = true;
                        }
                        else
                        {
                            // Cannot have FieldEscapeCharacter in middle of record
                            throw new CsvException("Unexpected field escape character found!");
                        }
                    }
                    else if (!IsEscapedFieldStarted && sp.Match(configuration.FieldSeparator))
                    {
                        // Add value to array
                        tokens.Add(field);
                        field = string.Empty;
                    }
                    else
                    {
                        field += sp.Read();
                    }
                } while (!sp.EndOfString);
            } while (IsEscapedFieldStarted);

            // add the final cell
            tokens.Add(field);
            this.IsEOF = sr.EndOfStream;
            return tokens.ToArray();
        }
    }
}