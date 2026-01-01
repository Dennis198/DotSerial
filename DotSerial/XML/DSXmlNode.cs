// using System.Xml;
// using System.Xml.Linq;
// using DotSerial.Common;

// namespace DotSerial.Core.XML
// {
//     public class DSXmlNode
//     {
//         /// <summary>Internal node </summary>
//         private readonly XmlNode _node;

//         /// <summary>
//         /// Constructor
//         /// </summary>
//         /// <param name="node">Node</param>
//         internal DSXmlNode(XmlNode node)
//         {
//             _node = node;
//         }

//         /// <summary>
//         /// Key of the node
//         /// </summary>
//         public string Key => GetKey();

//         /// <summary>
//         /// True, if node contains children
//         /// </summary>
//         public bool HasChildren => _node.ChildNodes.Count > 0;

//         private string GetKey()
//         {
//             if (null == _node || null == _node.Attributes || null == _node.Attributes[Constants.XmlAttributeID])
//             {
//                 throw new NotImplementedException();
//             }

//             return _node.Attributes[Constants.XmlAttributeID]!.Value;
//         }

//         /// <summary>
//         /// Returns the internal node
//         /// </summary>
//         /// <returns>IDSNode</returns>
//         internal XmlNode GetInternalData()
//         {
//             return _node;
//         }

//         /// <summary>
//         /// Returns the child with the given key, otherwise null.
//         /// </summary>
//         /// <param name="key">Key of child</param>
//         /// <returns>Child node</returns>
//         public DSXmlNode? GetChild(string key)
//         {
//             throw new NotImplementedException();
//             // var child = _node.GetChild(key);
//             // return new DSJsonNode(child);
//         }

//         /// <summary>
//         /// Returns all children of the node.
//         /// </summary>
//         /// <returns>All chidlren</returns>
//         public List<DSXmlNode>? GetChildren()
//         {
//             if (false == HasChildren)
//                 return null;

//             var children = new List<DSXmlNode>();
//             var tmp = _node.ChildNodes;
//             foreach (XmlNode c in tmp)
//             {
//                 children.Add(new DSXmlNode(c));
//             }

//             return children;
//         }

//         /// <summary>
//         /// Creates the node structure for a given obj
//         /// </summary>
//         /// <param name="obj">Object</param>
//         /// <param name="key">Key of the object</param>
//         /// <returns>DSJsonNode</returns>
//         public static DSXmlNode ToNode(object? obj, string? key = null)
//         {
//             // Determine key
//             string currKey = key ?? GeneralConstants.MainObjectKey;
            
//             XmlDocument xmlDoc = new();
//             XmlNode rootNode = xmlDoc.CreateElement(Constants.Object);

//             // Serialze Object
//             DotSerialXMLSerialize.Serialize(obj, xmlDoc, rootNode, Constants.MainObjectID);

//             return new DSXmlNode(rootNode);
//         }

//         /// <summary>
//         /// Converts the node to a JSON string.
//         /// </summary>
//         /// <returns>Json string</returns>
//         public string ToXmlString()
//         {
//             if (null == _node)
//             {
//                 throw new NotImplementedException();
//             }

//             // Convert
//             try
//             {
//                 XmlDocument xmlDoc = new ();
//                 xmlDoc.AppendChild(_node);

//                 XDocument doc = XDocument.Parse(xmlDoc.OuterXml);

//                 // Add decleration
//                 string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

//                 // Xml content
//                 string xmlString = doc.ToString();

//                 // Add new line so declaration is seperated in one line
//                 result += Environment.NewLine;

//                 // Add xml content to result
//                 result += xmlString;

//                 return result;
//             }
//             catch (Exception)
//             {
//                 throw;
//             }
//         }

//         /// <summary>
//         /// Parses a json string to a DSJsonNode
//         /// </summary>
//         /// <param name="xmlString">Json string</param>
//         /// <returns>DSJsonNode</returns>
//         public static DSXmlNode FromXmlString(string xmlString)
//         {
//             if (string.IsNullOrWhiteSpace(xmlString))
//             {
//                 throw new NotImplementedException();
//             }   

            
//             var root = JSONParserVisitor.Parse(xmlString);        

//             return root;
//         }

//         /// <summary>
//         /// Deserializes a json string to an object of type U
//         /// </summary>
//         /// <param name="xmlString">Json string</param>
//         /// <typeparam name="U">Type fo the object</typeparam>
//         /// <returns>Deserilized object</returns>
//         public static U ToObject<U>(string xmlString)
//         {
//             if (string.IsNullOrWhiteSpace(xmlString))
//             {
//                 throw new NotImplementedException();
//             }

//             // Parse json string to node
//             DSXmlNode dsNode = FromXmlString(xmlString);

//             return dsNode.ToObject<U>();
//         }

//         /// <summary>
//         /// Deserializes the node to an object of type U
//         /// </summary>
//         /// <typeparam name="U">Type fo the object</typeparam>
//         /// <returns>Deserilized object</returns>
//         public U ToObject<U>()
//         {
//             if (null == _node)
//             {
//                 throw new NotImplementedException();
//             }

//             // Deserilize object
//             var resultObj = _node.DeserializeAccept(new DeserializeObject(), typeof(U));

//             if (null == resultObj)
//             {
//                 throw new NotImplementedException();
//             }
            
//             // cast object to U
//             if (resultObj is U result)
//             {
//                 return result;
//             }
//             else
//             {
//                 throw new NotImplementedException();
//             }    
//         }          
//     }
// }