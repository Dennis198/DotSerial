using DotSerial.Common;
using DotSerial.Tree.Deserialize;

namespace DotSerial.Tree.Nodes
{
    /// <summary>
    /// Decorater for inner node
    /// </summary>
    public abstract class InnerNodeDecorater : IDSNode
    {
        /// <summary>
        /// The wrapped argument layout.
        /// </summary>
        protected IDSNode _wrappedInnerNode;

        /// <inheritdoc/>
        public string Key => _wrappedInnerNode.Key;

        /// <inheritdoc/>
        public bool IsQuoted => throw new DotSerialException($"{nameof(GetValue)} only for leaf implemented.");

        /// <summary>
        /// Construcot
        /// </summary>
        /// <param name="wrappedNode">Innernode to wrap</param>
        protected InnerNodeDecorater(IDSNode wrappedNode)
        {
            if (wrappedNode is not InnerNode)
            {
                throw new DotSerialException($"Wrapped node {wrappedNode} is not of type ${nameof(InnerNode)}.");
            }

            _wrappedInnerNode = wrappedNode;
        }

        /// <inheritdoc/>
        public string GetValue()
        {
            throw new DotSerialException($"{nameof(GetValue)} only for leaf implemented.");
        }

        /// <inheritdoc/>
        public bool HasChildren()
        {
            return _wrappedInnerNode.HasChildren();
        }

        /// <inheritdoc/>
        public virtual IDSNode GetChild(string key)
        {
            return _wrappedInnerNode.GetChild(key);
        }

        /// <inheritdoc/>
        public virtual List<IDSNode> GetChildren()
        {
            return _wrappedInnerNode.GetChildren();
        }

        /// <inheritdoc/>
        public virtual void AddChild(IDSNode? node)
        {
            _wrappedInnerNode.AddChild(node);
        }

        /// <inheritdoc/>
        public virtual object? DeserializeAccept(INodeDeserializeVisitor visitor, Type? type)
        {
            throw new NotImplementedException();
        }
    }
}
