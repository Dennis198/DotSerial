namespace DotSerial.Core.Tree
{
    internal abstract class DSInnerNodeWrapper : IDSNode
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
        protected DSInnerNodeWrapper(IDSNode wrappedNode)
        {
            if (wrappedNode is not DSInnerNode)
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
        public IDSNode GetChild(string key)
        {
            return _wrappedInnerNode.GetChild(key);
        }

        /// <inheritdoc/>
        public void AddChild(IDSNode? node)
        {
            _wrappedInnerNode.AddChild(node);
        }
    }
}