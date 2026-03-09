using DotSerial.Common;

namespace DotSerial.Xml
{
    /// <summary>
    /// Class which represents an Xml document
    /// </summary>
    internal class XmlDocument : DSDocument
    {
        /// <summary>
        /// Root node
        /// </summary>
        internal DSXmlNode? RootNode;

        /// <inheritdoc/>
        internal override void Load(string fileName)
        {
            try
            {
                if (false == LoadFileContent(fileName, out string content))
                {
                    throw new NotSupportedException();
                }

                var tmp = DSXmlNode.FromString(content);
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