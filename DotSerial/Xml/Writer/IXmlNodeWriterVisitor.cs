using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Xml.Writer
{
    /// <summary>
    /// Visitor interface for tree nodes (xml writer).
    /// </summary>
    internal interface IXmlNodeWriterVisitor
    {
        /// <summary>
        /// Writes the tree to a xml string.
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>xml string</returns>
        public static abstract ReadOnlySpan<char> Write(DSXmlNode node);

        /// <summary>
        /// Visit dictionary node
        /// </summary>
        /// <param name="node">Dictioanry node</param>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitDictionaryNode(
            DictionaryNode node,
            ref DotSerialStringBuilder sb,
            XmlWriterOptions options
        );

        /// <summary>
        /// Visit inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitInnerNode(InnerNode node, ref DotSerialStringBuilder sb, XmlWriterOptions options);

        /// <summary>
        /// Visit leaf node
        /// </summary>
        /// /// <param name="node">Leaf node</param>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitLeafNode(LeafNode node, ref DotSerialStringBuilder sb, XmlWriterOptions options);

        /// <summary>
        /// Visit list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitListNode(ListNode node, ref DotSerialStringBuilder sb, XmlWriterOptions options);
    }
}
