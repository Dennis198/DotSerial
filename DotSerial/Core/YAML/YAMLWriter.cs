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

using DotSerial.Core.Exceptions.Node;
using DotSerial.Core.Exceptions.YAML;
using DotSerial.Core.General;
using System.Text;

namespace DotSerial.Core.YAML
{
    /// <summary>
    /// Class which parses Yaml content
    /// </summary>
    internal static class YAMLWriter
    {
        /// <summary>
        /// Converts Yaml string to a Yaml node
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>Yaml node</returns>
        public static string ToYamlString(DSNode? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            StringBuilder sb = new();

            // Add document start
            sb.Append(YAMLConstants.YAMLDocumentStart);

            // Starts the convertion to a yaml string
            NodeToYaml(sb, node, -1);

            // Add document end
            sb.AppendLine();
            sb.Append(YAMLConstants.YAMLDocumentEnd);

            return sb.ToString();
        }

        /// <summary>
        /// Converts node to yaml
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        /// <param name="prefix">Key-Prefix</param>
        private static void NodeToYaml(StringBuilder sb, DSNode node, int level, string? prefix = null)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            if (node.Type == DSNodeType.InnerNode || node.Type == DSNodeType.Root)
            {
                // Inner node to yaml
                InnerNodeToYaml(sb, node, level + 1, prefix);
            }
            else if (node.Type == DSNodeType.Leaf)
            {
                // Leaf node to yaml
                LeafNodeToYaml(sb, node, level + 1);
            }
            else
            {
                throw new DSInvalidNodeTypeException(node.Type);
            }
        }

        /// <summary>
        /// Converts an inner node to yaml
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        /// <param name="prefix">Key-Prefix</param>
        private static void InnerNodeToYaml(StringBuilder sb, DSNode node, int level, string? prefix = null)
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
                KeyValuePairToYaml(sb, node.Key.ToString(), null, level);
                return;
            }

            if (node.PropType == DSNodePropertyType.Class)
            {
                ClassNodeToYaml(sb, node, level, prefix);
            }
            else if (node.PropType == DSNodePropertyType.List)
            {
                ListNodeToYaml(sb, node, level, prefix);
            }
            else if (node.PropType == DSNodePropertyType.Dictionary)
            {
                DictionaryNodeToYaml(sb, node, level);
            }
            else
            {
                throw new DSInvalidNodeTypeException(node.PropType);
            }
        }

        /// <summary>
        /// Converts an inner node to yaml
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        private static void LeafNodeToYaml(StringBuilder sb, DSNode node, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            // Check if type is not leaf => error
            if (node.Type != DSNodeType.Leaf)
            {
                throw new DSInvalidNodeTypeException(node.Type);
            }

            KeyValuePairToYaml(sb, node.Key.ToString(), node.Value, level);
        }

        /// <summary>
        /// Converts a class node to yaml
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        /// <param name="prefix">Key-Prefix</param>
        private static void ClassNodeToYaml(StringBuilder sb, DSNode node, int level, string? prefix = null)
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

            if (node.IsEmpty)
            {
                AddObjectStart(sb, node.Key.ToString(), level, prefix);

                if (false == string.IsNullOrWhiteSpace(prefix))
                {
                    level++;
                }

                // Add type info
                AddTypeInfo(sb, level, DSNodePropertyType.Class);
            }
            else
            {
                AddObjectStart(sb, node.Key.ToString(), level, prefix);

                if (false == string.IsNullOrWhiteSpace(prefix))
                {
                    level++;
                }

                // Add type info
                AddTypeInfo(sb, level, DSNodePropertyType.Class);

                var children = node.GetChildren();

                foreach (var keyValue in children)
                {
                    NodeToYaml(sb, keyValue, level);
                }
            }
        }

        /// <summary>
        /// Converts a list node to yaml
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        /// <param name="prefix">Key-Prefix</param>
        private static void ListNodeToYaml(StringBuilder sb, DSNode node, int level, string? prefix = null)
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
                AddObjectStart(sb, node.Key.ToString(), level, prefix);
                if (false == string.IsNullOrWhiteSpace(prefix))
                {
                    level++;
                }
                AddTypeInfo(sb, level, DSNodePropertyType.List);
                return;
            }

            // Check if list contains only primitive types
            if (node.IsPrimitiveList())
            {
                AddObjectStart(sb, node.Key.ToString(), level, prefix);

                if (false == string.IsNullOrWhiteSpace(prefix))
                {
                    level++;
                }

                // Add type info
                AddTypeInfo(sb, level, DSNodePropertyType.List);

                // List to yaml
                PrimitiveListToYaml(sb, node, level + 1);
            }
            else
            {
                AddObjectStart(sb, node.Key.ToString(), level, prefix);

                if (false == string.IsNullOrWhiteSpace(prefix))
                {
                    level++;
                }

                // Add type info
                AddTypeInfo(sb, level, DSNodePropertyType.List);

                sb.AppendLine();
                AddIndentation(sb, level + 1);
                sb.AppendFormat("\"{0}\":", node.Key);

                var children = node.GetChildren();

                StringBuilder sb2 = new();
                AddIndentation(sb2, level + 1);

                foreach (var keyValue in children)
                {
                    StringBuilder sb3 = new();
                    NodeToYaml(sb3, keyValue, level + 1, YAMLConstants.ListItemIndicator);
                    sb2.Append(sb3);
                }

                sb.Append(sb2);
            }
        }

        /// <summary>
        /// Converts a dictioanry to yaml
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        private static void DictionaryNodeToYaml(StringBuilder sb, DSNode node, int level, string? prefix = null)
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
                AddObjectStart(sb, node.Key.ToString(), level, prefix);
                if (false == string.IsNullOrWhiteSpace(prefix))
                {
                    level++;
                }
                AddTypeInfo(sb, level, DSNodePropertyType.Dictionary);
                return;
            }

            if (node.IsPrimitiveDictionary())
            {
                AddObjectStart(sb, node.Key.ToString(), level, prefix);

                if (false == string.IsNullOrWhiteSpace(prefix))
                {
                    level++;
                }

                AddTypeInfo(sb, level, DSNodePropertyType.Dictionary);

                StringBuilder sb2 = new();
                AddObjectStart(sb2, node.Key.ToString(), level + 1, prefix);

                for (int i = 0; i < node.Count; i++)
                {
                    string key = node.GetNthChild(i).Key;
                    var value = node.GetNthChild(i).GetFirstChild().Value;

                    KeyValuePairToYaml(sb2, key, value, level + 2);
                }

                sb.Append(sb2);
            }
            else
            {
                AddObjectStart(sb, node.Key.ToString(), level, prefix);

                if (false == string.IsNullOrWhiteSpace(prefix))
                {
                    level++;
                }

                AddTypeInfo(sb, level, DSNodePropertyType.Dictionary);

                StringBuilder sb2 = new();
                AddObjectStart(sb2, node.Key.ToString(), level + 1, prefix);

                List<DSNode> keyvaluePair = node.GetChildren();

                for (int i = 0; i < keyvaluePair.Count; i++)
                {
                    var keyValuePair = keyvaluePair[i];

                    if (false == node.IsNull)
                    {
                        StringBuilder sb3 = new();
                        DictionaryKeyValuePairToYaml(sb3, keyValuePair, level + 2);
                        sb2.Append(sb3);
                    }
                    else
                    {

                        throw new NullReferenceException();
                    }
                }

                sb.Append(sb2);
            }
        }       

        /// <summary>
        /// Converts a Dictonary KeyValuePair into yaml
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        private static void DictionaryKeyValuePairToYaml(StringBuilder sb, DSNode node, int level)
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

            ClassNodeToYaml(sb, node.GetFirstChild(), level);
        }

        /// <summary>
        /// Converts a primitive list into yaml
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Indentation level</param>
        private static void PrimitiveListToYaml(StringBuilder sb, DSNode node, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            sb.AppendLine();
            AddIndentation(sb, level);

            // Add Key
            sb.AppendFormat("\"{0}\":", node.Key);
            sb.AppendLine();

            // Get all children of node
            var children = node.GetChildren();

            foreach (var keyValue in children)
            {
                string? val = keyValue.IsNull ? YAMLConstants.Null : keyValue.Value;

                AddIndentation(sb, level + 1);

                if (keyValue.IsNull)
                {
                    sb.AppendFormat("- {0}", YAMLConstants.Null);
                }
                else
                {
                    sb.AppendFormat("- \"{0}\"", val);
                }               

                sb.AppendLine();
            }

            // Remove last New Line
            sb.Remove(sb.Length - 1, 1);
        }

        /// <summary>
        /// Helper methode to add object start to yaml
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        /// <param name="prefix">Key-Prefix</param>
        private static void AddObjectStart(StringBuilder sb, string key, int level, string? prefix)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new DSInvalidYAMLException();
            }

            sb.AppendLine();
            AddIndentation(sb, level);

            if (string.IsNullOrWhiteSpace(prefix))
            {
                sb.AppendFormat("\"{0}\":", key);
            }
            else
            {
                sb.AppendFormat("{0} \"{1}\":", prefix, key);
            }

        }

        /// <summary>
        /// Converts a key : value pair to an yaml string.
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="level">Indentation level</param>

        private static void KeyValuePairToYaml(StringBuilder sb, string key, string? value, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new DSInvalidYAMLException();
            }

            // Make sure key:value has its own line.
            sb.AppendLine();

            AddIndentation(sb, level);

            if (null == value)
            {
                sb.AppendFormat("\"{0}\": null", key);
            }
            else if (value == string.Empty)
            {
                sb.AppendFormat("\"{0}\": \"{{}}\"", key);
            }
            else
            {
                sb.AppendFormat("\"{0}\": \"{1}\"", key, value);
            }
        }
        
        /// <summary>
        /// Adds the node proprty type info to an jyaml object
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="type">Node property type</param>
        private static void AddTypeInfo(StringBuilder sb, int level, DSNodePropertyType type)
        {
            ArgumentNullException.ThrowIfNull(sb);
            string typeName = type.ConvertToString();
            KeyValuePairToYaml(sb, YAMLConstants.PropertyTypeKey, typeName, level + 1);
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

            sb.Append(YAMLConstants.WhiteSpace, level * YAMLConstants.IndentationSize);
        }        
    }
}