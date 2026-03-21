using DotSerial.Utilities;

namespace DotSerial.Xml.Writer
{
    /// <summary>
    /// Helper class with methode for writting xml.
    /// </summary>
    internal static class XmlWriterHelper
    {
        /// <summary>
        /// Add an empty list
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="isLastObject">True, if object is last object</param>
        internal static void AddEmptyList(ref DotSerialStringBuilder sb, int level, string? key = null)
        {
            if (key == null)
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(ref sb, level, XmlConstants.IndentationSize);
                AddEmptyTag(ref sb, XmlConstants.XmlListProp);
            }
            else
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(ref sb, level, XmlConstants.IndentationSize);
                AddEmptyTag(ref sb, XmlConstants.XmlListProp, key);
            }
        }

        /// <summary>
        /// Add an empty object
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="isLastObject">True, if object is last object</param>
        internal static void AddEmptyObject(ref DotSerialStringBuilder sb, int level, string? key = null)
        {
            if (key == null)
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(ref sb, level, XmlConstants.IndentationSize);
                AddEmptyTag(ref sb, XmlConstants.XmlInnerNodeProp);
            }
            else
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(ref sb, level, XmlConstants.IndentationSize);
                AddEmptyTag(ref sb, XmlConstants.XmlInnerNodeProp, key);
            }
        }

        /// <summary>
        /// Helper methode to add key value pair to xml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="value">Value of object</param>
        /// <param name="level">Indentation level</param>
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
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            // Maku sure that key/value pair is in new line
            sb.AppendLine();

            WriteMethods.AddIndentation(ref sb, level, XmlConstants.IndentationSize);

            AddStartTag(ref sb, XmlConstants.XmlLeafProp, key);

            if (null != value)
            {
                value = value.XmlEscape();
                if (needQuotes)
                {
                    value = StringMethods.AddStartAndEndQuotes(value);
                }
                sb.Append($"{value}");
            }

            AddEndTag(ref sb, XmlConstants.XmlLeafProp);
        }

        /// <summary>
        /// Helper methode to add list end symbol and to xml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="isLastObject">True, if object is last object</param>
        internal static void AddListEnd(ref DotSerialStringBuilder sb, int level)
        {
            sb.AppendLine();
            WriteMethods.AddIndentation(ref sb, level, XmlConstants.IndentationSize);
            AddEndTag(ref sb, XmlConstants.XmlListProp);
        }

        /// <summary>
        /// Helper methode to add list start symbol and to xml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        internal static void AddListStart(ref DotSerialStringBuilder sb, string key, int level)
        {
            ArgumentNullException.ThrowIfNull(key);

            if (null == key || key.Length == 0)
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            sb.AppendLine();
            WriteMethods.AddIndentation(ref sb, level, XmlConstants.IndentationSize);
            AddStartTag(ref sb, XmlConstants.XmlListProp, key);
        }

        /// <summary>
        /// Helper methode to add object end symbol and to xml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="isLastObject">True, if object is last object</param>
        internal static void AddObjectEnd(ref DotSerialStringBuilder sb, int level)
        {
            sb.AppendLine();
            WriteMethods.AddIndentation(ref sb, level, XmlConstants.IndentationSize);
            AddEndTag(ref sb, XmlConstants.XmlInnerNodeProp);
        }

        /// <summary>
        /// Helper methode to add object start symbol and to xml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        internal static void AddObjectStart(ref DotSerialStringBuilder sb, string key, int level)
        {
            ArgumentNullException.ThrowIfNull(key);

            if (null == key || key.Length == 0)
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            sb.AppendLine();
            WriteMethods.AddIndentation(ref sb, level, XmlConstants.IndentationSize);
            AddStartTag(ref sb, XmlConstants.XmlInnerNodeProp, key);
        }

        /// <summary>
        /// Add an empty tag
        /// </summary>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="tag">Tag of xml</param>
        /// <param name="name">Key attribute</param>
        private static void AddEmptyTag(ref DotSerialStringBuilder sb, string tag, string? name = null)
        {
            ArgumentNullException.ThrowIfNull(tag);
            ArgumentNullException.ThrowIfNull(name);

            if (name == null)
            {
                sb.Append($"<{tag} />");
            }
            else
            {
                name = name.XmlEscape();
                sb.Append($"<{tag} {XmlConstants.XmlAttributeKey}=\"{name}\"/>");
            }
        }

        /// <summary>
        /// Add end tag
        /// </summary>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="tag">Tag of xml</param>
        private static void AddEndTag(ref DotSerialStringBuilder sb, string tag)
        {
            ArgumentNullException.ThrowIfNull(tag);

            sb.Append($"</{tag}>");
        }

        /// <summary>
        /// Add start tag
        /// </summary>
        /// <param name="sb">DotSerialStringBuilder</param>
        /// <param name="tag">Tag of xml</param>
        /// <param name="name">Key attribute</param>
        private static void AddStartTag(ref DotSerialStringBuilder sb, string tag, string? name)
        {
            ArgumentNullException.ThrowIfNull(tag);

            if (name == null)
            {
                sb.Append($"<{tag}>");
            }
            else
            {
                name = name.XmlEscape();
                sb.Append($"<{tag} {XmlConstants.XmlAttributeKey}=\"{name}\">");
            }
        }
    }
}
