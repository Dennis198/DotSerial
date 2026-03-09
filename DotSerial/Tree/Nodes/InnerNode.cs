using DotSerial.Common;
using DotSerial.Tree.Deserialize;

namespace DotSerial.Tree.Nodes
{
    /// <summary>
    /// Inner node
    /// </summary>
    /// <param name="key">Key of the node</param>
    public class InnerNode(string key) : IDSNode
    {
        /// <inheritdoc/>
        public string Key { get; private set; } = key;

        /// <inheritdoc/>
        public bool IsQuoted => throw new DotSerialException($"{nameof(GetValue)} only for leaf implemented.");

        /// <summary>
        /// Children of the node
        /// </summary>
        private readonly List<IDSNode> _children = [];

        /// <inheritdoc/>
        public void AddChild(IDSNode? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (null == _children)
            {
                throw new DotSerialException($"{_children} can't be null.");
            }

            string key = node.Key;

            // Key is already taken
            foreach (var child in _children)
            {
                if (child.Key.Equals(key))
                {
                    throw new DotSerialException($"Child with the key {key} already present.");
                }
            }

            _children.Add(node);
        }

        /// <inheritdoc/>
        public IDSNode GetChild(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            if (null == _children)
            {
                throw new DotSerialException($"{_children} can't be null.");
            }

            foreach (var child in _children)
            {
                if (child.Key.Equals(key))
                {
                    return child;
                }
            }

            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public List<IDSNode> GetChildren()
        {
            if (null == _children)
            {
                throw new DotSerialException($"{_children} can't be null.");
            }

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
            if (null == _children)
            {
                return false;
            }

            return _children.Count > 0;
        }

        /// <inheritdoc/>
        public object? DeserializeAccept(INodeDeserializeVisitor visitor, Type? type)
        {
            return visitor.VisitInnerNode(this, type);
        }
    }
}
