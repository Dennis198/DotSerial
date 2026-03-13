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
        /// <param name="content">String content</param>
        /// <returns>List of ParseBookmarks</returns>
        internal static List<ParserBookmark> ExtractKeyValuePairsFromJsonObject(ReadOnlySpan<char> content)
        {
            var result = new List<ParserBookmark>();
            bool startObjectSymbolFound = false;
            bool keyFound = false;
            string foundKey = string.Empty;

            for (int i = 0; i < content.Length; i++)
            {
                char c = content[i];

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

                    int j = ReadOnlySpanMethods.SkipQuotedValue(content, i);

                    foundKey = content.Slice(i, j - i + 1).ToString();

                    i = j;
                }
                // Check if opening quote for the value is found (primitive)
                else if (c == CommonConstants.Quote && keyFound == true)
                {
                    // value is found
                    keyFound = false;

                    int j = ReadOnlySpanMethods.SkipQuotedValue(content, i);

                    var tmp = new ParserBookmark(i, j + 1, foundKey);
                    result.Add(tmp);

                    foundKey = string.Empty;
                    i = j;
                }
                // Check if opening symbol for the value is found (json object)
                else if (c == JsonConstants.ObjectStart && keyFound == true)
                {
                    // value is found
                    keyFound = false;

                    int j = ExtractJsonObject(content, i);

                    var tmp = new ParserBookmark(i, j, foundKey);
                    result.Add(tmp);

                    foundKey = string.Empty;
                    i = j;
                }
                // Check if opening symbol for the value is found (json list)
                else if (c == JsonConstants.ListStart && keyFound == true)
                {
                    // value is found
                    keyFound = false;

                    int j = ExtractJsonList(content, i);

                    var tmp = new ParserBookmark(i, j, foundKey);
                    result.Add(tmp);

                    foundKey = string.Empty;
                    i = j;
                }
                else if (keyFound == true)
                {
                    keyFound = false;

                    if (true == ReadOnlySpanMethods.EqualsNullString(content, i))
                    {
                        var tmp = new ParserBookmark(i, -1, foundKey);
                        result.Add(tmp);

                        foundKey = string.Empty;
                        i += 3;
                    }
                    else
                    {
                        int j = ReadOnlySpanMethods.SkipTillStopChars(content, i, JsonConstants.ParseStopChars, true);

                        var tmp = new ParserBookmark(i, j, foundKey);
                        result.Add(tmp);

                        foundKey = string.Empty;
                        i = j;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts object list from json string
        /// </summary>
        /// <param name="content">String content</param>
        /// <returns>List of ParserBookmark</returns>
        internal static List<ParserBookmark> ExtractObjectList(ReadOnlySpan<char> content)
        {
            var list = new List<ParserBookmark>();

            for (int i = 1; i < content.Length - 1; i++)
            {
                char c = content[i];

                if (char.IsWhiteSpace(c) || c == CommonConstants.Comma || c == JsonConstants.KeyValueSeperator)
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    int j = ReadOnlySpanMethods.SkipQuotedValue(content, i);

                    var tmp = new ParserBookmark(i, j + 1);
                    list.Add(tmp);

                    i = j;
                }
                else if (c == JsonConstants.ListStart)
                {
                    int j = ExtractJsonList(content, i);

                    if (false == IsEmptyList(content, i, j))
                    {
                        var tmp = new ParserBookmark(i, j);
                        list.Add(tmp);
                    }

                    i = j;
                }
                else if (c == JsonConstants.ObjectStart)
                {
                    int j = ExtractJsonObject(content, i);

                    if (false == IsEmptyObject(content, i, j))
                    {
                        var tmp = new ParserBookmark(i, j);
                        list.Add(tmp);
                    }

                    i = j;
                }
                else
                {
                    int j = ReadOnlySpanMethods.SkipTillStopChars(content, i, JsonConstants.ParseStopChars);

                    var tmp = new ParserBookmark(i, j);
                    list.Add(tmp);

                    i = j;
                }
            }

            return list;
        }

        /// <summary>
        /// Check if string is a json list.
        /// </summary>
        /// <param name="content">String content</param>
        /// <returns>True, if is a list</returns>
        internal static bool IsStringJsonList(ReadOnlySpan<char> content)
        {
            // // Check if first element is '['
            bool startFound = ReadOnlySpanMethods.EqualFirstNoWhiteSpaceChar(content, JsonConstants.ListStart);
            // // Check if last element is ']'
            bool endFound = ReadOnlySpanMethods.EqualLastNoWhiteSpaceChar(content, JsonConstants.ListEnd);

            return startFound && endFound;
        }

        /// <summary>
        /// Check if string is a json object.
        /// </summary>
        /// <param name="content">String content</param>
        /// <returns>True, if is a object</returns>
        internal static bool IsStringJsonObject(ReadOnlySpan<char> content)
        {
            // // Check if first element is '{'
            bool startFound = ReadOnlySpanMethods.EqualFirstNoWhiteSpaceChar(content, JsonConstants.ObjectStart);
            // // Check if last element is '}'
            bool endFound = ReadOnlySpanMethods.EqualLastNoWhiteSpaceChar(content, JsonConstants.ObjectEnd);

            return startFound && endFound;
        }

        /// <summary>
        /// Extracts a json list
        /// </summary>
        /// <param name="content">String content</param>
        /// <param name="startIndex">Index of the opeing symbol</param>
        /// <returns>Index of end symbol</returns>
        private static int ExtractJsonList(ReadOnlySpan<char> content, int startIndex)
        {
            if (content.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (content[startIndex] != JsonConstants.ListStart)
            {
                throw new DSJsonException("Invalid json.");
            }

            int i;
            for (i = startIndex + 1; i < content.Length; i++)
            {
                char c = content[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    i = ReadOnlySpanMethods.SkipQuotedValue(content, i);
                }
                else if (c == JsonConstants.ListStart)
                {
                    i = ExtractJsonList(content, i);
                }
                else if (c == JsonConstants.ObjectStart)
                {
                    i = ExtractJsonObject(content, i);
                }
                else if (c == JsonConstants.ListEnd)
                {
                    i++;
                    break;
                }
            }

            return i;
        }

        /// <summary>
        /// Extracts a json object
        /// </summary>
        /// <param name="content">String content</param>
        /// <param name="startIndex">Index of the opeing symbol</param>
        /// <returns>Index of end symbol</returns>
        private static int ExtractJsonObject(ReadOnlySpan<char> content, int startIndex)
        {
            if (content.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (content[startIndex] != JsonConstants.ObjectStart)
            {
                throw new DSJsonException("Invalid json.");
            }

            int i;
            for (i = startIndex + 1; i < content.Length; i++)
            {
                char c = content[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    i = ReadOnlySpanMethods.SkipQuotedValue(content, i);
                }
                else if (c == JsonConstants.ListStart)
                {
                    i = ExtractJsonList(content, i);
                }
                else if (c == JsonConstants.ObjectStart)
                {
                    i = ExtractJsonObject(content, i);
                }
                else if (c == JsonConstants.ObjectEnd)
                {
                    i++;
                    break;
                }
            }

            return i;
        }

        /// <summary>
        /// Check if content is empty list
        /// </summary>
        /// <param name="content">String content</param>
        /// <param name="startIndex">Start index</param>
        /// <param name="endIndex">End index</param>
        /// <returns>True, if content is empty list</returns>
        private static bool IsEmptyList(ReadOnlySpan<char> content, int startIndex, int endIndex)
        {
            if (content[startIndex] != JsonConstants.ListStart && content[endIndex] != JsonConstants.ListEnd)
            {
                throw new DSJsonException("Invalid json.");
            }

            for (int i = startIndex + 1; i < content.Length - 1; i++)
            {
                char c = content[i];

                if (false == char.IsWhiteSpace(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if content is empty object
        /// </summary>
        /// <param name="content">String content</param>
        /// <param name="startIndex">Start index</param>
        /// <param name="endIndex">End index</param>
        /// <returns>True, if content is empty object</returns>
        private static bool IsEmptyObject(ReadOnlySpan<char> content, int startIndex, int endIndex)
        {
            if (content[startIndex] != JsonConstants.ObjectStart && content[endIndex] != JsonConstants.ObjectEnd)
            {
                throw new DSJsonException("Invalid json.");
            }

            for (int i = startIndex + 1; i < content.Length - 1; i++)
            {
                char c = content[i];

                if (false == char.IsWhiteSpace(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
