using System.Text;

namespace DotSerial.Core.Tree
{
    /// <summary>
    /// Visitor interface for tree nodes.
    /// </summary>
    public interface INodeWriterVisitor
    {
        public static abstract string Write(IDSNode node);
        public abstract void VisitLeafNode(LeafNode node, StringBuilder sb, NodeVisitorOptions options);
        public abstract void VisitInnerNode(InnerNode node, StringBuilder sb, NodeVisitorOptions options);
        public abstract void VisitListNode(ListNode node,StringBuilder sb,NodeVisitorOptions options);
        public abstract void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, NodeVisitorOptions options);
    }
}