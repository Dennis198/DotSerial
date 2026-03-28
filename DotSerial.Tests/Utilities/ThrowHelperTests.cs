namespace DotSerial.Tests.Utilities
{
    public class ThrowHelperTests
    {
        [Fact]
        public void ThrowArgumentNullException_ShouldThrowArgumentNullException()
        {
            // Arrange
            string jsonString = "{\"key\": \"value}";

            // Act & Assert
            var ex = Assert.Throws<DotSerialException>(() => DSNode.FromString(jsonString, SerializeStrategy.Json));
            Assert.NotNull(ex);
            // Assert.Equal("Unterminated string at position 8.", ex.Message);
        }

        [Fact]
        public void ThrowArgumentNullException_ShouldThrowArgumentNullException2()
        {
            // Arrange
            string jsonString = "\"key\": \"value\"}";

            // Act & Assert
            var ex = Assert.Throws<DotSerialException>(() => DSNode.FromString(jsonString, SerializeStrategy.Json));
            Assert.NotNull(ex);
            // Assert.Equal("Unterminated string at position 8.", ex.Message);
        }
    }
}
