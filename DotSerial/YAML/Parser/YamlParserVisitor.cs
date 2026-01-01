using System.Text;
using DotSerial.Common;
using DotSerial.Utilities;
using DotSerial.Tree;
using DotSerial.Tree.Nodes;

namespace DotSerial.YAML.Parser
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

            var lines = YamlParserHelper.CreateLines(sb);
            YamlParserHelper.TrimLines(lines);

            var rootNode = _nodeFactory.CreateNode(CommonConstants.MainObjectKey, null, NodeType.InnerNode);

            var options = new YamlParserOptions(CommonConstants.MainObjectKey, 0, 0, lines.Count - 1);
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
                var dic = YamlParserHelper.ExtractKeyValuePairsFromYamlObject(lines, options.StartLineIndex, options.EndLineIndex);

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
                        string? strValue = YamlParserHelper.ExtractValueFromLine(lines[value.StartLineIndex]);
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
            if (true == YamlParserHelper.IsPrimitiveList(lines[options.StartLineIndex]))
            {
                // Extract primitive list
                var items = YamlParserHelper.ExtractPrimitiveList(lines, options.StartLineIndex, options.EndLineIndex);
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
                var items2 = YamlParserHelper.ExtractObjectList(lines,options.StartLineIndex, options.EndLineIndex);
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
                                
    }
}