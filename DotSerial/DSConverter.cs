using DotSerial.Common;
using DotSerial.Tree.Serialize;

namespace DotSerial
{
    /// <summary>
    /// Provides static methods for serializing objects to and from various formats,
    /// and for converting between objects and <see cref="DSNode"/> trees.
    /// </summary>
    public static class DSConverter
    {
        /// <summary>
        /// Deserializes a string representation into an object of type <typeparamref name="U"/>.
        /// </summary>
        /// <typeparam name="U">The target type to deserialize into.</typeparam>
        /// <param name="str">The serialized string to parse.</param>
        /// <param name="strategy">The serialization strategy (e.g. JSON, XML, YAML) used to interpret <paramref name="str"/>.</param>
        /// <returns>An instance of <typeparamref name="U"/> populated with the deserialized data.</returns>
        public static U Deserialize<U>(ReadOnlySpan<char> str, SerializeStrategy strategy)
        {
            try
            {
                return DSNode.ToObject<U>(str, strategy);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Reads a file from disk and deserializes its contents into an object of type <typeparamref name="U"/>.
        /// </summary>
        /// <typeparam name="U">The target type to deserialize into.</typeparam>
        /// <param name="path">The path to the file to read.</param>
        /// <param name="strategy">The serialization strategy used to interpret the file contents.</param>
        /// <returns>An instance of <typeparamref name="U"/> populated with the deserialized data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is null or whitespace.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the file at <paramref name="path"/> does not exist.</exception>
        public static U LoadFromFile<U>(string path, SerializeStrategy strategy)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            if (false == FileMethods.LoadFileContent(path, out string content))
            {
                throw new NotSupportedException();
            }

            var result = Deserialize<U>(content, strategy);
            return result;
        }

        /// <summary>
        /// Serializes an object and saves the result to a file on disk.
        /// </summary>
        /// <param name="path">The path of the file to write.</param>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="strategy">The serialization strategy used to format the output.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="path"/> is null or whitespace.</exception>
        public static void SaveToFile(string path, object? obj, SerializeStrategy strategy)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var content = Serialize(obj, strategy);
            FileMethods.SaveContentToFile(path, content);
        }

        /// <summary>
        /// Serializes an object into its string representation using the specified strategy.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="strategy">The serialization strategy (e.g. JSON, XML, YAML) used to format the output.</param>
        /// <returns>A string containing the serialized representation of <paramref name="obj"/>.</returns>
        public static string Serialize(object? obj, SerializeStrategy strategy)
        {
            var rootNode = SerializeObject.Serialize(obj, CommonConstants.MainObjectKey, strategy);
            var dsNode = new DSNode(rootNode, strategy);
            return dsNode.Stringify();
        }

        /// <summary>
        /// Converts an object into a <see cref="DSNode"/> tree using the specified strategy.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <param name="strategy">The serialization strategy used to build the node tree.</param>
        /// <returns>A <see cref="DSNode"/> representing the serialized object.</returns>
        public static DSNode ToNode(object? obj, SerializeStrategy strategy)
        {
            var rootNode = SerializeObject.Serialize(obj, CommonConstants.MainObjectKey, strategy);
            var dsNode = new DSNode(rootNode, strategy);
            return dsNode;
        }

        /// <summary>
        /// Parses a serialized string into a <see cref="DSNode"/> tree using the specified strategy.
        /// </summary>
        /// <param name="str">The serialized string to parse.</param>
        /// <param name="strategy">The serialization strategy used to interpret <paramref name="str"/>.</param>
        /// <returns>A <see cref="DSNode"/> representing the parsed content.</returns>
        public static DSNode ToNodeFromString(ReadOnlySpan<char> str, SerializeStrategy strategy)
        {
            var root = DSNode.FromString(str, strategy);
            return root;
        }
    }
}
