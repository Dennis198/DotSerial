using System.Text;

namespace DotSerial.Core.Tree
{
    /// <summary>
    /// Visitor interface for tree nodes.
    /// </summary>
    public interface INodeParserVisitor
    {
        public abstract void VisitLeafNode(LeafNode node, IDSNode? parent, StringBuilder sb);
        public abstract void VisitInnerNode(InnerNode node, IDSNode? parent, StringBuilder sb);
        public abstract void VisitListNode(ListNode node, IDSNode? parent, StringBuilder sb);
    }
}