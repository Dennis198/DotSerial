using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Yaml
{
    internal class YamlNodeStrategy : INodeStrategy
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
            bool needQuotes = DoValueNeedQuotes(value);
            string? strValue = value != null ? HelperMethods.PrimitiveToString(value) : null;

            return new LeafNode(key, strValue, needQuotes);
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