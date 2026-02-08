using System.Text;
using DotSerial.Common;
using DotSerial.Tree;
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

            // Create help object, which contains every line of the yaml file
            var lines = new MultiLineStringBuilder(sb);

            IDSNode rootNode;

            rootNode = null;

            // Check if its an empty yaml file
            if (0 == lines.Count)
            {
                rootNode = _nodeFactory.CreateNode(CommonConstants.MainObjectKey, null, NodeType.InnerNode);
                return new DSToonNode(rootNode);   
            }

            if (ToonParserHelper.IsToonSingleValue(lines))
            {
                rootNode = ParseMethods.ParsePrimitiveNode(lines.GetLine(0), 0, CommonConstants.MainObjectKey);
                return new DSToonNode(rootNode);
            }
            else if (ToonParserHelper.IsToonObject(lines))
            {
                rootNode = _nodeFactory.CreateNode(CommonConstants.MainObjectKey, null, NodeType.InnerNode);
                if (ToonParserHelper.IsEmptyObject(lines.GetLine(0)))
                {
                    return new DSToonNode(rootNode);    
                }
            } 
            else if (ToonParserHelper.IsToonList(lines))
            {
                rootNode = _nodeFactory.CreateNode(CommonConstants.MainObjectKey, null, NodeType.ListNode);
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
                ParserAccept(rootNode, new ToonParserVisitor(), lines);
            }

            return new DSToonNode(rootNode);
        }

        /// <summary>
        /// Parser for toon
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="lines">MultiLineStringBuilder</param>    
        private static void ParserAccept(IDSNode node, ToonParserVisitor visitor, MultiLineStringBuilder lines)
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
                throw new DSToonException("Parse: Unknown node type.");
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

            if (ToonParserHelper.IsToonObject(lines))
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
                        var childNode = _nodeFactory.CreateNode(key, strValue, NodeType.Leaf);
                        node.AddChild(childNode);
                    }
                    else if (ToonParserHelper.IsToonObject(value))
                    {
                        // Create inner node
                        var innerNode = _nodeFactory.CreateNode(key, null, NodeType.InnerNode);

                        if (false == ToonParserHelper.IsEmptyObject(value.GetLine(0)))
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
                        var listNode = _nodeFactory.CreateNode(key, null, NodeType.ListNode);

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
        public void VisitListNode(ListNode node, MultiLineStringBuilder lines)
        {
            throw new NotImplementedException();
        }            

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, MultiLineStringBuilder lines)
        {
            throw new NotImplementedException();
        }
    }
}