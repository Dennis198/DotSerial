using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Json
{
    /// <summary>
    /// Json strategy for parsing and writing.
    /// </summary>
    internal class JsonNodeStrategy : INodeStrategy
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

            key = key.UnescapeString(JsonConstants.CharsToEscape);

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

            value = value.UnescapeString(JsonConstants.CharsToEscape);

            if (false == needQuotes && false == IsValueValidWithoutQuotes(value))
            {
                throw new DotSerialException("NodeFactory: Invalid json value.");
            }

            return new LeafNode(key, value, needQuotes);
        }

        /// <inheritdoc/>
        public bool AreQuotesNeededForValue(object? value, string? strValue)
        {
            if (null == value)
                return false;

            Type type = value.GetType();

            if (type == typeof(string))
                return true;

            if (type == typeof(bool) || type == typeof(bool?))
                return false;

            if (TypeCheckMethods.IsNumericType(type))
                return false;

            return true;
        }

        /// <inheritdoc/>
        public bool AreQuotesNeededForKey(string key)
        {
            throw new NotImplementedException();
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

            return false;
        }
    }
}
