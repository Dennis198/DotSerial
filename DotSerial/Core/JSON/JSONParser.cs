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

using DotSerial.Core.Exceptions.JSON;
using DotSerial.Core.Exceptions.Node;
using DotSerial.Core.General;
using System.Text;

namespace DotSerial.Core.JSON
{
    /// <summary>
    /// Class can converts a json string to node/tree
    /// </summary>
    internal static class JSONParser
    {
        /// <summary>
        /// Converts a json string to anode
        /// </summary>
        /// <param name="jsonString">String</param>
        /// <returns>Node</returns>
        public static DSNode ToNode(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new DSInvalidJSONException(jsonString);
            }

            // Removes all whitespaces
            string tmp = RemoveWhiteSpace(jsonString);
            StringBuilder sb = new(tmp);

            // Remove start and end brackets
            sb.Remove(sb.Length - 1, 1);
            sb.Remove(0, 1);

            var rootDic = ExtractKeyValuePairsFromJsonObject(sb);

            if (rootDic.Count != 1)
            {
                throw new DSInvalidNodeTypeException(rootDic.Count, 1);
            }

            string rootKey = rootDic.Keys.First();

            var rootNode = new DSNode(rootKey, DSNodeType.InnerNode, DSNodePropertyType.Class);

            StringBuilder childSb = new (rootDic[rootKey]);
            ChildrenToNode(rootNode, childSb);

            return rootNode;
        }

        /// <summary>
        /// Converts a json string to children of a node
        /// </summary>
        /// <param name="parent">Parent node</param>
        /// <param name="sb">Stringbuilder</param>
        private static void ChildrenToNode(DSNode parent, StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(parent);

            // Check if string (parent) is a json object
            if (IsStringJsonObject(sb.ToString()))
            {           
                // Extract key, value pairs
                var dic = ExtractKeyValuePairsFromJsonObject(sb);

                var currPropType = DSNodePropertyType.Undefined;

                foreach(var keyValuepair in dic)
                {
                    // Convert key to int key
                    string key = keyValuepair.Key;

                    string? strValue = keyValuepair.Value;

                    if (key == JsonConstants.Version)
                    {
                        continue;
                    }

                    // Check if key is key for property info
                    if (key == JsonConstants.PropertyTypeKey)
                    {
                        currPropType = ParsePropertyTypeInfo(strValue);
                        parent.SetPropertyType(currPropType);
                        continue;
                    }

                    // Check if value is null
                    if (strValue == null)
                    {
                        DSNode child = new(key, null, DSNodeType.Leaf, DSNodePropertyType.Null);
                        parent.AppendChild(child);
                        continue;
                    }
                    else if (IsStringJsonObject(strValue))
                    {
                        // Check if type is class
                        if (currPropType == DSNodePropertyType.Class)
                        {
                            DSNode child = new(key, DSNodeType.InnerNode, DSNodePropertyType.Class);
                            StringBuilder sbChild = new(strValue);
                            ChildrenToNode(child, sbChild);
                            parent.AppendChild(child);
                        }
                        else if (currPropType == DSNodePropertyType.Dictionary)
                        {
                            StringBuilder sbChild = new(strValue);
                            DictionaryToNode(parent, sbChild);
                        }
                        else
                        {
                            throw new DSInvalidNodeTypeException(currPropType);
                        }
                    }
                    else if (currPropType == DSNodePropertyType.List)
                    {
                        StringBuilder sbChild = new(strValue);
                        ListToNode(parent, sbChild);
                    }
                    else // Primitive
                    {
                        DSNode child = new(key, strValue, DSNodeType.Leaf, DSNodePropertyType.Primitive);
                        parent.AppendChild(child);
                    }
                }
            }
            else
            {
                throw new DSInvalidJSONException(sb.ToString());
            }
        }

        /// <summary>
        /// Converts dictionaty to node
        /// </summary>
        /// <param name="parent">Parent node</param>
        /// <param name="sb">Stringbuilder</param>
        private static void DictionaryToNode(DSNode parent, StringBuilder sb)
        {
            ///     (node) (Dictionary)
            ///       |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (KeyValuePairs)
            ///  :     |     :
            ///  :    (D)    :  (Value of KeyvaluePairs)
            
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(parent);

            // Check if string (parent) is a json object
            if (IsStringJsonObject(sb.ToString()))
            {
                // Convert key to int key
                var dic = ExtractKeyValuePairsFromJsonObject(sb);

                int dicId = -1;

                foreach (var keyValuepair in dic)
                {
                    dicId++;
                    string key = keyValuepair.Key;

                    DSNode keyValuePairNode = new(key, DSNodeType.InnerNode, DSNodePropertyType.KeyValuePair); ;
                    DSNode? keyValuePairNodeValue = null;

                    string? strValue = keyValuepair.Value;

                    if (strValue == null)
                    {
                        keyValuePairNodeValue = new(key, null, DSNodeType.Leaf, DSNodePropertyType.KeyValuePairValue);
                        keyValuePairNode.AppendChild(keyValuePairNodeValue);
                        parent.AppendChild(keyValuePairNode);
                        continue;
                    }
                    else if (IsStringJsonObject(strValue))
                    {
                        keyValuePairNodeValue = new(key, null, DSNodeType.InnerNode, DSNodePropertyType.KeyValuePairValue);
                        StringBuilder sbChild = new(strValue);
                        ChildrenToNode(keyValuePairNodeValue, sbChild);
                        keyValuePairNodeValue.SetPropertyType(DSNodePropertyType.KeyValuePairValue);
                    }
                    else if (IsStringJsonList(strValue))
                    {
                        StringBuilder sbChild = new(strValue);
                        ListToNode(parent, sbChild);
                    }
                    else // Primitive
                    {
                        keyValuePairNodeValue = new(key, strValue, DSNodeType.Leaf, DSNodePropertyType.KeyValuePairValue);
                    }

                    keyValuePairNode.AppendChild(keyValuePairNodeValue);
                    parent.AppendChild(keyValuePairNode);
                }
            }
            else
            {
                throw new DSInvalidJSONException(sb.ToString());
            }
        }

        /// <summary>
        /// Converts list to node
        /// </summary>
        /// <param name="parent">Parent node</param>
        /// <param name="sb">Stringbuilder</param>
        private static void ListToNode(DSNode parent, StringBuilder sb)
        {
            ///      (node) (List)
            ///        |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (Items)
            
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(parent);

            // Check if type is list => error
            if (parent.PropType != DSNodePropertyType.List)
            {
                throw new DSInvalidNodeTypeException(parent.PropType);
            }

            // Check if list is list of primitive or objects
            if (sb[1] == JsonConstants.Quote || sb[1] == JsonConstants.N)
            {
                // Extract primitive list
                var items = ExtractPrimitiveList(sb);
                for (int i = 0; i < items.Count; i++)
                {
                    string? value = items[i];
                    var child = new DSNode(i.ToString(), value, DSNodeType.Leaf, DSNodePropertyType.Primitive);
                    parent.AppendChild(child);
                }
            }
            else
            {
                // Extract object list
                var items = ExtractObjectList(sb);
                for (int i = 0; i < items.Count; i++)
                {
                    DSNode childNode = new(i.ToString(), DSNodeType.InnerNode, DSNodePropertyType.Class);
                    StringBuilder sbChild = new(items[i]);
                    ChildrenToNode(childNode, sbChild);
                    parent.AppendChild(childNode);
                }
            }
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
                if (c == JsonConstants.Quote)
                {
                    StringBuilder sb2 = new ();

                    // Extract value
                    i = AppendStringValue(sb2, i, sb.ToString());

                    // Remove opening and closing value
                    sb2.Remove(0, 1);
                    sb2.Remove(sb2.Length - 1, 1);

                    // Add value to result
                    result.Add(sb2.ToString());
                }
                else if (c == JsonConstants.N)
                {
                    if (i + 3 > sb.Length - 1) throw new DSInvalidJSONException(sb.ToString());

                    i++;
                    if (sb[i] != JsonConstants.U) throw new DSInvalidJSONException(sb.ToString());
                    i++;
                    if (sb[i] != JsonConstants.L) throw new DSInvalidJSONException(sb.ToString());
                    i++;
                    if (sb[i] != JsonConstants.L) throw new DSInvalidJSONException(sb.ToString());

                    // Add value to result
                    result.Add(null);
                }
            }

            return result;
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
                if (c == JsonConstants.Quote && keyFound == false)
                {
                    // Quote is opening
                    keyFound = true;

                    StringBuilder sb2 = new();
                    i = AppendStringValue(sb2, i, sb.ToString());

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
                else if (c == JsonConstants.Quote && keyFound == true)
                {
                    // value is found
                    keyFound = false;

                    StringBuilder sb2 = new();
                    i = AppendStringValue(sb2, i, sb.ToString());

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
                else if (c == JsonConstants.N && keyFound == true)
                {
                    // value is found => null
                    keyFound = false;

                    if (i + 3 > sb.Length -1) throw new DSInvalidJSONException(sb.ToString());

                    i++;
                    if (sb[i] != JsonConstants.U) throw new DSInvalidJSONException(sb.ToString());
                    i++;
                    if (sb[i] != JsonConstants.L) throw new DSInvalidJSONException(sb.ToString());
                    i++;
                    if (sb[i] != JsonConstants.L) throw new DSInvalidJSONException(sb.ToString());

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
                else if (c == JsonConstants.Quote)
                {
                    StringBuilder sb = new();
                    i = AppendStringValue(sb, i, jsonString);
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
                else if (c == JsonConstants.Quote)
                {
                    StringBuilder sb = new ();
                    i = AppendStringValue(sb, i, jsonString);
                    continue;
                }
            }

            throw new DSInvalidJSONException(jsonString);
        }

        /// <summary>
        /// Apends the whole string from starting quote to end quote to
        /// the sting
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="startIndex">Index of the opeing quote</param>
        /// <param name="jsonString">string</param>
        /// <returns>Index of the closing quote</returns>
        private static int AppendStringValue(StringBuilder sb, int startIndex, string jsonString)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new DSInvalidJSONException(jsonString);
            }

            if (jsonString.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (jsonString[startIndex] != JsonConstants.Quote)
            {
                throw new DSInvalidJSONException(jsonString);
            }

            sb.Append(JsonConstants.Quote);

            for (int j = startIndex + 1; j < jsonString.Length; j++)
            {
                var c2 = jsonString[j];
                if (c2 == '\\')
                {
                    sb.Append(c2);
                    sb.Append(jsonString[j + 1]);
                    j++;
                }
                if (c2 == JsonConstants.Quote)
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

        /// <summary>
        /// Removes all whitespaces inside a string
        /// except is whitespace is between quotes.
        /// </summary>
        /// <param name="jsonString">String</param>
        /// <returns>String without whitespaces.</returns>
        private static string RemoveWhiteSpace(string jsonString)
        {
            // Check if value has value
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                return jsonString;
            }

            StringBuilder sb = new();
            int stringLength = jsonString.Length;

            for (int i = 0; i < stringLength; i++)
            {
                var c = jsonString[i];

                // If char is a quoto extract everything
                // till the closing quote is reached
                if (c == JsonConstants.Quote)
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
            string tmp = RemoveWhiteSpace(str);

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
            string tmp = RemoveWhiteSpace(str);

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
        /// Parse the node property type info
        /// </summary>
        /// <param name="value">string</param>
        /// <returns>DSNodePropertyType</returns>
        private static DSNodePropertyType ParsePropertyTypeInfo(string? value)
        {
            // Check if value has value
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DSInvalidJSONException();
            }

            return value.ConvertToDSNodePropertyType();
        }
    }
}
