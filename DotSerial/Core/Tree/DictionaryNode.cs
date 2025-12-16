using System.Text;

namespace DotSerial.Core.Tree
{
    public class DictionaryNode : InnerNodeDecorater
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wrappedNode">Wrapped node</param>
        public DictionaryNode(IDSNode wrappedNode) : base(wrappedNode)
        {}

        /// <inheritdoc/>
        public override void Accept (INodeVisitor visitor, StringBuilder sb, NodeVisitorOptions options)
        {
            visitor.VisitDictionaryNode(this, sb, options);
        } 
    }
}