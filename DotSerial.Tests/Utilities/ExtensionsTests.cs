using System.Text;
using DotSerial.Utilities;

namespace DotSerial.Tests.Utilities
{
    public class ExtensionsTests
    {
        [Fact]
        public void TrimStartAndEnd()
        {
            // Arrange
            StringBuilder sb = new("  Hello World!     ");

            // Act
            var result = sb.TrimStartAndEnd();

            // Assert
            Assert.Equal("Hello World!", result.ToString());
        }

        [Fact]
        public void TrimEnd()
        {
            // Arrange
            StringBuilder sb = new("  Hello World!     ");

            // Act
            var result = sb.TrimEnd();

            // Assert
            Assert.Equal("  Hello World!", result.ToString());
        }

        [Fact]
        public void Trim()
        {
            // Arrange
            StringBuilder sb = new("  Hello World!     ");

            // Act
            var result = sb.Trim();

            // Assert
            Assert.Equal("Hello World!     ", result.ToString());
        }

        [Fact]
        public void EqualsContent_True()
        {
            // Arrange
            StringBuilder sb = new("  Hello World!     ");
            StringBuilder sb2 = new("Hello World!");

            // Act
            var result = sb.EqualsContent(sb2, 2);

            // Assert
            Assert.True(result);
        }      

        [Fact]
        public void EqualsContent_False()
        {
            // Arrange
            StringBuilder sb = new("  Hello World!     ");
            StringBuilder sb2 = new("Hello World!");

            // Act
            var result = sb.EqualsContent(sb2, 1);

            // Assert
            Assert.False(result);
        }        

        [Fact]
        public void IndexOf_Found()
        {
            // Arrange
            StringBuilder sb = new("  Hello World!     ");

            // Act
            var result = sb.IndexOf("World");

            // Assert
            Assert.Equal(8, result);
        }       

        [Fact]
        public void IndexOf_NotFound()
        {
            // Arrange
            StringBuilder sb = new("  Hello World!     ");

            // Act
            var result = sb.IndexOf("Bye");

            // Assert
            Assert.Equal(-1, result);
        }         

        [Fact]
        public void SubString()
        {
            // Arrange
            StringBuilder sb = new("  Hello World!     ");

            // Act
            var result = sb.SubString(2, 11);

            // Assert
            Assert.Equal("Hello World", result.ToString());
        }      

        [Fact]
        public void EqualsNullString_True()
        {
            // Arrange
            StringBuilder sb = new("null");

            // Act
            var result = sb.EqualsNullString();

            // Assert
            Assert.True(result);
        }    

        [Fact]
        public void EqualsNullString_False()
        {
            // Arrange
            StringBuilder sb = new("abcd");

            // Act
            var result = sb.EqualsNullString();

            // Assert
            Assert.False(result);
        } 

        [Fact]
        public void EqualsNullString_True_Index()
        {
            // Arrange
            StringBuilder sb = new("  null");

            // Act
            var result = sb.EqualsNullString(2);

            // Assert
            Assert.True(result);
        }    

        [Fact]
        public void EqualsNullString_False_Index()
        {
            // Arrange
            StringBuilder sb = new("  null");

            // Act
            var result = sb.EqualsNullString(1);

            // Assert
            Assert.False(result);
        }     

        [Fact]
        public void EqualFirstNoWhiteSpaceChar_True()
        {
            // Arrange
            StringBuilder sb = new("  null");

            // Act
            var result = sb.EqualFirstNoWhiteSpaceChar('n');

            // Assert
            Assert.True(result);
        }    

        [Fact]
        public void EqualFirstNoWhiteSpaceChar_False()
        {
            // Arrange
            StringBuilder sb = new("  null");

            // Act
            var result = sb.EqualsNullString('u');

            // Assert
            Assert.False(result);
        }      

        [Fact]
        public void EqualLastNoWhiteSpaceChar_True()
        {
            // Arrange
            StringBuilder sb = new("  null  ");

            // Act
            var result = sb.EqualLastNoWhiteSpaceChar('l');

            // Assert
            Assert.True(result);
        }    

        [Fact]
        public void EqualLastNoWhiteSpaceChar_False()
        {
            // Arrange
            StringBuilder sb = new("  null  ");

            // Act
            var result = sb.EqualLastNoWhiteSpaceChar('u');

            // Assert
            Assert.False(result);
        }       

        [Fact]
        public void CountQuotedValues_Zero()
        {
            // Arrange
            StringBuilder sb = new("  abc  ");

            // Act
            var result = sb.CountQuotedValues();

            // Assert
            Assert.Equal(0, result);
        }              

        [Fact]
        public void CountQuotedValues_Three()
        {
            // Arrange
            StringBuilder sb = new("  \"abc\", \"def\", \"ghi\"  ");

            // Act
            var result = sb.CountQuotedValues();

            // Assert
            Assert.Equal(3, result);
        }   

        [Fact]
        public void CountQuotedValues_Three_Null()
        {
            // Arrange
            StringBuilder sb = new("  \"abc\", null, \"ghi\"  ");

            // Act
            var result = sb.CountQuotedValues();

            // Assert
            Assert.Equal(3, result);
        }        

        [Fact]
        public void CountQuotedValues_Three_WithoutNull()
        {
            // Arrange
            StringBuilder sb = new("  \"abc\", null, \"ghi\"  ");

            // Act
            var result = sb.CountQuotedValues(false);

            // Assert
            Assert.Equal(2, result);
        }      

        [Fact]
        public void SkipStringValue()
        {
            // Arrange
            StringBuilder sb = new("  \"abc\", null, \"ghi\"  ");

            // Act
            var result = sb.SkipStringValue(2);

            // Assert
            Assert.Equal(6, result);
        }    

        [Fact]
        public void SkipEnclosedValue()
        {
            // Arrange
            StringBuilder sb = new("  <abc>, null, \"ghi\"  ");

            // Act
            var result = sb.SkipEnclosedValue(2, '<', '>');

            // Assert
            Assert.Equal(6, result);
        }                                                          
    }
}