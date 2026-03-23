using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Yaml
{
    /// <summary>
    /// Yaml strategy for parsing and writing.
    /// </summary>
    internal class YamlNodeStrategy : INodeStrategy
    {
        /// <inheritdoc/>
        public IDSNode CreateNode(string key, object? value, TreeNodeType type)
        {
            if (null == key || key.Length == 0)
            {
                throw new DotSerialException("NodeFactory: Key can't be null.");
            }

            if (null != value && (type != TreeNodeType.Leaf))
            {
                throw new DotSerialException("NodeFactory: Only leaf nodes can have a value.");
            }

            // Create inner node
            if (type != TreeNodeType.Leaf)
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
        public IDSNode CreateNodeFromString(string key, string? value, TreeNodeType type)
        {
            if (key.HasStartAndEndQuotes())
            {
                key = StringMethods.RemoveStartAndEndQuotes(key);
            }

            if (key == null || key.Length == 0)
            {
                throw new DotSerialException("NodeFactory: Key can't be null.");
            }

            if (null != value && (type != TreeNodeType.Leaf))
            {
                throw new DotSerialException("NodeFactory: Only leaf nodes can have a value.");
            }

            key = key.UnescapeString(YamlConstants.CharsToEscape);

            // Create inner node
            if (type != TreeNodeType.Leaf)
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

            value = value.UnescapeString(YamlConstants.CharsToEscape);

            if (false == needQuotes && false == IsValueValidWithoutQuotes(value))
            {
                throw new DotSerialException("NodeFactory: Invalid yaml value.");
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

            if (strValue.ContainsChars(YamlConstants.YamlSpecialChars))
            {
                return true;
            }
            for (int i = 0; i < strValue.Length; i++)
            {
                char c = strValue[i];
                if (YamlConstants.YamlSpecialChars.Contains(c))
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

            if (key.ContainsChars(YamlConstants.YamlSpecialChars))
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

            if (value.ContainsChars(YamlConstants.YamlSpecialChars))
                return false;

            return true;
        }
    }
}
