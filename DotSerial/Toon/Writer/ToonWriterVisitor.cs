using System.Text;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Toon.Writer
{
    /// <summary>
    /// Implementation of the visitor for toon writer.
    /// </summary>
    internal class ToonWriterVisitor : IToonNodeWriterVisitor
    {       
        /// <inheritdoc/>
        public static string Write(DSToonNode node)
        {
            ArgumentNullException.ThrowIfNull(node);

            StringBuilder sb = new();

            var internalNode = node.GetInternalData();
            WriterAccept(internalNode, new ToonWriterVisitor(), sb, new ToonWriterOptions(0, false));


            // Trim start and ending
            sb = sb.TrimStartAndEnd();
            
            return sb.ToString();
        }

        /// <summary>
        /// Writer for Toon
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="sb">Stringbuild</param>
        /// <param name="options">Additional options</param>
        private static void WriterAccept(IDSNode node, ToonWriterVisitor visitor, StringBuilder sb, ToonWriterOptions options)
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
        public void VisitLeafNode(LeafNode node, StringBuilder sb, ToonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;
            string? value = node.GetValue();
            string? prefix = options.GetPrefix();
            
            if (options.AddKey)
            {
                string key = node.Key;
                ToonWriterHelper.AddKeyValuePair(sb, key, value, level, prefix);
            }
            else
            {
                ToonWriterHelper.AddOnlyValue(sb, value, level, prefix);
            }
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, StringBuilder sb, ToonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                if (options.AddKey)
                {
                    ToonWriterHelper.AddObjectStart(sb, node.Key, level, options.GetPrefix());
                    level++;
                }

                var children = node.GetChildren();
                
                foreach(var child in children)
                {
                    WriterAccept(child, this, sb, new ToonWriterOptions(level, true, options.NumberOfPrefix));

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
                    ToonWriterHelper.AddEmptyObject(sb, node.Key.ToString(), level, options.GetPrefix());
                }
            }            
        }     

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, StringBuilder sb, ToonWriterOptions options)
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
                        ToonWriterHelper.AddListStart(sb, node, level, options.AddKey, options.GetPrefix());
                        level++;
                    }

                    if (ToonWriterHelper.UseToonSchema(node, out _))
                    {
                        ToonWriterHelper.AddSchemaList(sb, node, level);
                    }
                    else
                    {
                        foreach (var child in children)
                        {
                            StringBuilder sbChild = new();
                            WriterAccept(child, this, sbChild, new ToonWriterOptions(level, false, options.NumberOfPrefix + 1));

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

                }
                else
                {
                    // if (options.AddKey)
                    // {
                        ToonWriterHelper.AddListStart(sb, node, level, options.AddKey, options.GetPrefix());
                        // level++;
                    // }

                    // List to Toon
                    ToonWriterHelper.AddPrimitiveList(sb, node);
                }
            }
            else
            {
                // if (options.AddKey)
                // {
                //     // Empty node
                //     ToonWriterHelper.AddEmptyList(sb, node.Key, level, options.GetPrefix());
                // }
                // else
                // {
                //     // Empty node
                //     ToonWriterHelper.AddEmptyList(sb, null, level, options.GetPrefix());
                // }
            }            
        }           

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, ToonWriterOptions options)
        {
            // TODO
            return;
            // throw new NotImplementedException();
        }
    }
}