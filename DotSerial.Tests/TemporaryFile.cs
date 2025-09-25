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

namespace DotSerial.Tests
{
    /// <summary>
    /// Only used for testing
    /// Source: https://www.geekality.net/blog/c-temporary-file-handler-for-unit-tests-and-such
    /// </summary>
    public sealed class TemporaryFile(FileInfo temporaryFile) : IDisposable
    {
        private readonly FileInfo file = temporaryFile;
        public FileInfo FileInfo { get { return file; } }

        public TemporaryFile() : this(Path.GetTempFileName()) { }
        public TemporaryFile(string fileName) : this(new FileInfo(fileName)) { }

        public TemporaryFile(Stream initialFileContents) : this()
        {
            using var file = new FileStream(this, FileMode.Open);
            initialFileContents.CopyTo(file);
        }

        public static implicit operator FileInfo(TemporaryFile temporaryFile)
        {
            return temporaryFile.file;
        }
        public static implicit operator string(TemporaryFile temporaryFile)
        {
            return temporaryFile.file.FullName;
        }
        public static explicit operator TemporaryFile(FileInfo temporaryFile)
        {
            return new TemporaryFile(temporaryFile);
        }

        private volatile bool disposed;
        public void Dispose()
        {
            try
            {
                file.Delete();
                disposed = true;
            }
            catch (Exception) { } // Ignore
        }
        ~TemporaryFile()
        {
            if (!disposed) Dispose();
        }
    }
}
