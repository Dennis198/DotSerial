#region License
//Copyright (c) 2025 Dennis Sölch

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
#endregion

using System.Text;

using DotSerial.Common;
using DotSerial.Utilities;
using DotSerial.Tree.Nodes;

namespace DotSerial.Json.Writer
{
    /// <summary>
    /// Implementation of the visitor for json writer.
    /// </summary>
    internal class JsonWriterVisitor : IJsonNodeWriterVisitor
    {
        /// <inheritdoc/>
        public static string Write(DSJsonNode? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            StringBuilder sb = new();
            var internalNode = node.GetInternalData();
            WriterAccept(internalNode, new JsonWriterVisitor(), sb, new JsonWriterOptions(0, false));

            sb.Remove(sb.Length - 1, 1);

            // Trim start and ending
            sb = sb.TrimStartAndEnd();

            return sb.ToString();
        } 

        /// <summary>
        /// Writer for json
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="sb">Stringbuild</param>
        /// <param name="options">Additional options</param>
        private static void WriterAccept(IDSNode node, JsonWriterVisitor visitor, StringBuilder sb, JsonWriterOptions options)
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
        public void VisitLeafNode(LeafNode node, StringBuilder sb, JsonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            int level = options.Level;
            
            string? value = node.GetValue();
            bool needQuotes = node.IsQuoted;
            
            if (options.AddKey)
            {
                string key = node.Key;
                JsonWriterHelper.AddKeyValuePair(sb, key, value, level, needQuotes);    
            }
            else
            {
                JsonWriterHelper.AddOnlyValue(sb, value, level, needQuotes);
            }
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, StringBuilder sb, JsonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            int level = options.Level;

            if (node.HasChildren())
            {
                if (options.AddKey)
                {
                    JsonWriterHelper.AddObjectStart(sb, node.Key, level);    
                }
                else
                {
                    sb.AppendLine();
                    WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                    sb.Append(JsonConstants.ObjectStart);
                }
                
                var children = node.GetChildren();
                foreach(var child in children)
                {
                    WriterAccept(child, this, sb, new JsonWriterOptions(level + 1));
                }

                // Remove last ','
                sb.Remove(sb.Length - 1, 1);

                JsonWriterHelper.AddObjectEnd(sb, level);
            }
            else
            {
                if (options.AddKey)
                {
                    // Empty node
                    JsonWriterHelper.AddEmptyObject(sb, level, node.Key);
                }
                else
                {
                    // Empty node
                    JsonWriterHelper.AddEmptyObject(sb, level);
                }                
            }
        }

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, StringBuilder sb, JsonWriterOptions options)
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

                    foreach (var child in children)
                    {
                        WriterAccept(child, this, sb, new JsonWriterOptions(level + 1, false));
                    }

                    sb.Remove(sb.Length - 1, 1);
                    sb.AppendLine();
                    WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                    sb.Append(JsonConstants.ListEnd);
                    sb.Append(CommonConstants.Comma);

                }
                else
                {
                    sb.AppendLine();
                    WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);

                    JsonWriterHelper.AddPrimitiveList(sb, node, options);
                }
            }
            else
            {
                if (options.AddKey)
                {
                    // Empty node
                    JsonWriterHelper.AddEmptyList(sb, level, node.Key);
                }
                else
                {
                    // Empty node
                    JsonWriterHelper.AddEmptyList(sb, level);
                }                
            }
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, JsonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);
            
            int level = options.Level;

            if (node.HasChildren())
            {
                var children = node.GetChildren();
                if (options.AddKey)
                {
                    JsonWriterHelper.AddObjectStart(sb, node.Key.ToString(), level);    
                }
                else
                {
                    WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                    sb.Append(JsonConstants.ObjectStart);
                }

                foreach(var child in children)
                {
                    WriterAccept(child, this, sb, new JsonWriterOptions(level + 1));
                }

                // Remove last ','
                sb.Remove(sb.Length - 1, 1);

                JsonWriterHelper.AddObjectEnd(sb, level); 
            }
            else
            {
                if (options.AddKey)
                {
                    // Empty node
                    JsonWriterHelper.AddEmptyObject(sb, level, node.Key);
                }
                else
                {
                    // Empty node
                    JsonWriterHelper.AddEmptyObject(sb, level);
                }
            }
        }

    }
}