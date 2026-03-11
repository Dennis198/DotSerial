using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Json.Writer
{
    /// <summary>
    /// Visitor interface for tree nodes (json writer).
    /// </summary>
    internal interface IJsonNodeWriterVisitor
    {
        /// <summary>
        /// Writes the tree to a json string.
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>Json string</returns>
        public static abstract ReadOnlySpan<char> Write(DSJsonNode node);

        /// <summary>
        /// Visit dictionary node
        /// </summary>
        /// <param name="node">Dictioanry node</param>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitDictionaryNode(
            DictionaryNode node,
            ref DotSerialStringBuilder sb,
            JsonWriterOptions options
        );

        /// <summary>
        /// Visit inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitInnerNode(InnerNode node, ref DotSerialStringBuilder sb, JsonWriterOptions options);

        /// <summary>
        /// Visit leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitLeafNode(LeafNode node, ref DotSerialStringBuilder sb, JsonWriterOptions options);

        /// <summary>
        /// Visit list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitListNode(ListNode node, ref DotSerialStringBuilder sb, JsonWriterOptions options);
    }
}
