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

        internal static IDSNode CreateNotLeafNode(string key, NodeType type)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new DotSerialException("NodeFactory: Key can't be null.");
            }

            if (type == NodeType.Leaf)
            {
                throw new DotSerialException("NodeFactory: Leaf nodes are not allowed in this context.");
            }

            switch(type)
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