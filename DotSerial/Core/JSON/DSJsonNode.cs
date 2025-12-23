using DotSerial.Core.General;
using DotSerial.Core.JSON.Parser;
using DotSerial.Core.JSON.Writer;
using DotSerial.Core.Tree;

namespace DotSerial.Core.JSON
{
    /// <summary>
    /// JSON Node
    /// </summary>
    public class DSJsonNode
    {
        /// <summary>Wrapped node </summary>
        private readonly IDSNode _node;

        /// <summary> Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">Node</param>
        internal DSJsonNode(IDSNode node)
        {
            _node = node;
        }

        /// <summary>
        /// Key of the node
        /// </summary>
        public string Key => _node.Key;

        /// <summary>
        /// True, if node contains children
        /// </summary>
        public bool HasChildren => _node.HasChildren();

        /// <summary>
        /// Returns the child with the given key, otherwise null.
        /// </summary>
        /// <param name="key">Key of child</param>
        /// <returns>Child node</returns>
        public DSJsonNode? GetChild(string key)
        {
            var child = _node.GetChild(key);
            return new DSJsonNode(child);
        }

        /// <summary>
        /// Returns all children of the node.
        /// </summary>
        /// <returns>All chidlren</returns>
        public List<DSJsonNode>? GetChildren()
        {
            return [];
        }

        /// <summary>
        /// Adds a child node.
        /// </summary>
        /// <param name="node">Child node</param>
        public void AddChild(DSJsonNode? node)
        {
            // TODO NUR Leafs erlauben
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the node structure for a given obj
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="key">Key of the object</param>
        /// <returns>DSJsonNode</returns>
        public static DSJsonNode ToNode(object? obj, string? key = null)
        {
            // Determine key
            string currKey = key ?? GeneralConstants.MainObjectKey;
            
            // Serialize object
            var rootNode = DSSerialize.Serialize2(obj, currKey);

            return new DSJsonNode(rootNode);
        }

        /// <summary>
        /// Converts the node to a JSON string.
        /// </summary>
        /// <returns>Json string</returns>
        public string ToJsonString()
        {
            if (null == _node)
            {
                throw new NotImplementedException();
            }

            // Convert
            string jsonString = JSONWriterVisitor.Write(_node);

            return jsonString;
        }

        /// <summary>
        /// Parses a json string to a DSJsonNode
        /// </summary>
        /// <param name="jsonString">Json string</param>
        /// <returns>DSJsonNode</returns>
        public static DSJsonNode FromJsonString(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new NotImplementedException();
            }   

            var root = JSONParserVisitor.Parse(jsonString);        

            return new DSJsonNode(root);
        }

        /// <summary>
        /// Deserializes a json string to an object of type U
        /// </summary>
        /// <param name="jsonString">Json string</param>
        /// <typeparam name="U">Type fo the object</typeparam>
        /// <returns>Deserilized object</returns>
        public static U ToObject<U>(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new NotImplementedException();
            }

            // Parse json string to node
            DSJsonNode dsNode = FromJsonString(jsonString);

            return dsNode.ToObject<U>();
        }

        /// <summary>
        /// Deserializes the node to an object of type U
        /// </summary>
        /// <typeparam name="U">Type fo the object</typeparam>
        /// <returns>Deserilized object</returns>
        public U ToObject<U>()
        {
            if (null == _node)
            {
                throw new NotImplementedException();
            }

            // Deserilize object
            var resultObj = _node.DeserializeAccept(new DeserializeObject(), typeof(U));

            if (null == resultObj)
            {
                throw new NotImplementedException();
            }
            
            // cast object to U
            if (resultObj is U result)
            {
                return result;
            }
            else
            {
                throw new NotImplementedException();
            }    
        }
    }
}