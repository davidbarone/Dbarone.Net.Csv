using System;
using System.Collections.Generic;
using System.Text;

namespace Dbarone.Net.Csv
{
    /// <summary>
    /// Provides parsing functions for a string similar to StreamReader.
    /// </summary>
    public class StringParser
    {
        char[] innerString;
        int position = -1;

        /// <summary>
        /// Creates a new StringParser instance.
        /// </summary>
        /// <param name="str">The input string.</param>
        public StringParser(string str)
        {
            innerString = str.ToCharArray();
            position = -1;
        }

        /// <summary>
        /// Matches a character.
        /// </summary>
        /// <param name="token">The token char to match.</param>
        /// <returns>Returns true and advances the position if a match. If no match, the position is not modified.</returns>
        public bool Match(char? token)
        {
            if (!token.HasValue)
                return false;

            if (Peek() == token)
            {
                var c = Read();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Reads a character from the string, and advances the position.
        /// </summary>
        /// <returns>Returns the read character.</returns>
        public char? Read()
        {
            if (Eof)
                return null;
            else
            {
                position++;
                return innerString[position];
            }
        }

        /// <summary>
        /// Peeks the next character without advancing the position.
        /// </summary>
        /// <returns>Returns the peeked character.</returns>
        public char? Peek()
        {
            if (Eof)
                return null;
            else
                return innerString[position + 1];
        }

        /// <summary>
        /// Returns true if EOF
        /// </summary>
        public bool Eof
        {
            get
            {
                return !(position < innerString.Length - 1 && innerString.Length > 0);
            }
        }
    }
}