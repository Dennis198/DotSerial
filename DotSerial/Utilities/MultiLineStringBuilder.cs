using System.Text;
using DotSerial.Common;

namespace DotSerial.Utilities
{
    /// <summary>
    /// MulitLineStringBuilder contains multiple StringBuilder
    /// where each line is a StringBuilder.
    /// </summary>
    internal class MultiLineStringBuilder
    {
        /// <summary> Internal data </summary>
        private readonly List<StringBuilder>? _lines = null;

        /// <summary>
        /// Number of lines
        /// </summary>
        internal int Count => null != _lines ? _lines.Count : throw new NotImplementedException();

        // Special case. (Currenly only needed in yaml/toon)
        internal bool IsOneLineObject = false;

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="lines"Lists of StringBuilder></param>
        private MultiLineStringBuilder(List<StringBuilder> lines)
        {
            _lines = lines;
            TrimLines(_lines);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        internal MultiLineStringBuilder(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            _lines = CreateLines(sb);
            TrimLines(_lines);
        }

        /// <summary>
        /// Creates a shallow copy of a range of lines.
        /// </summary>
        /// <param name="start">Start index</param>
        /// <param name="end">End index</param>
        /// <returns>A shallow copy of a range of elements in the source.</returns>
        internal MultiLineStringBuilder Slice(int start, int end)
        {
            if (null == _lines)
            {
                throw new NotImplementedException();
            }

            if (end < start)
            {
                throw new NotImplementedException();
            }

            if (start < 0 || end > _lines.Count - 1)
            {
                throw new NotImplementedException();
            }

            int length = end - start + 1;
            var subLines = _lines.Slice(start, length);
            return new MultiLineStringBuilder(subLines);
        }

        /// <summary>
        /// Clears all lines
        /// </summary>
        internal void Clear()
        {
            if (null == _lines)
            {
                throw new NotImplementedException();
            }

            _lines.Clear();
        }

        /// <summary>
        /// Get a specific line
        /// </summary>
        /// <param name="i">Index of line</param>
        /// <returns>Line as a StringBuilder</returns>
        internal StringBuilder GetLine(int i)
        {
            if (null == _lines)
            {
                throw new NotImplementedException();
            }

            if (i < 0 || i > _lines.Count - 1)
            {
                throw new NotImplementedException();
            }

            return _lines[i];
        }

        /// <summary>
        /// Create a list of stringbuilders representing each lines of the
        /// yaml string
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <returns>List<StringBuilder></returns>
        private static List<StringBuilder> CreateLines(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            List<StringBuilder> result = [];
            bool createNewLine = false;
            StringBuilder currentLine = new();

            result.Add(currentLine);

            for (int i = 0; i < sb.Length; i++)
            {
                if (createNewLine)
                {
                    currentLine = new StringBuilder();
                    result.Add(currentLine);
                    createNewLine = false;
                }

                var c = sb[i];
                if (c == CommonConstants.Quote)
                {
                    i = ParseMethods.AppendStringValue(currentLine, i, sb);
                }
                // Both is needed for crossplatform
                else if (c.IsNewLine())
                {
                    createNewLine = true;
                }
                else
                {
                    currentLine.Append(c);
                }
            }

            return result;
        }

        /// <summary>
        /// Trims the lines by removing empty lines and trailing whitespace
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        private static void TrimLines(List<StringBuilder> lines)
        {
            // Remove Empty Lines
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                if (lines[i].IsNullOrWhiteSpace())
                {
                    lines.RemoveAt(i);
                }
            }

            // Remove trailing whitespace
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = lines[i].TrimEnd();
            }
        }
    }
}
