using System.Text;
using DotSerial.Common;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Toon.Writer
{
    internal static class ToonWriterHelper
    {
        /// <summary>
        /// Converts a key : value pair to an toon string.
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="level">Indentation level</param>
        /// <param name="needQuotes">True, if value needs quotes</param>
        /// <param name="prefix">Prefix for key</param>
        internal static void AddKeyValuePair(StringBuilder sb, string key, string? value, int level, bool needQuotes, string? prefix = null)
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

            if (KeyNeedQuotes(key))
            {
                key = key.AddQuotesAtStartAndEnd();
            }

            sb.AppendFormat("{0}: ", key);

            if (null == value)
            {
                sb.Append(CommonConstants.Null);
            }
            else
            {
                if (needQuotes)
                {
                    sb.AppendFormat("\"{0}\"", value);
                }
                else
                {
                    sb.AppendFormat("{0}", value);
                }
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

            WriteMethods.AddIndentation(sb, level, ToonConstants.IndentationSize);

            if (null != prefix)
            {
                sb.AppendFormat("{0}", prefix);
            }

            if (null == value)
            {
                sb.Append(CommonConstants.Null);
            }
            else if (value == string.Empty)
            {
                sb.Append("\"\"");
            }
            else if (needQuotes)
            {
                sb.AppendFormat("\"{0}\"", value);
            }
            else
            {
                sb.AppendFormat("{0}", value);
            }              
        }           

        /// <summary>
        /// Helper methode to add object start to toon
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        /// <param name="prefix">Prefix</param>
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

            if (KeyNeedQuotes(key))
            {
                key = key.AddQuotesAtStartAndEnd();
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
        /// Helper methode to add list start to toon
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="lNode">List node</param>
        /// <param name="level">Indentation level</param>
        /// <param name="addKey">True, to add key</param>
        /// <param name="prefix">Prefix</param>
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

            if (key != string.Empty && KeyNeedQuotes(key))
            {
                key = key.AddQuotesAtStartAndEnd();
            }

            sb.AppendLine();
            WriteMethods.AddIndentation(sb, level, ToonConstants.IndentationSize);

            if (string.IsNullOrWhiteSpace(prefix))
            {
                if (addKey)
                    sb.AppendFormat("{0}[{1}]", key, count);
                else
                    sb.AppendFormat("[{0}]", count);
            }
            else
            {
                if (addKey)
                    sb.AppendFormat("{0}{1}[{2}]", prefix, key, count);
                else
                    sb.AppendFormat("{0}[{1}]", prefix, count);

            }       

            if (UseToonSchema(lNode, out string? schema))
            {
                sb.Append(schema);
            }

            sb.Append(": ");
        }

        /// <summary>
        /// Helper method to add a list with Toon Schema
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="node">ListNode</param>
        /// <param name="level">Indentation level</param>
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
                    else
                    {
                        if (chilChild.IsQuoted)
                        {
                            sb.AppendFormat("\"{0}\"", value);
                        }
                        else
                        {
                            sb.AppendFormat("{0}", value);
                        }
                    }

                    sb.Append(CommonConstants.Comma);
                }

                // Remove last ","
                sb.Remove(sb.Length - 1, 1);
            }
        }        

        /// <summary>
        /// Helper method to add a primitive list to toon
        /// </summary>
        /// <param name="sb">StinrgBuilder</param>
        /// <param name="node">ListNode</param>
        internal static void AddPrimitiveList(StringBuilder sb, ListNode node)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            // Get all children of node
            var children = node.GetChildren();

            foreach (var child in children)
            {
                string? val = child.GetValue();
                if (null == val)
                {
                    sb.Append(CommonConstants.Null);
                }
                else
                {
                    if (child.IsQuoted)
                    {
                        sb.AppendFormat("\"{0}\"", val);
                    }
                    else
                    {
                        sb.AppendFormat("{0}", val);
                    }
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
            }
            else
            {
                sb.AppendLine();
                WriteMethods.AddIndentation(sb, level, ToonConstants.IndentationSize);
            }
        }     

        /// <summary>
        /// Check if listNode can be written with Toon Schema. If so,
        ///  the schema is returned in the out parameter.
        /// </summary>
        /// <param name="listNode">ListNode</param>
        /// <param name="keys">Schema keys</param>
        /// <returns>True, if schema should be used</returns>
        internal static bool UseToonSchema(ListNode listNode, out string? keys)
        {
            keys = null;

            if (!listNode.HasChildren())
            {
                return false;
            }

            List<string> keysTmp = [];
            bool firstIt = true;
            
            var children = listNode.GetChildren();
            keys = string.Empty;

            foreach(var child in children)
            {

                if (child is InnerNode)
                {
                     var childChidlren = child.GetChildren();

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

            for (int i = 0; i < keysTmp.Count; i++)
            {
                if (KeyNeedQuotes(keysTmp[i]))
                {
                    keys += "\"" + keysTmp[i] + "\"" + ",";
                }
                else
                {
                    keys += keysTmp[i] + ",";
                }
            }

            // Remove last ','
            keys = keys[..^1];
            // Wrap list of keys in {}
            keys = string.Format("{{{0}}}", keys);

            return true;
        }

        private static bool KeyNeedQuotes(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new NotImplementedException();
            }   

            if (key.HaveLeadingOrTrailingWhitespace())
            {
                return true;
            }

            if (key.IsNumericValue())
            {
                return false;
            }

            for(int i = 0; i < key.Length; i++)
            {
                char c = key[i];
                if (ToonConstants.ToonSpecialChars.Contains(c))
                    return true;
            }    

            return false;
        }           
    }
}