using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Toon.Parser
{
    /// <summary>
    /// Visitor interface for tree nodes (toon parser).
    /// </summary>
    internal interface IToonNodeParserVisitor
    {
        /// <summary>
        /// Parse the toon string to create tree structure
        /// </summary>
        /// <param name="content">Toon content</param>
        /// <returns>Root node of tree.</returns>
        public static abstract DSToonNode Parse(ReadOnlySpan<char> content);

        /// <summary>
        /// Visitor for directory node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="lines">Lines</param>
        /// <param name="content">Toon content</param>
        /// <param name="isRootElement">True, if root element</param>
        public abstract void VisitDictionaryNode(
            DictionaryNode node,
            MulitLineReadOnlySpan lines,
            ReadOnlySpan<char> content,
            bool isRootElement = false
        );

        /// <summary>
        /// Visitor for inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="lines">Lines</param>
        /// <param name="content">Toon content</param>
        /// <param name="isRootElement">True, if root element</param>
        public abstract void VisitInnerNode(
            InnerNode node,
            MulitLineReadOnlySpan lines,
            ReadOnlySpan<char> content,
            bool isRootElement = false
        );

        /// <summary>
        /// Visitor for leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="lines">Lines</param>
        /// <param name="content">Toon content</param>
        /// <param name="isRootElement">True, if root element</param>
        public abstract void VisitLeafNode(
            LeafNode node,
            MulitLineReadOnlySpan lines,
            ReadOnlySpan<char> content,
            bool isRootElement = false
        );

        /// <summary>
        /// Visitor for list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="lines">Lines</param>
        /// <param name="content">Toon content</param>
        /// <param name="isRootElement">True, if root element</param>
        public abstract void VisitListNode(
            ListNode node,
            MulitLineReadOnlySpan lines,
            ReadOnlySpan<char> content,
            bool isRootElement = false
        );
    }
}
