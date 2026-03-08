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

namespace DotSerial.Json.Parser
{
    /// <summary>
    /// Helper class with methode for parsing json.
    /// </summary>
    internal static class JsonParserHelper
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
            bool startObjectSymbolFound = false;
            bool keyFound = false;
            string foundKey = string.Empty;            

            for (int i = 0; i < sb.Length; i++)
            {
                char c = sb[i];

                if (c == JsonConstants.ObjectStart && false == startObjectSymbolFound)
                {
                    startObjectSymbolFound = true;
                    continue;
                }

                if (char.IsWhiteSpace(c) || c == CommonConstants.Comma || c == JsonConstants.KeyValueSeperator)
                {
                    continue;
                }

                if (false == startObjectSymbolFound)
                {
                    continue;
                }

                // Check if opening quote for the key is found
                if (c == CommonConstants.Quote && keyFound == false)
                {
                    // Quote is opening
                    keyFound = true;

                    StringBuilder sb2 = new();
                    i = ParseMethods.AppendStringValue(sb2, i, sb);

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
                    i = ParseMethods.AppendStringValue(sb2, i, sb);
                   
                    if (false == result.ContainsKey(foundKey))
                    {
                        throw new DSJsonException("Key not found.");
                    }

                    // Add key
                    result[foundKey] = sb2;

                    // Reset found key
                    foundKey = string.Empty;
                }
                // Check if opening symbol for the value is found (json object)
                else if (c == JsonConstants.ObjectStart && keyFound == true)
                {
                    // value is found
                    keyFound = false;

                    if (false == result.ContainsKey(foundKey))
                    {
                        throw new DSJsonException("Key not found.");
                    }

                    // Extract value
                    int j = ExtractJsonObject(sb, i, out StringBuilder tmp);

                    result[foundKey] = tmp;

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

                    if (false == result.ContainsKey(foundKey))
                    {
                        throw new DSJsonException("Key not found.");
                    }

                     // Extract value
                    int j = ExtractJsonList(sb, i, out StringBuilder tmp);

                    // Add object to result
                    result[foundKey] = tmp;

                    // Reset found key
                    foundKey = string.Empty;

                    // Update index
                    i = j;
                }
                else if (keyFound == true)
                {
                    // value is found => null
                    keyFound = false;

                    if (true == sb.EqualsNullString(i))
                    {
                        i += 3;
                        result[foundKey] = null;
                    }
                    else
                    {
                        StringBuilder sb2 = new();
                        i = ParseMethods.AppendTillStopChars(sb2, i, sb, JsonConstants.ParseStopChars);
                    
                        if (false == result.ContainsKey(foundKey))
                        {
                            throw new DSJsonException("Key not found.");
                        }

                        // Add key
                        result[foundKey] = sb2;

                        // Reset found key
                        foundKey = string.Empty;
                    }
                }
            }

            return result;
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

                if (char.IsWhiteSpace(c) || c == CommonConstants.Comma || c == JsonConstants.KeyValueSeperator)
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    int j = sb.SkipStringValue(i);

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
                    int j = ExtractJsonList(sb, i, out StringBuilder tmp);

                    // Add object to result
                    list.Add(tmp);

                    i = j;
                }
                // Check if opening symbol is found
                else if (c == JsonConstants.ObjectStart)
                {
                    // Extract object
                    int j = ExtractJsonObject(sb, i, out StringBuilder tmp);

                    // Add object to result
                    list.Add(tmp);

                    // Update index
                    i = j;
                }
                else
                {
                    int j = sb.SkipTillStopChars(i, JsonConstants.ParseStopChars);

                    // Add key
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

        /// <summary>
        /// Extracts a json object
        /// </summary>
        /// <param name="sb">Strginbuilder</param>
        /// <param name="startIndex">Index of the opeing symbol</param>
        /// <returns>Index of end symbol</returns>
        private static int ExtractJsonObject(StringBuilder sb, int startIndex, out StringBuilder objContent)
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

            objContent = new StringBuilder();
            objContent.Append(JsonConstants.ObjectStart);
            int i;
            for (i = startIndex + 1; i < sb.Length; i++)
            {
                char c = sb[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    StringBuilder sb2 = new();
                    i = ParseMethods.AppendStringValue(sb2, i, sb);

                    objContent.Append(sb2);
                }
                else if (c == JsonConstants.ListStart)
                {
                    i = ExtractJsonList(sb, i, out StringBuilder sb2);

                    objContent.Append(sb2);
                }
                else if (c == JsonConstants.ObjectStart)
                {
                    i = ExtractJsonObject(sb, i, out StringBuilder sb2);

                    objContent.Append(sb2);
                }
                else if (c == JsonConstants.ObjectEnd)
                {
                    objContent.Append(c);
                    break;
                }
                else 
                {
                    objContent.Append(c);
                }
            }

            return i;
        }    

        /// <summary>
        /// Extracts a json list
        /// </summary>
        /// <param name="sb">StingBuilder</param>
        /// <param name="startIndex">Index of the opeing symbol</param>
        /// <returns>Index of end symbol</returns>
        private static int ExtractJsonList(StringBuilder sb, int startIndex, out StringBuilder listContent)
        {
            if (sb.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (sb[startIndex] != JsonConstants.ListStart)
            {
                throw new DSJsonException("Invalid json.");
            }

            listContent = new StringBuilder();
            listContent.Append(JsonConstants.ListStart);
            int i;
            for (i = startIndex + 1; i < sb.Length; i++)
            {
                char c = sb[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    StringBuilder sb2 = new();
                    i = ParseMethods.AppendStringValue(sb2, i, sb);

                    listContent.Append(sb2);
                }
                else if (c == JsonConstants.ListStart)
                {
                    i = ExtractJsonList(sb, i, out StringBuilder sb2);

                    listContent.Append(sb2);
                }
                else if (c == JsonConstants.ObjectStart)
                {
                    i = ExtractJsonObject(sb, i, out StringBuilder sb2);

                    listContent.Append(sb2);
                }
                else if (c == JsonConstants.ListEnd)
                {
                    listContent.Append(c);
                    break;
                }
                else 
                {
                    listContent.Append(c);
                }
            }

            return i;
        }            
    }
}