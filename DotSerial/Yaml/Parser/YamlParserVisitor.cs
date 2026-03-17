using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Yaml.Parser
{
    /// <summary>
    /// Implementation of the visitor for yaml parser.
    /// </summary>
    internal class YamlParserVisitor : IYamlNodeParserVisitor
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <inheritdoc/>
        public static DSYamlNode Parse(ReadOnlySpan<char> content)
        {
            IDSNode rootNode;

            // TODO
            var trimedContent = content.Trim();

            // Create help object, which contains every line of the yaml file
            var lines = new MulitLineReadOnlySpan(trimedContent);

            // Remove start stop symbols
            lines = YamlParserHelper.RemoveStartStopSymbols(lines, trimedContent);

            // Check if its an empty yaml file
            if (0 == lines.Count)
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    StategyType.Yaml,
                    CommonConstants.MainObjectKey,
                    null,
                    NodeType.InnerNode
                );
                return new DSYamlNode(rootNode);
            }

            if (YamlParserHelper.IsYamlSingleValue(lines, trimedContent))
            {
                rootNode = ParseMethods.ParsePrimitiveNode2(
                    StategyType.Yaml,
                    lines.GetLineContent(0, trimedContent),
                    0,
                    CommonConstants.MainObjectKey
                );
                return new DSYamlNode(rootNode);
            }
            else if (YamlParserHelper.IsYamlObject(lines, trimedContent))
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    StategyType.Yaml,
                    CommonConstants.MainObjectKey,
                    null,
                    NodeType.InnerNode
                );
                if (YamlParserHelper.IsEmptyObject(lines.GetLineContent(0, trimedContent)))
                {
                    return new DSYamlNode(rootNode);
                }
            }
            else if (YamlParserHelper.IsYamlList(lines, trimedContent))
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    StategyType.Yaml,
                    CommonConstants.MainObjectKey,
                    null,
                    NodeType.ListNode
                );
                if (YamlParserHelper.IsEmptyList(lines.GetLineContent(0, trimedContent)))
                {
                    return new DSYamlNode(rootNode);
                }
            }
            else
            {
                throw new DSYamlException("Parse: String is not yaml.");
            }

            if (lines.Count > 0)
            {
                ParserAccept(rootNode, new YamlParserVisitor(), lines, trimedContent);
            }

            return new DSYamlNode(rootNode);
        }

        /// <summary>
        /// Parser for yaml
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <param name="content">Yaml content</param>
        private static void ParserAccept(
            IDSNode node,
            YamlParserVisitor visitor,
            MulitLineReadOnlySpan lines,
            ReadOnlySpan<char> content
        )
        {
            if (node is LeafNode leafNode)
            {
                visitor.VisitLeafNode(leafNode, lines, content);
            }
            else if (node is InnerNode innerNode)
            {
                visitor.VisitInnerNode(innerNode, lines, content);
            }
            else if (node is ListNode listNode)
            {
                visitor.VisitListNode(listNode, lines, content);
            }
            else if (node is DictionaryNode dicNode)
            {
                visitor.VisitDictionaryNode(dicNode, lines, content);
            }
            else
            {
                throw new DSYamlException("Parse: Unknown node type.");
            }
        }

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, MulitLineReadOnlySpan lines, ReadOnlySpan<char> content)
        {
            // Currenlty not needed
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, MulitLineReadOnlySpan lines, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(lines);

            // Check if helpObj is yaml-Object
            if (YamlParserHelper.IsYamlObject(lines, content))
            {
                // Extract key, value pairs
                var dic = YamlParserHelper.ExtractKeyValuePairsFromYamlObject(lines, content);

                foreach (var keyValuePair in dic)
                {
                    // Convert key to int key
                    string key = keyValuePair.Key;
                    var value = keyValuePair.Value;

                    if (YamlParserHelper.IsYamlPrimitiveLine(value, content))
                    {
                        string? strValue = YamlParserHelper.ExtractValueFromLine(value.GetLineContent(0, content));
                        var childNode = _nodeFactory.CreateNodeFromString(
                            StategyType.Yaml,
                            key,
                            strValue,
                            NodeType.Leaf
                        );
                        node.AddChild(childNode);
                    }
                    else if (YamlParserHelper.IsYamlObject(value, content))
                    {
                        // Create inner node
                        var innerNode = _nodeFactory.CreateNodeFromString(
                            StategyType.Yaml,
                            key,
                            null,
                            NodeType.InnerNode
                        );

                        if (false == YamlParserHelper.IsEmptyObject(value.GetLineContent(0, content)))
                        {
                            // Parse inner node
                            ParserAccept(innerNode, new YamlParserVisitor(), value, content);
                        }

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (YamlParserHelper.IsYamlList(value, content))
                    {
                        // Create list node
                        var listNode = _nodeFactory.CreateNodeFromString(
                            StategyType.Yaml,
                            key,
                            null,
                            NodeType.ListNode
                        );

                        if (false == YamlParserHelper.IsEmptyList(value.GetLineContent(0, content)))
                        {
                            // Parse list node
                            ParserAccept(listNode, new YamlParserVisitor(), value, content);
                        }

                        // Add inner node to parent
                        node.AddChild(listNode);
                    }
                    else
                    {
                        throw new DSYamlException("Parse: String is not a yaml object.");
                    }
                }
            }
            else
            {
                throw new DSYamlException("Parse: String is not a yaml object.");
            }
        }

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, MulitLineReadOnlySpan lines, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(lines);

            if (YamlParserHelper.IsYamlList(lines, content))
            {
                // Extract object list
                var items2 = YamlParserHelper.ExtractObjectList(lines, content);
                int index = 0;
                foreach (var keyValuePair in items2)
                {
                    // Convert key to int key
                    string key = index.ToString();
                    var value = keyValuePair;

                    if (YamlParserHelper.IsYamlSingleValue(value, content))
                    {
                        var val = value.GetLineContent(0, content);
                        // Remove starting whitespaces
                        val = val.Trim();
                        var innerNode = ParseMethods.ParsePrimitiveNode2(StategyType.Yaml, val, 0, key);
                        node.AddChild(innerNode);
                    }
                    else if (YamlParserHelper.IsYamlObject(value, content))
                    {
                        // Create inner node
                        var innerNode =
                            _nodeFactory.CreateNodeFromString(StategyType.Yaml, key, null, NodeType.InnerNode)
                                as InnerNode
                            ?? throw new NotImplementedException();

                        if (false == YamlParserHelper.IsEmptyObject(value.GetLineContent(0, content)))
                        {
                            // Parse inner node
                            ParserAccept(innerNode, new YamlParserVisitor(), value, content);
                        }

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (YamlParserHelper.IsYamlList(value, content))
                    {
                        // Create list node
                        var listNode = _nodeFactory.CreateNodeFromString(
                            StategyType.Yaml,
                            key,
                            null,
                            NodeType.ListNode
                        );

                        if (false == YamlParserHelper.IsEmptyList(value.GetLineContent(0, content)))
                        {
                            // Parse list node
                            ParserAccept(listNode, new YamlParserVisitor(), value, content);
                        }

                        // Add inner node to parent
                        node.AddChild(listNode);
                    }
                    else
                    {
                        throw new DSYamlException("Parse: String is not a yaml object.");
                    }

                    index++;
                }
            }
            else
            {
                throw new DSYamlException("Parse: String is not a yaml list.");
            }
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, MulitLineReadOnlySpan lines, ReadOnlySpan<char> content)
        {
            // Currenlty not needed
            throw new NotImplementedException();
        }
    }
}
