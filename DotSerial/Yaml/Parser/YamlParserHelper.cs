#region License
//Copyright (c) 2026 Dennis Sölch

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
using DotSerial.Utilities;

namespace DotSerial.Yaml.Parser
{
    /// <summary>
    /// Helper class with methode for parsing yaml.
    /// </summary>
    internal static class YamlParserHelper
    {

        /// <summary>
        /// Extracts key value pairs from yaml object
        /// </summary>
        /// <param name="lines">MultiLineStringBuilder</param>
        /// <returns>Dictionary<string, MultiLineStringBuilder></returns>
        internal static Dictionary<string, MultiLineStringBuilder> ExtractKeyValuePairsFromYamlObject(MultiLineStringBuilder lines)
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
                    int eIndex = GetEndIndexOfYamlObject(lines, i);

                    var helpObj = lines.Slice(sIndex, eIndex);
                    
                    if (sIndex == eIndex && !IsYamlList(helpObj))
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

        /// <summary>
        /// Extracts list of objcts from yaml string
        /// </summary>
        /// <param name="lines">MultiLineStringBuilder</param>
        /// <returns>List<MultiLineStringBuilder></returns>
        internal static List<MultiLineStringBuilder> ExtractObjectList(MultiLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var result = new List<MultiLineStringBuilder>();
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

                if (true == line.EqualFirstNoWhiteSpaceChar(YamlConstants.ListItemIndicator))
                {
                    int endIndex = GetEndIndexOfYamlObject(lines, i);

                    // Remove List indicator of the objects
                    RemoveListItemIndicator(lines, i, endIndex);

                    var helpObj = lines.Slice(i, endIndex);
                    result.Add(helpObj);
                    i = endIndex;
                }
                
            }

            return result;
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
                        throw new DSYamlException("Invalid yaml");
                    }

                    return null;
                }
            }

            throw new NotImplementedException();
        }                  
 
        /// <summary>
        /// Removes the List indicator for the objects
        /// </summary>
        /// <param name="lines">MultiLineStringBuilder</param>
        /// <param name="startIndex">Start index of the objcets</param>
        /// <param name="endIndex">End index of the objects</param>
        private static void RemoveListItemIndicator(MultiLineStringBuilder lines, int startIndex, int endIndex)
        {
            var startLine = lines.GetLine(startIndex);
            int index = LineLevel(startLine) * YamlConstants.IndentationSize;

            if (startLine[index] != YamlConstants.ListItemIndicator)
            {
                throw new NotImplementedException();
            }

            startLine.Remove(index, 1);

            for (int i = startIndex; i <= endIndex; i++)
            {
                var line = lines.GetLine(i);
                for(int j = index; j < index + YamlConstants.IndentationSize; j++)
                {
                    if (char.IsWhiteSpace(line[index]))
                    {
                        line.Remove(index, 1);
                    }
                    else
                    {
                        break;
                    }
                }   
            }
        }           

        /// <summary>
        /// Check if MultiLineStringBuilder is a yaml object
        /// </summary>
        /// <param name="lines">MultiLineStringBuilder</param>
        /// <returns>True, if yaml object</returns>
        internal static bool IsYamlObject(MultiLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var firstLine = lines.GetLine(0);

            if (null == firstLine)
            {
                throw new NotImplementedException();
            }

            // "'key': {}"
            if (IsEmptyObject(firstLine))
            {
                return true;
            }

            if (IsYamlList(lines))
            {
                return false;
            }

            if (IsYamlSingleValue(lines))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if MultiLineStringBuilder is a yaml list
        /// </summary>
        /// <param name="lines">MultiLineStringBuilder</param>
        /// <returns>True, if yaml list</returns>
        internal static bool IsYamlList(MultiLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var firstLine = lines.GetLine(0);

            if (null == firstLine)
            {
                throw new NotImplementedException();
            }

            // 1. "'key': []"
            // 2. "[]"
            if (IsEmptyList(firstLine))
            {
                return true;
            }

            // 1. "- 'item'"
            // 2. "- 'key' : 'item'"
            if (firstLine.EqualFirstNoWhiteSpaceChar(YamlConstants.ListItemIndicator))
            {
                return true;
            }

            return false;
        } 

        /// <summary>
        /// Check if MultiLineStringBuilder is a single value
        /// </summary>
        /// <param name="lines">MultiLineStringBuilder</param>
        /// <returns>True, if yaml single value</returns>
        internal static bool IsYamlSingleValue(MultiLineStringBuilder lines)
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

        /// <summary>
        /// Check if MultiLineStringBuilder is a key only a key value pair
        /// </summary>
        /// <param name="lines">MultiLineStringBuilder</param>
        /// <returns>True, if yaml key value pair</returns>
        internal static bool IsYamlPrimitiveLine(MultiLineStringBuilder lines)
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

            if (IsYamlList(lines))
            {
                return false;
            }

            if (IsYamlSingleValue(lines))
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
                        throw new DSYamlException("Invalid yaml");
                    }

                    i += 3;
                    valueWasFound = true;
                }
            }

            return keyWasFound && valueWasFound;
        } 
                                
        /// <summary>
        /// Check is string builder is "Key": {}
        /// </summary>
        /// <param name="line">StringBuilder</param>
        /// <returns>True, if string is an empty yaml object</returns>
        internal static bool IsEmptyObject(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            bool closedBracletFound = false;

            for (int i = line.Length -1 ; i >= 0; i--)
            {
                char c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == '}')
                {
                    if (true == closedBracletFound)
                    {
                        throw new NotImplementedException();
                    }
                    closedBracletFound = true;
                }
                else if (c == '{')
                {
                    if (false == closedBracletFound)
                    {
                        throw new NotImplementedException();
                    }

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
        /// Check is string builder is "Key": []
        /// </summary>
        /// <param name="line">StringBuilder</param>
        /// <returns>True, if string is an empty yaml list</returns>
        internal static bool IsEmptyList(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);
            
            bool closedBracletFound = false;

            for (int i = line.Length -1 ; i >= 0; i--)
            {
                char c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == ']')
                {
                    if (true == closedBracletFound)
                    {
                        throw new NotImplementedException();
                    }
                    closedBracletFound = true;
                }
                else if (c == '[')
                {
                    if (false == closedBracletFound)
                    {
                        throw new NotImplementedException();
                    }

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
        /// Removes the yaml start and end symbols if there.
        /// </summary>
        /// <param name="lines">MultiLineStringBuilder</param>
        /// <returns>MultiLineStringBuilder without start end symbols</returns>
        internal static MultiLineStringBuilder RemoveStartStopSymbols(MultiLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);
            if (lines.Count == 0)
            {
                return lines;
            }

            int startIndex = 0;
            int endIndex = lines.Count - 1;

            var firstLine = lines.GetLine(0);
            if (firstLine.Equals(YamlConstants.YamlDocumentStart))
            {
                startIndex++;
            }

            var lastLine = lines.GetLine(lines.Count - 1);
            if (lastLine.Equals(YamlConstants.YamlDocumentEnd))
            {
                endIndex--;
            }

            if (endIndex < startIndex)
            {
                lines.Clear();
                return lines;
            }

            return lines.Slice(startIndex, endIndex);
        }       

        /// <summary>
        /// Gets the end index of an yaml object.
        /// </summary>
        /// <param name="lines">MultiLineStringBuilder</param>
        /// <param name="startIndex">Start index of the object</param>
        /// <returns>End index</returns>
        private static int GetEndIndexOfYamlObject(MultiLineStringBuilder lines, int startIndex)
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

            return level / YamlConstants.IndentationSize;
        }  

        /// <summary>
        /// Check if line is a key line
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <returns>True, if line is key line</returns>
        private static bool IsKeyLine(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            return line.EqualLastNoWhiteSpaceChar(YamlConstants.KeyValueSeperator);
             
        }                                                                          
    }
}