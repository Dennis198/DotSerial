
using System.Text;
using DotSerial.Core.General;
using DotSerial.Core.Misc;
using DotSerial.Core.Tree;

namespace DotSerial.Core.YAML.Writer
{
    public class YamlWriterVisitor : IYamlNodeWriterVisitor
    {
        /// <inheritdoc/>
        public static string Write(DSYamlNode node)
        {
            ArgumentNullException.ThrowIfNull(node);

            StringBuilder sb = new();

            // Add document start
            sb.Append(YAMLConstants.YAMLDocumentStart);

            var internalNode = node.GetInternalData();
            WriterAccept(internalNode, new YamlWriterVisitor(), sb, new YamlWriterOptions(0, false));

            // Add document end
            sb.AppendLine();
            sb.Append(YAMLConstants.YAMLDocumentEnd);

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

            KeyValuePairToYaml(sb, node.Key.ToString(), node.GetValue(), level, options.Prefix);
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
                    AddObjectStart(sb, node.Key.ToString(), level, options.Prefix);
                    level++;
                }

                var children = node.GetChildren();
                string tmpPrefix = options.Prefix;
                foreach(var keyValue in children)
                {
                    WriterAccept(keyValue, this, sb, new YamlWriterOptions(level, true, tmpPrefix));
                    if (false == string.IsNullOrWhiteSpace(options.Prefix))
                    {
                        tmpPrefix = " ";
                    }                    
                }
            }
            else
            {
                if (options.AddKey)
                {
                    AddObjectStart(sb, node.Key.ToString(), level, options.Prefix);
                    sb.Append(" {}");
                }
                else
                {
                    throw new NotImplementedException();    
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
                        AddObjectStart(sb, node.Key.ToString(), level, options.Prefix);
                        level++;
                    }

                    StringBuilder sb2 = new();
                    foreach (var keyValue in children)
                    {
                        StringBuilder sb3 = new();
                        WriterAccept(keyValue, this, sb3, new YamlWriterOptions(level, false, YAMLConstants.ListItemIndicator.ToString()));
                        sb2.Append(sb3);
                    }

                    sb.Append(sb2);
                }
                else
                {
                    if (options.AddKey)
                    {
                        AddObjectStart(sb, node.Key.ToString(), level, options.Prefix);
                        level++;
                    }

                    // List to yaml
                    PrimitiveListToYaml(sb, node, level);
                }
            }
            else
            {
                AddObjectStart(sb, node.Key.ToString(), level, options.Prefix);
                sb.Append(" []");
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
                    AddObjectStart(sb, node.Key.ToString(), level, options.Prefix);
                    level++;
                }

                foreach(var keyValue in children)
                {
                    WriterAccept(keyValue, this, sb, new YamlWriterOptions(level, true));
                }
            }
            else
            {
                AddObjectStart(sb, node.Key.ToString(), level, options.Prefix);
                sb.Append(" {}");
            }
        }   

        // ==================================================================================================


        /// <summary>
        /// Helper methode to add object start to yaml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        /// <param name="prefix">Key-Prefix</param>
        private static void AddObjectStart(StringBuilder sb, string key, int level, string? prefix)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new NotImplementedException();
            }

            sb.AppendLine();
            WriteMethods.AddIndentation(sb, level, YAMLConstants.IndentationSize);

            if (string.IsNullOrWhiteSpace(prefix))
            {
                sb.AppendFormat("\"{0}\":", key);
            }
            else
            {
                sb.AppendFormat("{0} \"{1}\":", prefix, key);
            }

        }

        /// <summary>
        /// Converts a key : value pair to an yaml string.
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="level">Indentation level</param>

        private static void KeyValuePairToYaml(StringBuilder sb, string key, string? value, int level, string? prefix = null)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new NotImplementedException();
            }

            // Make sure key:value has its own line.
            sb.AppendLine();

            WriteMethods.AddIndentation(sb, level, YAMLConstants.IndentationSize);

            if (null != prefix)
            {
                sb.AppendFormat("{0} ", prefix);
            }

            if (null == value)
            {
                sb.AppendFormat("\"{0}\": null", key);
            }
            else if (value == string.Empty)
            {
                sb.AppendFormat("\"{0}\": \"\"", key);
            }
            else
            {
                sb.AppendFormat("\"{0}\": \"{1}\"", key, value);
            }
        }

        /// <summary>
        /// Converts a primitive list into yaml
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Indentation level</param>
        private static void PrimitiveListToYaml(StringBuilder sb, ListNode node, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            sb.AppendLine();
            // WriteMethods.AddIndentation(sb, level, YAMLConstants.IndentationSize);

            // // Add Key
            // sb.AppendFormat("\"{0}\":", node.Key);
            // sb.AppendLine();

            // Get all children of node
            var children = node.GetChildren();

            foreach (var keyValue in children)
            {
                if (keyValue is LeafNode leaf)
                {

                    string? val = leaf.GetValue();

                    WriteMethods.AddIndentation(sb, level, YAMLConstants.IndentationSize);

                    if (null == val)
                    {
                        sb.AppendFormat("- {0}", GeneralConstants.Null);
                    }
                    else
                    {
                        sb.AppendFormat("- \"{0}\"", val);
                    }               

                    sb.AppendLine();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            // Remove last New Line
            sb.Remove(sb.Length - 1, 1);
        }        
    }
}