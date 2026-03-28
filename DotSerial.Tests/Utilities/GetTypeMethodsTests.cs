namespace DotSerial.Tests.Utilities
{
    public class GetTypeMethodsTests
    {
        #region Array

        [Fact]
        public void GetItemTypeOfArray_Object()
        {
            // Arrange
            int[] tmp = [];

            // Act
            Type? result = DotSerial.Utilities.GetTypeMethods.GetItemTypeOfArray(tmp);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetItemTypeOfArray_Type()
        {
            // Arrange
            int[] tmp = [];

            // Act
            Type? result = DotSerial.Utilities.GetTypeMethods.GetItemTypeOfArray(tmp.GetType());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(int), result);
        }

        #endregion

        #region List

        [Fact]
        public void GetItemTypeOfIEnumerable_ListInt_Type()
        {
            // Arrange
            Type tmp = typeof(List<int>);

            // Act
            var result = DotSerial.Utilities.GetTypeMethods.GetItemTypeOfIEnumerable(tmp);

            // Assert
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetItemTypeOfIEnumerable_ListInt_Object()
        {
            // Arrange
            var tmp = new List<int>();

            // Act
            var result = DotSerial.Utilities.GetTypeMethods.GetItemTypeOfIEnumerable(tmp);

            // Assert
            Assert.Equal(typeof(int), result);
        }

        #endregion

        #region Dictionary

        [Fact]
        public void GetKeyValueTypeOfDictionary_Object()
        {
            // Arrange
            Dictionary<int, string> tmp = [];

            // Act
            bool result = DotSerial.Utilities.GetTypeMethods.GetKeyValueTypeOfDictionary(
                tmp,
                out Type typeKey,
                out Type typeValue
            );

            // Assert
            Assert.True(result);
            Assert.NotNull(typeKey);
            Assert.NotNull(typeValue);
            Assert.Equal(typeof(int), typeKey);
            Assert.Equal(typeof(string), typeValue);
        }

        [Fact]
        public void GetKeyValueTypeOfDictionary_Type()
        {
            // Arrange
            Dictionary<int, string> tmp = [];

            // Act
            bool result = DotSerial.Utilities.GetTypeMethods.GetKeyValueTypeOfDictionary(
                tmp.GetType(),
                out Type typeKey,
                out Type typeValue
            );

            // Assert
            Assert.True(result);
            Assert.NotNull(typeKey);
            Assert.NotNull(typeValue);
            Assert.Equal(typeof(int), typeKey);
            Assert.Equal(typeof(string), typeValue);
        }

        #endregion
    }
}
