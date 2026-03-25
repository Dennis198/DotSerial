using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Tree.Creation
{
    /// <summary>
    /// Node strategy interface
    /// </summary>
    internal interface INodeStrategy
    {
        /// <summary>
        /// Creates a node
        /// </summary>
        /// <param name="key">Key of the node</param>
        /// <param name="Value">Value of the node</param>
        /// <param name="type">Type of the node</param>
        /// <returns>IDSNode</returns>
        public IDSNode CreateNode(string key, object? value, TreeNodeType type);

        /// <summary>
        /// Creates a node from string value
        /// /// </summary>
        /// <param name="key">Key of the node</param>
        /// <param name="bookmark">Parser bookmark</param>
        /// <param name="content">Content</param>
        /// <param name="type">Type of the node</param>
        /// <returns>IDSNode</returns>
        public IDSNode CreateNodeFromString(
            string key,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content,
            TreeNodeType type
        );

        /// <summary>
        /// Check if a object and his string represntive need quotes.
        /// </summary>
        /// <param name="value">Object</param>
        /// <param name="strValue">Object as string</param>
        /// <returns>True, if quotes are needed.</returns>
        public bool AreQuotesNeededForValue(object? value, string? strValue);

        /// <summary>
        /// Check if a key needes quotes
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>True, if quotes are needed</returns>
        public bool AreQuotesNeededForKey(string key);

        /// <summary>
        /// Check if the key is valid without quotes
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>True, if value dont need quotes</returns>
        public bool IsValueValidWithoutQuotes(string value);

        /// <summary>
        /// Creates all nodes, which are not leafs.
        /// </summary>
        /// <param name="key">Key of the node</param>
        /// <param name="type">Type of the node</param>
        /// <returns>IDSNode</returns>
        protected static IDSNode CreateInnerNode(string key, TreeNodeType type)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                ThrowHelper.ThrowKeyNodeNullException();
            }

            if (type == TreeNodeType.Leaf)
            {
                ThrowHelper.ThrowWrongNodeTypeException();
            }

            switch (type)
            {
                case TreeNodeType.InnerNode:
                    return new InnerNode(key);
                case TreeNodeType.ListNode:
                {
                    var wrapper = new InnerNode(key);
                    return new ListNode(wrapper);
                }
                case TreeNodeType.DictionaryNode:
                {
                    var wrapper = new InnerNode(key);
                    return new DictionaryNode(wrapper);
                }
                default:
                    ThrowHelper.ThrowUnknownNodeTypeException();
                    throw new Exception("Unreachable code.");
            }
        }
    }
}
