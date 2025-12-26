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
            sb.Remove(0, 4);

            var lines = CreateLines(sb);
            TrimLines(lines);

            var rootNode = _nodeFactory.CreateNode(GeneralConstants.MainObjectKey, null, NodeType.InnerNode);

            var options = new YamlParserOptions(GeneralConstants.MainObjectKey, 0, -1, lines.Count - 1);

            ParserAccept(rootNode, new YamlParserVisitor(), lines, options);

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
                var dic = ExtractKeyValuePairsFromYamlObject(lines, options.StartLineIndex + 1);

                foreach (var keyValuePair in dic)
                {
                    // Convert key to int key
                    string key = keyValuePair.Key;
                    var value = keyValuePair.Value;

                    // Check if value is yaml object or key value pair
                    if (value.IsYamlObject())
                    {

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
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, List<StringBuilder> lines, YamlParserOptions options)
        {
            throw new NotImplementedException();
        }

        // =========================================================================================


        /// <summary>
        /// Extracts key value pairs from yaml object
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <param name="startIndex">Start index if object</param>
        /// <returns></returns>
        private static Dictionary<string, YamlParserOptions> ExtractKeyValuePairsFromYamlObject(List<StringBuilder> lines, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (startIndex < 0 || lines.Count - 1 < startIndex)
            {
                throw new NotImplementedException();
            }

            var result = new Dictionary<string, YamlParserOptions>();
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
                    var helpObj = new YamlParserOptions(key, currLevel, i, i);
                   
                    result.Add(key, helpObj);
                }
                else
                {
                    string key = ExtractKeyFromLine(lines[i]);
                    int endIndex = GetEndIndexOfYamlObject(lines, i);
                    var helpObj = new YamlParserOptions(key, currLevel, i, endIndex);
                    result.Add(key, helpObj);
                    i = endIndex;
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