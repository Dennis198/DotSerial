using DotSerial.Common;
using DotSerial.Utilities;

namespace DotSerial.Yaml.Parser
{
    /// <summary>
    /// Helper class with methode for parsing yaml.
    /// </summary>
    internal static class YamlParserHelper
    {
        /// <summary>
        /// Extracts key value pairs from yaml object
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <param name="content">Yaml content</param>
        /// /// <returns>Dictionary<string, MulitLineReadOnlySpan></returns>
        internal static Dictionary<string, MulitLineParserBookmark> ExtractKeyValuePairsFromYamlObject(
            MulitLineParserBookmark lines,
            ReadOnlySpan<char> content
        )
        {
            ArgumentNullException.ThrowIfNull(lines);

            var result = new Dictionary<string, MulitLineParserBookmark>();
            int objLevel = ParseMethods.LineLevel(lines.GetLineContent(0, content), YamlConstants.IndentationSize);

            for (int i = 0; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                var line = lines.GetLineContent(i, content);
                int currLevel = ParseMethods.LineLevel(line, YamlConstants.IndentationSize);
                if (currLevel < objLevel)
                {
                    break;
                }

                // "Key":
                if (IsKeyLine(line))
                {
                    string key = ExtractKeyFromLine(line);

                    int sIndex = i + 1;
                    int eIndex = GetEndIndexOfYamlObject(lines, i, content);

                    var helpObj = lines.SliceFromTo(sIndex, eIndex);

                    if (sIndex == eIndex && !IsYamlList(helpObj, content))
                    {
                        // Special case an object with exaclty one item.
                        // Must be marked, otherwise there is no way to
                        // differentiated between an object or a simple
                        // Key, Value pair for an primitive.
                        helpObj.IsOneLineObject = true;
                    }

                    result.Add(key, helpObj);
                    i = eIndex;
                }
                // "Key": "Value"
                else
                {
                    string key = ExtractKeyFromLine(line);
                    var helpObj = lines.SliceFromTo(i, i);

                    result.Add(key, helpObj);
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts list of objcts from yaml string
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <param name="content">Yaml content</param>
        /// <returns>List<MulitLineReadOnlySpan></returns>
        internal static List<MulitLineParserBookmark> ExtractObjectList(
            MulitLineParserBookmark lines,
            ReadOnlySpan<char> content
        )
        {
            ArgumentNullException.ThrowIfNull(lines);

            var result = new List<MulitLineParserBookmark>();
            int objLevel = ParseMethods.LineLevel(lines.GetLineContent(0, content), YamlConstants.IndentationSize);

            for (int i = 0; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                var line = lines.GetLineContent(i, content);
                int currLevel = ParseMethods.LineLevel(line, YamlConstants.IndentationSize);
                if (currLevel < objLevel)
                {
                    break;
                }

                if (true == ReadOnlySpanMethods.EqualFirstNoWhiteSpaceChar(line, YamlConstants.ListItemIndicator))
                {
                    int endIndex = GetEndIndexOfYamlObject(lines, i, content);

                    // Remove List indicator of the objects
                    RemoveListItemIndicator(lines, i, endIndex, content);

                    var helpObj = lines.SliceFromTo(i, endIndex);
                    result.Add(helpObj);
                    i = endIndex;
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts the value from a line
        /// </summary>
        /// <param name="lines">DotSerialStringBuilder-List</param>
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
                else if (c == YamlConstants.KeyValueSeperator)
                {
                    start = i;
                    break;
                }
                else
                {
                    i = ReadOnlySpanMethods.SkipTillStopChar(line, i, YamlConstants.KeyValueSeperator);
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
                    return ReadOnlySpanMethods.SliceFromTo(line, i, j).ToString();
                }
                else
                {
                    int j = ReadOnlySpanMethods.SkipTillStopChar(line, i, null);
                    return ReadOnlySpanMethods.SliceFromTo(line, i, j).ToString();
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the List indicator for the objects
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <param name="startIndex">Start index of the objcets</param>
        /// <param name="endIndex">End index of the objects</param>
        /// <param name="content">Yaml content</param>
        private static void RemoveListItemIndicator(
            MulitLineParserBookmark lines,
            int startIndex,
            int endIndex,
            ReadOnlySpan<char> content
        )
        {
            var startBookMark = lines.GetLine(startIndex);
            var startLine = ReadOnlySpanMethods.SliceFromTo(content, startBookMark.Start, startBookMark.End);
            int index =
                ParseMethods.LineLevel(startLine, YamlConstants.IndentationSize) * YamlConstants.IndentationSize;

            if (startLine[index] != YamlConstants.ListItemIndicator)
            {
                throw new NotImplementedException();
            }

            var newBookmark = new ParserBookmark(startBookMark.Start + index + 1, startBookMark.End);
            lines.SetLine(startIndex, newBookmark);

            for (int i = startIndex; i <= endIndex; i++)
            {
                var tmpBookmark = lines.GetLine(i);
                var line = ReadOnlySpanMethods.SliceFromTo(content, tmpBookmark.Start, tmpBookmark.End);
                int tmpIndex = 0;

                if (i != startIndex)
                {
                    tmpIndex = index;
                }

                for (int j = tmpIndex; j < tmpIndex + YamlConstants.IndentationSize; j++)
                {
                    if (char.IsWhiteSpace(line[j]))
                    {
                        var tmpNewBookmark = new ParserBookmark(tmpBookmark.Start + j + 1, tmpBookmark.End);
                        lines.SetLine(i, tmpNewBookmark);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Check if MulitLineReadOnlySpan is a yaml object
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <param name="content">Yaml content</param>
        /// <returns>True, if yaml object</returns>
        internal static bool IsYamlObject(MulitLineParserBookmark lines, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var firstLine = lines.GetLineContent(0, content);

            if (null == firstLine)
            {
                throw new NotImplementedException();
            }

            // "'key': {}"
            if (IsEmptyObject(firstLine))
            {
                return true;
            }

            if (IsYamlList(lines, content))
            {
                return false;
            }

            if (IsYamlSingleValue(lines, content))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check if MulitLineReadOnlySpan is a yaml list
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <param name="content">Yaml content</param>
        /// <returns>True, if yaml list</returns>
        internal static bool IsYamlList(MulitLineParserBookmark lines, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var firstLine = lines.GetLineContent(0, content);

            if (null == firstLine)
            {
                throw new NotImplementedException();
            }

            // 1. "'key': []"
            // 2. "[]"
            if (IsEmptyList(firstLine))
            {
                if (lines.Count != 1)
                {
                    return false;
                }
                return true;
            }

            // 1. "- 'item'"
            // 2. "- 'key' : 'item'"
            if (ReadOnlySpanMethods.EqualFirstNoWhiteSpaceChar(firstLine, YamlConstants.ListItemIndicator))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if MulitLineReadOnlySpan is a single value
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <param name="content">Yaml content</param>
        /// <returns>True, if yaml single value</returns>
        internal static bool IsYamlSingleValue(MulitLineParserBookmark lines, ReadOnlySpan<char> content)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count != 1)
            {
                return false;
            }

            var firstLine = lines.GetLineContent(0, content);

            if (IsEmptyList(firstLine) || IsEmptyObject(firstLine))
            {
                return false;
            }

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

                if (true == keyFound && c == YamlConstants.KeyValueSeperator)
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
                    i = ReadOnlySpanMethods.SkipTillStopChar(firstLine, i, YamlConstants.KeyValueSeperator);
                    keyFound = true;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if MulitLineReadOnlySpan is only a key value pair
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <param name="content">Yaml content</param>
        /// <returns>True, if yaml key value pair</returns>
        internal static bool IsYamlPrimitiveLine(MulitLineParserBookmark lines, ReadOnlySpan<char> content)
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

            if (IsEmptyObject(lines.GetLineContent(0, content)))
            {
                return false;
            }

            if (IsYamlList(lines, content))
            {
                return false;
            }

            if (IsYamlSingleValue(lines, content))
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
                else if (c == YamlConstants.KeyValueSeperator)
                {
                    start = i;
                    break;
                }
                else
                {
                    i = ReadOnlySpanMethods.SkipTillStopChar(firstLine, i, YamlConstants.KeyValueSeperator);
                }
            }

            if (start == -1)
            {
                return false;
            }

            // Skip seperator
            start++;

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
        /// Check is string builder is "Key": {}
        /// </summary>
        /// <param name="line">DotSerialStringBuilder</param>
        /// <returns>True, if string is an empty yaml object</returns>
        internal static bool IsEmptyObject(ReadOnlySpan<char> line)
        {
            bool closedBracletFound = false;

            for (int i = line.Length - 1; i >= 0; i--)
            {
                char c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == CommonConstants.BracesEnd)
                {
                    if (true == closedBracletFound)
                    {
                        throw new NotImplementedException();
                    }
                    closedBracletFound = true;
                }
                else if (c == CommonConstants.BracesStart)
                {
                    if (false == closedBracletFound)
                    {
                        throw new NotImplementedException();
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Check is string builder is "Key": []
        /// </summary>
        /// <param name="line">DotSerialStringBuilder</param>
        /// <returns>True, if string is an empty yaml list</returns>
        internal static bool IsEmptyList(ReadOnlySpan<char> line)
        {
            bool closedBracletFound = false;

            for (int i = line.Length - 1; i >= 0; i--)
            {
                char c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == CommonConstants.BracketsEnd)
                {
                    if (true == closedBracletFound)
                    {
                        throw new NotImplementedException();
                    }
                    closedBracletFound = true;
                }
                else if (c == CommonConstants.BracketsStart)
                {
                    if (false == closedBracletFound)
                    {
                        throw new NotImplementedException();
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the yaml start and end symbols if there.
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <param name="content">Yaml content</param>
        /// <returns>MulitLineReadOnlySpan without start end symbols</returns>
        internal static MulitLineParserBookmark RemoveStartStopSymbols(
            MulitLineParserBookmark lines,
            ReadOnlySpan<char> content
        )
        {
            ArgumentNullException.ThrowIfNull(lines);
            if (lines.Count == 0)
            {
                return lines;
            }

            int startIndex = 0;
            int endIndex = lines.Count - 1;

            var firstLine = lines.GetLineContent(0, content);
            if (firstLine.SequenceEqual(YamlConstants.YamlDocumentStart))
            {
                startIndex++;
            }

            var lastLine = lines.GetLineContent(lines.Count - 1, content);
            if (lastLine.SequenceEqual(YamlConstants.YamlDocumentEnd))
            {
                endIndex--;
            }

            if (endIndex < startIndex)
            {
                lines.Clear();
                return lines;
            }

            return lines.SliceFromTo(startIndex, endIndex);
        }

        /// <summary>
        /// Gets the end index of an yaml object.
        /// </summary>
        /// <param name="lines">MulitLineReadOnlySpan</param>
        /// <param name="startIndex">Start index of the object</param>
        /// <param name="content">Yaml content</param>
        /// <returns>End index</returns>
        private static int GetEndIndexOfYamlObject(
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
                YamlConstants.IndentationSize
            );

            for (int i = startIndex + 1; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                int currLevel = ParseMethods.LineLevel(lines.GetLineContent(i, content), YamlConstants.IndentationSize);
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
        /// Extracts the key from a line
        /// </summary>
        /// <param name="lines">DotSerialStringBuilder-List</param>
        /// <returns>Key of the line</returns>
        private static string ExtractKeyFromLine(ReadOnlySpan<char> line)
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
                else
                {
                    int j = ReadOnlySpanMethods.SkipTillStopChar(line, i, YamlConstants.KeyValueSeperator);
                    return ReadOnlySpanMethods.SliceFromTo(line, i, j).ToString();
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if line is a key line
        /// </summary>
        /// <param name="lines">DotSerialStringBuilder-List</param>
        /// <returns>True, if line is key line</returns>
        private static bool IsKeyLine(ReadOnlySpan<char> line)
        {
            return ReadOnlySpanMethods.EqualLastNoWhiteSpaceChar(line, YamlConstants.KeyValueSeperator);
        }
    }
}
