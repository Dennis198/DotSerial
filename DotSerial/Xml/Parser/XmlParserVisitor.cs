#region License
//Copyright (c) 2026 Dennis Sölch

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

using System.Text;
using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Xml.Parser
{
    /// <summary>
    /// Implementation of the visitor for xml parser.
    /// </summary>
    internal class XmlParserVisitor : IXmlNodeParserVisitor
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactoryObsolete _nodeFactory = NodeFactoryObsolete.Instance;

        /// <inheritdoc/>
        public static DSXmlNode Parse(string str)
        {
            // Remove xml declaration
            str = str.Replace(XmlConstants.XmlDeclaration, string.Empty);

            // Remove not needed whitespaces
            StringBuilder sb = XmlParserHelper.RemoveWhiteSpaceXmlString(str);

            var rootTmp = XmlParserHelper.ExtractKeyValuePairsFromXmlObject(sb);

            if (rootTmp.Count != 1)
            {
                throw new DSXmlException("Parse: Xml must have exactly one root element.");
            }

            var rootTagKeyPair = rootTmp.Keys.First();
            var rootValue = rootTmp.Values.First();

            IDSNode rootNode;

            if (rootTagKeyPair.IsXmlObject())
            {
                rootNode = _nodeFactory.CreateNode(CommonConstants.MainObjectKey, null, NodeType.InnerNode);                
            }
            else if (rootTagKeyPair.IsXmlList())
            {
                rootNode = _nodeFactory.CreateNode(CommonConstants.MainObjectKey, null, NodeType.ListNode);
            }
            else if (rootTagKeyPair.IsXmlPrimitive())
            {
                if(null == rootValue)
                {
                    throw new DSXmlException("Parse: String is not a xml object.");
                }

                rootNode = ParseMethods.ParsePrimitiveNode(rootValue, 0, CommonConstants.MainObjectKey);
                return new DSXmlNode(rootNode);
            }
            else
            {
                throw new DSXmlException("Parse: String is not a xml object.");
            }
            
            if(null != rootValue)
            {
                ParserAccept(rootNode, new XmlParserVisitor(), rootValue, rootTagKeyPair);
            }

            return new DSXmlNode(rootNode);
        }

        /// <summary>
        /// Parser for xml
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
                        if(null == strValue)
                        {
                            throw new DSXmlException("Parse: String is not a xml object.");
                        }
                        var childNode = ParseMethods.ParsePrimitiveNode(strValue, 0, key);
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
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            if (tagKeyPair.IsXmlList())
            {
                var dic = XmlParserHelper.ExtractKeyValuePairsFromXmlObject(sb);
                int i = 0;
                foreach(var keyValuepair in dic)                
                {
                    string key = i.ToString();
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
                        if(null == strValue)
                        {
                            throw new DSXmlException("Parse: String is not a xml object.");
                        }
                        var childNode = ParseMethods.ParsePrimitiveNode(strValue, 0, key);
                        node.AddChild(childNode);
                    }
                    else
                    {
                        throw new DSXmlException("Parse: String is not a xml object.");
                    }

                    i++;
                }
            }
            else
            {
                throw new DSXmlException("Parse: String is not a xml list.");
            }

        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, XmlTagKeyPair tagKeyPair)
        {
            // Currenlty not needed
            // Will be procced in the VisitInnerNode
            throw new NotImplementedException();
        }
    }
}