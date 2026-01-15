using System.Text;
using DotSerial.Utilities;

namespace DotSerial.XML.Writer
{
    public static class XmlWriterHelper
    {
        internal static void AddKeyValuePair(StringBuilder sb, string key, string? value, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            // Maku sure that key/value pair is in new line
            sb.AppendLine();

            WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);

            AddStartTag(sb, XmlConstants.Property, key);

            if (null == value)
            {
                sb.Append("null");
            }
            else if (value == string.Empty)
            {
                sb.Append("");
            }
            else
            {
                sb.Append(value);
            }

            AddEndTag(sb, XmlConstants.Property);
        }

        internal static void AddObjectStart(StringBuilder sb, string key, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            sb.AppendLine();
            WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);
            AddStartTag(sb, XmlConstants.Object, key);
        }

        internal static void AddObjectEnd(StringBuilder sb, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);

            sb.AppendLine();
            WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);
            AddEndTag(sb, XmlConstants.Object);
        }        

        internal static void AddEmptyObject(StringBuilder sb, int level, string? key = null)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);
                AddEmptyTag(sb, XmlConstants.Object);
            }
            else
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, XmlConstants.IndentationSize);
                AddEmptyTag(sb, XmlConstants.Object, key);
            }
        }

        private static void AddStartTag(StringBuilder sb, string tag, string? name)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(tag);

            sb.AppendFormat("<{0} name=\"{1}\">", tag, name);

            if (string.IsNullOrWhiteSpace(name))
            {
                sb.AppendFormat("<{0}>", tag);
            }
            else
            {
                sb.AppendFormat("<{0} name=\"{1}\">", tag, name);
            }            
        }

        private static void AddEndTag(StringBuilder sb, string tag)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(tag);

            sb.AppendFormat("</{0}>", tag);
        }

        private static void AddEmptyTag(StringBuilder sb, string tag, string? name = null)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(tag);
            ArgumentNullException.ThrowIfNull(name);

            if (string.IsNullOrWhiteSpace(name))
            {
                sb.AppendFormat("<{0} />", tag);
            }
            else
            {
                sb.AppendFormat("<{0} name=\"{1}\"/>", tag, name);
            }
        }        
    }
}