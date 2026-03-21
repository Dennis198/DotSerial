using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Xml.Parser
{
    /// <summary>
    /// Visitor interface for tree nodes (xml parser).
    /// </summary>
    internal interface IXmlNodeParserVisitor
    {
        /// <summary>
        /// Visitor for leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="tagKeyPair">XmlTagKeyPair</param>
        /// <param name="bookmark">Parser Bookmark</param>
        /// <param name="content">Xml content</param>
        public abstract void VisitLeafNode(
            LeafNode node,
            XmlTagKeyPair tagKeyPair,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content
        );

        /// <summary>
        /// Visitor for inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="tagKeyPair">XmlTagKeyPair</param>
        /// <param name="bookmark">Parser Bookmark</param>
        /// <param name="content">Xml content</param>
        public abstract void VisitInnerNode(
            InnerNode node,
            XmlTagKeyPair tagKeyPair,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content
        );

        /// <summary>
        /// Visitor for list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="tagKeyPair">XmlTagKeyPair</param>
        /// <param name="bookmark">Parser Bookmark</param>
        /// <param name="content">Xml content</param>
        public abstract void VisitListNode(
            ListNode node,
            XmlTagKeyPair tagKeyPair,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content
        );

        /// <summary>
        /// Visitor for directory node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="tagKeyPair">XmlTagKeyPair</param>
        /// <param name="bookmark">Parser Bookmark</param>
        /// <param name="content">Xml content</param>
        public abstract void VisitDictionaryNode(
            DictionaryNode node,
            XmlTagKeyPair tagKeyPair,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content
        );
    }
}
