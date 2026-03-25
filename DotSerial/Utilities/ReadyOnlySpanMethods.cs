using System.Runtime.CompilerServices;
using DotSerial.Common;

namespace DotSerial.Utilities
{
    /// <summary>
    /// Helper Methods for ReadOnlySpans (char)
    /// </summary>
    internal static class ReadOnlySpanMethods
    {
        /// <summary>
        /// Check if the fist non whitespace char equals c
        /// </summary>
        /// <param name="source">Content</param>
        /// <param name="c">Char to check</param>
        /// <returns>True, if first non whitespace char equals c</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool EqualFirstNoWhiteSpaceChar(ReadOnlySpan<char> source, char c)
        {
            for (int i = 0; i < source.Length; i++)
            {
                var currChar = source[i];
                if (char.IsWhiteSpace(currChar))
                {
                    continue;
                }
                else if (currChar == c)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if the last non whitespace char equals c
        /// </summary>
        /// <param name="source">Content</param>
        /// <param name="c">Char to check</param>
        /// <returns>True, if last non whitespace char equals c</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool EqualLastNoWhiteSpaceChar(ReadOnlySpan<char> source, char c)
        {
            for (int i = source.Length - 1; i >= 0; i--)
            {
                var currChar = source[i];
                if (char.IsWhiteSpace(currChar))
                {
                    continue;
                }
                else if (currChar == c)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if the content starting at startIndex equals "null"
        /// </summary>
        /// <param name="source">Content</param>
        /// <param name="startIndex">Start index</param>
        /// <returns>True, if equals null</returns>
        internal static bool EqualsNullString(ReadOnlySpan<char> source, int startIndex = 0)
        {
            if ((uint)startIndex >= (uint)source.Length)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
            }

            if (source.Length - startIndex != 4)
            {
                return false;
            }

            if (char.ToLower(source[startIndex]) != CommonConstants.N)
                return false;
            if (char.ToLower(source[startIndex + 1]) != CommonConstants.U)
                return false;
            if (char.ToLower(source[startIndex + 2]) != CommonConstants.L)
                return false;
            if (char.ToLower(source[startIndex + 3]) != CommonConstants.L)
                return false;

            return true;
        }

        /// <summary>
        /// Returns the off set of the trimed ReadOnlySpan.
        /// </summary>
        /// <param name="source">Content</param>
        /// <param name="startTrim">Offset for start</param>
        /// <param name="endTrim">Offset for end</param>
        /// <returns>True, if ReadOnlySpan can be trimmed</returns>
        internal static bool GetTrimParameter(ReadOnlySpan<char> source, out int startTrim, out int endTrim)
        {
            startTrim = 0;
            endTrim = 0;

            if (source.IsEmpty)
            {
                return false;
            }

            bool canBeTrimed = false;

            if (char.IsWhiteSpace(source[0]))
            {
                int count = 0;
                for (int i = 0; i < source.Length; i++)
                {
                    if (char.IsWhiteSpace(source[i]))
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }

                startTrim = count;
                canBeTrimed = true;
            }

            if (char.IsWhiteSpace(source[^1]))
            {
                int count = 0;
                for (int i = source.Length - 1; i > -1; i--)
                {
                    if (char.IsWhiteSpace(source[i]))
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }

                endTrim = count;
                canBeTrimed = true;
            }

            if (startTrim >= source.Length)
            {
                startTrim = 0;
                endTrim = source.Length;
            }

            return canBeTrimed;
        }

        /// <summary>
        /// Check if start and end char is a quote
        /// </summary>
        /// <param name="source">Content</param>
        /// <returns>True, if start end and end char is quote</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool HasStartAndEndQuotes(ReadOnlySpan<char> source)
        {
            if (source.Length < 2)
                return false;

            return source[0] == CommonConstants.Quote && source[^1] == CommonConstants.Quote;
        }

        /// <summary>
        /// Check if content is null or only contains whitespaces
        /// </summary>
        /// <param name="source">Content</param>
        /// <returns>True, if null or only whitspaces</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsNullOrWhiteSpace(ReadOnlySpan<char> source)
        {
            if (source.IsEmpty)
                return true;

            for (int i = 0; i < source.Length; i++)
            {
                if (!char.IsWhiteSpace(source[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the index, where the closed char is hit.
        /// </summary>
        /// <param name="source">Content</param>
        /// <param name="startIndex">Start index</param>
        /// <param name="openChar">Open char</param>
        /// <param name="closeChar">Close char</param>
        /// <returns>Index, of the next close char</returns>
        internal static int SkipEnclosingValue(ReadOnlySpan<char> source, int startIndex, char openChar, char closeChar)
        {
            if (source.IsEmpty || source.IsWhiteSpace())
            {
                throw new ArgumentException("Source is empty or whitespace.");
            }

            if ((uint)startIndex >= (uint)source.Length)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
            }

            if (source[startIndex] != openChar)
            {
                ThrowHelper.ThrowUnexpectedNonWhiteSpaceCharException(startIndex, source[startIndex]);
            }

            if (openChar == CommonConstants.Quote || closeChar == CommonConstants.Quote)
            {
                throw new ArgumentException($"Open and/or close char can't be quotes.");
            }

            bool isEscaped = false;
            int numOpen = 0;

            for (int j = startIndex + 1; j < source.Length; j++)
            {
                char c = source[j];

                if (isEscaped)
                {
                    isEscaped = false;
                }
                else if (c == CommonConstants.Backslash)
                {
                    isEscaped = true;
                }
                else if (c == closeChar)
                {
                    if (numOpen == 0)
                    {
                        return j;
                    }
                    else
                    {
                        numOpen--;
                    }
                }
                else if (c == openChar)
                {
                    numOpen++;
                }
                else if (c == CommonConstants.Quote)
                {
                    j = SkipQuotedValue(source, j);
                }
            }

            ThrowHelper.ThrowMissingClosingCharException(startIndex, closeChar);
            throw new Exception("Unreachable code");
        }

        /// <summary>
        /// Returns the index, where the next non escape quote char is hit.
        /// </summary>
        /// <param name="source">Content</param>
        /// <param name="startIndex">Start index</param>
        /// <returns>Index of the next non escape quote.</returns>
        internal static int SkipQuotedValue(ReadOnlySpan<char> source, int startIndex)
        {
            if (source.IsEmpty || source.IsWhiteSpace())
            {
                throw new ArgumentException("Source is empty or whitespace.");
            }

            if ((uint)startIndex >= (uint)source.Length)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
            }

            if (source[startIndex] != CommonConstants.Quote)
            {
                ThrowHelper.ThrowUnexpectedNonWhiteSpaceCharException(startIndex, source[startIndex]);
            }

            bool isEscaped = false;

            for (int j = startIndex + 1; j < source.Length; j++)
            {
                char c = source[j];

                if (isEscaped)
                {
                    isEscaped = false;
                }
                else if (c == CommonConstants.Backslash)
                {
                    isEscaped = true;
                }
                else if (c == CommonConstants.Quote)
                {
                    return j;
                }
            }

            ThrowHelper.ThrowUnterminatedStringException(startIndex);
            throw new Exception("Unreachable code");
        }

        /// <summary>
        /// Returns the index, where the next stop char is hit.
        /// </summary>
        /// <param name="source">Content</param>
        /// <param name="startIndex">Start index</param>
        /// <param name="stopChar">Stop char</param>
        /// <param name="stopAtNewLine">True, if also should be stopped at newline</param>
        /// <returns>Index, of the next hit of the stop char.</returns>
        internal static int SkipTillStopChar(
            ReadOnlySpan<char> source,
            int startIndex,
            char? stopChar,
            bool stopAtNewLine = false
        )
        {
            if ((uint)startIndex >= (uint)source.Length)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
            }

            if (null == stopChar && !stopAtNewLine)
            {
                return source.Length - 1;
            }

            bool isEscaped = false;

            for (int j = startIndex; j < source.Length; j++)
            {
                char c = source[j];

                if (stopAtNewLine && c.IsNewLine())
                {
                    return j - 1;
                }

                if (isEscaped)
                {
                    isEscaped = false;
                }
                else if (c == CommonConstants.Backslash)
                {
                    isEscaped = true;
                }
                else if (c == stopChar)
                {
                    return j - 1;
                }
            }

            if (isEscaped)
            {
                ThrowHelper.ThrowUnterminatedCharException(startIndex);
                throw new Exception("Unreachable code");
            }

            return source.Length - 1;
        }

        /// <summary>
        /// Returns the index of the content, before the stop char was hit.
        /// </summary>
        /// <param name="source">Content</param>
        /// <param name="startIndex">Start index</param>
        /// <param name="stopChars">Stop chars</param>
        /// <param name="stopAtNewLine">True, if also should be stopped at a newline</param>
        /// <returns>Index before stop char was hit.</returns>
        internal static int SkipTillStopChars(
            ReadOnlySpan<char> source,
            int startIndex,
            ReadOnlySpan<char> stopChars,
            bool stopAtNewLine = false
        )
        {
            if ((uint)startIndex >= (uint)source.Length)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
            }

            if (stopChars.IsEmpty)
            {
                return source.Length - 1;
            }

            bool isEscaped = false;

            for (int j = startIndex; j < source.Length; j++)
            {
                char c = source[j];

                if (stopAtNewLine && c.IsNewLine())
                {
                    return j - 1;
                }

                if (isEscaped)
                {
                    isEscaped = false;
                }
                else if (c == CommonConstants.Backslash)
                {
                    isEscaped = true;
                }
                else if (stopChars.IndexOf(c) >= 0)
                {
                    return j - 1;
                }
            }

            if (isEscaped)
            {
                ThrowHelper.ThrowUnterminatedCharException(startIndex);
                throw new Exception("Unreachable code");
            }

            return source.Length - 1;
        }

        /// <summary>
        /// Creates a new ReadOnylSpan from a source ReadOnylSpan
        /// </summary>
        /// <param name="source">Source ReadOnlySpan</param>
        /// <param name="start">Start index</param>
        /// <param name="end">End index</param>
        /// <returns>New ReadOnlySpan</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ReadOnlySpan<char> SliceFromTo(ReadOnlySpan<char> source, int start, int end)
        {
            if ((uint)start > (uint)source.Length || (uint)end > (uint)source.Length || start > end)
            {
                throw new ArgumentOutOfRangeException();
            }

            int length = end - start + 1;

            return source.Slice(start, length);
        }
    }
}
