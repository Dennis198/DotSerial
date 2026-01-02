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
using DotSerial.Tree.Nodes;

namespace DotSerial.Tree
{
    /// <summary>
    /// Implementation of factory pattern as singelton
    /// </summary>
    public sealed class NodeFactory : INodeFactory
    {
        /// <summary>
        /// Factory instance
        /// </summary>
        private static NodeFactory? _instance = null;

        /// <summary>
        /// Private constructor
        /// </summary>
        private NodeFactory(){}

        /// <summary>
        /// Returns the instance
        /// </summary>
        public static NodeFactory Instance
        {
            get
            {
                if (null != _instance)
                {
                    return _instance;
                }

                _instance = new NodeFactory();
                return _instance;
            }
        }

        /// <inheritdoc/>
        public IDSNode CreateNode(string key, string? value, NodeType type)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new DotSerialException("NodeFactory: Key can't be null.");
            }

            if (null != value && (type != NodeType.Leaf))
            {
                throw new DotSerialException("NodeFactory: Only leaf nodes can have a value.");
            }

            switch(type)
            {
                case NodeType.Leaf:
                    return new LeafNode(key, value);
                case NodeType.InnerNode:
                    return new InnerNode(key);
                case NodeType.ListNode:
                {
                    var wrapper = new InnerNode(key);
                    return new ListNode(wrapper);
                }
                case NodeType.DictionaryNode:
                {
                    var wrapper = new InnerNode(key);
                    return new DictionaryNode(wrapper);
                }
                default:
                    throw new NotImplementedException();
            }
        }
    }
}