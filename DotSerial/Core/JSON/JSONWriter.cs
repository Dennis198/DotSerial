using DotSerial.Core.General;
using System.Text;

namespace DotSerial.Core.JSON
{
    // https://github.com/zanders3/json/blob/master/src/JSONParser.cs
    internal static class JSONWriter
    {
        private const int IndentationSize = 2;
        private const char Quote = '"';
        private const string NullString = "\"null\"";

        public static string Convert(DSNode node)
        {
            StringBuilder sb = new();
            sb.Append('{');
            ConvertNode(sb, node, 0);
            sb.Remove(sb.Length - 1, 1);
            sb.AppendLine();
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static void ConvertNode(StringBuilder sb, DSNode node, int level, bool addKey = true)
        {
            if (node.Type == DSNodeType.InnerNode || node.Type == DSNodeType.Root)
            {
                ConvertInnerNode(sb, node, level + 1, addKey);
            }
            else if (node.Type == DSNodeType.Leaf)
            {
                ConvertLeafNode(sb, node, level + 1);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static void ConvertInnerNode(StringBuilder sb, DSNode node, int level, bool addKey = true)
        {
            if (node.Type == DSNodeType.Leaf)
            {
                throw new NotImplementedException();
            }

            if (node.IsNull)
            {
                AddKeyValuePair(sb, node.Key.ToString(), null, level);
                return;
            }

            if (node.PropType == DSNodePropertyType.Class)
            {
                ConvertClassNode(sb, node, level, addKey);
            }
            else if (node.PropType == DSNodePropertyType.List)
            {
                ConvertListNode(sb, node, level, addKey);
            }
            else if (node.PropType == DSNodePropertyType.Dictionary)
            {
                ConvertDictionaryNode(sb, node, level);
            }
            else if (node.PropType == DSNodePropertyType.KeyValuePair)
            {
                ConvertKeyValuePair(sb, node, level);
            }
            else if (node.PropType == DSNodePropertyType.KeyValuePairValue)
            {
                ConvertClassNode(sb, node, level, addKey);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static void ConvertKeyValuePair(StringBuilder sb, DSNode node, int level)
        {
            if (node.PropType != DSNodePropertyType.KeyValuePair)
            {
                throw new NotImplementedException();
            }

            // TODO Überarbeiten
            var keyNode = node.GetChild(0);
            int key = int.Parse(keyNode.Value);

            var valueNode = node.GetChild(1);
            valueNode = valueNode.Clone(key);
            ConvertClassNode(sb, valueNode, level, true);
        }

        private static void ConvertDictionaryNode(StringBuilder sb, DSNode node, int level)
        {
            if (node.PropType != DSNodePropertyType.Dictionary)
            {
                throw new NotImplementedException();
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

                List<string> keys = node.GetDicionaryNodeKeys();
                List<DSNode> values = node.GetDicionaryNodeVales();

                for (int i = 0; i < keys.Count; i++)
                {
                    string key = keys[i];
                    var value = values[i];

                    AddKeyValuePair(sb2, key, value.Value, level + 1, false);
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

                List<DSNode> keyvaluePair = node.GetDictionaryKeyValuePairs();

                for (int i = 0; i < keyvaluePair.Count; i++)
                {
                    var keyValuePair = keyvaluePair[i];

                    if (false == node.IsNull)
                    {
                        StringBuilder sb3 = new();
                        ConvertNode(sb3, keyValuePair, level + 1);
                        sb2.Append(sb3);
                    }
                    else
                    {

                        throw new NotImplementedException();
                    }
                }

                sb2.Remove(sb2.Length - 1, 1);
                AddObjectEnd(sb2, level + 1, true);

                sb.Append(sb2);
                AddObjectEnd(sb, level);
            }
        }

        private static void ConvertListNode(StringBuilder sb, DSNode node, int level, bool addKey = true)
        {
            if (node.PropType != DSNodePropertyType.List)
            {
                throw new NotImplementedException();
            }

            if (node.IsEmpty)
            {
                AddObjectStart(sb, node.Key.ToString(), level);
                AddTypeInfo(sb, level, DSNodePropertyType.List);
                AddObjectEnd(sb, level);
                return;
            }

            if (node.IsPrimitiveList())
            {
                if (addKey)
                {
                    AddObjectStart(sb, node.Key.ToString(), level);
                }
                else
                {
                    sb.AppendLine();
                    sb.Append(' ', level * IndentationSize);
                    sb.Append('{');
                }

                AddTypeInfo(sb, level, DSNodePropertyType.List);

                CreatePrimitiveList(sb, node, level + 1);

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
                    sb.Append(' ', level * IndentationSize);
                    sb.Append('{');
                }

                AddTypeInfo(sb, level, DSNodePropertyType.List);
                sb.AppendLine();
                sb.Append(' ', (level + 1) * IndentationSize);
                sb.AppendFormat("\"{0}\": ", node.Key);                

                var children = node.GetChildren();

                StringBuilder sb2 = new();
                sb2.AppendLine();
                sb2.Append(' ', (level + 1) * IndentationSize);
                sb2.Append('[');

                foreach (var keyValue in children)
                {
                    StringBuilder sb3 = new();
                    ConvertNode(sb3, keyValue.Value, level + 1, false);
                    sb2.Append(sb3);
                }

                sb2.Remove(sb2.Length - 1, 1);
                sb2.AppendLine();
                sb2.Append(' ', (level + 1) * IndentationSize);
                sb2.Append(']');


                sb.Append(sb2);
                AddObjectEnd(sb, level);
            }
        }

        private static void ConvertClassNode(StringBuilder sb, DSNode node, int level, bool addKey = true)
        {
            if (node.Type == DSNodeType.Leaf)
            {
                throw new NotImplementedException();
            }

            if (node.IsNull)
            {
                sb.AppendFormat("\"{0}\": \"null\",", node.Key);
            }
            else if (node.IsEmpty)
            {
                AddObjectStart(sb, node.Key.ToString(), level);
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
                    sb.Append(' ', level * IndentationSize);
                    sb.AppendLine();
                    sb.Append(' ', level * IndentationSize);
                    sb.Append('{');
                }
                    
                AddTypeInfo(sb, level, DSNodePropertyType.Class);

                var children = node.GetChildren();

                foreach(var keyValue in children)
                {
                    ConvertNode(sb, keyValue.Value, level);
                }

                // Remove last ','
                sb.Remove(sb.Length - 1, 1);

                AddObjectEnd(sb, level);
            }
        }

        private static void ConvertLeafNode(StringBuilder sb, DSNode node, int level)
        {
            if (node.Type != DSNodeType.Leaf)
            {
                throw new NotImplementedException();
            }

            AddKeyValuePair(sb, node.Key.ToString(), node.Value, level);
        }

        private static void AddTypeInfo(StringBuilder sb, int level, DSNodePropertyType type)
        {
            string typeName = type switch
            {
                DSNodePropertyType.Class => "Class",
                DSNodePropertyType.List => "List",
                DSNodePropertyType.Dictionary => "Dictionary",
                _ => throw new NotImplementedException(),
            };
            AddKeyValuePair(sb, "-1", typeName, level + 1);
        }

        private static void AddKeyValuePair(StringBuilder sb, string key, string? value, int level, bool allowEmptyClass = true)
        {
            sb.AppendLine();
            AddIndentation(sb, level);
            allowEmptyClass = true;
            if (null == value && allowEmptyClass)
            {
                sb.AppendFormat("\"{0}\": \"null\",", key);
            }
            else if (value == string.Empty && allowEmptyClass)
            {
                sb.AppendFormat("\"{0}\": \"{{}}\",", key);
            }
            else
            {
                sb.AppendFormat("\"{0}\": \"{1}\",", key, value);
            }
        }

        private static void AddObjectStart(StringBuilder sb, string key, int level)
        {           
            sb.AppendLine();
            AddIndentation(sb, level);
            sb.AppendFormat("\"{0}\": {{", key);
        }

        private static void AddObjectEnd(StringBuilder sb, int level, bool isLastObject = false)
        {
            sb.AppendLine();
            sb.Append(' ', level * IndentationSize);
            if (isLastObject)
            {
                sb.Append('}');
            }
            else
            {
                sb.Append("},");
            }
        }

        private static void CreatePrimitiveList(StringBuilder sb, DSNode node, int level)
        {
            sb.AppendLine();
            sb.Append(' ', level * IndentationSize);
            sb.AppendFormat("\"{0}\": ", node.Key);

            var children = node.GetChildren();

            sb.Append('[');

            foreach (var keyValue in children)
            {
                sb.Append(Quote);
                if (keyValue.Value.IsNull)
                {
                    sb.Append("null");
                }
                else
                {
                    sb.Append(keyValue.Value.Value);
                }
                sb.Append(Quote);
                sb.Append(", ");
            }

            sb.Remove(sb.Length - 2, 2);
            sb.Append(']');
        }

        private static void AddIndentation(StringBuilder sb, int level)
        {
            sb.Append(' ', level * IndentationSize);
        }

    }
}
