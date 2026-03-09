using DotSerial.Common;

namespace DotSerial.Yaml
{
    /// <summary>
    /// Class which represents a Yaml document
    /// </summary>
    internal class YamlDocument : DSDocument
    {
        /// <summary>
        /// Root node
        /// </summary>
        internal DSYamlNode? RootNode;

        /// <inheritdoc/>
        internal override void Load(string fileName)
        {
            try
            {
                if (false == LoadFileContent(fileName, out string content))
                {
                    throw new NotSupportedException();
                }

                var tmp = DSYamlNode.FromString(content);
                RootNode = tmp;
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        internal override void Save(string fileName)
        {
            try
            {
                if (null == RootNode)
                {
                    throw new NullReferenceException(nameof(RootNode));
                }

                var content = RootNode.Stringify();

                if (false == SaveContentToFile(fileName, content))
                {
                    throw new NotSupportedException();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
