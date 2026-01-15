using System.Xml;
using System.Xml.Linq;
using DotSerial.Common;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.XML
{
    /// <summary>
    /// XML Node
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

        /// <summary>
        /// Returns the child with the given key, otherwise null.
        /// </summary>
        /// <param name="key">Key of child</param>
        /// <returns>Child node</returns>
        public DSXmlNode? GetChild(string key)
        {
            throw new NotImplementedException();
            // var child = _node.GetChild(key);
            // return new DSJsonNode(child);
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
        public static DSXmlNode ToNode(object? obj, string? key = null)
        {
            throw new NotImplementedException();
            // try
            // {
            //     // Determine key
            //     string currKey = key ?? CommonConstants.MainObjectKey;
                
            //     XmlDocument xmlDoc = new();
            //     XmlNode rootNode = xmlDoc.CreateElement(currKey);

            //     // Serialze Object
            //     DotSerialXMLSerialize.Serialize(obj, xmlDoc, rootNode, currKey);

            //     return new DSXmlNode(rootNode);
            // }
            // catch (DotSerialException ex)
            // {
            //     throw new DSXmlException(ex.Message);
            // }
            // catch
            // {
            //     throw;
            // }            
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSXmlException">DotSerial Exception.</exception>
        public string Stringify()
        {
            throw new NotImplementedException();
            // try
            // {
            //     if (null == _node)
            //     {
            //         throw new DSXmlException($"{_node} can't be null.");
            //     }

            //     XmlDocument xmlDoc = new ();
            //     xmlDoc.AppendChild(_node);

            //     XDocument doc = XDocument.Parse(xmlDoc.OuterXml);

            //     // Add decleration
            //     string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            //     // Xml content
            //     string xmlString = doc.ToString();

            //     // Add new line so declaration is seperated in one line
            //     result += Environment.NewLine;

            //     // Add xml content to result
            //     result += xmlString;

            //     return result;
            // }
            // catch (DotSerialException ex)
            // {
            //     throw new DSXmlException(ex.Message);
            // }
            // catch
            // {
            //     throw;
            // }              
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSXmlException">DotSerial Exception.</exception>
        public static DSXmlNode FromString(string str)
        {
            throw new NotImplementedException();
            // try
            // {
            //     if (string.IsNullOrWhiteSpace(str))
            //     {
            //         throw new DSXmlException($"{str} can't be null or whitespace.");
            //     }   

            //     XmlDocument xmlDoc = new();

            //     xmlDoc.LoadXml(str);
            //     XmlNodeList s = xmlDoc.GetElementsByTagName(XmlConstants.DotSerial) ?? throw new NullReferenceException();
            //     XmlNode xnodeParameters = s.Item(0) ?? throw new NullReferenceException();
            //     XmlNode rootNode = xnodeParameters.ChildNodes[0] ?? throw new NullReferenceException();

            //     var root = new DSXmlNode(rootNode);       

            //     return root;
            // }
            // catch (DotSerialException ex)
            // {
            //     throw new DSXmlException(ex.Message);
            // }
            // catch
            // {
            //     throw;
            // }             
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSXmlException">DotSerial Exception.</exception>
        public static U ToObject<U>(string xmlString)
        {
            throw new NotImplementedException();
            // try
            // {
            //     if (string.IsNullOrWhiteSpace(xmlString))
            //     {
            //         throw new NotImplementedException();
            //     }

            //     // Parse json string to node
            //     DSXmlNode dsNode = FromString(xmlString);
            //     var tmp = dsNode.GetInternalData();
            //     var result = CreateInstanceMethods.CreateInstanceGeneric<U>();
            //     DotSerialXMLDeserialize.Deserialize(result, tmp);

            //     return result;
            // }
            // catch (DotSerialException ex)
            // {
            //     throw new DSXmlException(ex.Message);
            // }
            // catch
            // {
            //     throw;
            // }                
        }        
    }
}