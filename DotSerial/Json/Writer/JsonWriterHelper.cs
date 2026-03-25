using DotSerial.Common;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Json.Writer
{
    /// <summary>
    /// Helper class with methode for writting json.
    /// </summary>
    internal static class JsonWriterHelper
    {
        /// <summary>
        /// Add empty list
        /// </summary>
        /// <param name="sb"Stringbuilder></param>
        /// <param name="level">Level</param>
        /// <param name="Key">Key</param>
        internal static void AddEmptyList(ref DotSerialStringBuilder sb, int level, string? key = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);
                sb.Append("[],");
            }
            else
            {
                key = key.EscapeChars(JsonConstants.CharsToEscape);
                sb.AppendLine();
                WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);
                sb.Append($"\"{key}\": [],");
            }
        }

        /// <summary>
        /// Add empty Object
        /// </summary>
        /// <param name="sb"Stringbuilder></param>
        /// <param name="level">Level</param>
        /// <param name="Key">Key</param>
        internal static void AddEmptyObject(ref DotSerialStringBuilder sb, int level, string? key = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);
                sb.Append("{},");
            }
            else
            {
                key = key.EscapeChars(JsonConstants.CharsToEscape);
                sb.AppendLine();
                WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);
                sb.Append($"\"{key}\": {{}},");
            }
        }

        /// <summary>
        /// Add a key value pair
        /// </summary>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="level">Level</param>
        /// <param name="needQuotes">True, if value needs quotes</param>
        internal static void AddKeyValuePair(
            ref DotSerialStringBuilder sb,
            string key,
            string? value,
            int level,
            bool needQuotes
        )
        {
            ArgumentNullException.ThrowIfNull(key);

            if (null == key || key.Length == 0)
            {
                ThrowHelper.ThrowKeyNodeNullException();
            }

            // Maku sure that key/value pair is in new line
            sb.AppendLine();

            WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);

            key = key.EscapeChars(JsonConstants.CharsToEscape);
            sb.Append($"\"{key}\": ");

            if (null == value)
            {
                sb.Append("null,");
            }
            else
            {
                value = value.EscapeChars(JsonConstants.CharsToEscape);
                if (needQuotes)
                {
                    value = StringMethods.AddStartAndEndQuotes(value);
                }
                sb.Append($"{value},");
            }
        }

        /// <summary>
        /// Helper methode to add object end symbol and to json
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="isLastObject">True, if object is last object</param>
        internal static void AddObjectEnd(ref DotSerialStringBuilder sb, int level, bool isLastObject = false)
        {
            sb.AppendLine();
            WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);

            if (isLastObject)
            {
                sb.Append(JsonConstants.ObjectEnd);
            }
            else
            {
                sb.Append(JsonConstants.ObjectEnd);
                sb.Append(CommonConstants.Comma);
            }
        }

        /// <summary>
        /// Helper methode to add object start symbol and to json
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        internal static void AddObjectStart(ref DotSerialStringBuilder sb, string key, int level)
        {
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                ThrowHelper.ThrowKeyNodeNullException();
            }

            key = key.EscapeChars(JsonConstants.CharsToEscape);

            sb.AppendLine();
            WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);
            sb.Append($"\"{key}\": {{");
        }

        /// <summary>
        /// Appends only the value without the key
        /// </summary>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="value">Value</param>
        /// <param name="level">Level</param>
        /// <param name="needQuotes">True, if value needs quotes</param>
        internal static void AddOnlyValue(ref DotSerialStringBuilder sb, string? value, int level, bool needQuotes)
        {
            // Maku sure that Value pair is in new line
            sb.AppendLine();

            WriteMethods.AddIndentation(ref sb, level, JsonConstants.IndentationSize);

            if (null == value)
            {
                sb.Append("null,");
            }
            else
            {
                value = value.EscapeChars(JsonConstants.CharsToEscape);
                if (needQuotes)
                {
                    value = StringMethods.AddStartAndEndQuotes(value);
                }
                sb.Append($"{value},");
            }
        }

        /// <summary>
        /// Adds primitive list
        /// </summary>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="node">ListNode</param>
        /// <param name="options">Options</param>
        internal static void AddPrimitiveList(ref DotSerialStringBuilder sb, ListNode node, JsonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (options.AddKey)
            {
                // Add Key
                string key = node.Key.EscapeChars(JsonConstants.CharsToEscape);
                sb.Append($"\"{key}\": ");
                sb.Append(JsonConstants.ListStart);
            }
            else
            {
                sb.Append(JsonConstants.ListStart);
            }

            // Get all children of node
            var children = node.GetChildren();

            foreach (var child in children)
            {
                string? val = child.GetValue();
                bool needQuotes = child.IsQuoted;

                if (null == val)
                {
                    sb.Append(CommonConstants.Null);
                }
                else
                {
                    val = val.EscapeChars(JsonConstants.CharsToEscape);
                    if (needQuotes)
                    {
                        sb.Append(CommonConstants.Quote);
                        sb.Append(val);
                        sb.Append(CommonConstants.Quote);
                    }
                    else
                    {
                        sb.Append(val);
                    }
                }

                sb.Append(CommonConstants.CommaAndWhiteSpace);
            }

            // Remove last ", "
            sb.Truncate(sb.Length - 2);

            sb.Append(JsonConstants.ListEnd);
            sb.Append(CommonConstants.Comma);
        }
    }
}
