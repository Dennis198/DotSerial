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
            tmp2.AddChild(tmp);
            var resultNode = new DSJsonNode(tmp2);

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
        public void ToObject_ExampleClass()
        {   
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<ExampleClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_EmptyClass()
        {   
            // Arrange
            var example = new EmptyClass();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<EmptyClass>(jsonString);

            // Assert
            Assert.NotNull(result);
        }   

        [Fact]
        public void ToObject_NoAttributeClass()
        {   
            // Arrange
            var example = NoAttributeClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<NoAttributeClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }  

        [Fact]
        public void ToObject_AccessModifierClass()
        {   
            // Arrange
            var example = AccessModifierClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<AccessModifierClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }  

        [Fact]
        public void ToObject_SimpleClass()
        {   
            // Arrange
            var example = SimpleClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<SimpleClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }      

        [Fact]
        public void ToObject_NullClass()
        {   
            // Arrange
            var example = NullClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<NullClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }  

        [Fact]
        public void ToObject_IEnumerableClass()
        {   
            // Arrange
            var example = IEnumerableClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();
            
            // Act
            var result = DSJsonNode.ToObject<IEnumerableClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }      

        [Fact]
        public void ToObject_NestedClasss()
        {   
            // Arrange
            var example = NestedClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<NestedClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }    

        [Fact]
        public void ToObject_NestedNestedClass()
        {   
            // Arrange
            var example = NestedNestedClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<NestedNestedClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }     

        [Fact]
        public void ToObject_PrimitiveClass()
        {   
            // Arrange
            var example = PrimitiveClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<PrimitiveClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }           

        [Fact]
        public void ToObject_DictionaryClass()
        {   
            // Arrange
            var example = DictionaryClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<DictionaryClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }      

        [Fact]
        public void ToObject_EnumClass()
        {   
            // Arrange
            var example = EnumClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<EnumClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }     

        [Fact]
        public void ToObject_DateTimeClass()
        {   
            // Arrange
            var example = DateTimeClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<DateTimeClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }   

        [Fact]
        public void ToObject_StructClass()
        {   
            // Arrange
            var example = StructClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<StructClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }     

        [Fact]
        public void ToObject_RecordClass()
        {   
            // Arrange
            var example = RecordClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<RecordClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }     

        [Fact]
        public void ToObject_ParsableClass()
        {   
            // Arrange
            var example = ParsableClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<ParsableClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }  

        [Fact]
        public void ToObject_PathClass()
        {   
            // Arrange
            var example = PathClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<PathClass>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }    

        [Fact]
        public void ToObject_ClassWithoutParameterlessConstructor()
        {   
            // Arrange
            var example = ClassWithoutParameterlessConstructor.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<ClassWithoutParameterlessConstructor>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }   

        [Fact]
        public void ToObject_ClassRecordNoParameterlessConstructor()
        {   
            // Arrange
            var example = ClassRecordNoParameterlessConstructor.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<ClassRecordNoParameterlessConstructor>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }       

        [Fact]
        public void ToObject_PrimitiveClassIEnumarable()
        {   
            // Arrange
            var example = PrimitiveClassIEnumarable.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<PrimitiveClassIEnumarable>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }    

        [Fact]
        public void ToObject_MultiDimClassIEnumarble()
        {   
            // Arrange
            var example = MultiDimClassIEnumarble.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.ToJsonString();

            // Act
            var result = DSJsonNode.ToObject<MultiDimClassIEnumarble>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }                                                                                                                           
     
    }
}