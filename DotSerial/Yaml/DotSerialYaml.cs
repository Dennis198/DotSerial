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

using DotSerial.Common;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Serialize;

namespace DotSerial.Yaml
{
    /// <summary> Serialize and Deserialize an object with Yaml
    /// </summary>
    public class DotSerialYaml : ISerial<DotSerialYaml>
    {
        /// <summary>
        /// Yaml Document
        /// </summary>
        private YamlDocument? _document;

        /// <inheritdoc/>
        public static void SaveToFile(string path, object? obj)
        {
            try
            {
                var document = Serialize(obj);
                SaveToFile(path, document);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSYamlException">Argument null.</exception>
        public static void SaveToFile(string path, DotSerialYaml serialObj)
        {
            ArgumentNullException.ThrowIfNull(serialObj);

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (null == serialObj._document)
            {
                throw new DSYamlException($"{serialObj._document} can't be null");
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
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSYamlException">Argument null.</exception>
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

                YamlDocument yamlDoc = new();

                yamlDoc.Load(path);

                var desObj = new DotSerialYaml
                {
                    _document = yamlDoc
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
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSYamlException">Argument null.</exception>
        public static DotSerialYaml Serialize(object? obj)
        {
            // Serialze Object
            var rootNode = SerializeObject.Serialize(obj, CommonConstants.MainObjectKey, StategyType.Yaml);

            var result = new DotSerialYaml
            {
                _document = new YamlDocument()
            };

            result._document.RootNode = new DSYamlNode(rootNode);

            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSYamlException">Argument null.</exception>
        public static U Deserialize<U>(DotSerialYaml serialObj)
        {
            ArgumentNullException.ThrowIfNull(serialObj);

            if (null == serialObj._document)
            {
                throw new DSYamlException($"{serialObj._document} can't be null");
            }

            // Get root element    
            var rootNode = serialObj._document.RootNode ?? throw new NullReferenceException();
            var result = IDSSerialNode<U>.ToObject<U>(rootNode.GetInternalData());

            return result;
        }     

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        public DSYamlNode GetRootNode()
        {
            if (null == _document)
            {
                throw new DSYamlException($"{_document} can't be null");
            }

            var result = _document.RootNode;

            if (result is null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return result;
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