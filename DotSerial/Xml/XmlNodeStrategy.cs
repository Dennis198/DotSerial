using System.Text;
using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Xml
{
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
                return INodeStrategy.CreateNotLeafNode(key, type);
            }

            if (null == value)
            {
                // throw new DotSerialException("NodeFactory: value can't be null.");
                return new LeafNode(key, null, false);
            }

            // bool needQuotes = DoValueNeedQuotes(value);
            // bool needQuotes = false;//DoValueNeedQuotes(value);
            string? strValue = value != null ? HelperMethods.PrimitiveToString(value) : null;
            // TODO <, >, & ersetzen mit &lt; &gt; &amp;

            return new LeafNode(key, strValue, false);
        }

        /// <inheritdoc/>
        public IDSNode CreateNodeFromString(string key, string? value, NodeType type)
        {
            string keyWithoutQuotes = key;
            if (keyWithoutQuotes[0] == CommonConstants.Quote && keyWithoutQuotes[^1] == CommonConstants.Quote)
            {
                keyWithoutQuotes = ParseMethods.RemoveStartAndEndQuotes(key);
            }
            // else
            // {
            //     // throw new DotSerialException("NodeFactory: Key must be quoted.");
            //     throw  new NotImplementedException();
            // }

           if (string.IsNullOrWhiteSpace(keyWithoutQuotes))
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
                return INodeStrategy.CreateNotLeafNode(keyWithoutQuotes, type);
            }

            if (null == value)
            {
                return new LeafNode(keyWithoutQuotes, null, false);
            }

            if (value.Equals("\"\""))
            {
                return new LeafNode(keyWithoutQuotes, string.Empty, true);
            }

            // TODO
            StringBuilder tmp = new(value.ToString());

            if (tmp.Length == 0)
            {
                return new LeafNode(keyWithoutQuotes, string.Empty, false);
            }

            if (tmp.EqualsNullString())
            {
                return new LeafNode(keyWithoutQuotes, CommonConstants.Null, false);
            }

            // TODO <, >, & ersetzen mit &lt; &gt; &amp;

            return new LeafNode(keyWithoutQuotes, tmp.ToString(), false);
        }

        private bool DoValueNeedQuotes(object? value)
        {
            if (null == value)
                return false;

            return true;
                
            // Type type = value.GetType();

            // if (type == typeof(string))
            //     return true;

            // if (type == typeof(bool) || type == typeof(bool?))
            //     return false;

            // if (TypeCheckMethods.IsNumericType(type))
            //     return false;             

            // return true;
        }
    }
}