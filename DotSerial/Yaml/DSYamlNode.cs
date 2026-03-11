using DotSerial.Common;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Tree.Serialize;
using DotSerial.Yaml.Parser;
using DotSerial.Yaml.Writer;

namespace DotSerial.Yaml
{
    /// <summary>
    /// Yaml Node
    /// </summary>
    public class DSYamlNode : IDSSerialNode<DSYamlNode>
    {
        /// <summary>Internal node </summary>
        private readonly IDSNode _node;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">Node</param>
        internal DSYamlNode(IDSNode node)
        {
            _node = node;
        }

        /// <inheritdoc/>
        public bool HasChildren => _node.HasChildren();

        /// <inheritdoc/>
        public string Key => _node.Key;

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSYamlException">DotSerial Exception.</exception>
        public static DSYamlNode FromString(string str)
        {
            try
            {
                if (null == str)
                {
                    throw new DSYamlException($"{str} can't be null.");
                }

                var root = YamlParserVisitor.Parse(str);

                return root;
            }
            catch (DotSerialException ex)
            {
                throw new DSYamlException(ex.Message);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSYamlException">DotSerial Exception.</exception>
        public static DSYamlNode ToNode(object? obj, string? key = null)
        {
            try
            {
                // Determine key
                string currKey = key ?? CommonConstants.MainObjectKey;

                // Serialize object
                var rootNode = SerializeObject.Serialize(obj, currKey, StategyType.Yaml);

                return new DSYamlNode(rootNode);
            }
            catch (DotSerialException ex)
            {
                throw new DSYamlException(ex.Message);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSYamlException">DotSerial Exception.</exception>
        public static U ToObject<U>(string str)
        {
            try
            {
                if (null == str)
                {
                    throw new DSYamlException($"{str} can't be null.");
                }

                // Parse yaml string to node
                DSYamlNode dsNode = FromString(str);

                return IDSSerialNode<U>.ToObject<U>(dsNode.GetInternalData());
            }
            catch (DotSerialException ex)
            {
                throw new DSYamlException(ex.Message);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        public DSYamlNode? GetChild(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            var child = _node.GetChild(key);
            return new DSYamlNode(child);
        }

        /// <inheritdoc/>
        public List<DSYamlNode>? GetChildren()
        {
            if (false == HasChildren)
                return null;

            var children = new List<DSYamlNode>();
            var tmp = _node.GetChildren();
            foreach (var c in tmp)
            {
                children.Add(new DSYamlNode(c));
            }

            return children;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSYamlException">DotSerial Exception.</exception>
        public string Stringify()
        {
            try
            {
                if (null == _node)
                {
                    throw new DSYamlException($"{_node} can't be null.");
                }

                // Convert
                var yamlString = YamlWriterVisitor.Write(this);

                return new string(yamlString); // TODO
            }
            catch (DotSerialException ex)
            {
                throw new DSYamlException(ex.Message);
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
