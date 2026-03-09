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
            int objLevel = ParseMethods.LineLevel(lines.GetLine(0), YamlConstants.IndentationSize);

            for (int i = 0; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                var line = lines.GetLine(i);
                int currLevel = ParseMethods.LineLevel(line, YamlConstants.IndentationSize);
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
            int objLevel = ParseMethods.LineLevel(lines.GetLine(0), YamlConstants.IndentationSize);

            for (int i = 0; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                var line = lines.GetLine(i);
                int currLevel = ParseMethods.LineLevel(line, YamlConstants.IndentationSize);
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

            int start = -1;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    i = line.SkipStringValue(i);
                }
                else if (c == YamlConstants.KeyValueSeperator)
                {
                    start = i;
                }
                else
                {
                    i = line.SkipTillStopChar(i, YamlConstants.KeyValueSeperator);
                }
            }

            if (start == -1)
            {
                throw new NotImplementedException();
            }

            // Skip seperator
            start++;

            for (int i = start; i < line.Length; i++)
            {
                var c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                   _ = ParseMethods.AppendStringValue(keyBuilder, i, line);                    
                    return keyBuilder.ToString();
                }
                else
                {
                   _ = ParseMethods.AppendTillStopChar(keyBuilder, i, line, null);
                   return keyBuilder.ToString();
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
            int index = ParseMethods.LineLevel(startLine, YamlConstants.IndentationSize) * YamlConstants.IndentationSize;

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
                if(lines.Count != 1)
                {
                    return false;
                }
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

            if (IsEmptyList(firstLine) || IsEmptyObject(firstLine))
            {
                return false;
            }             

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

            bool seperatorFound = false;
            bool keyFound = false;

            for (int i = 0; i < firstLine.Length; i++)
            {
                char c = firstLine[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (true == keyFound && c == YamlConstants.KeyValueSeperator)
                {
                    seperatorFound = true;
                    continue;
                }

                if (seperatorFound)
                {
                    return false;
                }

                if (c == CommonConstants.Quote)
                {
                    i = firstLine.SkipStringValue(i);
                    keyFound = true;
                }
                else
                {
                    i = firstLine.SkipTillStopChar(i, YamlConstants.KeyValueSeperator);
                    keyFound = true;
                }
            }

            return true;
        }   

        /// <summary>
        /// Check if MultiLineStringBuilder is only a key value pair
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

            if (IsEmptyObject(lines.GetLine(0)))
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
            int start = -1;
            for (int i = 0; i < firstLine.Length; i++)
            {
                char c = firstLine[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    i = firstLine.SkipStringValue(i);
                }
                else if (c == YamlConstants.KeyValueSeperator)
                {
                    start = i;
                }
                else
                {
                    i = firstLine.SkipTillStopChar(i, YamlConstants.KeyValueSeperator);
                }
            }

            if (start == -1)
            {
                return false;
            }

            bool valueWasFound =  false;

            for (int i = start; i < firstLine.Length; i++)
            {
                var c = firstLine[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (valueWasFound)
                {
                    throw new NotImplementedException();
                }                

                if (c == CommonConstants.Quote)
                {
                    i = firstLine.SkipStringValue(i);                        
                    valueWasFound = true;
                }
                else
                {
                    i= firstLine.SkipTillStopChar(i, null);
                    valueWasFound = true;
                }
            }

            return valueWasFound;
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
                else if (c == CommonConstants.BracesEnd)
                {
                    if (true == closedBracletFound)
                    {
                        throw new NotImplementedException();
                    }
                    closedBracletFound = true;
                }
                else if (c == CommonConstants.BracesStart)
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
                else if (c == CommonConstants.BracketsEnd)
                {
                    if (true == closedBracletFound)
                    {
                        throw new NotImplementedException();
                    }
                    closedBracletFound = true;
                }
                else if (c == CommonConstants.BracketsStart)
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
            int objLevel = ParseMethods.LineLevel(lines.GetLine(startIndex), YamlConstants.IndentationSize);

            for (int i = startIndex + 1; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                int currLevel = ParseMethods.LineLevel(lines.GetLine(i), YamlConstants.IndentationSize);
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

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }                 

                if (c == CommonConstants.Quote)
                {
                    _ = ParseMethods.AppendStringValue(keyBuilder, i, line);
                    return keyBuilder.ToString();
                }
                else
                {
                    _ = ParseMethods.AppendTillStopChar(keyBuilder, i, line, YamlConstants.KeyValueSeperator);
                    return keyBuilder.ToString();
                }
            }

            throw new NotImplementedException();
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