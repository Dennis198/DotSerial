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

        private static void NodeToYaml(StringBuilder sb, DSNode node, int level, bool addKey = true)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            if (node.Type == DSNodeType.InnerNode || node.Type == DSNodeType.Root)
            {
                // Inner node to json
                InnerNodeToYaml(sb, node, level + 1, addKey);
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

        private static void InnerNodeToYaml(StringBuilder sb, DSNode node, int level, bool addKey = true)
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
                ClassNodeToYaml(sb, node, level, addKey);
            }
            else if (node.PropType == DSNodePropertyType.List)
            {
                // ListNodeToJson(sb, node, level, addKey);
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
        
        private static void ClassNodeToYaml(StringBuilder sb, DSNode node, int level, bool addKey = true)
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
                AddObjectStart(sb, node.Key.ToString(), level);

                // Add type info
                AddTypeInfo(sb, level, DSNodePropertyType.Class);
            }
            else
            {
                // if (addKey)
                // {
                AddObjectStart(sb, node.Key.ToString(), level);
                // }
                // else
                // {
                //     AddIndentation(sb, level);
                //     sb.AppendLine();
                //     AddIndentation(sb, level);
                //     sb.Append('{');
                // }

                // Add type info
                AddTypeInfo(sb, level, DSNodePropertyType.Class);

                var children = node.GetChildren();

                foreach(var keyValue in children)
                {
                    NodeToYaml(sb, keyValue, level);
                }
            }
        }        
        
        private static void AddObjectStart(StringBuilder sb, string key, int level)
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
            sb.AppendFormat("{0}:", key);
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