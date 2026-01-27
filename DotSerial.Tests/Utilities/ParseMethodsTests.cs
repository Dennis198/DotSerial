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

using System.Text;

namespace DotSerial.Tests.Utilities
{
    public class ParserMethodsTests
    {
        [Fact]
        public void AppendStringValue()
        {
            // Arrange
            string str = "    \"Hello\"            \"World. DotSerial\"   ";
            StringBuilder sbStr = new(str);
            StringBuilder sb = new();

            // Act
            var result = DotSerial.Utilities.ParseMethods.AppendStringValue(sb, 4, sbStr);

            // Assert
            Assert.Equal(10, result);
            Assert.Equal("\"Hello\"", sb.ToString());
        }

        [Fact]
        public void AppendEnclosingValue()
        {
            // Arrange
            string str = "    <<Hello>>            \"World. DotSerial\"   ";
            StringBuilder sbStr = new(str);
            StringBuilder sb = new();

            // Act
            var result = DotSerial.Utilities.ParseMethods.AppendEnclosingValue(sb, 4, sbStr, '<', '>');

            // Assert
            Assert.Equal(12, result);
            Assert.Equal("<<Hello>>", sb.ToString());
        }        

        [Fact]
        public void RemoveWhiteSpace()
        {
            // Arrange
            string str = "    \"Hello\"            \"World. DotSerial\"   ";

            // Act
            var result = DotSerial.Utilities.ParseMethods.RemoveWhiteSpace(str);

            // Assert
            Assert.Equal("\"Hello\"\"World. DotSerial\"", result.ToString());
        }

        [Fact]
        public void ParsePrimitiveNode()
        {
            // Arrange
            string str = "\"4.4\"";
            StringBuilder strBuilder = new(str);

            // Act
            var result = DotSerial.Utilities.ParseMethods.ParsePrimitiveNode(strBuilder, 0, "testKey");

            // Assert
            Assert.Equal("testKey", result.Key);
            Assert.Equal("4.4", result.GetValue());
        }        
    }
}