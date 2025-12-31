using DotSerial.Core.Tree.Nodes;

namespace DotSerial.Core.Tree.Deserialize
{
    /// <summary>
    /// Visitor interface for tree nodes.
    /// </summary>
    public interface INodeDeserializeVisitor
    {
        public abstract object? VisitLeafNode(LeafNode node, Type? type);
        public abstract object? VisitInnerNode(InnerNode node, Type? type);
        public abstract object? VisitListNode(ListNode node, Type? type);
        public abstract object? VisitDictionaryNode(DictionaryNode node, Type? type);
    }
}