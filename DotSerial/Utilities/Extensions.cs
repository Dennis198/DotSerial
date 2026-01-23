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
        public static bool EqualsContent(this StringBuilder sb, StringBuilder sbToCheck, int startIndex = 0)
        {
            // TODO Null Check damit machen????
            
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(sbToCheck);

            if (sb.Length - startIndex < sbToCheck.Length)
            {
                return false;
            }

            for (int i = 0; i < sbToCheck.Length; i++)
            {
                if (sb[i + startIndex] != sbToCheck[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Trim starting whitespace from StringBuilder
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <returns>Stringbuilder</returns>
        public static StringBuilder Trim(this StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (sb.Length == 0)
            {
                return new StringBuilder();
            }

            int i = 0;
            for (; i < sb.Length; i++)
                if (!char.IsWhiteSpace(sb[i]))
                    break;
            int len = sb.Length - i;
            var result = sb.SubString(i, len);

            return result;
        }

        /// <summary>
        /// Trim end whitespace from StringBuilder
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <returns>Stringbuilder</returns>
        public static StringBuilder TrimEnd(this StringBuilder sb)
        {            
            ArgumentNullException.ThrowIfNull(sb);

            if (sb.Length == 0)
            {
                return new StringBuilder();
            }

            int i = sb.Length - 1;

            for (; i >= 0; i--)
                if (!char.IsWhiteSpace(sb[i]))
                    break;

            if (i < sb.Length - 1)
            {
                StringBuilder result = sb.SubString(0, i + 1);
                return result;
            }

            return new StringBuilder(sb.ToString());
        }            

        /// <summary>
        /// asdasd
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static int IndexOf(this StringBuilder sb, string str, int startIndex = 0)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(str);

            if (str.Length == 0)
            {
                throw new ArgumentException($"{nameof(str)} is empty.");
            }

            if (startIndex < 0 || startIndex >= sb.Length)
            {
                return -1;
            }

            int strLength = str.Length;

            if (strLength > sb.Length)
            {
                return -1;
            }

            // Get first char to match
            char firstChar = str[0];

            for (int i = startIndex; i <= sb.Length - strLength; i++)
            {
                char c = sb[i];

                // Check if first character matches
                if (c == firstChar)
                {   
                    // If only one char to match, return index
                    if (strLength == 1)
                    {
                        return i;
                    }

                    // Check if string fits in remaining length
                    if (strLength - 1 >= sb.Length - i)
                    {
                        return -1;
                    }

                    for (int j = 1; j < strLength; j++)
                    {
                        if (sb[i + j] != str[j])
                        {
                            break;
                        }

                        if (j == strLength - 1)
                        {
                            return i;
                        }
                    }
                }
            }

            return -1;
        }

        public static int LastIndexOf(this StringBuilder sb, string str, int startIndex = 0)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(str);

            if (str.Length == 0)
            {
                throw new ArgumentException($"{nameof(str)} is empty.");
            }

            if (startIndex < 0 || startIndex >= sb.Length)
            {
                return -1;
            }

            int strLength = str.Length;

            if (strLength > sb.Length)
            {
                return -1;
            }

            // Reverse string to match
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);

            // Get first char to match
            char firstChar = charArray[0];
            int effectiveStartIndex = startIndex == 0 ? sb.Length - 1 : startIndex;

            for (int i = effectiveStartIndex; i >= 0; i--)
            {
                char c = sb[i];

                // Check if first character matches
                if (c == firstChar)
                {
                    // If only one char to match, return index
                    if (strLength == 1)
                    {
                        return i;
                    }

                    // Check if string fits in remaining length
                    if (i - strLength + 1 < 0)
                    {
                        return -1;
                    }

                    for (int j = 1; j < strLength; j++)
                    {
                        if (sb[i - j] != charArray[j])
                        {
                            break;
                        }

                        if (j == strLength - 1)
                        {
                            return i;
                        }
                    }
                }
            }

            return -1;
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

            if (char.ToLower(input[0]) != CommonConstants.N)
                return false;
            if (char.ToLower(input[1]) != CommonConstants.U)
                return false;
            if (char.ToLower(input[2]) != CommonConstants.L)
                return false;
            if (char.ToLower(input[3]) != CommonConstants.L)
                return false;
            
            return true;
        }

        /// <summary>
        /// Check if the first non white space char equals the given char.
        /// </summary>
        /// <param name="input">StringBuilder</param>
        /// <param name="c">Char to check</param>
        /// <returns>True, if char is equal.</returns>
        public static bool EqualFirstNoWhiteSpaceChar(this StringBuilder input, char c)
        {
            ArgumentNullException.ThrowIfNull(input);

            for (int i = 0; i < input.Length; i++)
            {
                var currChar = input[i];
                if (char.IsWhiteSpace(currChar))
                {
                    continue;
                }
                else if (currChar == c)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if the last non white space char equals the given char.
        /// </summary>
        /// <param name="input">StringBuilder</param>
        /// <param name="c">Char to check</param>
        /// <returns>True, if char is equal.</returns>
        public static bool EqualLastNoWhiteSpaceChar(this StringBuilder input, char c)
        {
            ArgumentNullException.ThrowIfNull(input);

            for (int i = input.Length - 1; i >= 0; i--)
            {
                var currChar = input[i];
                if (char.IsWhiteSpace(currChar))
                {
                    continue;
                }
                else if (currChar == c)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Counts the values between " in a string.
        /// </summary>
        /// <param name="input">StringBuilder</param>
        /// <param name="considerNull">True, if null should also be counted as a quoted value.</param>
        /// <returns></returns>
        public static int CountQuotedValues(this StringBuilder input, bool considerNull = true)
        {
            ArgumentNullException.ThrowIfNull(input);

            int count = 0;

            for (int i = 0; i < input.Length; i++)
            {
                var currChar = input[i];
                if (CommonConstants.Quote == currChar)
                {
                    i = input.SkipStringValue(i);

                    count++;
                }
                else if (CommonConstants.N == currChar)
                {
                    if (i + 3 > input.Length - 1) throw new NotImplementedException();

                    i++;
                    if (input[i] != CommonConstants.U) throw new NotImplementedException();
                    i++;
                    if (input[i] != CommonConstants.L) throw new NotImplementedException();
                    i++;
                    if (input[i] != CommonConstants.L) throw new NotImplementedException();
                    
                    if (considerNull)
                    {
                        count++;
                    }
                    
                }
            }

            return count;
        }

        internal static int SkipStringValue(this StringBuilder sb, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (sb.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(sb.ToString());
            }

            if (sb.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (sb[startIndex] != CommonConstants.Quote)
            {
                throw new ArgumentException(sb.ToString());
            }        

            for (int j = startIndex + 1; j < sb.Length; j++)
            {
                var c2 = sb[j];
                if (c2 == '\\')
                {
                    j++;
                }
                else if (c2 == CommonConstants.Quote)
                {
                    return j;
                }
            }

            return sb.Length - 1;
        }         

    }
}