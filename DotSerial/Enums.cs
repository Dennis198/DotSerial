namespace DotSerial
{
    /// <summary>
    /// Node Type
    /// </summary>
    public enum NodeType
    {
        Object = 0,
        Value = 1,
        Array = 2,
    }

    /// <summary>
    /// /// Serialize Strategy
    /// </summary>
    public enum SerializeStrategy
    {
        Json = 0,
        Toon = 1,
        Xml = 2,
        Yaml = 3,
    }
}
