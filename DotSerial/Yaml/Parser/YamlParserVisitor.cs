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
using DotSerial.Tree;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Yaml.Parser
{
    /// <summary>
    /// Implementation of the visitor for yaml parser.
    /// </summary>
    public class YamlParserVisitor : IYamlNodeParserVisitor
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <inheritdoc/>
        public static DSYamlNode Parse(string yamlString)
        {
            StringBuilder sb = new(yamlString);

            // Create help object, which contains every line of the yaml file
            var lines = new MultiLineStringBuilder(sb);

            // Remove start stop symbols
            lines = YamlParserHelper.RemoveStartStopSymbols(lines);

            IDSNode rootNode;

            // Check if its an empty yaml file
            if (0 == lines.Count)
            {
                rootNode = _nodeFactory.CreateNode(CommonConstants.MainObjectKey, null, NodeType.InnerNode);
                return new DSYamlNode(rootNode);   
            }

            if (YamlParserHelper.IsYamlSingleValue(lines))
            {
                rootNode = ParseMethods.ParsePrimitiveNode(lines.GetLine(0), 0, CommonConstants.MainObjectKey);
                return new DSYamlNode(rootNode);
            }
            else if (YamlParserHelper.IsYamlObject(lines))
            {
                rootNode = _nodeFactory.CreateNode(CommonConstants.MainObjectKey, null, NodeType.InnerNode);
                if (YamlParserHelper.IsEmptyObject(lines.GetLine(0)))
                {
                    return new DSYamlNode(rootNode);    
                }
            }
            else if (YamlParserHelper.IsYamlList(lines))
            {
                rootNode = _nodeFactory.CreateNode(CommonConstants.MainObjectKey, null, NodeType.ListNode);
                if (YamlParserHelper.IsEmptyList(lines.GetLine(0)))
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
                ParserAccept(rootNode, new YamlParserVisitor(), lines);
            }

            return new DSYamlNode(rootNode);
        }

        /// <summary>
        /// Parser for yaml
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="lines">MultiLineStringBuilder</param>    
        private static void ParserAccept(IDSNode node, YamlParserVisitor visitor, MultiLineStringBuilder lines)
        {
            if (node is LeafNode leafNode)
            {
                visitor.VisitLeafNode(leafNode, lines);    
            }
            else if (node is InnerNode innerNode)
            {
                visitor.VisitInnerNode(innerNode, lines);    
            }
            else if (node is ListNode listNode)
            {
                visitor.VisitListNode(listNode, lines);    
            }
            else if (node is DictionaryNode dicNode)
            {
                visitor.VisitDictionaryNode(dicNode, lines);
            }
            else
            {
                throw new DSYamlException("Parse: Unknown node type.");
            }   
        }             

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, MultiLineStringBuilder lines)
        {
            // Currenlty not needed
            throw new NotImplementedException();
        }        

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, MultiLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(lines);

            // Check if helpObj is yaml-Object
            if (YamlParserHelper.IsYamlObject(lines))
            {
                // Extract key, value pairs
                var dic = YamlParserHelper.ExtractKeyValuePairsFromYamlObject(lines);

                foreach (var keyValuePair in dic)
                {
                    // Convert key to int key
                    string key = keyValuePair.Key;
                    var value = keyValuePair.Value;

                    if (YamlParserHelper.IsYamlPrimitiveLine(value))
                    {
                        string? strValue = YamlParserHelper.ExtractValueFromLine(value.GetLine(0));
                        var childNode = _nodeFactory.CreateNode(key, strValue, NodeType.Leaf);
                        node.AddChild(childNode);
                    }
                    else if (YamlParserHelper.IsYamlObject(value))
                    {
                        // Create inner node
                        var innerNode = _nodeFactory.CreateNode(key, null, NodeType.InnerNode);

                        if (false == YamlParserHelper.IsEmptyObject(value.GetLine(0)))
                        {
                            // Parse inner node
                            ParserAccept(innerNode, new YamlParserVisitor(), value);
                        }

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (YamlParserHelper.IsYamlList(value))
                    {
                        // Create list node
                        var listNode = _nodeFactory.CreateNode(key, null, NodeType.ListNode);

                        if (false == YamlParserHelper.IsEmptyList(value.GetLine(0)))
                        {
                            // Parse list node
                            ParserAccept(listNode, new YamlParserVisitor(), value);
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
        public void VisitListNode(ListNode node, MultiLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(lines);

            if (YamlParserHelper.IsYamlList(lines))
            {
                // Extract object list
                var items2 = YamlParserHelper.ExtractObjectList(lines);
                int index = 0;
                foreach (var keyValuePair in items2)
                {
                    // Convert key to int key
                    string key = index.ToString();
                    var value = keyValuePair;

                    if (YamlParserHelper.IsYamlSingleValue(value))
                    {
                        var val = value.GetLine(0);
                        val = val.Trim();
                        var innerNode = ParseMethods.ParsePrimitiveNode(val, 0, key);
                        node.AddChild(innerNode);

                    }
                    else if (YamlParserHelper.IsYamlObject(value))
                    {       
                        // Create inner node
                        var innerNode = _nodeFactory.CreateNode(key, null, NodeType.InnerNode) as InnerNode ?? throw new NotImplementedException();

                        if (false == YamlParserHelper.IsEmptyObject(value.GetLine(0)))
                        {
                            // Parse inner node
                            ParserAccept(innerNode, new YamlParserVisitor(), value);
                        }

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }
                    else if (YamlParserHelper.IsYamlList(value))
                    {
                        // Create list node
                        var listNode = _nodeFactory.CreateNode(key, null, NodeType.ListNode);

                        if (false == YamlParserHelper.IsEmptyList(value.GetLine(0)))
                        {
                            // Parse list node
                            ParserAccept(listNode, new YamlParserVisitor(), value);
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
        public void VisitDictionaryNode(DictionaryNode node, MultiLineStringBuilder lines)
        {
            // Currenlty not needed
            throw new NotImplementedException();
        }        
                                
    }
}