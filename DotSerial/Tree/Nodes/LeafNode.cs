using DotSerial.Tree.Deserialize;

namespace DotSerial.Tree.Nodes
{
    /// <summary>
    /// Leaf node
    /// </summary>
    /// <param name="key">Key of node</param>
    /// <param name="value">Value of node</param>
    /// <param name="value">Truel, if value must be quoted</param>
    public class LeafNode(string key, string? value, bool isQuoted) : IDSNode
    {
        /// <summary>
        /// Value of the leaf
        /// </summary>
        private readonly string? _value = value;

        /// <inheritdoc/>
        public int Count => throw new DotSerialException($"{nameof(Count)} can't be called on a leaf node.");

        /// <inheritdoc/>
        public bool IsQuoted { get; private set; } = isQuoted;

        /// <inheritdoc/>
        public string Key { get; private set; } = key;

        /// <inheritdoc/>
        public ICollection<string> Keys =>
            throw new DotSerialException($"{nameof(Keys)} can't be called on a leaf node.");

        /// <inheritdoc/>
        public ICollection<IDSNode> Values =>
            throw new DotSerialException($"{nameof(Values)} can't be called on a leaf node.");

        /// <inheritdoc/>
        public void AddChild(IDSNode? node)
        {
            throw new DotSerialException($"{nameof(AddChild)} can't be called on a leaf node.");
        }

        /// <inheritdoc/>
        public void Clear()
        {
            throw new DotSerialException($"{nameof(Clear)} can't be called on a leaf node.");
        }

        /// <inheritdoc/>
        public bool ContainsKey(string key)
        {
            throw new DotSerialException($"{nameof(ContainsKey)} can't be called on a leaf node.");
        }

        /// <inheritdoc/>
        public object? DeserializeAccept(INodeDeserializeVisitor visitor, Type? type)
        {
            return visitor.VisitLeafNode(this, type);
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
        public bool Remove(string key)
        {
            throw new DotSerialException($"{nameof(Remove)} can't be called on a leaf node.");
        }
    }
}
