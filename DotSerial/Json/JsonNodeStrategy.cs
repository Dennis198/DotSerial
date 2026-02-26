using System.Text;
using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Json
{
    internal class JsonNodeStrategy : INodeStrategy
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
            string? strValue = HelperMethods.PrimitiveToString(value);

            return new LeafNode(key, strValue, needQuotes);
        }
    
        /// <inheritdoc/>
        public IDSNode CreateNodeFromString(string key, string? value, NodeType type)
        {
            string keyWithoutQuotes = key;
            if (keyWithoutQuotes[0] == CommonConstants.Quote && keyWithoutQuotes[^1] == CommonConstants.Quote)
            {
                keyWithoutQuotes = ParseMethods.RemoveStartAndEndQuotes(key);
            }

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


            // TODO
            StringBuilder tmp = new(value.ToString());
            if (tmp.IsNullOrWhiteSpace() || tmp.EqualsNullString())
            {
                return new LeafNode(keyWithoutQuotes, null, false);
            }

            bool needQuotes = false;

            // TODO richtig machen
            if (tmp[0] == CommonConstants.Quote && tmp[^1] == CommonConstants.Quote)
            {
                needQuotes = true;
                // Remove opening and closing quote
                tmp.Remove(0, 1);
                tmp.Remove(tmp.Length - 1, 1);
            }
            else
            {
                if (false == IsValueValidWithoutQuotes(tmp))
                {
                    throw new DotSerialException("NodeFactory: Invalid json value.");
                }
            }

            return new LeafNode(keyWithoutQuotes, tmp.ToString(), needQuotes);
        }

        private static bool DoValueNeedQuotes(object? value)
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

        private static bool IsValueValidWithoutQuotes(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);
            
            if (sb.EqualsNullString())
                return true;
                
            if (sb.EqualsBooleanString())
                return true;

            if (sb.IsNumericValue())
                return true;                 

            return false;
        }
    }
}