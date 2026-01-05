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
using DotSerial.Common;

namespace DotSerial.Utilities
{
    internal static class Extensions
    {
        /// <summary>
        /// Trim end whitespace from StringBuilder
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <returns>Stringbuilder</returns>
        public static StringBuilder TrimEnd(this StringBuilder sb)
        {            
            ArgumentNullException.ThrowIfNull(sb);

            if (sb.Length == 0) return sb;

            int i = sb.Length - 1;

            for (; i >= 0; i--)
                if (!char.IsWhiteSpace(sb[i]))
                    break;

            if (i < sb.Length - 1)
                sb.Length = i + 1;

            return sb;
        }

        /// <summary>
        /// Builds a Stringbuilder from another strting builder
        /// </summary>
        /// <param name="input">Stringbuilder</param>
        /// <param name="index">Index</param>
        /// <param name="length">Length</param>
        /// <returns>Sub stringbuilder</returns>
        public static StringBuilder SubString(this StringBuilder input, int index, int length)
        {
            StringBuilder subString = new();
            if (index + length - 1 >= input.Length || index < 0) 
            {
                throw new ArgumentOutOfRangeException(nameof(input)); 
            }
            int endIndex = index + length;
            for (int i = index; i < endIndex; i++)
            {
                subString.Append(input[i]);
            }

            return subString;
        }

        /// <summary>
        /// Indicates whether a specified string of an stringbuilder is null, empty,
        /// or consists only of white-space characters.
        /// </summary>
        /// <param name="input">Stringbuilder</param>
        /// <returns>True, if null or whitespace.</returns>
        public static bool IsNullOrWhiteSpace(this StringBuilder input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length == 0)
            {
                return true;
            }

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (!char.IsWhiteSpace(c))
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if Stringbuilder content equals "null".
        /// </summary>
        /// <param name="input">Stringbuilder</param>
        /// <returns>True, if content of Stringbuilder only contains "null".</returns>
        public static bool EqualsNullString(this StringBuilder input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if (input.Length != 4)
            {
                return false;
            }

            if (Char.ToLower(input[0]) != CommonConstants.N)
                return false;
            if (Char.ToLower(input[1]) != CommonConstants.U)
                return false;
            if (Char.ToLower(input[2]) != CommonConstants.L)
                return false;
            if (Char.ToLower(input[3]) != CommonConstants.L)
                return false;
            
            return true;
        }

    }
}