using System.Xml;

using DotSerial.Interfaces;

namespace DotSerial.Core.XML
{
    /// <summary> Serialize and Deserialize an object with System.Xml
    /// </summary>
    public class XMLSerial : ISerial<XmlDocument>
    {
        /// <summary>
        /// Current Version
        /// </summary>
        public static readonly Version s_Version = new(1, 0);

        /// <inheritdoc/>
        public static XmlDocument CreateSerializedObject(object? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            if (false == obj.GetType().IsClass)
            {
                throw new NotSupportedException();
            }

            XmlDocument xmlDoc = new();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration(s_Version.ToString(), "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            // Create root element
            XmlElement xnodeRoot = xmlDoc.CreateElement(Constants.MainElementName);
            xmlDoc.AppendChild(xnodeRoot);

            // Serialze Object
            XMLSerial_Serialize.Serialize(obj, xmlDoc, xnodeRoot, Constants.MainObjectID);

            return xmlDoc;
        }

        /// <inheritdoc/>
        public static bool DeserializeObject(object obj, XmlDocument serialObj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(serialObj);

            if (false == obj.GetType().IsClass)
            {
                throw new NotSupportedException();
            }

            // Get root element
            XmlNodeList s = serialObj.GetElementsByTagName(Constants.MainElementName) ?? throw new NullReferenceException();
            XmlNode xnodeParameters = s.Item(0) ?? throw new NullReferenceException();
            XmlNode rootNode = xnodeParameters.ChildNodes[0] ?? throw new NullReferenceException();

            XMLSerial_Deserialize.Deserialize(obj, rootNode);

            return true;
        }

        /// <inheritdoc/>
        public static string AsString(XmlDocument xmlDoc)
        {
            ArgumentNullException.ThrowIfNull(xmlDoc);

            using StringWriter sw = new();
            using XmlTextWriter tx = new(sw);
            xmlDoc.WriteTo(tx);
            string strXmlText = sw.ToString();
            return strXmlText;
        }

        /// <inheritdoc/>
        public static bool IsTypeSupported(Type t)
        {
            // Primitive + string.
            if (Misc.HelperMethods.IsPrimitive(t))
            {
                return true;
            }

            if (Misc.HelperMethods.ImplementsIEnumerable(t))
            {
                if (Misc.HelperMethods.IsDictionary(t) ||
                    Misc.HelperMethods.IsList(t) ||
                    Misc.HelperMethods.IsArray(t))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (Misc.HelperMethods.IsClass(t) || Misc.HelperMethods.IsStruct(t))
            {
                return true;
            }

            return false;
        }
    }
}
