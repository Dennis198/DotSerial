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
        private static readonly WriterFactory _writerFactory = WriterFactory.Instance;
        private static readonly ParserFactory _parserFactory = ParserFactory.Instance;

        /// <summary>Internal node </summary>
        private readonly IDSNode _node;

        /// <summary>
        /// Key of the node.
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
        internal DSNode(IDSNode node)
        {
            _node = node;
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

                return new DSNode(rootNode);
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
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public ICollection<string> Keys => throw new NotImplementedException();

        public ICollection<DSNode> Values => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(string key, DSNode value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<string, DSNode> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, DSNode> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, DSNode>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, DSNode>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, DSNode> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out DSNode value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
