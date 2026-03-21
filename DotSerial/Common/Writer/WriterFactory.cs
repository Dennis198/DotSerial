using DotSerial.Json.Writer;
using DotSerial.Toon.Writer;
using DotSerial.Tree.Creation;
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
        private readonly Dictionary<StategyType, IWriteStrategy> _strategies = new();

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
            _strategies.Add(StategyType.Json, new JsonWriterVisitor());
            _strategies.Add(StategyType.Toon, new ToonWriterVisitor());
            _strategies.Add(StategyType.Xml, new XmlWriterVisitor());
            _strategies.Add(StategyType.Yaml, new YamlWriterVisitor());
        }

        /// <summary>
        /// Writes the tree to a string using the specified strategy.
        /// </summary>
        /// <param name="category">The strategy type</param>
        /// <param name="node">The node to write</param>
        /// <returns>Serialized string</returns>
        internal ReadOnlySpan<char> Write(StategyType category, DSNode node)
        {
            if (_strategies.TryGetValue(category, out var strategy))
            {
                return strategy.Write(node);
            }
            throw new NotSupportedException($"Strategy '{category}' is not supported.");
        }
    }
}
