using DotSerial.XML;
using DotSerial.Tree;

namespace DotSerial.Tests.Core.XML
{
    public class DSXmlNodeTests
    {
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;  

        [Fact]
        public void Create()
        {
            // Arrange
            var tmp = _nodeFactory.CreateNode("key", "value", NodeType.Leaf);

            // Act
            var result = new DSXmlNode(tmp);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("key", result.Key);
            Assert.False(result.HasChildren);
        }

        [Fact]
        public void GetChild()
        {
            // Arrange
            var tmp = _nodeFactory.CreateNode("child", "value", NodeType.Leaf);
            var tmp2 = _nodeFactory.CreateNode("key", null, NodeType.InnerNode);
            tmp2.AddChild(tmp);
            var resultNode = new DSXmlNode(tmp2);

            // Act
            var result = resultNode.GetChild("child");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("child", result.Key);
            Assert.False(result.HasChildren);
        }

        [Fact]
        public void ToNode()
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();

            // Act
            var result = DSXmlNode.ToNode(example);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Stringify()
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);

            // Act
            var result = tmp.Stringify();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ToObject_ExampleClass()
        {   
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var jsonString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<ExampleClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }
    }
}