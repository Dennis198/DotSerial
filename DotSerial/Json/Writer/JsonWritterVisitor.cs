using DotSerial.Common;
using DotSerial.Common.Writer;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Json.Writer
{
    /// <summary>
    /// Implementation of the visitor for json writer.
    /// </summary>
    internal class JsonWriterVisitor : IJsonNodeWriterVisitor, IWriteStrategy
    {
        public ReadOnlySpan<char> Write(DSNode node)
        {
            ArgumentNullException.ThrowIfNull(node);

            char[]? result = null;

            DotSerialStringBuilder dtSB = new(8192);
            try
            {
                var internalNode = node.GetInternalData();
                WriterAccept(internalNode, new JsonWriterVisitor(), ref dtSB, new JsonWriterOptions(0, false));

                dtSB.Truncate(dtSB.Length - 1);
                dtSB.Trim();

                result = dtSB.ToArray();
            }
            finally
            {
                dtSB.Dispose();
            }

            return result.AsSpan();
        }

        /// <inheritdoc/>
        public static ReadOnlySpan<char> Write(DSJsonNode? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            char[]? result = null;

            DotSerialStringBuilder dtSB = new(8192);
            try
            {
                var internalNode = node.GetInternalData();
                WriterAccept(internalNode, new JsonWriterVisitor(), ref dtSB, new JsonWriterOptions(0, false));

                dtSB.Truncate(dtSB.Length - 1);
                dtSB.Trim();

                result = dtSB.ToArray();
            }
            finally
            {
                dtSB.Dispose();
            }

            return result.AsSpan();
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, ref DotSerialStringBuilder sb, JsonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                var children = node.GetChildren();
                if (options.AddKey)
                {
                    JsonWriterHelper.AddObjectStart(ref sb, node.Key.ToString(), level);
                }
                else
                {
                    WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);
                    sb.Append(JsonConstants.ObjectStart);
                }

                foreach (var child in children)
                {
                    WriterAccept(child, this, ref sb, new JsonWriterOptions(level + 1));
                }

                // Remove last ','
                sb.Truncate(sb.Length - 1);

                JsonWriterHelper.AddObjectEnd(ref sb, level);
            }
            else
            {
                if (options.AddKey)
                {
                    // Empty node
                    JsonWriterHelper.AddEmptyObject(ref sb, level, node.Key);
                }
                else
                {
                    // Empty node
                    JsonWriterHelper.AddEmptyObject(ref sb, level);
                }
            }
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, ref DotSerialStringBuilder sb, JsonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                if (options.AddKey)
                {
                    JsonWriterHelper.AddObjectStart(ref sb, node.Key, level);
                }
                else
                {
                    sb.AppendLine();
                    WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);
                    sb.Append(JsonConstants.ObjectStart);
                }

                var children = node.GetChildren();
                foreach (var child in children)
                {
                    WriterAccept(child, this, ref sb, new JsonWriterOptions(level + 1));
                }

                // Remove last ','
                sb.Truncate(sb.Length - 1);

                JsonWriterHelper.AddObjectEnd(ref sb, level);
            }
            else
            {
                if (options.AddKey)
                {
                    // Empty node
                    JsonWriterHelper.AddEmptyObject(ref sb, level, node.Key);
                }
                else
                {
                    // Empty node
                    JsonWriterHelper.AddEmptyObject(ref sb, level);
                }
            }
        }

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, ref DotSerialStringBuilder sb, JsonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            string? value = node.GetValue();
            bool needQuotes = node.IsQuoted;

            if (options.AddKey)
            {
                string key = node.Key;
                JsonWriterHelper.AddKeyValuePair(ref sb, key, value, level, needQuotes);
            }
            else
            {
                JsonWriterHelper.AddOnlyValue(ref sb, value, level, needQuotes);
            }
        }

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, ref DotSerialStringBuilder sb, JsonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                var children = node.GetChildren();

                if (false == node.IsPrimitiveList())
                {
                    if (options.AddKey)
                    {
                        sb.AppendLine();
                        WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);
                        sb.Append($"\"{node.Key}\": [");
                    }
                    else
                    {
                        sb.AppendLine();
                        WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);
                        sb.Append(JsonConstants.ListStart);
                    }

                    foreach (var child in children)
                    {
                        WriterAccept(child, this, ref sb, new JsonWriterOptions(level + 1, false));
                    }

                    sb.Truncate(sb.Length - 1);
                    sb.AppendLine();
                    WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);
                    sb.Append(JsonConstants.ListEnd);
                    sb.Append(CommonConstants.Comma);
                }
                else
                {
                    sb.AppendLine();
                    WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);

                    JsonWriterHelper.AddPrimitiveList(ref sb, node, options);
                }
            }
            else
            {
                if (options.AddKey)
                {
                    // Empty node
                    JsonWriterHelper.AddEmptyList(ref sb, level, node.Key);
                }
                else
                {
                    // Empty node
                    JsonWriterHelper.AddEmptyList(ref sb, level);
                }
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
            JsonWriterVisitor visitor,
            ref DotSerialStringBuilder sb,
            JsonWriterOptions options
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
