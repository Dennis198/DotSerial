using System.Drawing;
using System.Numerics;
using System.Text;
using DotSerial.Core.General;
using DotSerial.Core.Misc;
using DotSerial.Core.Tree;

namespace DotSerial.Core.YAML.Parser
{
    public class YamlParserVisitor : IYamlNodeParserVisitor
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <inheritdoc/>
        public static DSYamlNode Parse(string yamlString)
        {
            StringBuilder sb = new(yamlString);
            sb.Remove(sb.Length - 4, 4);
            sb.Remove(0, 3);

            var lines = CreateLines(sb);
            TrimLines(lines);

            var rootNode = _nodeFactory.CreateNode(GeneralConstants.MainObjectKey, null, NodeType.InnerNode);

            var options = new YamlParserOptions(GeneralConstants.MainObjectKey, 0, 0, lines.Count - 1);
            options.SetIsYamlObject();

            if (lines.Count > 0)
            {
                ParserAccept(rootNode, new YamlParserVisitor(), lines, options);
            }

            return new DSYamlNode(rootNode);
        }

        /// <summary>
        /// Parser for json
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="sb">Stringbuilder</param>
        private static void ParserAccept(IDSNode node, YamlParserVisitor visitor, List<StringBuilder> lines, YamlParserOptions options)
        {
            if (node is LeafNode leafNode)
            {
                visitor.VisitLeafNode(leafNode, lines, options);    
            }
            else if (node is InnerNode innerNode)
            {
                visitor.VisitInnerNode(innerNode, lines, options);    
            }
            else if (node is ListNode listNode)
            {
                visitor.VisitListNode(listNode, lines, options);    
            }
            else if (node is DictionaryNode dicNode)
            {
                visitor.VisitDictionaryNode(dicNode, lines, options);
            }
            else
            {
                throw new NotImplementedException();
            }   
        }          

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, List<StringBuilder> lines, YamlParserOptions options)
        {
            // Currenlty not needed
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, List<StringBuilder> lines, YamlParserOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(lines);
            ArgumentNullException.ThrowIfNull(options);

            // Check if helpObj is yaml-Object
            if (options.IsYamlObject())
            {
                // Extract key, value pairs
                var dic = ExtractKeyValuePairsFromYamlObject(lines, options.StartLineIndex, options.EndLineIndex);

                foreach (var keyValuePair in dic)
                {
                    // Convert key to int key
                    string key = keyValuePair.Key;
                    var value = keyValuePair.Value;

                    // Check if value is yaml object or key value pair
                    if (value.IsYamlObject())
                    {
                        if (false == value.IsList)
                        {
                            // Create inner node
                            var innerNode = _nodeFactory.CreateNode(key, null, NodeType.InnerNode) as InnerNode ?? throw new NotImplementedException();

                            if (false == value.IsEmptyObject)
                            {
                                // Parse inner node
                                ParserAccept(innerNode, new YamlParserVisitor(), lines, value);
                            }

                            // Add inner node to parent
                            node.AddChild(innerNode);
                        }
                        else if (value.IsList)
                        {
                            // Create list node
                            var listNode = _nodeFactory.CreateNode(key, null, NodeType.ListNode);

                            if (false == value.IsEmptyList)
                            {
                                // Parse inner node
                                ParserAccept(listNode, new YamlParserVisitor(), lines, value);
                            }

                            // Add inner node to parent
                            node.AddChild(listNode);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        string? strValue = ExtractValueFromLine(lines[value.StartLineIndex]);
                        var childNode = _nodeFactory.CreateNode(key, strValue, NodeType.Leaf);
                        node.AddChild(childNode);
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, List<StringBuilder> lines, YamlParserOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(lines);
            ArgumentNullException.ThrowIfNull(options);

            // Check if list is list of primitive or objects
            if (true == IsPrimitiveList(lines[options.StartLineIndex]))
            {
                // Extract primitive list
                var items = ExtractPrimitiveList(lines, options.StartLineIndex, options.EndLineIndex);
                for (int i = 0; i < items.Count; i++)
                {
                    string? value = items[i];
                    var child = _nodeFactory.CreateNode(i.ToString(), value, NodeType.Leaf);
                    node.AddChild(child);
                }
            }
            else
            {
                // Extract object list
                var items2 = ExtractObjectList(lines,options.StartLineIndex, options.EndLineIndex);
                // var items = ExtractKeyValuePairsFromYamlObject(lines, options.StartLineIndex + 1);
                int index = 0;
                foreach (var keyValuePair in items2)
                {
                     // Convert key to int key
                    string key = index.ToString();
                    var value = keyValuePair;

                    if (false == value.IsList)
                    {
                        // Create inner node
                        var innerNode = _nodeFactory.CreateNode(key, null, NodeType.InnerNode) as InnerNode ?? throw new NotImplementedException();

                        // Parse inner node
                        ParserAccept(innerNode, new YamlParserVisitor(), lines, value);

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (value.IsList)
                    {
                        // Create list node
                        var listNode = _nodeFactory.CreateNode(key, null, NodeType.ListNode);

                        // Parse inner node
                        ParserAccept(listNode, new YamlParserVisitor(), lines, value);

                        // Add inner node to parent
                        node.AddChild(listNode);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }       

                    index++;             
                }
            }
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, List<StringBuilder> lines, YamlParserOptions options)
        {
            // Currenlty not needed
            throw new NotImplementedException();
        }

        // =========================================================================================


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
                    if (c == GeneralConstants.Quote)
                    {
                        StringBuilder dontNeed = new();
                        j = ParseMethods.AppendStringValue(dontNeed, j, lines[i].ToString());
                        // Remove ending quote
                        dontNeed.Remove(dontNeed.Length - 1, 1);
                        // Remove starting quote
                        dontNeed.Remove(0, 1);

                        result.Add(dontNeed.ToString());
                    }
                    else if (c == GeneralConstants.N)
                    {
                        if (j + 3 > lines[i].Length - 1) throw new NotImplementedException();

                        j++;
                        if (lines[i][j] != GeneralConstants.U) throw new NotImplementedException();
                        j++;
                        if (lines[i][j] != GeneralConstants.L) throw new NotImplementedException();
                        j++;
                        if (lines[i][j] != GeneralConstants.L) throw new NotImplementedException();

                        result.Add(null);
                    }
                }
            }

            return result;
        }

        private static List<YamlParserOptions> ExtractObjectList(List<StringBuilder> lines, int startIndex, int eIndex)
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

                if (true == CheckFirstNoWhiteSpaceChar(lines[i], YAMLConstants.ListItemIndicator))
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

        private static bool CheckFirstNoWhiteSpaceChar(StringBuilder line, char c)
        {
            ArgumentNullException.ThrowIfNull(line);

            for (int i = 0; i < line.Length; i++)
            {
                var currChar = line[i];
                if (Char.IsWhiteSpace(currChar))
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
        /// Extracts key value pairs from yaml object
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <param name="startIndex">Start index if object</param>
        /// <returns></returns>
        private static Dictionary<string, YamlParserOptions> ExtractKeyValuePairsFromYamlObject(List<StringBuilder> lines, int startIndex, int endIndex)
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

                if (false == IsKeyLine(lines[i]))
                {
                    string key = ExtractKeyFromLine(lines[i]);
                    var helpObj = new YamlParserOptions(key, currLevel, i, i);

                    if (IsEmptyObject(lines[i]))
                    {
                        helpObj.SetIsYamlObject();
                        helpObj.IsEmptyObject = true;
                    }

                    if (IsEmptyList(lines[i]))
                    {
                        helpObj.SetIsYamlObject();
                        helpObj.SetIsList();
                        helpObj.IsEmptyList = true;
                    }
                   
                    result.Add(key, helpObj);
                }
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

        private static bool IsEmptyObject(StringBuilder line)
        {
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
                if (c == GeneralConstants.Quote)
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
        private static string? ExtractValueFromLine(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            StringBuilder keyBuilder = new();
            bool keyWasFound = false;

            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == GeneralConstants.Quote)
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
                else if (keyWasFound && c == GeneralConstants.N)
                {
                    if (i + 3 > line.Length - 1) throw new NotImplementedException();

                    i++;
                    if (line[i] != GeneralConstants.U) throw new NotImplementedException();
                    i++;
                    if (line[i] != GeneralConstants.L) throw new NotImplementedException();
                    i++;
                    if (line[i] != GeneralConstants.L) throw new NotImplementedException();
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
                if (c == GeneralConstants.WhiteSpace)
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
                if (c == GeneralConstants.WhiteSpace)
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

        private static bool IsPrimitiveList(StringBuilder line)
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
                else if (c == GeneralConstants.Quote)
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
                if (c == GeneralConstants.Quote)
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