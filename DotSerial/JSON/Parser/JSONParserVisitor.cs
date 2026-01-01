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
using DotSerial.Core.Exceptions.JSON;
using DotSerial.Core.General;
using DotSerial.Core.Misc;
using DotSerial.Core.Tree;
using DotSerial.Core.Tree.Nodes;

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

            var rootNode = _nodeFactory.CreateNode(GeneralConstants.MainObjectKey, null, NodeType.InnerNode);

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
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            if (JsonParserHelper.IsStringJsonObject(sb.ToString()))
            {
                // Extract key, value pairs
                var dic = JsonParserHelper.ExtractKeyValuePairsFromJsonObject(sb);

                foreach(var keyValuepair in dic)
                {
                     // Convert key to int key
                    string key = keyValuepair.Key;
                    string? strValue = keyValuepair.Value;

                    if (null == strValue)
                    {
                        var childNode = _nodeFactory.CreateNode(key, null, NodeType.Leaf);
                        node.AddChild(childNode);
                    }
                    else if (JsonParserHelper.IsStringJsonObject(strValue))
                    {
                        // Create inner node
                        var innerNode = _nodeFactory.CreateNode(key, null, NodeType.InnerNode) as InnerNode ?? throw new DSInvalidJSONException(sb.ToString());

                        // Create stringbuilder for inner content
                        StringBuilder innerSb = new (strValue);

                        // Parse inner node
                        ParserAccept(innerNode, new JSONParserVisitor(), innerSb);

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (JsonParserHelper.IsStringJsonList(strValue))
                    {
                        // Create list node
                        if (_nodeFactory.CreateNode(key, null, NodeType.ListNode) is not ListNode listNode)
                        {
                            throw new DSInvalidJSONException(sb.ToString());
                        }

                        // Create stringbuilder for list content
                        StringBuilder listSb = new (strValue);

                        // Parse list node
                        ParserAccept(listNode, new JSONParserVisitor(), listSb);

                        node.AddChild(listNode);
                    }
                    else
                    {
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
        public void VisitListNode(ListNode node, StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            // Implementation for parsing list node from JSON can be added here
            // This is a placeholder for demonstration purposes

            if (JsonParserHelper.IsStringJsonList(sb.ToString()))
            {
                // Check if list is list of primitive or objects
                if (sb[1] == GeneralConstants.Quote || sb[1] == GeneralConstants.N)
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
                            var innerNode = _nodeFactory.CreateNode(i.ToString(), null, NodeType.InnerNode) as InnerNode ?? throw new DSInvalidJSONException(sb.ToString());

                            // Create stringbuilder for inner content
                            StringBuilder innerSb = new (items[i]);

                            // Parse inner node
                            ParserAccept(innerNode, new JSONParserVisitor(), innerSb);

                            // Add inner node to parent
                            node.AddChild(innerNode);
                        }
                        else if (JsonParserHelper.IsStringJsonList(items[i]))
                        {
                            // Create list node
                            if (_nodeFactory.CreateNode(i.ToString(), null, NodeType.ListNode) is not ListNode listNode)
                            {
                                throw new DSInvalidJSONException(sb.ToString());
                            }

                            // Create stringbuilder for list content
                            StringBuilder listSb = new (items[i]);

                            // Parse list node
                            ParserAccept(listNode, new JSONParserVisitor(), listSb);

                            node.AddChild(listNode);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb)
        {
            // Currenlty not needed
            throw new NotImplementedException();
        }        

    }
}