using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Xml;

namespace DotSerial.Tests.Xml
{
    public class DSXmlNodeTests
    {
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        [Fact]
        public void Create()
        {
            // Arrange
            var tmp = _nodeFactory.CreateNode(StategyType.Xml, "key", "value", NodeType.Leaf);

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
            var tmp = _nodeFactory.CreateNode(StategyType.Xml, "child", "value", NodeType.Leaf);
            var tmp2 = _nodeFactory.CreateNode(StategyType.Xml, "key", null, NodeType.InnerNode);
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
        public void FromString()
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.FromString(xmlString);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Primitive()
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSXmlNode.ToNode(tmp);
            string xmlString = node.Stringify();

            // Act
            var result = DSXmlNode.ToObject<double>(xmlString);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Fact]
        public void Primitive_Null()
        {
            // Arrange
            string? tmp = null;
            var node = DSXmlNode.ToNode(tmp);
            string xmlString = node.Stringify();

            // Act
            var result = DSXmlNode.ToObject<string>(xmlString);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Fact]
        public void Primitive_Empty()
        {
            // Arrange
            string? tmp = string.Empty;
            var node = DSXmlNode.ToNode(tmp);
            string xmlString = node.Stringify();

            // Act
            var result = DSXmlNode.ToObject<string>(xmlString);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Fact]
        public void PrimitiveSpecialChar()
        {
            // Arrange
            string tmp = "{{}<><:;[[]-?!#!";
            var node = DSXmlNode.ToNode(tmp);
            string xmlString = node.Stringify();

            // Act
            var result = DSXmlNode.ToObject<string>(xmlString);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Fact]
        public void List()
        {
            // Arrange
            double[] tmp = [1.1, 2.2, 3.3, 4.4];
            var node = DSXmlNode.ToNode(tmp);
            string xmlString = node.Stringify();

            // Act
            var result = DSXmlNode.ToObject<double[]>(xmlString);

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
            var node = DSXmlNode.ToNode(tmp);
            string xmlString = node.Stringify();

            // Act
            var result = DSXmlNode.ToObject<double[]>(xmlString);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void List_Empty()
        {
            // Arrange
            double[]? tmp = [];
            var node = DSXmlNode.ToNode(tmp);
            string xmlString = node.Stringify();

            // Act
            var result = DSXmlNode.ToObject<double[]>(xmlString);

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
            var node = DSXmlNode.ToNode(tmp);
            string xmlString = node.Stringify();

            // Act
            var result = DSXmlNode.ToObject<Dictionary<string, double>?>(xmlString);

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
            var node = DSXmlNode.ToNode(tmp);
            string xmlString = node.Stringify();

            // Act
            var result = DSXmlNode.ToObject<Dictionary<string, double>?>(xmlString);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Dictionary_Empty()
        {
            // Arrange
            Dictionary<string, double> tmp = [];
            var node = DSXmlNode.ToNode(tmp);
            string xmlString = node.Stringify();

            // Act
            var result = DSXmlNode.ToObject<Dictionary<string, double>>(xmlString);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ToObject_ExampleClass()
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<ExampleClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_EmptyClass()
        {
            // Arrange
            var example = new EmptyClass();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<EmptyClass>(xmlString);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ToObject_ListFirstElementNull()
        {
            // Arrange
            var example = ListFirstElementNull.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<ListFirstElementNull>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_NoAttributeClass()
        {
            // Arrange
            var example = NoAttributeClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<NoAttributeClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_AccessModifierClass()
        {
            // Arrange
            var example = AccessModifierClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<AccessModifierClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_SimpleClass()
        {
            // Arrange
            var example = SimpleClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<SimpleClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_NullClass()
        {
            // Arrange
            var example = NullClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<NullClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_IEnumerableClass()
        {
            // Arrange
            var example = IEnumerableClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<IEnumerableClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_NestedClasss()
        {
            // Arrange
            var example = NestedClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<NestedClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_NestedNestedClass()
        {
            // Arrange
            var example = NestedNestedClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<NestedNestedClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_PrimitiveClass()
        {
            // Arrange
            var example = PrimitiveClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<PrimitiveClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_DictionaryClass()
        {
            // Arrange
            var example = DictionaryClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<DictionaryClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_EnumClass()
        {
            // Arrange
            var example = EnumClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<EnumClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_DateTimeClass()
        {
            // Arrange
            var example = DateTimeClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<DateTimeClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_StructClass()
        {
            // Arrange
            var example = StructClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<StructClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_RecordClass()
        {
            // Arrange
            var example = RecordClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<RecordClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ParsableClass()
        {
            // Arrange
            var example = ParsableClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<ParsableClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_PathClass()
        {
            // Arrange
            var example = PathClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<PathClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassWithoutParameterlessConstructor()
        {
            // Arrange
            var example = ClassWithoutParameterlessConstructor.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<ClassWithoutParameterlessConstructor>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassRecordNoParameterlessConstructor()
        {
            // Arrange
            var example = ClassRecordNoParameterlessConstructor.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<ClassRecordNoParameterlessConstructor>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_PrimitiveClassIEnumarable()
        {
            // Arrange
            var example = PrimitiveClassIEnumarable.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<PrimitiveClassIEnumarable>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_MultiDimClassIEnumarble()
        {
            // Arrange
            var example = MultiDimClassIEnumarble.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<MultiDimClassIEnumarble>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_EmptyObjectClass()
        {
            // Arrange
            var example = EmptyObjectClass.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<EmptyObjectClass>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassWithOneList()
        {
            // Arrange
            var example = ClassWithOneList.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<ClassWithOneList>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassWithOneDictionary()
        {
            // Arrange
            var example = ClassWithOneDictionary.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<ClassWithOneDictionary>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassWithOnePrimitive()
        {
            // Arrange
            var example = ClassWithOnePrimitive.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var xmlString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<ClassWithOnePrimitive>(xmlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassSpecialCharsKeys()
        {
            // Arrange
            var example = ClassSpecialCharsKeys.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var resultString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<ClassSpecialCharsKeys>(resultString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_ClassSpecialCharsValue()
        {
            // Arrange
            var example = ClassSpecialCharsValue.CreateTestDefault();
            var tmp = DSXmlNode.ToNode(example);
            var resultString = tmp.Stringify();

            // Act
            var result = DSXmlNode.ToObject<ClassSpecialCharsValue>(resultString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }
    }
}
