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

using DotSerial.Core.Tree;

namespace DotSerial.Tests.Core.Tree
{
    public class NodeExtensionsTests
    {
        [Theory]
        [InlineData(DSNodeType.Root, "Root")]
        [InlineData(DSNodeType.InnerNode, "Inner node")]
        [InlineData(DSNodeType.Leaf, "Leaf")]
        public void ConvertToString_DSNodeType(DSNodeType type, string expected)
        {
            // Act
            var result = NodeExtensions.ConvertToString(type);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Undefined", DSNodePropertyType.Undefined)]
        [InlineData("Primitive", DSNodePropertyType.Primitive)]
        [InlineData("Class", DSNodePropertyType.Class)]
        [InlineData("List", DSNodePropertyType.List)]
        [InlineData("Dictionary", DSNodePropertyType.Dictionary)]
        [InlineData("KeyValuePair", DSNodePropertyType.KeyValuePair)]
        [InlineData("KeyValuePairValue", DSNodePropertyType.KeyValuePairValue)]
        [InlineData("Null", DSNodePropertyType.Null)]
        public void ConvertToDSNodePropertyType(string str, DSNodePropertyType expected)
        {
            // Act
            var result = NodeExtensions.ConvertToDSNodePropertyType(str);

            // Assert
            Assert.Equal(expected, result);
        }
        
        [Theory]
        [InlineData(DSNodePropertyType.Undefined, "Undefined")]
        [InlineData(DSNodePropertyType.Primitive, "Primitive")]
        [InlineData(DSNodePropertyType.Class, "Class")]
        [InlineData(DSNodePropertyType.List, "List")]
        [InlineData(DSNodePropertyType.Dictionary, "Dictionary")]
        [InlineData(DSNodePropertyType.KeyValuePair, "KeyValuePair")]
        [InlineData(DSNodePropertyType.KeyValuePairValue, "KeyValuePairValue")]
        [InlineData(DSNodePropertyType.Null, "Null")]
        public void ConvertToString_DSNodePropertyType(DSNodePropertyType type, string expected)
        {
            // Act
            var result = NodeExtensions.ConvertToString(type);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}