namespace DotSerial.Core.JSON.Writer
{
    /// <summary>
    /// Options for node visitors.
    /// </summary>
    public struct JsonNodeVisitorOptions
    {
        internal int Level { get; private set; }
        internal bool AddKey { get; private set; }
        internal string? Prefix { get; private set; }

        public JsonNodeVisitorOptions(int level, bool addKey = true, string? prefix = null)
        {
            Level = level;
            AddKey = addKey;
            Prefix = prefix;
        }
    }
}