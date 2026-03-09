using System.Text;
using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Toon.Parser
{
    /// <summary>
    /// Implementation of the visitor for toon parser.
    /// </summary>
    internal class ToonParserVisitor : IToonNodeParserVisitor
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <inheritdoc/>
        public static DSToonNode Parse(string str)
        {
            StringBuilder sb = new(str);

            // Create help object, which contains every line of the toon file
            var lines = new ToonMulitLineStringBuilder(sb);

            IDSNode rootNode;

            // Check if its an empty yaml file
            if (0 == lines.Count)
            {
                rootNode = _nodeFactory.CreateNodeFromString(StategyType.Toon, CommonConstants.MainObjectKey, null, NodeType.InnerNode);
                return new DSToonNode(rootNode);   
            }

            if (ToonParserHelper.IsToonSingleValue(lines))
            {
                rootNode = ParseMethods.ParsePrimitiveNode(StategyType.Toon, lines.GetLine(0), 0, CommonConstants.MainObjectKey);
                return new DSToonNode(rootNode);
            }
            else if (ToonParserHelper.IsToonObject(lines, true))
            {
                rootNode = _nodeFactory.CreateNodeFromString(StategyType.Toon, CommonConstants.MainObjectKey, null, NodeType.InnerNode);
                if (ToonParserHelper.IsEmptyObject(lines))
                {
                    return new DSToonNode(rootNode);    
                }                
            } 
            else if (ToonParserHelper.IsToonList(lines, true))
            {
                rootNode = _nodeFactory.CreateNodeFromString(StategyType.Toon, CommonConstants.MainObjectKey, null, NodeType.ListNode);
                if (ToonParserHelper.IsEmptyList(lines.GetLine(0)))
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
                ParserAccept(rootNode, new ToonParserVisitor(), lines, true);
            }

            return new DSToonNode(rootNode);
        }

        /// <summary>
        /// Parser for toon
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="lines">ToonMulitLineStringBuilder</param>    
        private static void ParserAccept(IDSNode node, ToonParserVisitor visitor, ToonMulitLineStringBuilder lines, bool isRootElement = false)
        {
            if (node is LeafNode leafNode)
            {
                visitor.VisitLeafNode(leafNode, lines, isRootElement);    
            }
            else if (node is InnerNode innerNode)
            {
                visitor.VisitInnerNode(innerNode, lines, isRootElement);    
            }
            else if (node is ListNode listNode)
            {
                visitor.VisitListNode(listNode, lines, isRootElement);    
            }
            else if (node is DictionaryNode dicNode)
            {
                visitor.VisitDictionaryNode(dicNode, lines, isRootElement);
            }
            else
            {
                throw new DSToonException("Parse: Unknown node type.");
            }   
        }          

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, ToonMulitLineStringBuilder lines, bool isRootElement = false)
        {
            // Currenlty not needed
            throw new NotImplementedException();
        }     

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, ToonMulitLineStringBuilder lines, bool isRootElement = false)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(lines);

            if (ToonParserHelper.IsToonObject(lines, isRootElement))
            {
                // Extract key, value pairs
                var dic = ToonParserHelper.ExtractKeyValuePairsFromToonObject(lines);

                foreach (var keyValuePair in dic)
                {
                    // Convert key to int key
                    string key = keyValuePair.Key;
                    var value = keyValuePair.Value;

                    if (ToonParserHelper.IsToonPrimitiveLine(value))
                    {
                        string? strValue = ToonParserHelper.ExtractValueFromLine(value.GetLine(0));
                        var childNode = _nodeFactory.CreateNodeFromString(StategyType.Toon, key, strValue, NodeType.Leaf);
                        node.AddChild(childNode);
                    }
                    else if (ToonParserHelper.IsToonObject(value))
                    {
                        // Create inner node
                        var innerNode = _nodeFactory.CreateNodeFromString(StategyType.Toon, key, null, NodeType.InnerNode);

                        if (false == ToonParserHelper.IsEmptyObject(value))
                        {
                            // Parse inner node
                            ParserAccept(innerNode, new ToonParserVisitor(), value);
                        }

                        // Add inner node to parent
                        node.AddChild(innerNode);
                    }   
                    else if (ToonParserHelper.IsToonList(value))
                    {
                        // Create list node
                        var listNode = _nodeFactory.CreateNodeFromString(StategyType.Toon, key, null, NodeType.ListNode);
                        
                        if (false == ToonParserHelper.IsEmptyList(value.GetLine(0)))
                        {
                            // Parse list node
                            ParserAccept(listNode, new ToonParserVisitor(), value);
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
        public void VisitListNode(ListNode node, ToonMulitLineStringBuilder lines, bool isRootElement = false)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(lines);

            if (ToonParserHelper.IsToonList(lines, isRootElement))
            {
                // Check if list have a key line from parsing
                if (false == string.IsNullOrWhiteSpace(lines.KeyLine))
                {
                    // Check if list uses toon schema
                    if (ToonParserHelper.IsSchemaList(lines))
                    {
                        ToonParserHelper.ParseSchemaList(node, lines);
                    }
                    else
                    {
                        // Extract object list
                        var items = ToonParserHelper.ExtractObjectList(lines);
                        int index = 0;
                        foreach(var keyValuePair in items)
                        {
                            // Convert key to int key
                            string key = index.ToString();
                            var value = keyValuePair;

                            if (ToonParserHelper.IsToonPrimitiveLine(value))
                            {
                                string? strValue = ToonParserHelper.ExtractValueFromLine(value.GetLine(0));
                                var childNode = _nodeFactory.CreateNodeFromString(StategyType.Toon, key, strValue, NodeType.Leaf);
                                
                                node.AddChild(childNode);
                            }
                            else if (ToonParserHelper.IsToonSingleValue(value))
                            {
                                var tmp = value.GetLine(0).TrimStartAndEnd();
                                var childNode = ParseMethods.ParsePrimitiveNode(StategyType.Toon, tmp, 0, key);
                                node.AddChild(childNode);
                            }
                            else if (ToonParserHelper.IsToonObject(value))
                            {
                                // Create inner node
                                var innerNode = _nodeFactory.CreateNodeFromString(StategyType.Toon, key, null, NodeType.InnerNode);
                                
                                if (false == ToonParserHelper.IsEmptyObject(value))
                                {
                                    // Parse inner node
                                    ParserAccept(innerNode, new ToonParserVisitor(), value);
                                }

                                // Add inner node to parent
                                node.AddChild(innerNode);
                            }   
                            else if (ToonParserHelper.IsToonList(value))
                            {
                                // Create list node
                                var listNode = _nodeFactory.CreateNodeFromString(StategyType.Toon, key, null, NodeType.ListNode);
                                
                                if (false == ToonParserHelper.IsEmptyList(value.GetLine(0)))
                                {
                                    // Parse list node
                                    ParserAccept(listNode, new ToonParserVisitor(), value);
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
                    if (ToonParserHelper.IsPrimitiveList(lines))
                    {
                        ToonParserHelper.ParsePrimitiveList(node, lines.GetLine(0));
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

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, ToonMulitLineStringBuilder lines, bool isRootElement = false)
        {
            // Currenlty not needed
            // Will be procced in the VisitInnerNode
            throw new NotImplementedException();
        }
    }
}