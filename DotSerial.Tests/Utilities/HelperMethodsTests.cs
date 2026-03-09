namespace DotSerial.Tests.Utilities
{
    public class HelperMethodsTests
    {

        [Fact]
        public void ImplementsIEnumerable_ListInt_Object()
        {
            // Arrange
            var tmp = new List<int>();

            // Act
            var result = DotSerial.Utilities.HelperMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ImplementsIEnumerable_Int_Object()
        {
            // Arrange
            int tmp = 5;

            // Act
            var result = DotSerial.Utilities.HelperMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ImplementsIEnumerable_ListInt_Type()
        {
            // Arrange
            Type tmp = typeof(List<int>);

            // Act
            var result = DotSerial.Utilities.HelperMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ImplementsIEnumerable_Int_Type()
        {
            // Arrange
            Type tmp = typeof(int);

            // Act
            var result = DotSerial.Utilities.HelperMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void BoolToString_True()
        {
            // Arrange
            bool tmp = true;

            // Act
            string result = DotSerial.Utilities.HelperMethods.BoolToString(tmp);

            // Assert
            Assert.Equal("true", result);
        }

        [Fact]
        public void BoolToString_False()
        {
            // Arrange
            bool tmp = false;

            // Act
            string result = DotSerial.Utilities.HelperMethods.BoolToString(tmp);

            // Assert
            Assert.Equal("false", result);
        }

        [Fact]
        public void StringToBool_False()
        {
            // Arrange
            string tmp = "false";

            // Act
            bool result = DotSerial.Utilities.HelperMethods.StringToBool(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void StringToBool_True()
        {
            // Arrange
            string tmp = "true";

            // Act
            bool result = DotSerial.Utilities.HelperMethods.StringToBool(tmp);

            // Assert
            Assert.True(result);
        }

    }
}
