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
        /// Parse the yaml string to create tree structure
        /// </summary>
        /// <param name="str">Yaml string</param>
        /// <returns>Root node of tree.</returns>
        public static abstract DSYamlNode Parse(string str);

        /// <summary>
        /// Visitor for leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="lines">Lines</param>
        public abstract void VisitLeafNode(LeafNode node, MultiLineStringBuilder lines);

        /// <summary>
        /// Visitor for inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="lines">Lines</param>
        public abstract void VisitInnerNode(InnerNode node, MultiLineStringBuilder lines);

        /// <summary>
        /// Visitor for list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="lines">Lines</param>
        public abstract void VisitListNode(ListNode node, MultiLineStringBuilder lines);

        /// <summary>
        /// Visitor for directory node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="lines">Lines</param>
        public abstract void VisitDictionaryNode(DictionaryNode node, MultiLineStringBuilder lines);
    }
}
