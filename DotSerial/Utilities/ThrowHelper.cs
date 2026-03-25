using System.Diagnostics.CodeAnalysis;

namespace DotSerial.Utilities
{
    /// <summary>
    /// Helper class for throwing exceptions.
    /// </summary>
    internal static class ThrowHelper
    {
        /// <summary>
        /// Throws an exception indicating a different strategy was used.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowDifferentStrategyException()
        {
            throw new DotSerialException($"Different strategy exception.");
        }

        /// <summary>
        /// Throws an exception for a duplicate node key type.
        /// </summary>
        /// <param name="key">The duplicate key.</param>
        [DoesNotReturn]
        internal static void ThrowDuplicateNodeKeyTypeException(string key)
        {
            throw new ArgumentException($"Duplicate node key: {key}.");
        }

        /// <summary>
        /// Throws a generic parser exception with a custom message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        [DoesNotReturn]
        internal static void ThrowGenericParserException(string message)
        {
            throw new DotSerialException(message);
        }

        /// <summary>
        /// Throws an exception if the provided object is null.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        internal static void ThrowIfNullException(object? obj)
        {
            if (obj is null)
            {
                throw new DotSerialException($"Object {nameof(obj)} cannot be null.");
            }
        }

        /// <summary>
        /// Throws an IndexOutOfRangeException.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowIndexOutOfRangeException()
        {
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Throws an exception indicating an inner node cannot have a value.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowInnerNodeValueException()
        {
            throw new DotSerialException($"Inner node cannot have a value.");
        }

        /// <summary>
        /// Throws an exception indicating a key node is null.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowKeyNodeNullException()
        {
            throw new DotSerialException($"Key cannot be null.");
        }

        /// <summary>
        /// Throws an exception for a missing closing character at a specific position.
        /// </summary>
        /// <param name="position">The position of the missing character.</param>
        /// <param name="c">The missing character.</param>
        [DoesNotReturn]
        internal static void ThrowMissingClosingCharException(int position, char c)
        {
            throw new DotSerialException($"Missing closing char '{c}' at position {position}.");
        }

        /// <summary>
        /// Throws an exception when no value is found for a specific key.
        /// </summary>
        /// <param name="position">The position of the key.</param>
        /// <param name="key">The key with no value.</param>
        [DoesNotReturn]
        internal static void ThrowNoValueFoundForKeyException(int position, string key)
        {
            throw new DotSerialException($"No value found for key '{key}' at position {position}.");
        }

        /// <summary>
        /// Throws an exception indicating the strategy is not supported.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowStrategyNotSupportedException()
        {
            throw new DotSerialException($"Strategy not supported.");
        }

        /// <summary>
        /// Throws an exception indicating the type is not supported.
        /// </summary>
        /// <param name="type">The unsupported type.</param>
        [DoesNotReturn]
        internal static void ThrowTypeIsNotSupportedException(Type type)
        {
            throw new DotSerialException($"Type {type} is not supported.");
        }

        /// <summary>
        /// Throws an exception for an unexpected non-whitespace character at a specific position.
        /// </summary>
        /// <param name="position">The position of the character.</param>
        /// <param name="c">The unexpected character.</param>
        [DoesNotReturn]
        internal static void ThrowUnexpectedNonWhiteSpaceCharException(int position, char c)
        {
            throw new DotSerialException($"Unexpected non-whitespace character '{c}' at position {position}.");
        }

        /// <summary>
        /// Throws an exception for an unknown node type.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowUnknownNodeTypeException()
        {
            throw new DotSerialException($"Unknown node type.");
        }

        /// <summary>
        /// Throws an exception for an unterminated character at a specific position.
        /// </summary>
        /// <param name="position">The position of the unterminated character.</param>
        [DoesNotReturn]
        internal static void ThrowUnterminatedCharException(int position)
        {
            throw new DotSerialException($"Unterminated char at position {position}.");
        }

        /// <summary>
        /// Throws an exception for an unterminated string at a specific position.
        /// </summary>
        /// <param name="position">The position of the unterminated string.</param>
        [DoesNotReturn]
        internal static void ThrowUnterminatedStringException(int position)
        {
            throw new DotSerialException($"Unterminated string at position {position}.");
        }

        /// <summary>
        /// Throws an exception for a wrong node type.
        /// </summary>
        [DoesNotReturn]
        internal static void ThrowWrongNodeTypeException()
        {
            throw new DotSerialException($"Wrong node type.");
        }

        /// <summary>
        /// Throws an exception for a wrong type.
        /// </summary>
        /// <param name="type">The wrong type.</param>
        [DoesNotReturn]
        internal static void ThrowWrongTypeException(Type type)
        {
            throw new DotSerialException($"Type {type} is wrong.");
        }
    }
}
