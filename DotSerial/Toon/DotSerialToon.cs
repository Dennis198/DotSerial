using DotSerial.Common;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Serialize;

namespace DotSerial.Toon
{
    /// <summary> Serialize and Deserialize an object with Toon
    /// </summary>
    public class DotSerialToon : ISerial<DotSerialToon>
    {
        /// <summary>
        /// Toon Document
        /// </summary>
        private ToonDocument? _document;

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
        /// <exception cref="DSToonException">Argument null.</exception>
        public static void SaveToFile(string path, DotSerialToon serialObj)
        {
            ArgumentNullException.ThrowIfNull(serialObj);

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (null == serialObj._document)
            {
                throw new DSToonException($"{serialObj._document} can't be null");
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
        /// <exception cref="DSToonException">Argument null.</exception>
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

                ToonDocument toonDoc = new();

                toonDoc.Load(path);

                var desObj = new DotSerialToon { _document = toonDoc };

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
        /// <exception cref="DSToonException">Argument null.</exception>
        public static DotSerialToon Serialize(object? obj)
        {
            // Serialze Object
            var rootNode = SerializeObject.Serialize(obj, CommonConstants.MainObjectKey, StategyType.Toon);

            var result = new DotSerialToon { _document = new ToonDocument() };

            result._document.RootNode = new DSToonNode(rootNode);

            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSToonException">Argument null.</exception>
        public static U Deserialize<U>(DotSerialToon serialObj)
        {
            ArgumentNullException.ThrowIfNull(serialObj);

            if (null == serialObj._document)
            {
                throw new DSToonException($"{serialObj._document} can't be null");
            }

            // Get root element
            var rootNode = serialObj._document.RootNode ?? throw new NullReferenceException();
            var result = IDSSerialNode<U>.ToObject<U>(rootNode.GetInternalData());

            return result;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        public DSToonNode GetRootNode()
        {
            if (null == _document)
            {
                throw new DSToonException($"{_document} can't be null");
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
