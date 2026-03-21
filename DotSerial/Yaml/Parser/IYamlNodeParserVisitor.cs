using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Yaml.Parser
{
    /// <summary>
    /// Visitor interface for tree nodes (yaml parser).
    /// </summary>
    internal interface IYamlNodeParserVisitor
    {
        /// <summary>
        /// Visitor for leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="lines">Lines</param>
        /// <param name="content">Yaml content</param>
        public abstract void VisitLeafNode(LeafNode node, MulitLineParserBookmark lines, ReadOnlySpan<char> content);

        /// <summary>
        /// Visitor for inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="lines">Lines</param>
        /// <param name="content">Yaml content</param>
        public abstract void VisitInnerNode(InnerNode node, MulitLineParserBookmark lines, ReadOnlySpan<char> content);

        /// <summary>
        /// Visitor for list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="lines">Lines</param>
        /// <param name="content">Yaml content</param>
        public abstract void VisitListNode(ListNode node, MulitLineParserBookmark lines, ReadOnlySpan<char> content);

        /// <summary>
        /// Visitor for directory node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="lines">Lines</param>
        /// <param name="content">Yaml content</param>
        public abstract void VisitDictionaryNode(
            DictionaryNode node,
            MulitLineParserBookmark lines,
            ReadOnlySpan<char> content
        );
    }
}
