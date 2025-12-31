using DotSerial.Core.General;
using DotSerial.Core.Tree;
using DotSerial.Core.Tree.Deserialize;
using DotSerial.Core.Tree.Nodes;
using DotSerial.Core.Tree.Serialize;
using DotSerial.Core.YAML.Parser;
using DotSerial.Core.YAML.Writer;

namespace DotSerial.Core.YAML
{
    /// <summary>
    /// YAML Node
    /// </summary>
    public class DSYamlNode
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

        /// <summary>
        /// Key of the node
        /// </summary>
        public string Key => _node.Key;

        /// <summary>
        /// True, if node contains children
        /// </summary>
        public bool HasChildren => _node.HasChildren();

        /// <summary>
        /// Returns the internal node
        /// </summary>
        /// <returns>IDSNode</returns>
        internal IDSNode GetInternalData()
        {
            return _node;
        }        

        /// <summary>
        /// Returns the child with the given key, otherwise null.
        /// </summary>
        /// <param name="key">Key of child</param>
        /// <returns>Child node</returns>
        public DSYamlNode? GetChild(string key)
        {
            var child = _node.GetChild(key);
            return new DSYamlNode(child);
        }

        /// <summary>
        /// Creates the node structure for a given obj
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="key">Key of the object</param>
        /// <returns>DSJsonNode</returns>
        public static DSYamlNode ToNode(object? obj, string? key = null)
        {
            // Determine key
            string currKey = key ?? GeneralConstants.MainObjectKey;
            
            // Serialize object
            var rootNode = SerializeObject.Serialize(obj, currKey);

            return new DSYamlNode(rootNode);
        }       

        /// <summary>
        /// Converts the node to a JSON string.
        /// </summary>
        /// <returns>Yaml string</returns>
        public string ToYamlString()
        {            
            if (null == _node)
            {
                throw new NotImplementedException();
            }

            // Convert
            string yamlString = YamlWriterVisitor.Write(this);

            return yamlString;
        }         

        /// <summary>
        /// Parses a json string to a DSJsonNode
        /// </summary>
        /// <param name="jsonString">Json string</param>
        /// <returns>DSJsonNode</returns>
        public static DSYamlNode FromYamlString(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new NotImplementedException();
            }   

            var root = YamlParserVisitor.Parse(jsonString);        

            return root;
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
            DSYamlNode dsNode = FromYamlString(jsonString);

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