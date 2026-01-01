using DotSerial.Tree.Nodes;

namespace DotSerial.Tree
{
    public sealed class NodeFactory : INodeFactory
    {
        /// <summary>
        /// Factory instance
        /// </summary>
        private static NodeFactory? _instance = null;

        /// <summary>
        /// Private constructor
        /// </summary>
        private NodeFactory(){}

        /// <summary>
        /// Returns the instance
        /// </summary>
        public static NodeFactory Instance
        {
            get
            {
                if (null != _instance)
                {
                    return _instance;
                }

                _instance = new NodeFactory();
                return _instance;
            }
        }

        /// <inheritdoc/>
        public IDSNode CreateNode(string key, string? value, NodeType type)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new NotImplementedException();
            }

            if (null != value && (type != NodeType.Leaf))
            {
                throw new NotImplementedException();
            }

            switch(type)
            {
                case NodeType.Leaf:
                    return new LeafNode(key, value);
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
                    throw new NotImplementedException();
            }
        }
    }
}