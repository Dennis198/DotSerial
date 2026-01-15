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

namespace DotSerial.JSON.Parser
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
        internal static List<StringBuilder?> ExtractObjectList(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            var list = new List<StringBuilder?>();

            for (int i = 1; i < sb.Length - 1; i++)
            {
                char c = sb[i];

                if (c == CommonConstants.Quote)
                {
                    StringBuilder sbValue = new ();
                    int j = ParseMethods.AppendStringValue(sbValue, i, sb.ToString());

                    // Add key
                    int len = j - i + 1;
                    var tmp = sb.SubString(i, len);

                    // Add object to result
                    list.Add(tmp);

                    i = j;
                }
                else if (c == JsonConstants.ListStart)
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
                    list.Add(null);
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
            
            // // Check if first element is '{'
            bool startFound = sb.EqualFirstNoWhiteSpaceChar(JsonConstants.ObjectStart);
            // // Check if last element is '}'
            bool endFound = sb.EqualLastNoWhiteSpaceChar(JsonConstants.ObjectEnd);

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

            // // Check if first element is '['
            bool startFound = sb.EqualFirstNoWhiteSpaceChar(JsonConstants.ListStart);
            // // Check if last element is ']'
            bool endFound = sb.EqualLastNoWhiteSpaceChar(JsonConstants.ListEnd);

            return startFound && endFound;
        }      
    }
}