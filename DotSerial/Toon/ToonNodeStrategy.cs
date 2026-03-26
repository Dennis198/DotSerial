using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Toon
{
    /// <summary>
    /// Toon strategy for parsing and writing.
    /// </summary>
    internal class ToonNodeStrategy : INodeStrategy
    {
        /// <inheritdoc/>
        public bool AreQuotesNeededForKey(string key)
        {
            if (key == null || key.Length == 0)
            {
                ThrowHelper.ThrowKeyNodeNullException();
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                return true;
            }

            if (key.HasLeadingOrTrailingWhitespaces())
            {
                return true;
            }

            if (key.IsNumericValue())
            {
                return false;
            }

            if (key.ContainsChars(ToonConstants.ToonSpecialChars))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool AreQuotesNeededForValue(object? value, string? strValue)
        {
            if (null == value)
                return false;

            Type type = value.GetType();

            if (type == typeof(bool) || type == typeof(bool?))
                return false;

            bool isNumeric = TypeCheckMethods.IsNumericType(type);

            if (type == typeof(string) && isNumeric)
                return true;
            if (type != typeof(string) && isNumeric)
                return false;

            if (string.Empty == strValue)
                return true;

            if (string.IsNullOrWhiteSpace(strValue))
            {
                return true;
            }

            if (strValue.EqualsNullString())
                return true;

            if (strValue.EqualsBooleanString())
                return true;

            if (strValue.HasLeadingOrTrailingWhitespaces())
                return true;

            if (strValue.ContainsChars(ToonConstants.ToonSpecialChars))
            {
                return true;
            }

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

            string? strValue = value != null ? HelperMethods.PrimitiveToString(value) : null;
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

            key = key.UnescapeString(ToonConstants.CharsToEscape);

            // Create inner node
            if (type != TreeNodeType.Leaf)
            {
                return INodeStrategy.CreateInnerNode(key, type, parent);
            }

            if (bookmark.IsNull())
            {
                return new LeafNode(key, null, false, parent);
            }

            // TODO " null" ListFirstElementNull
            // string? value = bookmark.GetContent(content).ToString();
            string? value = bookmark.GetContent(content).ToString().Trim();

            if (string.IsNullOrWhiteSpace(value) || value.EqualsNullString())
            {
                return new LeafNode(key, null, false, parent);
            }

            bool needQuotes = false;

            if (value.HasStartAndEndQuotes())
            {
                needQuotes = true;
                value = StringMethods.RemoveStartAndEndQuotes(value);
            }

            value = value.UnescapeString(ToonConstants.CharsToEscape);

            if (false == needQuotes && false == IsValueValidWithoutQuotes(value))
            {
                ThrowHelper.ThrowUnterminatedStringException(bookmark.Start);
            }

            return new LeafNode(key, value, needQuotes, parent);
        }

        /// <inheritdoc/>
        public bool IsValueValidWithoutQuotes(string value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (value.EqualsNullString())
                return true;

            if (value.EqualsBooleanString())
                return true;

            if (value.IsNumericValue())
                return true;

            if (value.HasLeadingOrTrailingWhitespaces())
                return false;

            if (value.ContainsChars(ToonConstants.ToonSpecialChars))
                return false;

            return true;
        }
    }
}
