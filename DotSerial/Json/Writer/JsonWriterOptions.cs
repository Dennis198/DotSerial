namespace DotSerial.Json.Writer
{
    /// <summary>
    /// Additional options for node visitors.
    /// </summary>
    internal struct JsonWriterOptions(int level, bool addKey = true)
    {
        /// <summary>
        /// Indentation level
        /// </summary>
        internal int Level { get; private set; } = level;

        /// <summary>
        /// Add Key to object
        /// </summary>
        internal bool AddKey { get; private set; } = addKey;
    }
}
