
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Toon.Writer
{
    internal static class ToonWriterHelper
    {
        /// <summary>
        /// Converts a key : value pair to an yaml string.
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="level">Indentation level</param>
        internal static void AddKeyValuePair(StringBuilder sb, string key, string? value, int level, string? prefix = null)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new NotImplementedException();
            }

            // Make sure key:value has its own line.
            sb.AppendLine();

            WriteMethods.AddIndentation(sb, level, ToonConstants.IndentationSize);

            if (null != prefix)
            {
                sb.AppendFormat("{0}", prefix);
            }

            sb.AppendFormat("{0}: ", key);

            if (null == value)
            {
                sb.Append(CommonConstants.Null);
            }
            if(UseQuotes(value))
            {
                sb.AppendFormat("\"{0}\"", value);
            }
            else 
            {
                sb.Append(value);
            }
            // else if (value == string.Empty)
            // {
            //     // todo RICHTIG?
            //     sb.AppendFormat("{0}: \"\"", key);
            // }
            // else
            // {
            //     sb.AppendFormat("{0}: {1}", key, value);
            // }
        }  

        /// <summary>
        /// Appends only the value without the key
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="value">Value</param>
        /// <param name="level">Level</param>
        internal static void AddOnlyValue(StringBuilder sb, string? value, int level, string? prefix = null)
        {
            ArgumentNullException.ThrowIfNull(sb);

            // Maku sure that Value pair is in new line
            sb.AppendLine();

            WriteMethods.AddIndentation(sb, level, ToonConstants.IndentationSize);

            if (null != prefix)
            {
                sb.AppendFormat("{0}", prefix);
            }

            // if (null == value)
            // {
            //     sb.Append("null");
            // }
            // else if (value == string.Empty)
            // {
            //     sb.Append("\"\"");
            // }
            // else
            // {
            //     sb.AppendFormat("\"{0}\"", value);
            // }

            if (null == value)
            {
                sb.Append(CommonConstants.Null);
            }
            if(UseQuotes(value))
            {
                sb.AppendFormat("\"{0}\"", value);
            }
            else 
            {
                sb.Append(value);
            }            
        }           

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
            WriteMethods.AddIndentation(sb, level, ToonConstants.IndentationSize);

            if (string.IsNullOrWhiteSpace(prefix))
            {
                sb.AppendFormat("\"{0}\":", key);
            }
            else
            {
                sb.AppendFormat("{0}\"{1}\":", prefix, key);
            }
        }    

        internal static void AddListStart(StringBuilder sb, ListNode lNode, int level, bool addKey, string? prefix)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(lNode);

            string key = lNode.Key;

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new NotImplementedException();
            }

            key = addKey ? key : string.Empty;

            int count = lNode.GetChildren().Count;

            sb.AppendLine();
            WriteMethods.AddIndentation(sb, level, ToonConstants.IndentationSize);

            if (string.IsNullOrWhiteSpace(prefix))
            {
                sb.AppendFormat("{0}[{1}]", key, count);
            }
            else
            {
                sb.AppendFormat("{0}{1}[{2}]", prefix, key, count);
            }       

            if (UseToonSchema(lNode, out string schema))
            {
                sb.Append(schema);
            }

            sb.Append(": ");
        }

        internal static void AddSchemaList(StringBuilder sb, ListNode node, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            // Get all children of node
            var children = node.GetChildren();

            foreach(var child in children)
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, ToonConstants.IndentationSize);

                var childChidlren = child.GetChildren();                
                foreach(var chilChild in childChidlren)
                {
                    string? value = chilChild.GetValue();
                    if (null == value)
                    {
                        sb.Append(CommonConstants.Null);
                    }
                    if(UseQuotes(value))
                    {
                        sb.AppendFormat("\"{0}\"", value);
                    }
                    else 
                    {
                        sb.Append(value);
                    }  

                    sb.Append(CommonConstants.Comma);
                }

                // Remove last ","
                sb.Remove(sb.Length - 1, 1);
            }
        }        

        internal static void AddPrimitiveList(StringBuilder sb, ListNode node)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            // Get all children of node
            var children = node.GetChildren();

            foreach (var child in children)
            {
                string? val = child.GetValue();

                // if (null == val)
                // {
                //     sb.Append(CommonConstants.Null);
                // }
                // else
                // {
                //     sb.Append(CommonConstants.Quote);
                //     sb.Append(val);
                //     sb.Append(CommonConstants.Quote);
                // }
                if (null == val)
                {
                    sb.Append(CommonConstants.Null);
                }
                if(UseQuotes(val))
                {
                    sb.AppendFormat("\"{0}\"", val);
                }
                else 
                {
                    sb.Append(val);
                }                 

                sb.Append(CommonConstants.Comma);
            }            

            // Remove last ","
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
                WriteMethods.AddIndentation(sb, level, ToonConstants.IndentationSize);
                sb.Append("{}");
            }
        }           

        private static bool UseQuotes(string? value)
        {
            if (null == value)
            {
                return false;
            }
            
            if (value == string.Empty)
            {
                return true;
            }

            if (value.Equals(CommonConstants.Null))
            {
                return true;
            }
            if (value.Equals(CommonConstants.TrueString))
            {
                return true;
            }       
            if (value.Equals(CommonConstants.FalseString))
            {
                return true;
            }                 

            char firstChar =  value[0];
            char lastChar = value[^1];

            // '-'
            if (firstChar == CommonConstants.Minus)
            {
                return true;
            }
            // Leading/Trailing whitespapce
            if (firstChar == CommonConstants.WhiteSpace || lastChar == CommonConstants.WhiteSpace)
            {
                return true;
            }
            // array delimiter ','
            if (value.Contains(CommonConstants.Comma))
            {
                return true;
            }            
            // ':'
            if (value.Contains(ToonConstants.KeyValueSeperator))
            {
                return true;
            }
            // '"'
            if (value.Contains(CommonConstants.Quote))
            {
                return true;
            }       
            // '\'
            if (value.Contains(CommonConstants.Backslash))
            {
                return true;
            }     
            // '{', '}'
            if (value.Contains(CommonConstants.BracesStart) || value.Contains(CommonConstants.BracesEnd))
            {
                return true;
            }       
            // '[', ']'
            if (value.Contains(CommonConstants.BracketsStart) || value.Contains(CommonConstants.BracketsEnd))
            {
                return true;
            }       
            // NewLine
            if (value.Contains(Environment.NewLine))
            {
                return true;
            }  
            // Is Number     
            if(UsingCompiledRegex(value))
            {
                return true;
            }               

            return false;                  
        }

        // [GeneratedRegex(@"^-?[0-9]+(?:\.[0-9]+)?$")]
        // private static partial Regex IsDigitRegex();
        public static bool UsingCompiledRegex(string stringValue)
        {
            var pattern = @"^-?[0-9]+(?:\.[0-9]+)?$"; 
            var regex = new Regex(pattern);
            return regex.IsMatch(stringValue);
            // return IsDigitRegex().IsMatch(stringValue); 
        }        

        internal static bool UseToonSchema(ListNode listNode, out string? keys)
        {
            keys = null;

            if (!listNode.HasChildren())
            {
                return false;
            }

            List<string> keysTmp = new();
            bool firstIt = true;
            
            var children = listNode.GetChildren();
            keys = string.Empty;

            foreach(var child in children)
            {
                // TODO Sonder fall null => LeafNode

                if (child is InnerNode)
                {
                     var childChidlren = child.GetChildren();
                    // foreach (var carchildChild in childChidlren)
                    // {
                    //     if (carchildChild is not LeafNode)
                    //     {
                    //         keys = null;
                    //         return false;
                    //     }

                    //     keys += keys + carchildChild.Key + ",";
                    // }

                    for (int i = 0; i < childChidlren.Count; i++)
                    {
                        if (childChidlren[i] is not LeafNode carchildChild)
                        {
                            keys = null;
                            return false;
                        }

                        string keyValue = carchildChild.Key;
                        if (firstIt)
                        {
                            keysTmp.Add(keyValue);
                        }
                        else
                        {
                            if (false == keysTmp[i].Equals(keyValue))
                            {
                                keys = null;
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    keys = null;
                    return false;
                }

                firstIt  = false;
            }

            // Remove last ';'
            for (int i = 0; i < keysTmp.Count; i++)
            {
                keys += keysTmp[i] + ",";
            }

            keys = keys[..^1];
            keys = string.Format("{{{0}}}", keys);

            return true;
        }
    }
}