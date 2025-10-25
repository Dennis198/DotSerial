#region License
//Copyright (c) 2025 Dennis Sölch

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
#endregion

using System.Text;
using DotSerial.Core.General;
using DotSerial.Core.Tree;

namespace DotSerial.Core.Misc
{
    internal static class ParseMethods
    {
        /// <summary>
        /// Parse the node property type info
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>DSNodePropertyType</returns>
        internal static DSNodePropertyType ParsePropertyTypeInfo(string? value)
        {
            // Check if value has value
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(value);
            }

            return value.ConvertToDSNodePropertyType();
        }

        /// <summary>
        /// Apends the whole string from starting quote to end quote to
        /// the stringbuilder.
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="startIndex">Index of the opeing quote</param>
        /// <param name="str">string</param>
        /// <returns>Index of the closing quote</returns>
        internal static int AppendStringValue(StringBuilder sb, int startIndex, string str)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException(str);
            }

            if (str.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (str[startIndex] != GeneralConstants.Quote)
            {
                throw new ArgumentException(str);
            }

            sb.Append(GeneralConstants.Quote);

            for (int j = startIndex + 1; j < str.Length; j++)
            {
                var c2 = str[j];
                if (c2 == '\\')
                {
                    sb.Append(c2);
                    sb.Append(str[j + 1]);
                    j++;
                }
                if (c2 == GeneralConstants.Quote)
                {
                    sb.Append(c2);
                    return j;
                }
                else
                {
                    sb.Append(c2);
                }
            }

            return str.Length - 1;
        }     

        /// <summary>
        /// Removes all whitespaces inside a string
        /// except whitespace is between quotes.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>String without whitespaces.</returns>
        internal static string RemoveWhiteSpace(string str)
        {
            // Check if value has value
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            StringBuilder sb = new();
            int stringLength = str.Length;

            for (int i = 0; i < stringLength; i++)
            {
                var c = str[i];

                // If char is a quoto extract everything
                // till the closing quote is reached
                if (c == GeneralConstants.Quote)
                {
                    i = AppendStringValue(sb, i, str);
                    continue;
                }
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                sb.Append(c);
            }

            return sb.ToString();
        }              
    }
}