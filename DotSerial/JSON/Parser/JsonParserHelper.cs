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

using System.Text;
using DotSerial.Common;
using DotSerial.Utilities;
using DotSerial.Json;
using DotSerial.Tree.Nodes;
using DotSerial.Tree;

namespace DotSerial.JSON.Parser
{
    /// <summary>
    /// Helper class with methode for parsing json.
    /// </summary>
    public static class JsonParserHelper
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <summary>
        /// Extracts key value pairs from json object
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <returns>Dictionary<string, string></returns>
        internal static Dictionary<string, StringBuilder?> ExtractKeyValuePairsFromJsonObject(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            var result = new Dictionary<string, StringBuilder?>();
            bool keyFound = false;
            string foundKey = string.Empty;

            for (int i = 0; i < sb.Length; i++)
            {
                char c = sb[i];

                // Check if opening quote for the key is found
                if (c == CommonConstants.Quote && keyFound == false)
                {
                    // Quote is opening
                    keyFound = true;

                    StringBuilder sb2 = new();
                    i = ParseMethods.AppendStringValue(sb2, i, sb.ToString());

                    // Remove opening and closing quote
                    sb2.Remove(0, 1);
                    sb2.Remove(sb2.Length - 1, 1);

                    // Save key
                    foundKey = sb2.ToString();

                    // Add key
                    result.Add(foundKey, null);
                }
                // Check if opening quote for the value is found (primitive)
                else if (c == CommonConstants.Quote && keyFound == true)
                {
                    // value is found
                    keyFound = false;

                    StringBuilder sb2 = new();
                    i = ParseMethods.AppendStringValue(sb2, i, sb.ToString());

                    // Remove opening and closing quote
                    sb2.Remove(0, 1);
                    sb2.Remove(sb2.Length - 1, 1);
                   
                    if (false == result.ContainsKey(foundKey))
                    {
                        throw new DSJsonException("Key not found.");
                    }

                    // Add key
                    result[foundKey] = sb2;

                    // Reset found key
                    foundKey = string.Empty;
                }
                // Check if "null" value is found  
                else if (c == CommonConstants.N && keyFound == true)
                {
                    // value is found => null
                    keyFound = false;

                    if (i + 3 > sb.Length -1) throw new DSJsonException("Invalid json");

                    i++;
                    if (sb[i] != CommonConstants.U) throw new DSJsonException("Invalid json");
                    i++;
                    if (sb[i] != CommonConstants.L) throw new DSJsonException("Invalid json");
                    i++;
                    if (sb[i] != CommonConstants.L) throw new DSJsonException("Invalid json");

                    // Add key
                    result[foundKey] = null;

                    // Reset found key
                    foundKey = string.Empty;
                }
                // Check if opening symbol for the value is found (json object)
                else if (c == JsonConstants.ObjectStart && keyFound == true)
                {
                    // value is found
                    keyFound = false;

                    // Extract value
                    int j = ExtractJsonObject(sb, i);

                    if (false == result.ContainsKey(foundKey))
                    {
                        throw new DSJsonException("Key not found.");
                    }

                    // Add key
                    int len = j - i + 1;
                    result[foundKey] = sb.SubString(i, len);

                    // Reset found key
                    foundKey = string.Empty;

                    // Update index
                    i = j;
                }
                // Check if opening symbol for the value is found (json list)
                else if (c == JsonConstants.ListStart && keyFound == true)
                {
                    // value is found
                    keyFound = false;

                    // Extract value
                    int j = ExtractJsonList(sb, i);

                    if (false == result.ContainsKey(foundKey))
                    {
                        throw new DSJsonException("Key not found.");
                    }
                    // Add key
                    int len = j - i + 1;
                    result[foundKey] = sb.SubString(i, len);

                    // Reset found key
                    foundKey = string.Empty;

                    // Update index
                    i = j;
                }
            }

            return result;
        }  

        /// <summary>
        /// Extracts a json object
        /// </summary>
        /// <param name="sb">Strginbuilder</param>
        /// <param name="startIndex">Index of the opeing symbol</param>
        /// <returns>Index of end symbol</returns>
        private static int ExtractJsonObject(StringBuilder sb, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (sb.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (sb[startIndex] != JsonConstants.ObjectStart)
            {
                throw new DSJsonException("Invalid json.");
            }

            int numberNewObjects = 0;

            for (int i = startIndex + 1; i < sb.Length; i++)
            {
                char c = sb[i];
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
                else if (c == CommonConstants.Quote)
                {
                    StringBuilder _ = new();
                    i = ParseMethods.AppendStringValue(_, i, sb.ToString());
                    continue;
                }
            }

            throw new DSJsonException("Invalid json.");
        }    

        /// <summary>
        /// Extracts a json list
        /// </summary>
        /// <param name="sb">StingBuilder</param>
        /// <param name="startIndex">Index of the opeing symbol</param>
        /// <returns>Index of end symbol</returns>
        private static int ExtractJsonList(StringBuilder sb, int startIndex)
        {
            if (sb.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (sb[startIndex] != JsonConstants.ListStart)
            {
                throw new DSJsonException("Invalid json.");
            }

            int numberNewObjects = 0;

            for (int i = startIndex + 1; i < sb.Length; i++)
            {
                char c = sb[i];
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
                else if (c == CommonConstants.Quote)
                {
                    StringBuilder _ = new ();
                    i = ParseMethods.AppendStringValue(_, i, sb.ToString());
                    continue;
                }
            }

            throw new DSJsonException("Invalid json.");
        }  

        /// <summary>
        /// Extracts object list from json string
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <returns>List<string></returns>
        internal static List<StringBuilder> ExtractObjectList(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            var list = new List<StringBuilder>();

            for (int i = 1; i < sb.Length-1; i++)
            {
                char c = sb[i];

                if (c == JsonConstants.ListStart)
                {
                     // Extract value
                    int j = ExtractJsonList(sb, i);

                    // Add key
                    int len = j - i + 1;
                    var tmp = sb.SubString(i, len);

                    // Add object to result
                    list.Add(tmp);

                    i = j;
                }
                // Check if opening symbol is found
                else if (c == JsonConstants.ObjectStart)
                {
                    // Extract object
                    int j = ExtractJsonObject(sb, i);

                    int len = j - i + 1;
                    var tmp = sb.SubString(i, len);

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
        internal static bool IsStringJsonObject(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            bool startFound = false;
            bool endFound = false;

            // Check if first element is '{'
            for (int i = 0; i < sb.Length; i++)
            {
                char c = sb[i]; 
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == JsonConstants.ObjectStart)
                {
                    startFound = true;
                    break;
                }
                else
                {
                    return false;
                }
            }

            // Check if last element is '}'
            for (int i = sb.Length - 1; i >= 0; i--)
            {
                char c = sb[i]; 
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == JsonConstants.ObjectEnd)
                {
                    endFound = true;
                    break;
                }
                else 
                {
                    return false;
                }
            }

            return startFound && endFound;
        }  

        /// <summary>
        /// Check if string is a json list.
        /// </summary>
        /// <param name="sb">StrginBuilder</param>
        /// <returns>True, if is a list</returns>
        internal static bool IsStringJsonList(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            bool startFound = false;
            bool endFound = false;

            // Check if first element is '{'
            for (int i = 0; i < sb.Length; i++)
            {
                char c = sb[i]; 
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == JsonConstants.ListStart)
                {
                    startFound = true;
                    break;
                }
                else
                {
                    return false;
                }
            }

            // Check if last element is '}'
            for (int i = sb.Length - 1; i >= 0; i--)
            {
                char c = sb[i]; 
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == JsonConstants.ListEnd)
                {
                    endFound = true;
                    break;
                }
                else 
                {
                    return false;
                }
            }

            return startFound && endFound;
        }      

        /// <summary>
        /// Extracts list of primitives from json string
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <returns>List<string></returns>
        internal static List<string?> ExtractPrimitiveList(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            var result = new List<string?>();

            for (int i = 0; i < sb.Length; i++)
            {
                char c = sb[i];

                // Check if opening quote is found
                if (c == CommonConstants.Quote)
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
                else if (c == CommonConstants.N)
                {
                    if (i + 3 > sb.Length - 1) throw new DSJsonException("Invalid json");

                    i++;
                    if (sb[i] != CommonConstants.U) throw new DSJsonException("Invalid json");
                    i++;
                    if (sb[i] != CommonConstants.L) throw new DSJsonException("Invalid json");
                    i++;
                    if (sb[i] != CommonConstants.L) throw new DSJsonException("Invalid json");

                    // Add value to result
                    result.Add(null);
                }
            }

            return result;
        }

        /// <summary>
        /// Parses primitive node without a key, e.g "3.14"
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="startIndex">StartIndex</param>
        /// <returns>Leafnode</returns>
        internal static IDSNode ParsePrimitiveNode(StringBuilder sb, int startIndex)
        {
            if (sb.IsNullOrWhiteSpace() || sb.EqualsNullString())
            {
                return _nodeFactory.CreateNode(CommonConstants.MainObjectKey, null, NodeType.Leaf);
            }

            StringBuilder sbPrim = new();

            int i = ParseMethods.AppendStringValue(sbPrim, startIndex, sb.ToString());
            if (i != sb.Length -1)
            {
                throw new NotImplementedException();
            }

            // Remove opening and closing quote
            sbPrim.Remove(0, 1);
            sbPrim.Remove(sbPrim.Length - 1, 1);
            string nodeValue = sbPrim.ToString();
            
            return _nodeFactory.CreateNode(CommonConstants.MainObjectKey, nodeValue, NodeType.Leaf);
        }
    }
}