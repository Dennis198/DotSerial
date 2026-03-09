using System.Text;

namespace DotSerial.Tests.Utilities
{
    public class WriterMethodsTests
    {
        [Theory]
        [InlineData("HelloWorld", 0)]
        [InlineData("HelloWorld", 2)]
        [InlineData("HelloWorld", 4)]
        [InlineData("HelloWorld", 10)]
        public void AddIndentation(string str, int count)
        {
            // Arrange
            StringBuilder sb = new();

            // Act
            DotSerial.Utilities.WriteMethods.AddIndentation(sb, count, 4);
            sb.Append(str);

            // Assert
            Assert.Equal(new string(' ', count * 4) + str, sb.ToString());
        }

    }
}