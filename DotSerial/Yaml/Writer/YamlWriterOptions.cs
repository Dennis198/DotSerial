namespace DotSerial.Yaml.Writer
{
    /// <summary>
    /// Additional options for node visitors.
    /// </summary>
    /// <param name="level">Level of indication</param>
    /// <param name="addKey">Add key</param>
    /// <param name="numberOfListPrefix">Number of listitem indicators to add.</param>
    internal struct YamlWriterOptions(int level, bool addKey = true, int numberOfListPrefix = 0)
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
        private static readonly string? s_ListItemIndicator = YamlConstants.ListItemIndicator.ToString();

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

            for (int i = 0; i < NumberOfPrefix; i++)
            {
                result += string.Format("{0} ", s_ListItemIndicator);
            }

            return result;
        }

        /// <summary>
        /// Decreases the number of ListItemIndicator(s)
        /// </summary>
        internal void DecreasePrefixCount()
        {
            NumberOfPrefix--;

            if (NumberOfPrefix < 0)
            {
                NumberOfPrefix = 0;
            }
        }
    }
}
