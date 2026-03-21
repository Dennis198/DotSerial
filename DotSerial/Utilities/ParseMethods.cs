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
        /// Parses primitive node without a key, e.g "3.14"
        /// </summary>
        /// <param name="strategyType">Strategy type</param>
        /// <param name="content">Content</param>
        /// <param name="startIndex">StartIndex</param>
        /// <param name="key">Key of the node</param>
        /// <returns>Leafnode</returns>
        internal static IDSNode ParsePrimitiveNode(
            StategyType strategyType,
            ReadOnlySpan<char> content,
            int startIndex,
            string key,
            char[]? stopChars = null
        )
        {
            if (ReadOnlySpanMethods.IsNullOrWhiteSpace(content))
            {
                return _nodeFactory.CreateNodeFromString(strategyType, key, null, NodeType.Leaf);
            }

            string? nodeValue;
            int end;

            if (ReadOnlySpanMethods.HasStartAndEndQuotes(content))
            {
                end = ReadOnlySpanMethods.SkipQuotedValue(content, startIndex);
                if (end != content.Length - 1)
                {
                    throw new DotSerialException("Parse: Can't parse single value.");
                }
            }
            else
            {
                end = ReadOnlySpanMethods.SkipTillStopChars(content, startIndex, stopChars);
                if (end != content.Length - 1)
                {
                    throw new DotSerialException("Parse: Can't parse single value.");
                }
            }

            nodeValue = ReadOnlySpanMethods.SliceFromTo(content, startIndex, end).ToString();

            return _nodeFactory.CreateNodeFromString(strategyType, key, nodeValue, NodeType.Leaf);
        }

        /// <summary>
        /// Returns the indentation level of a line
        /// </summary>
        /// <param name="lines">ReadOnlySpan</param>
        /// <param name="indentationSize">IndentationSize</param>
        /// <returns>indentation level of a line</returns>
        internal static int LineLevel(ReadOnlySpan<char> line, int indentationSize)
        {
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
    }
}
