using DotSerial.Tree.Deserialize;

namespace DotSerial.Tree.Nodes
{
    /// <summary>
    /// Inner Node interface
    /// </summary>
    public interface IDSNode
    {
        /// <summary>
        /// Number of child nodes
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// True, if the node value must be quoted when serialized
        /// </summary>
        public bool IsQuoted { get; }

        /// <summary>
        /// Key of the node
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// List of child nodes keys
        /// </summary>
        public ICollection<string> Keys { get; }

        /// <summary>
        /// List of child nodes
        /// </summary>
        public ICollection<IDSNode> Values { get; }

        /// <summary>
        /// /// Append child node
        /// </summary>
        /// <param name="node">Child node</param>
        public abstract void AddChild(IDSNode? node);

        /// <summary>
        /// Clears all child nodes
        /// </summary>
        public void Clear();

        /// <summary>
        /// Check if the node contains child with the key
        /// </summary>
        /// <param name="key">Child key</param>
        public abstract bool ContainsKey(string key);

        /// <summary>
        /// Deserialize visitor
        /// </summary>
        /// <param name="visitor">Visitor</param>
        /// <param name="type">Type of the object</param>
        /// <returns>Deserialized object</returns>
        public abstract object? DeserializeAccept(INodeDeserializeVisitor visitor, Type? type);

        /// <summary>
        /// Gets child node
        /// </summary>
        /// <param name="key">Key of the child node</param>
        /// <returns>Node</returns>
        public abstract IDSNode GetChild(string key);

        /// <summary>
        /// Get all children
        /// </summary>
        /// <returns>List of children</returns>
        public abstract List<IDSNode> GetChildren();

        /// <summary>
        /// Returns the value of the node.
        /// </summary>
        /// <returns>Value of node</returns>
        public abstract string? GetValue();

        /// <summary>
        /// Get info, if node has children.
        /// </summary>
        /// <returns>True, if node has children</returns>
        public abstract bool HasChildren();

        /// <summary>
        /// Remove child node with the key
        /// </summary>
        /// <param name="key">Key of the child node</param>
        /// <returns>True if the child node was removed, otherwise false</returns>
        public abstract bool Remove(string key);
    }
}
