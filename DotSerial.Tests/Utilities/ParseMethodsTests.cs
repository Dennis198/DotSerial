using DotSerial.Tree.Creation;

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
            var result = DotSerial.Utilities.ParseMethods.ParsePrimitiveNode(StategyType.Json, span, 0, "testKey");

            // Assert
            Assert.Equal("testKey", result.Key);
            Assert.Equal("4.4", result.GetValue());
        }
    }
}
