using System.Text;

namespace DotSerial.Core.Tree
{
    /// <summary>
    /// Lead node
    /// </summary>
    public class LeafNode : IDSNode
    {
        /// <inheritdoc/>
        public string Key {get; private set;}
        /// <summary>
        /// Value of the leaf
        /// </summary>
        private readonly string? _value;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key">Key of node</param>
        /// <param name="value">Value of node</param>
        public LeafNode(string key, string? value)
        {
            Key = key;
            _value = value;
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
        public IDSNode GetChild(string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public List<IDSNode> GetChildren()
        {
            throw new NotImplementedException();
        }        

        /// <inheritdoc/>
        public void AddChild(IDSNode? node)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Accept (INodeVisitor visitor, StringBuilder sb, int level)
        {
            visitor.VisitLeafNode(this, sb, level);
        }

    }
}