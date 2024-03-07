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

        /// <summary>
        /// Is set to true if the underlying stream is at end of stream.
        /// </summary>
        public bool IsEOF { get; set; }

        /// <summary>
        /// Returns the number of lines processed by the immediately previous Tokenise() method call.
        /// </summary>
        public int LinesLastProcessed { get; set; }

        /// <summary>
        /// Initialises a new Tokeniser instance from a configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Tokeniser(CsvConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Initialises a new Tokeniser instance.
        /// </summary>
        public Tokeniser()
        {
            this.configuration = new CsvConfiguration();
        }

        /// <summary>
        /// Returns the next line, or final line if end of stream reached. Allow for non-standard line terminators to be specified.
        /// The Read() method of StringReader only recognises standard end of line markers line CRLF and LF.
        /// </summary>
        /// <param name="sr">The StreamReader instance.</param>
        /// <returns>Returns 1 line of characters up to the end of line marker (or end of stream).</returns>
        public string? GetLine(StreamReader sr)
        {
            List<char> arr = new List<char>();
            char[] EOLChar = this.configuration.LineSeparator.ToCharArray();

            if (sr.EndOfStream)
            {
                return null;
            }

            while (true)
            {
                bool matchEOL = false;
                var position = sr.BaseStream.Position;

                // Check if next chars match EOL
                for (int j = 0; j < EOLChar.Length; j++)
                {
                    int i = sr.Read();

                    if (i == -1)
                    {
                        // End of stream
                        return new string(arr.ToArray());
                    }
                    if (j == 0 && EOLChar[j] == i)
                    {
                        matchEOL = true;
                    }
                    else if (matchEOL == true && EOLChar[j] == i)
                    {
                        // continue reading end of line characters
                    }
                    else if (matchEOL == true && EOLChar[j] != i)
                    {
                        // started to match, but found diff - roll back pointer and continue reading characters
                        sr.BaseStream.Position = position;
                        matchEOL = false;
                        break;
                    }
                    else
                    {
                        // just read normal character - add to array
                        arr.Add((char)i);
                        break;
                    }
                }
                if (matchEOL)
                {
                    // found end of line
                    return new string(arr.ToArray());
                }
            }
        }


        /// <summary>
        /// Gets the tokens on the next record. Note that this may read more than 1 line of the file.
        /// </summary>
        /// <param name="sr">The input stream reader.</param>
        /// <returns>Returns a string array of tokens</returns>
        /// <exception cref="CsvException">Throws an exception in certain error conditions.</exception>
        public string[]? Tokenise(StreamReader sr)
        {
            if (sr == null)
            {
                throw new CsvException("StreamReader is null!");
            }

            this.LinesLastProcessed = 0;
            bool IsEscapedFieldStarted = false;
            string field = "";  // the current field value
            List<string> tokens = new List<string>();
            bool hasReadData = false;       // to check for completely blank lines

            do
            {
                // If we're processing multiple lines, need to add line delimiter to the current field value.
                if (this.LinesLastProcessed > 0)
                {
                    field += this.configuration.LineSeparator;
                }

                // Read next line from stream
                var str = GetLine(sr);
                this.LinesLastProcessed++;

                if (str == null)
                {
                    throw new CsvException("Unexpected EOF!");
                }
                else
                {
                    var sp = new StringParser(str);

                    do
                    {
                        if (IsEscapedFieldStarted && sp.Match(configuration.FieldEscape))
                        {
                            hasReadData = true;

                            // match another text delimiter immediately after previous text delimiter, i.e. "" - treat as escaped text delimiter.
                            if (sp.Match(configuration.FieldEscape))
                            {
                                // another text delimiter means treat as text in value
                                field += configuration.FieldEscape;
                            }
                            else
                            {
                                // closing text delimiter
                                IsEscapedFieldStarted = false;
                            }
                        }
                        else if (!IsEscapedFieldStarted && sp.Match(configuration.FieldEscape))
                        {
                            hasReadData = true;

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
                            hasReadData = true;

                            // Add value to array
                            tokens.Add(field);
                            field = string.Empty;
                        }
                        else
                        {
                            hasReadData = true;

                            field += sp.Read();
                        }
                    } while (!sp.Eof);
                }
            } while (IsEscapedFieldStarted);

            // add the final cell
            tokens.Add(field);
            this.IsEOF = sr.EndOfStream;

            if (hasReadData)
            {
                return tokens.ToArray();
            }
            else
            {
                return null;
            }
        }
    }
}