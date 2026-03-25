using DotSerial.Tree.Deserialize;
using DotSerial.Utilities;

namespace DotSerial.Tree.Nodes
{
    /// <summary>
    /// Inner node
    /// </summary>
    /// <param name="key">Key of the node</param>
    public class InnerNode(string key) : IDSNode
    {
        /// <summary>
        /// Children of the node
        /// </summary>
        private readonly List<IDSNode> _children = [];

        /// <inheritdoc/>
        public int Count => _children.Count;

        /// <inheritdoc/>
        public bool IsQuoted => throw new DotSerialException($"{nameof(GetValue)} only for leaf implemented.");

        /// <inheritdoc/>
        public string Key { get; set; } = key;

        /// <inheritdoc/>
        public ICollection<string> Keys
        {
            get
            {
                ThrowHelper.ThrowIfNullException(_children);

                var keys = new List<string>();

                foreach (var child in _children)
                {
                    keys.Add(child.Key);
                }

                return keys;
            }
        }

        /// <inheritdoc/>
        public ICollection<IDSNode> Values
        {
            get
            {
                ThrowHelper.ThrowIfNullException(_children);
                return _children;
            }
        }

        /// <inheritdoc/>
        public void AddChild(IDSNode? node)
        {
            ArgumentNullException.ThrowIfNull(node);
            ThrowHelper.ThrowIfNullException(_children);

            string key = node.Key;

            if (ContainsKey(key))
            {
                ThrowHelper.ThrowDuplicateNodeKeyTypeException(key);
            }

            _children.Add(node);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            ThrowHelper.ThrowIfNullException(_children);
            _children.Clear();
        }

        /// <inheritdoc/>
        public bool ContainsKey(string key)
        {
            ArgumentNullException.ThrowIfNull(key);
            ThrowHelper.ThrowIfNullException(_children);
            foreach (var child in _children)
            {
                if (child.Key.Equals(key))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public object? DeserializeAccept(INodeDeserializeVisitor visitor, Type? type)
        {
            return visitor.VisitInnerNode(this, type);
        }

        /// <inheritdoc/>
        public IDSNode GetChild(string key)
        {
            ArgumentNullException.ThrowIfNull(key);
            ThrowHelper.ThrowIfNullException(_children);

            foreach (var child in _children)
            {
                if (child.Key.Equals(key))
                {
                    return child;
                }
            }

            throw new ArgumentException($"Child with the key {key} not found.");
        }

        /// <inheritdoc/>
        public List<IDSNode> GetChildren()
        {
            ThrowHelper.ThrowIfNullException(_children);
            return _children;
        }

        /// <inheritdoc/>
        public string GetValue()
        {
            throw new DotSerialException($"{nameof(GetValue)} only for leaf implemented.");
        }

        /// <inheritdoc/>
        public bool HasChildren()
        {
            ThrowHelper.ThrowIfNullException(_children);
            return _children.Count > 0;
        }

        /// <inheritdoc/>
        public bool Remove(string key)
        {
            ArgumentNullException.ThrowIfNull(key);
            ThrowHelper.ThrowIfNullException(_children);

            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i].Key.Equals(key))
                {
                    _children.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
    }
}
