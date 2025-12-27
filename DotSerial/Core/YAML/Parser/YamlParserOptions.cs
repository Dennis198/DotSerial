namespace DotSerial.Core.YAML.Parser
{
    public class YamlParserOptions
    {
        internal string Key;
        internal int Level;
        internal int StartLineIndex;
        internal int EndLineIndex;
        internal bool IsList;
        internal bool IsYamlListObject;

        internal YamlParserOptions(string key, int level, int startLineIndex, int endLineIndex, bool isList = false, bool isYamlObject = false)
        {
            Key = key;
            Level = level;
            this.StartLineIndex = startLineIndex;
            this.EndLineIndex = endLineIndex;
            IsList = isList;
            IsYamlListObject = isYamlObject;
        }

        /// <summary>
        /// Check if object is a yaml object or just a key value pair
        /// </summary>
        /// <returns>True, if object</returns>
        internal bool IsYamlObject()
        {
            if (IsYamlListObject)
            {
                return true;
            }

            return StartLineIndex != EndLineIndex;
        }
    }
}