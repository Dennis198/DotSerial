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
        public override void WritterAccept(INodeWriterVisitor visitor, StringBuilder sb, NodeVisitorOptions options)
        {
            visitor.VisitDictionaryNode(this, sb, options);
        } 

        /// <inheritdoc/>
        public override void ParserAccept(INodeParserVisitor visitor, IDSNode? parent, StringBuilder sb)
        {
            visitor.VisitDictionaryNode(this, parent, sb);
        }

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