using DotSerial.Common;
using DotSerial.Json;
using DotSerial.Toon;
using DotSerial.Tree.Nodes;
using DotSerial.Xml;
using DotSerial.Yaml;

namespace DotSerial.Tree.Creation
{
    /// <summary>
    /// Node factory (singleton)
    /// </summary>
    internal sealed class NodeFactory
    {
        private static readonly Lazy<NodeFactory> _instance = new(() => new NodeFactory());
        private readonly Dictionary<StategyType, INodeStrategy> _strategies = [];

        /// <summary>
        /// Returns the instance
        /// </summary>
        public static NodeFactory Instance => _instance.Value;

        /// <summary>
        /// Private constructor
        /// </summary>
        private NodeFactory()
        {
            //  TODO Auslagern, in diespeareten Ornder json, xml, ..
            // Initialize strategies
            _strategies.Add(StategyType.Json, new JsonNodeStrategy());
            _strategies.Add(StategyType.Toon, new ToonNodeStrategy());
            _strategies.Add(StategyType.Xml, new XmlNodeStrategy());
            _strategies.Add(StategyType.Yaml, new YamlNodeStrategy());
        }

        /// <summary>
        /// Creates a node of the given object
        /// </summary>
        /// <param name="category">Create node strategy</param>
        /// <param name="key">Key of the node</param>
        /// <param name="Value">Value of the node</param>
        /// <param name="type">Type of the node</param>
        /// <returns>IDSNode</returns>
        internal IDSNode CreateNode(StategyType category, string key, object? value, NodeType type)
        {
            if (_strategies.TryGetValue(category, out var strategy))
            {
                return strategy.CreateNode(key, value, type);
            }
            throw new NotSupportedException($"Strategy '{category}' is not supported.");
        }

        /// <summary>
        /// Creates a node of the given string value
        /// </summary>
        /// <param name="category">Create node strategy</param>
        /// <param name="key">Key of the node</param>
        /// <param name="Value">Value of the node</param>
        /// <param name="type">Type of the node</param>
        /// <returns>IDSNode</returns>
        internal IDSNode CreateNodeFromString(StategyType category, string key, string? value, NodeType type)
        {
            if (_strategies.TryGetValue(category, out var strategy))
            {
                return strategy.CreateNodeFromString(key, value, type);
            }
            throw new NotSupportedException($"Strategy '{category}' is not supported.");
        }

        /// <summary>
        /// Check if a key needes quotes
        /// </summary>
        /// <param name="category">Create node strategy</param>
        /// <param name="key">Key of the node</param>
        /// <returns>True, if quotes are needed</returns>
        internal bool AreQuotesNeededForKey(StategyType category, string key)
        {
            if (_strategies.TryGetValue(category, out var strategy))
            {
                return strategy.AreQuotesNeededForKey(key);
            }
            throw new NotSupportedException($"Strategy '{category}' is not supported.");
        }

        /// <summary>
        /// Wrapps a node in another node.
        /// </summary>
        /// <param name="node">Node to be wrapped.</param>
        /// <param name="targetType">Wrapped type</param>
        /// <returns>IDSNode</returns>
        internal static IDSNode WrappNode(IDSNode node, NodeType targetType)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (targetType != NodeType.ListNode && targetType != NodeType.DictionaryNode)
            {
                throw new DotSerialException("NodeFactory: Target node can't be leaf or inner node.");
            }

            if (node is InnerNode wrapper)
            {
                switch (targetType)
                {
                    case NodeType.ListNode:
                    {
                        return new ListNode(wrapper);
                    }
                    case NodeType.DictionaryNode:
                    {
                        return new DictionaryNode(wrapper);
                    }
                    default:
                        throw new DotSerialException("NodeFactory: Target node can't be leaf or inner node.");
                }
            }
            else
            {
                throw new DotSerialException("NodeFactory: Wrapped node must be an inner node.");
            }
        }
    }
}
