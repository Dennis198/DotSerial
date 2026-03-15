using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Json.Parser
{
    /// <summary>
    /// Implementation of the visitor for json parser.
    /// </summary>
    internal class JsonParserVisitor : IJsonNodeParserVisitor
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <inheritdoc/>
        public static DSJsonNode Parse(ReadOnlySpan<char> content)
        {
            IDSNode rootNode;

            // TODO
            var trimedContent = content.Trim();

            if (JsonParserHelper.IsStringJsonObject(trimedContent))
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    StategyType.Json,
                    CommonConstants.MainObjectKey,
                    null,
                    NodeType.InnerNode
                );
            }
            else if (JsonParserHelper.IsStringJsonList(trimedContent))
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    StategyType.Json,
                    CommonConstants.MainObjectKey,
                    null,
                    NodeType.ListNode
                );
            }
            else
            {
                rootNode = ParseMethods.ParsePrimitiveNode2(
                    StategyType.Json,
                    trimedContent,
                    0,
                    CommonConstants.MainObjectKey,
                    JsonConstants.ParseStopChars
                );
                return new DSJsonNode(rootNode);
            }

            ParserAccept(rootNode, new JsonParserVisitor(), trimedContent);

            return new DSJsonNode(rootNode);
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, ReadOnlySpan<char> sb)
        {
            // Currenlty not needed
            // Will be procced in the VisitInnerNode
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (JsonParserHelper.IsStringJsonObject(content))
            {
                // Extract key, value pairs
                var dic = JsonParserHelper.ExtractKeyValuePairsFromJsonObject(content);

                foreach (var bookmark in dic)
                {
                    if (null == bookmark.Key)
                    {
                        throw new NotImplementedException();
                    }

                    string key = bookmark.Key;
                    ReadOnlySpan<char> strValue = bookmark.IsNull()
                        ? null
                        : content.Slice(bookmark.Start, bookmark.End - bookmark.Start + 1);

                    if (null == strValue)
                    {
                        var childNode = _nodeFactory.CreateNodeFromString(StategyType.Json, key, null, NodeType.Leaf);
                        node.AddChild(childNode);
                    }
                    else if (JsonParserHelper.IsStringJsonObject(strValue))
                    {
                        // Create inner node
                        var innerNode =
                            _nodeFactory.CreateNodeFromString(StategyType.Json, key, null, NodeType.InnerNode)
                                as InnerNode
                            ?? throw new DSJsonException("Parse: Can't create inner node");

                        // Parse inner node
                        ParserAccept(innerNode, new JsonParserVisitor(), strValue);

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (JsonParserHelper.IsStringJsonList(strValue))
                    {
                        // Create list node
                        var listNode =
                            _nodeFactory.CreateNodeFromString(StategyType.Json, key, null, NodeType.ListNode)
                                as ListNode
                            ?? throw new DSJsonException("Parse: Can't create list node");

                        // Parse list node
                        ParserAccept(listNode, new JsonParserVisitor(), strValue);

                        node.AddChild(listNode);
                    }
                    else
                    {
                        string leafValue = strValue.ToString();
                        var childNode = _nodeFactory.CreateNodeFromString(
                            StategyType.Json,
                            key,
                            leafValue,
                            NodeType.Leaf
                        );
                        node.AddChild(childNode);
                    }
                }
            }
            else
            {
                throw new DSJsonException("Parse: String is not a json object.");
            }
        }

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, ReadOnlySpan<char> content)
        {
            // Currenlty not needed
            // Will be procced in the VisitInnerNode
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (JsonParserHelper.IsStringJsonList(content))
            {
                // Extract object list
                var items = JsonParserHelper.ExtractObjectList(content);

                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    ReadOnlySpan<char> strValue = item.IsNull()
                        ? null
                        : content.Slice(item.Start, item.End - item.Start + 1);
                    strValue = strValue.Trim();

                    if (item.IsNull() || ReadOnlySpanMethods.EqualsNullString(strValue))
                    {
                        var child = _nodeFactory.CreateNodeFromString(
                            StategyType.Json,
                            i.ToString(),
                            null,
                            NodeType.Leaf
                        );
                        node.AddChild(child);
                    }
                    else if (JsonParserHelper.IsStringJsonObject(strValue))
                    {
                        // Create inner node
                        var innerNode =
                            _nodeFactory.CreateNodeFromString(StategyType.Json, i.ToString(), null, NodeType.InnerNode)
                                as InnerNode
                            ?? throw new DSJsonException("Parse: Can't create inner node");

                        // Parse inner node
                        ParserAccept(innerNode, new JsonParserVisitor(), strValue);

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (JsonParserHelper.IsStringJsonList(strValue))
                    {
                        // Create list node
                        var listNode =
                            _nodeFactory.CreateNodeFromString(StategyType.Json, i.ToString(), null, NodeType.ListNode)
                                as ListNode
                            ?? throw new DSJsonException("Parse: Can't create list node");

                        // Parse list node
                        ParserAccept(listNode, new JsonParserVisitor(), strValue);

                        node.AddChild(listNode);
                    }
                    else
                    {
                        string? value = strValue.ToString();
                        var child = _nodeFactory.CreateNodeFromString(
                            StategyType.Json,
                            i.ToString(),
                            value,
                            NodeType.Leaf
                        );
                        node.AddChild(child);
                    }
                }
            }
            else
            {
                throw new DSJsonException("Parse: String is not a json list.");
            }
        }

        /// <summary>
        /// Parser for json
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="content">Stringbuilder</param>
        private static void ParserAccept(IDSNode node, JsonParserVisitor visitor, ReadOnlySpan<char> content)
        {
            if (node is LeafNode leafNode)
            {
                visitor.VisitLeafNode(leafNode, content);
            }
            else if (node is InnerNode innerNode)
            {
                visitor.VisitInnerNode(innerNode, content);
            }
            else if (node is ListNode listNode)
            {
                visitor.VisitListNode(listNode, content);
            }
            else if (node is DictionaryNode dicNode)
            {
                visitor.VisitDictionaryNode(dicNode, content);
            }
            else
            {
                throw new DSJsonException("Parse: Unknown node type.");
            }
        }
    }
}
