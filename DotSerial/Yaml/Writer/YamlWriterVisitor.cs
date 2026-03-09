using System.Text;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Yaml.Writer
{
    /// <summary>
    /// Implementation of the visitor for yaml writer.
    /// </summary>
    internal class YamlWriterVisitor : IYamlNodeWriterVisitor
    {
        /// <inheritdoc/>
        public static string Write(DSYamlNode node)
        {
            ArgumentNullException.ThrowIfNull(node);

            StringBuilder sb = new();

            // Add document start
            sb.Append(YamlConstants.YamlDocumentStart);

            var internalNode = node.GetInternalData();
            WriterAccept(internalNode, new YamlWriterVisitor(), sb, new YamlWriterOptions(0, false));

            // Add document end
            sb.AppendLine();
            sb.Append(YamlConstants.YamlDocumentEnd);

            // Trim start and ending
            sb = sb.TrimStartAndEnd();

            return sb.ToString();
        }

        /// <summary>
        /// Writer for yaml
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="sb">Stringbuild</param>
        /// <param name="options">Additional options</param>
        private static void WriterAccept(IDSNode node, YamlWriterVisitor visitor, StringBuilder sb, YamlWriterOptions options)
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
        public void VisitLeafNode(LeafNode node, StringBuilder sb, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;
            string? value = node.GetValue();
            string? prefix = options.GetPrefix();
            
            if (options.AddKey)
            {
                string key = node.Key;
                YamlWriterHelper.AddKeyValuePair(sb, key, value, level, node.IsQuoted,prefix);
            }
            else
            {
                YamlWriterHelper.AddOnlyValue(sb, value, level, node.IsQuoted, prefix);
            }

        }           

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, StringBuilder sb, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                if (options.AddKey)
                {
                    YamlWriterHelper.AddObjectStart(sb, node.Key, level, options.GetPrefix());
                    level++;
                }

                var children = node.GetChildren();
                
                foreach(var child in children)
                {
                    WriterAccept(child, this, sb, new YamlWriterOptions(level, true, options.NumberOfPrefix));

                    if (child is LeafNode)
                    {
                        // If the child is a leaf node, the list indicator(s) have been used
                        // in this level, so we need to reset the count for the next nodes.
                        level += options.NumberOfPrefix;
                        options.NumberOfPrefix = 0;
                    }

                    if(0 != options.NumberOfPrefix)
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
                    YamlWriterHelper.AddEmptyObject(sb, node.Key.ToString(), level, options.GetPrefix());
                }
            }
        } 


        /// <inheritdoc/>
        public void VisitListNode(ListNode node, StringBuilder sb, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                if (false == node.IsPrimitiveList())
                {    
                    var children = node.GetChildren();  

                    if (options.AddKey)
                    {
                        YamlWriterHelper.AddObjectStart(sb, node.Key, level, options.GetPrefix());
                        level++;
                    }

                    foreach (var child in children)
                    {
                        StringBuilder sbChild = new();
                        WriterAccept(child, this, sbChild, new YamlWriterOptions(level, false, options.NumberOfPrefix + 1));

                        if(0 != options.NumberOfPrefix)
                        {
                            // Is the currenlty writen node the first item of a list,
                            // then the following items dont need the list indicator
                            // in the same level.
                            options.DecreasePrefixCount();
                            level++;
                        }
                        sb.Append(sbChild);
                    }

                }
                else
                {
                    if (options.AddKey)
                    {
                        YamlWriterHelper.AddObjectStart(sb, node.Key, level, options.GetPrefix());
                        level++;
                    }

                    // List to yaml
                    YamlWriterHelper.AddPrimitiveList(sb, node, level, options);
                }
            }
            else
            {
                if (options.AddKey)
                {
                    // Empty node
                    YamlWriterHelper.AddEmptyList(sb, node.Key, level, options.GetPrefix());
                }
                else
                {
                    // Empty node
                    YamlWriterHelper.AddEmptyList(sb, null, level, options.GetPrefix());
                }
            }
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                var children = node.GetChildren();  

                if (options.AddKey)
                {
                    YamlWriterHelper.AddObjectStart(sb, node.Key, level, options.GetPrefix());
                    level++;
                }

                foreach(var child in children)
                {
                    WriterAccept(child, this, sb, new YamlWriterOptions(level, true));
                }
            }
            else
            {
                if (options.AddKey)
                {
                    // Empty node
                    YamlWriterHelper.AddEmptyObject(sb, node.Key, level, options.GetPrefix());
                }
                else
                {
                    // Empty node
                    YamlWriterHelper.AddEmptyObject(sb, null, level, options.GetPrefix());
                }
            }
        }   
    }
}