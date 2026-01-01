namespace DotSerial.YAML.Parser
{
    public class YamlParserOptions
    {
        internal string Key;
        internal int Level;
        internal int StartLineIndex;
        internal int EndLineIndex;

        internal bool IsList;
        internal bool IsObject;

        internal bool IsEmptyList;
        internal bool IsEmptyObject;

        internal YamlParserOptions(string key, int level, int startLineIndex, int endLineIndex)
        {
            Key = key;
            Level = level;
            this.StartLineIndex = startLineIndex;
            this.EndLineIndex = endLineIndex;
            // IsList = isList;
            // IsYamlListObject = isYamlObject;
        }

        // TODO SET ISList & IsObject extra

        internal void SetIsList()
        {   
            IsList = true;
        }

        internal void SetIsYamlObject()
        {
            IsObject = true;
        }

        /// <summary>
        /// Check if object is a yaml object or just a key value pair
        /// </summary>
        /// <returns>True, if object</returns>
        internal bool IsYamlObject()
        {
            // if (IsYamlListObject)
            // {
            //     return true;
            // }

            return IsObject;
        }
    }
}