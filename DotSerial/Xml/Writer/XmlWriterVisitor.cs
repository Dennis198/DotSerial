using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Xml.Writer
{
    /// <summary>
    /// Implementation of the visitor for xml writer.
    /// </summary>
    internal class XmlWriterVisitor : IXmlNodeWriterVisitor
    {
        /// <inheritdoc/>
        public static ReadOnlySpan<char> Write(DSXmlNode? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            char[]? result = null;

            DotSerialStringBuilder dtSB = new(8192);
            try
            {
                dtSB.AppendLine(XmlConstants.XmlDeclaration);

                var internalNode = node.GetInternalData();
                WriterAccept(internalNode, new XmlWriterVisitor(), ref dtSB, new XmlWriterOptions(0));
                result = dtSB.ToArray();

                // Trim start and ending
                // sb = sb.TrimStartAndEnd();
            }
            finally
            {
                dtSB.Dispose();
            }

            return result.AsSpan();
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, ref DotSerialStringBuilder sb, XmlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                XmlWriterHelper.AddObjectStart(ref sb, node.Key, level);

                var children = node.GetChildren();
                foreach (var child in children)
                {
                    WriterAccept(child, this, ref sb, new XmlWriterOptions(level + 1));
                }

                XmlWriterHelper.AddObjectEnd(ref sb, level);
            }
            else
            {
                // Empty node
                XmlWriterHelper.AddEmptyObject(ref sb, level, node.Key);
            }
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, ref DotSerialStringBuilder sb, XmlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                XmlWriterHelper.AddObjectStart(ref sb, node.Key, level);

                var children = node.GetChildren();
                foreach (var child in children)
                {
                    WriterAccept(child, this, ref sb, new XmlWriterOptions(level + 1));
                }

                XmlWriterHelper.AddObjectEnd(ref sb, level);
            }
            else
            {
                // Empty node
                XmlWriterHelper.AddEmptyObject(ref sb, level, node.Key);
            }
        }

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, ref DotSerialStringBuilder sb, XmlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            string? value = node.GetValue();
            string key = node.Key;

            XmlWriterHelper.AddKeyValuePair(ref sb, key, value, level, node.IsQuoted);
        }

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, ref DotSerialStringBuilder sb, XmlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                XmlWriterHelper.AddListStart(ref sb, node.Key, level);

                var children = node.GetChildren();
                foreach (var child in children)
                {
                    WriterAccept(child, this, ref sb, new XmlWriterOptions(level + 1));
                }

                XmlWriterHelper.AddListEnd(ref sb, level);
            }
            else
            {
                XmlWriterHelper.AddEmptyList(ref sb, level, node.Key);
            }
        }

        /// <summary>
        /// Writer for json
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="sb">Stringbuild</param>
        /// <param name="options">Additional options</param>
        private static void WriterAccept(
            IDSNode node,
            XmlWriterVisitor visitor,
            ref DotSerialStringBuilder sb,
            XmlWriterOptions options
        )
        {
            if (node is LeafNode leafNode)
            {
                visitor.VisitLeafNode(leafNode, ref sb, options);
            }
            else if (node is InnerNode innerNode)
            {
                visitor.VisitInnerNode(innerNode, ref sb, options);
            }
            else if (node is ListNode listNode)
            {
                visitor.VisitListNode(listNode, ref sb, options);
            }
            else if (node is DictionaryNode dicNode)
            {
                visitor.VisitDictionaryNode(dicNode, ref sb, options);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
