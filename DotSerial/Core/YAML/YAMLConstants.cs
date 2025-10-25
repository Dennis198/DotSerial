namespace DotSerial.Core.YAML
{
    internal static class YAMLConstants
    {
        /// <summary>
        /// Indentation Size
        /// </summary>
        internal const int IndentationSize = 2;
        /// <summary>
        /// Start string of yaml document
        /// </summary>
        internal const string YAMLDocumentStart = "---";
        /// <summary>
        /// End string of yaml document
        /// </summary>
        internal const string YAMLDocumentEnd = "...";
        /// <summary>
        /// End string of yaml document
        /// </summary>
        internal const string ListItemIndicator = "-";
        /// <summary>
        /// End string of yaml document
        /// </summary>
        internal const char KeyValueSeperator = ':';          
        /// <summary>
        /// Json null string
        /// </summary>
        internal const string NullListItem = "- null";   
    }
}