namespace DotSerial.Toon.Writer
{
    /// <summary>
    /// Additional options for node visitors.
    /// </summary>
    internal struct ToonWriterOptions(int level, bool addKey = true, int numberOfListPrefix = 0)
    {
        /// <summary>
        /// Indentation level
        /// </summary>
        internal int Level { get; private set; } = level;
        /// <summary>
        /// Add Key to object
        /// </summary>
        internal bool AddKey { get; private set; } = addKey;
        /// <summary>
        /// Number of ListItem Indicator to add.
        /// </summary>
        internal int NumberOfPrefix = numberOfListPrefix;
        private static readonly string? s_ListItemIndicator = ToonConstants.ListItemIndicator.ToString();

        /// <summary>
        /// Returns the ListItemIndicator(s) Prefix.
        /// </summary>
        /// <returns>ListItemIndicator(s)</returns>
        internal readonly string? GetPrefix()
        {
            if (NumberOfPrefix < 1)
            {
                return null;
            }

            string result = string.Empty;
            result += string.Format("{0} ", s_ListItemIndicator);    

            return result;
        }
    }
}