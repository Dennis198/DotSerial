using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Json.Parser
{
    /// <summary>
    /// Visitor interface for tree nodes (json parser).
    /// </summary>
    internal interface IJsonNodeParserVisitor
    {
        /// <summary>
        /// Visitor for leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="bookmark">Parser Bookmark</param>
        /// <param name="content">String content</param>
        public abstract void VisitLeafNode(LeafNode node, ParserBookmark bookmark, ReadOnlySpan<char> content);

        /// <summary>
        /// Visitor for inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="bookmark">Parser Bookmark</param>
        /// <param name="content">String content</param>
        public abstract void VisitInnerNode(InnerNode node, ParserBookmark bookmark, ReadOnlySpan<char> content);

        /// <summary>
        /// Visitor for list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="bookmark">Parser Bookmark</param>
        /// <param name="content">String content</param>
        public abstract void VisitListNode(ListNode node, ParserBookmark bookmark, ReadOnlySpan<char> content);

        /// <summary>
        /// Visitor for directory node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="bookmark">Parser Bookmark</param>
        /// <param name="content">String content</param>
        public abstract void VisitDictionaryNode(
            DictionaryNode node,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content
        );
    }
}
