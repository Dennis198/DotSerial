using DotSerial.Core.JSON;
using DotSerial.Core.Tree;

namespace DotSerial.Tests.Core.JSON
{
    public class DSJsonNodeTests
    {
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;
        
        [Fact]
        public void Create()
        {
            // Arrange
            var tmp = _nodeFactory.CreateNode("key", "value", NodeType.Leaf);

            // Act
            var result = new DSJsonNode(tmp);

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
            tmp.AddChild(tmp2);
            var resultNode = new DSJsonNode(tmp);

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
            var result = DSJsonNode.ToNode(example);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ToJsonString()
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);

            // Act
            var result = tmp.ToJsonString();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FromJsonString()
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.FromJsonString(jsonString);

            // Assert
            Assert.NotNull(result);
        }            

        [Fact]
        public void ToObject_FromString()
        {   
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<ExampleClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(example.Boolean, result.Boolean);
            Assert.Equal(example.Number, result.Number);
            Assert.Equal(example.Text, result.Text);
        }
    }
}