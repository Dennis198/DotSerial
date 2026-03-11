using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Yaml.Writer
{
    /// <summary>
    /// Visitor interface for tree nodes (yaml writer).
    /// </summary>
    internal interface IYamlNodeWriterVisitor
    {
        /// <summary>
        /// Writes the tree to a yaml string.
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>Yaml string</returns>
        public static abstract ReadOnlySpan<char> Write(DSYamlNode node);

        /// <summary>
        /// Visit dictionary node
        /// </summary>
        /// <param name="node">Dictioanry node</param>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// /// <param name="options">Additional options</param>
        public abstract void VisitDictionaryNode(
            DictionaryNode node,
            ref DotSerialStringBuilder sb,
            YamlWriterOptions options
        );

        /// <summary>
        /// Visit inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitInnerNode(InnerNode node, ref DotSerialStringBuilder sb, YamlWriterOptions options);

        /// <summary>
        /// Visit leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitLeafNode(LeafNode node, ref DotSerialStringBuilder sb, YamlWriterOptions options);

        /// <summary>
        /// Visit list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitListNode(ListNode node, ref DotSerialStringBuilder sb, YamlWriterOptions options);
    }
}
