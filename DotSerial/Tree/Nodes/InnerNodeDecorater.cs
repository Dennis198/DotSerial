#region License
//Copyright (c) 2026 Dennis Sölch

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
#endregion

using DotSerial.Common;
using DotSerial.Tree.Deserialize;

namespace DotSerial.Tree.Nodes
{
    /// <summary>
    /// Decorater for inner node
    /// </summary>
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
                throw new DotSerialException($"Wrapped node {wrappedNode} is not of type ${nameof(InnerNode)}.");
            }
            
            _wrappedInnerNode = wrappedNode;
        }

        /// <inheritdoc/>
        public string GetValue()
        {
            throw new DotSerialException($"{nameof(GetValue)} only for leaf implemented.");
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
        public virtual object? DeserializeAccept(INodeDeserializeVisitor visitor, Type? type)
        {
            throw new NotImplementedException();
        }
    }
}