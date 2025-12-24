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

using System.Text;
using DotSerial.Core.Misc;

namespace DotSerial.Core.JSON.Writer
{
    /// <summary>
    /// Helper class with methode for writting json.
    /// </summary>
    public static class JsonWriterHelper
    {
        /// <summary>
        /// Helper methode to add object start symbol and to json
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="key">Key of object</param>
        /// <param name="level">Indentation level</param>
        internal static void AddObjectStart(StringBuilder sb, string key, int level)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(key);

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));
            }

            sb.AppendLine();
            WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);
            sb.AppendFormat("\"{0}\": {{", key);
        }

        /// <summary>
        /// Helper methode to add object end symbol and to json
        /// </summary>
        /// <param name="sb">Strinbuilder</param>
        /// <param name="level">Indentation level</param>
        /// <param name="isLastObject">True, if object is last object</param>
        internal static void AddObjectEnd(StringBuilder sb, int level, bool isLastObject = false)
        {
            ArgumentNullException.ThrowIfNull(sb);

            sb.AppendLine();
            WriteMethods.AddIndentation(sb, level, JsonConstants.IndentationSize);

            if (isLastObject)
            {
                sb.Append(JsonConstants.ObjectEnd);
            }
            else
            {
                sb.Append(JsonConstants.ObjectEnd + ",");
            }
        }
    }
}