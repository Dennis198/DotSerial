using System.Text;
using DotSerial.Utilities;

namespace DotSerial.Toon.Parser
{
    /// <summary>
    /// Wrapper for MultiLineStringBuilder to store the lines of the parsed object and additional information (e.g. key line).
    /// </summary>
    internal class ToonMulitLineStringBuilder
    {
        /// <summary>
        /// Key line of the parsed object. (Currently only for lists)
        /// </summary>
        internal string? KeyLine = null;

        /// <summary>
        /// Wrapped MultiLineStringBuilder to store the lines of the parsed object.
        /// </summary>
        private readonly MultiLineStringBuilder? _lines = null;

        /// <summary>
        /// Number of lines
        /// </summary>
        internal int Count => null != _lines ? _lines.Count : throw new NotImplementedException();

        // Special case. (Currenly only needed in yaml/toon)
        internal bool IsOneLineObject
        {
            get
            {
                if (_lines == null)
                    throw new NotImplementedException();

                return _lines.IsOneLineObject;
            }
            set
            {
                if (_lines == null)
                    throw new NotImplementedException();

                _lines.IsOneLineObject = value;
            }
        }

        internal ToonMulitLineStringBuilder(StringBuilder sb, string? keyLine = null)
        {
            KeyLine = keyLine;
            _lines = new MultiLineStringBuilder(sb);
        }

        private ToonMulitLineStringBuilder(MultiLineStringBuilder lines)
        {
            _lines = lines;
        }

        /// <summary>
        /// Creates a shallow copy of a range of lines.
        /// </summary>
        /// <param name="start">Start index</param>
        /// <param name="end">End index</param>
        /// <returns>A shallow copy of a range of elements in the source.</returns>
        internal ToonMulitLineStringBuilder Slice(int start, int end)
        {
            if (_lines == null)
            {
                throw new NotImplementedException();
            }

            var lines = _lines.Slice(start, end);
            return new ToonMulitLineStringBuilder(lines);
        }

        /// <summary>
        /// Get a specific line
        /// </summary>
        /// <param name="i">Line index</param>
        /// <returns>Line as a StringBuilder</returns>
        internal StringBuilder GetLine(int i)
        {
            if (_lines == null)
            {
                throw new NotImplementedException();
            }

            return _lines.GetLine(i);
        }
    }
}
