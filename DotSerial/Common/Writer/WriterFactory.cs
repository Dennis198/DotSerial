using DotSerial.Json.Writer;
using DotSerial.Toon.Writer;
using DotSerial.Xml.Writer;
using DotSerial.Yaml.Writer;

namespace DotSerial.Common.Writer
{
    /// <summary>
    /// Writer factory (singleton)
    /// </summary>
    internal sealed class WriterFactory
    {
        private static readonly Lazy<WriterFactory> _instance = new(() => new WriterFactory());
        private readonly Dictionary<SerializeStrategy, IWriteStrategy> _strategies = [];

        /// <summary>
        /// Returns the instance
        /// </summary>
        public static WriterFactory Instance => _instance.Value;

        /// <summary>
        /// Private constructor
        /// </summary>
        private WriterFactory()
        {
            // Initialize strategies
            _strategies.Add(SerializeStrategy.Json, new JsonWriterVisitor());
            _strategies.Add(SerializeStrategy.Toon, new ToonWriterVisitor());
            _strategies.Add(SerializeStrategy.Xml, new XmlWriterVisitor());
            _strategies.Add(SerializeStrategy.Yaml, new YamlWriterVisitor());
        }

        /// <summary>
        /// Writes the tree to a string using the specified strategy.
        /// </summary>
        /// <param name="category">The strategy type</param>
        /// <param name="node">The node to write</param>
        /// <returns>Serialized string</returns>
        internal ReadOnlySpan<char> Write(SerializeStrategy category, DSNode node)
        {
            if (_strategies.TryGetValue(category, out var strategy))
            {
                return strategy.Write(node);
            }
            throw new NotSupportedException($"Strategy '{category}' is not supported.");
        }
    }
}
