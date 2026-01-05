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

using DotSerial.Tree.Nodes;

namespace DotSerial.Tree
{
    /// <summary>
    /// Node factory interface
    /// </summary>
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

        /// <summary>
        /// Wrapps a node in another node.
        /// </summary>
        /// <param name="node">Node to be wrapped.</param>
        /// <param name="targetType">Wrapped type</param>
        /// <returns>IDSNode</returns>
        public abstract IDSNode WrappNode(IDSNode node, NodeType targetType);
    }
}