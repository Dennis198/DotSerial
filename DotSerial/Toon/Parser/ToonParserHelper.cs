using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;
using DotSerial.Common;
using DotSerial.Utilities;

namespace DotSerial.Toon.Parser
{
    internal static  class ToonParserHelper
    {

        internal static Dictionary<string, MultiLineStringBuilder> ExtractKeyValuePairsFromToonObject(MultiLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var result = new Dictionary<string, MultiLineStringBuilder>();
            int objLevel = LineLevel(lines.GetLine(0));

            for (int i = 0; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                var line = lines.GetLine(i);
                int currLevel = LineLevel(line);
                if (currLevel < objLevel)
                {
                    break;
                }

                // "Key":
                if (IsKeyLine(line))
                {
                    string key = ExtractKeyFromLine(line);

                    int sIndex = i + 1;
                    int eIndex = GetEndIndexOfToonObject(lines, i);

                    var helpObj = lines.Slice(sIndex, eIndex);
                    
                    if (sIndex == eIndex && !IsToonList(helpObj))
                    {
                        // Special case an object with exaclty one item.
                        // Must be marked, otherwise there is no way to
                        // differentiated between an object or a simple
                        // Key, Value pair for an primitive.
                        helpObj.IsOneLineObject = true;
                    }

                    result.Add(key, helpObj);
                    i = eIndex;                    

                }
                // "Key": "Value"
                else
                {
                    string key = ExtractKeyFromLine(line);
                    var helpObj = lines.Slice(i, i);
                   
                    result.Add(key, helpObj);
                }
            }

            return result;            
        }

        internal static bool IsToonObject(MultiLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var firstLine = lines.GetLine(0);

            if (null == firstLine)
            {
                throw new NotImplementedException();
            }

            // "'key':"
            if (IsEmptyObject(firstLine))
            {
                return true;
            }

            if (IsToonList(lines))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Extracts the value from a line
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <returns>Value of the line</returns>
        internal static string? ExtractValueFromLine(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            StringBuilder keyBuilder = new();
            bool keyWasFound = false;

            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == CommonConstants.Quote)
                {
                    if (keyWasFound)
                    {
                        _ = ParseMethods.AppendStringValue(keyBuilder, i, line);
                        // Remove ending quote
                        keyBuilder.Remove(keyBuilder.Length - 1, 1);
                        // Remove starting quote
                        keyBuilder.Remove(0, 1);
                        
                        return keyBuilder.ToString();
                    }
                    else
                    {
                        i = line.SkipStringValue(i);
                        keyWasFound = true;
                    }
                }
                else if (keyWasFound && c == CommonConstants.N)
                {
                    if (false == line.EqualsNullString(i))
                    {
                        throw new DSToonException("Invalid toon");
                    }

                    return null;
                }
            }

            throw new NotImplementedException();
        }          

        /// <summary>
        /// Check if MultiLineStringBuilder is a key only a key value pair
        /// </summary>
        /// <param name="lines">MultiLineStringBuilder</param>
        /// <returns>True, if yaml key value pair</returns>
        internal static bool IsToonPrimitiveLine(MultiLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count != 1)
            {
                return false;
            }

            if (lines.IsOneLineObject)
            {
                return false;
            }

            if (IsToonList(lines))
            {
                return false;
            }

            if (IsToonSingleValue(lines))
            {
                return false;
            }

            var firstLine = lines.GetLine(0);
            bool keyWasFound = false;
            bool valueWasFound =  false;

            for (int i = 0; i < firstLine.Length; i++)
            {
                var c = firstLine[i];
                
                if (false == char.IsWhiteSpace(c))
                {
                    if (keyWasFound && valueWasFound)
                    {
                        throw new NotImplementedException();
                    }
                }

                if (c == CommonConstants.Quote)
                {
                    if (keyWasFound)
                    {
                        i = firstLine.SkipStringValue(i);                        
                        valueWasFound = true;
                    }
                    else
                    {
                        i = firstLine.SkipStringValue(i);   
                        keyWasFound = true;
                    }
                }
                else if (keyWasFound && c == CommonConstants.N)
                {
                    if (false == firstLine.EqualsNullString(i))
                    {
                        throw new DSToonException("Invalid toon");
                    }

                    i += 3;
                    valueWasFound = true;
                }
            }

            return keyWasFound && valueWasFound;
        }         

        internal static bool IsToonSingleValue(MultiLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count != 1)
            {
                return false;
            }

            var firstLine = lines.GetLine(0);

            // "null"
            if (firstLine.EqualsNullString())
            {
                return true;
            }

            // "'Key': "
            if (IsKeyLine(firstLine))
            {
                return false;
            }            

            if (1 == firstLine.CountQuotedValues())
            {
                return true;
            }

            return false;
        }

        internal static bool IsToonList(MultiLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var firstLine = lines.GetLine(0);

            if (null == firstLine)
            {
                throw new NotImplementedException();
            }

            // // 1. "'key'[0]:"
            // // 2. "[0]:"
            // if (IsEmptyList(firstLine))
            // {
            //     return true;
            // }

            if (-1 != ParseListCount(firstLine))
            {
                return true;
            }

            // 1. "- 'item'"
            // 2. "- 'key' : 'item'"
            if (firstLine.EqualFirstNoWhiteSpaceChar(ToonConstants.ListItemIndicator))
            {
                return true;
            }

            return false;
        }

        internal static bool IsEmptyList(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            int count = ParseListCount(line);

            return 0 == count;

            // bool countIndicatorFound =  false;
            // string countAsString = string.Empty;

            // for(int i = 0; i < line.Length - 1; i++)
            // {
            //     char c = line[i];

            //     if (char.IsWhiteSpace(c))
            //     {
            //         continue;
            //     }
            //     else if (c == CommonConstants.Quote)
            //     {
            //         if (true == countIndicatorFound)
            //         {
            //             throw new NotImplementedException();
            //         }
            //         i = line.SkipStringValue(i);
            //     }
            //     else if (c == CommonConstants.BracketsStart)
            //     {
            //         if (true == countIndicatorFound)
            //         {
            //             throw new NotImplementedException();
            //         }
            //         countIndicatorFound = true;
            //     }
            //     else if (c == CommonConstants.BracketsEnd)
            //     {
            //         if (false == countIndicatorFound)
            //         {
            //             throw new NotImplementedException();
            //         }
            //         break;
            //     }
            //     else if (char.IsDigit(c))
            //     {
            //         if (true == countIndicatorFound)
            //         {
            //             countAsString += c;
            //         }
            //     }
            //     else if (c == ToonConstants.KeyValueSeperator)
            //     {
            //         return false;
            //     }
            // }

            // if (false == int.TryParse(countAsString, out int listCount))
            // {
            //     return false;
                
            // }

        }

        internal static int ParseListCount(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            bool countIndicatorStartFound =  false;
            bool countIndicatorEndFound =  false;
            bool firstDigitFromCountFound = false;
            string countAsString = string.Empty;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == CommonConstants.Quote)
                {
                    if (true == countIndicatorStartFound)
                    {
                        throw new NotImplementedException();
                    }
                    i = line.SkipStringValue(i);
                }
                else if (c == CommonConstants.BracketsStart)
                {
                    if (true == countIndicatorStartFound)
                    {
                        throw new NotImplementedException();
                    }
                    countIndicatorStartFound = true;
                }
                else if (c == CommonConstants.BracketsEnd)
                {
                    if (false == countIndicatorStartFound)
                    {
                        throw new NotImplementedException();
                    }
                    countIndicatorEndFound = true;
                    break;
                }
                else if (char.IsDigit(c))
                {
                    if (true == countIndicatorStartFound)
                    {
                        countAsString += c;
                        firstDigitFromCountFound = true;
                    }
                }    
                else
                {
                    if (true == countIndicatorStartFound)
                    {
                        throw new NotImplementedException();
                    }
                }            
            }

            if (false == countIndicatorStartFound || false == countIndicatorEndFound)
            {
                return -1;
            }

            // TODO TESTEN OB "22 33" geht?
            if (false == int.TryParse(countAsString, out int listCount))
            {
                return -1;
            }

            if (listCount < 0)
            {
                return -1;
            }

            return listCount;
        }


        internal static bool IsEmptyObject(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            if (false == line.EqualLastNoWhiteSpaceChar(ToonConstants.KeyValueSeperator))
            {
                return false;
            }

            bool seperatorFound = false;

            for (int i = line.Length - 1; i >= 0; i--)
            {   
                char c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c ==  ToonConstants.KeyValueSeperator)
                {
                    if (true == seperatorFound)
                    {
                        throw new NotImplementedException();
                    }
                    seperatorFound = true;
                }
                else if (c == CommonConstants.BracketsEnd)
                {
                    return false;
                }
                else if (c == CommonConstants.BracesEnd)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if line is a key line
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <returns>True, if line is key line</returns>
        private static bool IsKeyLine(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            return line.EqualLastNoWhiteSpaceChar(ToonConstants.KeyValueSeperator);
             
        }       

        /// <summary>
        /// Returns the indentation level of a line
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <returns>indentation level of a line</returns>
        private static int LineLevel(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            int level = 0;
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == CommonConstants.WhiteSpace)
                {
                    level++;
                }
                else
                {
                    break;
                }
            }

            return level / ToonConstants.IndentationSize;
        }             

        /// <summary>
        /// Extracts the key from a line
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <returns>Key of the line</returns>
        private static string ExtractKeyFromLine(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            StringBuilder keyBuilder = new();
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == CommonConstants.Quote)
                {
                    _ = ParseMethods.AppendStringValue(keyBuilder, i, line);
                    // Remove ending quote
                    keyBuilder.Remove(keyBuilder.Length - 1, 1);
                     // Remove starting quote
                    keyBuilder.Remove(0, 1);
                    return keyBuilder.ToString();
                }
            }

            throw new NotImplementedException();
        }          

        /// <summary>
        /// Gets the end index of an yaml object.
        /// </summary>
        /// <param name="lines">MultiLineStringBuilder</param>
        /// <param name="startIndex">Start index of the object</param>
        /// <returns>End index</returns>
        private static int GetEndIndexOfToonObject(MultiLineStringBuilder lines, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count - 1 < startIndex)
            {
                throw new NotImplementedException();
            }

            int endIndex = -1;
            int objLevel = LineLevel(lines.GetLine(startIndex));

            for (int i = startIndex + 1; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                int currLevel = LineLevel(lines.GetLine(i));
                if (currLevel <= objLevel)
                {
                    endIndex = i - 1;
                    break;
                }
            }

            if (endIndex == -1)
            {
                endIndex = lines.Count - 1;
            }

            return endIndex;
        }           
    }
}