using DotSerial.Common;

namespace DotSerial.Toon
{
    internal static class ToonConstants
    {
        /// <summary>
        /// Indentation Size
        /// </summary>
        internal const int IndentationSize = 2;
        /// <summary>
        /// End string of yaml document
        /// </summary>
        internal const char ListItemIndicator = '-';
        /// <summary>
        /// End string of yaml document
        /// </summary>
        internal const char KeyValueSeperator = ':';
        /// <summary>
        /// Chars which must be escaped
        /// </summary>
        internal readonly static char[] CharsToEscape = [CommonConstants.Quote, CommonConstants.Backslash];        
        /// <summary>
        /// Specials chars which must be quoted
        /// </summary>
        internal static readonly char[] ToonSpecialChars = ['-', ':', ',', '[', ']', '{', '}', '\'', '"', '`'];
    }
}