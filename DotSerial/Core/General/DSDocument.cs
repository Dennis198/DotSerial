#region License
//Copyright (c) 2025 Dennis Sölch

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
#endregion

namespace DotSerial.Core.General
{
    /// <summary>
    /// Class which represents an Document
    /// </summary>
    internal abstract class DSDocument
    {
        /// <summary>
        /// Tree which holds the object representation
        /// </summary>
        internal DSNode? Tree;

        /// <summary>
        /// Save file to disk
        /// </summary>
        /// <param name="fileName">Path</param>
        public abstract void Save(string fileName);

        /// <summary>
        /// Load file from disk
        /// </summary>
        /// <param name="fileName">Path</param>
        public abstract void Load(string fileName);

        /// <summary>
        /// Loads the file content into a string.
        /// </summary>
        /// <param name="fileName">Path</param>
        /// <param name="content">File content</param>
        /// <returns>True, if success.</returns>
        internal static bool LoadFileContent(string fileName, out string content)
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

        /// <summary>
        /// Saves a string into a file.
        /// </summary>
        /// <param name="fileName">Path</param>
        /// <param name="content">File content</param>
        /// <returns>True, if success.</returns>
        internal static bool SaveContentToFile(string fileName, string content)
        {
            File.WriteAllText(fileName, content);
            return true;
        }
    }
}
