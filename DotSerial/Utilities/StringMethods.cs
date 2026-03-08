using System.Runtime.InteropServices;
using DotSerial.Common;

namespace DotSerial.Utilities
{
    internal static class StringMethods
    {
        /// <summary>
        /// Add quotes at the start end at the end of a string.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>"str"</returns>
        internal static string AddStartAndEndQuotes(string str)
        {
            ArgumentNullException.ThrowIfNull(str);

            string result = CommonConstants.Quote + str + CommonConstants.Quote;
            return result;
        }

        /// <summary>
        /// Removes start end end quotes from a string.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>String without start and end quotes</returns>
        internal static string RemoveStartAndEndQuotes(string str)
        {
            ArgumentNullException.ThrowIfNull(str);

            if (false == str.HasStartAndEndQuotes())
            {
                throw new NotSupportedException($"String don't have start and end quotes");
            }

            return str[1..^1]; 
        }

#region  Extensions

        /// <summary>
        /// Check if a string contains a char from a char array
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="specialChars">Special chars</param>
        /// <returns>True, if a special char appeares in the string</returns>
        internal static bool ContainsChars(this string str, char[] specialChars)
        {
            ArgumentNullException.ThrowIfNull(str);
            ArgumentNullException.ThrowIfNull(specialChars);

            if (specialChars.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (specialChars.Contains(c))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if string equals "null"
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>True, if string equals "null"</returns>
        internal static bool EqualsNullString(this string str)
        {
            ArgumentNullException.ThrowIfNull(str);

            if (str.Length != 4)
            {
                return false;
            }

            if (char.ToLower(str[0]) != CommonConstants.N)
                return false;
            if (char.ToLower(str[1]) != CommonConstants.U)
                return false;
            if (char.ToLower(str[2]) != CommonConstants.L)
                return false;
            if (char.ToLower(str[3]) != CommonConstants.L)
                return false;
            
            return true;
        }

        /// <summary>
        /// Check if string equals "true" or "false"
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>True, if string equals "true" or "false"</returns>
        internal static bool EqualsBooleanString(this string str)
        {
            ArgumentNullException.ThrowIfNull(str);

            if (str.EqualsFalseString() || str.EqualsTrueString())
            {
                return true;
            }

            return false;
        }          

        /// <summary>
        /// Check if string equals "true"
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>True, if string equals "true"</returns>
        internal static bool EqualsTrueString(this string str)
        {
            ArgumentNullException.ThrowIfNull(str);

            if (str.Length != 4)
            {
                return false;
            }

            if (char.ToLower(str[0]) != CommonConstants.T)
                return false;
            if (char.ToLower(str[1]) != CommonConstants.R)
                return false;
            if (char.ToLower(str[2]) != CommonConstants.U)
                return false;
            if (char.ToLower(str[3]) != CommonConstants.E)
                return false;
            
            return true;
        }        


        /// <summary>
        /// Check if string equals "false"
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>True, if string equals "false"</returns>
        internal static bool EqualsFalseString(this string str)
        {
            ArgumentNullException.ThrowIfNull(str);

            if (str.Length != 5)
            {
                return false;
            }

            if (char.ToLower(str[0]) != CommonConstants.F)
                return false;
            if (char.ToLower(str[1]) != CommonConstants.A)
                return false;
            if (char.ToLower(str[2]) != CommonConstants.L)
                return false;
            if (char.ToLower(str[3]) != CommonConstants.S)
                return false;
            if (char.ToLower(str[4]) != CommonConstants.E)
                return false;
            
            return true;
        }          

        /// <summary>
        /// Check if a string has start and end quotes
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>True, if the first and last char in a string is a quote.</returns>
        internal static bool HasStartAndEndQuotes(this string str)
        {
            ArgumentNullException.ThrowIfNull(str);

            if (str.Length < 2)
            {
                return false;
            }

            if (str[0] == CommonConstants.Quote && str[^1] == CommonConstants.Quote)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if a string has leading or trailing whitespaces.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>True, if string has leading or/and trailing whitespaces.</returns>
        internal static bool HasLeadingOrTrailingWhitespaces(this string str)
        {
            ArgumentNullException.ThrowIfNull(str);

            if (str.Length == 0)
            {
                return false;
            }

            if (char.IsWhiteSpace(str[0]) || char.IsWhiteSpace(str[^1]))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check is a string represents a numeric value
        /// </summary>
        /// <param name="str">String</param>
        /// <returns></returns>
        internal static bool IsNumericValue(this string str)
        {
            ArgumentNullException.ThrowIfNull(str);

            if (str.Length == 0)
            {
                return false;
            }

            if (double.TryParse(str, out _))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static readonly Dictionary<char, string> _xmlEscapeCharDic = new ()
        {
            {'<', "&lt;"},
            {'&', "&amp;"},
            {'"', "&quot;"},
            {'\'', "&apos;"},            
            {'>', "&gt;"},            
        };

        internal static string XmlEscape(this string str)
        {
            ArgumentNullException.ThrowIfNull(str);

            if (str.Length == 0)
            {
                return string.Empty;
            }

            // Create array with max possible number of chars (&apos; => 6 chars)
            char[] tmp = new char[str.Length * 6];
            int index = 0;

            for(int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                if (_xmlEscapeCharDic.TryGetValue(c, out string? replaceString))
                {
                    for (int j = 0; j < replaceString.Length; j++)
                    {
                        tmp[index++] = replaceString[j];
                    }
                }
                else
                {
                    tmp[index++] = c;
                }
            }

            return new string(tmp, 0, index);
        }

        internal static string XmlUnEscape(this string str)
        {
            ArgumentNullException.ThrowIfNull(str);

            if (str.Length == 0)
            {
                return string.Empty;
            }

            char[] tmp = new char[str.Length];
            int index = 0;

            for(int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                if (c == CommonConstants.AND && i + 3 < str.Length)
                {
                    if (str[i + 1] == 'l' && str[i + 2] == 't' && str[i + 3] == ';')
                    {
                        tmp[index++] = '<';   
                        i+=3;
                    }
                    else if (str[i + 1] == 'g' && str[i + 2] == 't' && str[i + 3] == ';')
                    {
                        tmp[index++] = '>'; 
                        i+=3;  
                    }
                    else if (i + 4 < str.Length)
                    {
                        if (str[i + 1] == 'a' && str[i + 2] == 'm' && str[i + 3] == 'p' && str[i + 4] == ';')
                        {
                            tmp[index++] = CommonConstants.AND;   
                            i+=4;
                        }
                        else if (i + 5 < str.Length)
                        {
                            if (str[i + 1] == 'q' && str[i + 2] == 'u' && str[i + 3] == 'o' && str[i + 4] == 't' &&
                                str[i + 5] == ';')
                            {
                                tmp[index++] = CommonConstants.Quote;  
                                i+=5; 
                            }
                            else if (str[i + 1] == 'a' && str[i + 2] == 'p' && str[i + 3] == 'o' && str[i + 4] == 's' &&
                                    str[i + 5] == ';')
                            {
                                tmp[index++] = '\'';   
                                i+=5;
                            }
                            else
                            {
                                tmp[index++] = c;    
                            }
                        }
                        else
                        {
                            tmp[index++] = c;    
                        }
                    }
                    else
                    {
                        tmp[index++] = c;
                    }
                }
                else
                {
                    tmp[index++] = c;
                }
            }

            return new string(tmp, 0, index);             
        }

        internal static string EscapeChars(this string str, char[] charsToEscape)
        {
            ArgumentNullException.ThrowIfNull(str);

            if (null == charsToEscape || charsToEscape.Length == 0)
            {
                return str;
            }

            if (str.Length == 0)
            {
                return string.Empty;
            }

            // Create array with max possible number of chars
            char[] tmp = new char[str.Length * 2];
            int index = 0;

            for(int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                if (charsToEscape.Contains(c))
                {
                    tmp[index++] = CommonConstants.Backslash;
                    tmp[index++] = c;
                }
                else
                {
                    tmp[index++] = c;
                }
            }

            return new string(tmp, 0, index);
        }

        internal static string UnescapeString(this string str, char[] charsToEscape)
        {
            ArgumentNullException.ThrowIfNull(str);

            if (null == charsToEscape || charsToEscape.Length == 0)
            {
                return str;
            }

            if (str.Length == 0)
            {
                return string.Empty;
            }

            char[] tmp = new char[str.Length];
            int index = 0;

            for(int i = 0; i < str.Length; i++)
            {
                char c = str[i];

                if (c == CommonConstants.Backslash && i + 1 < str.Length)
                {                    
                    char escapedChar = str[i + 1];
                    if (charsToEscape.Contains(escapedChar))
                    {
                        tmp[index++] = escapedChar;

                        // Skip Escaped char in next iteration
                        i++;
                    }
                    else
                    {
                        tmp[index++] = c;
                    }
                }
                else
                {
                    tmp[index++] = c;
                }
            }

            return new string(tmp, 0, index);            
        }

#endregion

    }
}