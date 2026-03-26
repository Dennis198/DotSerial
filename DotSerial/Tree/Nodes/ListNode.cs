using DotSerial.Tree.Deserialize;

namespace DotSerial.Tree.Nodes
{
    /// <summary>
    /// List node
    /// </summary>
    /// <param name="wrappedNode">Wrapped node</param>
    public class ListNode(IDSNode wrappedNode) : InnerNodeDecorater(wrappedNode)
    {
        /// <inheritdoc/>
        public override object? DeserializeAccept(INodeDeserializeVisitor visitor, Type? type)
        {
            return visitor.VisitListNode(this, type);
        }

        /// <summary>
        /// Check if list is of primitive types only or null
        /// </summary>
        public bool IsPrimitiveList()
        {
            var children = GetChildren();
            foreach (var child in children)
            {
                if (child is not LeafNode)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
