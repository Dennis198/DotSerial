using System.Text;

namespace DotSerial.Core.Tree
{
    public abstract class InnerNodeDecorater : IDSNode
    {
        /// <summary>
        /// The wrapped argument layout.
        /// </summary>
        protected IDSNode _wrappedInnerNode;

         /// <inheritdoc/>
        public string Key => _wrappedInnerNode.Key;

        /// <summary>
        /// Construcot
        /// </summary>
        /// <param name="wrappedNode">Innernode to wrap</param>
        protected InnerNodeDecorater(IDSNode wrappedNode)
        {
            if (wrappedNode is not InnerNode)
            {
                throw new NotImplementedException();
            }
            
            _wrappedInnerNode = wrappedNode;
        }

        /// <inheritdoc/>
        public string GetValue()
        {
            return _wrappedInnerNode.GetValue();
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
        public virtual void Accept (INodeVisitor visitor, StringBuilder sb, NodeVisitorOptions options)
        {
            throw new NotImplementedException();
        }        
    }
}