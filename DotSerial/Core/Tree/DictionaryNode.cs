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

        public bool IsPrimitiveDictionary()
        {
            var children = GetChildren();
            foreach(var child in children)
            {
                if (child.HasChildren())
                {
                    if (child.GetChildren()[0] is not LeafNode)
                    {
                        return false;
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            return true;
        }
    }
}