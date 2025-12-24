using System.Text;

namespace DotSerial.Core.Tree
{
    /// <summary>
    /// Inner node
    /// </summary>
    public class InnerNode : IDSNode
    {
        /// <inheritdoc/>
        public string Key {get; private set;}

        /// <summary>
        /// Children of the node
        /// </summary>
        private List<IDSNode> _children = [];

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key">Key of the node</param>
        public InnerNode(string key)
        {
            Key = key;
        }

        /// <inheritdoc/>
        public void AddChild(IDSNode? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (null == _children)
            {
                throw new NotImplementedException();
            }

            string key = node.Key;

            // Key is already taken
            foreach (var child in _children)
            {
                if (child.Key.Equals(key))
                {
                    throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }

            return _children;
        }

        /// <inheritdoc/>
        public string GetValue()
        {
            throw new NotImplementedException();
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

        public object? DeserializeAccept(INodeDeserializeVisitor visitor, Type? type)
        {
            return visitor.VisitInnerNode(this, type);
        }
    }
}