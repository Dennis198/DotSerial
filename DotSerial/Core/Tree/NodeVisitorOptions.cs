namespace DotSerial.Core.Tree
{
    /// <summary>
    /// Options for node visitors.
    /// </summary>
    public struct NodeVisitorOptions
    {
        internal int Level { get; private set; }
        internal bool AddKey { get; private set; }
        internal string? Prefix { get; private set; }

        public NodeVisitorOptions(int level, bool addKey = true, string? prefix = null)
        {
            Level = level;
            AddKey = addKey;
            Prefix = prefix;
        }
    }
}