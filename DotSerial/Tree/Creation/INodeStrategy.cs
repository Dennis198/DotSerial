using DotSerial.Common;
using DotSerial.Tree.Nodes;

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
        public IDSNode CreateNode(string key, object? value, NodeType type);

        /// <summary>
        /// Creates a node from string value
        /// </summary>
        /// <param name="key">Key of the node</param>
        /// <param name="Value">Value of the node</param>
        /// <param name="type">Type of the node</param>
        /// <returns>IDSNode</returns>
        public IDSNode CreateNodeFromString(string key, string? value, NodeType type);

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
        protected static IDSNode CreateInnerNode(string key, NodeType type)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new DotSerialException("NodeFactory: Key can't be null.");
            }

            if (type == NodeType.Leaf)
            {
                throw new DotSerialException("NodeFactory: Leaf nodes are not allowed in this context.");
            }

            switch (type)
            {
                case NodeType.InnerNode:
                    return new InnerNode(key);
                case NodeType.ListNode:
                {
                    var wrapper = new InnerNode(key);
                    return new ListNode(wrapper);
                }
                case NodeType.DictionaryNode:
                {
                    var wrapper = new InnerNode(key);
                    return new DictionaryNode(wrapper);
                }
                default:
                    throw new DotSerialException($"NodeFactory: Unkown node type {type}.");
            }
        }
    }
}
