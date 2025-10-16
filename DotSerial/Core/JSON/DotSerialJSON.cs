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

using DotSerial.Core.General;
using DotSerial.Core.Misc;
using DotSerial.Interfaces;

namespace DotSerial.Core.JSON
{
    /// <summary> Serialize and Deserialize an object with Json
    /// </summary>
    internal class DotSerialJSON : ISerial<DotSerialJSON>
    {
        /// <summary>
        /// Json Document
        /// </summary>
        private JSONDocument? _document;

        /// <summary>
        /// Current Version
        /// </summary>
        private static readonly Version s_Version = new(1, 0, 0);

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
        public static void SaveToFile(string path, DotSerialJSON serialObj)
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

                JSONDocument jsonDoc = new();

                jsonDoc.Load(path);

                var desObj = new DotSerialJSON
                {
                    _document = jsonDoc
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
        public static DotSerialJSON Serialize(object? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            if (false == obj.GetType().IsClass)
            {
                throw new NotSupportedException();
            }

            // Create root element
            var rootNode = new DSNode(JsonConstants.DotSerial, DSNodeType.InnerNode, DSNodePropertyType.Class);
            var versionNode = new DSNode(JsonConstants.Version, s_Version.ToString(), DSNodeType.Leaf, DSNodePropertyType.Primitive);
            rootNode.AppendChild(versionNode);

            // Serialze Object
            var node = DSSerialize.Serialize(obj, JsonConstants.MainObjectKey);
            rootNode.AppendChild(node);

            var result = new DotSerialJSON
            {
                _document = new JSONDocument()
            };

            result._document.Tree = rootNode;

            return result;
        }

        /// <inheritdoc/>
        public static U Deserialize<U>(DotSerialJSON serialObj)
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
            var rootNode = serialObj._document.Tree;
            var node = rootNode?.GetChild(JsonConstants.MainObjectKey);

            DSDeserialize.Deserialize(result, node);

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
                string result = JSONWriter.ToJsonString(_document.Tree);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
