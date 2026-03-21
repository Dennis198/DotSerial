namespace DotSerial.Common
{
    /// <summary>
    /// Class which represents an Document
    /// </summary>
    internal static class FileMethods
    {
        /// <summary>
        /// Loads the file content into a string.
        /// </summary>
        /// <param name="fileName">Path</param>
        /// <param name="content">File content</param>
        /// <returns>True, if success.</returns>
        internal static bool LoadFileContent(string fileName, out string content)
        {
            try
            {
                content = string.Empty;

                if (false == File.Exists(fileName))
                {
                    return false;
                }

                content = File.ReadAllText(fileName);

                if (string.IsNullOrWhiteSpace(content))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Saves a string into a file.
        /// </summary>
        /// <param name="fileName">Path</param>
        /// <param name="content">File content</param>
        /// <returns>True, if success.</returns>
        internal static bool SaveContentToFile(string fileName, string content)
        {
            try
            {
                File.WriteAllText(fileName, content);
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}
