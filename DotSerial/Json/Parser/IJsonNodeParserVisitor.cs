using DotSerial.Tree.Nodes;

namespace DotSerial.Json.Parser
{
    /// <summary>
    /// Visitor interface for tree nodes (json parser).
    /// </summary>
    internal interface IJsonNodeParserVisitor
    {
        /// <summary>
        /// Parse the json string to create tree structure
        /// </summary>
        /// <param name="content">Json string</param>
        /// <returns>Root node of tree.</returns>
        public static abstract DSJsonNode Parse(ReadOnlySpan<char> content);

        /// <summary>
        /// Visitor for leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="content">String content</param>
        public abstract void VisitLeafNode(LeafNode node, ReadOnlySpan<char> content);

        /// <summary>
        /// Visitor for inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="content">String content</param>
        public abstract void VisitInnerNode(InnerNode node, ReadOnlySpan<char> content);

        /// <summary>
        /// Visitor for list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="content">String content</param>
        public abstract void VisitListNode(ListNode node, ReadOnlySpan<char> content);

        /// <summary>
        /// Visitor for directory node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="content">String content</param>
        public abstract void VisitDictionaryNode(DictionaryNode node, ReadOnlySpan<char> content);
    }
}
