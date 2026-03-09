using DotSerial.Common;

namespace DotSerial.Json
{
    /// <summary>
    /// Class which represents an Json document
    /// </summary>
    internal class JsonDocument : DSDocument
    {
        /// <summary>
        /// Root node
        /// </summary>
        internal DSJsonNode? RootNode;

        /// <inheritdoc/>
        internal override void Load(string fileName)
        {
            try
            {
                if (false == LoadFileContent(fileName, out string content))
                {
                    throw new NotSupportedException();
                }

                var tmp = DSJsonNode.FromString(content);
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
