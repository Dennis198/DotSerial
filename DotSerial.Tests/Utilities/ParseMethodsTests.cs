using System.Text;
using DotSerial.Tree.Creation;

namespace DotSerial.Tests.Utilities
{
    public class ParserMethodsTests
    {
        [Fact]
        public void AppendStringValue()
        {
            // Arrange
            string str = "    \"Hello\"            \"World. DotSerial\"   ";
            StringBuilder sbStr = new(str);
            StringBuilder sb = new();

            // Act
            var result = DotSerial.Utilities.ParseMethods.AppendStringValue(sb, 4, sbStr);

            // Assert
            Assert.Equal(10, result);
            Assert.Equal("\"Hello\"", sb.ToString());
        }

        [Fact]
        public void AppendEnclosingValue()
        {
            // Arrange
            string str = "    <<Hello>>            \"World. DotSerial\"   ";
            StringBuilder sbStr = new(str);
            StringBuilder sb = new();

            // Act
            var result = DotSerial.Utilities.ParseMethods.AppendEnclosingValue(sb, 4, sbStr, '<', '>');

            // Assert
            Assert.Equal(12, result);
            Assert.Equal("<<Hello>>", sb.ToString());
        }

        [Fact]
        public void RemoveWhiteSpace()
        {
            // Arrange
            string str = "    \"Hello\"            \"World. DotSerial\"   ";

            // Act
            var result = DotSerial.Utilities.ParseMethods.RemoveWhiteSpace(str);

            // Assert
            Assert.Equal("\"Hello\"\"World. DotSerial\"", result.ToString());
        }

        [Fact]
        public void ParsePrimitiveNode()
        {
            // Arrange
            string str = "\"4.4\"";
            StringBuilder strBuilder = new(str);

            // Act
            var result = DotSerial.Utilities.ParseMethods.ParsePrimitiveNode(
                StategyType.Json,
                strBuilder,
                0,
                "testKey"
            );

            // Assert
            Assert.Equal("testKey", result.Key);
            Assert.Equal("4.4", result.GetValue());
        }
    }
}
