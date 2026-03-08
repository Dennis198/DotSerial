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
using DotSerial.Common;
using DotSerial.Utilities;
using DotSerial.Tree.Nodes;
using DotSerial.Tree.Creation;

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
        internal static void AddObjectStart(StringBuilder sb, string key, int level, string? prefix)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new NotImplementedException();
            }

            // Make sure key:value has its own line.            
            sb.AppendLine();

            WriteMethods.AddIndentation(sb, level, YamlConstants.IndentationSize);
            key = key.EscapeChars(YamlConstants.CharsToEscape);
            if (_nodeFactory.AreQuotesNeededForKey(StategyType.Yaml, key))
            {
                key = StringMethods.AddStartAndEndQuotes(key);
            }

            if (string.IsNullOrWhiteSpace(prefix))
            {
                sb.AppendFormat("{0}:", key);
            }
            else
            {
                sb.AppendFormat("{0}{1}:", prefix, key);
            }
        }

        /// <summary>
        /// Converts a key : value pair to an yaml string.
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="level">Indentation level</param>
        /// <param name="needQuotes">True, if value needs quotes</param>
        /// <param name="prefix">Key-Prefix</param>
        internal static void AddKeyValuePair(StringBuilder sb, string key, string? value, int level, bool needQuotes, string? prefix = null)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (null == key || key.Length == 0)
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            // Make sure key:value has its own line.
            sb.AppendLine();

            WriteMethods.AddIndentation(sb, level, YamlConstants.IndentationSize);

            if (null != prefix)
            {
                sb.AppendFormat("{0}", prefix);
            }

            key = key.EscapeChars(YamlConstants.CharsToEscape);
            if (_nodeFactory.AreQuotesNeededForKey(StategyType.Yaml, key))
            {
                key = StringMethods.AddStartAndEndQuotes(key);
            }

            if (null == value)
            {
                sb.AppendFormat("{0}: null", key);
            }
            else
            {
                value = value.EscapeChars(YamlConstants.CharsToEscape);
                if (needQuotes)
                {
                    value = StringMethods.AddStartAndEndQuotes(value);
                }
                sb.AppendFormat("{0}: {1}", key, value);
            }
        }   

        /// <summary>
        /// Converts a primitive list into yaml
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Indentation level</param>
        internal static void AddPrimitiveList(StringBuilder sb, ListNode node, int level, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            sb.AppendLine();
            bool skipFirstIndentation = false;

            if (!string.IsNullOrWhiteSpace(options.GetPrefix()))
            {
                WriteMethods.AddIndentation(sb, level, YamlConstants.IndentationSize);
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
                        WriteMethods.AddIndentation(sb, level, YamlConstants.IndentationSize);
                    }
                    skipFirstIndentation = false;

                    if (null == val)
                    {
                        sb.AppendFormat("- {0}", CommonConstants.Null);
                    }
                    else
                    {
                        val = val.EscapeChars(YamlConstants.CharsToEscape);
                        if (needQuotes)
                        {
                            val = StringMethods.AddStartAndEndQuotes(val);
                        }
                        sb.AppendFormat("- {0}", val);
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

        /// <summary>
        /// Add empty Object
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        /// <param name="prefix">Key-Prefix</param>
        internal static void AddEmptyObject(StringBuilder sb, string? key, int level, string? prefix)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (false == string.IsNullOrWhiteSpace(key))
            {
                AddObjectStart(sb, key, level, prefix);
                sb.Append(" {}");
            }
            else
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, YamlConstants.IndentationSize);
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
        internal static void AddEmptyList(StringBuilder sb, string? key, int level, string? prefix)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (false == string.IsNullOrWhiteSpace(key))
            {
                AddObjectStart(sb, key, level, prefix);
                sb.Append(" []");
            }
            else
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, YamlConstants.IndentationSize);
                sb.Append("[]");
            }            
        }   

        /// <summary>
        /// Appends only the value without the key
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="value">Value</param>
        /// <param name="level">Level</param>
        /// <param name="needQuotes">True, if value needs quotes</param>
        /// <param name="prefix">Prefix</param>
        internal static void AddOnlyValue(StringBuilder sb, string? value, int level, bool needQuotes, string? prefix = null)
        {
            ArgumentNullException.ThrowIfNull(sb);

            // Maku sure that Value pair is in new line
            sb.AppendLine();

            WriteMethods.AddIndentation(sb, level, YamlConstants.IndentationSize);

            if (null != prefix)
            {
                sb.AppendFormat("{0}", prefix);
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
                sb.AppendFormat("{0}", value);
            }
        }                

    }
}