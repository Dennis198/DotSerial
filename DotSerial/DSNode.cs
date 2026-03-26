using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Net;
using DotSerial.Common;
using DotSerial.Common.Parser;
using DotSerial.Common.Writer;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Tree.Serialize;
using DotSerial.Utilities;

namespace DotSerial
{
    /// <summary>
    /// Represents a node in the DotSerial document object model, providing a dictionary-based interface
    /// for navigating and manipulating a serialized data tree.
    /// </summary>
    public class DSNode : IDictionary<string, DSNode>
    {
        /// <summary>
        /// The serialization strategy (e.g. JSON, XML, YAML) associated with this node.
        /// </summary>
        public readonly SerializeStrategy Strategy;

        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <summary>Parser factory instance</summary>
        private static readonly ParserFactory _parserFactory = ParserFactory.Instance;

        /// <summary>Writer factory instance</summary>
        private static readonly WriterFactory _writerFactory = WriterFactory.Instance;

        /// <summary>Internal node </summary>
        private IDSNode _node;

        /// <summary>
        /// Initializes a new <see cref="DSNode"/> wrapping the given internal node.
        /// </summary>
        /// <param name="node">The internal node to wrap.</param>
        /// <param name="strategy">The serialization strategy associated with this node.</param>
        internal DSNode(IDSNode node, SerializeStrategy strategy)
        {
            _node = node;
            Strategy = strategy;
        }

        /// <summary>Gets the number of child nodes.</summary>
        public int Count => _node.Count;

        /// <summary>Gets a value indicating whether this node is a leaf node with a quoted value.</summary>
        public bool IsQuoted => _node.IsQuoted;

        /// <summary>Gets a value indicating whether the node is read-only.</summary>
        public bool IsReadOnly => false;

        /// <summary>Gets the key associated with this node.</summary>
        public string Key => _node.Key;

        /// <summary>Gets a collection containing the keys of all child nodes.</summary>
        public ICollection<string> Keys => _node.Keys;

        /// <summary>
        /// Gets the parent node of this node, or <c>null</c> if this node is the root.
        /// </summary>
        /// <returns>The parent <see cref="DSNode"/> or <c>null</c>.</returns>
        public DSNode? Parent => _node.Parent is not null ? new DSNode(_node.Parent, Strategy) : null;

        /// <summary>Gets a collection containing all child nodes.</summary>
        public ICollection<DSNode> Values => [.. _node.Values.Select(child => new DSNode(child, Strategy))];

        /// <summary>
        /// Gets or sets the child node with the specified key.
        /// </summary>
        /// <param name="key">The key of the child node.</param>
        /// <exception cref="DotSerialException">Thrown on set when <paramref name="key"/> belongs to a node with a different strategy.</exception>
        public DSNode this[string key]
        {
            get { return new DSNode(_node.GetChild(key), Strategy); }
            set
            {
                ArgumentNullException.ThrowIfNull(key);

                if (Strategy != value.Strategy)
                {
                    ThrowHelper.ThrowDifferentStrategyException();
                }

                var node = value.GetInternalData();
                // Set key for node
                node.Key = key;
                _node.Remove(key);

                _node.AddChild(node);
            }
        }

        /// <summary>
        /// Parses a serialized string and returns the root <see cref="DSNode"/> of the resulting tree.
        /// </summary>
        /// <param name="str">The serialized string to parse.</param>
        /// <param name="strategy">The serialization strategy used to interpret <paramref name="str"/>.</param>
        /// <returns>The root <see cref="DSNode"/> of the parsed tree.</returns>
        public static DSNode FromString(ReadOnlySpan<char> str, SerializeStrategy strategy)
        {
            var root = _parserFactory.Parse(strategy, str);
            return root;
        }

        /// <summary>
        /// Serializes an object into its string representation using the specified strategy.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="strategy">The serialization strategy used to format the output.</param>
        /// <returns>A string containing the serialized representation of <paramref name="obj"/>.</returns>
        public static string Stringify(object? obj, SerializeStrategy strategy)
        {
            string currKey = CommonConstants.MainObjectKey;
            var rootNode = SerializeObject.Serialize(obj, currKey, strategy);
            var jsonString = _writerFactory.Write(strategy, new DSNode(rootNode, strategy));

            return new string(jsonString);
        }

        /// <summary>
        /// Converts an object into a <see cref="DSNode"/> tree using the specified strategy.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <param name="strategy">The serialization strategy used to build the node tree.</param>
        /// <param name="key">An optional key for the root node. Defaults to the library's main object key when <see langword="null"/>.</param>
        /// <returns>A <see cref="DSNode"/> representing the serialized object.</returns>
        public static DSNode ToNode(object? obj, SerializeStrategy strategy, string? key = null)
        {
            string currKey = key ?? CommonConstants.MainObjectKey;
            var rootNode = SerializeObject.Serialize(obj, currKey, strategy);

            return new DSNode(rootNode, strategy);
        }

        /// <summary>
        /// Parses a serialized string and deserializes it into an object of type <typeparamref name="U"/>.
        /// </summary>
        /// <typeparam name="U">The target type to deserialize into.</typeparam>
        /// <param name="str">The serialized string to parse.</param>
        /// <param name="strategy">The serialization strategy used to interpret <paramref name="str"/>.</param>
        /// <returns>An instance of <typeparamref name="U"/> populated with the deserialized data.</returns>
        public static U ToObject<U>(ReadOnlySpan<char> str, SerializeStrategy strategy)
        {
            DSNode dsNode = FromString(str, strategy);

            return IDSNode.ToObject<U>(dsNode.GetInternalData());
        }

        /// <summary>
        /// Adds a child node with the specified key.
        /// </summary>
        /// <param name="key">The key of the child node to add.</param>
        /// <param name="value">The <see cref="DSNode"/> to add.</param>
        /// <exception cref="DotSerialException">Thrown when <paramref name="value"/> uses a different strategy than this node.</exception>
        public void Add(string key, DSNode value)
        {
            ArgumentNullException.ThrowIfNull(key);

            if (Strategy != value.Strategy)
            {
                ThrowHelper.ThrowDifferentStrategyException();
            }

            var node = value.GetInternalData();
            node.Key = key;

            _node.AddChild(node);
        }

        /// <summary>
        /// Adds a child node represented by a key/value pair.
        /// </summary>
        /// <param name="item">The key/value pair containing the key and the <see cref="DSNode"/> to add.</param>
        public void Add(KeyValuePair<string, DSNode> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Removes all child nodes from this node.
        /// </summary>
        public void Clear()
        {
            _node.Clear();
        }

        /// <summary>
        /// Determines whether the node contains a child with the key of the specified pair.
        /// </summary>
        /// <param name="item">The key/value pair whose key to locate.</param>
        /// <returns><see langword="true"/> if a child with the given key exists; otherwise <see langword="false"/>.</returns>
        public bool Contains(KeyValuePair<string, DSNode> item)
        {
            return ContainsKey(item.Key);
        }

        /// <summary>
        /// Determines whether the node contains a child with the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns><see langword="true"/> if a child with the given key exists; otherwise <see langword="false"/>.</returns>
        public bool ContainsKey(string key)
        {
            return _node.ContainsKey(key);
        }

        /// <summary>
        /// Copies all child nodes as key/value pairs into the specified array, starting at <paramref name="arrayIndex"/>.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="arrayIndex"/> is out of range.</exception>
        /// <exception cref="ArgumentException">Thrown when the array does not have enough space.</exception>
        public void CopyTo(KeyValuePair<string, DSNode>[] array, int arrayIndex)
        {
            ArgumentNullException.ThrowIfNull(array);

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

        /// <summary>
        /// Returns an enumerator that iterates through the child nodes as key/value pairs.
        /// </summary>
        /// <returns>An enumerator for the child nodes.</returns>
        public IEnumerator<KeyValuePair<string, DSNode>> GetEnumerator()
        {
            foreach (var child in _node.GetChildren())
            {
                yield return new KeyValuePair<string, DSNode>(child.Key, new DSNode(child, Strategy));
            }
        }

        /// <summary>
        /// Returns the <see cref="NodeType"/> of this node (value, object, or array).
        /// </summary>
        /// <returns>The <see cref="NodeType"/> of this node.</returns>
        /// <exception cref="DotSerialException">Thrown when the internal node type is unrecognized.</exception>
        public NodeType GetNodeType()
        {
            if (_node is LeafNode)
            {
                return NodeType.Value;
            }
            if (_node is InnerNode || _node is DictionaryNode)
            {
                return NodeType.Object;
            }
            if (_node is ListNode)
            {
                return NodeType.Array;
            }

            ThrowHelper.ThrowUnknownNodeTypeException();
            throw new Exception("Unreachable code.");
        }

        /// <summary>
        /// Returns the raw string value of this leaf node, or <see langword="null"/> if the node has no value.
        /// </summary>
        /// <returns>The raw string value, or <see langword="null"/>.</returns>
        public string? GetNodeValue()
        {
            return _node.GetValue();
        }

        /// <summary>
        /// Removes the child node with the specified key.
        /// </summary>
        /// <param name="key">The key of the child node to remove.</param>
        /// <returns><see langword="true"/> if the child was found and removed; otherwise <see langword="false"/>.</returns>
        public bool Remove(string key)
        {
            return _node.Remove(key);
        }

        /// <summary>
        /// Removes the child node identified by the key of the specified pair.
        /// </summary>
        /// <param name="item">The key/value pair whose key identifies the child to remove.</param>
        /// <returns><see langword="true"/> if the child was found and removed; otherwise <see langword="false"/>.</returns>
        public bool Remove(KeyValuePair<string, DSNode> item)
        {
            return Remove(item.Key);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="string"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(string? value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="bool"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(bool value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="byte"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(byte value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="sbyte"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(sbyte value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="char"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(char value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="decimal"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(decimal value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="double"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(double value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="float"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(float value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="int"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(int value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="uint"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(uint value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="nint"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(nint value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="nuint"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(nuint value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="long"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(long value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="ulong"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(ulong value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="short"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(short value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="ushort"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(ushort value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="DateTime"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(DateTime value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="Guid"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(Guid value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="TimeSpan"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(TimeSpan value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="Uri"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(Uri value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="IPAddress"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(IPAddress value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="Version"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(Version value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to the specified <see cref="CultureInfo"/>.</summary>
        /// <param name="value">The value to set.</param>
        public void SetNodeValue(CultureInfo value)
        {
            SetNodeValuePrivate(value);
        }

        /// <summary>Sets the leaf value of this node to <see langword="null"/>.</summary>
        public void SetNodeValueNull()
        {
            SetNodeValuePrivate(null);
        }

        /// <summary>
        /// Serializes this node and its children into a string using the node's associated strategy.
        /// </summary>
        /// <returns>A string containing the serialized representation of this node.</returns>
        /// <exception cref="DotSerialException">Thrown when the internal node is <see langword="null"/>.</exception>
        public string Stringify()
        {
            ArgumentNullException.ThrowIfNull(_node);

            var jsonString = _writerFactory.Write(Strategy, this);

            return new string(jsonString);
        }

        /// <summary>
        /// Attempts to add a child node represented by the specified key/value pair.
        /// </summary>
        /// <param name="item">The key/value pair to add.</param>
        /// <returns><see langword="true"/> if the node was added successfully; otherwise <see langword="false"/>.</returns>
        public bool TryAdd(KeyValuePair<string, DSNode> item)
        {
            return TryAdd(item.Key, item.Value);
        }

        /// <summary>
        /// Attempts to add a child node with the specified key.
        /// Returns <see langword="false"/> if a child with the same key already exists or the strategies differ.
        /// </summary>
        /// <param name="key">The key of the child node to add.</param>
        /// <param name="value">The <see cref="DSNode"/> to add.</param>
        /// <returns><see langword="true"/> if the node was added successfully; otherwise <see langword="false"/>.</returns>
        public bool TryAdd(string key, DSNode value)
        {
            ArgumentNullException.ThrowIfNull(key);

            try
            {
                if (Strategy != value.Strategy)
                {
                    return false;
                }

                if (ContainsKey(key))
                {
                    return false;
                }

                var node = value.GetInternalData();
                node.Key = key;

                _node.AddChild(node);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to get the raw string value of this leaf node.
        /// </summary>
        /// <param name="value">When this method returns, contains the raw string value if the node has a value; otherwise, <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the node has a value; otherwise, <see langword="false"/>.</returns>
        public bool TryGetNodeValue([MaybeNullWhen(false)] out string value)
        {
            try
            {
                value = GetNodeValue();

                if (null == value)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// Gets the child node associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the child node to retrieve.</param>
        /// <param name="value">When this method returns, contains the child node if found; otherwise <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if a child node with the given key was found; otherwise <see langword="false"/>.</returns>
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

        /// <summary>
        /// Returns the internal node
        /// </summary>
        /// <returns>IDSNode</returns>
        internal IDSNode GetInternalData()
        {
            return _node;
        }

        /// <summary>
        /// Sets the value of this node.
        /// </summary>
        /// <param name="value">The value to set.</param>
        private void SetNodeValuePrivate(object? value)
        {
            _node = _nodeFactory.CreateNode(Strategy, _node.Key, value, TreeNodeType.Leaf, _node.Parent);

            if (null != _node.Parent)
            {
                _node.Parent.Remove(_node.Key);
                _node.Parent.AddChild(_node);
            }
        }
    }
}
