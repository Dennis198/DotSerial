
using DotSerial.Core.General;
using DotSerial.Core.Misc;
using DotSerial.Interfaces;

namespace DotSerial.Core.YAML
{
    public class DotSerialYAML : ISerial<DotSerialYAML>
    {
        /// <summary>
        /// Json Document
        /// </summary>
        private YAMLDocument? _document;

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
        public static void SaveToFile(string path, DotSerialYAML serialObj)
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

                YAMLDocument jsonDoc = new();

                jsonDoc.Load(path);

                var desObj = new DotSerialYAML
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
        public static DotSerialYAML Serialize(object? obj)
        {
            ArgumentNullException.ThrowIfNull(obj);

            if (false == obj.GetType().IsClass)
            {
                throw new NotSupportedException();
            }

            // Create root element
            var rootNode = new DSNode(YAMLConstants.DotSerial, DSNodeType.InnerNode, DSNodePropertyType.Class);
            var versionNode = new DSNode(YAMLConstants.Version, s_Version.ToString(), DSNodeType.Leaf, DSNodePropertyType.Primitive);
            rootNode.AppendChild(versionNode);

            // Serialze Object
            var node = DSSerialize.Serialize(obj, YAMLConstants.MainObjectKey);
            rootNode.AppendChild(node);

            var result = new DotSerialYAML
            {
                _document = new YAMLDocument()
            };

            result._document.Tree = rootNode;

            return result;
        }

        /// <inheritdoc/>
        public static U Deserialize<U>(DotSerialYAML serialObj)
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
            var node = rootNode?.GetChild(YAMLConstants.MainObjectKey);

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
                string result = YAMLWriter.ToYamlString(_document.Tree);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }                     
    }
}