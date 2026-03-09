using DotSerial.Common;
using DotSerial.Tree.Deserialize;

namespace DotSerial.Tree.Nodes
{
    /// <summary>
    /// Leaf node
    /// </summary>
    /// <param name="key">Key of node</param>
    /// <param name="value">Value of node</param>
    public class LeafNode(string key, string? value) : IDSNode
    {
        // TODO als  Primär machen!
        internal LeafNode(string key, string? value, bool isQuoted) : this(key, value)
        {
            IsQuoted = isQuoted;
        }

        /// <inheritdoc/>
        public string Key { get; private set; } = key;
        
        /// <inheritdoc/>
        public bool IsQuoted { get; private set; } = false;

        /// <summary>
        /// Value of the leaf
        /// </summary>
        private readonly string? _value = value;

        /// <inheritdoc/>
        public string? GetValue()
        {
            return _value;
        }

        /// <inheritdoc/>
        public bool HasChildren()
        {
            return false;
        }

        /// <inheritdoc/>
        public IDSNode GetChild(string key)
        {
            throw new DotSerialException($"{nameof(GetChild)} can't be called on a leaf node.");
        }

        /// <inheritdoc/>
        public List<IDSNode> GetChildren()
        {
            throw new DotSerialException($"{nameof(GetChildren)} can't be called on a leaf node.");
        }        

        /// <inheritdoc/>
        public void AddChild(IDSNode? node)
        {
            throw new DotSerialException($"{nameof(AddChild)} can't be called on a leaf node.");
        }

        /// <inheritdoc/>
        public object? DeserializeAccept(INodeDeserializeVisitor visitor, Type? type)
        {
            return visitor.VisitLeafNode(this, type);
        }
    }
}