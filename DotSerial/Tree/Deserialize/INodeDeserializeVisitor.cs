using DotSerial.Tree.Nodes;

namespace DotSerial.Tree.Deserialize
{
    /// <summary>
    /// Visitor interface for tree nodes (Deserialize).
    /// </summary>
    public interface INodeDeserializeVisitor
    {
        /// <summary>
        /// Deserialize leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="type">Type</param>
        /// <returns>Object</returns>
        public abstract object? VisitLeafNode(LeafNode node, Type? type);

        /// <summary>
        /// Deserialize inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="type">Type</param>
        /// <returns>Object</returns>
        public abstract object? VisitInnerNode(InnerNode node, Type? type);

        /// <summary>
        /// Deserialize list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="type">Type</param>
        /// <returns>Object</returns>
        public abstract object? VisitListNode(ListNode node, Type? type);

        /// <summary>
        /// Deserialize dictionary node
        /// </summary>
        /// <param name="node">Dictioanry node</param>
        /// <param name="type">Type</param>
        /// <returns>Object</returns>
        public abstract object? VisitDictionaryNode(DictionaryNode node, Type? type);
    }
}
