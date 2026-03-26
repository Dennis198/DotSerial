namespace DotSerial.Common.Writer
{
    public interface IWriteStrategy
    {
        /// <summary>
        /// Writes the node to a string.
        /// </summary>
        /// <param name="node">Node to write</param>
        /// <returns>String representation of the node</returns>
        public ReadOnlySpan<char> Write(DSNode node);
    }
}
