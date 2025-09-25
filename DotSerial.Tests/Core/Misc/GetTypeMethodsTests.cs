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
    public class GetTypeMethodsTests
    {
        #region Array

        [Fact]
        public void GetItemTypeOfArray_Object()
        {
            // Arrange
            int[] tmp = [];

            // Act
            Type? result = DotSerial.Core.Misc.GetTypeMethods.GetItemTypeOfArray(tmp);

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
            Type? result = DotSerial.Core.Misc.GetTypeMethods.GetItemTypeOfArray(tmp.GetType());

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
            var result = DotSerial.Core.Misc.GetTypeMethods.GetItemTypeOfIEnumerable(tmp);

            // Assert
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetItemTypeOfIEnumerable_ListInt_Object()
        {
            // Arrange
            var tmp = new List<int>();

            // Act
            var result = DotSerial.Core.Misc.GetTypeMethods.GetItemTypeOfIEnumerable(tmp);

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
            bool result = DotSerial.Core.Misc.GetTypeMethods.GetKeyValueTypeOfDictionary(tmp, out Type typeKey, out Type typeValue);

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
            bool result = DotSerial.Core.Misc.GetTypeMethods.GetKeyValueTypeOfDictionary(tmp.GetType(), out Type typeKey, out Type typeValue);

            // Assert
            Assert.True(result);
            Assert.NotNull(typeKey);
            Assert.NotNull(typeValue);
            Assert.Equal(typeof(int), typeKey);
            Assert.Equal(typeof(string), typeValue);
        }

        [Fact]
        public void GetDictionaryTypeFromKeyValue_IntString()
        {
            // Arrange
            Dictionary<int, string> tmp = [];

            // Act
            var result = DotSerial.Core.Misc.GetTypeMethods.GetDictionaryTypeFromKeyValue(typeof(int), typeof(string));

            // Assert
            Assert.Equal(tmp.GetType(), result);
        }

        #endregion
    }
}
