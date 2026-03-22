using System.Collections;
using System.Diagnostics.CodeAnalysis;
using DotSerial.Common;
using DotSerial.Common.Parser;
using DotSerial.Common.Writer;
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

        /// <summary>Internal node </summary>
        private readonly IDSNode _node;

        /// <summary>
        /// /// Key of the node.
        /// </summary>
        public string Key => _node.Key;

        /// <summary>
        /// Check if node has children.
        /// </summary>
        /// <returns>True if the node has children, otherwise false.</returns>
        public bool HasChildren => _node.HasChildren();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">Node</param>
        internal DSNode(IDSNode node, StategyType strategyType)
        {
            _node = node;
            StrategyType = strategyType;
        }

        public static DSNode FromString(ReadOnlySpan<char> str, StategyType strategy)
        {
            try
            {
                var root = _parserFactory.Parse(strategy, str);
                return root;
            }
            catch
            {
                throw;
            }
        }

        public static DSNode ToNode(object? obj, StategyType strategy, string? key = null)
        {
            try
            {
                // Determine key
                string currKey = key ?? CommonConstants.MainObjectKey;

                // Serialize object
                var rootNode = SerializeObject.Serialize(obj, currKey, strategy);

                return new DSNode(rootNode, strategy);
            }
            catch
            {
                throw;
            }
        }

        public static U ToObject<U>(ReadOnlySpan<char> str, StategyType strategy)
        {
            try
            {
                // Parse json string to node
                DSNode dsNode = FromString(str, strategy);

                return IDSSerialNode<U>.ToObject<U>(dsNode.GetInternalData());
            }
            catch
            {
                throw;
            }
        }

        public string Stringify(StategyType strategy)
        {
            if (null == _node)
            {
                throw new DotSerialException($"{_node} can't be null.");
            }

            try
            {
                // Convert
                var jsonString = _writerFactory.Write(strategy, this);

                return new string(jsonString);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the internal node
        /// </summary>
        /// <returns>IDSNode</returns>
        internal IDSNode GetInternalData()
        {
            return _node;
        }

        # region Interface Implementation

        public DSNode this[string key]
        {
            get { return new DSNode(_node.GetChild(key), StrategyType); }
            set
            {
                ArgumentNullException.ThrowIfNull(key);

                var node = value.GetInternalData();

                node.Remove(key);

                _node.AddChild(node);
            }
        }

        public ICollection<string> Keys => throw new NotImplementedException();

        public ICollection<DSNode> Values => throw new NotImplementedException();

        /// <summary>
        /// Number of items
        /// </summary>
        public int Count => _node.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(string key, DSNode value)
        {
            ArgumentNullException.ThrowIfNull(key);

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
                array[arrayIndex++] = new KeyValuePair<string, DSNode>(child.Key, new DSNode(child, StrategyType));
            }
        }

        public IEnumerator<KeyValuePair<string, DSNode>> GetEnumerator()
        {
            foreach (var child in _node.GetChildren())
            {
                yield return new KeyValuePair<string, DSNode>(child.Key, new DSNode(child, StrategyType));
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

        #endregion

        public readonly StategyType StrategyType;

        public void GetNodeType()
        {
            throw new NotImplementedException();
        }

        public string GetNodeValue()
        {
            throw new NotImplementedException();
        }

        public void SetNodeValue(string value)
        {
            throw new NotImplementedException();
        }
    }
}
