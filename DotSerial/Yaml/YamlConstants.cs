using DotSerial.Common;

namespace DotSerial.Yaml
{
    /// <summary>
    /// Yaml constants
    /// </summary>
    internal static class YamlConstants
    {
        /// <summary>
        /// Indentation Size
        /// </summary>
        internal const int IndentationSize = 2;
        /// <summary>
        /// Start string of yaml document
        /// </summary>
        internal const string YamlDocumentStart = "---";
        /// <summary>
        /// End string of yaml document
        /// </summary>
        internal const string YamlDocumentEnd = "...";
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
        /// Special chars which must be quoted
        /// </summary>
        internal static readonly char[] YamlSpecialChars = ['-', '?', ':', ',', '[', ']', '{', '}', '#', '&', '*', '!', '|', '>', '\'', '"', '%', '@', '`'];
    }
}