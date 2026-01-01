using DotSerial.Tree.Nodes;

namespace DotSerial.Tree
{
    public interface INodeFactory
    {
        /// <summary>
        /// Creates a node of the given type
        /// </summary>
        /// <param name="key">Key of the node</param>
        /// <param name="Value">Value of the node</param>
        /// <param name="type">Type of the node</param>
        /// <returns>IDSNode</returns>
        public abstract IDSNode CreateNode(string key, string? value, NodeType type);
    }
}