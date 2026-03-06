using DotSerial.Common;
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
        public IDSNode CreateNode(string key, object? value, NodeType type)
        {
            if (string.IsNullOrWhiteSpace(key))
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

            // TODO <, >, & ersetzen mit &lt; &gt; &amp;
            string? strValue = value != null ? HelperMethods.PrimitiveToString(value) : null;            

            return new LeafNode(key, strValue, false);
        }

        /// <inheritdoc/>
        public IDSNode CreateNodeFromString(string key, string? value, NodeType type)
        {
            if (key.HasStartAndEndQuotes())
            {
                key = StringMethods.RemoveStartAndEndQuotes(key);
            }

           if (string.IsNullOrWhiteSpace(key))
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

            if (value.Equals("\"\""))
            {
                return new LeafNode(key, string.Empty, true);
            }

            if (value.Length == 0)
            {
                return new LeafNode(key, string.Empty, false);
            }

            if (value.EqualsNullString())
            {
                return new LeafNode(key, CommonConstants.Null, false);
            }

            // TODO <, >, & ersetzen mit &lt; &gt; &amp;

            return new LeafNode(key, value, false);
        }

        /// <inheritdoc/>
        public bool IsValueValidWithoutQuotes(string value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool AreQuotesNeededForKey(string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool AreQuotesNeededForValue(object? value, string? strValue)
        {
            throw new NotImplementedException();
        }        
    }
}