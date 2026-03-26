using DotSerial.Utilities;

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
            DotSerialStringBuilder sb2 = new(128);
            try
            {
                // Act
                WriteMethods.AddIndentation(ref sb2, count, 4);
                sb2.Append(str);

                // Assert
                Assert.Equal(new string(' ', count * 4) + str, sb2.ToString());
            }
            finally
            {
                sb2.Dispose();
            }
        }
    }
}
