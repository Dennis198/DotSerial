using System.Text;
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
        public static string Write(DSXmlNode? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            StringBuilder sb = new();

            // Add Xml Declaration
            StringBuilder sbXmlDeclaration = new();
            sbXmlDeclaration.AppendLine(XmlConstants.XmlDeclaration);

            var internalNode = node.GetInternalData();
            WriterAccept(internalNode, new XmlWriterVisitor(), sb, new XmlWriterOptions(0));
            
            // Trim start and ending
            sb = sb.TrimStartAndEnd();
            
            // Append to xml declaration
            sbXmlDeclaration.Append(sb);

            // Set final stringbuilder
            sb = sbXmlDeclaration;

            return sb.ToString();
        }

        /// <summary>
        /// Writer for json
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="sb">Stringbuild</param>
        /// <param name="options">Additional options</param>
        private static void WriterAccept(IDSNode node, XmlWriterVisitor visitor, StringBuilder sb, XmlWriterOptions options)
        {
            if (node is LeafNode leafNode)
            {
                visitor.VisitLeafNode(leafNode, sb, options);    
            }
            else if (node is InnerNode innerNode)
            {
                visitor.VisitInnerNode(innerNode, sb, options);    
            }
            else if (node is ListNode listNode)
            {
                visitor.VisitListNode(listNode, sb, options);    
            }
            else if (node is DictionaryNode dicNode)
            {
                visitor.VisitDictionaryNode(dicNode, sb, options);
            }
            else
            {
                throw new NotImplementedException();
            }            
        }            

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, StringBuilder sb, XmlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            int level = options.Level;

            string? value = node.GetValue();
            string key = node.Key;

            XmlWriterHelper.AddKeyValuePair(sb, key, value, level, node.IsQuoted);
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, StringBuilder sb, XmlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            int level = options.Level;

            if (node.HasChildren())
            {
                XmlWriterHelper.AddObjectStart(sb, node.Key, level);

                var children = node.GetChildren();
                foreach(var child in children)
                {
                    WriterAccept(child, this, sb, new XmlWriterOptions(level + 1));
                }

                XmlWriterHelper.AddObjectEnd(sb, level);
            }
            else
            {
                // Empty node
                XmlWriterHelper.AddEmptyObject(sb, level, node.Key);
            }

        }
        
        /// <inheritdoc/>
        public void VisitListNode(ListNode node, StringBuilder sb, XmlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            int level = options.Level;

            if (node.HasChildren())
            {
                XmlWriterHelper.AddListStart(sb, node.Key, level);

                var children = node.GetChildren();
                foreach(var child in children)
                {
                    WriterAccept(child, this, sb, new XmlWriterOptions(level + 1));
                }

                XmlWriterHelper.AddListEnd(sb, level);
            }
            else
            {
                XmlWriterHelper.AddEmptyList(sb, level, node.Key);
            }
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, XmlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            int level = options.Level;

            if (node.HasChildren())
            {
                XmlWriterHelper.AddObjectStart(sb, node.Key, level);

                var children = node.GetChildren();
                foreach(var child in children)
                {
                    WriterAccept(child, this, sb, new XmlWriterOptions(level + 1));
                }

                XmlWriterHelper.AddObjectEnd(sb, level);
            }
            else
            {
                // Empty node
                XmlWriterHelper.AddEmptyObject(sb, level, node.Key);
            }
        }
    }
}