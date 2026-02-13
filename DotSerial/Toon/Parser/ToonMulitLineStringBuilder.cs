using System.Net.Security;
using System.Text;
using DotSerial.Utilities;

namespace DotSerial.Toon.Parser
{
    internal class ToonMulitLineStringBuilder
    {
        internal string? KeyLine = null;

        private readonly MultiLineStringBuilder? _lines = null;
        internal int Count => null != _lines ? _lines.Count : throw new NotImplementedException();
        // Special case. (Currenly only needed in yaml)
        internal bool IsOneLineObject = false; // TODO brauch ich das?
                
        internal ToonMulitLineStringBuilder(StringBuilder sb, string? keyLine = null)
        {
            KeyLine =  keyLine;
            _lines = new MultiLineStringBuilder(sb);
        }

        internal ToonMulitLineStringBuilder(MultiLineStringBuilder lines)
        {
            _lines = lines;
        }

        internal ToonMulitLineStringBuilder Slice(int start, int end)
        {
            if (_lines == null)
            {
                throw new NotImplementedException();
            }

            var lines = _lines.Slice(start, end);
            return new ToonMulitLineStringBuilder(lines);
        }

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