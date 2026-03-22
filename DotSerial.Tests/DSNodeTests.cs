using DotSerial.Tree;
using DotSerial.Tree.Creation;

namespace DotSerial.Tests
{
    public class DSNodeTests
    {
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        // private readonly StategyType strategy = StategyType.Yaml;

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Create(SerializeStrategy strategy)
        {
            // Arrange
            var tmp = _nodeFactory.CreateNode(strategy, "key", "value", NodeType.Leaf);

            // Act
            var result = new DSNode(tmp, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("key", result.Key);
            Assert.False(result.HasChildren);
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

        //    [Theory]
        // [InlineData(StategyType.Json)]
        // [InlineData(StategyType.Toon)]
        // [InlineData(StategyType.Xml)]
        // [InlineData(StategyType.Yaml)]
        // public void GetChild()
        // {
        //     // Arrange
        //     var tmp = _nodeFactory.CreateNode(strategy, "child", "value", NodeType.Leaf);
        //     var tmp2 = _nodeFactory.CreateNode(strategy, "key", null, NodeType.InnerNode);
        //     tmp2.AddChild(tmp);
        //     var resultNode = new DSNode(tmp2);

        //     // Act
        //     var result = resultNode.GetChild("child");

        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.Equal("child", result.Key);
        //     Assert.False(result.HasChildren);
        // }

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
    }
}
