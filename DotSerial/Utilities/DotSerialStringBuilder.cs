using System.Buffers;
using System.Runtime.InteropServices.Marshalling;
using System.Security;
using System.Threading.Channels;
using System.Xml.Schema;

namespace DotSerial.Utilities
{
    public ref struct DotSerialStringBuilder
    {
        private char[] _buffer;
        private int _position;

        public DotSerialStringBuilder(int initCapacity = 1024)
        {
            _buffer = ArrayPool<char>.Shared.Rent(initCapacity);
            _position = 0;
        }

        public void Append(ReadOnlySpan<char> value)
        {
            EnsureCapacity(_position + value.Length);
            value.CopyTo(_buffer.AsSpan(_position));
            _position += value.Length;
        }

        public void AppendFormat<T>(T value, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
            where T : ISpanFormattable
        {
            const int maxFormatLength = 128;
            EnsureCapacity(_position + maxFormatLength);

            if (value.TryFormat(_buffer.AsSpan(_position), out int charsWritten, format, provider))
            {
                _position += charsWritten;
            }
            else
            {
                EnsureCapacity(_position + maxFormatLength * 2);
                if (value.TryFormat(_buffer.AsSpan(_position), out charsWritten, format, provider))
                {
                    _position += charsWritten;
                }
            }
        }

        public void AppendLine() => AppendLine(ReadOnlySpan<char>.Empty);

        public void AppendLine(ReadOnlySpan<char> value)
        {
            int totalLength = value.Length + Environment.NewLine.Length;
            EnsureCapacity(_position + totalLength);

            value.CopyTo(_buffer.AsSpan(_position));
            _position += value.Length;

            Environment.NewLine.AsSpan().CopyTo(_buffer.AsSpan(_position));
            _position += Environment.NewLine.Length;
        }

        public ReadOnlySpan<char> AsSpan()
        {
            return _buffer.AsSpan(0, _position);
        }

        public void Clear()
        {
            _position = 0;
        }

        /// <summary>
        /// Dispose methode.
        /// </summary>
        public void Dispose()
        {
            if (_buffer != null)
            {
                ArrayPool<char>.Shared.Return(_buffer);
                _buffer = null;
            }
        }

        public void Insert(int index, ReadOnlySpan<char> value)
        {
            if ((uint)index > (uint)_position)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (value.IsEmpty)
                return;

            int requiredCapacity = _position + value.Length;
            EnsureCapacity(requiredCapacity);

            ReadOnlySpan<char> remaining = _buffer.AsSpan(index, _position - index);
            remaining.CopyTo(_buffer.AsSpan(index + value.Length));

            value.CopyTo(_buffer.AsSpan(index));

            _position += value.Length;
        }

        public override string ToString()
        {
            return new string(_buffer, 0, _position);
        }

        public bool TryWriteTo(Span<char> destination, out int charsWritten)
        {
            if (_buffer.AsSpan(0, _position).TryCopyTo(destination))
            {
                charsWritten = _position;
                return true;
            }

            charsWritten = 0;
            return false;
        }

        private void EnsureCapacity(int requiredCapacity)
        {
            if (requiredCapacity > _buffer.Length)
            {
                int newSize = Math.Max(_buffer.Length * 2, requiredCapacity);
                char[] newBuffer = ArrayPool<char>.Shared.Rent(newSize);

                _buffer.AsSpan(0, _position).CopyTo(newBuffer);
                ArrayPool<char>.Shared.Return(_buffer);

                _buffer = newBuffer;
            }
        }
    }
}
