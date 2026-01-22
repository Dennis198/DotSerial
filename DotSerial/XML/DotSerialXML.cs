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

using DotSerial.Utilities;
using DotSerial.Common;
using DotSerial.Tree.Serialize;

namespace DotSerial.XML
{
    /// <summary> Serialize and Deserialize an object with System.Xml
    /// </summary>
    public class DotSerialXML : ISerial<DotSerialXML>
    {
        /// <summary>
        /// XML Document
        /// </summary>
        private XmlDocument? _document;

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
                throw new DSXmlException($"{serialObj._document} can't be null");
            }

            try
            {
                serialObj._document.Save(path);
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

                xmlDoc.Load(path);

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
            // Serialze Object
            var rootNode = SerializeObject.Serialize(obj, CommonConstants.MainObjectKey);            

            var result = new DotSerialXML
            {
                _document = new XmlDocument()
            };

            result._document.RootNode = new DSXmlNode(rootNode);

            return result;            
        }

        /// <inheritdoc/>
        public static U Deserialize<U>(DotSerialXML serialObj)
        {
            ArgumentNullException.ThrowIfNull(serialObj);

            if (null == serialObj._document)
            {
                throw new DSXmlException($"{serialObj._document} can't be null");
            }

            // Get root element
            var rootNode = serialObj._document.RootNode ?? throw new NullReferenceException();
            var result = IDSSerialNode<U>.ToObject<U>(rootNode.GetInternalData());

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

            if (TypeCheckMethods.IsClass(t) || TypeCheckMethods.IsStruct(t) || TypeCheckMethods.IsSpecialParsableObject(t))
            {
                return true;
            }

            return false;
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
                string result = _document.RootNode?.Stringify() ?? string.Empty;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
