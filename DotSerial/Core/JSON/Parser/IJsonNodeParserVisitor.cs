using System.Text;
using DotSerial.Core.Tree;

namespace DotSerial.Core.JSON.Parser
{
    /// <summary>
    /// Visitor interface for tree nodes.
    /// </summary>
    public interface IJsonNodeParserVisitor
    {
        public static abstract DSJsonNode Parse(string str);
        public abstract void VisitLeafNode(LeafNode node, IDSNode? parent, StringBuilder sb);
        public abstract void VisitInnerNode(InnerNode node, IDSNode? parent, StringBuilder sb);
        public abstract void VisitListNode(ListNode node, IDSNode? parent, StringBuilder sb);
        public abstract void VisitDictionaryNode(DictionaryNode node, IDSNode? parent, StringBuilder sb);
    }
}