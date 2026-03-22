using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using DotSerial.Common;
using DotSerial.Common.Parser;
using DotSerial.Common.Writer;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Tree.Serialize;

namespace DotSerial
{
    public class DSNode : IDictionary<string, DSNode>
    {
        /// <summary>Writer factory instance</summary>
        private static readonly WriterFactory _writerFactory = WriterFactory.Instance;

        /// <summary>Parser factory instance</summary>
        private static readonly ParserFactory _parserFactory = ParserFactory.Instance;

        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <summary>Internal node </summary>
        private IDSNode _node;

        /// <summary>
        /// Key of the node.
        /// </summary>
        public string Key => _node.Key;

        public readonly SerializeStrategy Strategy;

        /// <summary>
        /// Check if node has children.
        /// </summary>
        /// <returns>True if the node has children, otherwise false.</returns>
        public bool HasChildren => _node.HasChildren();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="strategy">Serialization strategy</param>
        internal DSNode(IDSNode node, SerializeStrategy strategy)
        {
            _node = node;
            Strategy = strategy;
        }

        public static DSNode FromString(ReadOnlySpan<char> str, SerializeStrategy strategy)
        {
            var root = _parserFactory.Parse(strategy, str);
            return root;
        }

        public static DSNode ToNode(object? obj, SerializeStrategy strategy, string? key = null)
        {
            string currKey = key ?? CommonConstants.MainObjectKey;
            var rootNode = SerializeObject.Serialize(obj, currKey, strategy);

            return new DSNode(rootNode, strategy);
        }

        public static U ToObject<U>(ReadOnlySpan<char> str, SerializeStrategy strategy)
        {
            DSNode dsNode = FromString(str, strategy);

            return IDSSerialNode<U>.ToObject<U>(dsNode.GetInternalData());
        }

        public string Stringify()
        {
            if (null == _node)
            {
                throw new DotSerialException($"{_node} can't be null.");
            }

            var jsonString = _writerFactory.Write(Strategy, this);

            return new string(jsonString);
        }

        public static string Stringify(object? obj, SerializeStrategy strategy)
        {
            string currKey = CommonConstants.MainObjectKey;
            var rootNode = SerializeObject.Serialize(obj, currKey, strategy);
            var jsonString = _writerFactory.Write(strategy, new DSNode(rootNode, strategy));

            return new string(jsonString);
        }

        /// <summary>
        /// Returns the internal node
        /// </summary>
        /// <returns>IDSNode</returns>
        internal IDSNode GetInternalData()
        {
            return _node;
        }

        public DSNode this[string key]
        {
            get { return new DSNode(_node.GetChild(key), Strategy); }
            set
            {
                ArgumentNullException.ThrowIfNull(key);

                if (Strategy != value.Strategy)
                {
                    throw new DotSerialException("Can't set a node with a different strategy type.");
                }

                var node = value.GetInternalData();

                node.Remove(key);

                _node.AddChild(node);
            }
        }

        public ICollection<string> Keys => _node.Keys;

        public ICollection<DSNode> Values => [.. _node.Values.Select(child => new DSNode(child, Strategy))];

        /// <summary>
        /// Number of items
        /// </summary>
        public int Count => _node.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(string key, DSNode value)
        {
            ArgumentNullException.ThrowIfNull(key);

            if (Strategy != value.Strategy)
            {
                throw new DotSerialException("Can't add a node with a different strategy type.");
            }

            var node = value.GetInternalData();

            _node.AddChild(node);
        }

        public void Add(KeyValuePair<string, DSNode> item)
        {
            Add(item.Key, item.Value);
        }

        public bool TryAdd(KeyValuePair<string, DSNode> item)
        {
            return TryAdd(item.Key, item.Value);
        }

        public bool TryAdd(string key, DSNode value)
        {
            ArgumentNullException.ThrowIfNull(key);

            if (Strategy != value.Strategy)
            {
                return false;
            }

            if (ContainsKey(key))
            {
                return false;
            }

            var node = value.GetInternalData();

            _node.AddChild(node);
            return true;
        }

        public void Clear()
        {
            _node.Clear();
        }

        public bool Contains(KeyValuePair<string, DSNode> item)
        {
            return ContainsKey(item.Key);
        }

        public bool ContainsKey(string key)
        {
            return _node.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, DSNode>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Index is out of range.");
            }

            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException("The array does not have enough space to copy the elements.");
            }

            foreach (var child in _node.GetChildren())
            {
                array[arrayIndex++] = new KeyValuePair<string, DSNode>(child.Key, new DSNode(child, Strategy));
            }
        }

        public IEnumerator<KeyValuePair<string, DSNode>> GetEnumerator()
        {
            foreach (var child in _node.GetChildren())
            {
                yield return new KeyValuePair<string, DSNode>(child.Key, new DSNode(child, Strategy));
            }
        }

        public bool Remove(string key)
        {
            return _node.Remove(key);
        }

        public bool Remove(KeyValuePair<string, DSNode> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out DSNode value)
        {
            try
            {
                value = this[key];
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ABC GetNodeType()
        {
            if (_node is LeafNode)
            {
                return ABC.Value;
            }
            if (_node is InnerNode || _node is DictionaryNode)
            {
                return ABC.Object;
            }
            if (_node is ListNode)
            {
                return ABC.Array;
            }

            throw new DotSerialException("Unknown node type.");
        }

        public string? GetNodeValue()
        {
            return _node.GetValue();
        }

        public void SetNodeValue(string? value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(bool value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(byte value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(sbyte value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(char value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(decimal value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(double value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(float value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(int value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(uint value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(nint value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(nuint value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(long value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(ulong value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(short value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(ushort value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(DateTime value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(Guid value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(TimeSpan value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(Uri value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(IPAddress value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(Version value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }

        public void SetNodeValue(CultureInfo value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, NodeType.Leaf);
        }
    }
}
