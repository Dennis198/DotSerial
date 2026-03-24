using System.Diagnostics.CodeAnalysis;

namespace DotSerial.Utilities
{
    internal static class ThrowHelper
    {
        #region  Common Exceptions

        [DoesNotReturn]
        internal static void ThrowKeyNodeNullException()
        {
            throw new DotSerialException($"Key cannot be null.");
        }

        [DoesNotReturn]
        internal static void ThrowInnerNodeValueException()
        {
            throw new DotSerialException($"Inner node cannot have a value.");
        }

        [DoesNotReturn]
        internal static void ThrowUnknownNodeTypeException()
        {
            throw new DotSerialException($"Unknown node type.");
        }

        # endregion

        #region  Parse Exceptions

        [DoesNotReturn]
        internal static void ThrowGenericParserException(string message)
        {
            throw new DotSerialException(message);
        }

        [DoesNotReturn]
        internal static void ThrowUnexpectedNonWhiteSpaceCharException(int position, char c)
        {
            throw new DotSerialException($"Unexpected non-whitespace character '{c}' at position {position}.");
        }

        [DoesNotReturn]
        internal static void ThrowNoValueFoundForKeyException(int position, string key)
        {
            throw new DotSerialException($"No value found for key '{key}' at position {position}.");
        }

        [DoesNotReturn]
        internal static void ThrowUnterminatedStringException(int position)
        {
            throw new DotSerialException($"Unterminated string at position {position}.");
        }

        [DoesNotReturn]
        internal static void ThrowUnterminatedCharException(int position)
        {
            throw new DotSerialException($"Unterminated char at position {position}.");
        }

        [DoesNotReturn]
        internal static void ThrowMissingClosingCharException(int position, char c)
        {
            throw new DotSerialException($"Missing closing char '{c}' at position {position}.");
        }

        #endregion
    }
}
