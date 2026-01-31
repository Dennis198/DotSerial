using System.Text;
using DotSerial.Tree.Nodes;

namespace DotSerial.Toon.Parser
{
    /// <summary>
    /// Implementation of the visitor for toon parser.
    /// </summary>
    internal class ToonParserVisitor : IToonNodeParserVisitor
    {
        /// <inheritdoc/>
        public static DSToonNode Parse(string str)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, StringBuilder sb)
        {
            throw new NotImplementedException();
        }     

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, StringBuilder sb)
        {
            throw new NotImplementedException();
        }       

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, StringBuilder sb)
        {
            throw new NotImplementedException();
        }            

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb)
        {
            throw new NotImplementedException();
        }
    }
}