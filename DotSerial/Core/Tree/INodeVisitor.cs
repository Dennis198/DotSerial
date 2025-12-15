using System.Text;

namespace DotSerial.Core.Tree
{
    /// <summary>
    /// Visitor interface for tree nodes.
    /// </summary>
    public interface INodeVisitor
    {
        public abstract void VisitLeafNode(LeafNode node, StringBuilder sb, int level);
        public abstract void VisitInnerNode(InnerNode node, StringBuilder sb, int level);
        public abstract void VisitListNode(ListNode node,StringBuilder sb, int level);
        public abstract void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, int level);
    }
}