using DotSerial.Core.Exceptions.Node;
using DotSerial.Core.General;
using System.Text;

namespace DotSerial.Core.YAML
{
    /// <summary>
    /// Class which parses Yaml content
    /// </summary>
    internal static class YAMLWriter
    {
        // https://docs.ansible.com/ansible/latest/reference_appendices/YAMLSyntax.html
        // https://www.yamllint.com/

        /// <summary>
        /// Converts Yaml string to a Yaml node
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>Yaml node</returns>
        public static string ToYamlString(DSNode? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            StringBuilder sb = new();

            sb.Append("---");

            // Starts the convertion to a json string
            NodeToYaml(sb, node, -1);

            sb.AppendLine();
            sb.Append("...");

            return sb.ToString();
        }

        private static void NodeToYaml(StringBuilder sb, DSNode node, int level, string? prefix = null)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            if (node.Type == DSNodeType.InnerNode || node.Type == DSNodeType.Root)
            {
                // Inner node to json
                InnerNodeToYaml(sb, node, level + 1, prefix);
            }
            else if (node.Type == DSNodeType.Leaf)
            {
                // Leaf node to json
                LeafNodeToYaml(sb, node, level + 1);
            }
            else
            {
                throw new DSInvalidNodeTypeException(node.Type);
            }
        }        

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
                // DictionaryNodeToJson(sb, node, level);
            }
            else
            {
                throw new DSInvalidNodeTypeException(node.PropType);
            }
        }

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
                // if (addKey)
                // {
                AddObjectStart(sb, node.Key.ToString(), level, prefix);
                // }
                // else
                // {
                //     AddIndentation(sb, level);
                //     sb.AppendLine();
                //     AddIndentation(sb, level);
                //     sb.Append('{');
                // }

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
        /// Converts a list node to Json
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="node">Node</param>
        /// <param name="level">Level/Height of node in tree</param>
        /// <param name="addKey">True if key should be added to json</param>
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
                // if (addKey)
                // {
                AddObjectStart(sb, node.Key.ToString(), level, prefix);
                // }
                // else
                // {
                //     sb.AppendLine();
                //     AddIndentation(sb, level);
                //     sb.Append(JsonConstants.ObjectStart);
                // }

                if (false == string.IsNullOrWhiteSpace(prefix))
                {
                    level++;
                }

                // Add type info
                AddTypeInfo(sb, level, DSNodePropertyType.List);

                // List to json
                PrimitiveListToJson(sb, node, level + 1);

                // AddObjectEnd(sb, level);
            }
            else
            {
                // if (addKey)
                // {
                AddObjectStart(sb, node.Key.ToString(), level, prefix);
                // }
                // else
                // {
                //     sb.AppendLine();
                //     AddIndentation(sb, level);
                //     sb.Append(JsonConstants.ObjectStart);
                // }

                if (false == string.IsNullOrWhiteSpace(prefix))
                {
                    level++;
                }

                // Add type info
                AddTypeInfo(sb, level, DSNodePropertyType.List);

                sb.AppendLine();
                AddIndentation(sb, level + 1);
                sb.AppendFormat("{0}: ", node.Key);

                var children = node.GetChildren();

                StringBuilder sb2 = new();
                // sb2.AppendLine();
                AddIndentation(sb2, level + 1);
                // sb2.Append(JsonConstants.ListStart);

                foreach (var keyValue in children)
                {
                    StringBuilder sb3 = new();
                    NodeToYaml(sb3, keyValue, level + 1, "-");
                    sb2.Append(sb3);
                }

                // sb2.Remove(sb2.Length - 1, 1);
                // sb2.AppendLine();
                // AddIndentation(sb2, level + 1);
                // sb2.Append(JsonConstants.ListEnd);


                sb.Append(sb2);
                // AddObjectEnd(sb, level);
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
            sb.AppendFormat("{0}: ", node.Key);
            sb.AppendLine();

            // Get all children of node
            var children = node.GetChildren();

            // sb.Append(JsonConstants.ListStart);

            foreach (var keyValue in children)
            {
                string? val = keyValue.IsNull ? "null" : keyValue.Value;

                // if (keyValue.IsNull)
                // {
                //     sb.Append(val);
                // }
                // else
                // {
                //     sb.Append(JsonConstants.Quote);
                //     sb.Append(val);
                //     sb.Append(JsonConstants.Quote);
                // }
                AddIndentation(sb, level + 1);
                sb.AppendFormat("- \"{0}\"", val);

                // sb.Append(", ");
                sb.AppendLine();
            }

            // Remove last New Line
            sb.Remove(sb.Length - 1, 1);

            // sb.Append(JsonConstants.ListEnd);
        }        
        
        private static void AddObjectStart(StringBuilder sb, string key, int level, string? prefix)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                // throw new DSInvalidJSONException(key);
                throw new NotImplementedException();
            }

            sb.AppendLine();
            AddIndentation(sb, level);

            if (string.IsNullOrWhiteSpace(prefix))
            {
                sb.AppendFormat("{0}:", key);
            }
            else
            {
                sb.AppendFormat("{0} {1}:", prefix, key);
            }
            
        }        

        private static void KeyValuePairToYaml(StringBuilder sb, string key, string? value, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                // throw new DSInvalidJSONException(key);
                throw new NotImplementedException();
            }

            // Make sure key:value has its own line.
            sb.AppendLine();

            AddIndentation(sb, level);

            if (null == value)
            {
                sb.AppendFormat("{0}: null", key);
            }
            else if (value == string.Empty)
            {
                sb.AppendFormat("{0}: \"{{}}\"", key);
            }
            else
            {
                sb.AppendFormat("{0}: \"{1}\"", key, value);
            }
        }
        
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