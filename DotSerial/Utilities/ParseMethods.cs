using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Channels;
using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;

namespace DotSerial.Utilities
{
    /// <summary>
    /// Class is used to verious methods for parsing.
    /// </summary>
    internal static class ParseMethods
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <summary>
        /// Apends the whole string from starting quote to end quote to
        /// the stringbuilder.
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="startIndex">Index of the opeing quote</param>
        /// <param name="str">string</param>
        /// <returns>Index of the closing quote</returns>
        internal static int AppendStringValue(StringBuilder sb, int startIndex, StringBuilder str)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (str.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(str.ToString());
            }

            if (str.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (str[startIndex] != CommonConstants.Quote)
            {
                throw new ArgumentException(str.ToString());
            }

            bool isEscaped = false;
            sb.Append(CommonConstants.Quote);

            for (int j = startIndex + 1; j < str.Length; j++)
            {
                var c2 = str[j];

                if (isEscaped)
                {
                    sb.Append(c2);
                    isEscaped = false;
                }
                else if (c2 == CommonConstants.Backslash)
                {
                    isEscaped = true;
                    sb.Append(c2);
                }
                else if (c2 == CommonConstants.Quote)
                {
                    sb.Append(c2);
                    return j;
                }
                else
                {
                    sb.Append(c2);
                }
            }

            throw new DotSerialException("Parse: Escapable char is not escaped.");
        }

        /// <summary>
        /// Append a string to a StringBuilder till a char is reached.
        /// </summary>
        /// <param name="sb">StrginBuilder to append</param>
        /// <param name="startIndex">Start index</param>
        /// <param name="str">StringBuilder to check</param>
        /// <param name="stopChar">Stop char</param>
        /// <returns>StringBuilder with the appended chars</returns>
        internal static int AppendTillStopChar(StringBuilder sb, int startIndex, StringBuilder str, char? stopChar)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (str.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(str.ToString());
            }

            if (null == stopChar)
            {
                sb.Append(str.SubString(startIndex, str.Length - startIndex));
                return str.Length - 1;
            }

            if (str.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            bool isEscaped = false;
            sb.Append(str[startIndex]);

            for (int i = startIndex + 1; i < str.Length; i++)
            {
                var c = str[i];

                if (isEscaped)
                {
                    sb.Append(c);
                    isEscaped = false;
                }
                else if (c == CommonConstants.Backslash)
                {
                    isEscaped = true;
                    sb.Append(c);
                }
                else if (c == stopChar)
                {
                    return i - 1;
                }
                else
                {
                    sb.Append(c);
                }
            }

            if (true == isEscaped)
            {
                throw new DotSerialException("Parse: Escapable char is not escaped.");
            }

            return str.Length - 1;
        }

        /// <summary>
        /// Append a string to a StringBuilder till a char is reached.
        /// </summary>
        /// <param name="sb">StrginBuilder to append</param>
        /// <param name="startIndex">Start index</param>
        /// <param name="str">StringBuilder to check</param>
        /// <param name="stopChars">Stop chars</param>
        /// <returns>StringBuilder with the appended chars</returns>
        internal static int AppendTillStopChars(StringBuilder sb, int startIndex, StringBuilder str, char[]? stopChars)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (str.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(str.ToString());
            }

            if (null == stopChars)
            {
                sb.Append(str.SubString(startIndex, str.Length - startIndex));
                return str.Length - 1;
            }

            if (str.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            bool isEscaped = false;
            sb.Append(str[startIndex]);

            for (int i = startIndex + 1; i < str.Length; i++)
            {
                var c = str[i];

                if (isEscaped)
                {
                    sb.Append(c);
                    isEscaped = false;
                }
                else if (c == CommonConstants.Backslash)
                {
                    isEscaped = true;
                    sb.Append(c);
                }
                else if (stopChars.Contains(c))
                {
                    return i - 1;
                }
                else
                {
                    sb.Append(c);
                }
            }

            if (true == isEscaped)
            {
                throw new DotSerialException("Parse: Escapable char is not escaped.");
            }

            return str.Length - 1;
        }

        /// <summary>
        /// Appends an enclosed value from starting openChar to closing closeChar to
        /// the stringbuilder.
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="startIndex">Index of the opeing quote</param>
        /// <param name="str">Stringbuilder</param>
        /// <param name="openChar">Open char</param>
        /// <param name="closeChar">Closing char</param>
        /// <returns>Index of the closing char</returns>
        internal static int AppendEnclosingValue(
            StringBuilder sb,
            int startIndex,
            StringBuilder str,
            char openChar,
            char closeChar
        )
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (str.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(str.ToString());
            }

            if (str.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (str[startIndex] != openChar)
            {
                throw new ArgumentException(str.ToString());
            }

            bool isEscaped = false;
            sb.Append(openChar);

            int numOpen = 0;

            for (int i = startIndex + 1; i < str.Length; i++)
            {
                var c = str[i];

                if (isEscaped)
                {
                    sb.Append(c);
                    isEscaped = false;
                }
                else if (c == CommonConstants.Backslash)
                {
                    isEscaped = true;
                    sb.Append(c);
                }
                else if (c == closeChar)
                {
                    if (numOpen == 0)
                    {
                        sb.Append(c);
                        return i;
                    }
                    else
                    {
                        numOpen--;
                        sb.Append(c);
                    }
                }
                else if (c == openChar)
                {
                    numOpen++;
                    sb.Append(c);
                }
                else
                {
                    sb.Append(c);
                }
            }

            if (true == isEscaped)
            {
                throw new DotSerialException("Parse: Escapable char is not escaped.");
            }

            return str.Length - 1;
        }

        /// <summary>
        /// Removes all whitespaces inside a string
        /// except whitespace is between quotes.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>String without whitespaces.</returns>
        internal static StringBuilder RemoveWhiteSpace(string str)
        {
            // Check if value has value
            if (string.IsNullOrWhiteSpace(str))
            {
                return new StringBuilder();
            }

            StringBuilder sb = new();
            int stringLength = str.Length;
            StringBuilder strBuilder = new(str);

            for (int i = 0; i < stringLength; i++)
            {
                var c = str[i];

                // If char is a quoto extract everything
                // till the closing quote is reached
                if (c == CommonConstants.Quote)
                {
                    i = AppendStringValue(sb, i, strBuilder);
                    continue;
                }
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                sb.Append(c);
            }

            return sb;
        }

        /// <summary>
        /// Parses primitive node without a key, e.g "3.14"
        /// </summary>
        /// <param name="strategyType">Strategy type</param>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="startIndex">StartIndex</param>
        /// <param name="key">Key of the node</param>
        /// <returns>Leafnode</returns>
        internal static IDSNode ParsePrimitiveNode(
            StategyType strategyType,
            StringBuilder sb,
            int startIndex,
            string key,
            char[]? stopChars = null
        )
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (sb.IsNullOrWhiteSpace())
            {
                return _nodeFactory.CreateNodeFromString(strategyType, key, null, NodeType.Leaf);
            }

            StringBuilder sbPrim = new();

            if (sb.HasStartAndEndQuotes())
            {
                int i = AppendStringValue(sbPrim, startIndex, sb);
                if (i != sb.Length - 1)
                {
                    throw new DotSerialException("Parse: Can't parse single value.");
                }
            }
            else
            {
                int i = AppendTillStopChars(sbPrim, startIndex, sb, stopChars);
                if (i != sb.Length - 1)
                {
                    throw new DotSerialException("Parse: Can't parse single value.");
                }
            }

            string nodeValue = sbPrim.ToString();

            return _nodeFactory.CreateNodeFromString(strategyType, key, nodeValue, NodeType.Leaf);
        }

        internal static IDSNode ParsePrimitiveNode2(
            StategyType strategyType,
            ReadOnlySpan<char> sb,
            int startIndex,
            string key,
            char[]? stopChars = null
        )
        {
            if (ReadOnlySpanMethods.IsNullOrWhiteSpace(sb))
            {
                return _nodeFactory.CreateNodeFromString(strategyType, key, null, NodeType.Leaf);
            }

            string? nodeValue;
            int end;

            if (ReadOnlySpanMethods.HasStartAndEndQuotes(sb))
            {
                end = ReadOnlySpanMethods.SkipQuotedValue(sb, startIndex);
                if (end != sb.Length - 1)
                {
                    throw new DotSerialException("Parse: Can't parse single value.");
                }

                nodeValue = sb.Slice(startIndex, end - startIndex + 1).ToString();
            }
            else
            {
                end = ReadOnlySpanMethods.SkipTillStopChars(sb, startIndex, stopChars);
                if (end != sb.Length - 1)
                {
                    throw new DotSerialException("Parse: Can't parse single value.");
                }
                // nodeValue = sb[startIndex..end].ToString();
            }

            nodeValue = sb.Slice(startIndex, end - startIndex + 1).ToString();

            return _nodeFactory.CreateNodeFromString(strategyType, key, nodeValue, NodeType.Leaf);
        }

        /// <summary>
        /// Returns the indentation level of a line
        /// </summary>
        /// <param name="lines">Stringbuilder</param>
        /// <param name="indentationSize">IndentationSize</param>
        /// <returns>indentation level of a line</returns>
        internal static int LineLevel(StringBuilder line, int indentationSize)
        {
            ArgumentNullException.ThrowIfNull(line);

            if (indentationSize < 1)
            {
                throw new DotSerialException("Parse: Indentation size must be at least 1.");
            }

            int level = 0;
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == CommonConstants.WhiteSpace)
                {
                    level++;
                }
                else
                {
                    break;
                }
            }

            return level / indentationSize;
        }

        internal static int LineLevel2(ReadOnlySpan<char> line, int indentationSize)
        {
            if (indentationSize < 1)
            {
                throw new DotSerialException("Parse: Indentation size must be at least 1.");
            }

            // TODO mit indeof????
            int level = 0;
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == CommonConstants.WhiteSpace)
                {
                    level++;
                }
                else
                {
                    break;
                }
            }

            return level / indentationSize;
        }
    }
}
