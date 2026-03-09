using DotSerial.Tree.Deserialize;

namespace DotSerial.Tree.Nodes
{
    /// <summary>
    /// Dictioanry node
    /// </summary>
    /// <param name="wrappedNode">Wrapped node</param>
    public class DictionaryNode(IDSNode wrappedNode) : InnerNodeDecorater(wrappedNode)
    {        
        /// <inheritdoc/>
        public override object? DeserializeAccept(INodeDeserializeVisitor visitor, Type? type)
        {
            return visitor.VisitDictionaryNode(this, type);
        }
    }
}