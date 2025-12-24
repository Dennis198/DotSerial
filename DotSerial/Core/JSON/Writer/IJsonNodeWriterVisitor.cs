using System.Text;
using DotSerial.Core.Tree;

namespace DotSerial.Core.JSON.Writer
{
    /// <summary>
    /// Visitor interface for tree nodes (json).
    /// </summary>
    public interface IJsonNodeWriterVisitor
    {
        public static abstract string Write(DSJsonNode node);
        public abstract void VisitLeafNode(LeafNode node, StringBuilder sb, NodeVisitorOptions options);
        public abstract void VisitInnerNode(InnerNode node, StringBuilder sb, NodeVisitorOptions options);
        public abstract void VisitListNode(ListNode node,StringBuilder sb,NodeVisitorOptions options);
        public abstract void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, NodeVisitorOptions options);
    }
}