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
        /// Helper methode to add object start symbol and to json
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
            WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
            sb.AppendFormat("\"{0}\": {{", key);
        }

        /// <summary>
        /// Helper methode to add object end symbol and to json
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="isLastObject">True, if object is last object</param>
        internal static void AddObjectEnd(StringBuilder sb, int level, bool isLastObject = false)
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
                sb.Append(JsonConstants.ObjectEnd);
                sb.Append(CommonConstants.Comma);
            }
        }

        /// <summary>
        /// Add a key value pair
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="level">Level</param>
        /// <param name="needQuotes">True, if value needs quotes</param>
        internal static void AddKeyValuePair(StringBuilder sb, string key, string? value, int level, bool needQuotes)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            // Maku sure that key/value pair is in new line
            sb.AppendLine();

            WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);

            if (null == value)
            {
                sb.AppendFormat("\"{0}\": null,", key);
            }
            else if (value == string.Empty)
            {
                // NEEDED?????
                sb.AppendFormat("\"{0}\": \"\",", key);
            }
            else if (needQuotes)
            {
                sb.AppendFormat("\"{0}\": \"{1}\",", key, value);
            }
            else
            {
                sb.AppendFormat("\"{0}\": {1},", key, value);
            }
        } 

        /// <summary>
        /// Appends only the value without the key
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="value">Value</param>
        /// <param name="level">Level</param>
        /// <param name="needQuotes">True, if value needs quotes</param>
        internal static void AddOnlyValue(StringBuilder sb, string? value, int level, bool needQuotes)
        {
            ArgumentNullException.ThrowIfNull(sb);

            // Maku sure that Value pair is in new line
            sb.AppendLine();

            WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);

            if (null == value)
            {
                sb.Append("null,");
            }
            else if (value == string.Empty)
            {
                // NEEDED?????
                sb.Append("\"\",");
            }
            else if (needQuotes)
            {
                sb.AppendFormat("\"{0}\",", value);
            }
            else
            {
                sb.AppendFormat("{0},", value);
            }            
        }  

        /// <summary>
        /// Add empty Object
        /// </summary>
        /// <param name="sb"Stringbuilder></param>
        /// <param name="level">Level</param>
        /// <param name="Key">Key</param>
        internal static void AddEmptyObject(StringBuilder sb, int level, string? key = null)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                sb.Append("{},");            
            }
            else
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                sb.AppendFormat("\"{0}\": {{}},", key);
            }
        }        

        /// <summary>
        /// Add empty list
        /// </summary>
        /// <param name="sb"Stringbuilder></param>
        /// <param name="level">Level</param>
        /// <param name="Key">Key</param>
        internal static void AddEmptyList(StringBuilder sb, int level, string? key = null)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                sb.Append("[],");            
            }
            else
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
                sb.AppendFormat("\"{0}\": [],", key);
            }
        }

        /// <summary>
        /// Adds primitive list
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">ListNode</param>
        /// <param name="options">Options</param>
        internal static void AddPrimitiveList(StringBuilder sb, ListNode node, JsonWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            if (options.AddKey)
            {
                // Add Key
                sb.AppendFormat("\"{0}\": ", node.Key);
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
                else if (needQuotes)
                {
                    sb.Append(CommonConstants.Quote);
                    sb.Append(val);
                    sb.Append(CommonConstants.Quote);
                }
                else
                {
                    sb.Append(val);
                }

                sb.Append(CommonConstants.CommaAndWhiteSpace);
            }

            // Remove last ", "
            sb.Remove(sb.Length - 2, 2);

            sb.Append(JsonConstants.ListEnd);
            sb.Append(CommonConstants.Comma);
        }
    }
}