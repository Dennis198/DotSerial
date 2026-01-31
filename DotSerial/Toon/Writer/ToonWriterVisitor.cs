using System.Text;
using DotSerial.Tree.Nodes;

namespace DotSerial.Toon.Writer
{
    /// <summary>
    /// Implementation of the visitor for toon writer.
    /// </summary>
    internal class ToonWriterVisitor : IToonNodeWriterVisitor
    {       
        /// <inheritdoc/>
        public static string Write(DSToonNode node)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, StringBuilder sb, ToonWriterOptions options)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, StringBuilder sb, ToonWriterOptions options)
        {
            throw new NotImplementedException();
        }     

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, StringBuilder sb, ToonWriterOptions options)
        {
            throw new NotImplementedException();
        }           

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, ToonWriterOptions options)
        {
            throw new NotImplementedException();
        }
    }
}