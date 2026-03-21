using DotSerial.Tree;
using DotSerial.Tree.Creation;

namespace DotSerial.Tests.Xml
{
    public class DSXmlNodeTests
    {
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;
        private readonly StategyType _strategy = StategyType.Xml;

        [Fact]
        public void Create()
        {
            // Arrange
            var tmp = _nodeFactory.CreateNode(_strategy, "key", "value", NodeType.Leaf);

            // Act
            var result = new DSNode(tmp);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("key", result.Key);
            Assert.False(result.HasChildren);
        }

        // [Fact]
        // public void GetChild()
        // {
        //     // Arrange
        //     var tmp = _nodeFactory.CreateNode(_strategy, "child", "value", NodeType.Leaf);
        //     var tmp2 = _nodeFactory.CreateNode(_strategy, "key", null, NodeType.InnerNode);
        //     tmp2.AddChild(tmp);
        //     var resultNode = new DSNode(tmp2);

        //     // Act
        //     var result = resultNode.GetChild("child");

        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.Equal("child", result.Key);
        //     Assert.False(result.HasChildren);
        // }

        [Fact]
        public void ToNode()
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();

            // Act
            var result = DSNode.ToNode(example, _strategy);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Stringify()
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);

            // Act
            var result = tmp.Stringify(_strategy);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FromString()
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.FromString(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Primitive()
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSNode.ToNode(tmp, _strategy);
            string outputString = node.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<double>(outputString, _strategy);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Fact]
        public void Primitive_Null()
        {
            // Arrange
            string? tmp = null;
            var node = DSNode.ToNode(tmp, _strategy);
            string outputString = node.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<string>(outputString, _strategy);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Fact]
        public void Primitive_Empty()
        {
            // Arrange
            string? tmp = string.Empty;
            var node = DSNode.ToNode(tmp, _strategy);
            string outputString = node.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<string>(outputString, _strategy);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Fact]
        public void PrimitiveSpecialChar()
        {
            // Arrange
            string tmp = "{{}<><:;[[]-?!#!";
            var node = DSNode.ToNode(tmp, _strategy);
            string outputString = node.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<string>(outputString, _strategy);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Fact]
        public void List()
        {
            // Arrange
            double[] tmp = [1.1, 2.2, 3.3, 4.4];
            var node = DSNode.ToNode(tmp, _strategy);
            string outputString = node.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<double[]>(outputString, _strategy);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(tmp.Length, result.Length);
            Assert.Equal(tmp[0], result[0]);
            Assert.Equal(tmp[1], result[1]);
            Assert.Equal(tmp[2], result[2]);
            Assert.Equal(tmp[3], result[3]);
        }

        [Fact]
        public void List_Null()
        {
            // Arrange
            double[]? tmp = null;
            var node = DSNode.ToNode(tmp, _strategy);
            string outputString = node.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<double[]>(outputString, _strategy);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void List_Empty()
        {
            // Arrange
            double[]? tmp = [];
            var node = DSNode.ToNode(tmp, _strategy);
            string outputString = node.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<double[]>(outputString, _strategy);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Dictionary()
        {
            // Arrange
            Dictionary<string, double> tmp = [];
            tmp.Add("test1", 1.1);
            tmp.Add("test2", 2.2);
            var node = DSNode.ToNode(tmp, _strategy);
            string outputString = node.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<Dictionary<string, double>?>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(tmp.Count, result.Count);
            Assert.Equal(tmp["test1"], result["test1"]);
            Assert.Equal(tmp["test2"], result["test2"]);
        }

        [Fact]
        public void Dictionary_Null()
        {
            // Arrange
            Dictionary<string, double>? tmp = null;
            var node = DSNode.ToNode(tmp, _strategy);
            string outputString = node.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<Dictionary<string, double>?>(outputString, _strategy);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Dictionary_Empty()
        {
            // Arrange
            Dictionary<string, double> tmp = [];
            var node = DSNode.ToNode(tmp, _strategy);
            string outputString = node.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<Dictionary<string, double>>(outputString, _strategy);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ToObject_ExampleClass()
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<ExampleClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_EmptyClass()
        {
            // Arrange
            var example = new EmptyClass();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<EmptyClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ToObject_ListFirstElementNull()
        {
            // Arrange
            var example = ListFirstElementNull.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<ListFirstElementNull>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_NoAttributeClass()
        {
            // Arrange
            var example = NoAttributeClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<NoAttributeClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_AccessModifierClass()
        {
            // Arrange
            var example = AccessModifierClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<AccessModifierClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_SimpleClass()
        {
            // Arrange
            var example = SimpleClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<SimpleClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_NullClass()
        {
            // Arrange
            var example = NullClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<NullClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_IEnumerableClass()
        {
            // Arrange
            var example = IEnumerableClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<IEnumerableClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_NestedClasss()
        {
            // Arrange
            var example = NestedClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<NestedClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_NestedNestedClass()
        {
            // Arrange
            var example = NestedNestedClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<NestedNestedClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_PrimitiveClass()
        {
            // Arrange
            var example = PrimitiveClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<PrimitiveClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_DictionaryClass()
        {
            // Arrange
            var example = DictionaryClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<DictionaryClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_EnumClass()
        {
            // Arrange
            var example = EnumClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<EnumClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_DateTimeClass()
        {
            // Arrange
            var example = DateTimeClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<DateTimeClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_StructClass()
        {
            // Arrange
            var example = StructClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<StructClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_RecordClass()
        {
            // Arrange
            var example = RecordClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<RecordClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ParsableClass()
        {
            // Arrange
            var example = ParsableClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<ParsableClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_PathClass()
        {
            // Arrange
            var example = PathClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<PathClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassWithoutParameterlessConstructor()
        {
            // Arrange
            var example = ClassWithoutParameterlessConstructor.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<ClassWithoutParameterlessConstructor>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassRecordNoParameterlessConstructor()
        {
            // Arrange
            var example = ClassRecordNoParameterlessConstructor.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<ClassRecordNoParameterlessConstructor>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_PrimitiveClassIEnumarable()
        {
            // Arrange
            var example = PrimitiveClassIEnumarable.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<PrimitiveClassIEnumarable>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_MultiDimClassIEnumarble()
        {
            // Arrange
            var example = MultiDimClassIEnumarble.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<MultiDimClassIEnumarble>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_EmptyObjectClass()
        {
            // Arrange
            var example = EmptyObjectClass.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<EmptyObjectClass>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassWithOneList()
        {
            // Arrange
            var example = ClassWithOneList.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<ClassWithOneList>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassWithOneDictionary()
        {
            // Arrange
            var example = ClassWithOneDictionary.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<ClassWithOneDictionary>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassWithOnePrimitive()
        {
            // Arrange
            var example = ClassWithOnePrimitive.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var outputString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<ClassWithOnePrimitive>(outputString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassSpecialCharsKeys()
        {
            // Arrange
            var example = ClassSpecialCharsKeys.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var resultString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<ClassSpecialCharsKeys>(resultString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassSpecialCharsValue()
        {
            // Arrange
            var example = ClassSpecialCharsValue.CreateTestDefault();
            var tmp = DSNode.ToNode(example, _strategy);
            var resultString = tmp.Stringify(_strategy);

            // Act
            var result = DSNode.ToObject<ClassSpecialCharsValue>(resultString, _strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }
    }
}
