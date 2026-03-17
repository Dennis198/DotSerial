using DotSerial.Common;

namespace DotSerial.Utilities
{
    internal class MulitLineReadOnlySpan
    {
        // Special case. (Currenly only needed in yaml/toon)
        internal bool IsOneLineObject = false;

        internal string? KeyLine = null;

        /// <summary> Internal data </summary>
        private readonly List<ParserBookmark>? _lines = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        internal MulitLineReadOnlySpan(ReadOnlySpan<char> content)
        {
            _lines = CreateLines(content);
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="lines"Lists of StringBuilder></param>
        private MulitLineReadOnlySpan(List<ParserBookmark> lines)
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

            if (_lines == null)
            {
                return str;
            }

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
            if (null == _lines)
            {
                throw new NotImplementedException();
            }

            _lines.Clear();
        }

        /// <summary>
        /// Get a specific line
        /// </summary>
        /// <param name="i">Index of line</param>
        /// <returns>Line as a StringBuilder</returns>
        internal ParserBookmark GetLine(int i)
        {
            if (null == _lines)
            {
                throw new NotImplementedException();
            }

            if (i < 0 || i > _lines.Count - 1)
            {
                throw new NotImplementedException();
            }

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
            return content.Slice(bookmark.Start, bookmark.End - bookmark.Start + 1);
        }

        /// <summary>
        /// Sets the line i with a new bookmark.
        /// </summary>
        /// <param name="i">Index</param>
        /// <param name="bookmark">ParserBookmark</param>
        internal void SetLine(int i, ParserBookmark bookmark)
        {
            if (null == _lines)
            {
                throw new NotImplementedException();
            }

            if (0 > i || i > _lines.Count - 1)
            {
                throw new NotImplementedException();
            }

            _lines[i] = bookmark;
        }

        /// <summary>
        /// Creates a shallow copy of a range of lines.
        /// </summary>
        /// <param name="start">Start index</param>
        /// <param name="end">End index</param>
        /// <returns>A shallow copy of a range of elements in the source.</returns>
        internal MulitLineReadOnlySpan SliceFromTo(int start, int end)
        {
            if (null == _lines)
            {
                throw new NotImplementedException();
            }

            if (end < start)
            {
                throw new NotImplementedException();
            }

            if (start < 0 || end > _lines.Count - 1)
            {
                throw new NotImplementedException();
            }

            int length = end - start + 1;
            var subLines = _lines.Slice(start, length);
            return new MulitLineReadOnlySpan(subLines);
        }

        /// <summary>
        /// Create a list of stringbuilders representing each lines of the
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
                    result.Add(new ParserBookmark(currStart, currEnd));
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

            result.Add(new ParserBookmark(currStart, content.Length - 1));

            return result;
        }
    }
}
