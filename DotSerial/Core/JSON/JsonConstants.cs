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

namespace DotSerial.Core.JSON
{
    internal static class JsonConstants
    {
        /// <summary>
        /// XML Object display
        /// </summary>
        public const string DotSerial = "DotSerial";
        /// <summary>
        /// MainObject ID
        /// </summary>
        public const string MainObjectKey = "0";
        /// <summary>
        /// Version
        /// </summary>
        public const string Version = "version";

        /// <summary>
        /// Indentation Size
        /// </summary>
        internal const int IndentationSize = 2;
        /// <summary>
        /// Json key for prop type
        /// </summary>
        internal const string PropertyTypeKey = "type";
        /// <summary>
        /// Whitespace
        /// </summary>
        internal const char WhiteSpace = ' ';
        /// <summary>
        /// Quote
        /// </summary>
        internal const char Quote = '"';
        /// <summary>
        /// Json object start char
        /// </summary>
        internal const char ObjectStart = '{';
        /// <summary>
        /// Jsoobject end char
        /// </summary>
        internal const char ObjectEnd = '}';
        /// <summary>
        /// Json list start char
        /// </summary>
        internal const char ListStart = '[';
        /// <summary>
        /// Json list end char
        /// </summary>
        internal const char ListEnd = ']';
        /// <summary>
        /// Json null string
        /// </summary>
        internal const string Null = "null";
        /// <summary>
        /// Json null string with Quotes
        /// </summary>
        internal const string NullWithQuotes = "\"null\"";
        /// <summary>
        /// Coma char
        /// </summary>
        internal const char Comma = ',';
    }
}
