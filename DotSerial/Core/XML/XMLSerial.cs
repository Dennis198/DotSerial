using System.Xml;
using DotSerial.Interfaces;

namespace DotSerial.Core.XML
{
    /// <summary> Serialize and Deserialize an object with System.Xml
    /// </summary>
    public class XMLSerial : ISerial<XmlDocument>
    {
        /// <inheritdoc/>
        public static XmlDocument CreateSerializedObject(object obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            XmlDocument xmlDoc = new();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            // Create root element
            XmlElement xnodeRoot = xmlDoc.CreateElement(Constants.MainElementName);
            xmlDoc.AppendChild(xnodeRoot);

            // Serialze Object
            XMLSerial_Serialize.Serialize(obj, xmlDoc, xnodeRoot);

            return xmlDoc;
        }

        /// <inheritdoc/>
        public static bool DeserializeObject(object obj, XmlDocument serialObj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(serialObj);

            // Get root element
            XmlNodeList s = serialObj.GetElementsByTagName(Constants.MainElementName) ?? throw new NullReferenceException();
            XmlNode xnodeParameters = s.Item(0) ?? throw new NullReferenceException();
            XmlNode rootNode = xnodeParameters.ChildNodes[0] ?? throw new NullReferenceException();

            XMLSerial_Deserialize.Deserialize(obj, rootNode);

            return true;
        }
    }
}
