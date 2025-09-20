namespace DotSerial.Tests.Core.Misc
{
    public class HelperMethodsTests
    {

        [Fact]
        public void ImplementsIEnumerable_ListInt_Object()
        {
            // Arrange
            var tmp = new List<int>();

            // Act
            var result = DotSerial.Core.Misc.HelperMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ImplementsIEnumerable_Int_Object()
        {
            // Arrange
            int tmp = 5;

            // Act
            var result = DotSerial.Core.Misc.HelperMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ImplementsIEnumerable_ListInt_Type()
        {
            // Arrange
            Type tmp = typeof(List<int>);

            // Act
            var result = DotSerial.Core.Misc.HelperMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ImplementsIEnumerable_Int_Type()
        {
            // Arrange
            Type tmp = typeof(int);

            // Act
            var result = DotSerial.Core.Misc.HelperMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void BoolToInt_True()
        {
            // Arrange
            bool tmp = true;

            // Act
            int result = DotSerial.Core.Misc.HelperMethods.BoolToInt(tmp);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void BoolToInt_False()
        {
            // Arrange
            bool tmp = false;

            // Act
            int result = DotSerial.Core.Misc.HelperMethods.BoolToInt(tmp);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void IntToBool_False()
        {
            // Arrange
            int tmp = 0;

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IntToBool(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IntToBool_True()
        {
            // Arrange
            int tmp = 1;

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IntToBool(tmp);

            // Assert
            Assert.True(result);
        }

    }
}
