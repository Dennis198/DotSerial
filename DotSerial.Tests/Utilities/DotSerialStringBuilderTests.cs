using DotSerial.Utilities;

namespace DotSerial.Tests.Utilities
{
    public class DotSerialStringBuilderTests
    {
        [Fact]
        public void Append_ShouldAddTextToBuffer()
        {
            // Arrange
            var builder = new DotSerialStringBuilder();

            // Act
            builder.Append("Hello".AsSpan());

            // Assert
            Assert.Equal("Hello", builder.ToString());
        }

        [Fact]
        public void Append_ShouldIncreaseLength()
        {
            // Arrange
            var builder = new DotSerialStringBuilder();

            // Act
            builder.Append("Hello".AsSpan());

            // Assert
            Assert.Equal(5, builder.Length);
        }

        [Fact]
        public void Clear_ShouldResetBuffer()
        {
            // Arrange
            var builder = new DotSerialStringBuilder();
            builder.Append("Hello".AsSpan());

            // Act
            builder.Clear();

            // Assert
            Assert.Equal(0, builder.Length);
            Assert.Equal(string.Empty, builder.ToString());
        }

        [Fact]
        public void Trim_ShouldRemoveLeadingAndTrailingWhitespace()
        {
            // Arrange
            var builder = new DotSerialStringBuilder("   Hello World   ");

            // Act
            builder.Trim();

            // Assert
            Assert.Equal("Hello World", builder.ToString());
        }

        [Fact]
        public void Trim_ShouldHandleEmptyBuffer()
        {
            // Arrange
            var builder = new DotSerialStringBuilder();

            // Act
            builder.Trim();

            // Assert
            Assert.Equal(string.Empty, builder.ToString());
        }

        [Fact]
        public void Indexer_ShouldGetAndSetCharacters()
        {
            // Arrange
            var builder = new DotSerialStringBuilder("Hello");

            // Act
            builder[0] = 'Y';

            // Assert
            Assert.Equal('Y', builder[0]);
            Assert.Equal("Yello", builder.ToString());
        }

        [Fact]
        public void Remove_ShouldDeleteSpecifiedRange()
        {
            // Arrange
            var builder = new DotSerialStringBuilder("Hello World");

            // Act
            builder.Remove(5, 6);

            // Assert
            Assert.Equal("Hello", builder.ToString());
        }

        [Fact]
        public void Insert_ShouldAddTextAtSpecifiedPosition()
        {
            // Arrange
            var builder = new DotSerialStringBuilder("Hello");
            builder.Insert(5, " World".AsSpan());

            // Act & Assert
            Assert.Equal("Hello World", builder.ToString());
        }

        [Fact]
        public void IsNullOrWhiteSpace_ShouldReturnTrueForEmptyOrWhitespace()
        {
            // Arrange
            var builder = new DotSerialStringBuilder("   ");

            // Act & Assert
            Assert.True(builder.IsNullOrWhiteSpace());
        }

        [Fact]
        public void IsNullOrWhiteSpace_ShouldReturnFalseForNonWhitespace()
        {
            // Arrange
            var builder = new DotSerialStringBuilder("Hello");

            // Act & Assert
            Assert.False(builder.IsNullOrWhiteSpace());
        }
    }
}
