using DotSerial.Common;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Yaml.Writer
{
    /// <summary>
    /// Helper class with methode for writting yaml.
    /// </summary>
    internal static class YamlWriterHelper
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <summary>
        /// Helper methode to add object start to yaml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        /// <param name="prefix">Key-Prefix</param>
        internal static void AddObjectStart(ref DotSerialStringBuilder sb, string key, int level, string? prefix)
        {
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new NotImplementedException();
            }

            // Make sure key:value has its own line.
            sb.AppendLine();

            WriteMethods.AddIndentation(ref sb, level, YamlConstants.IndentationSize);
            key = key.EscapeChars(YamlConstants.CharsToEscape);
            if (_nodeFactory.AreQuotesNeededForKey(SerializeStrategy.Yaml, key))
            {
                key = StringMethods.AddStartAndEndQuotes(key);
            }

            if (string.IsNullOrWhiteSpace(prefix))
            {
                sb.Append($"{key}:");
            }
            else
            {
                sb.Append($"{prefix}{key}:");
            }
        }

        /// <summary>
        /// Converts a key : value pair to an yaml string.
        /// </summary>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="level">Indentation level</param>
        /// <param name="needQuotes">True, if value needs quotes</param>
        /// <param name="prefix">Key-Prefix</param>
        internal static void AddKeyValuePair(
            ref DotSerialStringBuilder sb,
            string key,
            string? value,
            int level,
            bool needQuotes,
            string? prefix = null
        )
        {
            ArgumentNullException.ThrowIfNull(key);

            if (null == key || key.Length == 0)
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            // Make sure key:value has its own line.
            sb.AppendLine();

            WriteMethods.AddIndentation(ref sb, level, YamlConstants.IndentationSize);

            if (null != prefix)
            {
                sb.Append($"{prefix}");
            }

            key = key.EscapeChars(YamlConstants.CharsToEscape);
            if (_nodeFactory.AreQuotesNeededForKey(SerializeStrategy.Yaml, key))
            {
                key = StringMethods.AddStartAndEndQuotes(key);
            }

            if (null == value)
            {
                sb.Append($"{key}: null");
            }
            else
            {
                value = value.EscapeChars(YamlConstants.CharsToEscape);
                if (needQuotes)
                {
                    value = StringMethods.AddStartAndEndQuotes(value);
                }
                sb.Append($"{key}: {value}");
            }
        }

        /// <summary>
        /// Converts a primitive list into yaml
        /// </summary>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Indentation level</param>
        internal static void AddPrimitiveList(
            ref DotSerialStringBuilder sb,
            ListNode node,
            int level,
            YamlWriterOptions options
        )
        {
            ArgumentNullException.ThrowIfNull(node);

            sb.AppendLine();
            bool skipFirstIndentation = false;

            if (!string.IsNullOrWhiteSpace(options.GetPrefix()))
            {
                WriteMethods.AddIndentation(ref sb, level, YamlConstants.IndentationSize);
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
                    bool needQuotes = leaf.IsQuoted;

                    if (!skipFirstIndentation)
                    {
                        WriteMethods.AddIndentation(ref sb, level, YamlConstants.IndentationSize);
                    }
                    skipFirstIndentation = false;

                    if (null == val)
                    {
                        sb.Append($"- {CommonConstants.Null}");
                    }
                    else
                    {
                        val = val.EscapeChars(YamlConstants.CharsToEscape);
                        if (needQuotes)
                        {
                            val = StringMethods.AddStartAndEndQuotes(val);
                        }
                        sb.Append($"- {val}");
                    }

                    sb.AppendLine();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            // Remove last New Line
            sb.Truncate(sb.Length - 1);
        }

        /// <summary>
        /// Add empty Object
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        /// <param name="prefix">Key-Prefix</param>
        internal static void AddEmptyObject(ref DotSerialStringBuilder sb, string? key, int level, string? prefix)
        {
            if (false == string.IsNullOrWhiteSpace(key))
            {
                AddObjectStart(ref sb, key, level, prefix);
                sb.Append(" {}");
            }
            else
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(ref sb, level, YamlConstants.IndentationSize);
                sb.Append("{}");
            }
        }

        /// <summary>
        /// Add empty list
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        /// <param name="prefix">Key-Prefix</param>
        internal static void AddEmptyList(ref DotSerialStringBuilder sb, string? key, int level, string? prefix)
        {
            if (false == string.IsNullOrWhiteSpace(key))
            {
                AddObjectStart(ref sb, key, level, prefix);
                sb.Append(" []");
            }
            else
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(ref sb, level, YamlConstants.IndentationSize);
                sb.Append("[]");
            }
        }

        /// <summary>
        /// Appends only the value without the key
        /// </summary>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="value">Value</param>
        /// <param name="level">Level</param>
        /// <param name="needQuotes">True, if value needs quotes</param>
        /// <param name="prefix">Prefix</param>
        internal static void AddOnlyValue(
            ref DotSerialStringBuilder sb,
            string? value,
            int level,
            bool needQuotes,
            string? prefix = null
        )
        {
            // Maku sure that Value pair is in new line
            sb.AppendLine();

            WriteMethods.AddIndentation(ref sb, level, YamlConstants.IndentationSize);

            if (null != prefix)
            {
                sb.Append($"{prefix}");
            }

            if (null == value)
            {
                sb.Append(CommonConstants.Null);
            }
            else
            {
                value = value.EscapeChars(YamlConstants.CharsToEscape);
                if (needQuotes)
                {
                    value = StringMethods.AddStartAndEndQuotes(value);
                }
                sb.Append($"{value}");
            }
        }
    }
}
