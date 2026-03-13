using System.Threading.Channels;
using DotSerial.Common;

namespace DotSerial.Utilities
{
    internal static class ReadOnlySpanMethods
    {
        public static ReadOnlySpan<char> RemoveWhitespacesOutsideQuotes(
            ReadOnlySpan<char> input,
            Span<char> destination,
            out int length
        )
        {
            int destIndex = 0;
            bool inQuotes = false;

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                // Prüfen, ob das aktuelle Zeichen ein eskaptes Anführungszeichen ist (\")
                bool isEscaped = i > 0 && input[i - 1] == '\\';

                if (c == '\"' && !isEscaped)
                {
                    inQuotes = !inQuotes;
                }

                // Regel: Behalte das Zeichen, wenn...
                // 1. wir uns innerhalb von Anführungszeichen befinden
                // 2. ODER es kein Whitespace ist
                if (inQuotes || !char.IsWhiteSpace(c))
                {
                    destination[destIndex++] = c;
                }
            }

            length = destIndex;
            return destination[..destIndex];
        }

        /// <summary>
        /// Check if the fist non whitespace char equals c
        /// </summary>
        /// <param name="source">Content</param>
        /// <param name="c">Char to check</param>
        /// <returns>True, if first non whitespace char equals c</returns>
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

            if (source.Length - startIndex < 4)
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
        /// Check if start and end char is a quote
        /// </summary>
        /// <param name="source">Content</param>
        /// <returns>True, if start end and end char is quote</returns>
        internal static bool HasStartAndEndQuotes(ReadOnlySpan<char> source)
        {
            if (source.Length < 2)
                return true;

            return source[0] == CommonConstants.Quote && source[^1] == CommonConstants.Quote;
        }

        /// <summary>
        /// Check if content is null or only contains whitespaces
        /// </summary>
        /// <param name="source">Content</param>
        /// <returns>True, if null or only whitspaces</returns>
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
                throw new ArgumentException($"Expected quote at index {startIndex}.");
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

            throw new Exception("Parse Error: Closing quote missing or invalid escape sequence.");
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
                    return j;
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
                    return j;
                }
            }

            if (isEscaped)
            {
                throw new Exception("Parse Error: Trailing escape character.");
            }

            return source.Length - 1;
        }
    }
}
