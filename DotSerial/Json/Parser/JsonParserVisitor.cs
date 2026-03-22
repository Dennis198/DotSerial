using DotSerial.Common;
using DotSerial.Common.Parser;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Json.Parser
{
    /// <summary>
    /// Implementation of the visitor for json parser.
    /// </summary>
    internal class JsonParserVisitor : IJsonNodeParserVisitor, IParserStrategy
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <inheritdoc/>
        public DSNode Parse(ReadOnlySpan<char> content)
        {
            IDSNode rootNode;
            var orgBookmark = new ParserBookmark(content, true);

            if (JsonParserHelper.IsStringJsonObject(orgBookmark, content))
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    SerializeStrategy.Json,
                    CommonConstants.MainObjectKey,
                    null,
                    TreeNodeType.InnerNode
                );
            }
            else if (JsonParserHelper.IsStringJsonList(orgBookmark, content))
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    SerializeStrategy.Json,
                    CommonConstants.MainObjectKey,
                    null,
                    TreeNodeType.ListNode
                );
            }
            else
            {
                rootNode = ParseMethods.ParsePrimitiveNode(
                    SerializeStrategy.Json,
                    content.Trim(),
                    0,
                    CommonConstants.MainObjectKey,
                    JsonConstants.ParseStopChars
                );
                return new DSNode(rootNode, SerializeStrategy.Json);
            }

            ParserAccept(rootNode, new JsonParserVisitor(), orgBookmark, content);

            return new DSNode(rootNode, SerializeStrategy.Json);
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, ParserBookmark bookmark, ReadOnlySpan<char> sb)
        {
            // Currenlty not needed
            // Will be procced in the VisitInnerNode
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, ParserBookmark bookmark, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (JsonParserHelper.IsStringJsonObject(bookmark, content))
            {
                // Extract key, value pairs
                var dic = JsonParserHelper.ExtractKeyValuePairsFromJsonObject(bookmark, content);

                foreach (var innerBookmark in dic)
                {
                    if (null == innerBookmark.Key)
                    {
                        throw new NotImplementedException();
                    }

                    string key = innerBookmark.Key;

                    if (innerBookmark.IsNull())
                    {
                        var childNode = _nodeFactory.CreateNodeFromString(
                            SerializeStrategy.Json,
                            key,
                            null,
                            TreeNodeType.Leaf
                        );
                        node.AddChild(childNode);
                    }
                    else if (JsonParserHelper.IsStringJsonObject(innerBookmark, content))
                    {
                        // Create inner node
                        var innerNode =
                            _nodeFactory.CreateNodeFromString(SerializeStrategy.Json, key, null, TreeNodeType.InnerNode)
                                as InnerNode
                            ?? throw new DSJsonException("Parse: Can't create inner node");

                        // Parse inner node
                        ParserAccept(innerNode, new JsonParserVisitor(), innerBookmark, content);

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (JsonParserHelper.IsStringJsonList(innerBookmark, content))
                    {
                        // Create list node
                        var listNode =
                            _nodeFactory.CreateNodeFromString(SerializeStrategy.Json, key, null, TreeNodeType.ListNode)
                                as ListNode
                            ?? throw new DSJsonException("Parse: Can't create list node");

                        // Parse list node
                        ParserAccept(listNode, new JsonParserVisitor(), innerBookmark, content);

                        node.AddChild(listNode);
                    }
                    else
                    {
                        var primContent = innerBookmark.GetContent(content);
                        var childNode = ParseMethods.ParsePrimitiveNode(SerializeStrategy.Json, primContent, 0, key);
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
        public void VisitLeafNode(LeafNode node, ParserBookmark bookmark, ReadOnlySpan<char> content)
        {
            // Currenlty not needed
            // Will be procced in the VisitInnerNode
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, ParserBookmark bookmark, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (JsonParserHelper.IsStringJsonList(bookmark, content))
            {
                // Extract object list
                var items = JsonParserHelper.ExtractObjectList(bookmark, content);

                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];

                    if (item.IsNull() || ReadOnlySpanMethods.EqualsNullString(content, item.Start))
                    {
                        var child = _nodeFactory.CreateNodeFromString(
                            SerializeStrategy.Json,
                            i.ToString(),
                            null,
                            TreeNodeType.Leaf
                        );
                        node.AddChild(child);
                    }
                    else if (JsonParserHelper.IsStringJsonObject(item, content))
                    {
                        // Create inner node
                        var innerNode =
                            _nodeFactory.CreateNodeFromString(
                                SerializeStrategy.Json,
                                i.ToString(),
                                null,
                                TreeNodeType.InnerNode
                            ) as InnerNode
                            ?? throw new DSJsonException("Parse: Can't create inner node");

                        // Parse inner node
                        ParserAccept(innerNode, new JsonParserVisitor(), item, content);

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (JsonParserHelper.IsStringJsonList(item, content))
                    {
                        // Create list node
                        var listNode =
                            _nodeFactory.CreateNodeFromString(
                                SerializeStrategy.Json,
                                i.ToString(),
                                null,
                                TreeNodeType.ListNode
                            ) as ListNode
                            ?? throw new DSJsonException("Parse: Can't create list node");

                        // Parse list node
                        ParserAccept(listNode, new JsonParserVisitor(), item, content);

                        node.AddChild(listNode);
                    }
                    else
                    {
                        var primContent = item.GetContent(content);
                        var childNode = ParseMethods.ParsePrimitiveNode(
                            SerializeStrategy.Json,
                            primContent,
                            0,
                            i.ToString()
                        );
                        node.AddChild(childNode);
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
        /// <param name="content">ReadOnlySpan</param>
        private static void ParserAccept(
            IDSNode node,
            JsonParserVisitor visitor,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content
        )
        {
            if (node is LeafNode leafNode)
            {
                visitor.VisitLeafNode(leafNode, bookmark, content);
            }
            else if (node is InnerNode innerNode)
            {
                visitor.VisitInnerNode(innerNode, bookmark, content);
            }
            else if (node is ListNode listNode)
            {
                visitor.VisitListNode(listNode, bookmark, content);
            }
            else if (node is DictionaryNode dicNode)
            {
                visitor.VisitDictionaryNode(dicNode, bookmark, content);
            }
            else
            {
                throw new DSJsonException("Parse: Unknown node type.");
            }
        }
    }
}
