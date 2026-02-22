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
    /// Leaf node
    /// </summary>
    /// <param name="key">Key of node</param>
    /// <param name="value">Value of node</param>
    public class LeafNode(string key, string? value) : IDSNode
    {
        /// <inheritdoc/>
        public string Key { get; private set; } = key;
        
        /// <inheritdoc/>
        public bool IsQuoted { get; private set; } = false;

        /// <summary>
        /// Value of the leaf
        /// </summary>
        private readonly string? _value = value;

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
            throw new DotSerialException($"{nameof(GetChild)} can't be called on a leaf node.");
        }

        /// <inheritdoc/>
        public List<IDSNode> GetChildren()
        {
            throw new DotSerialException($"{nameof(GetChildren)} can't be called on a leaf node.");
        }        

        /// <inheritdoc/>
        public void AddChild(IDSNode? node)
        {
            throw new DotSerialException($"{nameof(AddChild)} can't be called on a leaf node.");
        }

        /// <inheritdoc/>
        public object? DeserializeAccept(INodeDeserializeVisitor visitor, Type? type)
        {
            return visitor.VisitLeafNode(this, type);
        }
    }
}