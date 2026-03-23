using DotSerial.Common.Writer;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Yaml.Writer
{
    /// <summary>
    /// Implementation of the visitor for yaml writer.
    /// </summary>
    internal class YamlWriterVisitor : IYamlNodeWriterVisitor, IWriteStrategy
    {
        /// <inheritdoc/>
        public ReadOnlySpan<char> Write(DSNode node)
        {
            ArgumentNullException.ThrowIfNull(node);

            char[]? result = null;

            DotSerialStringBuilder dtSB = new(8192);
            try
            {
                // Add document start
                // dtSB.Append(YamlConstants.YamlDocumentStart);

                var internalNode = node.GetInternalData();
                WriterAccept(internalNode, new YamlWriterVisitor(), ref dtSB, new YamlWriterOptions(0, false));

                // Add document end
                // dtSB.AppendLine();
                // dtSB.Append(YamlConstants.YamlDocumentEnd);

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
        public void VisitDictionaryNode(DictionaryNode node, ref DotSerialStringBuilder sb, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                var children = node.GetChildren();

                if (options.AddKey)
                {
                    YamlWriterHelper.AddObjectStart(ref sb, node.Key, level, options.GetPrefix());
                    level++;
                }

                foreach (var child in children)
                {
                    WriterAccept(child, this, ref sb, new YamlWriterOptions(level, true));
                }
            }
            else
            {
                if (options.AddKey)
                {
                    // Empty node
                    YamlWriterHelper.AddEmptyObject(ref sb, node.Key, level, options.GetPrefix());
                }
                else
                {
                    // Empty node
                    YamlWriterHelper.AddEmptyObject(ref sb, null, level, options.GetPrefix());
                }
            }
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, ref DotSerialStringBuilder sb, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                if (options.AddKey)
                {
                    YamlWriterHelper.AddObjectStart(ref sb, node.Key, level, options.GetPrefix());
                    level++;
                }

                var children = node.GetChildren();

                foreach (var child in children)
                {
                    WriterAccept(child, this, ref sb, new YamlWriterOptions(level, true, options.NumberOfPrefix));

                    if (child is LeafNode)
                    {
                        // If the child is a leaf node, the list indicator(s) have been used
                        // in this level, so we need to reset the count for the next nodes.
                        level += options.NumberOfPrefix;
                        options.NumberOfPrefix = 0;
                    }

                    if (0 != options.NumberOfPrefix)
                    {
                        // Is the currenlty writen node the first item of a list,
                        // then the following items dont need the list indicator
                        // in the same level.
                        options.DecreasePrefixCount();
                        level++;
                    }
                }
            }
            else
            {
                // Empty node
                if (options.AddKey)
                {
                    YamlWriterHelper.AddEmptyObject(ref sb, node.Key.ToString(), level, options.GetPrefix());
                }
            }
        }

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, ref DotSerialStringBuilder sb, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;
            string? value = node.GetValue();
            string? prefix = options.GetPrefix();

            if (options.AddKey)
            {
                string key = node.Key;
                YamlWriterHelper.AddKeyValuePair(ref sb, key, value, level, node.IsQuoted, prefix);
            }
            else
            {
                YamlWriterHelper.AddOnlyValue(ref sb, value, level, node.IsQuoted, prefix);
            }
        }

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, ref DotSerialStringBuilder sb, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                if (false == node.IsPrimitiveList())
                {
                    var children = node.GetChildren();

                    if (options.AddKey)
                    {
                        YamlWriterHelper.AddObjectStart(ref sb, node.Key, level, options.GetPrefix());
                        level++;
                    }

                    foreach (var child in children)
                    {
                        WriterAccept(
                            child,
                            this,
                            ref sb,
                            new YamlWriterOptions(level, false, options.NumberOfPrefix + 1)
                        );

                        if (0 != options.NumberOfPrefix)
                        {
                            // Is the currenlty writen node the first item of a list,
                            // then the following items dont need the list indicator
                            // in the same level.
                            options.DecreasePrefixCount();
                            level++;
                        }
                    }
                }
                else
                {
                    if (options.AddKey)
                    {
                        YamlWriterHelper.AddObjectStart(ref sb, node.Key, level, options.GetPrefix());
                        level++;
                    }

                    // List to yaml
                    YamlWriterHelper.AddPrimitiveList(ref sb, node, level, options);
                }
            }
            else
            {
                if (options.AddKey)
                {
                    // Empty node
                    YamlWriterHelper.AddEmptyList(ref sb, node.Key, level, options.GetPrefix());
                }
                else
                {
                    // Empty node
                    YamlWriterHelper.AddEmptyList(ref sb, null, level, options.GetPrefix());
                }
            }
        }

        /// <summary>
        /// Writer for yaml
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="sb">Stringbuild</param>
        /// <param name="options">Additional options</param>
        private static void WriterAccept(
            IDSNode node,
            YamlWriterVisitor visitor,
            ref DotSerialStringBuilder sb,
            YamlWriterOptions options
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
