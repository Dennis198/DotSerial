
namespace DotSerial.Tests.Core.Misc
{
    public class CreateInstanceMethodsTests
    {
        [Fact]
        public void CreateInstanceGeneric_Class()
        {
            // Arrange
            var simpleClass = new SimpleClass();

            // Act
            object result = DotSerial.Core.Misc.CreateInstanceMethods.CreateInstanceGeneric(simpleClass.GetType());

            // Assert
            Assert.Equal(simpleClass.GetType(), result.GetType());
        }

        [Fact]
        public void CreateInstanceGenericT_Class()
        {
            // Arrange
            var simpleClass = new SimpleClass();

            // Act
            object result = DotSerial.Core.Misc.CreateInstanceMethods.CreateInstanceGeneric<SimpleClass>();

            // Assert
            Assert.Equal(simpleClass.GetType(), result.GetType());
        }

        [Fact]
        public void CreateInstanceArray_int()
        {
            // Arrange
            var array = new int[5];

            // Act
            Array result = DotSerial.Core.Misc.CreateInstanceMethods.CreateInstanceArray(array.GetType(), array.Length);

            // Assert
            Assert.Equal(array.GetType(), result.GetType());
            Assert.Equal(array.Length, result.Length);
        }
    }
}
