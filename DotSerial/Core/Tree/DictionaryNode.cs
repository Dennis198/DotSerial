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

        public override object? DeserializeAccept(INodeDeserializeVisitor visitor, Type? type)
        {
            return visitor.VisitDictionaryNode(this, type);
        }

        public bool IsPrimitiveDictionary()
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