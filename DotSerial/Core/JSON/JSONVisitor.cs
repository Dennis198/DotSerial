using System.Text;
using DotSerial.Core.General;
using DotSerial.Core.Misc;
using DotSerial.Core.Tree;

namespace DotSerial.Core.JSON
{
    /// <summary>
    /// Visitor interface for tree nodes.
    /// </summary>
    public class JSONVisitor : INodeVisitor
    {
        public void VisitLeafNode(LeafNode node, StringBuilder sb, int level)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            // Make sure key:value has its own line.
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

        public void VisitInnerNode(InnerNode node, StringBuilder sb, int level)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            if (node.HasChildren())
            {
                AddObjectStart(sb, node.Key.ToString(), level);

                var children = node.GetChildren();
                foreach(var keyValue in children)
                {
                    keyValue.Accept(this, sb, level + 1);
                }

                // Remove last ','
                sb.Remove(sb.Length - 1, 1);

                AddObjectEnd(sb, level);
            }
            else
            {
                // TODO
                // AddObjectStart(sb, node.Key.ToString(), level);                
                // AddObjectEnd(sb, level);
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                sb.Append("{},");
            }
        }

        public void VisitListNode(ListNode node, StringBuilder sb, int level)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            if (node.HasChildren())
            {
                var children = node.GetChildren();
                if (false == node.IsPrimitiveList())
                {

                }
                else
                {
                    // Primitive Liste
                    sb.AppendLine();
                    WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                    // Add Key
                    sb.AppendFormat("\"{0}\": ", node.Key);
                    sb.Append(JsonConstants.ListStart);

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
                }
            }
            else
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                sb.Append("[],");
            }
        }

        public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, int level)
        {
            throw new NotImplementedException();
        }

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