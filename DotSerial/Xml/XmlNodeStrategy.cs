using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Xml
{
    /// <summary>
    /// Xml strategy for parsing and writing.
    /// </summary>
    internal class XmlNodeStrategy : INodeStrategy
    {
        /// <inheritdoc/>
        public bool AreQuotesNeededForKey(string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool AreQuotesNeededForValue(object? value, string? strValue)
        {
            if (null == value)
                return false;

            Type type = value.GetType();

            if (type == typeof(string) && string.IsNullOrWhiteSpace(strValue))
                return true;

            return false;
        }

        /// <inheritdoc/>
        public IDSNode CreateNode(string key, object? value, TreeNodeType type, IDSNode? parent)
        {
            if (null == key || key.Length == 0)
            {
                ThrowHelper.ThrowKeyNodeNullException();
            }

            if (null != value && (type != TreeNodeType.Leaf))
            {
                ThrowHelper.ThrowInnerNodeValueException();
            }

            // Create inner node
            if (type != TreeNodeType.Leaf)
            {
                return INodeStrategy.CreateInnerNode(key, type, parent);
            }

            if (null == value)
            {
                return new LeafNode(key, null, false, parent);
            }

            string? strValue = value != null ? ConverterMethods.PrimitiveToString(value) : null;
            bool needQuotes = AreQuotesNeededForValue(value, strValue);

            return new LeafNode(key, strValue, needQuotes, parent);
        }

        /// <inheritdoc/>
        public IDSNode CreateNodeFromString(
            string key,
            ParserBookmark bookmark,
            ReadOnlySpan<char> content,
            TreeNodeType type,
            IDSNode? parent
        )
        {
            if (key.HasStartAndEndQuotes())
            {
                key = StringMethods.RemoveStartAndEndQuotes(key);
            }

            if (key == null || key.Length == 0)
            {
                ThrowHelper.ThrowKeyNodeNullException();
            }

            if (false == bookmark.IsNull() && (type != TreeNodeType.Leaf))
            {
                ThrowHelper.ThrowInnerNodeValueException();
            }

            key = key.XmlUnEscape();

            // Create inner node
            if (type != TreeNodeType.Leaf)
            {
                return INodeStrategy.CreateInnerNode(key, type, parent);
            }

            if (bookmark.IsNull())
            {
                return new LeafNode(key, null, false, parent);
            }

            string value = bookmark.GetContent(content).ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                return new LeafNode(key, null, false, parent);
            }

            bool needQuotes = false;

            if (value.HasStartAndEndQuotes())
            {
                needQuotes = true;
                value = StringMethods.RemoveStartAndEndQuotes(value);
            }

            value = value.XmlUnEscape();

            return new LeafNode(key, value, needQuotes, parent);
        }

        /// <inheritdoc/>
        public bool IsValueValidWithoutQuotes(string value)
        {
            throw new NotImplementedException();
        }
    }
}
