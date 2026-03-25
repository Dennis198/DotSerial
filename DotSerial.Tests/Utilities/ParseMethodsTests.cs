using DotSerial.Utilities;

namespace DotSerial.Tests.Utilities
{
    public class ParserMethodsTests
    {
        [Fact]
        public void ParsePrimitiveNode()
        {
            // Arrange
            string str = "\"4.4\"";
            var span = str.AsSpan();

            // Act
            var result = ParseMethods.ParsePrimitiveNode(
                SerializeStrategy.Json,
                span,
                new ParserBookmark(span, false),
                "testKey"
            );

            // Assert
            Assert.Equal("testKey", result.Key);
            Assert.Equal("4.4", result.GetValue());
        }
    }
}
