using System.Text;
using DotSerial.Common;
using DotSerial.Utilities;

namespace DotSerial.YAML.Parser
{
    public class MultiLineStringBuilder
    {
        private List<StringBuilder>? _lines = null;
        public bool IsOneLineObject = false;

        public int Count => null != _lines ? _lines.Count : throw new NotImplementedException();

        /// <summary>
        /// Copnstructor
        /// </summary>
        /// <param name="sb"></param>
        internal MultiLineStringBuilder(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            _lines = CreateLines(sb);
            TrimLines(_lines);
        }

        private MultiLineStringBuilder(List<StringBuilder> lines)
        {
            _lines = lines;
            TrimLines(_lines);
        }

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

            int length  = end - start + 1;
            var subLines = _lines.Slice(start, length);
            return new MultiLineStringBuilder(subLines);
        }

        internal void Clear()
        {
            if (null == _lines)
            {
                throw new NotImplementedException();
            }
            
            _lines.Clear();
        }

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
                    i = ParseMethods.AppendStringValue(currentLine, i, sb.ToString());
                }
                // Both is needed for crossplatform
                else if (c == '\n' || c == '\r')
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