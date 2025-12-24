using System.Text;
using DotSerial.Core.General;
using DotSerial.Core.Misc;
using DotSerial.Core.Tree;

namespace DotSerial.Core.JSON.Writer
{
    /// <summary>
    /// Visitor interface for tree nodes.
    /// </summary>
    public class JSONWriterVisitor : IJsonNodeWriterVisitor
    {
        /// <summary>
        /// Converts the node to a string.
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>String</returns>
        public static string Write(DSJsonNode? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            StringBuilder sb = new();

            // Add First '{'
            // sb.Append(JsonConstants.ObjectStart);

            // node.WritterAccept(new JSONWriterVisitor(), sb, new NodeVisitorOptions(0, false));
            var internalNode = node.GetInternalData();
            DSJsonNode.WritterAccept(internalNode, new JSONWriterVisitor(), sb, new JsonNodeVisitorOptions(0, false));

            sb.Remove(sb.Length - 1, 1);

            // Add Last '}'
            // AddObjectEnd(sb, 0, true);

            return sb.ToString();
        } 

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, StringBuilder sb, JsonNodeVisitorOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            int level = options.Level;

            // Maku sure that key/value pair is in new line
            sb.AppendLine();

            WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);

            string key = node.Key;
            string? value = node.GetValue();

            if (null == value)
            {
                sb.AppendFormat("\"{0}\": null,", key);
            }
            else if (value == string.Empty)
            {
                sb.AppendFormat("\"{0}\": \"\",", key);
            }
            else
            {
                sb.AppendFormat("\"{0}\": \"{1}\",", key, value);
            }
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, StringBuilder sb, JsonNodeVisitorOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            int level = options.Level;

            if (node.HasChildren())
            {
                if (options.AddKey)
                {
                    AddObjectStart(sb, node.Key.ToString(), level);    
                }
                else
                {
                    WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                    sb.AppendLine();
                    WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                    sb.Append(JsonConstants.ObjectStart);
                }
                

                var children = node.GetChildren();
                foreach(var keyValue in children)
                {
                    // keyValue.WritterAccept(this, sb, new NodeVisitorOptions(level + 1));
                    DSJsonNode.WritterAccept(keyValue, this, sb, new JsonNodeVisitorOptions(level + 1));
                }

                // Remove last ','
                sb.Remove(sb.Length - 1, 1);

                AddObjectEnd(sb, level);
            }
            else
            {
                // Empty node
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                sb.Append("{},");
            }
        }

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, StringBuilder sb, JsonNodeVisitorOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            int level = options.Level;

            if (node.HasChildren())
            {
                var children = node.GetChildren();

                if (false == node.IsPrimitiveList())
                {
                    if (options.AddKey)
                    {
                        sb.AppendLine();
                        WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                        sb.AppendFormat("\"{0}\": [", node.Key);
                    }
                    else
                    {
                        sb.AppendLine();
                        WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                        sb.Append(JsonConstants.ListStart);
                    }

                    StringBuilder sb2 = new();
                    foreach (var keyValue in children)
                    {
                        StringBuilder sb3 = new();
                        // keyValue.WritterAccept(this, sb3, new NodeVisitorOptions(level + 1, false));
                        DSJsonNode.WritterAccept(keyValue, this, sb3, new JsonNodeVisitorOptions(level + 1, false));
                        sb2.Append(sb3);
                    }

                    sb2.Remove(sb2.Length - 1, 1);
                    sb2.AppendLine();
                    WriteMethods.AddIndentation(sb2, level, JsonConstants.IndentationSize);
                    sb2.Append(JsonConstants.ListEnd + ",");


                    sb.Append(sb2);

                }
                else
                {
                    sb.AppendLine();
                    WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);

                    if (options.AddKey)
                    {
                        // Add Key
                        sb.AppendFormat("\"{0}\": ", node.Key);
                        sb.Append(JsonConstants.ListStart);
                    }
                    else
                    {
                        sb.Append(JsonConstants.ListStart);
                    }

                    foreach (var keyValue in children)
                    {
                        string? val = keyValue.GetValue();

                        if (null == val)
                        {
                            sb.Append(GeneralConstants.Null);
                        }
                        else
                        {
                            sb.Append(GeneralConstants.Quote);
                            sb.Append(val);
                            sb.Append(GeneralConstants.Quote);
                        }

                        sb.Append(", ");
                    }

                    // Remove last ", "
                    sb.Remove(sb.Length - 2, 2);

                    sb.Append(JsonConstants.ListEnd);
                    sb.Append(',');
                }
            }
            else
            {
                // Empty list
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                sb.Append("[],");
            }
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, JsonNodeVisitorOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);
            
            int level = options.Level;

            if (node.HasChildren())
            {
                var children = node.GetChildren();
                if (options.AddKey)
                {
                    AddObjectStart(sb, node.Key.ToString(), level);    
                }
                else
                {
                    WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                    sb.AppendLine();
                    WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                    sb.Append(JsonConstants.ObjectStart);
                }

                if (false == node.IsPrimitiveDictionary())
                {
                    foreach(var keyValue in children)
                    {
                        // var valNode = keyValue.GetChild(keyValue.Key);
                        // keyValue.WritterAccept(this, sb, new NodeVisitorOptions(level + 1));
                        DSJsonNode.WritterAccept(keyValue, this, sb, new JsonNodeVisitorOptions(level + 1));
                        // if (keyValue.HasChildren())
                        // {
                        //     var valNode = keyValue.GetChild(keyValue.Key);
                        //     valNode.WritterAccept(this, sb, new NodeVisitorOptions(level + 1));
                        // }
                        // else
                        // {
                        //     throw new NotImplementedException();
                        // }
                    }
                }
                else
                {
                    foreach(var keyValue in children)
                    {
                       var valNode = keyValue;
                    //    valNode.WritterAccept(this, sb, new NodeVisitorOptions(level + 1));
                       DSJsonNode.WritterAccept(valNode, this, sb, new JsonNodeVisitorOptions(level + 1));
                    } 
                }


                // Remove last ','
                sb.Remove(sb.Length - 1, 1);

                AddObjectEnd(sb, level); 
            }
            else
            {
                if (options.AddKey)
                {
                    // Empty node
                    sb.AppendLine();
                    WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                    sb.AppendFormat("\"{0}\": {{}},", node.Key);
                }
                else
                {
                    // Empty node
                    sb.AppendLine();
                    WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                    sb.Append("{},");
                }

            }
        }

        /// <inheritdoc/>
        // public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, NodeVisitorOptions options)
        // {
        //     ArgumentNullException.ThrowIfNull(node);
        //     ArgumentNullException.ThrowIfNull(sb);
            
        //     int level = options.Level;

        //     if (node.HasChildren())
        //     {
        //         var children = node.GetChildren();
        //         if (options.AddKey)
        //         {
        //             AddObjectStart(sb, node.Key.ToString(), level);    
        //         }
        //         else
        //         {
        //             WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
        //             sb.AppendLine();
        //             WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
        //             sb.Append(JsonConstants.ObjectStart);
        //         }

        //         foreach(var keyValue in children)
        //         {
        //             if (keyValue.HasChildren())
        //             {
        //                 var valNode = keyValue.GetChild(keyValue.Key);
        //                 valNode.WritterAccept(this, sb, new NodeVisitorOptions(level + 1));
        //             }
        //             else
        //             {
        //                 throw new NotImplementedException();
        //             }
        //         }

        //         // Remove last ','
        //         sb.Remove(sb.Length - 1, 1);

        //         AddObjectEnd(sb, level); 
        //     }
        //     else
        //     {
        //         // Empty node
        //         sb.AppendLine();
        //         WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
        //         sb.Append("{},");
        //     }

        // }

        /// <summary>
        /// Helper methode to add object start symbol and to json
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        private static void AddObjectStart(StringBuilder sb, string key, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            sb.AppendLine();
            WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
            sb.AppendFormat("\"{0}\": {{", key);
        }

        /// <summary>
        /// Helper methode to add object end symbol and to json
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="isLastObject">True, if object is last object</param>
        private static void AddObjectEnd(StringBuilder sb, int level, bool isLastObject = false)
        {
            ArgumentNullException.ThrowIfNull(sb);

            sb.AppendLine();
            WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);

            if (isLastObject)
            {
                sb.Append(JsonConstants.ObjectEnd);
            }
            else
            {
                sb.Append(JsonConstants.ObjectEnd + ",");
            }
        }
    }
}