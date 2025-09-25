#region License
//Copyright (c) 2025 Dennis Sölch

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
#endregion

using System.Xml;
using System.Xml.Linq;
using DotSerial.Core.Misc;
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
        private static readonly Version s_Version = new(1, 0, 0);

        /// <inheritdoc/>
        public static void SaveToFile(string path, object? obj)
        {           
            try
            {
                var xmlDocument = Serialize(obj);
                SaveToFile(path, xmlDocument);
            }
            catch 
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public static void SaveToFile(string path, DotSerialXML serialObj)
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
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public static U LoadFromFile<U>(string path)
        {

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

                var result = Deserialize<U>(desObj);
                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public static DotSerialXML Serialize(object? obj)
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
        public static U Deserialize<U>(DotSerialXML serialObj)
        {
            ArgumentNullException.ThrowIfNull(serialObj);

            var result = CreateInstanceMethods.CreateInstanceGeneric<U>();

            if (false == result?.GetType().IsClass)
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

            DotSerialXMLDeserialize.Deserialize(result, rootNode);

            return result;
        }


        /// <inheritdoc/>
        public static bool IsTypeSupported(Type t)
        {
            // Primitive + string.
            if (TypeCheckMethods.IsPrimitive(t))
            {
                return true;
            }

            if (HelperMethods.ImplementsIEnumerable(t))
            {
                if (TypeCheckMethods.IsDictionary(t) ||
                    TypeCheckMethods.IsList(t) ||
                    TypeCheckMethods.IsArray(t))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (TypeCheckMethods.IsClass(t) || TypeCheckMethods.IsStruct(t))
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
        internal static void CreateAttribute(XmlDocument xmlDoc, XmlElement xmlElement, string name, string value)
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

        /// <summary>
        /// Converts the serialized object to an string.
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            if (null == _document)
            {
                return string.Empty;
            }

            try
            {
                XDocument doc = XDocument.Parse(_document.OuterXml);

                // Add decleration
                string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

                // Xml content
                string xmlString = doc.ToString();

                // Add new line so declaration is seperated in one line
                result += Environment.NewLine;

                // Add xml content to result
                result += xmlString;

                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
