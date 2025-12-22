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
        public override void WritterAccept(INodeWriterVisitor visitor, StringBuilder sb, NodeVisitorOptions options)
        {
            visitor.VisitListNode(this, sb, options);
        } 

        /// <inheritdoc/>
        public override void ParserAccept(INodeParserVisitor visitor, IDSNode? parent, StringBuilder sb)
        {
            visitor.VisitListNode(this, parent, sb);
        }

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