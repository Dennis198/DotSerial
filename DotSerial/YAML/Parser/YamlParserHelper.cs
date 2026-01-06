using System.Text;
using DotSerial.Common;
using DotSerial.Utilities;

namespace DotSerial.YAML.Parser
{
    public static class YamlParserHelper
    {
        /// <summary>
        /// Extracts list of primitives from yaml string
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <param name="startIndex">Start index if list</param>
        /// <param name="endIndex">End index of list</param>
        /// <returns>List<string></returns>
        internal static List<string?> ExtractPrimitiveList(List<StringBuilder> lines, int startIndex, int endIndex)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (startIndex < 0 || startIndex > endIndex || startIndex > lines.Count - 1)
            {
                throw new NotImplementedException();
            }
            if (startIndex > lines.Count - 1)
            {
                throw new NotImplementedException();
            }

            var result = new List<string?>();
            for (int i = startIndex; i <= endIndex; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    var c = lines[i][j];
                    if (c == CommonConstants.Quote)
                    {
                        StringBuilder dontNeed = new();
                        j = ParseMethods.AppendStringValue(dontNeed, j, lines[i].ToString());
                        // Remove ending quote
                        dontNeed.Remove(dontNeed.Length - 1, 1);
                        // Remove starting quote
                        dontNeed.Remove(0, 1);

                        result.Add(dontNeed.ToString());
                    }
                    else if (c == CommonConstants.N)
                    {
                        if (j + 3 > lines[i].Length - 1) throw new NotImplementedException();

                        j++;
                        if (lines[i][j] != CommonConstants.U) throw new NotImplementedException();
                        j++;
                        if (lines[i][j] != CommonConstants.L) throw new NotImplementedException();
                        j++;
                        if (lines[i][j] != CommonConstants.L) throw new NotImplementedException();

                        result.Add(null);
                    }
                }
            }

            return result;
        }

        internal static List<YamlParserOptions> ExtractObjectList(List<StringBuilder> lines, int startIndex, int eIndex)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (startIndex < 0 || eIndex < startIndex)
            {
                throw new NotImplementedException();
            }

            var result = new List<YamlParserOptions>();
            int objLevel = LineLevel(lines[startIndex]);
            int index = 0;

            for (int i = startIndex; i <= eIndex; i++)
            {
                // Check if we reached the end of the object
                int currLevel = LineLevel(lines[i]);
                if (currLevel < objLevel)
                {
                    break;
                }

                if (true == lines[i].EqualFirstNoWhiteSpaceChar(YAMLConstants.ListItemIndicator))
                {
                    string key = index.ToString();
                    int endIndex = GetEndIndexOfYamlObject(lines, i);
                    bool isList = false;
                    if (i != endIndex)
                    {
                        // TODO Sonderfall empty
                        isList = IsLineListItem(lines, i, 2);
                    }

                    var helpObj = new YamlParserOptions(key, currLevel, i, endIndex);
                    helpObj.SetIsYamlObject();
                    if (isList)
                        helpObj.SetIsList();

                    result.Add(helpObj);
                    RemoveListItemIndicator(lines, i, endIndex);
                    i = endIndex;
                    index++;
                }
                
            }

            return result;
        } 

        private static void RemoveListItemIndicator(List<StringBuilder> lines, int startIndex, int endIndex)
        {
            int index = LineLevel(lines[startIndex]) * YAMLConstants.IndentationSize;

            if (lines[startIndex][index] != YAMLConstants.ListItemIndicator)
            {
                throw new NotImplementedException();
            }

            lines[startIndex].Remove(index, 1);

            for (int i = startIndex; i <= endIndex; i++)
            {
                for(int j = index; j < index + YAMLConstants.IndentationSize; j++)
                {
                    if (Char.IsWhiteSpace(lines[i][index]))
                    {
                        lines[i].Remove(index, 1);
                    }
                    else
                    {
                        break;
                    }
                }   
            }
        }      

        /// <summary>
        /// Extracts key value pairs from yaml object
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <param name="startIndex">Start index if object</param>
        /// <returns></returns>
        internal static Dictionary<string, YamlParserOptions> ExtractKeyValuePairsFromYamlObject(List<StringBuilder> lines, int startIndex, int endIndex)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (startIndex < 0 || endIndex < startIndex)
            {
                throw new NotImplementedException();
            }

            var result = new Dictionary<string, YamlParserOptions>();
            int objLevel = LineLevel(lines[startIndex]);

            for (int i = startIndex; i <= endIndex; i++)
            {
                // Check if we reached the end of the object
                int currLevel = LineLevel(lines[i]);
                if (currLevel < objLevel)
                {
                    break;
                }

                // "Key": "Value"
                if (false == IsKeyLine(lines[i]))
                {
                    string key = ExtractKeyFromLine(lines[i]);
                    var helpObj = new YamlParserOptions(key, currLevel, i, i);

                    // "Key" : {}
                    if (IsEmptyObject(lines[i]))
                    {
                        helpObj.SetIsYamlObject();
                        helpObj.IsEmptyObject = true;
                    }

                    // "Key" : []
                    if (IsEmptyList(lines[i]))
                    {
                        helpObj.SetIsYamlObject();
                        helpObj.SetIsList();
                        helpObj.IsEmptyList = true;
                    }
                   
                    result.Add(key, helpObj);
                }
                // "Key": 
                else
                {
                    string key = ExtractKeyFromLine(lines[i]);

                    int sIndex = i + 1;
                    int eIndex = GetEndIndexOfYamlObject(lines, i);

                    bool isList = IsLineListItem(lines, sIndex, 1);

                    var helpObj = new YamlParserOptions(key, currLevel, sIndex, eIndex);
                    helpObj.SetIsYamlObject();

                    if (isList)
                        helpObj.SetIsList();

                    result.Add(key, helpObj);
                    i = eIndex;
                }
            }

            return result;
        } 

        /// <summary>
        /// Check if Line is start of yaml object.
        /// </summary>
        /// <param name="line">Line to check</param>
        /// <returns>True, if line is beginning of a yaml object</returns>
        internal static bool IsYamlObject(List<StringBuilder> lines, int startIndex, int endIndex)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var start = lines[startIndex];

            // "Key": 
            if (true == IsKeyLine(start))
            {
                return true;
            }
            // "Key": "Value"
            else
            {
                // "Key" : {}
                if (IsEmptyObject(start))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if line is start of yaml list
        /// </summary>
        /// <param name="lines">Lines</param>
        /// <param name="startIndex">List start index</param>
        /// <param name="listNestedCount">Nested number to check</param>
        /// <returns>True, if line is beginning of yaml list.</returns>
        internal static bool IsYamlList(List<StringBuilder> lines, int startIndex, int listNestedCount = 1)
        {
            ArgumentNullException.ThrowIfNull(lines);
            
            var start = lines[startIndex];

            if (true == IsKeyLine(start))
            {
                int indexOfFirstItem = startIndex + 1;
                bool isList = IsLineListItem(lines, indexOfFirstItem, listNestedCount);
                return isList;
            }
            else
            {
                if (IsEmptyList(start))
                {
                    return true;   
                }
            }

            return false;
        }


        private static bool IsLineListItem(List<StringBuilder> lines, int index, int minCount)
        {
            var sb = lines[index];
            int count = 0;
            for (int i = 0; i < sb.Length; i++)
            {
                char c = sb[i];

                if (Char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == YAMLConstants.ListItemIndicator)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            if (count >= minCount)
            {
                return true;
            }
            else
            {
                return false;
            }
            // return CheckFirstNoWhiteSpaceChar(sb, YAMLConstants.ListItemIndicator);
        }                                 

        private static bool IsEmptyObject(StringBuilder line)
        {
            // TODO mit extensions lösen
            bool closedBracletFound = false;

            for (int i = line.Length -1 ; i >= 0; i--)
            {
                char c = line[i];

                if (Char.IsWhiteSpace(c))
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

        private static bool IsEmptyList(StringBuilder line)
        {
            bool closedBracletFound = false;

            for (int i = line.Length -1 ; i >= 0; i--)
            {
                char c = line[i];

                if (Char.IsWhiteSpace(c))
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
        /// Returns the end index of a yaml object
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <param name="startIndex">Start index of object</param>
        /// <returns></returns>
        private static int GetEndIndexOfYamlObject(List<StringBuilder> lines, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count - 1 <= startIndex)
            {
                throw new NotImplementedException();
            }

            int endIndex = -1;
            int objLevel = LineLevel(lines[startIndex]);

            for (int i = startIndex + 1; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                int currLevel = LineLevel(lines[i]);
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
                    _ = ParseMethods.AppendStringValue(keyBuilder, i, line.ToString());
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
                        _ = ParseMethods.AppendStringValue(keyBuilder, i, line.ToString());
                        // Remove ending quote
                        keyBuilder.Remove(keyBuilder.Length - 1, 1);
                        // Remove starting quote
                        keyBuilder.Remove(0, 1);
                        
                        return keyBuilder.ToString();
                    }
                    else
                    {
                        StringBuilder dontNeed = new();
                        i = ParseMethods.AppendStringValue(dontNeed, i, line.ToString());
                        keyWasFound = true;
                    }
                }
                else if (keyWasFound && c == CommonConstants.N)
                {
                    if (i + 3 > line.Length - 1) throw new NotImplementedException();

                    i++;
                    if (line[i] != CommonConstants.U) throw new NotImplementedException();
                    i++;
                    if (line[i] != CommonConstants.L) throw new NotImplementedException();
                    i++;
                    if (line[i] != CommonConstants.L) throw new NotImplementedException();
                    return null;
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

            return level / YAMLConstants.IndentationSize;
        }  

        /// <summary>
        /// Check if line is a key line
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <returns>True, if line is key line</returns>
        private static bool IsKeyLine(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            return line.EqualLastNoWhiteSpaceChar(YAMLConstants.KeyValueSeperator);
             
            // for (int i = line.Length - 1; i >= 0; i--)
            // {
            //     var c = line[i];
            //     if (c == CommonConstants.WhiteSpace)
            //     {
            //         continue;
            //     }
            //     else if (c == YAMLConstants.KeyValueSeperator)
            //     {
            //         return true;
            //     }
            //     else
            //     {
            //         return false;
            //     }
            // }

            // return false;
        }    

        internal static bool IsPrimitiveList(StringBuilder line)
        {
            // TODO SCHAUEN WAS PASSIERT WENN ERSTE NULL IST ÜBERALL!!!!

            ArgumentNullException.ThrowIfNull(line);

            bool keyOrValueFound = false;
            int numListIndicator = 0;

            for (int i = 0; i < line.Length; i++ )
            {
                var c = line[i];
                if (Char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == YAMLConstants.ListItemIndicator)
                {
                    if (keyOrValueFound)
                    {
                        throw new NotImplementedException();
                    }
                    if (numListIndicator > 0)
                    {
                        return false;
                    }
                    numListIndicator++;
                    continue;
                }
                else if (c == CommonConstants.Quote)
                {                    
                    StringBuilder dontNeed = new();
                    i = ParseMethods.AppendStringValue(dontNeed, i, line.ToString());
                    keyOrValueFound = true;
                }
                else if (c == YAMLConstants.KeyValueSeperator)
                {
                    if (keyOrValueFound)
                    {
                        return false;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            return true;
        }     

        /// <summary>
        /// Create a list of stringbuilders representing each lines of the
        /// yaml string
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <returns>List<StringBuilder></returns>
        internal static List<StringBuilder> CreateLines(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            List<StringBuilder> result = [];
            bool createNewLine = false;
            StringBuilder currentLine = new();

            result.Add(currentLine);

            for (int i = 0; i < sb.Length; i++)
            {
                if (createNewLine)
                {
                    currentLine = new StringBuilder();
                    result.Add(currentLine);
                    createNewLine = false;
                }

                var c = sb[i];
                if (c == CommonConstants.Quote)
                {
                    i = ParseMethods.AppendStringValue(currentLine, i, sb.ToString());
                }
                // Both is needed for crossplatform
                else if (c == '\n' || c == '\r')
                {
                    createNewLine = true;
                }
                else
                {
                    currentLine.Append(c);
                }
            }

            return result;
        }   

        /// <summary>
        /// Trims the lines by removing empty lines and trailing whitespace
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        internal static void TrimLines(List<StringBuilder> lines)
        {
            // Remove Empty Lines
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                if (lines[i].IsNullOrWhiteSpace())
                {
                    lines.RemoveAt(i);
                }
            }
            
            // Remove trailing whitespace
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].TrimEnd();
            }
        }                                                           
    }
}