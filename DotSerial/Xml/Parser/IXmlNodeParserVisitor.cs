using System.Text;
using DotSerial.Tree.Nodes;

namespace DotSerial.Xml.Parser
{
    /// <summary>
    /// Visitor interface for tree nodes (xml parser).
    /// </summary>
    internal interface IXmlNodeParserVisitor
    {
        /// <summary>
        /// Parse the xml string to create tree structure
        /// </summary>
        /// <param name="str">Xml string</param>
        /// <returns>Root node of tree.</returns>
        public static abstract DSXmlNode Parse(string str);

        /// <summary>
        /// Visitor for leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="sb">Stringbuilder</param>
        public abstract void VisitLeafNode(LeafNode node, StringBuilder sb, XmlTagKeyPair tagKeyPair);

        /// <summary>
        /// Visitor for inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="sb">Stringbuilder</param>
        public abstract void VisitInnerNode(InnerNode node, StringBuilder sb, XmlTagKeyPair tagKeyPair);

        /// <summary>
        /// Visitor for list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="sb">Stringbuilder</param>
        public abstract void VisitListNode(ListNode node, StringBuilder sb, XmlTagKeyPair tagKeyPair);

        /// <summary>
        /// Visitor for directory node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="sb">Stringbuilder</param>
        public abstract void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, XmlTagKeyPair tagKeyPair);
    }
}
