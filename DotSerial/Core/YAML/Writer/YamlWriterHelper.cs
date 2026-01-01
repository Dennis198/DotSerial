using System.Text;
using DotSerial.Core.General;
using DotSerial.Core.Misc;
using DotSerial.Core.Tree.Nodes;

namespace DotSerial.Core.YAML.Writer
{
    public static class YamlWriterHelper
    {
        /// <summary>
        /// Helper methode to add object start to yaml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        /// <param name="prefix">Key-Prefix</param>
        internal static void AddObjectStart(StringBuilder sb, string key, int level, string? prefix)
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
                sb.AppendFormat("{0}\"{1}\":", prefix, key);
            }

        }

        /// <summary>
        /// Converts a key : value pair to an yaml string.
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="level">Indentation level</param>

        internal static void KeyValuePairToYaml(StringBuilder sb, string key, string? value, int level, string? prefix = null)
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
                sb.AppendFormat("{0}", prefix);
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
        internal static void PrimitiveListToYaml(StringBuilder sb, ListNode node, int level, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            sb.AppendLine();
            // WriteMethods.AddIndentation(sb, level, YAMLConstants.IndentationSize);

            // // Add Key
            // sb.AppendFormat("\"{0}\":", node.Key);
            // sb.AppendLine();

            bool skipFirstIndentation = false;
            if (!string.IsNullOrWhiteSpace(options.GetPrefix()))
            {
                WriteMethods.AddIndentation(sb, level, YAMLConstants.IndentationSize);
                sb.Append(options.GetPrefix());                
                level += options.NumberOfPrefix;
                skipFirstIndentation = true;
                options.DecreasePrefixCount();
            }

            // Get all children of node
            var children = node.GetChildren();

            foreach (var keyValue in children)
            {
                if (keyValue is LeafNode leaf)
                {

                    string? val = leaf.GetValue();

                    if (!skipFirstIndentation)
                    {
                        WriteMethods.AddIndentation(sb, level, YAMLConstants.IndentationSize);
                    }
                    skipFirstIndentation = false;

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