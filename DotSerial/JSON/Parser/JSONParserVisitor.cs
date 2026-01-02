#region License
//Copyright (c) 2025 Dennis Sölch

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
using DotSerial.Utilities;
using DotSerial.Tree;
using DotSerial.Tree.Nodes;
using DotSerial.Json;

namespace DotSerial.JSON.Parser
{
    /// <summary>
    /// Implementation of the visitor for json parser.
    /// </summary>
    public class JSONParserVisitor : IJsonNodeParserVisitor
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <inheritdoc/>
        public static DSJsonNode Parse(string jsonString)
        {
            // Removes all whitespaces
            string tmp = ParseMethods.RemoveWhiteSpace(jsonString);

            StringBuilder sb = new(tmp);
            var rootNode = _nodeFactory.CreateNode(CommonConstants.MainObjectKey, null, NodeType.InnerNode);

            ParserAccept(rootNode, new JSONParserVisitor(), sb);

            return new DSJsonNode(rootNode);
        }

        /// <summary>
        /// Parser for json
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="sb">Stringbuilder</param>
        private static void ParserAccept(IDSNode node, JSONParserVisitor visitor, StringBuilder sb)
        {
            if (node is LeafNode leafNode)
            {
                visitor.VisitLeafNode(leafNode, sb);    
            }
            else if (node is InnerNode innerNode)
            {
                visitor.VisitInnerNode(innerNode, sb);    
            }
            else if (node is ListNode listNode)
            {
                visitor.VisitListNode(listNode, sb);    
            }
            else if (node is DictionaryNode dicNode)
            {
                visitor.VisitDictionaryNode(dicNode, sb);
            }
            else
            {
                throw new NotImplementedException();
            }   
        }            

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, StringBuilder sb)
        {
            // Currenlty not needed
            // Will be procced in the VisitInnerNode
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            if (JsonParserHelper.IsStringJsonObject(sb))
            {
                // Extract key, value pairs
                var dic = JsonParserHelper.ExtractKeyValuePairsFromJsonObject(sb);

                foreach(var keyValuepair in dic)
                {
                     // Convert key to int key
                    string key = keyValuepair.Key;
                    StringBuilder? strValue = keyValuepair.Value;

                    if (null == strValue)
                    {
                        var childNode = _nodeFactory.CreateNode(key, null, NodeType.Leaf);
                        node.AddChild(childNode);
                    }
                    else if (JsonParserHelper.IsStringJsonObject(strValue))
                    {
                        // Create inner node
                        var innerNode = _nodeFactory.CreateNode(key, null, NodeType.InnerNode) as InnerNode ?? throw new DSJsonException("Parse: Can't create inner node");

                        // Create stringbuilder for inner content
                        StringBuilder innerSb = strValue;

                        // Parse inner node
                        ParserAccept(innerNode, new JSONParserVisitor(), innerSb);

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (JsonParserHelper.IsStringJsonList(strValue))
                    {
                        // Create list node
                        var listNode = _nodeFactory.CreateNode(key, null, NodeType.ListNode) as ListNode ?? throw new DSJsonException("Parse: Can't create list node");

                        // Create stringbuilder for list content
                        StringBuilder listSb = strValue;

                        // Parse list node
                        ParserAccept(listNode, new JSONParserVisitor(), listSb);

                        node.AddChild(listNode);
                    }
                    else
                    {
                        string leafValue = strValue.ToString();
                        var childNode = _nodeFactory.CreateNode(key, leafValue, NodeType.Leaf);
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
        public void VisitListNode(ListNode node, StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            if (JsonParserHelper.IsStringJsonList(sb))
            {
                // Check if list is list of primitive or objects
                if (sb[1] == CommonConstants.Quote || sb[1] == CommonConstants.N)
                {
                    // Extract primitive list
                    var items = JsonParserHelper.ExtractPrimitiveList(sb);
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
                    var items = JsonParserHelper.ExtractObjectList(sb);
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (JsonParserHelper.IsStringJsonObject(items[i]))
                        {
                            // Create inner node
                            var innerNode = _nodeFactory.CreateNode(i.ToString(), null, NodeType.InnerNode) as InnerNode ?? throw new DSJsonException("Parse: Can't create inner node");

                            // Create stringbuilder for inner content
                            StringBuilder innerSb = items[i];

                            // Parse inner node
                            ParserAccept(innerNode, new JSONParserVisitor(), innerSb);

                            // Add inner node to parent
                            node.AddChild(innerNode);
                        }
                        else if (JsonParserHelper.IsStringJsonList(items[i]))
                        {
                            // Create list node
                            var listNode = _nodeFactory.CreateNode(i.ToString(), null, NodeType.ListNode) as ListNode ?? throw new DSJsonException("Parse: Can't create list node");

                            // Create stringbuilder for list content
                            StringBuilder listSb = items[i];

                            // Parse list node
                            ParserAccept(listNode, new JSONParserVisitor(), listSb);

                            node.AddChild(listNode);
                        }
                        else
                        {
                            throw new DSJsonException("Parse: String is not a json object or list.");
                        }
                    }
                }
            }
            else
            {
                throw new DSJsonException("Parse: String is not a json list.");
            }
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb)
        {
            // Currenlty not needed
            // Will be procced in the VisitInnerNode
            throw new NotImplementedException();
        }        

    }
}