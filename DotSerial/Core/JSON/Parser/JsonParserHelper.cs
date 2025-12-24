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
using DotSerial.Core.Exceptions.JSON;
using DotSerial.Core.General;
using DotSerial.Core.Misc;

namespace DotSerial.Core.JSON.Parser
{
    /// <summary>
    /// Helper class with methode for parsing json.
    /// </summary>
    public static class JsonParserHelper
    {
        /// <summary>
        /// Extracts key value pairs from json object
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <returns>Dictionary<string, string></returns>
        internal static Dictionary<string, string?> ExtractKeyValuePairsFromJsonObject(StringBuilder sb)
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
        internal static List<string> ExtractObjectList(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            var list = new List<string>();

            for (int i = 1; i < sb.Length-1; i++)
            {
                char c = sb[i];

                if (c == JsonConstants.ListStart)
                {
                     // Extract value
                    int j = ExtractJsonList(sb.ToString(), i);

                    // Add key
                    int len = j - i + 1;
                    string tmp = sb.ToString(i, len);

                    // Add object to result
                    list.Add(tmp);

                    i = j;
                }
                // Check if opening symbol is found
                else if (c == JsonConstants.ObjectStart)
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
        internal static bool IsStringJsonObject(string? str)
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
        internal static bool IsStringJsonList(string str)
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
        internal static List<string?> ExtractPrimitiveList(StringBuilder sb)
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