using DotSerial.Core.Tree.Deserialize;

namespace DotSerial.Core.Tree.Nodes
{
    public class ListNode : InnerNodeDecorater
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wrappedNode">Wrapped node</param>
        public ListNode(IDSNode wrappedNode) : base(wrappedNode)
        {}

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
            foreach(var child in children)
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