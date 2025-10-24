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

using DotSerial.Core.Exceptions.Node;
using DotSerial.Core.Exceptions.YAML;
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
        /// <summary>
        /// Help object for yaml parsing
        /// </summary>
        [DebuggerDisplay("Key = {Key}, Level = {Level}, Start = {startLineIndex}, End = {endLineIndex}")]
        internal class YamlHelpObject
        {
            internal string Key;
            internal int Level;
            internal int StartLineIndex;
            internal int EndLineIndex;

            internal YamlHelpObject(string key, int level, int startLineIndex, int endLineIndex)
            {
                Key = key;
                Level = level;
                this.StartLineIndex = startLineIndex;
                this.EndLineIndex = endLineIndex;
            }

            /// <summary>
            /// Check if object is a yaml object or just a key value pair
            /// </summary>
            /// <returns>True, if object</returns>
            internal bool IsYamlObject()
            {
                return StartLineIndex != EndLineIndex;
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
                throw new DSInvalidYAMLException();
            }

            StringBuilder sb = new(yamlString);
            sb.Remove(sb.Length - 4, 4);
            sb.Remove(0, 4);

            var lines = CreateLines(sb);
            TrimLines(lines);

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

        /// <summary>
        /// Converts a yaml string to children of a node
        /// </summary>
        /// <param name="parent">Parent node</param>
        /// <param name="lines">Stringbuilder-List</param>
        /// <param name="helpObj">Help-Obejct</param>
        private static void ChildrenToNode(DSNode parent, List<StringBuilder> lines, YamlHelpObject helpObj)
        {
            ArgumentNullException.ThrowIfNull(parent);
            ArgumentNullException.ThrowIfNull(lines);
            ArgumentNullException.ThrowIfNull(helpObj);

            // Check if helpObj is yaml-Object
            if (helpObj.IsYamlObject())
            {
                // Extract key, value pairs
                var dic = ExtractKeyValuePairsFromYamlObject(lines, helpObj.StartLineIndex + 1);
                var currPropType = GetPropertyTypeFromYamlObject(lines, helpObj);

                // Set property type of parent
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
                        continue;
                    }

                    // Check if value is yaml object or key value pair
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
                            throw new DSInvalidNodeTypeException(currPropType);
                        }
                    }
                    else
                    {
                        string? strValue = ExtractValueFromLine(lines[value.StartLineIndex]);
                        DSNode child = new(key, strValue, DSNodeType.Leaf, DSNodePropertyType.Primitive);
                        parent.AppendChild(child);
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Converts dictionaty to node
        /// </summary>
        /// <param name="parent">Parent node</param>
        /// <param name="lines">Stringbuilder-List</param>
        /// <param name="helpObj">Help-Obejct</param>
        private static void DictionaryToNode(DSNode parent, List<StringBuilder> lines, YamlHelpObject helpObj)
        {
            ///     (node) (Dictionary)
            ///       |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (KeyValuePairs)
            ///  :     |     :
            ///  :    (D)    :  (Value of KeyvaluePairs)
                        
            ArgumentNullException.ThrowIfNull(parent);
            ArgumentNullException.ThrowIfNull(lines);
            ArgumentNullException.ThrowIfNull(helpObj);

            // Check if type is list => error
            if (parent.PropType != DSNodePropertyType.Dictionary)
            {
                throw new DSInvalidNodeTypeException(parent.PropType);
            }

            var dic = ExtractKeyValuePairsFromYamlObject(lines, helpObj.StartLineIndex + 1);
            
            foreach (var keyValuePair in dic)
            {
                string key = keyValuePair.Key;
                var value = keyValuePair.Value;
                    
                DSNode keyValuePairNode = new(key, DSNodeType.InnerNode, DSNodePropertyType.KeyValuePair); ;
                DSNode? keyValuePairNodeValue;

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
                    string? strValue = ExtractValueFromLine(lines[value.StartLineIndex]);
                    keyValuePairNodeValue = new(key, strValue, DSNodeType.Leaf, DSNodePropertyType.KeyValuePairValue);
                }

                keyValuePairNode.AppendChild(keyValuePairNodeValue);
                parent.AppendChild(keyValuePairNode);
            }
        }

        /// <summary>
        /// Converts list to node
        /// </summary>
        /// <param name="parent">Parent node</param>
        /// <param name="lines">Stringbuilder-List</param>
        /// <param name="helpObj">Help-Obejct</param>
        private static void ListToNode(DSNode parent, List<StringBuilder> lines, YamlHelpObject helpObj)
        {
            ///      (node) (List)
            ///        |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (Items)

            ArgumentNullException.ThrowIfNull(parent);
            ArgumentNullException.ThrowIfNull(lines);
            ArgumentNullException.ThrowIfNull(helpObj);

            // Check if type is list => error
            if (parent.PropType != DSNodePropertyType.List)
            {
                throw new DSInvalidNodeTypeException(parent.PropType);
            }

            // Check if list is list of primitive or objects
            if (false == IsKeyLine(lines[helpObj.StartLineIndex + 1]))
            {
                // Extract primitive list
                var items = ExtractPrimitiveList(lines, helpObj.StartLineIndex + 1, helpObj.EndLineIndex);
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
                var items = ExtractKeyValuePairsFromYamlObject(lines, helpObj.StartLineIndex + 1);
                for (int i = 0; i < items.Count; i++)
                {
                    DSNode childNode = new(i.ToString(), DSNodeType.InnerNode, DSNodePropertyType.Class);
                    ChildrenToNode(childNode, lines, items[i.ToString()]);
                    parent.AppendChild(childNode);
                }
            }
        }

        /// <summary>
        /// Extracts list of primitives from yaml string
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <param name="startIndex">Start index if list</param>
        /// <param name="endIndex">End index of list</param>
        /// <returns>List<string></returns>
        private static List<string?> ExtractPrimitiveList(List<StringBuilder> lines, int startIndex, int endIndex)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (startIndex < 0 || startIndex > endIndex || startIndex > lines.Count - 1)
            {
                throw new DSInvalidYAMLException();
            }
            if (startIndex > lines.Count - 1)
            {
                throw new DSInvalidYAMLException();
            }

            var result = new List<string?>();
            for (int i = startIndex; i <= endIndex; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    var c = lines[i][j];
                    if (c == YAMLConstants.Quote)
                    {
                        StringBuilder dontNeed = new();
                        j = AppendStringValue(dontNeed, j, lines[i].ToString());
                        // Remove ending quote
                        dontNeed.Remove(dontNeed.Length - 1, 1);
                        // Remove starting quote
                        dontNeed.Remove(0, 1);

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

        /// <summary>
        /// Extracts key value pairs from yaml object
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <param name="startIndex">Start index if object</param>
        /// <returns></returns>
        private static Dictionary<string, YamlHelpObject> ExtractKeyValuePairsFromYamlObject(List<StringBuilder> lines, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (startIndex < 0 || lines.Count - 1 < startIndex)
            {
                throw new DSInvalidYAMLException();
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
                    var helpObj = new YamlHelpObject(key, currLevel, i, i);
                   

                    result.Add(key, helpObj);
                }
                else
                {
                    string key = ExtractKeyFromLine(lines[i]);
                    int endIndex = GetEndIndexOfYamlObject(lines, i);
                    var helpObj = new YamlHelpObject(key, currLevel, i, endIndex);
                    result.Add(key, helpObj);
                    i = endIndex;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the property type from yaml object
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <param name="helpObj">Help-Obejct</param>
        /// <returns>DSNodePropertyType</returns>
        private static DSNodePropertyType GetPropertyTypeFromYamlObject(List<StringBuilder> lines, YamlHelpObject helpObj)
        {
            ArgumentNullException.ThrowIfNull(lines);
            ArgumentNullException.ThrowIfNull(helpObj);

            int startIndex = helpObj.StartLineIndex;
            int endIndex = helpObj.EndLineIndex;

            for (int i = startIndex + 1; i <= endIndex; i++)
            {
                if (false == IsKeyLine(lines[i]))
                {
                    string key = ExtractKeyFromLine(lines[i]);
                    if (key == YAMLConstants.PropertyTypeKey)
                    {
                        string? value = ExtractValueFromLine(lines[i]);
                        var tmp = ParsePropertyTypeInfo(value);
                        return tmp;
                    }

                }
            }

            throw new DSInvalidYAMLException();
        }

        /// <summary>
        /// Create a list of stringbuilders representing each lines of the
        /// yaml string
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <returns>List<StringBuilder></returns>
        private static List<StringBuilder> CreateLines(StringBuilder sb)
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
                throw new DSInvalidYAMLException();
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
                if (c == YAMLConstants.WhiteSpace)
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

        /// <summary>
        /// Extracts the value from a line
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <returns>Value of the line</returns>
        private static string? ExtractValueFromLine(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            StringBuilder keyBuilder = new();
            bool keyWasFound = false;

            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == YAMLConstants.Quote)
                {
                    if (keyWasFound)
                    {
                        _ = AppendStringValue(keyBuilder, i, line.ToString());
                        // Remove ending quote
                        keyBuilder.Remove(keyBuilder.Length - 1, 1);
                        // Remove starting quote
                        keyBuilder.Remove(0, 1);
                        
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

            throw new DSInvalidYAMLException();
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
                if (c == YAMLConstants.Quote)
                {
                    _ = AppendStringValue(keyBuilder, i, line.ToString());
                    // Remove ending quote
                    keyBuilder.Remove(keyBuilder.Length - 1, 1);
                     // Remove starting quote
                    keyBuilder.Remove(0, 1);
                    return keyBuilder.ToString();
                }
            }

            throw new DSInvalidYAMLException();
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
                throw new DSInvalidYAMLException(yamlString);
            }

            if (yamlString.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (yamlString[startIndex] != YAMLConstants.Quote)
            {
                throw new DSInvalidYAMLException(yamlString);
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
                throw new DSInvalidYAMLException();
            }

            return value.ConvertToDSNodePropertyType();
        }        
        
        /// <summary>
        /// Trims the lines by removing empty lines and trailing whitespace
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        private static void TrimLines(List<StringBuilder> lines)
        {
            // Remove Empty Lines
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                string lineStr = lines[i].ToString();
                if (string.IsNullOrWhiteSpace(lineStr))
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