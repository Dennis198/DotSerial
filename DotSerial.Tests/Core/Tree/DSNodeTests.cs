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
    public class DSNodeTests
    {
        [Fact]
        public void IsNull_True()
        {
            // Arrange
            var tmp = new DSNode("0", null, DSNodeType.Leaf, DSNodePropertyType.Primitive);

            // Assert
            Assert.True(tmp.IsNull);
        }

        [Fact]
        public void IsNull_False()
        {
            // Arrange
            var tmp = new DSNode("0", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);

            // Assert
            Assert.False(tmp.IsNull);
        }

        [Fact]
        public void IsEmpty_True()
        {
            // Arrange
            var tmp = new DSNode("0", DSNodeType.InnerNode, DSNodePropertyType.Class);

            // Assert
            Assert.True(tmp.IsEmpty);
        }

        [Fact]
        public void IsEmpty_False()
        {
            // Arrange
            var tmp = new DSNode("0", DSNodeType.InnerNode, DSNodePropertyType.Class);
            var tmp2 = new DSNode("0", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);
            tmp.AppendChild(tmp2);

            // Assert
            Assert.False(tmp.IsNull);
        }

        [Fact]
        public void HasChildren_False()
        {
            // Arrange
            var tmp = new DSNode("0", DSNodeType.InnerNode, DSNodePropertyType.Class);

            // Assert
            Assert.False(tmp.HasChildren);
        }

        [Fact]
        public void HasChildren_True()
        {
            // Arrange
            var tmp = new DSNode("0", DSNodeType.InnerNode, DSNodePropertyType.Class);
            var tmp2 = new DSNode("0", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);
            tmp.AppendChild(tmp2);

            // Assert
            Assert.True(tmp.HasChildren);
        }

        [Fact]
        public void Count()
        {
            // Arrange
            var tmp = new DSNode("0", DSNodeType.InnerNode, DSNodePropertyType.Class);
            var tmp2 = new DSNode("0", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);
            var tmp3 = new DSNode("1", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);
            tmp.AppendChild(tmp2);
            tmp.AppendChild(tmp3);

            // Assert
            Assert.Equal(2, tmp.Count);
        }

        [Fact]
        public void GetChildren()
        {
            // Arrange
            var tmp = new DSNode("0", DSNodeType.InnerNode, DSNodePropertyType.Class);
            var tmp2 = new DSNode("0", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);
            var tmp3 = new DSNode("1", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);
            tmp.AppendChild(tmp2);
            tmp.AppendChild(tmp3);

            var children = tmp.GetChildren();

            // Assert
            Assert.NotNull(children);
            Assert.Equal(2, children.Count);
        }

        [Fact]
        public void GetGetFirstChildChildren()
        {
            // Arrange
            var tmp = new DSNode("0", DSNodeType.InnerNode, DSNodePropertyType.Class);
            var tmp2 = new DSNode("0", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);
            var tmp3 = new DSNode("1", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);
            tmp.AppendChild(tmp2);
            tmp.AppendChild(tmp3);

            var child = tmp.GetFirstChild();

            // Assert
            Assert.NotNull(child);
            Assert.Equal("0", child.Key);
        }

        [Fact]
        public void GetNthChild()
        {
            // Arrange
            var tmp = new DSNode("0", DSNodeType.InnerNode, DSNodePropertyType.Class);
            var tmp2 = new DSNode("0", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);
            var tmp3 = new DSNode("1", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);
            tmp.AppendChild(tmp2);
            tmp.AppendChild(tmp3);

            var child = tmp.GetNthChild(1);

            // Assert
            Assert.NotNull(child);
            Assert.Equal("1", child.Key);
        }

        [Fact]
        public void GetChild()
        {
            // Arrange
            var tmp = new DSNode("0", DSNodeType.InnerNode, DSNodePropertyType.Class);
            var tmp2 = new DSNode("0", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);
            var tmp3 = new DSNode("1", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);
            tmp.AppendChild(tmp2);
            tmp.AppendChild(tmp3);

            var child = tmp.GetChild("1");

            // Assert
            Assert.NotNull(child);
            Assert.Equal("1", child.Key);
        }

        [Fact]
        public void ConvertValue()
        {
            // Arrange
            var tmp = new DSNode("0", 5, DSNodeType.Leaf, DSNodePropertyType.Primitive);

            var child = tmp.ConvertValue(typeof(int));

            // Assert
            Assert.NotNull(child);
            Assert.Equal(typeof(int), child.GetType());
            Assert.Equal(5, child);
        }

    }
}
