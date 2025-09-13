using System.Xml;

using DotSerial.Interfaces;

namespace DotSerial.Core.XML
{
    /// <summary> Serialize and Deserialize an object with System.Xml
    /// </summary>
    public class DotSerialXML : ISerial<DotSerialXML>
    {
        /// <summary>
        /// XML Document
        /// </summary>
        private XmlDocument? _document;

        /// <summary>
        /// Current Version
        /// </summary>
        public static readonly Version s_Version = new(0, 0, 1);

        /// <inheritdoc/>
        public static bool Save(string path, object? obj)
        {           
            try
            {
                var xmlDocument = CreateSerializedObject(obj);
                return Save(path, xmlDocument);
            }
            catch 
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public static bool Save(string path, DotSerialXML serialObj)
        {
            ArgumentNullException.ThrowIfNull(serialObj);

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (null == serialObj._document)
            {
                throw new NullReferenceException();
            }

            try
            {
                using var fileStream = File.Open(path, FileMode.Create);
                serialObj._document.Save(fileStream);
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public static bool Load(string path, object obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            try
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException(path);
                }

                XmlDocument xmlDoc = new();

                using (var fileStream = File.OpenRead(path))
                {
                    xmlDoc.Load(fileStream);
                }

                var desObj = new DotSerialXML
                {
                    _document = xmlDoc
                };

                var result = DeserializeObject(obj, desObj);
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public static DotSerialXML CreateSerializedObject(object? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            if (false == obj.GetType().IsClass)
            {
                throw new NotSupportedException();
            }

            XmlDocument xmlDoc = new();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration(new Version(1, 0).ToString(), "utf-8", null);
            xmlDoc.AppendChild(xmlDecl);

            // Create root element
            XmlElement xnodeRoot = xmlDoc.CreateElement(Constants.DotSerial);
            xmlDoc.AppendChild(xnodeRoot);

            CreateAttribute(xmlDoc, xnodeRoot, Constants.XmlAttributeVersion, s_Version.ToString());
            CreateAttribute(xmlDoc, xnodeRoot, Constants.XmlAttributeProducer, "DotSerial");

            // Serialze Object
            DotSerialXMLSerialize.Serialize(obj, xmlDoc, xnodeRoot, Constants.MainObjectID);

            var result = new DotSerialXML
            {
                _document = xmlDoc
            };

            return result;
        }

        /// <inheritdoc/>
        public static bool DeserializeObject(object obj, DotSerialXML serialObj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            ArgumentNullException.ThrowIfNull(serialObj);

            if (false == obj.GetType().IsClass)
            {
                throw new NotSupportedException();
            }

            if (null == serialObj._document)
            {
                throw new NullReferenceException();
            }

            // Get root element
            XmlDocument doc = serialObj._document;
            XmlNodeList s = doc.GetElementsByTagName(Constants.DotSerial) ?? throw new NullReferenceException();
            XmlNode xnodeParameters = s.Item(0) ?? throw new NullReferenceException();
            XmlNode rootNode = xnodeParameters.ChildNodes[0] ?? throw new NullReferenceException();

            DotSerialXMLDeserialize.Deserialize(obj, rootNode);

            return true;
        }

        /// <inheritdoc/>
        public static string AsString(DotSerialXML serialObj)
        {
            ArgumentNullException.ThrowIfNull(serialObj);

            if (null == serialObj._document)
            {
                throw new NullReferenceException();
            }

            using StringWriter sw = new();
            using XmlTextWriter tx = new(sw);
            var xmlDoc = serialObj._document;
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

        /// <summary>
        /// Create XML Attribute
        /// </summary>
        /// <param name="xmlDoc">XmlDoc</param>
        /// <param name="xmlElement">XmlElement</param>
        /// <param name="name">Name of attribute</param>
        /// <param name="value">Value of attribute</param>
        public static void CreateAttribute(XmlDocument xmlDoc, XmlElement xmlElement, string name, string value)
        {
            ArgumentNullException.ThrowIfNull(xmlDoc);
            ArgumentNullException.ThrowIfNull(xmlElement);
            ArgumentNullException.ThrowIfNull(value);
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(name);
            }

            // Create Attribute
            var xnodeParaID = xmlDoc.CreateAttribute(name);
            xnodeParaID.InnerText = value;
            xmlElement.Attributes?.Append(xnodeParaID);
        }
    }
}
