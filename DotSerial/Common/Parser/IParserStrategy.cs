namespace DotSerial.Common.Parser
{
    public interface IParserStrategy
    {
        /// <summary>
        /// Parses the string to a node.
        /// </summary>
        /// <param name="str">String to parse</param>
        /// <returns>Parsed node</returns>
        public DSNode Parse(ReadOnlySpan<char> str);
    }
}
