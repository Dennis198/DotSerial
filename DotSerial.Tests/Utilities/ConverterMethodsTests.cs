namespace DotSerial.Tests.Utilities
{
    public class ConverterMethodsTests
    {
        [Fact]
        public void ConvertDeserializedList_IntArray()
        {
            // Arrange
            int[] tmp = [5];
            List<object?>? tmp2 = [1, 2, 3, 4];

            // Act
            object? result = DotSerial.Utilities.ConverterMethods.ConvertDeserializedList(tmp2, tmp.GetType());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tmp.GetType(), result.GetType());
        }

        [Fact]
        public void ConvertDeserializedList_IntList()
        {
            // Arrange
            List<int> tmp = [];
            List<object?>? tmp2 = [1, 2, 3, 4];

            // Act
            object? result = DotSerial.Utilities.ConverterMethods.ConvertDeserializedList(tmp2, tmp.GetType());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tmp.GetType(), result.GetType());
        }

        [Fact]
        public void ConvertDeserializedDictionary_IntString()
        {
            // Arrange
            Dictionary<int, string> tmp = [];
            Dictionary<object, object?> tmp2 = [];
            tmp2.Add(0, "A");
            tmp2.Add(1, "B");
            tmp2.Add(2, "C");

            // Act
            object? result = DotSerial.Utilities.ConverterMethods.ConvertDeserializedDictionary(tmp2, tmp.GetType());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tmp.GetType(), result.GetType());
        }

        [Fact]
        public void ConvertEnumToObject_TestEnum()
        {
            // Arrange
            var enu = TestEnum.First;

            // Act
            object result = DotSerial.Utilities.ConverterMethods.ConvertEnumToObject(enu.GetType(), enu);

            // Assert
            Assert.Equal(enu.GetType(), result.GetType());
            Assert.Equal((int)enu, (int)result);
        }

        [Fact]
        public void ConvertStringToPrimitive_Int()
        {
            // Arrange
            string str = "1893";
            int expected = 1893;

            // Act
            object? result = DotSerial.Utilities.ConverterMethods.ConvertStringToPrimitive(str, expected.GetType());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.GetType(), result.GetType());
            Assert.Equal(expected, (int)result);
        }

        [Fact]
        public void ConvertStringToSpecialParsableObject_DateTime()
        {
            // Arrange
            string str = DateTime.Now.ToString();
            DateTime expected = DateTime.Now;

            // Act
            object? result = DotSerial.Utilities.ConverterMethods.ConvertStringToSpecialParsableObject(
                str,
                expected.GetType()
            );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.GetType(), result.GetType());
            Assert.Equal(expected.ToString(), ((DateTime)(result)).ToString());
        }
    }
}
