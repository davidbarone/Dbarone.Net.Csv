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

        /// <summary>
        /// Gets the tokens on the next record. Note that this may read more than 1 line of the file.
        /// </summary>
        /// <returns>Returns a string array of tokens</returns>
        public string[] Tokenise(StreamReader sr)
        {
            this.LinesLastProcessed = 0;
            bool IsEscapedFieldStarted = false;
            string field = "";  // the current field value
            List<string> tokens = new List<string>();

            do
            {
                // Read next line from stream
                var str = sr.ReadLine();
                this.LinesLastProcessed++;

                if (IsEscapedFieldStarted && Match(sr, configuration.FieldEscapeCharacter))
                {
                    // match another text delimiter immediately after previous text delimiter, i.e. "" - treat as escaped text delimiter.
                    if (Match(sr, configuration.FieldEscapeCharacter))
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
                else if (!IsEscapedFieldStarted && Match(sr, configuration.FieldEscapeCharacter))
                {
                    // text delimiter denoting start of value
                    IsEscapedFieldStarted = true;
                }
                else if (!IsEscapedFieldStarted && Match(sr, configuration.FieldSeparator))
                {
                    // Add value to array
                    tokens.Add(field);
                    field = string.Empty;
                }
                else if (!IsEscapedFieldStarted && !string.IsNullOrEmpty(field) && Match(sr, configuration.FieldEscapeCharacter))
                    // Cannot have FieldEscapeCharacter in middle of record
                    throw new Exception("Unexpected field escape character found!");
                else
                {
                    field += this.Read(sr);
                }
            } while (!IsEscapedFieldStarted);

            // add the final cell
            tokens.Add(field);
            this.IsEOF = sr.EndOfStream;
            return tokens.ToArray();
        }

        #region Private Methods

        /// <summary>
        /// Matches a character.
        /// </summary>
        /// <param name="token">The input token character.</param>
        /// <returns>Returns true and advances the cursor if match.</returns>
        private bool Match(StreamReader sr, char? token)
        {
            if (!token.HasValue)
                return false;

            if (Peek(sr) == token)
            {
                Read(sr);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Reads a character from the string, and advances the position.
        /// </summary>
        /// <returns></returns>
        private char? Read(StreamReader sr)
        {
            if (sr.EndOfStream)
            {
                return null;
            }
            else
            {
                return (char)sr.Read();
            }
        }

        /// <summary>
        /// Peeks the next character without advancing the position.
        /// </summary>
        /// <returns>Returns the next character.</returns>
        private char? Peek(StreamReader sr)
        {
            if (sr.EndOfStream)
            {
                return null;
            }
            else
            {
                return (char)sr.Peek();
            }
        }

        #endregion
    }
}