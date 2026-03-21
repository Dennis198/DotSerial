using DotSerial.Common;
using DotSerial.Common.Parser;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Toon.Parser
{
    /// <summary>
    /// Implementation of the visitor for toon parser.
    /// </summary>
    internal class ToonParserVisitor : IToonNodeParserVisitor, IParserStrategy
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        public DSNode Parse2(ReadOnlySpan<char> content)
        {
            IDSNode rootNode;

            if (content.IsEmpty)
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    StategyType.Toon,
                    CommonConstants.MainObjectKey,
                    null,
                    NodeType.InnerNode
                );
                return new DSNode(rootNode);
            }

            // Create help object, which contains every line of the yaml file
            var lines = new MulitLineParserBookmark(content);

            if (ToonParserHelper.IsToonSingleValue(lines, content))
            {
                rootNode = ParseMethods.ParsePrimitiveNode(
                    StategyType.Toon,
                    lines.GetLineContent(0, content),
                    0,
                    CommonConstants.MainObjectKey
                );
                return new DSNode(rootNode);
            }
            else if (ToonParserHelper.IsToonObject(lines, content, true))
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    StategyType.Toon,
                    CommonConstants.MainObjectKey,
                    null,
                    NodeType.InnerNode
                );
                if (ToonParserHelper.IsEmptyObject(lines, content))
                {
                    return new DSNode(rootNode);
                }
            }
            else if (ToonParserHelper.IsToonList(lines, content, true))
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    StategyType.Toon,
                    CommonConstants.MainObjectKey,
                    null,
                    NodeType.ListNode
                );
                if (ToonParserHelper.IsEmptyList(lines.GetLineContent(0, content)))
                {
                    return new DSNode(rootNode);
                }
            }
            else
            {
                throw new DSToonException("Parse: String is not toon.");
            }

            if (lines.Count > 0)
            {
                ParserAccept(rootNode, new ToonParserVisitor(), lines, content, true);
            }

            return new DSNode(rootNode);
        }

        /// <inheritdoc/>
        public static DSToonNode Parse(ReadOnlySpan<char> content)
        {
            IDSNode rootNode;

            if (content.IsEmpty)
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    StategyType.Toon,
                    CommonConstants.MainObjectKey,
                    null,
                    NodeType.InnerNode
                );
                return new DSToonNode(rootNode);
            }

            // Create help object, which contains every line of the yaml file
            var lines = new MulitLineParserBookmark(content);

            if (ToonParserHelper.IsToonSingleValue(lines, content))
            {
                rootNode = ParseMethods.ParsePrimitiveNode(
                    StategyType.Toon,
                    lines.GetLineContent(0, content),
                    0,
                    CommonConstants.MainObjectKey
                );
                return new DSToonNode(rootNode);
            }
            else if (ToonParserHelper.IsToonObject(lines, content, true))
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    StategyType.Toon,
                    CommonConstants.MainObjectKey,
                    null,
                    NodeType.InnerNode
                );
                if (ToonParserHelper.IsEmptyObject(lines, content))
                {
                    return new DSToonNode(rootNode);
                }
            }
            else if (ToonParserHelper.IsToonList(lines, content, true))
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    StategyType.Toon,
                    CommonConstants.MainObjectKey,
                    null,
                    NodeType.ListNode
                );
                if (ToonParserHelper.IsEmptyList(lines.GetLineContent(0, content)))
                {
                    return new DSToonNode(rootNode);
                }
            }
            else
            {
                throw new DSToonException("Parse: String is not toon.");
            }

            if (lines.Count > 0)
            {
                ParserAccept(rootNode, new ToonParserVisitor(), lines, content, true);
            }

            return new DSToonNode(rootNode);
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(
            DictionaryNode node,
            MulitLineParserBookmark lines,
            ReadOnlySpan<char> content,
            bool isRootElement = false
        )
        {
            // Currenlty not needed
            // Will be procced in the VisitInnerNode
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitInnerNode(
            InnerNode node,
            MulitLineParserBookmark lines,
            ReadOnlySpan<char> content,
            bool isRootElement = false
        )
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(lines);

            if (ToonParserHelper.IsToonObject(lines, content, isRootElement))
            {
                // Extract key, value pairs
                var dic = ToonParserHelper.ExtractKeyValuePairsFromToonObject(lines, content);

                foreach (var keyValuePair in dic)
                {
                    // Convert key to int key
                    string key = keyValuePair.Key;
                    var value = keyValuePair.Value;

                    if (ToonParserHelper.IsToonPrimitiveLine(value, content))
                    {
                        string? strValue = ToonParserHelper.ExtractValueFromLine(value.GetLineContent(0, content));
                        var childNode = _nodeFactory.CreateNodeFromString(
                            StategyType.Toon,
                            key,
                            strValue,
                            NodeType.Leaf
                        );
                        node.AddChild(childNode);
                    }
                    else if (ToonParserHelper.IsToonObject(value, content))
                    {
                        // Create inner node
                        var innerNode = _nodeFactory.CreateNodeFromString(
                            StategyType.Toon,
                            key,
                            null,
                            NodeType.InnerNode
                        );

                        if (false == ToonParserHelper.IsEmptyObject(value, content))
                        {
                            // Parse inner node
                            ParserAccept(innerNode, new ToonParserVisitor(), value, content);
                        }

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (ToonParserHelper.IsToonList(value, content))
                    {
                        // Create list node
                        var listNode = _nodeFactory.CreateNodeFromString(
                            StategyType.Toon,
                            key,
                            null,
                            NodeType.ListNode
                        );

                        if (false == ToonParserHelper.IsEmptyList(value.GetLineContent(0, content)))
                        {
                            // Parse list node
                            ParserAccept(listNode, new ToonParserVisitor(), value, content);
                        }

                        // Add inner node to parent
                        node.AddChild(listNode);
                    }
                    else
                    {
                        throw new DSToonException("Parse: String is not a toon object.");
                    }
                }
            }
            else
            {
                throw new DSToonException("Parse: String is not a toon object.");
            }
        }

        /// <inheritdoc/>
        public void VisitLeafNode(
            LeafNode node,
            MulitLineParserBookmark sb,
            ReadOnlySpan<char> content,
            bool isRootElement = false
        )
        {
            // Currenlty not needed
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitListNode(
            ListNode node,
            MulitLineParserBookmark lines,
            ReadOnlySpan<char> content,
            bool isRootElement = false
        )
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(lines);

            if (ToonParserHelper.IsToonList(lines, content, isRootElement))
            {
                // Check if list have a key line from parsing
                if (false == string.IsNullOrWhiteSpace(lines.KeyLine))
                {
                    // Check if list uses toon schema
                    if (ToonParserHelper.IsSchemaList(lines))
                    {
                        ToonParserHelper.ParseSchemaList(node, lines, content);
                    }
                    else
                    {
                        // Extract object list
                        var items = ToonParserHelper.ExtractObjectList(lines, content);
                        int index = 0;
                        foreach (var keyValuePair in items)
                        {
                            // Convert key to int key
                            string key = index.ToString();
                            var value = keyValuePair;

                            if (ToonParserHelper.IsToonPrimitiveLine(value, content))
                            {
                                string? strValue = ToonParserHelper.ExtractValueFromLine(
                                    value.GetLineContent(0, content)
                                );
                                var childNode = _nodeFactory.CreateNodeFromString(
                                    StategyType.Toon,
                                    key,
                                    strValue,
                                    NodeType.Leaf
                                );

                                node.AddChild(childNode);
                            }
                            else if (ToonParserHelper.IsToonSingleValue(value, content))
                            {
                                var tmp = value.GetLineContent(0, content).Trim();
                                var childNode = ParseMethods.ParsePrimitiveNode(StategyType.Toon, tmp, 0, key);
                                node.AddChild(childNode);
                            }
                            else if (ToonParserHelper.IsToonObject(value, content))
                            {
                                // Create inner node
                                var innerNode = _nodeFactory.CreateNodeFromString(
                                    StategyType.Toon,
                                    key,
                                    null,
                                    NodeType.InnerNode
                                );

                                if (false == ToonParserHelper.IsEmptyObject(value, content))
                                {
                                    // Parse inner node
                                    ParserAccept(innerNode, new ToonParserVisitor(), value, content);
                                }

                                // Add inner node to parent
                                node.AddChild(innerNode);
                            }
                            else if (ToonParserHelper.IsToonList(value, content))
                            {
                                // Create list node
                                var listNode = _nodeFactory.CreateNodeFromString(
                                    StategyType.Toon,
                                    key,
                                    null,
                                    NodeType.ListNode
                                );

                                if (false == ToonParserHelper.IsEmptyList(value.GetLineContent(0, content)))
                                {
                                    // Parse list node
                                    ParserAccept(listNode, new ToonParserVisitor(), value, content);
                                }

                                // Add inner node to parent
                                node.AddChild(listNode);
                            }
                            else
                            {
                                throw new DSToonException("Parse: String is not a toon object.");
                            }
                            index++;
                        }
                    }
                }
                else
                {
                    if (ToonParserHelper.IsPrimitiveList(lines, content))
                    {
                        ToonParserHelper.ParsePrimitiveList(node, lines.GetLineContent(0, content));
                    }
                    else
                    {
                        throw new DSToonException("Parse: String is not a toon object.");
                    }
                }
            }
            else
            {
                throw new DSToonException("Parse: String is not a toon list.");
            }
        }

        /// <summary>
        /// Parser for toon
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <param name="content">Toon content</param>
        /// <param name="isRootElement">True, if root element</param>
        private static void ParserAccept(
            IDSNode node,
            ToonParserVisitor visitor,
            MulitLineParserBookmark lines,
            ReadOnlySpan<char> content,
            bool isRootElement = false
        )
        {
            if (node is LeafNode leafNode)
            {
                visitor.VisitLeafNode(leafNode, lines, content, isRootElement);
            }
            else if (node is InnerNode innerNode)
            {
                visitor.VisitInnerNode(innerNode, lines, content, isRootElement);
            }
            else if (node is ListNode listNode)
            {
                visitor.VisitListNode(listNode, lines, content, isRootElement);
            }
            else if (node is DictionaryNode dicNode)
            {
                visitor.VisitDictionaryNode(dicNode, lines, content, isRootElement);
            }
            else
            {
                throw new DSToonException("Parse: Unknown node type.");
            }
        }
    }
}
