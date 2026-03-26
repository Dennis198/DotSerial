using DotSerial.Common;
using DotSerial.Common.Parser;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Xml.Parser
{
    /// <summary>
    /// Implementation of the visitor for xml parser.
    /// </summary>
    internal class XmlParserVisitor : IXmlNodeParserVisitor, IParserStrategy
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <inheritdoc/>
        public DSNode Parse(ReadOnlySpan<char> content)
        {
            // Remove xml declaration

            // TODO Check if declaration exist
            var orgBookmark = new ParserBookmark(XmlConstants.XmlDeclaration.Length, content.Length - 1);

            var rootTmp = XmlParserHelper.ExtractKeyValuePairsFromXmlObject(orgBookmark, content);

            if (rootTmp.Count != 1)
            {
                throw new DotSerialException("Parse: Xml must have exactly one root element.");
            }

            var rootTagKeyPair = rootTmp.Keys.First();
            var rootBookmark = rootTmp.Values.First();

            IDSNode rootNode;

            if (rootTagKeyPair.IsXmlObject())
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    SerializeStrategy.Xml,
                    CommonConstants.MainObjectKey,
                    ParserBookmark.Empty,
                    [],
                    TreeNodeType.InnerNode,
                    null
                );
            }
            else if (rootTagKeyPair.IsXmlList())
            {
                rootNode = _nodeFactory.CreateNodeFromString(
                    SerializeStrategy.Xml,
                    CommonConstants.MainObjectKey,
                    ParserBookmark.Empty,
                    [],
                    TreeNodeType.ListNode,
                    null
                );
            }
            else if (rootTagKeyPair.IsXmlPrimitive())
            {
                rootNode = ParseMethods.ParsePrimitiveNode(
                    SerializeStrategy.Xml,
                    content,
                    rootBookmark,
                    CommonConstants.MainObjectKey,
                    null
                );

                return new DSNode(rootNode, SerializeStrategy.Xml);
            }
            else
            {
                ThrowHelper.ThrowGenericParserException("String is not a xml object.");
                throw new Exception("Unreachable code");
            }

            if (false == rootBookmark.IsNull())
            {
                ParserAccept(rootNode, new XmlParserVisitor(), rootTagKeyPair, rootBookmark, content);
            }

            return new DSNode(rootNode, SerializeStrategy.Xml);
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(
            DictionaryNode node,
            XmlTagKeyPair tagKeyPair,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content
        )
        {
            // Currenlty not needed
            // Will be procced in the VisitInnerNode
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitInnerNode(
            InnerNode node,
            XmlTagKeyPair tagKeyPair,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content
        )
        {
            ArgumentNullException.ThrowIfNull(node);

            if (tagKeyPair.IsXmlObject())
            {
                var dic = XmlParserHelper.ExtractKeyValuePairsFromXmlObject(bookmark, content);

                foreach (var keyValuepair in dic)
                {
                    string key = keyValuepair.Key.Key;
                    string tag = keyValuepair.Key.Tag;
                    var tmpBoomkark = keyValuepair.Value;

                    if (keyValuepair.Key.IsXmlObject())
                    {
                        // Create inner node
                        var innerNode = _nodeFactory.CreateNodeFromString(
                            SerializeStrategy.Xml,
                            key,
                            ParserBookmark.Empty,
                            [],
                            TreeNodeType.InnerNode,
                            node
                        );

                        if (false == tmpBoomkark.IsNull())
                        {
                            // Parse inner node
                            ParserAccept(innerNode, new XmlParserVisitor(), keyValuepair.Key, tmpBoomkark, content);
                        }

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (keyValuepair.Key.IsXmlList())
                    {
                        // Create list node
                        var listNode = _nodeFactory.CreateNodeFromString(
                            SerializeStrategy.Xml,
                            key,
                            ParserBookmark.Empty,
                            [],
                            TreeNodeType.ListNode,
                            node
                        );

                        if (false == tmpBoomkark.IsNull())
                        {
                            // Parse list node
                            ParserAccept(listNode, new XmlParserVisitor(), keyValuepair.Key, tmpBoomkark, content);
                        }

                        // Add inner node to parent
                        node.AddChild(listNode);
                    }
                    else if (keyValuepair.Key.IsXmlPrimitive())
                    {
                        var childNode = ParseMethods.ParsePrimitiveNode(
                            SerializeStrategy.Xml,
                            content,
                            tmpBoomkark,
                            key,
                            node
                        );
                        node.AddChild(childNode);
                    }
                    else
                    {
                        ThrowHelper.ThrowGenericParserException("String is not a xml object.");
                    }
                }
            }
            else
            {
                ThrowHelper.ThrowGenericParserException("String is not a xml object.");
            }
        }

        /// <inheritdoc/>
        public void VisitLeafNode(
            LeafNode node,
            XmlTagKeyPair tagKeyPair,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content
        )
        {
            // Currenlty not needed
            // Will be procced in the VisitInnerNode
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitListNode(
            ListNode node,
            XmlTagKeyPair tagKeyPair,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content
        )
        {
            ArgumentNullException.ThrowIfNull(node);

            if (tagKeyPair.IsXmlList())
            {
                var dic = XmlParserHelper.ExtractKeyValuePairsFromXmlObject(bookmark, content);
                int i = 0;
                foreach (var keyValuepair in dic)
                {
                    string key = i.ToString();
                    string tag = keyValuepair.Key.Tag;
                    var tmpBoomkark = keyValuepair.Value;

                    if (keyValuepair.Key.IsXmlObject())
                    {
                        // Create inner node
                        var innerNode = _nodeFactory.CreateNodeFromString(
                            SerializeStrategy.Xml,
                            key,
                            ParserBookmark.Empty,
                            [],
                            TreeNodeType.InnerNode,
                            node
                        );

                        if (false == tmpBoomkark.IsNull())
                        {
                            // Parse inner node
                            ParserAccept(innerNode, new XmlParserVisitor(), keyValuepair.Key, tmpBoomkark, content);
                        }

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (keyValuepair.Key.IsXmlList())
                    {
                        // Create list node
                        var listNode = _nodeFactory.CreateNodeFromString(
                            SerializeStrategy.Xml,
                            key,
                            ParserBookmark.Empty,
                            [],
                            TreeNodeType.ListNode,
                            node
                        );

                        if (false == tmpBoomkark.IsNull())
                        {
                            // Parse list node
                            ParserAccept(listNode, new XmlParserVisitor(), keyValuepair.Key, tmpBoomkark, content);
                        }

                        // Add inner node to parent
                        node.AddChild(listNode);
                    }
                    else if (keyValuepair.Key.IsXmlPrimitive())
                    {
                        var childNode = ParseMethods.ParsePrimitiveNode(
                            SerializeStrategy.Xml,
                            content,
                            tmpBoomkark,
                            key,
                            node
                        );
                        node.AddChild(childNode);
                    }
                    else
                    {
                        ThrowHelper.ThrowGenericParserException("String is not a xml object.");
                    }

                    i++;
                }
            }
            else
            {
                ThrowHelper.ThrowGenericParserException("String is not a xml list.");
            }
        }

        /// <summary>
        /// Parser for xml
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="tagKeyPair">XmlTagKeyPair</param>
        /// <param name="bookmark">Parser Bookmark</param>
        /// <param name="content">Xml content</param>
        private static void ParserAccept(
            IDSNode node,
            XmlParserVisitor visitor,
            XmlTagKeyPair tagKeyPair,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content
        )
        {
            if (node is LeafNode leafNode)
            {
                visitor.VisitLeafNode(leafNode, tagKeyPair, bookmark, content);
            }
            else if (node is InnerNode innerNode)
            {
                visitor.VisitInnerNode(innerNode, tagKeyPair, bookmark, content);
            }
            else if (node is ListNode listNode)
            {
                visitor.VisitListNode(listNode, tagKeyPair, bookmark, content);
            }
            else if (node is DictionaryNode dicNode)
            {
                visitor.VisitDictionaryNode(dicNode, tagKeyPair, bookmark, content);
            }
            else
            {
                ThrowHelper.ThrowUnknownNodeTypeException();
            }
        }
    }
}
