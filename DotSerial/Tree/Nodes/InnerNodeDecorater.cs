using DotSerial.Tree.Deserialize;
using DotSerial.Utilities;

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
        public string Key
        {
            get => _wrappedInnerNode.Key;
            set => _wrappedInnerNode.Key = value;
        }

        /// <inheritdoc/>
        public bool IsQuoted => throw new DotSerialException($"{nameof(GetValue)} only for leaf implemented.");

        /// <inheritdoc/>
        public int Count => _wrappedInnerNode.Count;

        /// <inheritdoc/>
        public ICollection<string> Keys => _wrappedInnerNode.Keys;

        /// <inheritdoc/>
        public ICollection<IDSNode> Values => _wrappedInnerNode.Values;

        /// <summary>
        /// Construcot
        /// </summary>
        /// <param name="wrappedNode">Innernode to wrap</param>
        protected InnerNodeDecorater(IDSNode wrappedNode)
        {
            if (wrappedNode is not InnerNode)
            {
                ThrowHelper.ThrowWrongNodeTypeException();
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

        /// <inheritdoc/>
        public void Clear()
        {
            _wrappedInnerNode.Clear();
        }

        /// <inheritdoc/>
        public bool ContainsKey(string key)
        {
            return _wrappedInnerNode.ContainsKey(key);
        }

        /// <inheritdoc/>
        public bool Remove(string key)
        {
            return _wrappedInnerNode.Remove(key);
        }
    }
}
