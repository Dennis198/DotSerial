using DotSerial.Common;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Tree.Serialize;
using DotSerial.Xml.Parser;
using DotSerial.Xml.Writer;

namespace DotSerial.Xml
{
    /// <summary>
    /// Xml Node
    /// </summary>
    public class DSXmlNode : IDSSerialNode<DSXmlNode>
    {
        /// <summary>Internal node </summary>
        private readonly IDSNode _node;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">Node</param>
        internal DSXmlNode(IDSNode node)
        {
            _node = node;
        }

        /// <inheritdoc/>
        public bool HasChildren => _node.HasChildren();

        /// <inheritdoc/>
        public string Key => _node.Key;

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSXmlException">DotSerial Exception.</exception>
        public static DSXmlNode FromString(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    throw new DSXmlException($"{str} can't be null or whitespace.");
                }

                var root = XmlParserVisitor.Parse(str);

                return root;
            }
            catch (DotSerialException ex)
            {
                throw new DSXmlException(ex.Message);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSXmlException">DotSerial Exception.</exception>
        public static DSXmlNode ToNode(object? obj, string? key = null)
        {
            try
            {
                // Determine key
                string currKey = key ?? CommonConstants.MainObjectKey;

                // Serialize object
                var rootNode = SerializeObject.Serialize(obj, currKey, StategyType.Xml);

                return new DSXmlNode(rootNode);
            }
            catch (DotSerialException ex)
            {
                throw new DSXmlException(ex.Message);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSXmlException">DotSerial Exception.</exception>
        public static U ToObject<U>(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    throw new DSXmlException($"{str} can't be null or whitespace.");
                }

                // Parse xml string to node
                DSXmlNode dsNode = FromString(str);

                return IDSSerialNode<U>.ToObject<U>(dsNode.GetInternalData());
            }
            catch (DotSerialException ex)
            {
                throw new DSXmlException(ex.Message);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the child with the given key, otherwise null.
        /// </summary>
        /// <param name="key">Key of child</param>
        /// <returns>Child node</returns>
        public DSXmlNode? GetChild(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            var child = _node.GetChild(key);
            return new DSXmlNode(child);
        }

        /// <inheritdoc/>
        public List<DSXmlNode>? GetChildren()
        {
            if (false == HasChildren)
                return null;

            var children = new List<DSXmlNode>();
            var tmp = _node.GetChildren();
            foreach (var c in tmp)
            {
                children.Add(new DSXmlNode(c));
            }

            return children;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSXmlException">DotSerial Exception.</exception>
        public string Stringify()
        {
            if (null == _node)
            {
                throw new DSXmlException($"{_node} can't be null.");
            }

            try
            {
                // Convert
                var xmlString = XmlWriterVisitor.Write(this);

                return new string(xmlString);
            }
            catch (DotSerialException ex)
            {
                throw new DSXmlException(ex.Message);
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
