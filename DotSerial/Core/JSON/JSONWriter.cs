using DotSerial.Core.General;
using System.Text;

namespace DotSerial.Core.JSON
{
    internal static class JSONWriter
    {
        private const int LEVEL_WIDTH = 2;
        public static string Convert(DSNode node)
        {
            StringBuilder sb = new();
            sb.AppendLine("{");
            ConvertNode(sb, node, 0);
            sb.AppendLine();
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static void ConvertNode(StringBuilder sb, DSNode node, int level)
        {
            if (node.Type == DSNodeType.InnerNode || node.Type == DSNodeType.Root)
            {
                ConvertInnerNode(sb, node, level + 1);
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

        private static void ConvertInnerNode(StringBuilder sb, DSNode node, int level)
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

                ConvertClassNode(sb, node, level);

                //sb.AppendLine("}");
            }
            else if (node.PropType == DSNodePropertyType.List)
            {
                if (node.IsEmpty)
                {
                    sb.AppendFormat("\"{0}\": \"{{}}\",", node.Key);
                    return;
                }
            }
            else if (node.PropType == DSNodePropertyType.Dictionary)
            {
                if (node.IsEmpty)
                {
                    sb.AppendFormat("\"{0}\": \"{{}}\",", node.Key);
                    return;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static void ConvertClassNode(StringBuilder sb, DSNode node, int level)
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
                sb.AppendFormat("\"{0}\": \"{{}}\",", node.Key);
            }
            else
            {
                sb.AppendFormat("\"{0}\": {{", node.Key);

                var children = node.GetChildren();

                foreach(var keyValue in children)
                {
                    ConvertNode(sb, keyValue.Value, level);
                }

                // Remove last ','
                sb.Remove(sb.Length - 1, 1);

                sb.AppendLine();
                sb.Append(' ', level * LEVEL_WIDTH);
                sb.Append('}');
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
