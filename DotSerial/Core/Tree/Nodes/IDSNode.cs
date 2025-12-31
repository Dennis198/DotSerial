using DotSerial.Core.Tree.Deserialize;

namespace DotSerial.Core.Tree.Nodes
{
    /// <summary>
    /// Node interface
    /// </summary>
    public interface IDSNode
    {
        /// <summary>
        /// Key of the node
        /// </summary>
        public string Key {get;}

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
        /// Append child node
        /// </summary>
        /// <param name="node">Child node</param>
        public abstract void AddChild(IDSNode? node);

        public abstract object? DeserializeAccept(INodeDeserializeVisitor visitor, Type? type);
    }
}