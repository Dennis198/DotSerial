namespace DotSerial.Core.YAML.Parser
{
    public class YamlParserOptions
    {
        internal string Key;
        internal int Level;
        internal int StartLineIndex;
        internal int EndLineIndex;

        internal YamlParserOptions(string key, int level, int startLineIndex, int endLineIndex)
        {
            Key = key;
            Level = level;
            this.StartLineIndex = startLineIndex;
            this.EndLineIndex = endLineIndex;
        }

        /// <summary>
        /// Check if object is a yaml object or just a key value pair
        /// </summary>
        /// <returns>True, if object</returns>
        internal bool IsYamlObject()
        {
            return StartLineIndex != EndLineIndex;
        }
    }
}