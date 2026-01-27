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
