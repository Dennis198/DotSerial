using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace DotSerial.Utilities
{
    /// <summary>
    /// Custom StringBuilder
    /// </summary>
    public ref struct DotSerialStringBuilder
    {
        /// <summary>
        /// Interner buffer
        /// </summary>
        private char[]? _buffer;

        /// <summary>
        /// Current position in the buffer
        /// </summary>
        private int _position;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="initCapacity">Initial capacity of builder</param>
        public DotSerialStringBuilder(int initCapacity = 8192)
        {
            _buffer = ArrayPool<char>.Shared.Rent(initCapacity);
            _position = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">String</param>
        public DotSerialStringBuilder(string? value)
        {
            if (value == null)
            {
                _buffer = ArrayPool<char>.Shared.Rent(1024);
                _position = 0;
                return;
            }

            _buffer = ArrayPool<char>.Shared.Rent(value.Length);

            value.AsSpan().CopyTo(_buffer);

            _position = value.Length;
        }

        /// <summary>
        /// Konstruktor, der mit einem ReadOnlySpan initialisiert wird
        /// </summary>
        /// <param name="value">Der initiale Inhalt</param>
        public DotSerialStringBuilder(ReadOnlySpan<char> value)
        {
            // Passenden Buffer aus dem Pool mieten
            _buffer = ArrayPool<char>.Shared.Rent(value.Length);

            // Daten in den Buffer kopieren
            value.CopyTo(_buffer);

            // Aktuelle Position setzen
            _position = value.Length;
        }

        /// <summary>
        /// Length of the builder
        /// </summary>
        public readonly int Length => _position;

        /// <summary>
        /// Get range
        /// </summary>
        public readonly ReadOnlySpan<char> this[Range range]
        {
            get
            {
                var (offset, length) = range.GetOffsetAndLength(_position);

                return _buffer.AsSpan(offset, length);
            }
        }

        /// <summary>
        /// Get/Set value at position index
        /// </summary>
        public readonly char this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)index >= (uint)_position)
                {
                    ThrowIndexOutOfRangeException();
                }

                return _buffer.AsSpan()[index];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if ((uint)index >= (uint)_position)
                {
                    ThrowIndexOutOfRangeException();
                }

                _buffer.AsSpan()[index] = value;
            }
        }

        /// <summary>
        /// Convert to span.
        /// </summary>
        /// <returns>ReadOnlySpan<char></returns>
        public readonly ReadOnlySpan<char> AsSpan()
        {
            return _buffer.AsSpan(0, _position);
        }

        /// <summary>
        /// Check if a given value exists inside the buffer
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public readonly bool Contains(ReadOnlySpan<char> value, StringComparison comparison = StringComparison.Ordinal)
        {
            return IndexOf(value, comparison) >= 0;
        }

        /// <summary>
        /// Search for a specific value and give the index of its first occurence
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Position in buffer</returns>
        public readonly int IndexOf(char value)
        {
            return _buffer.AsSpan()[.._position].IndexOf(value);
        }

        /// <summary>
        /// Search for a specific value and give the index of its first occurence
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="comparison">Comparison</param>
        /// <returns>Position in buffer</returns>
        public readonly int IndexOf(ReadOnlySpan<char> value, StringComparison comparison = StringComparison.Ordinal)
        {
            return MemoryExtensions.IndexOf(_buffer.AsSpan()[.._position], value, comparison);
        }

        /// <summary>
        /// Prüft, ob der Builder leer ist oder nur aus Whitespace-Zeichen besteht.
        /// </summary>
        public readonly bool IsNullOrWhiteSpace()
        {
            if (_position == 0)
                return true;

            ReadOnlySpan<char> content = _buffer.AsSpan(0, _position);

            for (int i = 0; i < content.Length; i++)
            {
                if (!char.IsWhiteSpace(content[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Search for a specific value and give the index of its last occurence
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Position in buffer</returns>
        public readonly int LastIndexOf(char value)
        {
            return _buffer.AsSpan()[.._position].LastIndexOf(value);
        }

        /// <summary>
        /// Converts the builder to an char array
        /// </summary>
        /// <returns></returns>
        public readonly char[] ToArray()
        {
            if (_position == 0)
            {
                return [];
            }

            char[] result = new char[_position];
            _buffer.AsSpan()[.._position].CopyTo(result);
            return result;
        }

        /// <summary>
        /// Converts the builder to a string.
        /// </summary>
        /// <returns>String</returns>
        public override readonly string ToString()
        {
            if (null == _buffer)
            {
                throw new NullReferenceException($"{_buffer} is null");
            }
            return new string(_buffer, 0, _position);
        }

        /// <summary>
        /// Converts the builder to a string.
        /// </summary>
        /// <param name="startIndex">Start index</param>
        /// <param name="length">length</param>
        /// <returns>String</returns>
        public readonly string ToString(int startIndex, int length)
        {
            if (null == _buffer)
            {
                throw new NullReferenceException($"{_buffer} is null");
            }

            if ((uint)startIndex + (uint)length > (uint)_position)
            {
                throw new IndexOutOfRangeException();
            }

            return new string(_buffer.AsSpan().Slice(startIndex, length));
        }

        /// <summary>
        /// Write content to another span.
        /// </summary>
        /// <param name="destination">Desitination span</param>
        /// <param name="charsWritten">Chars written</param>
        /// <returns>True, if successfull</returns>
        public readonly bool TryWriteTo(Span<char> destination, out int charsWritten)
        {
            if (_buffer.AsSpan(0, _position).TryCopyTo(destination))
            {
                charsWritten = _position;
                return true;
            }

            charsWritten = 0;
            return false;
        }

        /// <summary>
        /// Append text
        /// </summary>
        /// <param name="value">Text to append</param>
        public void Append(ReadOnlySpan<char> value)
        {
            EnsureCapacity(_position + value.Length);
            value.CopyTo(_buffer.AsSpan(_position));
            _position += value.Length;
        }

        /// <summary>
        /// Append char
        /// </summary>
        /// <param name="value">Char to append</param>
        public void Append(char value)
        {
            EnsureCapacity(_position + 1);
            _position++;
            this[^1] = value;
        }

        /// <summary>
        /// Appends a specified number of copies of the string representation of a Unicode character to this instance.
        /// </summary>
        /// <param name="value">Test to append</param>
        /// <param name="repeatCount">Specific count</param>
        public void Append(ReadOnlySpan<char> value, int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                Append(value);
            }
        }

        /// <summary>
        /// Appends a specified number of copies of the char.
        /// </summary>
        /// <param name="value">Test to append</param>
        /// <param name="repeatCount">Specific count</param>
        public void Append(char value, int repeatCount)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                Append(value);
            }
        }

        /// <summary>
        /// Append format text
        /// </summary>
        /// <param name="value">Text to append</param>
        /// <param name="format">Format</param>
        /// <param name="provider">IFormatProvider</param>
        /// <typeparam name="T"></typeparam>
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

        /// <summary>
        /// Appends a new line
        /// </summary>
        public void AppendLine() => AppendLine([]);

        /// <summary>
        /// Appends a new line and adds the value afterwards.
        /// </summary>
        /// <param name="value">Test to append</param>
        public void AppendLine(ReadOnlySpan<char> value)
        {
            int totalLength = value.Length + Environment.NewLine.Length;
            EnsureCapacity(_position + totalLength);

            value.CopyTo(_buffer.AsSpan(_position));
            _position += value.Length;

            Environment.NewLine.AsSpan().CopyTo(_buffer.AsSpan(_position));
            _position += Environment.NewLine.Length;
        }

        /// <summary>
        /// Clears the internal buffer
        /// </summary>
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

        /// <summary>
        /// Insert text at position
        /// </summary>
        /// <param name="index">Position</param>
        /// <param name="value">Text to insert</param>
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

        /// <summary>
        /// Removes at startIndex till length from the builder
        /// </summary>
        /// <param name="startIndex">Start index</param>
        /// <param name="length">Length to remove</param>
        public void Remove(int startIndex, int length)
        {
            if (startIndex < 0 || length < 0 || (uint)startIndex + (uint)length > _position)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (length == 0)
                return;

            int remainingSourceIndex = startIndex + length;
            int remainingCount = _position + remainingSourceIndex;

            if (remainingCount > 0)
            {
                ReadOnlySpan<char> tail = _buffer.AsSpan(remainingSourceIndex, remainingCount);
                tail.CopyTo(_buffer.AsSpan(startIndex));
            }

            _position -= length;
        }

        /// <summary>
        /// Truncates the builder to the given length
        /// </summary>
        /// <param name="newLength">New length</param>
        public void Truncate(int newLength)
        {
            if ((uint)newLength < (uint)_position)
            {
                _position = newLength;
            }
        }

        [DoesNotReturn]
        private static void ThrowIndexOutOfRangeException()
        {
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Escures that the capacity is enough
        /// </summary>
        /// <param name="requiredCapacity">Required capacity</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCapacity(int requiredCapacity)
        {
            if ((uint)requiredCapacity > (uint)(_buffer?.Length ?? 0))
            {
                Grow(requiredCapacity);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Grow(int minCapacity)
        {
            int currentCapacity = _buffer?.Length ?? 0;

            int newCapacity = Math.Max(currentCapacity * 2, minCapacity);

            if (newCapacity < 16)
                newCapacity = 16;

            char[] newBuffer = ArrayPool<char>.Shared.Rent(newCapacity);

            if (_position > 0 && _buffer != null)
            {
                _buffer.AsSpan(0, _position).CopyTo(newBuffer);
                ArrayPool<char>.Shared.Return(_buffer);
            }

            _buffer = newBuffer;
        }
    }
}
