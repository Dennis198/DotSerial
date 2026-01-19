using System.Text;
using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Nodes;

namespace DotSerial.XML.Parser
{
       /// <summary>
    /// Implementation of the visitor for xml parser.
    /// </summary>
    public class XmlParserVisitor : IXmlNodeParserVisitor
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <inheritdoc/>
        public static DSXmlNode Parse(string str)
        {
            // TODO remove whitespace

            StringBuilder sb = new(str);

            var rootTmp = XmlParserHelper.ExtractKeyValuePairsFromXmlObject(sb);

            if (rootTmp.Count != 1)
            {
                throw new DSXmlException("Parse: XML must have exactly one root element.");
            }

            var rootTagKeyPair = rootTmp.Keys.First();
            var rootValue = rootTmp.Values.First();

            IDSNode rootNode;

            rootNode = _nodeFactory.CreateNode(CommonConstants.MainObjectKey, null, NodeType.InnerNode);

            
            ParserAccept(rootNode, new XmlParserVisitor(), rootValue, rootTagKeyPair);

            return new DSXmlNode(rootNode);
        }

        /// <summary>
        /// Parser for json
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="sb">Stringbuilder</param>
        private static void ParserAccept(IDSNode node, XmlParserVisitor visitor, StringBuilder sb, XmlTagKeyPair tagKeyPair)
        {
            if (node is LeafNode leafNode)
            {
                visitor.VisitLeafNode(leafNode, sb, tagKeyPair);    
            }
            else if (node is InnerNode innerNode)
            {
                visitor.VisitInnerNode(innerNode, sb, tagKeyPair);    
            }
            else if (node is ListNode listNode)
            {
                visitor.VisitListNode(listNode, sb, tagKeyPair);    
            }
            else if (node is DictionaryNode dicNode)
            {
                visitor.VisitDictionaryNode(dicNode, sb, tagKeyPair);
            }
            else
            {
                throw new DSXmlException("Parse: Unknown node type.");
            }   
        }              

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, StringBuilder sb, XmlTagKeyPair tagKeyPair)
        {
            // Currenlty not needed
            // Will be procced in the VisitInnerNode
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, StringBuilder sb, XmlTagKeyPair tagKeyPair)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            if (tagKeyPair.IsXmlObject())
            {
                var dic = XmlParserHelper.ExtractKeyValuePairsFromXmlObject(sb);

                foreach(var keyValuepair in dic)
                {
                    // TODO process key value pair
                    string key = keyValuepair.Key.Key;
                    string tag = keyValuepair.Key.Tag;
                    StringBuilder? strValue = keyValuepair.Value;

                    if (keyValuepair.Key.IsXmlObject())
                    {
                        // Create inner node
                        var innerNode = _nodeFactory.CreateNode(key, null, NodeType.InnerNode);

                        if (null != strValue)
                        {
                            // Parse inner node
                            ParserAccept(innerNode, new XmlParserVisitor(), strValue, keyValuepair.Key);
                        }

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (keyValuepair.Key.IsXmlList())
                    {
                        // Create list node
                        var listNode = _nodeFactory.CreateNode(key, null, NodeType.ListNode);

                        if (null != strValue)
                        {
                            // Parse list node
                            ParserAccept(listNode, new XmlParserVisitor(), strValue, keyValuepair.Key);
                        }

                        // Add inner node to parent
                        node.AddChild(listNode); 
                    }
                    else if (keyValuepair.Key.IsXmlPrimitive())
                    {
                        string? leafValue = XmlParserHelper.ExtractNodeValue(strValue);
                        var childNode = _nodeFactory.CreateNode(key, leafValue, NodeType.Leaf);
                        node.AddChild(childNode);
                    }
                    else
                    {
                        throw new DSXmlException("Parse: String is not a xml object.");
                    }
                }
            }
            else
            {
                throw new DSXmlException("Parse: String is not a xml object.");
            }
        }

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, StringBuilder sb, XmlTagKeyPair tagKeyPair)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, XmlTagKeyPair tagKeyPair)
        {
            throw new NotImplementedException();
        }
    }
}