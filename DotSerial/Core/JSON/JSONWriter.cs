using DotSerial.Core.General;
using System.Text;
using System.Xml.Linq;

namespace DotSerial.Core.JSON
{
    internal static class JSONWriter
    {
        private const int LEVEL_WIDTH = 2;
        const string quote = "\"";

        public static string Convert(DSNode node)
        {
            StringBuilder sb = new();
            sb.AppendLine("{");
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
                sb.AppendLine();
                sb.Append(' ', level * LEVEL_WIDTH);
                sb.AppendFormat("\"{0}\": \"null\",", node.Key);
                return;
            }

            //sb.AppendFormat("\"{0}\": ", node.Key);

            if (node.PropType == DSNodePropertyType.Class)
            {
                //sb.Append('{');

                ConvertClassNode(sb, node, level, addKey);

                //sb.AppendLine("}");
            }
            else if (node.PropType == DSNodePropertyType.List)
            {
                if (node.IsEmpty)
                {
                    sb.AppendFormat("\"{0}\": \"{{}}\",", node.Key);
                    return;
                }

                ConvertListNode(sb, node, level);
            }
            else if (node.PropType == DSNodePropertyType.Dictionary)
            {
                if (node.IsEmpty)
                {
                    sb.AppendLine();
                    sb.Append(' ', level * LEVEL_WIDTH);
                    sb.AppendFormat("\"{0}\": \"{{}}\",", node.Key);
                    return;
                }

                ConvertDictionaryNode(sb, node, level);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static void AddTypeInfo(StringBuilder sb, int level, DSNodePropertyType type)
        {
            sb.AppendLine();
            sb.Append(' ', (level + 1) * LEVEL_WIDTH);
            string typeName = type switch
            {
                DSNodePropertyType.Class => "Class",
                DSNodePropertyType.List => "List",
                DSNodePropertyType.Dictionary => "Dictionary",
                _ => throw new NotImplementedException(),
            };
            sb.AppendFormat("\"{0}\": \"{1}\",", -1, typeName);
        }

        private static void ConvertDictionaryNode(StringBuilder sb, DSNode node, int level)
        {
            if (node.PropType != DSNodePropertyType.Dictionary)
            {
                throw new NotImplementedException();
            }

            if (node.IsPrimitiveDictionary())
            {
                sb.AppendLine();
                sb.Append(' ', level * LEVEL_WIDTH);
                sb.AppendFormat("\"{0}\": {{", node.Key);

                AddTypeInfo(sb, level, DSNodePropertyType.Dictionary);
                sb.AppendLine();
                sb.Append(' ', (level + 1) * LEVEL_WIDTH);
                sb.AppendFormat("\"{0}\": ", node.Key);

                List<string> keys = node.GetDicionaryNodeKeys();
                List<DSNode> values = node.GetDicionaryNodeVales();

                StringBuilder sb2 = new StringBuilder();
                
                sb2.Append('{');

                for (int i = 0; i < keys.Count; i++)
                {
                    string key = keys[i];
                    var value = values[i];


                    sb2.AppendLine();
                    sb2.Append(' ', (level + 1) * LEVEL_WIDTH);
                    if (false == node.IsNull)
                    {
                        sb2.AppendFormat("\"{0}\": \"{1}\",", key, value.Value);
                    }
                    else
                    {
                        sb2.AppendFormat("\"{0}\": \"null\",", key);
                    }
                }

                sb2.Remove(sb2.Length - 1, 1);
                sb2.AppendLine();
                sb2.Append(' ', (level + 1) * LEVEL_WIDTH);
                sb2.Append('}');


                sb.Append(sb2);
                sb.AppendLine();
                sb.Append(' ', level * LEVEL_WIDTH);
                sb.Append("},");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static void ConvertListNode(StringBuilder sb, DSNode node, int level)
        {
            if (node.PropType != DSNodePropertyType.List)
            {
                throw new NotImplementedException();
            }
            

            if (node.IsPrimitiveList())
            {
                sb.AppendLine();
                sb.Append(' ', level * LEVEL_WIDTH);
                sb.AppendFormat("\"{0}\": {{", node.Key);

                AddTypeInfo(sb, level, DSNodePropertyType.List);
                sb.AppendLine();
                sb.Append(' ', (level + 1) * LEVEL_WIDTH);
                sb.AppendFormat("\"{0}\": ", node.Key);

                var children = node.GetChildren();

                string tmp = "[";

                foreach (var keyValue in children)
                {
                    tmp += quote;
                    tmp += keyValue.Value.Value;
                    tmp += quote;
                    tmp += ", ";
                }

                tmp = tmp[..^2];
                tmp += "]";


                sb.Append(tmp);
                sb.AppendLine();
                sb.Append(' ', level * LEVEL_WIDTH);
                sb.Append("},");
            }
            else
            {
                sb.AppendLine();
                sb.Append(' ', level * LEVEL_WIDTH);
                sb.AppendFormat("\"{0}\": {{", node.Key);

                AddTypeInfo(sb, level, DSNodePropertyType.List);
                sb.AppendLine();
                sb.Append(' ', (level + 1) * LEVEL_WIDTH);
                sb.AppendFormat("\"{0}\": ", node.Key);

                var children = node.GetChildren();

                string tmp = "[";
                tmp += Environment.NewLine;
                foreach (var keyValue in children)
                {
                    StringBuilder sb2 = new StringBuilder();
                    ConvertNode(sb2, keyValue.Value, level + 1, false);
                    tmp += sb2.ToString();
                    tmp += Environment.NewLine;
 
                }

                tmp = tmp[..^3];
                tmp += "]";


                sb.Append(tmp);
                sb.AppendLine();
                sb.Append(' ', level * LEVEL_WIDTH);
                sb.Append("},");
            }
        }

        private static void ConvertClassNode(StringBuilder sb, DSNode node, int level, bool addKey = true)
        {
            if (node.Type == DSNodeType.Leaf)
            {
                throw new NotImplementedException();
            }

            //sb.AppendLine();
            sb.Append(' ', level * LEVEL_WIDTH);

            if (node.IsNull)
            {
                sb.AppendFormat("\"{0}\": \"null\",", node.Key);
            }
            else if (node.IsEmpty)
            {
                //sb.AppendFormat("\"{0}\": \"{{}}\",", node.Key);
                sb.AppendFormat("\"{0}\": {{", node.Key);
                AddTypeInfo(sb, level, DSNodePropertyType.Class);
                // Remove last ','
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
                sb.Append(' ', level * LEVEL_WIDTH);
                sb.Append("},");
            }
            else
            {
                if (addKey)
                    sb.AppendFormat("\"{0}\": {{", node.Key);
                else
                    sb.Append('{');

                AddTypeInfo(sb, level, DSNodePropertyType.Class);

                var children = node.GetChildren();

                foreach(var keyValue in children)
                {
                    ConvertNode(sb, keyValue.Value, level);
                }

                // Remove last ','
                sb.Remove(sb.Length - 1, 1);

                sb.AppendLine();
                sb.Append(' ', level * LEVEL_WIDTH);
                sb.Append("},");
            }
        }

        private static void ConvertLeafNode(StringBuilder sb, DSNode node, int level)
        {
            if (node.Type != DSNodeType.Leaf)
            {
                throw new NotImplementedException();
            }

            sb.AppendLine();
            sb.Append(' ', level * LEVEL_WIDTH);
            if (false == node.IsNull)
            {
                sb.AppendFormat("\"{0}\": \"{1}\",", node.Key, node.Value);
            }
            else
            {
                sb.AppendFormat("\"{0}\": \"null\",", node.Key);
            }            
        }

        //private const int LEVELTAB = 2;
        //internal static string ConvertToString(DSNode node)
        //{
        //    StringBuilder sb = new();
        //    return sb.ToString();
        //}

        //private static void Test(StringBuilder sb, DSNode node)
        //{
        //    ArgumentNullException.ThrowIfNull(node);

        //    sb.Append('"');
        //    sb.Append(node.Key);
        //    sb.Append('"');
        //    sb.Append(':');
        //}

        //private static void AddKeyValueSimple(StringBuilder sb, string key, string value, int level)
        //{
        //    sb.Append(' ', level * LEVELTAB);
        //    sb.Append('"');
        //    sb.Append(key);
        //    sb.Append('"');
        //    sb.Append(':');
        //    sb.Append(' ');
        //    sb.Append('"');
        //    sb.Append(value);
        //    sb.Append('"');
        //    sb.Append(',');
        //    sb.Append(Environment.NewLine);
        //}

        //private static void AddKey(StringBuilder sb, string key, int level)
        //{
        //    sb.Append(' ', level * LEVELTAB);
        //    sb.Append('"');
        //    sb.Append(key);
        //    sb.Append('"');
        //    sb.Append(':');
        //}
    }
}
