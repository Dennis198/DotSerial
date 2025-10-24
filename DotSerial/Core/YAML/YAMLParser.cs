using DotSerial.Core.Exceptions.Node;
using DotSerial.Core.General;
using System.Diagnostics;
using System.Text;

namespace DotSerial.Core.YAML
{
    /// <summary>
    /// Class which parses Yaml content
    /// </summary>
    internal static class YAMLParser
    {
        [DebuggerDisplay("Key = {Key}, Level = {Level}, Start = {startLineIndex}, End = {endLineIndex}")]
        internal class YamlHelpObject
        {
            internal string Key;
            internal int Level;
            internal int startLineIndex;
            internal int endLineIndex;
            internal DSNodePropertyType type;

            internal bool IsYamlObject()
            {
                return startLineIndex != endLineIndex;
            }
        }

        /// <summary>
        /// Converts Yaml string to a Yaml node
        /// </summary>
        /// <param name="yamlString">Yaml string</param>
        /// <returns>Yaml node</returns>
        public static DSNode ToNode(string yamlString)
        {
            if (string.IsNullOrWhiteSpace(yamlString))
            {
                // throw new DSInvalidJSONException(jsonString);
                throw new NotImplementedException();
            }

            StringBuilder sb = new(yamlString);
            sb.Remove(sb.Length - 4, 4);
            sb.Remove(0, 4);

            var lines = DSTest(sb);

            // TODO Remove empty lines
            // TODO Whitespace at end

            var rootDic = ExtractKeyValuePairsFromYamlObject(lines, 0);

            if (rootDic.Count != 1)
            {
                throw new DSInvalidNodeTypeException(rootDic.Count, 1);
            }

            string rootKey = rootDic.Keys.First();
            var rootNode = new DSNode(rootKey, DSNodeType.InnerNode, DSNodePropertyType.Class);

            ChildrenToNode(rootNode, lines, rootDic[rootKey]);

            return rootNode;
        }

        private static void ChildrenToNode(DSNode parent, List<StringBuilder> lines, YamlHelpObject helpObj)
        {
            if (helpObj.IsYamlObject())
            {
                // Extract key, value pairs
                var dic = ExtractKeyValuePairsFromYamlObject(lines, helpObj.startLineIndex + 1);
                var currPropType = GetPropertyTypeFromYamlObject(lines, helpObj);
                parent.SetPropertyType(currPropType);

                foreach (var keyValuePair in dic)
                {
                    // Convert key to int key
                    string key = keyValuePair.Key;
                    var value = keyValuePair.Value;

                    if (key == YAMLConstants.Version)
                    {
                        continue;
                    }

                    if (key == YAMLConstants.PropertyTypeKey)
                    {
                        // TODO
                        continue;
                    }

                    if (value.IsYamlObject())
                    {
                        if (currPropType == DSNodePropertyType.Class)
                        {
                            DSNode child = new(key, DSNodeType.InnerNode, DSNodePropertyType.Class);
                            ChildrenToNode(child, lines, value);
                            parent.AppendChild(child);
                        }
                        else if (currPropType == DSNodePropertyType.Dictionary)
                        {
                            DictionaryToNode(parent, lines, value);
                        }
                        else if (currPropType == DSNodePropertyType.List)
                        {
                            ListToNode(parent, lines, value);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        string? strValue = ExtractValueFromLine(lines[value.startLineIndex]);
                        if (strValue == null)
                        {
                            // TODO if kann weg oder?
                            DSNode child = new(key, null, DSNodeType.Leaf, DSNodePropertyType.Null);
                            parent.AppendChild(child);
                            continue;
                        }
                        else
                        {
                            DSNode child = new(key, strValue, DSNodeType.Leaf, DSNodePropertyType.Primitive);
                            parent.AppendChild(child);
                        }

                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static void DictionaryToNode(DSNode parent, List<StringBuilder> lines, YamlHelpObject helpObj)
        {
            // Check if type is list => error
            if (parent.PropType != DSNodePropertyType.Dictionary)
            {
                throw new DSInvalidNodeTypeException(parent.PropType);
            }

            var dic = ExtractKeyValuePairsFromYamlObject(lines, helpObj.startLineIndex + 1);
            
            foreach (var keyValuePair in dic)
            {
                // Convert key to int key
                string key = keyValuePair.Key;
                var value = keyValuePair.Value;
                    
                DSNode keyValuePairNode = new(key, DSNodeType.InnerNode, DSNodePropertyType.KeyValuePair); ;
                DSNode? keyValuePairNodeValue = null;

                if (value == null)
                {
                    keyValuePairNodeValue = new(key, null, DSNodeType.Leaf, DSNodePropertyType.KeyValuePairValue);
                    keyValuePairNode.AppendChild(keyValuePairNodeValue);
                    parent.AppendChild(keyValuePairNode);
                    continue;
                }
                else if (value.IsYamlObject())
                {
                    keyValuePairNodeValue = new(key, null, DSNodeType.InnerNode, DSNodePropertyType.KeyValuePairValue);
                    ChildrenToNode(keyValuePairNodeValue, lines, value);
                    keyValuePairNodeValue.SetPropertyType(DSNodePropertyType.KeyValuePairValue);
                }
                else // Primitive
                {
                    string? strValue = ExtractValueFromLine(lines[value.startLineIndex]);
                    keyValuePairNodeValue = new(key, strValue, DSNodeType.Leaf, DSNodePropertyType.KeyValuePairValue);
                }

                keyValuePairNode.AppendChild(keyValuePairNodeValue);
                parent.AppendChild(keyValuePairNode);
            }
        }

        private static void ListToNode(DSNode parent, List<StringBuilder> lines, YamlHelpObject helpObj)
        {
            // Check if type is list => error
            if (parent.PropType != DSNodePropertyType.List)
            {
                throw new DSInvalidNodeTypeException(parent.PropType);
            }

            // Check if list is list of primitive or objects
            if (false == IsKeyLine(lines[helpObj.startLineIndex + 1]))
            {
                // Extract primitive list
                var items = ExtractPrimitiveList(lines, helpObj.startLineIndex + 1, helpObj.endLineIndex);
                for (int i = 0; i < items.Count; i++)
                {
                    string? value = items[i];
                    var child = new DSNode(i.ToString(), value, DSNodeType.Leaf, DSNodePropertyType.Primitive);
                    parent.AppendChild(child);
                }
            }
            else
            {
                // Extract object list
                var items = ExtractKeyValuePairsFromYamlObject(lines, helpObj.startLineIndex + 1);
                for (int i = 0; i < items.Count; i++)
                {
                    DSNode childNode = new(i.ToString(), DSNodeType.InnerNode, DSNodePropertyType.Class);
                    ChildrenToNode(childNode, lines, items[i.ToString()]);
                    parent.AppendChild(childNode);
                }
            }
        }
        
        private static List<string?> ExtractPrimitiveList(List<StringBuilder> lines, int startIndex, int endIndex)
        {
            var result = new List<string?>();
            int currLevel = LineLevel(lines[startIndex]);
            for (int i = startIndex; i <= endIndex; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    var c = lines[i][j];
                    if (c == YAMLConstants.Quote)
                    {
                        StringBuilder dontNeed = new();
                        j = AppendStringValue(dontNeed, j, lines[i].ToString());
                        dontNeed.Remove(dontNeed.Length - 1, 1); // Remove ending quote
                        dontNeed.Remove(0, 1); // Remove starting quote
                        result.Add(dontNeed.ToString());
                    }
                    else if (c == YAMLConstants.N)
                    {
                        if (j + 3 > lines[i].Length - 1) throw new NotImplementedException();

                        j++;
                        if (lines[i][j] != YAMLConstants.U) throw new NotImplementedException();
                        j++;
                        if (lines[i][j] != YAMLConstants.L) throw new NotImplementedException();
                        j++;
                        if (lines[i][j] != YAMLConstants.L) throw new NotImplementedException();

                        result.Add(null);
                    }
                }
            }

            return result;
        }

        private static Dictionary<string, YamlHelpObject> ExtractKeyValuePairsFromYamlObject(List<StringBuilder> lines, int startIndex)
        {
            if (lines.Count - 1 < startIndex)
            {
                throw new NotImplementedException();
            }

            var result = new Dictionary<string, YamlHelpObject>();

            int objLevel = LineLevel(lines[startIndex]);
            for (int i = startIndex; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                int currLevel = LineLevel(lines[i]);
                if (currLevel < objLevel)
                {
                    break;
                }

                if (false == IsKeyLine(lines[i]))
                {
                    string key = ExtractKeyFromLine(lines[i]);
                    string? value = ExtractValueFromLine(lines[i]);
                    var helpObj = new YamlHelpObject
                    {
                        Key = key,
                        Level = currLevel,
                        startLineIndex = i,
                        endLineIndex = i
                    };

                    result.Add(key, helpObj);
                }
                else
                {
                    string key = ExtractKeyFromLine(lines[i]);
                    // string value = ExtractValueFromLine(lines[i]);
                    int endIndex = GetEndIndexOfYamlObject(lines, i);
                    var helpObj = new YamlHelpObject
                    {
                        Key = key,
                        Level = currLevel,
                        startLineIndex = i,
                        endLineIndex = endIndex
                    };
                    result.Add(key, helpObj);
                    i = endIndex;
                }
            }

            return result;
        }
        
        private static DSNodePropertyType GetPropertyTypeFromYamlObject(List<StringBuilder> lines, YamlHelpObject helpObj)
        {
            var result = DSNodePropertyType.Undefined;
            int startIndex = helpObj.startLineIndex;
            int endIndex = helpObj.endLineIndex;

            for (int i = startIndex + 1; i <= endIndex; i++)
            {
                if (false == IsKeyLine(lines[i]))
                {
                    string key = ExtractKeyFromLine(lines[i]);
                    if(key == YAMLConstants.PropertyTypeKey)
                    {
                        string? value = ExtractValueFromLine(lines[i]);
                        result = ParsePropertyTypeInfo(value);
                        return result;
                    }
                   
                }
            }

            throw new NotImplementedException();
        }

        private static List<StringBuilder> DSTest(StringBuilder sb)
        {
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
                if (c == YAMLConstants.Quote)
                {
                    i = AppendStringValue(currentLine, i, sb.ToString());
                }
                else if (c.ToString() == Environment.NewLine)
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
        
        private static int GetEndIndexOfYamlObject(List<StringBuilder> lines, int startIndex)
        {
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

        private static int LineLevel(StringBuilder line)
        {
            int level = 0;
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == YAMLConstants.WhiteSpace)
                {
                    level++;
                }
                else
                {
                    break;
                }
            }

            return level/YAMLConstants.IndentationSize;
        }

        private static bool IsKeyLine(StringBuilder line)
        {
            for (int i = line.Length - 1; i >= 0; i--)
            {
                var c = line[i];
                if (c == YAMLConstants.WhiteSpace)
                {
                    continue;
                }
                else if (c == YAMLConstants.KeyValueSeperator)
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

        private static string? ExtractValueFromLine(StringBuilder line)
        {
            // if (true == IsKeyLine(line))
            // {
            //     throw new NotImplementedException();
            // }

            StringBuilder keyBuilder = new();
            bool keyWasFound = false;

            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == YAMLConstants.Quote)
                {
                    if (keyWasFound)
                    {
                        i = AppendStringValue(keyBuilder, i, line.ToString());
                        keyBuilder.Remove(keyBuilder.Length - 1, 1); // Remove ending quote
                        keyBuilder.Remove(0, 1); // Remove starting quote
                        return keyBuilder.ToString();
                    }
                    else
                    {
                        StringBuilder dontNeed = new();
                        i = AppendStringValue(dontNeed, i, line.ToString());
                        keyWasFound = true;
                    }
                }
                else if (keyWasFound && c == YAMLConstants.N)
                {
                    if (i + 3 > line.Length - 1) throw new NotImplementedException();

                    i++;
                    if (line[i] != YAMLConstants.U) throw new NotImplementedException();
                    i++;
                    if (line[i] != YAMLConstants.L) throw new NotImplementedException();
                    i++;
                    if (line[i] != YAMLConstants.L) throw new NotImplementedException();
                    return null;
                }
            }

            throw new NotImplementedException();
        }

        private static string ExtractKeyFromLine(StringBuilder line)
        {
            // if(false == IsKeyLine(line))
            // {
            //     throw new NotImplementedException();
            // }

            StringBuilder keyBuilder = new();
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == YAMLConstants.Quote)
                {
                    i = AppendStringValue(keyBuilder, i, line.ToString());
                    keyBuilder.Remove(keyBuilder.Length - 1, 1); // Remove ending quote
                    keyBuilder.Remove(0, 1); // Remove starting quote
                    return keyBuilder.ToString();
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Apends the whole string from starting quote to end quote to
        /// the sting
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="startIndex">Index of the opeing quote</param>
        /// <param name="yamlString">string</param>
        /// <returns>Index of the closing quote</returns>
        private static int AppendStringValue(StringBuilder sb, int startIndex, string yamlString)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (string.IsNullOrWhiteSpace(yamlString))
            {
                // throw new DSInvalidJSONException(jsonString);
                throw new NotImplementedException();
            }

            if (yamlString.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (yamlString[startIndex] != YAMLConstants.Quote)
            {
                // throw new DSInvalidJSONException(jsonString);
                throw new NotImplementedException();
            }

            sb.Append(YAMLConstants.Quote);

            for (int j = startIndex + 1; j < yamlString.Length; j++)
            {
                var c2 = yamlString[j];
                if (c2 == '\\')
                {
                    sb.Append(c2);
                    sb.Append(yamlString[j + 1]);
                    j++;
                }
                if (c2 == YAMLConstants.Quote)
                {
                    sb.Append(c2);
                    return j;
                }
                else
                {
                    sb.Append(c2);
                }
            }

            return yamlString.Length - 1;
        }        

        /// <summary>
        /// Parse the node property type info
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>DSNodePropertyType</returns>
        private static DSNodePropertyType ParsePropertyTypeInfo(string? value)
        {
            // Check if value has value
            if (string.IsNullOrWhiteSpace(value))
            {
                // throw new DSInvalidJSONException();
                throw new NotImplementedException();
            }

            return value.ConvertToDSNodePropertyType();
        }        
    }
}