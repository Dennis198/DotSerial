using DotSerial.Common;
using DotSerial.Json.Parser;
using DotSerial.Json.Writer;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Tree.Serialize;

namespace DotSerial.Json
{
    /// <summary>
    /// Json Node
    /// </summary>
    public class DSJsonNode : IDSSerialNode<DSJsonNode>
    {
        /// <summary>Internal node </summary>
        private readonly IDSNode _node;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">Node</param>
        internal DSJsonNode(IDSNode node)
        {
            _node = node;
        }

        /// <inheritdoc/>
        public bool HasChildren => _node.HasChildren();

        /// <inheritdoc/>
        public string Key => _node.Key;

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSJsonException">DotSerial Exception.</exception>
        public static DSJsonNode FromString(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    throw new DSJsonException($"{str} can't be null or whitespace.");
                }

                var root = JsonParserVisitor.Parse(str);

                return root;
            }
            catch (DotSerialException ex)
            {
                throw new DSJsonException(ex.Message);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSJsonException">DotSerial Exception.</exception>
        public static DSJsonNode ToNode(object? obj, string? key = null)
        {
            try
            {
                // Determine key
                string currKey = key ?? CommonConstants.MainObjectKey;

                // Serialize object
                var rootNode = SerializeObject.Serialize(obj, currKey, StategyType.Json);

                return new DSJsonNode(rootNode);
            }
            catch (DotSerialException ex)
            {
                throw new DSJsonException(ex.Message);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSJsonException">DotSerial Exception.</exception>
        public static U ToObject<U>(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    throw new DSJsonException($"{str} can't be null or whitespace.");
                }

                // Parse json string to node
                DSJsonNode dsNode = FromString(str);

                return IDSSerialNode<U>.ToObject<U>(dsNode.GetInternalData());
            }
            catch (DotSerialException ex)
            {
                throw new DSJsonException(ex.Message);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        public DSJsonNode? GetChild(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            var child = _node.GetChild(key);
            return new DSJsonNode(child);
        }

        /// <inheritdoc/>
        public List<DSJsonNode>? GetChildren()
        {
            if (false == HasChildren)
                return null;

            var children = new List<DSJsonNode>();
            var tmp = _node.GetChildren();
            foreach (var c in tmp)
            {
                children.Add(new DSJsonNode(c));
            }

            return children;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSJsonException">DotSerial Exception.</exception>
        public string Stringify()
        {
            if (null == _node)
            {
                throw new DSJsonException($"{_node} can't be null.");
            }

            try
            {
                // Convert
                var jsonString = JsonWriterVisitor.Write(this);

                return new string(jsonString);
            }
            catch (DotSerialException ex)
            {
                throw new DSJsonException(ex.Message);
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
    }
}
