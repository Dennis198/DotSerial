using DotSerial.Core.General;
using System.Text;

namespace DotSerial.Core.JSON
{
    internal static class JSONParser
    {
        private const char Quote = '"';
        private const char ObjectStart = '{';
        private const char ObjectEnd = '}';
        private const char ListStart = '[';
        private const char ListEnd = ']';
        private const string NullString = "null";
        private const string NullString2 = "\"null\"";

        public static DSNode Convert(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new NotImplementedException();
            }

            // Removes all whitespaces
            string tmp = RemoveWhiteSpace(jsonString);
            StringBuilder sb = new(tmp);

            // Remove start and end brackets
            sb.Remove(sb.Length - 1, 1);
            sb.Remove(0, 1);

            var rootDic = ExtractKeyValuePairsFromJsonObject(sb);

            if (rootDic.Count != 1 && !rootDic.ContainsKey("0"))
            {
                throw new NotImplementedException();
            }

            var rootNode = new DSNode(0, DSNodeType.InnerNode, DSNodePropertyType.Class);

            StringBuilder childSb = new StringBuilder(rootDic["0"]);
            CreateChildren(rootNode, childSb);

            return rootNode;
        }

        private static void CreateChildren(DSNode parent, StringBuilder sb, bool isDirectory = false)
        {
            if (IsStringJsonObject(sb.ToString()))
            {                
                var dic = ExtractKeyValuePairsFromJsonObject(sb);

                var currPropType = DSNodePropertyType.Undefined;
                int dicId = -1;

                foreach(var keyValuepair in dic)
                {
                    dicId++;
                    DSNode keyValuePairNode = null;
                    DSNode keyValuePairNodeKey = null;
                    DSNode keyValuePairNodeValue = null;


                    string strKey = keyValuepair.Key;
                    if (false == int.TryParse(strKey, out int key))
                    {
                        if (false == isDirectory)
                        {
                            throw new NotImplementedException();
                        }
                    }

                    string strValue = keyValuepair.Value;

                    if (strKey == "-1")
                    {
                        currPropType = ReadTypeInfo(strValue);
                        parent.SetPropType(currPropType);
                        dicId--;
                        continue;
                    }

                    if (isDirectory)
                    {
                        keyValuePairNode = new(dicId, DSNodeType.InnerNode, DSNodePropertyType.KeyValuePair);
                        keyValuePairNodeKey = new(0, strKey, DSNodeType.Leaf, DSNodePropertyType.KeyValuePairKey);
                    }

                    if (strValue == NullString)
                    {
                        if (isDirectory)
                        {
                            keyValuePairNodeValue = new(1, null, DSNodeType.Leaf, DSNodePropertyType.KeyValuePairValue);
                            keyValuePairNode.AppendChild(0, keyValuePairNodeKey);
                            keyValuePairNode.AppendChild(1, keyValuePairNodeValue);
                            parent.AppendChild(dicId, keyValuePairNode);
                        }
                        else
                        {
                            if (dicId != key)
                            {
                                int h = 0;
                            }
                            // TODO SCHAUEN; OB MAN DAS über undefined machen kann?
                            DSNode child = new(key, null, DSNodeType.Leaf, DSNodePropertyType.Null);
                            parent.AppendChild(dicId, child);
                        }
                        continue;
                    }
                    else if (strValue == NullString2)
                    {
                        int h = 0;
                    }
                    else if (IsStringJsonObject(strValue))
                    {
                        if (currPropType == DSNodePropertyType.Class)
                        {
                            DSNode child = new(key, DSNodeType.InnerNode, DSNodePropertyType.Class);
                            StringBuilder sbChild = new(strValue);
                            CreateChildren(child, sbChild);
                            parent.AppendChild(key, child);
                        }
                        else if (isDirectory)
                        {
                            keyValuePairNodeValue = new(1, null, DSNodeType.InnerNode, DSNodePropertyType.KeyValuePairValue);
                            StringBuilder sbChild = new(strValue);
                            CreateChildren(keyValuePairNodeValue, sbChild);
                            keyValuePairNodeValue.SetPropType(DSNodePropertyType.KeyValuePairValue);
                        }
                        else
                        {
                            //DSNode child = new(key, DSNodeType.InnerNode, DSNodePropertyType.Dictionary);
                            StringBuilder sbChild = new(strValue);
                            CreateChildren(parent, sbChild, true);
                            //parent.AppendChild(key, child);
                            //throw new NotImplementedException();
                        }
                    }
                    else if (IsStringJsonList(strValue))
                    {
                        StringBuilder sbChild = new(strValue);
                        CreateList(parent, sbChild);
                    }
                    else // Primitive
                    {
                        if (isDirectory)
                        {
                            keyValuePairNodeValue = new(1, strValue, DSNodeType.Leaf, DSNodePropertyType.KeyValuePairValue);
                        }
                        else
                        {
                            DSNode child = new(key, strValue, DSNodeType.Leaf, DSNodePropertyType.Primitive);
                            parent.AppendChild(key, child);
                        }
                    }

                    if (isDirectory)
                    {
                        keyValuePairNode.AppendChild(0, keyValuePairNodeKey);
                        keyValuePairNode.AppendChild(1, keyValuePairNodeValue);
                        parent.AppendChild(dicId, keyValuePairNode);
                    }
                }
            }
        }

        private static void CreateList(DSNode parent, StringBuilder sb)
        {
            if (parent.PropType != DSNodePropertyType.List)
            {
                throw new NotImplementedException();
            }

            if (sb[1] == Quote)
            {
                int h = 0;
                var items = ExtractPrimitiveList(sb);
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] == NullString)
                    {
                        var child = new DSNode(i, null, DSNodeType.Leaf, DSNodePropertyType.Null);
                        parent.AppendChild(i, child);
                    }
                    else
                    {
                        var child = new DSNode(i, items[i], DSNodeType.Leaf, DSNodePropertyType.Primitive);
                        parent.AppendChild(i, child);
                    }
                }
            }
            else
            {
                int h = 0;
                var items = ExtractObjectList(sb);
                for (int i = 0; i < items.Count; i++)
                {
                    DSNode childNode = new DSNode(i, DSNodeType.InnerNode, DSNodePropertyType.Class);
                    StringBuilder sbChild = new(items[i]);
                    CreateChildren(childNode, sbChild);
                    parent.AppendChild(i, childNode);
                }
            }
        }

        private static List<string> ExtractObjectList(StringBuilder sb)
        {
            var list = new List<string>();
            for (int i = 0; i < sb.Length; i++)
            {
                char c = sb[i];

                if (c == ObjectStart)
                {
                    int j = ExtractJsonObject(sb.ToString(), i);

                    int len = j - i + 1;
                    string tmp = sb.ToString(i, len);
                    list.Add(tmp);
                    i = j;
                    continue;
                }
            }

            return list;
        }

        private static List<string> ExtractPrimitiveList(StringBuilder sb)
        {
            var result = new List<string>();

            for (int i = 0; i < sb.Length; i++)
            {
                char c = sb[i];

                if (c == Quote)
                {
                    // TODO sb nicht wirklich benötigt
                    StringBuilder sb2 = new StringBuilder();
                    i = AppendStringValue(sb2, i, sb.ToString());
                    sb2.Remove(0, 1);
                    sb2.Remove(sb2.Length - 1, 1);
                    result.Add(sb2.ToString());
                    continue;
                }
            }
            return result;
        }

        private static Dictionary<string, string> ExtractKeyValuePairsFromJsonObject(StringBuilder sb)
        {
            var result = new Dictionary<string, string>();

            bool keyFound = false;
            string founedKey = string.Empty;

            for (int i = 0; i < sb.Length; i++)
            {
                char c = sb[i];

                if (c == Quote && keyFound == false)
                {
                    keyFound = true;

                    // TODO sb nicht wirklich benötigt
                    StringBuilder sb2 = new StringBuilder();
                    i = AppendStringValue(sb2, i, sb.ToString());
                    sb2.Remove(0, 1);
                    sb2.Remove(sb2.Length - 1, 1);
                    founedKey = sb2.ToString();
                    result.Add(founedKey, string.Empty);
                    continue;
                }
                else if (c == Quote && keyFound == true)
                {
                    keyFound = false;

                    // TODO sb nicht wirklich benötigt
                    StringBuilder sb2 = new StringBuilder();
                    i = AppendStringValue(sb2, i, sb.ToString());
                    sb2.Remove(0, 1);
                    sb2.Remove(sb2.Length - 1, 1);
                   
                    if (false == result.ContainsKey(founedKey))
                    {
                        throw new NotImplementedException();
                    }

                    result[founedKey] = sb2.ToString();
                    founedKey = string.Empty;
                    continue;
                }
                else if (c == ObjectStart && keyFound == true)
                {
                    keyFound = false;
                    // todo

                    int j = ExtractJsonObject(sb.ToString(), i);

                    if (false == result.ContainsKey(founedKey))
                    {
                        throw new NotImplementedException();
                    }
                    int len = j - i + 1;
                    result[founedKey] = sb.ToString(i, len);
                    founedKey = string.Empty;
                    i = j;
                    continue;
                }
                else if (c == ListStart && keyFound == true)
                {
                    keyFound = false;
                    // todo

                    int j = ExtractJsonList(sb.ToString(), i);

                    if (false == result.ContainsKey(founedKey))
                    {
                        throw new NotImplementedException();
                    }
                    int len = j - i + 1;
                    result[founedKey] = sb.ToString(i, len);
                    founedKey = string.Empty;
                    i = j;
                    continue;
                }
            }

            return result;
        }

        private static int ExtractJsonObject(string jsonString, int startIndex)
        {
            if (jsonString[startIndex] != ObjectStart)
            {
                throw new NotImplementedException();
            }

            int numberNewObjects = 0;
            int endIndex = -1;

            for (int i = startIndex + 1; i < jsonString.Length; i++)
            {
                char c = jsonString[i];
                if (c == ObjectEnd && numberNewObjects == 0)
                {
                    return i;
                }
                else if (c == ObjectEnd)
                {
                    numberNewObjects--;
                }
                else if (c == ObjectStart)
                {
                    numberNewObjects++;
                }
                else if (c == Quote)
                {
                    // TODO sb nicht wirklich benötigt
                    StringBuilder sb = new StringBuilder();
                    i = AppendStringValue(sb, i, jsonString);
                    continue;
                }
            }

            throw new NotImplementedException();
            //return -1;
        }

        private static int ExtractJsonList(string jsonString, int startIndex)
        {
            if (jsonString[startIndex] != ListStart)
            {
                throw new NotImplementedException();
            }

            int numberNewObjects = 0;
            int endIndex = -1;

            for (int i = startIndex + 1; i < jsonString.Length; i++)
            {
                char c = jsonString[i];
                if (c == ListEnd && numberNewObjects == 0)
                {
                    return i;
                }
                else if (c == ListEnd)
                {
                    numberNewObjects--;
                }
                else if (c == ListStart)
                {
                    numberNewObjects++;
                }
                else if (c == Quote)
                {
                    // TODO sb nicht wirklich benötigt
                    StringBuilder sb = new StringBuilder();
                    i = AppendStringValue(sb, i, jsonString);
                    continue;
                }
            }

            throw new NotImplementedException();
            //return -1;
        }

        private static int AppendStringValue(StringBuilder sb, int startIndex, string jsonString)
        {
            sb.Append(Quote);

            for (int j = startIndex + 1; j < jsonString.Length; j++)
            {
                var c2 = jsonString[j];
                if (c2 == '\\')
                {

                    sb.Append(c2);
                    sb.Append(jsonString[j + 1]);
                    j++;
                }
                if (c2 == Quote)
                {
                    sb.Append(c2);
                    return j;
                }
                else
                {
                    sb.Append(c2);
                }
            }

            return jsonString.Length - 1;
        }

        private static string RemoveWhiteSpace(string jsonString)
        {
            StringBuilder sb = new();
            int stringLength = jsonString.Length;

            for (int i = 0; i < stringLength; i++)
            {
                var c = jsonString[i];

                if (c == Quote)
                {
                    i = AppendStringValue(sb, i, jsonString);
                    continue;
                }
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                sb.Append(c);
            }

            return sb.ToString();
        }

        private static bool IsStringJsonObject(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            string tmp = RemoveWhiteSpace(str);

            if (tmp[0] != ObjectStart)
            {
                return false;
            }

            if (tmp[^1] != ObjectEnd)
            {
                return false;
            }

            return true;
        }

        private static bool IsStringJsonList(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            string tmp = RemoveWhiteSpace(str);

            if (tmp[0] != ListStart)
            {
                return false;
            }

            if (tmp[^1] != ListEnd)
            {
                return false;
            }

            return true;
        }

        private static DSNodePropertyType ReadTypeInfo(string value)
        {
            if (value.Equals("Class"))
            {
                return DSNodePropertyType.Class;
            }
            else if (value.Equals("List"))
            {
                return DSNodePropertyType.List;
            }
            else if (value.Equals("Dictionary"))
            {
                return DSNodePropertyType.Dictionary;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
