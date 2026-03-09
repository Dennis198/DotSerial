using DotSerial.Tree.Nodes;

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
        /// <param name="str">Toon string</param>
        /// <returns>Root node of tree.</returns>
        public static abstract DSToonNode Parse(string str);
        /// <summary>
        /// Visitor for leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="sb">Stringbuilder</param>
        public abstract void VisitLeafNode(LeafNode node, ToonMulitLineStringBuilder sb, bool isRootElement = false);
        /// <summary>
        /// Visitor for inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="sb">Stringbuilder</param>
        public abstract void VisitInnerNode(InnerNode node, ToonMulitLineStringBuilder sb, bool isRootElement = false);
        /// <summary>
        /// Visitor for list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="sb">Stringbuilder</param>
        public abstract void VisitListNode(ListNode node, ToonMulitLineStringBuilder sb, bool isRootElement = false);
        /// <summary>
        /// Visitor for directory node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="sb">Stringbuilder</param>
        public abstract void VisitDictionaryNode(DictionaryNode node, ToonMulitLineStringBuilder sb, bool isRootElement = false);
    }
}