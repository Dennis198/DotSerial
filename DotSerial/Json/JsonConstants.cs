using DotSerial.Common;

namespace DotSerial.Json
{
    /// <summary>
    /// Constants for json writing and parsing
    /// </summary>
    internal static class JsonConstants
    {
        /// <summary>
        /// Indentation Size
        /// </summary>
        internal const int IndentationSize = 2;

        /// <summary>
        /// Json object start char
        /// </summary>
        internal const char ObjectStart = '{';

        /// <summary>
        /// Jsoobject end char
        /// </summary>
        internal const char ObjectEnd = '}';

        /// <summary>
        /// Json list start char
        /// </summary>
        internal const char ListStart = '[';

        /// <summary>
        /// Json list end char
        /// </summary>
        internal const char ListEnd = ']';

        /// <summary>
        /// Json key value seperator char
        /// </summary>
        internal const char KeyValueSeperator = ':';

        /// <summary>
        /// Chars which must be escaped
        /// </summary>
        internal static readonly char[] CharsToEscape = [CommonConstants.Quote, CommonConstants.Backslash];

        /// <summary>
        /// Chars that indicate the end of a value when parsing a json string. This is used to determine where a value ends when parsing without quotes.
        /// </summary>
        internal static readonly char[] ParseStopChars =
        [
            ObjectStart,
            ObjectEnd,
            ListStart,
            ListEnd,
            CommonConstants.Comma,
            KeyValueSeperator,
        ];
    }
}
