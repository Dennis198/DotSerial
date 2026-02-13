using DotSerial.Common;
using DotSerial.Toon.Parser;
using DotSerial.Toon.Writer;
using DotSerial.Tree.Nodes;
using DotSerial.Tree.Serialize;

namespace DotSerial.Toon
{
    /// <summary>
    /// Toon node
    /// </summary>
    public class DSToonNode : IDSSerialNode<DSToonNode>
    {
        /// <summary>Internal node </summary>
        private readonly IDSNode _node;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">Node</param>
        internal DSToonNode(IDSNode node)
        {
            _node = node;
        }

        /// <inheritdoc/>
        public string Key => _node.Key;

        /// <inheritdoc/>
        public bool HasChildren => _node.HasChildren();

        /// <summary>
        /// Returns the internal node
        /// </summary>
        /// <returns>IDSNode</returns>
        internal IDSNode GetInternalData()
        {
            return _node;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        public DSToonNode? GetChild(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            var child = _node.GetChild(key);
            return new DSToonNode(child);
        }

        /// <inheritdoc/>
        public List<DSToonNode>? GetChildren()
        {
            if (false == HasChildren)
                return null;

            var children = new List<DSToonNode>();
            var tmp = _node.GetChildren();
            foreach (var c in tmp)
            {
                children.Add(new DSToonNode(c));
            }

            return children;
        }  

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSToonException">DotSerial Exception.</exception>
        public static DSToonNode ToNode(object? obj, string? key = null)
        {
            try
            {
                // Determine key
                string currKey = key ?? CommonConstants.MainObjectKey;
                
                // Serialize object
                var rootNode = SerializeObject.Serialize(obj, currKey);

                return new DSToonNode(rootNode);
            }
            catch(DotSerialException ex)
            {
                throw new DSToonException(ex.Message);
            }
            catch
            {
                throw;
            }
        }    

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSToonException">DotSerial Exception.</exception>
        public string Stringify()
        {
            if (null == _node)
            {
                throw new DSToonException($"{_node} can't be null.");
            }

            try
            {
                // Convert
                string str = ToonWriterVisitor.Write(this);

                return str;
            }
            catch(DotSerialException ex)
            {
                throw new DSToonException(ex.Message);
            }
            catch
            {
                throw;
            }            
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSToonException">DotSerial Exception.</exception>
        public static DSToonNode FromString(string str)
        {
            try
            {
                // if (string.IsNullOrWhiteSpace(str))
                // {
                //     throw new DSToonException($"{str} can't be null or whitespace.");
                // }   

                var root = ToonParserVisitor.Parse(str);        

                return root;
            }
            catch(DotSerialException ex)
            {
                throw new DSToonException(ex.Message);
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
                // if (string.IsNullOrWhiteSpace(str))
                // {
                //     throw new DSToonException($"{str} can't be null or whitespace.");
                // }

                // Parse json string to node
                DSToonNode dsNode = FromString(str);

                return IDSSerialNode<U>.ToObject<U>(dsNode.GetInternalData());
            }
            catch(DotSerialException ex)
            {
                throw new DSToonException(ex.Message);
            }
            catch
            {
                throw;
            }              
        }                     
    }
}