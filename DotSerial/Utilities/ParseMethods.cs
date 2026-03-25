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
        /// <param name="bookmark">Parser bookmark</param>
        /// <param name="key">Key of the node</param>
        /// <returns>Leafnode</returns>
        internal static IDSNode ParsePrimitiveNode(
            SerializeStrategy strategyType,
            ReadOnlySpan<char> content,
            ParserBookmark bookmark,
            string key
        )
        {
            if (ReadOnlySpanMethods.IsNullOrWhiteSpace(content))
            {
                return _nodeFactory.CreateNodeFromString(strategyType, key, bookmark, content, TreeNodeType.Leaf);
            }

            return _nodeFactory.CreateNodeFromString(strategyType, key, bookmark, content, TreeNodeType.Leaf);
        }

        /// <summary>
        /// Returns the indentation level of a line
        /// </summary>
        /// <param name="lines">ReadOnlySpan</param>
        /// <param name="indentationSize">IndentationSize</param>
        /// <returns>indentation level of a line</returns>
        internal static int LineLevel(ReadOnlySpan<char> line, int indentationSize)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(indentationSize, 1);

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
