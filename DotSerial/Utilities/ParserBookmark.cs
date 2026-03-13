namespace DotSerial.Utilities
{
    /// <summary>
    /// Help struct for slicing spans for parsing.
    /// </summary>
    internal readonly struct ParserBookmark
    {
        internal readonly int End;
        internal readonly string? Key = null;
        internal readonly int Line = 0;
        internal readonly int Start;

        internal ParserBookmark(int start, int end)
        {
            Start = start;
            End = end;
        }

        internal ParserBookmark(int start, int end, string key)
            : this(start, end)
        {
            Key = key;
        }

        /// <summary>
        /// Check if value represents a null value
        /// </summary>
        /// <returns>True, if parsed content was null.</returns>
        internal readonly bool IsNull()
        {
            return End == -1;
        }
    }
}
