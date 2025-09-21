
namespace DotSerial.Tests.Core.Misc
{
    public class ConverterMethodsTests
    {

        [Fact]
        public void ConvertDeserializedList_IntArray()
        {
            // Arrange
            int[] tmp = [5];
            List<object> tmp2 = [1, 2, 3, 4];

            // Act
            object? result = DotSerial.Core.Misc.ConverterMethods.ConvertDeserializedList(tmp2, tmp.GetType());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tmp.GetType(), result.GetType());
        }

        [Fact]
        public void ConvertDeserializedList_IntList()
        {
            // Arrange
            List<int> tmp = [];
            List<object> tmp2 = [1, 2, 3, 4];

            // Act
            object? result = DotSerial.Core.Misc.ConverterMethods.ConvertDeserializedList(tmp2, tmp.GetType());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tmp.GetType(), result.GetType());
        }

        [Fact]
        public void ConvertDeserializedDictionary_IntString()
        {
            // Arrange
            Dictionary<int, string> tmp = [];
            Dictionary<object, object?> tmp2= [];
            tmp2.Add(0, "A");
            tmp2.Add(1, "B");
            tmp2.Add(2, "C");

            // Act
            object? result = DotSerial.Core.Misc.ConverterMethods.ConvertDeserializedDictionary(tmp2, tmp.GetType());

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
            object result = DotSerial.Core.Misc.ConverterMethods.ConvertEnumToObject(enu.GetType(), enu);

            // Assert
            Assert.Equal(enu.GetType(), result.GetType());
            Assert.Equal((int)enu, (int)result);
        }
    }
}
