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

using System.Text;
using DotSerial.Tree.Nodes;

namespace DotSerial.XML.Writer
{
    /// <summary>
    /// Visitor interface for tree nodes (xml writer).
    /// </summary>
    public interface IXmlNodeWriterVisitor
    {
        /// <summary>
        /// Writes the tree to a xml string.
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>xml string</returns>
        public static abstract string Write(DSXmlNode node);
        /// <summary>
        /// Visit leaf node
        /// </summary>
        /// <param name="node">Leaf node</param>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitLeafNode(LeafNode node, StringBuilder sb, XmlWriterOptions options);
        /// <summary>
        /// Visit inner node
        /// </summary>
        /// <param name="node">Inner node</param>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitInnerNode(InnerNode node, StringBuilder sb, XmlWriterOptions options);
        /// <summary>
        /// Visit list node
        /// </summary>
        /// <param name="node">List node</param>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitListNode(ListNode node,StringBuilder sb, XmlWriterOptions options);
        /// <summary>
        /// Visit dictionary node
        /// </summary>
        /// <param name="node">Dictioanry node</param>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="options">Additional options</param>
        public abstract void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, XmlWriterOptions options);
    }
}