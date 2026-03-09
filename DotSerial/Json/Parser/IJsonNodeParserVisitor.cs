using System.Text;
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
        /// <param name="str">Json string</param>
        /// <returns>Root node of tree.</returns>
        public static abstract DSJsonNode Parse(string str);

        /// <summary>
        /// Visitor for leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="sb">Stringbuilder</param>
        public abstract void VisitLeafNode(LeafNode node, StringBuilder sb);

        /// <summary>
        /// Visitor for inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="sb">Stringbuilder</param>
        public abstract void VisitInnerNode(InnerNode node, StringBuilder sb);

        /// <summary>
        /// Visitor for list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="sb">Stringbuilder</param>
        public abstract void VisitListNode(ListNode node, StringBuilder sb);

        /// <summary>
        /// Visitor for directory node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="sb">Stringbuilder</param>
        public abstract void VisitDictionaryNode(DictionaryNode node, StringBuilder sb);
    }
}
