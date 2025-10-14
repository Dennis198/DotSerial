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

using DotSerial.Core.Exceptions;
using DotSerial.Core.Exceptions.JSON;
using DotSerial.Core.Exceptions.Node;
using DotSerial.Core.General;
using System.Text;

namespace DotSerial.Core.JSON
{
    /// <summary>
    /// Class can converts a node to a json string
    /// </summary>
    internal static class JSONWriter
    {
        /// <summary>
        /// Converts a node into an json string.
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>Json string</returns>
        public static string ToJsonString(DSNode? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            StringBuilder sb = new();

            // Add First '{'
            sb.Append(JsonConstants.ObjectStart);

            // Starts the convertion to a json string
            NodeToJson(sb, node, 0);

            sb.Remove(sb.Length - 1, 1);

            // Add Last '}'
            AddObjectEnd(sb, 0, true);

            return sb.ToString();
        }

        /// <summary>
        /// Converts node to json
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        /// <param name="addKey">True if key should be added to json</param>
        private static void NodeToJson(StringBuilder sb, DSNode node, int level, bool addKey = true)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            if (node.Type == DSNodeType.InnerNode || node.Type == DSNodeType.Root)
            {
                // Inner node to json
                InnerNodeToJson(sb, node, level + 1, addKey);
            }
            else if (node.Type == DSNodeType.Leaf)
            {
                // Leaf node to json
                LeafNodeToJson(sb, node, level + 1);
            }
            else
            {
                throw new DSInvalidNodeTypeException(node.Type);
            }
        }

        /// <summary>
        /// Converts an inner node to json
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        /// <param name="addKey">True if key should be added to json</param>
        private static void InnerNodeToJson(StringBuilder sb, DSNode node, int level, bool addKey = true)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            // Check if type is leaf => error
            if (node.Type == DSNodeType.Leaf)
            {
                throw new DSInvalidNodeTypeException(node.Type);
            }

            if (node.IsNull)
            {
                KeyValuePairToJson(sb, node.Key.ToString(), null, level);
                return;
            }

            if (node.PropType == DSNodePropertyType.Class)
            {
                ClassNodeToJson(sb, node, level, addKey);
            }
            else if (node.PropType == DSNodePropertyType.List)
            {
                ListNodeToJson(sb, node, level, addKey);
            }
            else if (node.PropType == DSNodePropertyType.Dictionary)
            {
                DictionaryNodeToJson(sb, node, level);
            }
            else
            {
                throw new DSInvalidNodeTypeException(node.PropType);
            }
        }

        /// <summary>
        /// Converts an inner node to json
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        private static void LeafNodeToJson(StringBuilder sb, DSNode node, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            // Check if type is not leaf => error
            if (node.Type != DSNodeType.Leaf)
            {
                throw new DSInvalidNodeTypeException(node.Type);
            }

            KeyValuePairToJson(sb, node.Key.ToString(), node.Value, level);
        }

        /// <summary>
        /// Converts a Dictonary KeyValuePair into json
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        private static void DictionaryKeyValuePairToJson(StringBuilder sb, DSNode node, int level)
        {
            /// (A)   (B)   (C) (KeyValuePairs)
            ///  :     |     :
            ///  :    (D)    :  (Value of KeyvaluePairs)
            
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            if (node.PropType != DSNodePropertyType.KeyValuePair)
            {
                throw new DSInvalidNodeTypeException(node.PropType);
            }

            ClassNodeToJson(sb, node.GetFirstChild(), level, true);
        }

        /// <summary>
        /// Converts a dictioanry to json
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        private static void DictionaryNodeToJson(StringBuilder sb, DSNode node, int level)
        {
            ///     (node) (Dictionary)
            ///       |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (KeyValuePairs)
            ///  :     |     :
            ///  :    (D)    :  (Value of KeyvaluePairs)
            
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            if (node.PropType != DSNodePropertyType.Dictionary)
            {
                throw new DSInvalidNodeTypeException(node.PropType);
            }

            if (node.IsEmpty)
            {
                AddObjectStart(sb, node.Key.ToString(), level);
                AddTypeInfo(sb, level, DSNodePropertyType.Dictionary);
                AddObjectEnd(sb, level);
                return;
            }

            if (node.IsPrimitiveDictionary())
            {
                AddObjectStart(sb, node.Key.ToString(), level);
                AddTypeInfo(sb, level, DSNodePropertyType.Dictionary);

                StringBuilder sb2 = new();
                AddObjectStart(sb2, node.Key.ToString(), level + 1);

                for (int i = 0; i < node.Count; i++)
                {
                    string key = node.GetNthChild(i).Key;
                    var value = node.GetNthChild(i).GetFirstChild().Value;

                    KeyValuePairToJson(sb2, key, value, level + 1);
                }

                sb2.Remove(sb2.Length - 1, 1);
                AddObjectEnd(sb2, level + 1, true);

                sb.Append(sb2);
                AddObjectEnd(sb, level);
            }
            else
            {
                AddObjectStart(sb, node.Key.ToString(), level);
                AddTypeInfo(sb, level, DSNodePropertyType.Dictionary);

                StringBuilder sb2 = new();
                AddObjectStart(sb2, node.Key.ToString(), level + 1);

                List<DSNode> keyvaluePair = node.GetChildren();

                for (int i = 0; i < keyvaluePair.Count; i++)
                {
                    var keyValuePair = keyvaluePair[i];

                    if (false == node.IsNull)
                    {
                        StringBuilder sb3 = new();
                        DictionaryKeyValuePairToJson(sb3, keyValuePair, level + 2);
                        sb2.Append(sb3);
                    }
                    else
                    {

                        throw new NullReferenceException();
                    }
                }

                sb2.Remove(sb2.Length - 1, 1);
                AddObjectEnd(sb2, level + 1, true);

                sb.Append(sb2);
                AddObjectEnd(sb, level);
            }
        }

        /// <summary>
        /// Converts a list node to Json
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        /// <param name="addKey">True if key should be added to json</param>
        private static void ListNodeToJson(StringBuilder sb, DSNode node, int level, bool addKey = true)
        {
            ///      (node) (List)
            ///        |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (Items)
            
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            // Check if type is list => error
            if (node.PropType != DSNodePropertyType.List)
            {
                throw new DSInvalidNodeTypeException(node.PropType);
            }

            // Check if list is empty
            if (node.IsEmpty)
            {
                AddObjectStart(sb, node.Key.ToString(), level);
                AddTypeInfo(sb, level, DSNodePropertyType.List);
                AddObjectEnd(sb, level);
                return;
            }

            // Check if list contains only primitive types
            if (node.IsPrimitiveList())
            {
                if (addKey)
                {
                    AddObjectStart(sb, node.Key.ToString(), level);
                }
                else
                {
                    sb.AppendLine();
                    AddIndentation(sb, level);
                    sb.Append(JsonConstants.ObjectStart);
                }

                // Add type info
                AddTypeInfo(sb, level, DSNodePropertyType.List);

                // List to json
                PrimitiveListToJson(sb, node, level + 1);

                AddObjectEnd(sb, level);
            }
            else
            {
                if (addKey)
                {
                    AddObjectStart(sb, node.Key.ToString(), level);
                }
                else
                {
                    sb.AppendLine();
                    AddIndentation(sb, level);
                    sb.Append(JsonConstants.ObjectStart);
                }

                // Add type info
                AddTypeInfo(sb, level, DSNodePropertyType.List);

                sb.AppendLine();
                AddIndentation(sb, level + 1);
                sb.AppendFormat("\"{0}\": ", node.Key);                

                var children = node.GetChildren();

                StringBuilder sb2 = new();
                sb2.AppendLine();
                AddIndentation(sb2, level + 1);
                sb2.Append(JsonConstants.ListStart);

                foreach (var keyValue in children)
                {
                    StringBuilder sb3 = new();
                    NodeToJson(sb3, keyValue, level + 1, false);
                    sb2.Append(sb3);
                }

                sb2.Remove(sb2.Length - 1, 1);
                sb2.AppendLine();
                AddIndentation(sb2, level + 1);
                sb2.Append(JsonConstants.ListEnd);


                sb.Append(sb2);
                AddObjectEnd(sb, level);
            }
        }

        /// <summary>
        /// Converts a class node to Json
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        /// <param name="addKey">True if key should be added to json</param>
        private static void ClassNodeToJson(StringBuilder sb, DSNode node, int level, bool addKey = true)
        {
            ///      (node) (Class)
            ///        |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (Properties)
            
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            // Check if type is leaf => error
            if (node.Type == DSNodeType.Leaf)
            {
                throw new DSInvalidNodeTypeException(node.Type);
            }

            if (node.IsNull)
            {
                sb.AppendFormat("\"{0}\": \"null\",", node.Key);
            }
            else if (node.IsEmpty)
            {
                AddObjectStart(sb, node.Key.ToString(), level);

                // Add type info
                AddTypeInfo(sb, level, DSNodePropertyType.Class);

                // Remove last ','
                sb.Remove(sb.Length - 1, 1);

                AddObjectEnd(sb, level);
            }
            else
            {
                if (addKey)
                {
                    AddObjectStart(sb, node.Key.ToString(), level);
                }
                else
                {
                    AddIndentation(sb, level);
                    sb.AppendLine();
                    AddIndentation(sb, level);
                    sb.Append('{');
                }

                // Add type info
                AddTypeInfo(sb, level, DSNodePropertyType.Class);

                var children = node.GetChildren();

                foreach(var keyValue in children)
                {
                    NodeToJson(sb, keyValue, level);
                }

                // Remove last ','
                sb.Remove(sb.Length - 1, 1);

                AddObjectEnd(sb, level);
            }
        }

        /// <summary>
        /// Adds the node proprty type info to an json object
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="type">Node property type</param>
        private static void AddTypeInfo(StringBuilder sb, int level, DSNodePropertyType type)
        {
            ArgumentNullException.ThrowIfNull(sb);
            string typeName = type.ConvertToString();
            KeyValuePairToJson(sb, JsonConstants.PropertyTypeKey, typeName, level + 1);
        }

        /// <summary>
        /// Helper methode to add object start symbol and to json
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        private static void AddObjectStart(StringBuilder sb, string key, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new DSInvalidJSONException(key);
            }

            sb.AppendLine();
            AddIndentation(sb, level);
            sb.AppendFormat("\"{0}\": {{", key);
        }

        /// <summary>
        /// Helper methode to add object end symbol and to json
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="isLastObject">True, if object is last object</param>
        private static void AddObjectEnd(StringBuilder sb, int level, bool isLastObject = false)
        {
            ArgumentNullException.ThrowIfNull(sb);

            sb.AppendLine();
            AddIndentation(sb, level);

            if (isLastObject)
            {
                sb.Append(JsonConstants.ObjectEnd);
            }
            else
            {
                sb.Append("},");
            }
        }

        /// <summary>
        /// Converts a primitive list into json
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Indentation level</param>
        private static void PrimitiveListToJson(StringBuilder sb, DSNode node, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            sb.AppendLine();
            AddIndentation(sb, level);

            // Add Key
            sb.AppendFormat("\"{0}\": ", node.Key);

            // Get all children of node
            var children = node.GetChildren();

            sb.Append(JsonConstants.ListStart);

            foreach (var keyValue in children)
            {
                string? val = keyValue.IsNull ? JsonConstants.Null : keyValue.Value;

                sb.Append(JsonConstants.Quote);
                sb.Append(val);
                sb.Append(JsonConstants.Quote);

                sb.Append(", ");
            }

            // Remove last ", "
            sb.Remove(sb.Length - 2, 2);

            sb.Append(JsonConstants.ListEnd);
        }

        /// <summary>
        /// Converts a key : value pair to an json string.
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="level">Indentation level</param>
        private static void KeyValuePairToJson(StringBuilder sb, string key, string? value, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new DSInvalidJSONException(key);
            }

            // Make sure key:value has its own line.
            sb.AppendLine();

            AddIndentation(sb, level);

            if (null == value)
            {
                sb.AppendFormat("\"{0}\": \"null\",", key);
            }
            else if (value == string.Empty)
            {
                sb.AppendFormat("\"{0}\": \"{{}}\",", key);
            }
            else
            {
                sb.AppendFormat("\"{0}\": \"{1}\",", key, value);
            }
        }

        /// <summary>
        /// Adds identation
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="level">Level</param>
        private static void AddIndentation(StringBuilder sb, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (level < 0)
            {
                throw new ArgumentException(nameof(level));
            }

            sb.Append(JsonConstants.WhiteSpace, level * JsonConstants.IndentationSize);
        }

    }
}
