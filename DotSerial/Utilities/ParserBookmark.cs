namespace DotSerial.Utilities
{
    /// <summary>
    /// Help struct for slicing spans for parsing.
    /// </summary>
    internal readonly struct ParserBookmark
    {
        internal readonly int End;
        internal readonly string? Key = null;
        internal readonly int RowEnd;
        internal readonly int RowStart;
        internal readonly int Start;

        internal ParserBookmark(int start, int end)
        {
            Start = start;
            End = end;
        }

        internal ParserBookmark(int start, int end, string? key)
            : this(start, end)
        {
            Key = key;
        }

        internal ParserBookmark(ReadOnlySpan<char> content, bool trimContent)
        {
            if (false == trimContent)
            {
                Start = 0;
                End = content.Length - 1;
            }
            else
            {
                if (ReadOnlySpanMethods.GetTrimParameter(content, out int startTrim, out int endTrim))
                {
                    Start = startTrim;
                    End = content.Length - 1 - endTrim;
                }
                else
                {
                    Start = 0;
                    End = content.Length - 1;
                }
            }
        }

        /// <summary>
        /// Trims the Bookmark, so starting and trailing whitespaces are removed
        /// </summary>
        /// <param name="bookmark">Untrimed bookmark</param>
        /// <param name="content">Content</param>
        /// <returns>Trimed Bookmark</returns>
        internal static ParserBookmark Trim(ParserBookmark bookmark, ReadOnlySpan<char> content)
        {
            var bookmarkContent = bookmark.GetContent(content);

            if (ReadOnlySpanMethods.GetTrimParameter(bookmarkContent, out int startTrim, out int endTrim))
            {
                return new ParserBookmark(bookmark.Start + startTrim, bookmark.End - endTrim, bookmark.Key);
            }
            else
            {
                return new ParserBookmark(bookmark.Start, bookmark.End, bookmark.Key);
            }
        }

        /// <summary>
        /// Slices the content, dependetent of the bookmark
        /// </summary>
        /// <param name="content">Content</param>
        /// <returns>Sliced ReadOnlySpan</returns>
        internal readonly ReadOnlySpan<char> GetContent(ReadOnlySpan<char> content)
        {
            if (content.Length < End)
            {
                throw new ArgumentOutOfRangeException();
            }

            return ReadOnlySpanMethods.SliceFromTo(content, Start, End);
        }

        /// <summary>
        /// Check if value represents a null value
        /// </summary>
        /// <returns>True, if parsed content was null.</returns>
        internal readonly bool IsNull()
        {
            return End == -1;
        }

        public static readonly ParserBookmark Empty = new(-1, -1);
    }
}
