using System.Threading.Channels;
using DotSerial.Json;
using DotSerial.Toon;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;
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
        private readonly Dictionary<SerializeStrategy, INodeStrategy> _strategies = [];

        /// <summary>
        /// Returns the instance
        /// </summary>
        public static NodeFactory Instance => _instance.Value;

        /// <summary>
        /// Private constructor
        /// </summary>
        private NodeFactory()
        {
            // Initialize strategies
            _strategies.Add(SerializeStrategy.Json, new JsonNodeStrategy());
            _strategies.Add(SerializeStrategy.Toon, new ToonNodeStrategy());
            _strategies.Add(SerializeStrategy.Xml, new XmlNodeStrategy());
            _strategies.Add(SerializeStrategy.Yaml, new YamlNodeStrategy());
        }

        /// <summary>
        /// Creates a node of the given object
        /// </summary>
        /// <param name="category">Create node strategy</param>
        /// <param name="key">Key of the node</param>
        /// <param name="Value">Value of the node</param>
        /// <param name="type">Type of the node</param>
        /// <returns>IDSNode</returns>
        internal IDSNode CreateNode(SerializeStrategy category, string key, object? value, TreeNodeType type)
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
        /// <param name="bookmark">Parser bookmark</param>
        /// <param name="content">Content span</param>
        /// <param name="type">Type of the node</param>
        /// <returns>IDSNode</returns>
        internal IDSNode CreateNodeFromString(
            SerializeStrategy category,
            string key,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content,
            TreeNodeType type
        )
        {
            if (_strategies.TryGetValue(category, out var strategy))
            {
                return strategy.CreateNodeFromString(key, bookmark, content, type);
            }
            throw new NotSupportedException($"Strategy '{category}' is not supported.");
        }

        /// <summary>
        /// Check if a key needes quotes
        /// </summary>
        /// <param name="category">Create node strategy</param>
        /// <param name="key">Key of the node</param>
        /// <returns>True, if quotes are needed</returns>
        internal bool AreQuotesNeededForKey(SerializeStrategy category, string key)
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
        internal static IDSNode WrappNode(IDSNode node, TreeNodeType targetType)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (targetType != TreeNodeType.ListNode && targetType != TreeNodeType.DictionaryNode)
            {
                throw new DotSerialException("NodeFactory: Target node can't be leaf or inner node.");
            }

            if (node is InnerNode wrapper)
            {
                switch (targetType)
                {
                    case TreeNodeType.ListNode:
                    {
                        return new ListNode(wrapper);
                    }
                    case TreeNodeType.DictionaryNode:
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
