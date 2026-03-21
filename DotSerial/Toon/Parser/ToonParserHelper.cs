using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Toon.Parser
{
    /// <summary>
    /// Helper class with methode for parsing toon.
    /// </summary>
    internal static class ToonParserHelper
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <summary>
        /// Extracts key value pairs from yaml object
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <returns>Dictionary<string, MulitLineReadOnlySpan></returns>
        internal static Dictionary<string, MulitLineParserBookmark> ExtractKeyValuePairsFromToonObject(
            MulitLineParserBookmark lines,
            ReadOnlySpan<char> content
        )
        {
            ArgumentNullException.ThrowIfNull(lines);

            var result = new Dictionary<string, MulitLineParserBookmark>();
            int objLevel = ParseMethods.LineLevel(lines.GetLineContent(0, content), ToonConstants.IndentationSize);

            for (int i = 0; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                var line = lines.GetLineContent(i, content);
                int currLevel = ParseMethods.LineLevel(line, ToonConstants.IndentationSize);
                if (currLevel < objLevel)
                {
                    break;
                }

                // "Key":
                if (IsKeyLine(line))
                {
                    string? key = ExtractKeyFromLine(line);

                    if (string.IsNullOrWhiteSpace(key))
                    {
                        throw new DSToonException("Invalid toon");
                    }

                    int sIndex = i + 1;
                    int eIndex;
                    if (
                        sIndex >= lines.Count
                        || currLevel
                            >= ParseMethods.LineLevel(
                                lines.GetLineContent(sIndex, content),
                                ToonConstants.IndentationSize
                            )
                    )
                    {
                        sIndex = i;
                        eIndex = i;
                    }
                    else
                    {
                        eIndex = GetEndIndexOfToonObject(lines, i, content);
                    }

                    var helpObj = lines.SliceFromTo(sIndex, eIndex);

                    if (sIndex == eIndex && !IsToonList(helpObj, content))
                    {
                        // Special case an object with exaclty one item.
                        // Must be marked, otherwise there is no way to
                        // differentiated between an object or a simple
                        // Key, Value pair for an primitive.
                        helpObj.IsOneLineObject = true;
                    }

                    // Set Key line
                    // Needed for list parsing
                    helpObj.KeyLine = line.ToString();

                    result.Add(key, helpObj);
                    i = eIndex;
                }
                // "Key": "Value"
                else
                {
                    string? key = ExtractKeyFromLine(line);

                    if (string.IsNullOrWhiteSpace(key))
                    {
                        throw new DSToonException("Invalid toon");
                    }

                    var helpObj = lines.SliceFromTo(i, i);

                    result.Add(key, helpObj);
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts list of objcts from toon string
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <returns>List<MulitLineReadOnlySpan></returns>
        internal static List<MulitLineParserBookmark> ExtractObjectList(
            MulitLineParserBookmark lines,
            ReadOnlySpan<char> content
        )
        {
            ArgumentNullException.ThrowIfNull(lines);

            var result = new List<MulitLineParserBookmark>();
            int objLevel = ParseMethods.LineLevel(lines.GetLineContent(0, content), ToonConstants.IndentationSize);

            for (int i = 0; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                var line = lines.GetLineContent(i, content);
                int currLevel = ParseMethods.LineLevel(line, ToonConstants.IndentationSize);
                if (currLevel < objLevel)
                {
                    break;
                }

                if (-1 != ParseListCount(line))
                {
                    // "Key":
                    if (IsKeyLine(line))
                    {
                        int sIndex = i + 1;
                        int eIndex;
                        if (
                            sIndex >= lines.Count
                            || currLevel
                                >= ParseMethods.LineLevel(
                                    lines.GetLineContent(sIndex, content),
                                    ToonConstants.IndentationSize
                                )
                        )
                        {
                            sIndex = i;
                            eIndex = i;
                        }
                        else
                        {
                            eIndex = GetEndIndexOfToonObject(lines, i, content);
                        }

                        var helpObj = lines.SliceFromTo(sIndex, eIndex);
                        // Set Key line
                        helpObj.KeyLine = line.ToString();

                        result.Add(helpObj);
                        i = eIndex;
                    }
                    else
                    {
                        int endIndex = GetEndIndexOfToonObject(lines, i, content);
                        var helpObj = lines.SliceFromTo(i, endIndex);
                        result.Add(helpObj);
                        i = endIndex;
                    }
                }
                else if (ReadOnlySpanMethods.EqualFirstNoWhiteSpaceChar(line, CommonConstants.Minus))
                {
                    // Has no list count, => Must be an object

                    int endIndex = GetEndIndexOfToonObject(lines, i, content);
                    var helpObj = lines.SliceFromTo(i, endIndex);

                    if (i == endIndex)
                    {
                        // Special case an object with exaclty one item.
                        // Must be marked, otherwise there is no way to
                        // differentiated between an object or a simple
                        // Key, Value pair for an primitive.
                        helpObj.IsOneLineObject = true;
                    }

                    RemoveListItemIndicator(helpObj, content);
                    result.Add(helpObj);
                    i = endIndex;
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts the value from a line
        /// </summary>
        /// <param name="lines">ReadOnlySpan</param>
        /// <returns>Value of the line</returns>
        internal static string? ExtractValueFromLine(ReadOnlySpan<char> line)
        {
            int start = -1;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    i = ReadOnlySpanMethods.SkipQuotedValue(line, i);
                }
                else if (c == ToonConstants.KeyValueSeperator)
                {
                    start = i;
                    break;
                }
                else
                {
                    i = ReadOnlySpanMethods.SkipTillStopChar(line, i, ToonConstants.KeyValueSeperator);
                }
            }

            if (start == -1)
            {
                throw new NotImplementedException();
            }

            // Skip seperator
            start++;

            for (int i = start; i < line.Length; i++)
            {
                var c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    int j = ReadOnlySpanMethods.SkipQuotedValue(line, i);
                    var tmp = ReadOnlySpanMethods.SliceFromTo(line, i, j);
                    return tmp.ToString();
                }
                else
                {
                    int j = ReadOnlySpanMethods.SkipTillStopChar(line, i, null);
                    var tmp = ReadOnlySpanMethods.SliceFromTo(line, i, j);
                    return tmp.ToString();
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if String is a empty toon list.
        /// </summary>
        /// <param name="line">ReadOnlySpan</param>
        /// <returns>true, if empty toon list</returns>
        internal static bool IsEmptyList(ReadOnlySpan<char> line)
        {
            int count = ParseListCount(line);

            return 0 == count;
        }

        /// <summary>
        /// Check if MulitLineReadOnlySpan is an empty toon object.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>True, if empty objcet</returns>
        internal static bool IsEmptyObject(MulitLineParserBookmark lines, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count != 1)
            {
                return false;
            }

            var line = lines.GetLineContent(0, content);

            if (-1 != ParseListCount(line))
            {
                return false;
            }

            if (false == ReadOnlySpanMethods.EqualLastNoWhiteSpaceChar(line, ToonConstants.KeyValueSeperator))
            {
                return false;
            }

            for (int i = line.Length - 1; i >= 0; i--)
            {
                char c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == ToonConstants.KeyValueSeperator)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if string is a primitive list
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        internal static bool IsPrimitiveList(MulitLineParserBookmark lines, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count != 1)
            {
                return false;
            }

            var firstLine = lines.GetLineContent(0, content);

            if (ReadOnlySpanMethods.EqualLastNoWhiteSpaceChar(firstLine, ToonConstants.KeyValueSeperator))
            {
                return false;
            }

            if (0 < ParseListCount(firstLine))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if MulitLineReadOnlySpan is a toon schema list
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <returns>True, if list wioth schema</returns>
        internal static bool IsSchemaList(MulitLineParserBookmark lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (string.IsNullOrWhiteSpace(lines.KeyLine))
            {
                return false;
            }

            var firstLine = lines.KeyLine.AsSpan();
            if (1 > ParseListCount(firstLine))
            {
                return false;
            }

            bool startSchemaFound = false;
            bool endSchemaFound = false;

            for (int i = 0; i < firstLine.Length; i++)
            {
                char c = firstLine[i];

                if (char.IsWhiteSpace(c) || c == CommonConstants.Comma)
                {
                    continue;
                }

                if (c == ToonConstants.KeyValueSeperator)
                {
                    break;
                }
                else if (c == CommonConstants.Quote)
                {
                    i = ReadOnlySpanMethods.SkipQuotedValue(firstLine, i);
                }
                else if (c == CommonConstants.BracesStart)
                {
                    if (startSchemaFound)
                    {
                        throw new NotImplementedException();
                    }
                    startSchemaFound = true;
                }
                else if (c == CommonConstants.BracesEnd)
                {
                    if (false == startSchemaFound)
                    {
                        throw new NotImplementedException();
                    }
                    endSchemaFound = true;
                }
                else
                {
                    i = ReadOnlySpanMethods.SkipTillStopChars(
                        firstLine,
                        i,
                        [CommonConstants.Comma, CommonConstants.BracesStart, CommonConstants.BracesEnd]
                    );
                }
            }

            return startSchemaFound && endSchemaFound;
        }

        /// <summary>
        /// Check if MulitLineReadOnlySpan is a list
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <returns>True, if list</returns>
        internal static bool IsToonList(
            MulitLineParserBookmark lines,
            ReadOnlySpan<char> content,
            bool rootElement = false
        )
        {
            ArgumentNullException.ThrowIfNull(lines);

            var firstLine = lines.GetLineContent(0, content);

            if (false == string.IsNullOrWhiteSpace(lines.KeyLine))
            {
                if (-1 != ParseListCount(lines.KeyLine))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (lines.Count != 1)
                {
                    return false;
                }

                if (-1 != ParseListCount(firstLine))
                {
                    if (rootElement)
                    {
                        if (null == ExtractKeyFromLine(firstLine))
                        {
                            return true;
                        }
                        else
                        {
                            // Root element must be a list, otherwise there would
                            // be no way to differentiate between a list and an object with one item.
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Check if MulitLineReadOnlySpan is a toon object
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <returns>True, if yaml object</returns>
        internal static bool IsToonObject(
            MulitLineParserBookmark lines,
            ReadOnlySpan<char> content,
            bool isRootElement = false
        )
        {
            ArgumentNullException.ThrowIfNull(lines);

            // "'key':"
            if (IsEmptyObject(lines, content))
            {
                return true;
            }

            if (IsToonList(lines, content, isRootElement))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if MulitLineReadOnlySpan is a key only a key value pair
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <returns>True, if yaml key value pair</returns>
        internal static bool IsToonPrimitiveLine(MulitLineParserBookmark lines, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count != 1)
            {
                return false;
            }

            if (lines.IsOneLineObject)
            {
                return false;
            }

            if (IsToonList(lines, content))
            {
                return false;
            }

            if (IsToonSingleValue(lines, content))
            {
                return false;
            }

            var firstLine = lines.GetLineContent(0, content);
            int start = -1;
            for (int i = 0; i < firstLine.Length; i++)
            {
                char c = firstLine[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    i = ReadOnlySpanMethods.SkipQuotedValue(firstLine, i);
                }
                else if (c == ToonConstants.KeyValueSeperator)
                {
                    start = i;
                    break;
                }
                else
                {
                    i = ReadOnlySpanMethods.SkipTillStopChar(firstLine, i, ToonConstants.KeyValueSeperator);
                }
            }

            bool valueWasFound = false;

            for (int i = start; i < firstLine.Length; i++)
            {
                var c = firstLine[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (valueWasFound)
                {
                    throw new NotImplementedException();
                }

                if (c == CommonConstants.Quote)
                {
                    i = ReadOnlySpanMethods.SkipQuotedValue(firstLine, i);
                    valueWasFound = true;
                }
                else
                {
                    i = ReadOnlySpanMethods.SkipTillStopChar(firstLine, i, null);
                    valueWasFound = true;
                }
            }

            return valueWasFound;
        }

        /// <summary>
        /// Check if MulitLineReadOnlySpan is a single value
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <returns>True, if toon single value</returns>
        internal static bool IsToonSingleValue(MulitLineParserBookmark lines, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count != 1)
            {
                return false;
            }

            var firstLine = lines.GetLineContent(0, content);

            // "null"
            if (ReadOnlySpanMethods.EqualsNullString(firstLine))
            {
                return true;
            }

            // "'Key': "
            if (IsKeyLine(firstLine))
            {
                return false;
            }

            bool seperatorFound = false;
            bool keyFound = false;

            for (int i = 0; i < firstLine.Length; i++)
            {
                char c = firstLine[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (true == keyFound && c == ToonConstants.KeyValueSeperator)
                {
                    seperatorFound = true;
                    continue;
                }

                if (seperatorFound)
                {
                    return false;
                }

                if (c == CommonConstants.Quote)
                {
                    i = ReadOnlySpanMethods.SkipQuotedValue(firstLine, i);
                    keyFound = true;
                }
                else
                {
                    i = ReadOnlySpanMethods.SkipTillStopChar(firstLine, i, ToonConstants.KeyValueSeperator);
                    keyFound = true;
                }
            }

            return true;
        }

        /// <summary>
        /// Parses values which are sepearted by comma.
        /// </summary>
        /// <param name="sb">ReadOnlySpan</param>
        /// <param name="startIndex">StartIndex for parsing</param>
        /// <returns>List of the values</returns>
        internal static List<string> ParseCommaSeperateValues(ReadOnlySpan<char> line, int startIndex = 0)
        {
            List<string> result = [];
            for (int i = startIndex; i < line.Length; i++)
            {
                char c = line[i];

                if (char.IsWhiteSpace(c) || c == CommonConstants.Comma)
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    int j = ReadOnlySpanMethods.SkipQuotedValue(line, i);
                    var tmp = ReadOnlySpanMethods.SliceFromTo(line, i, j);
                    result.Add(tmp.ToString());
                    i = j;
                }
                else
                {
                    int j = ReadOnlySpanMethods.SkipTillStopChar(line, i, CommonConstants.Comma);
                    var tmp = ReadOnlySpanMethods.SliceFromTo(line, i, j);
                    result.Add(tmp.ToString());
                    i = j;
                }
            }

            return result;
        }

        /// <summary>
        /// Parses the list count of a toon list from a string
        /// </summary>
        /// <param name="line">ReadOnlySpan</param>
        /// <returns>Count, if the toon list count indicator is there, -1 otherwise.</returns>
        internal static int ParseListCount(ReadOnlySpan<char> line)
        {
            bool countIndicatorStartFound = false;
            bool countIndicatorEndFound = false;
            string countAsString = string.Empty;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == CommonConstants.Quote)
                {
                    if (true == countIndicatorStartFound)
                    {
                        throw new NotImplementedException();
                    }

                    i = ReadOnlySpanMethods.SkipQuotedValue(line, i);
                }
                else if (c == CommonConstants.BracketsStart)
                {
                    if (true == countIndicatorStartFound)
                    {
                        throw new NotImplementedException();
                    }
                    countIndicatorStartFound = true;
                }
                else if (c == CommonConstants.BracketsEnd)
                {
                    if (false == countIndicatorStartFound)
                    {
                        throw new NotImplementedException();
                    }
                    countIndicatorEndFound = true;
                    break;
                }
                else if (char.IsDigit(c))
                {
                    if (true == countIndicatorStartFound)
                    {
                        countAsString += c;
                    }
                    else
                    {
                        i = ReadOnlySpanMethods.SkipTillStopChars(
                            line,
                            i,
                            [ToonConstants.KeyValueSeperator, CommonConstants.BracketsStart]
                        );
                    }
                }
                else if (c == ToonConstants.KeyValueSeperator)
                {
                    break;
                }
                else
                {
                    if (true == countIndicatorStartFound)
                    {
                        throw new NotImplementedException();
                    }

                    i = ReadOnlySpanMethods.SkipTillStopChars(
                        line,
                        i,
                        [ToonConstants.KeyValueSeperator, CommonConstants.BracketsStart]
                    );
                }
            }

            if (false == countIndicatorStartFound || false == countIndicatorEndFound)
            {
                return -1;
            }

            if (false == int.TryParse(countAsString, out int listCount))
            {
                return -1;
            }

            if (listCount < 0)
            {
                return -1;
            }

            return listCount;
        }

        /// <summary>
        /// Parse primitive list
        /// </summary>
        /// <param name="node">Listnode</param>
        /// <param name="line">ReadOnlySpan</param>
        internal static void ParsePrimitiveList(ListNode node, ReadOnlySpan<char> line)
        {
            ArgumentNullException.ThrowIfNull(node);

            int count = ParseListCount(line);
            var lItems = ExtractPrimitiveListFromLine(line);

            if (count != lItems.Count)
            {
                throw new NotImplementedException();
            }

            for (int i = 0; i < count; i++)
            {
                var listNode = _nodeFactory.CreateNodeFromString(
                    StategyType.Toon,
                    i.ToString(),
                    lItems[i],
                    NodeType.Leaf
                );
                node.AddChild(listNode);
            }
        }

        /// <summary>
        /// Parses a toon schema list and adds the objects to the node
        /// </summary>
        /// <param name="node">ListNode</param>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        internal static void ParseSchemaList(ListNode node, MulitLineParserBookmark lines, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(lines);

            var keys = ParseSchemaKeys(lines, content);

            if (keys.Count == 0)
            {
                throw new NotImplementedException();
            }

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines.GetLineContent(i, content);
                var child = _nodeFactory.CreateNodeFromString(StategyType.Toon, i.ToString(), null, NodeType.InnerNode);
                var values = ParseCommaSeperateValues(line, 0);

                if (keys.Count != values.Count)
                {
                    throw new NotImplementedException();
                }

                for (int j = 0; j < keys.Count; j++)
                {
                    string key = keys[j];
                    string value = values[j];
                    var childChild = _nodeFactory.CreateNodeFromString(StategyType.Toon, key, value, NodeType.Leaf);
                    child.AddChild(childChild);
                }

                node.AddChild(child);
            }
        }

        /// <summary>
        /// Extracts the key from a line
        /// </summary>
        /// <param name="lines">ReadOnlySpan</param>
        /// <returns>Key of the line</returns>
        private static string? ExtractKeyFromLine(ReadOnlySpan<char> line)
        {
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    int j = ReadOnlySpanMethods.SkipQuotedValue(line, i);
                    return ReadOnlySpanMethods.SliceFromTo(line, i, j).ToString();
                }
                else if (c == CommonConstants.BracketsStart)
                {
                    return null;
                }
                else
                {
                    int j = ReadOnlySpanMethods.SkipTillStopChars(
                        line,
                        i,
                        [ToonConstants.KeyValueSeperator, CommonConstants.BracketsStart]
                    );
                    return ReadOnlySpanMethods.SliceFromTo(line, i, j).ToString();
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses a primitive list from a line
        /// </summary>
        /// <param name="line">ReadOnlySpan</param>
        /// <returns>Values in a list</returns>
        private static List<string?> ExtractPrimitiveListFromLine(ReadOnlySpan<char> line)
        {
            List<string?> result = [];
            bool seperatorFound = false;

            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (char.IsWhiteSpace(c) || c == CommonConstants.Comma)
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    if (seperatorFound)
                    {
                        int j = ReadOnlySpanMethods.SkipQuotedValue(line, i);
                        var tmp = ReadOnlySpanMethods.SliceFromTo(line, i, j);
                        result.Add(tmp.ToString());
                        i = j;
                    }
                    else
                    {
                        i = ReadOnlySpanMethods.SkipQuotedValue(line, i);
                    }
                }
                else if (c == ToonConstants.KeyValueSeperator)
                {
                    if (seperatorFound)
                    {
                        throw new DSToonException("Invalid toon");
                    }
                    seperatorFound = true;
                }
                else
                {
                    if (seperatorFound)
                    {
                        int j = ReadOnlySpanMethods.SkipTillStopChar(line, i, CommonConstants.Comma);
                        var tmp = ReadOnlySpanMethods.SliceFromTo(line, i, j);
                        result.Add(tmp.ToString());
                        i = j;
                    }
                    else
                    {
                        i = ReadOnlySpanMethods.SkipTillStopChars(
                            line,
                            i,
                            [CommonConstants.Comma, ToonConstants.KeyValueSeperator]
                        );
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the end index of an yaml object.
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <param name="startIndex">Start index of the object</param>
        /// <returns>End index</returns>
        private static int GetEndIndexOfToonObject(
            MulitLineParserBookmark lines,
            int startIndex,
            ReadOnlySpan<char> content
        )
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count - 1 < startIndex)
            {
                throw new NotImplementedException();
            }

            int endIndex = -1;
            int objLevel = ParseMethods.LineLevel(
                lines.GetLineContent(startIndex, content),
                ToonConstants.IndentationSize
            );

            for (int i = startIndex + 1; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                int currLevel = ParseMethods.LineLevel(lines.GetLineContent(i, content), ToonConstants.IndentationSize);
                if (currLevel <= objLevel)
                {
                    endIndex = i - 1;
                    break;
                }
            }

            if (endIndex == -1)
            {
                endIndex = lines.Count - 1;
            }

            return endIndex;
        }

        /// <summary>
        /// Check if line is a key line
        /// </summary>
        /// <param name="lines">ReadOnlySpan</param>
        /// <returns>True, if line is key line</returns>
        private static bool IsKeyLine(ReadOnlySpan<char> line)
        {
            return ReadOnlySpanMethods.EqualLastNoWhiteSpaceChar(line, ToonConstants.KeyValueSeperator);
        }

        /// <summary>
        /// Parses the schema keys from the key line of a toon schema list
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private static List<string> ParseSchemaKeys(MulitLineParserBookmark lines, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (string.IsNullOrWhiteSpace(lines.KeyLine))
            {
                throw new NotImplementedException();
            }

            var firstLine = lines.KeyLine.AsSpan();

            int start = firstLine.IndexOf(CommonConstants.BracesStart.ToString(), 0);

            if (start == -1 || firstLine[start] != CommonConstants.BracesStart)
            {
                throw new NotImplementedException();
            }

            List<string> result = [];

            for (int i = start + 1; i < firstLine.Length; i++)
            {
                char c = firstLine[i];

                if (char.IsWhiteSpace(c) || c == CommonConstants.Comma)
                {
                    continue;
                }

                if (c == ToonConstants.KeyValueSeperator || c == CommonConstants.BracesEnd)
                {
                    break;
                }
                else if (c == CommonConstants.Quote)
                {
                    int j = ReadOnlySpanMethods.SkipQuotedValue(firstLine, i);
                    var tmp = ReadOnlySpanMethods.SliceFromTo(firstLine, i, j);
                    result.Add(tmp.ToString());
                    i = j;
                }
                else
                {
                    int j = ReadOnlySpanMethods.SkipTillStopChars(
                        firstLine,
                        i,
                        [CommonConstants.Comma, CommonConstants.BracesStart, CommonConstants.BracesEnd]
                    );
                    var tmp = ReadOnlySpanMethods.SliceFromTo(firstLine, i, j);
                    result.Add(tmp.ToString());
                    i = j;
                }
            }

            return result;
        }

        /// <summary>
        /// Removes the List indicator for the objects
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        private static void RemoveListItemIndicator(MulitLineParserBookmark lines, ReadOnlySpan<char> content)
        {
            var startBookMark = lines.GetLine(0);
            var startLine = lines.GetLineContent(0, content);
            int index =
                ParseMethods.LineLevel(startLine, ToonConstants.IndentationSize) * ToonConstants.IndentationSize;

            if (startLine[index] != ToonConstants.ListItemIndicator)
            {
                throw new NotImplementedException();
            }

            var newBookmark = new ParserBookmark(startBookMark.Start + index + 1, startBookMark.End);
            lines.SetLine(0, newBookmark);
        }
    }
}
