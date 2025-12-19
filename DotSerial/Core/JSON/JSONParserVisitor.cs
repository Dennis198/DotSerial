using System.Text;
using DotSerial.Core.Exceptions.JSON;
using DotSerial.Core.General;
using DotSerial.Core.Misc;
using DotSerial.Core.Tree;

namespace DotSerial.Core.JSON
{
    /// <summary>
    /// Visitor interface for tree nodes.
    /// </summary>
    public class JSONParserVisitor : INodeParserVisitor
    {
        /// <summary>
        /// Node factory;
        /// </summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, IDSNode? parent, StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            // Implementation for parsing leaf node from JSON can be added here
            // This is a placeholder for demonstration purposes
        }

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, IDSNode? parent, StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            if (IsStringJsonObject(sb.ToString()))
            {
                // Extract key, value pairs
                var dic = ExtractKeyValuePairsFromJsonObject(sb);

                foreach(var keyValuepair in dic)
                {
                     // Convert key to int key
                    string key = keyValuepair.Key;
                    string? strValue = keyValuepair.Value;

                    if (null == strValue)
                    {
                        var childNode = _nodeFactory.CreateNode(key, null, NodeType.Leaf);
                        node.AddChild(childNode);
                    }
                    else if (IsStringJsonObject(strValue))
                    {
                        // Create inner node
                        var innerNode = _nodeFactory.CreateNode(key, null, NodeType.InnerNode) as InnerNode ?? throw new DSInvalidJSONException(sb.ToString());

                        // Create stringbuilder for inner content
                        StringBuilder innerSb = new (strValue);

                        // Parse inner node
                        innerNode.ParserAccept(new JSONParserVisitor(), node, innerSb);

                        // Add inner node to parent
                        // node.AddChild(innerNode);
                    }
                    else if (IsStringJsonList(strValue))
                    {
                        // Create list node
                        if (_nodeFactory.CreateNode(key, null, NodeType.ListNode) is not ListNode listNode)
                        {
                            throw new DSInvalidJSONException(sb.ToString());
                        }

                        // Create stringbuilder for list content
                        StringBuilder listSb = new (strValue);

                        // Parse list node
                        listNode.ParserAccept(new JSONParserVisitor(), node, listSb);

                        // Add list node to parent // TODO
                        // node.AddChild(listNode);
                    }
                    else
                    {
                        var childNode = _nodeFactory.CreateNode(key, strValue, NodeType.Leaf);
                        node.AddChild(childNode);
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            parent?.AddChild(node);

            // Implementation for parsing inner node from JSON can be added here
            // This is a placeholder for demonstration purposes
        }

        /// <inheritdoc/>
        public void VisitListNode(ListNode node, IDSNode? parent, StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(sb);

            // Implementation for parsing list node from JSON can be added here
            // This is a placeholder for demonstration purposes

            if (IsStringJsonList(sb.ToString()))
            {
                // Check if list is list of primitive or objects
                if (sb[1] == GeneralConstants.Quote || sb[1] == GeneralConstants.N)
                {
                    // Extract primitive list
                    var items = ExtractPrimitiveList(sb);
                    for (int i = 0; i < items.Count; i++)
                    {
                        string? value = items[i];
                        var child = _nodeFactory.CreateNode(i.ToString(), value, NodeType.Leaf);
                        node.AddChild(child);
                    }
                }
                else
                {
                    // Extract object list
                    var items = ExtractObjectList(sb);
                    for (int i = 0; i < items.Count; i++)
                    {
                        // TODO
                        if (IsStringJsonObject(items[i]))
                        {
                            // Create inner node
                            var innerNode = _nodeFactory.CreateNode(i.ToString(), null, NodeType.InnerNode) as InnerNode ?? throw new DSInvalidJSONException(sb.ToString());

                            // Create stringbuilder for inner content
                            StringBuilder innerSb = new (items[i]);

                            // Parse inner node
                            innerNode.ParserAccept(new JSONParserVisitor(), node, innerSb);

                            // Add inner node to parent
                            // node.AddChild(innerNode);
                        }
                        else if (IsStringJsonList(items[i]))
                        {
                            // Create list node
                            if (_nodeFactory.CreateNode(i.ToString(), null, NodeType.ListNode) is not ListNode listNode)
                            {
                                throw new DSInvalidJSONException(sb.ToString());
                            }

                            // Create stringbuilder for list content
                            StringBuilder listSb = new (items[i]);

                            // Parse list node
                            listNode.ParserAccept(new JSONParserVisitor(), node, listSb);

                            // Add list node to parent // TODO
                            // node.AddChild(listNode);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        // DSNode childNode = new(i.ToString(), DSNodeType.InnerNode, DSNodePropertyType.Class);
                        // var child = _nodeFactory.CreateNode(i.ToString(), value, NodeType.Leaf);
                        // StringBuilder sbChild = new(items[i]);
                        // ChildrenToNode(childNode, sbChild);
                        // node.AddChild(childNode);
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            parent?.AddChild(node);
        }

        /// <summary>
        /// Extracts key value pairs from json object
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <returns>Dictionary<string, string></returns>
        private static Dictionary<string, string?> ExtractKeyValuePairsFromJsonObject(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            var result = new Dictionary<string, string?>();

            // Helper vars
            bool keyFound = false;
            string founedKey = string.Empty;

            for (int i = 0; i < sb.Length; i++)
            {
                char c = sb[i];

                // Check if opening quote for the key is found
                if (c == GeneralConstants.Quote && keyFound == false)
                {
                    // Quote is opening
                    keyFound = true;

                    StringBuilder sb2 = new();
                    i = ParseMethods.AppendStringValue(sb2, i, sb.ToString());

                    // Remove opening and closing quote
                    sb2.Remove(0, 1);
                    sb2.Remove(sb2.Length - 1, 1);

                    // Save key
                    founedKey = sb2.ToString();

                    // Add key
                    result.Add(founedKey, string.Empty);

                    continue;
                }
                // Check if opening quote for the value is found (primitive)
                else if (c == GeneralConstants.Quote && keyFound == true)
                {
                    // value is found
                    keyFound = false;

                    StringBuilder sb2 = new();
                    i = ParseMethods.AppendStringValue(sb2, i, sb.ToString());

                    // Remove opening and closing quote
                    sb2.Remove(0, 1);
                    sb2.Remove(sb2.Length - 1, 1);
                   
                    if (false == result.ContainsKey(founedKey))
                    {
                        throw new KeyNotFoundException();
                    }

                    // Add key
                    result[founedKey] = sb2.ToString();

                    // Reset found key
                    founedKey = string.Empty;

                    continue;
                }
                else if (c == GeneralConstants.N && keyFound == true)
                {
                    // value is found => null
                    keyFound = false;

                    if (i + 3 > sb.Length -1) throw new DSInvalidJSONException(sb.ToString());

                    i++;
                    if (sb[i] != GeneralConstants.U) throw new DSInvalidJSONException(sb.ToString());
                    i++;
                    if (sb[i] != GeneralConstants.L) throw new DSInvalidJSONException(sb.ToString());
                    i++;
                    if (sb[i] != GeneralConstants.L) throw new DSInvalidJSONException(sb.ToString());

                    // Add key
                    result[founedKey] = null;

                    // Reset found key
                    founedKey = string.Empty;

                }
                // Check if opening symbol for the value is found (json object)
                else if (c == JsonConstants.ObjectStart && keyFound == true)
                {
                    // value is found
                    keyFound = false;

                    // Extract value
                    int j = ExtractJsonObject(sb.ToString(), i);

                    if (false == result.ContainsKey(founedKey))
                    {
                        throw new KeyNotFoundException();
                    }

                    // Add key
                    int len = j - i + 1;
                    result[founedKey] = sb.ToString(i, len);

                    // Reset found key
                    founedKey = string.Empty;

                    // Update index
                    i = j;

                    continue;
                }
                // Check if opening symbol for the value is found (json list)
                else if (c == JsonConstants.ListStart && keyFound == true)
                {
                    // value is found
                    keyFound = false;

                    // Extract value
                    int j = ExtractJsonList(sb.ToString(), i);

                    if (false == result.ContainsKey(founedKey))
                    {
                        throw new KeyNotFoundException();
                    }
                    // Add key
                    int len = j - i + 1;
                    result[founedKey] = sb.ToString(i, len);

                    // Reset found key
                    founedKey = string.Empty;

                    // Update index
                    i = j;
                    continue;
                }
            }

            return result;
        }  

        /// <summary>
        /// Extracts a json object
        /// </summary>
        /// <param name="jsonString">string</param>
        /// <param name="startIndex">Index of the opeing symbol</param>
        /// <returns>Index of end symbol</returns>
        private static int ExtractJsonObject(string jsonString, int startIndex)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new DSInvalidJSONException(jsonString);
            }

            if (jsonString.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (jsonString[startIndex] != JsonConstants.ObjectStart)
            {
                throw new DSInvalidJSONException(jsonString);
            }

            int numberNewObjects = 0;

            for (int i = startIndex + 1; i < jsonString.Length; i++)
            {
                char c = jsonString[i];
                if (c == JsonConstants.ObjectEnd && numberNewObjects == 0)
                {
                    return i;
                }
                else if (c == JsonConstants.ObjectEnd)
                {
                    numberNewObjects--;
                }
                else if (c == JsonConstants.ObjectStart)
                {
                    numberNewObjects++;
                }
                else if (c == GeneralConstants.Quote)
                {
                    StringBuilder sb = new();
                    i = ParseMethods.AppendStringValue(sb, i, jsonString);
                    continue;
                }
            }

            throw new DSInvalidJSONException(jsonString);
        }    

        /// <summary>
        /// Extracts a json list
        /// </summary>
        /// <param name="jsonString">string</param>
        /// <param name="startIndex">Index of the opeing symbol</param>
        /// <returns>Index of end symbol</returns>
        private static int ExtractJsonList(string jsonString, int startIndex)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new DSInvalidJSONException(jsonString);
            }

            if (jsonString.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (jsonString[startIndex] != JsonConstants.ListStart)
            {
                throw new DSInvalidJSONException(jsonString);
            }

            int numberNewObjects = 0;

            for (int i = startIndex + 1; i < jsonString.Length; i++)
            {
                char c = jsonString[i];
                if (c == JsonConstants.ListEnd && numberNewObjects == 0)
                {
                    return i;
                }
                else if (c == JsonConstants.ListEnd)
                {
                    numberNewObjects--;
                }
                else if (c == JsonConstants.ListStart)
                {
                    numberNewObjects++;
                }
                else if (c == GeneralConstants.Quote)
                {
                    StringBuilder sb = new ();
                    i = ParseMethods.AppendStringValue(sb, i, jsonString);
                    continue;
                }
            }

            throw new DSInvalidJSONException(jsonString);
        }  

        /// <summary>
        /// Extracts object list from json string
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <returns>List<string></returns>
        private static List<string> ExtractObjectList(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            var list = new List<string>();

            for (int i = 0; i < sb.Length; i++)
            {
                char c = sb[i];

                // Check if opening symbol is found
                if (c == JsonConstants.ObjectStart)
                {
                    // Extract object
                    int j = ExtractJsonObject(sb.ToString(), i);

                    int len = j - i + 1;
                    string tmp = sb.ToString(i, len);

                    // Add object to result
                    list.Add(tmp);

                    // Update index
                    i = j;
                }
            }

            return list;
        }        

        /// <summary>
        /// Check if string is a json object.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>True, if is a object</returns>
        private static bool IsStringJsonObject(string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            // Remove all whitespaces
            string tmp = ParseMethods.RemoveWhiteSpace(str);

            // Check if first element is '{'
            if (tmp[0] != JsonConstants.ObjectStart)
            {
                return false;
            }

            // Check if first element is '}'
            if (tmp[^1] != JsonConstants.ObjectEnd)
            {
                return false;
            }

            return true;
        }  

        /// <summary>
        /// Check if string is a json list.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>True, if is a list</returns>
        private static bool IsStringJsonList(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            // Remove all whitespaces
            string tmp = ParseMethods.RemoveWhiteSpace(str);

            // Check if first element is '['
            if (tmp[0] != JsonConstants.ListStart)
            {
                return false;
            }

            // Check if last element is ']'
            if (tmp[^1] != JsonConstants.ListEnd)
            {
                return false;
            }

            return true;
        }      

        /// <summary>
        /// Extracts list of primitives from json string
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <returns>List<string></returns>
        private static List<string?> ExtractPrimitiveList(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            var result = new List<string?>();

            for (int i = 0; i < sb.Length; i++)
            {
                char c = sb[i];

                // Check if opening quote is found
                if (c == GeneralConstants.Quote)
                {
                    StringBuilder sb2 = new ();

                    // Extract value
                    i = ParseMethods.AppendStringValue(sb2, i, sb.ToString());

                    // Remove opening and closing value
                    sb2.Remove(0, 1);
                    sb2.Remove(sb2.Length - 1, 1);

                    // Add value to result
                    result.Add(sb2.ToString());
                }
                else if (c == GeneralConstants.N)
                {
                    if (i + 3 > sb.Length - 1) throw new DSInvalidJSONException(sb.ToString());

                    i++;
                    if (sb[i] != GeneralConstants.U) throw new DSInvalidJSONException(sb.ToString());
                    i++;
                    if (sb[i] != GeneralConstants.L) throw new DSInvalidJSONException(sb.ToString());
                    i++;
                    if (sb[i] != GeneralConstants.L) throw new DSInvalidJSONException(sb.ToString());

                    // Add value to result
                    result.Add(null);
                }
            }

            return result;
        }                                
    }
}