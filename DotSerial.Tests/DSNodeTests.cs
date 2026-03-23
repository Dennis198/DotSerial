using DotSerial.Tree;
using DotSerial.Tree.Creation;

namespace DotSerial.Tests
{
    public class DSNodeTests
    {
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Add(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);
            var newNode = DSNode.ToNode("Hello DotSerial!", strategy);

            // Act
            node.Add("3", newNode);

            // Assert
            Assert.NotNull(newNode);
            Assert.Equal("3", node["3"].Key);
            Assert.Equal("Hello DotSerial!", node["3"].GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Add_KeyExists(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);
            var newNode = DSNode.ToNode("Hello DotSerial!", strategy);

            // Assert
            Assert.Throws<ArgumentException>(() => node.Add("2", newNode));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Add_KeyValuePair(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);
            var newNode = DSNode.ToNode("Hello DotSerial!", strategy);

            // Act
            node.Add(new KeyValuePair<string, DSNode>("3", newNode));

            // Assert
            Assert.NotNull(newNode);
            Assert.Equal("3", node["3"].Key);
            Assert.Equal("Hello DotSerial!", node["3"].GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Add_KeyValuePair_KeyExists(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);
            var newNode = DSNode.ToNode("Hello DotSerial!", strategy);

            // Assert
            Assert.Throws<ArgumentException>(() => node.Add(new KeyValuePair<string, DSNode>("2", newNode)));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Clear(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);

            // Act
            node.Clear();

            // Assert
            Assert.Equal(0, node.Count);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ContainsKey_False(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);

            // Act
            bool result = node.ContainsKey("3");

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ContainsKey_True(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);

            // Act
            bool result = node.ContainsKey("2");

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Contains_False(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);

            // Act
            bool result = node.Contains(
                new KeyValuePair<string, DSNode>("3", DSNode.ToNode("Hello DotSerial!", strategy))
            );

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Contains_True(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);

            // Act
            bool result = node.Contains(
                new KeyValuePair<string, DSNode>("2", DSNode.ToNode("Hello DotSerial!", strategy))
            );

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Create(SerializeStrategy strategy)
        {
            // Arrange
            var tmp = _nodeFactory.CreateNode(strategy, "key", "value", TreeNodeType.Leaf);

            // Act
            var result = new DSNode(tmp, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("key", result.Key);
            Assert.Equal(0, result.Count);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Dictionary(SerializeStrategy strategy)
        {
            // Arrange
            Dictionary<string, double> tmp = [];
            tmp.Add("test1", 1.1);
            tmp.Add("test2", 2.2);
            var node = DSNode.ToNode(tmp, strategy);
            string outputString = node.Stringify();

            // Act
            var result = DSNode.ToObject<Dictionary<string, double>?>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(tmp.Count, result.Count);
            Assert.Equal(tmp["test1"], result["test1"]);
            Assert.Equal(tmp["test2"], result["test2"]);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Dictionary_Empty(SerializeStrategy strategy)
        {
            // Arrange
            Dictionary<string, double> tmp = [];
            var node = DSNode.ToNode(tmp, strategy);
            string outputString = node.Stringify();

            // Act
            var result = DSNode.ToObject<Dictionary<string, double>>(outputString, strategy);

            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Dictionary_Null(SerializeStrategy strategy)
        {
            // Arrange
            Dictionary<string, double>? tmp = null;
            var node = DSNode.ToNode(tmp, strategy);
            string outputString = node.Stringify();

            // Act
            var result = DSNode.ToObject<Dictionary<string, double>?>(outputString, strategy);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void FromString(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.FromString(outputString, strategy);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void GetNodeType_Array(SerializeStrategy strategy)
        {
            // Arrange
            int[] example = { 1, 2, 3 };
            var node = DSNode.ToNode(example, strategy);

            // Act
            var result = node.GetNodeType();

            // Assert
            Assert.Equal(NodeType.Array, result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void GetNodeType_Dictionary(SerializeStrategy strategy)
        {
            // Arrange
            Dictionary<string, double> example = new() { { "test1", 1.1 }, { "test2", 2.2 } };
            var node = DSNode.ToNode(example, strategy);

            // Act
            var result = node.GetNodeType();

            // Assert
            Assert.Equal(NodeType.Object, result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void GetNodeType_Object(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);

            // Act
            var result = node.GetNodeType();

            // Assert
            Assert.Equal(NodeType.Object, result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void GetNodeType_Value(SerializeStrategy strategy)
        {
            // Arrange
            int example = 5;
            var node = DSNode.ToNode(example, strategy);

            // Act
            var result = node.GetNodeType();

            // Assert
            Assert.Equal(NodeType.Value, result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void GetNodeValue_Object(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);

            // Act
            Assert.Throws<DotSerialException>(() => node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void GetNodeValue_Value(SerializeStrategy strategy)
        {
            // Arrange
            int example = 5;
            var node = DSNode.ToNode(example, strategy);

            // Act
            var result = node.GetNodeValue();

            // Assert
            Assert.Equal("5", result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void IndexerGet(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);

            // Act
            var newNode = node["2"];

            // Assert
            Assert.NotNull(newNode);
            Assert.Equal("2", newNode.Key);
            Assert.Equal("Hello DotSerial!", newNode.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void IndexerGet_KeyNotExist(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);

            // Act
            Assert.Throws<ArgumentException>(() => node["nonexistent_key"]);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void IndexerSet(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            string primValue = "HelloWorld";
            var node = DSNode.ToNode(example, strategy);

            // Act
            var newNode = DSNode.ToNode(primValue, strategy);
            node["3"] = newNode;

            var values = node.Values;
            var valuesList = values.ToList();

            // Assert
            Assert.NotNull(values);
            Assert.Equal(4, values.Count);
            Assert.Equal("0", valuesList[0].Key);
            Assert.Equal("1", valuesList[1].Key);
            Assert.Equal("2", valuesList[2].Key);
            Assert.Equal("3", valuesList[3].Key);
            Assert.Equal("HelloWorld", valuesList[3].GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void IndexerSetReplace(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            string primValue = "HelloWorld";
            var node = DSNode.ToNode(example, strategy);

            // Act
            var newNode = DSNode.ToNode(primValue, strategy);
            node["2"] = newNode;

            var values = node.Values;
            var valuesList = values.ToList();

            // Assert
            Assert.NotNull(values);
            Assert.Equal(3, values.Count);
            Assert.Equal("0", valuesList[0].Key);
            Assert.Equal("1", valuesList[1].Key);
            Assert.Equal("2", valuesList[2].Key);
            Assert.Equal("HelloWorld", valuesList[2].GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void IsReadOnly(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);

            // Act
            bool result = tmp.IsReadOnly;

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Key(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();

            // Act
            var result = DSNode.ToNode(example, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("0", result.Key);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Keys(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();

            // Act
            var node = DSNode.ToNode(example, strategy);
            var keys = node.Keys;
            var keysList = keys.ToList();

            // Assert
            Assert.NotNull(keys);
            Assert.Equal(3, keys.Count);
            Assert.Equal("0", keysList[0]);
            Assert.Equal("1", keysList[1]);
            Assert.Equal("2", keysList[2]);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void List(SerializeStrategy strategy)
        {
            // Arrange
            double[] tmp = [1.1, 2.2, 3.3, 4.4];
            var node = DSNode.ToNode(tmp, strategy);
            string outputString = node.Stringify();

            // Act
            var result = DSNode.ToObject<double[]>(outputString, strategy);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(tmp.Length, result.Length);
            Assert.Equal(tmp[0], result[0]);
            Assert.Equal(tmp[1], result[1]);
            Assert.Equal(tmp[2], result[2]);
            Assert.Equal(tmp[3], result[3]);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void List_Empty(SerializeStrategy strategy)
        {
            // Arrange
            double[]? tmp = [];
            var node = DSNode.ToNode(tmp, strategy);
            string outputString = node.Stringify();

            // Act
            var result = DSNode.ToObject<double[]>(outputString, strategy);

            // Assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void List_Null(SerializeStrategy strategy)
        {
            // Arrange
            double[]? tmp = null;
            var node = DSNode.ToNode(tmp, strategy);
            string outputString = node.Stringify();

            // Act
            var result = DSNode.ToObject<double[]>(outputString, strategy);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Primitive(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);
            string outputString = node.Stringify();

            // Act
            var result = DSNode.ToObject<double>(outputString, strategy);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void PrimitiveSpecialChar(SerializeStrategy strategy)
        {
            // Arrange
            string tmp = "{{}<><:;[[]-?!#!";
            var node = DSNode.ToNode(tmp, strategy);
            string outputString = node.Stringify();

            // Act
            var result = DSNode.ToObject<string>(outputString, strategy);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Primitive_Empty(SerializeStrategy strategy)
        {
            // Arrange
            string? tmp = string.Empty;
            var node = DSNode.ToNode(tmp, strategy);
            string outputString = node.Stringify();

            // Act
            var result = DSNode.ToObject<string>(outputString, strategy);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Primitive_Null(SerializeStrategy strategy)
        {
            // Arrange
            string? tmp = null;
            var node = DSNode.ToNode(tmp, strategy);
            string outputString = node.Stringify();

            // Act
            var result = DSNode.ToObject<string>(outputString, strategy);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Remove_False(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);

            // Act
            bool result = node.Remove("3");

            // Assert
            Assert.False(result);
            Assert.Equal(3, node.Count);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Remove_True(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);

            // Act
            bool result = node.Remove("2");

            // Assert
            Assert.True(result);
            Assert.Equal(2, node.Count);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValueNull(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValueNull();

            // Assert
            Assert.Null(node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_Boolean(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(true);

            // Assert
            Assert.Equal("true", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_Byte(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue((byte)123);

            // Assert
            Assert.Equal("123", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_Char(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue('A');

            // Assert
            Assert.Equal("A", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_CultureInfo(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(new System.Globalization.CultureInfo("en-US"));

            // Assert
            Assert.Equal("en-US", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_DateTime(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(new DateTime(2024, 1, 1, 12, 0, 0));

            // Assert
            Assert.Equal("1/1/2024 12:00:00 PM", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_Decimal(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(12.45m);

            // Assert
            Assert.Equal("12.45", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_Double(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(12.45d);

            // Assert
            Assert.Equal("12.45", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_Float(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(12.45f);

            // Assert
            Assert.Equal("12.45", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_GUID(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(new Guid("12345678-1234-1234-1234-1234567890ab"));

            // Assert
            Assert.Equal("12345678-1234-1234-1234-1234567890ab", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_IPAddress(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(System.Net.IPAddress.Parse("192.168.0.1"));

            // Assert
            Assert.Equal("192.168.0.1", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_Int(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(15);

            // Assert
            Assert.Equal("15", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_Long(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(15L);

            // Assert
            Assert.Equal("15", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_NInt(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue((nint)15);

            // Assert
            Assert.Equal("15", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_NUInt(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue((nuint)15);

            // Assert
            Assert.Equal("15", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_SByte(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue((sbyte)123);

            // Assert
            Assert.Equal("123", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_Short(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue((short)15);

            // Assert
            Assert.Equal("15", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_String(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue("Hello DotSerial!");

            // Assert
            Assert.Equal("Hello DotSerial!", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_TimeSpan(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(new TimeSpan(1, 2, 3, 4));

            // Assert
            Assert.Equal("1.02:03:04", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_UInt(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(15u);

            // Assert
            Assert.Equal("15", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_ULong(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(15UL);

            // Assert
            Assert.Equal("15", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_UShort(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue((ushort)15);

            // Assert
            Assert.Equal("15", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_Uri(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(new Uri("https://example.com"));

            // Assert
            Assert.Equal("https://example.com/", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void SetNodeValue_Version(SerializeStrategy strategy)
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, strategy);

            // Act
            node.SetNodeValue(new Version(1, 2, 3, 4));

            // Assert
            Assert.Equal("1.2.3.4", node.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Stringify(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);

            // Act
            var result = tmp.Stringify();

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToNode(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();

            // Act
            var result = DSNode.ToNode(example, strategy);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_AccessModifierClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = AccessModifierClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<AccessModifierClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_ClassRecordNoParameterlessConstructor(SerializeStrategy strategy)
        {
            // Arrange
            var example = ClassRecordNoParameterlessConstructor.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<ClassRecordNoParameterlessConstructor>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_ClassSpecialCharsKeys(SerializeStrategy strategy)
        {
            // Arrange
            var example = ClassSpecialCharsKeys.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var resultString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<ClassSpecialCharsKeys>(resultString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_ClassSpecialCharsValue(SerializeStrategy strategy)
        {
            // Arrange
            var example = ClassSpecialCharsValue.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var resultString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<ClassSpecialCharsValue>(resultString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_ClassWithOneDictionary(SerializeStrategy strategy)
        {
            // Arrange
            var example = ClassWithOneDictionary.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<ClassWithOneDictionary>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_ClassWithOneList(SerializeStrategy strategy)
        {
            // Arrange
            var example = ClassWithOneList.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<ClassWithOneList>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_ClassWithOnePrimitive(SerializeStrategy strategy)
        {
            // Arrange
            var example = ClassWithOnePrimitive.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<ClassWithOnePrimitive>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_ClassWithoutParameterlessConstructor(SerializeStrategy strategy)
        {
            // Arrange
            var example = ClassWithoutParameterlessConstructor.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<ClassWithoutParameterlessConstructor>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_DateTimeClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = DateTimeClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<DateTimeClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_DictionaryClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = DictionaryClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<DictionaryClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_EmptyClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = new EmptyClass();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<EmptyClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_EmptyObjectClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = EmptyObjectClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<EmptyObjectClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_EnumClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = EnumClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<EnumClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_ExampleClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<ExampleClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_IEnumerableClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = IEnumerableClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<IEnumerableClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_ListFirstElementNull(SerializeStrategy strategy)
        {
            // Arrange
            var example = ListFirstElementNull.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<ListFirstElementNull>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_MultiDimClassIEnumarble(SerializeStrategy strategy)
        {
            // Arrange
            var example = MultiDimClassIEnumarble.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<MultiDimClassIEnumarble>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_NestedClasss(SerializeStrategy strategy)
        {
            // Arrange
            var example = NestedClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<NestedClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_NestedNestedClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = NestedNestedClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<NestedNestedClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_NoAttributeClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = NoAttributeClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<NoAttributeClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_NullClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = NullClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<NullClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_ParsableClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = ParsableClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<ParsableClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_PathClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = PathClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<PathClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_PrimitiveClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = PrimitiveClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<PrimitiveClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_PrimitiveClassIEnumarable(SerializeStrategy strategy)
        {
            // Arrange
            var example = PrimitiveClassIEnumarable.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<PrimitiveClassIEnumarable>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_RecordClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = RecordClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<RecordClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_SimpleClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = SimpleClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<SimpleClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToObject_StructClass(SerializeStrategy strategy)
        {
            // Arrange
            var example = StructClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, strategy);
            var outputString = tmp.Stringify();

            // Act
            var result = DSNode.ToObject<StructClass>(outputString, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void TryAdd_False(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);
            var newNode = DSNode.ToNode("Hello DotSerial!", strategy);

            // Act
            bool result = node.TryAdd(new KeyValuePair<string, DSNode>("2", newNode));

            // Assert
            Assert.False(result);
            Assert.Equal(3, node.Count);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void TryAdd_True(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);
            var newNode = DSNode.ToNode("Hello DotSerial!", strategy);

            // Act
            bool result = node.TryAdd(new KeyValuePair<string, DSNode>("3", newNode));

            // Assert
            Assert.True(result);
            Assert.Equal("3", node["3"].Key);
            Assert.Equal("Hello DotSerial!", node["3"].GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void TryGetValue_False(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);

            // Act
            var result = node.TryGetValue("3", out DSNode? value);

            // Assert
            Assert.False(result);
            Assert.Equal(3, node.Count);
            Assert.Null(value);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void TryGetValue_True(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var node = DSNode.ToNode(example, strategy);

            // Act
            var result = node.TryGetValue("2", out DSNode? value);

            // Assert
            Assert.True(result);
            Assert.Equal(3, node.Count);
            Assert.Equal("Hello DotSerial!", value?.GetNodeValue());
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Values(SerializeStrategy strategy)
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();

            // Act
            var node = DSNode.ToNode(example, strategy);
            var values = node.Values;
            var valuesList = values.ToList();

            // Assert
            Assert.NotNull(values);
            Assert.Equal(3, values.Count);
            Assert.Equal("0", valuesList[0].Key);
            Assert.Equal("1", valuesList[1].Key);
            Assert.Equal("2", valuesList[2].Key);
        }
    }
}
