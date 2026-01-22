#region License
//Copyright (c) 2025 Dennis Sölch

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

namespace DotSerial.Tree.Deserialize
{
    /// <summary>
    /// Visitor interface for tree nodes (Deserialize).
    /// </summary>
    public interface INodeDeserializeVisitor
    {
        /// <summary>
        /// Deserialize leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="type">Type</param>
        /// <returns>Object</returns>
        public abstract object? VisitLeafNode(LeafNode node, Type? type);
        /// <summary>
        /// Deserialize inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="type">Type</param>
        /// <returns>Object</returns>
        public abstract object? VisitInnerNode(InnerNode node, Type? type);
        /// <summary>
        /// Deserialize list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="type">Type</param>
        /// <returns>Object</returns>
        public abstract object? VisitListNode(ListNode node, Type? type);
        /// <summary>
        /// Deserialize dictionary node
        /// </summary>
        /// <param name="node">Dictioanry node</param>
        /// <param name="type">Type</param>
        /// <returns>Object</returns>
        public abstract object? VisitDictionaryNode(DictionaryNode node, Type? type);
    }
}