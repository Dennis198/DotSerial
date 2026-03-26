using DotSerial.Common;

namespace DotSerial.Utilities
{
    internal class MulitLineParserBookmark
    {
        // Special case. (Currenly only needed in yaml/toon)
        internal bool IsOneLineObject = false;

        internal string? KeyLine = null;

        /// <summary> Internal data </summary>
        private readonly List<ParserBookmark> _lines;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sb">DotSerialStringBuilder</param>
        internal MulitLineParserBookmark(ReadOnlySpan<char> content)
        {
            _lines = CreateLines(content);
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="lines"Lists of ParserBookmark></param>
        private MulitLineParserBookmark(List<ParserBookmark> lines)
        {
            _lines = lines;
        }

        /// <summary>
        /// Number of lines
        /// </summary>
        internal int Count => null != _lines ? _lines.Count : throw new NotImplementedException();

        /// <summary>
        /// Returns the content of the MuliLineReadOnlySpan
        /// </summary>
        /// <param name="content">String content</param>
        /// <returns>Content of the MultiLineReadOnlySpan</returns>
        public string ToString(ReadOnlySpan<char> content)
        {
            string str = string.Empty;

            for (int i = 0; i < _lines.Count; i++)
            {
                var bookmark = _lines[i];
                str +=
                    content.Slice(bookmark.Start, bookmark.End - bookmark.Start + 1).ToString() + Environment.NewLine;
            }

            return str;
        }

        /// <summary>
        /// Clears all lines
        /// </summary>
        internal void Clear()
        {
            _lines.Clear();
        }

        /// <summary>
        /// Get a specific line
        /// </summary>
        /// <param name="i">Index of line</param>
        /// <returns>Line as a ParserBookmark</returns>
        internal ParserBookmark GetLine(int i)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(i);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(i, _lines.Count - 1);

            return _lines[i];
        }

        /// <summary>
        /// Gets the line content at index i
        /// </summary>
        /// <param name="i">Index</param>
        /// <param name="content">String content</param>
        /// <returns>Line content</returns>
        internal ReadOnlySpan<char> GetLineContent(int i, ReadOnlySpan<char> content)
        {
            var bookmark = GetLine(i);
            return bookmark.GetContent(content);
        }

        /// <summary>
        /// Sets the line i with a new bookmark.
        /// </summary>
        /// <param name="i">Index</param>
        /// <param name="bookmark">ParserBookmark</param>
        internal void SetLine(int i, ParserBookmark bookmark)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(i);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(i, _lines.Count - 1);

            _lines[i] = bookmark;
        }

        /// <summary>
        /// Creates a shallow copy of a range of lines.
        /// </summary>
        /// <param name="start">Start index</param>
        /// <param name="end">End index</param>
        /// <returns>A shallow copy of a range of elements in the source.</returns>
        internal MulitLineParserBookmark SliceFromTo(int start, int end)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(start);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(start, end);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(end, _lines.Count - 1);

            int length = end - start + 1;
            var subLines = _lines.Slice(start, length);
            return new MulitLineParserBookmark(subLines);
        }

        /// <summary>
        /// Create a list of ParserBookmark representing each lines of the
        /// yaml string
        /// </summary>
        /// <param name="content">String content</param>
        /// <returns>List<ParserBookmark></returns>
        private static List<ParserBookmark> CreateLines(ReadOnlySpan<char> content)
        {
            List<ParserBookmark> result = [];
            bool createNewLine = false;
            int currStart = 0;
            int currEnd = -1;

            for (int i = 0; i < content.Length; i++)
            {
                if (createNewLine)
                {
                    if (currEnd >= currStart)
                    {
                        // Add the line to the result list
                        result.Add(new ParserBookmark(currStart, currEnd));
                    }

                    createNewLine = false;
                    currStart = i;
                }

                var c = content[i];
                if (c == CommonConstants.Quote)
                {
                    i = ReadOnlySpanMethods.SkipQuotedValue(content, i);
                }
                else if (c.IsNewLine())
                {
                    createNewLine = true;
                    currEnd = i - 1;
                }
            }

            if (currStart < content.Length)
            {
                result.Add(new ParserBookmark(currStart, content.Length - 1));
            }

            return result;
        }
    }
}
