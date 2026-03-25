using DotSerial.Json.Parser;
using DotSerial.Toon.Parser;
using DotSerial.Utilities;
using DotSerial.Xml.Parser;
using DotSerial.Yaml.Parser;

namespace DotSerial.Common.Parser
{
    /// <summary>
    /// Parser factory (singleton)
    /// </summary>
    internal sealed class ParserFactory
    {
        private static readonly Lazy<ParserFactory> _instance = new(() => new ParserFactory());
        private readonly Dictionary<SerializeStrategy, IParserStrategy> _strategies = [];

        /// <summary>
        /// Returns the instance
        /// </summary>
        public static ParserFactory Instance => _instance.Value;

        /// <summary>
        /// Private constructor
        /// </summary>
        private ParserFactory()
        {
            // Initialize strategies
            _strategies.Add(SerializeStrategy.Json, new JsonParserVisitor());
            _strategies.Add(SerializeStrategy.Toon, new ToonParserVisitor());
            _strategies.Add(SerializeStrategy.Xml, new XmlParserVisitor());
            _strategies.Add(SerializeStrategy.Yaml, new YamlParserVisitor());
        }

        /// <summary>
        /// Parses the string to a node using the specified strategy.
        /// </summary>
        /// <param name="category">The strategy type</param>
        /// <param name="str">The string to parse</param>
        /// <returns>Parsed node</returns>
        internal DSNode Parse(SerializeStrategy category, ReadOnlySpan<char> str)
        {
            if (_strategies.TryGetValue(category, out var strategy))
            {
                return strategy.Parse(str);
            }

            ThrowHelper.ThrowStrategyNotSupportedException();
            throw new Exception("Unreachable code.");
        }
    }
}
