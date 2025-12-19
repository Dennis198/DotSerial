namespace DotSerial.Core.Tree
{
    /// <summary>
    /// Visitor interface for tree nodes.
    /// </summary>
    public interface INodeDeserializeVisitor
    {
        public abstract object? VisitLeafNode(LeafNode node, Type? type);
        public abstract object? VisitInnerNode(InnerNode node, Type? type);
        public abstract object? VisitListNode(ListNode node, Type? type);
    }
}