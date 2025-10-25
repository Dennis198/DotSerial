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
using DotSerial.Core.Misc;
using DotSerial.Core.Tree;
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
            string tmp = ParseMethods.RemoveWhiteSpace(jsonString);
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

                    if (key == GeneralConstants.Version)
                    {
                        continue;
                    }

                    // Check if key is key for property info
                    if (key == GeneralConstants.PropertyTypeKey)
                    {
                        // TODO Da von Reihenfolge abhängig gefährllich!!!!
                        currPropType = ParseMethods.ParsePropertyTypeInfo(strValue);
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
                var dic = ExtractKeyValuePairsFromJsonObject(sb);

                foreach (var keyValuepair in dic)
                {
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
                        // TODO KOMME ICH HIER JEMALS RAUS???
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
            if (sb[1] == GeneralConstants.Quote || sb[1] == GeneralConstants.N)
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

    }
}
