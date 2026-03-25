using System.Diagnostics.CodeAnalysis;

namespace DotSerial.Utilities
{
    /// <summary>
    /// Helper class for throwing exceptions.
    /// </summary>
    internal static class ThrowHelper
    {
        [DoesNotReturn]
        internal static void ThrowDifferentStrategyException()
        {
            throw new DotSerialException($"Different strategy exception.");
        }

        [DoesNotReturn]
        internal static void ThrowDuplicateNodeKeyTypeException(string key)
        {
            throw new ArgumentException($"Duplicate node key: {key}.");
        }

        [DoesNotReturn]
        internal static void ThrowGenericParserException(string message)
        {
            throw new DotSerialException(message);
        }

        internal static void ThrowIfNullException(object? obj)
        {
            if (obj is null)
            {
                throw new DotSerialException($"Object {nameof(obj)} cannot be null.");
            }
        }

        [DoesNotReturn]
        internal static void ThrowIndexOutOfRangeException()
        {
            throw new IndexOutOfRangeException();
        }

        [DoesNotReturn]
        internal static void ThrowInnerNodeValueException()
        {
            throw new DotSerialException($"Inner node cannot have a value.");
        }

        [DoesNotReturn]
        internal static void ThrowKeyNodeNullException()
        {
            throw new DotSerialException($"Key cannot be null.");
        }

        [DoesNotReturn]
        internal static void ThrowMissingClosingCharException(int position, char c)
        {
            throw new DotSerialException($"Missing closing char '{c}' at position {position}.");
        }

        [DoesNotReturn]
        internal static void ThrowNoValueFoundForKeyException(int position, string key)
        {
            throw new DotSerialException($"No value found for key '{key}' at position {position}.");
        }

        [DoesNotReturn]
        internal static void ThrowStrategyNotSupportedException()
        {
            throw new DotSerialException($"Strategy not supported.");
        }

        [DoesNotReturn]
        internal static void ThrowTypeIsNotSupportedException(Type type)
        {
            throw new DotSerialException($"Type {type} is not supported.");
        }

        [DoesNotReturn]
        internal static void ThrowUnexpectedNonWhiteSpaceCharException(int position, char c)
        {
            throw new DotSerialException($"Unexpected non-whitespace character '{c}' at position {position}.");
        }

        [DoesNotReturn]
        internal static void ThrowUnknownNodeTypeException()
        {
            throw new DotSerialException($"Unknown node type.");
        }

        [DoesNotReturn]
        internal static void ThrowUnterminatedCharException(int position)
        {
            throw new DotSerialException($"Unterminated char at position {position}.");
        }

        [DoesNotReturn]
        internal static void ThrowUnterminatedStringException(int position)
        {
            throw new DotSerialException($"Unterminated string at position {position}.");
        }

        [DoesNotReturn]
        internal static void ThrowWrongNodeTypeException()
        {
            throw new DotSerialException($"Wrong node type.");
        }

        [DoesNotReturn]
        internal static void ThrowWrongTypeException(Type type)
        {
            throw new DotSerialException($"Type {type} is wrong.");
        }
    }
}
