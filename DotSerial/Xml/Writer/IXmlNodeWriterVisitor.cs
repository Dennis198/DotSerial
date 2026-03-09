using System.Text;
using DotSerial.Tree.Nodes;

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
        public static abstract string Write(DSXmlNode node);

        /// <summary>
        /// Visit leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitLeafNode(LeafNode node, StringBuilder sb, XmlWriterOptions options);

        /// <summary>
        /// Visit inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitInnerNode(InnerNode node, StringBuilder sb, XmlWriterOptions options);

        /// <summary>
        /// Visit list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitListNode(ListNode node, StringBuilder sb, XmlWriterOptions options);

        /// <summary>
        /// Visit dictionary node
        /// </summary>
        /// <param name="node">Dictioanry node</param>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, XmlWriterOptions options);
    }
}
