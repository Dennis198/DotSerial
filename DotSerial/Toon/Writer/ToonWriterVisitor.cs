using DotSerial.Common.Writer;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Toon.Writer
{
    /// <summary>
    /// Implementation of the visitor for toon writer.
    /// </summary>
    internal class ToonWriterVisitor : IToonNodeWriterVisitor, IWriteStrategy
    {
        /// <inheritdoc/>
        public ReadOnlySpan<char> Write(DSNode node)
        {
            ArgumentNullException.ThrowIfNull(node);

            char[]? result = null;

            DotSerialStringBuilder dtSB = new(8192);
            try
            {
                var internalNode = node.GetInternalData();
                WriterAccept(internalNode, new ToonWriterVisitor(), ref dtSB, new ToonWriterOptions(0, false));
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
        public void VisitDictionaryNode(DictionaryNode node, ref DotSerialStringBuilder sb, ToonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                var children = node.GetChildren();

                if (options.AddKey)
                {
                    ToonWriterHelper.AddObjectStart(ref sb, node.Key, level, options.GetPrefix());
                    level++;
                }

                foreach (var child in children)
                {
                    WriterAccept(child, this, ref sb, new ToonWriterOptions(level, true));
                }
            }
            else
            {
                if (options.AddKey)
                {
                    // Empty node
                    ToonWriterHelper.AddEmptyObject(ref sb, node.Key, level, options.GetPrefix());
                }
            }
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, ref DotSerialStringBuilder sb, ToonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                if (options.AddKey)
                {
                    ToonWriterHelper.AddObjectStart(ref sb, node.Key, level, options.GetPrefix());
                    level++;
                }

                var children = node.GetChildren();

                foreach (var child in children)
                {
                    WriterAccept(child, this, ref sb, new ToonWriterOptions(level, true, options.NumberOfPrefix));

                    if (child is LeafNode)
                    {
                        // If the child is a leaf node, the list indicator(s) have been used
                        // in this level, so we need to reset the count for the next nodes.
                        level += options.NumberOfPrefix;
                        options.NumberOfPrefix = 0;
                    }
                }
            }
            else
            {
                // Empty node
                if (options.AddKey)
                {
                    ToonWriterHelper.AddEmptyObject(ref sb, node.Key.ToString(), level, options.GetPrefix());
                }
            }
        }

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, ref DotSerialStringBuilder sb, ToonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;
            string? value = node.GetValue();
            string? prefix = options.GetPrefix();

            if (options.AddKey)
            {
                string key = node.Key;
                ToonWriterHelper.AddKeyValuePair(ref sb, key, value, level, node.IsQuoted, prefix);
            }
            else
            {
                ToonWriterHelper.AddOnlyValue(ref sb, value, level, node.IsQuoted, prefix);
            }
        }

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, ref DotSerialStringBuilder sb, ToonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                if (false == node.IsPrimitiveList())
                {
                    var children = node.GetChildren();

                    ToonWriterHelper.AddListStart(ref sb, node, level, options.AddKey, options.GetPrefix());
                    level++;

                    if (ToonWriterHelper.UseToonSchema(node, out _))
                    {
                        ToonWriterHelper.AddSchemaList(ref sb, node, level);
                    }
                    else
                    {
                        foreach (var child in children)
                        {
                            WriterAccept(
                                child,
                                this,
                                ref sb,
                                new ToonWriterOptions(level, false, options.NumberOfPrefix + 1)
                            );
                        }
                    }
                }
                else
                {
                    ToonWriterHelper.AddListStart(ref sb, node, level, options.AddKey, options.GetPrefix());
                    // List to Toon
                    ToonWriterHelper.AddPrimitiveList(ref sb, node);
                }
            }
            else
            {
                // Empty list
                ToonWriterHelper.AddListStart(ref sb, node, level, options.AddKey, options.GetPrefix());
            }
        }

        /// <summary>
        /// Writer for Toon
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="options">Additional options</param>
        private static void WriterAccept(
            IDSNode node,
            ToonWriterVisitor visitor,
            ref DotSerialStringBuilder sb,
            ToonWriterOptions options
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
