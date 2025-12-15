using System.Text;

namespace DotSerial.Core.Tree
{
    public class ListNode : InnerNodeDecorater
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wrappedNode">Wrapped node</param>
        public ListNode(IDSNode wrappedNode) : base(wrappedNode)
        {}

        /// <inheritdoc/>
        public override void Accept (INodeVisitor visitor, StringBuilder sb, int level)
        {
            visitor.VisitListNode(this, sb, level);
        } 

        /// <summary>
        /// Check if list is of primitive types only or null
        /// </summary>
        public bool IsPrimitiveList()
        {
            var children = GetChildren();
            foreach(var child in children)
            {
                if (child is InnerNode)
                {
                    return false;
                }
            }
            return true;
        }
    }
}