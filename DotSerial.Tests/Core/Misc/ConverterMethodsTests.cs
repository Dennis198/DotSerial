#region License
//Copyright (c) 2025 Dennis Sölch

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
#endregion

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

        [Fact]
        public void ConvertStringToPrimitive_Int()
        {
            // Arrange
            string str = "1893";
            int expected = 1893;

            // Act
            object? result = DotSerial.Core.Misc.ConverterMethods.ConvertStringToPrimitive(str, expected.GetType());

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
            object? result = DotSerial.Core.Misc.ConverterMethods.ConvertStringToSpecialParsableObject(str, expected.GetType());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.GetType(), result.GetType());
            Assert.Equal(expected, (DateTime)result);
        }
    }
}
