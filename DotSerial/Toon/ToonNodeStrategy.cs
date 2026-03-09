using DotSerial.Common;
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
        public IDSNode CreateNode(string key, object? value, NodeType type)
        {
            if (null == key || key.Length == 0)
            {
                throw new DotSerialException("NodeFactory: Key can't be null.");
            }

            if (null != value && (type != NodeType.Leaf))
            {
                throw new DotSerialException("NodeFactory: Only leaf nodes can have a value.");
            }

            // Create inner node
            if (type != NodeType.Leaf)
            {
                return INodeStrategy.CreateInnerNode(key, type);
            }

            if (null == value)
            {
                return new LeafNode(key, null, false);
            }

            string? strValue = value != null ? HelperMethods.PrimitiveToString(value) : null;
            bool needQuotes = AreQuotesNeededForValue(value, strValue);

            return new LeafNode(key, strValue, needQuotes);
        }

        /// <inheritdoc/>
        public IDSNode CreateNodeFromString(string key, string? value, NodeType type)
        {
            if (key.HasStartAndEndQuotes())
            {
                key = StringMethods.RemoveStartAndEndQuotes(key);
            }

            if (key == null || key.Length == 0)
            {
                throw new DotSerialException("NodeFactory: Key can't be null.");
            }

            if (null != value && (type != NodeType.Leaf))
            {
                throw new DotSerialException("NodeFactory: Only leaf nodes can have a value.");
            }

            key = key.UnescapeString(ToonConstants.CharsToEscape);

            // Create inner node
            if (type != NodeType.Leaf)
            {
                return INodeStrategy.CreateInnerNode(key, type);
            }

            if (null == value)
            {
                return new LeafNode(key, null, false);
            }

            if (string.IsNullOrWhiteSpace(value) || value.EqualsNullString())
            {
                return new LeafNode(key, null, false);
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
                throw new DotSerialException("NodeFactory: Invalid toon value.");
            }

            return new LeafNode(key, value, needQuotes);
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
        public bool AreQuotesNeededForKey(string key)
        {
            if (key == null || key.Length == 0)
            {
                throw new NotImplementedException();
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
