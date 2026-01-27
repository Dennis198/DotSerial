#region License
//Copyright (c) 2026 Dennis Sölch

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

            AddStartTag(sb, XmlConstants.XmlLeafProp, key);

            if (null == value)
            {
                sb.Append("null");
            }
            else if (value == string.Empty)
            {
                sb.Append("\"\"");
            }
            else
            {
                sb.AppendFormat("\"{0}\"", value);
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

            if (string.IsNullOrWhiteSpace(key))
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

            if (string.IsNullOrWhiteSpace(key))
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

            if (string.IsNullOrWhiteSpace(key))
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

            if (string.IsNullOrWhiteSpace(key))
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

            if (string.IsNullOrWhiteSpace(name))
            {
                sb.AppendFormat("<{0}>", tag);
            }
            else
            {
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

            if (string.IsNullOrWhiteSpace(name))
            {
                sb.AppendFormat("<{0} />", tag);
            }
            else
            {
                sb.AppendFormat("<{0} {1}=\"{2}\"/>", tag,XmlConstants.XmlAttributeKey, name);
            }
        }        
    }
}