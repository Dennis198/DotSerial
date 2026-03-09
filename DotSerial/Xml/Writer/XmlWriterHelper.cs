using System.Text;
using DotSerial.Utilities;

namespace DotSerial.Xml.Writer
{
    /// <summary>
    /// Helper class with methode for writting xml.
    /// </summary>
    internal static class XmlWriterHelper
    {
        /// <summary>
        /// Helper methode to add key value pair to xml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="value">Value of object</param>
        /// <param name="level">Indentation level</param>
        /// <param name="needQuotes">True, if value needs quotes</param>
        internal static void AddKeyValuePair(StringBuilder sb, string key, string? value, int level, bool needQuotes)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (null == key || key.Length == 0)
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            // Maku sure that key/value pair is in new line
            sb.AppendLine();

            WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);

            AddStartTag(sb, XmlConstants.XmlLeafProp, key);

            if (null != value)
            {
                value = value.XmlEscape();
                if (needQuotes)
                {
                    value = StringMethods.AddStartAndEndQuotes(value);
                }
                sb.AppendFormat("{0}", value);
            }

            AddEndTag(sb, XmlConstants.XmlLeafProp);
        }

        /// <summary>
        /// Helper methode to add object start symbol and to xml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        internal static void AddObjectStart(StringBuilder sb, string key, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (null == key || key.Length == 0)
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            sb.AppendLine();
            WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);
            AddStartTag(sb, XmlConstants.XmlInnerNodeProp, key);
        }

        /// <summary>
        /// Helper methode to add object end symbol and to xml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="isLastObject">True, if object is last object</param>
        internal static void AddObjectEnd(StringBuilder sb, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);

            sb.AppendLine();
            WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);
            AddEndTag(sb, XmlConstants.XmlInnerNodeProp);
        }        

        /// <summary>
        /// Add an empty object
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="isLastObject">True, if object is last object</param>
        internal static void AddEmptyObject(StringBuilder sb, int level, string? key = null)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (key == null)
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);
                AddEmptyTag(sb, XmlConstants.XmlInnerNodeProp);
            }
            else
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);
                AddEmptyTag(sb, XmlConstants.XmlInnerNodeProp, key);
            }
        }
        
        /// <summary>
        /// Helper methode to add list start symbol and to xml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        internal static void AddListStart(StringBuilder sb, string key, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (null == key || key.Length == 0)
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            sb.AppendLine();
            WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);
            AddStartTag(sb, XmlConstants.XmlListProp, key);
        }

        /// <summary>
        /// Helper methode to add list end symbol and to xml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="isLastObject">True, if object is last object</param>
        internal static void AddListEnd(StringBuilder sb, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);

            sb.AppendLine();
            WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);
            AddEndTag(sb, XmlConstants.XmlListProp);
        }           

        /// <summary>
        /// Add an empty list
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="isLastObject">True, if object is last object</param>
        internal static void AddEmptyList(StringBuilder sb, int level, string? key = null)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (key == null)
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);
                AddEmptyTag(sb, XmlConstants.XmlListProp);
            }
            else
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);
                AddEmptyTag(sb, XmlConstants.XmlListProp, key);
            }
        }        

        /// <summary>
        /// Add start tag
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="tag">Tag of xml</param>
        /// <param name="name">Key attribute</param>
        private static void AddStartTag(StringBuilder sb, string tag, string? name)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(tag);

            if (name == null)
            {
                sb.AppendFormat("<{0}>", tag);
            }
            else
            {
                name = name.XmlEscape();
                sb.AppendFormat("<{0} {1}=\"{2}\">", tag, XmlConstants.XmlAttributeKey, name);
            }            
        }

        /// <summary>
        /// Add end tag
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="tag">Tag of xml</param>
        private static void AddEndTag(StringBuilder sb, string tag)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(tag);

            sb.AppendFormat("</{0}>", tag);
        }   

        /// <summary>
        /// Add an empty tag
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="tag">Tag of xml</param>
        /// <param name="name">Key attribute</param>
        private static void AddEmptyTag(StringBuilder sb, string tag, string? name = null)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(tag);
            ArgumentNullException.ThrowIfNull(name);

            if (name == null)
            {
                sb.AppendFormat("<{0} />", tag);
            }
            else
            {
                name = name.XmlEscape();
                sb.AppendFormat("<{0} {1}=\"{2}\"/>", tag,XmlConstants.XmlAttributeKey, name);
            }
        }        
    }
}