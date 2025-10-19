using DotSerial.Core.General;

namespace DotSerial.Core.YAML
{
    /// <summary>
    /// Class which represents a Yaml document
    /// </summary>
    internal class YAMLDocument : DSDocument
    {
        /// <inheritdoc/>
        public override void Load(string fileName)
        {
            try
            {
                if (false == LoadFileContent(fileName, out string content))
                {
                    throw new NotSupportedException();
                }

                var root = YAMLParser.ToNode(content);

                Tree = root;
            }
            catch
            {
                throw;
            }

        }

        /// <inheritdoc/>
        public override void Save(string fileName)
        {
            try
            {
                if (null == Tree)
                {
                    throw new NullReferenceException(nameof(Tree));
                }

                var content = YAMLWriter.ToYamlString(Tree);

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