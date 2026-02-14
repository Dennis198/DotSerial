using DotSerial.Toon;
using DotSerial.Tree;

namespace DotSerial.Tests.Toon
{
    public class DSToonNodeTests
    {
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;               

        [Fact]
        public void Create()
        {
            // Arrange
            var tmp = _nodeFactory.CreateNode("key", "value", NodeType.Leaf);

            // Act
            var result = new DSToonNode(tmp);

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
            var resultNode = new DSToonNode(tmp2);

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
            var result = DSToonNode.ToNode(example);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Stringify()
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);

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
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.FromString(toonString);

            // Assert
            Assert.NotNull(result);
        }            

        [Fact]
        public void Primitive()
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSToonNode.ToNode(tmp);
            string toonString = node.Stringify();

            // Act
            var result = DSToonNode.ToObject<double>(toonString);

            // Assert
            Assert.Equal(tmp, result);         
        }

        [Fact]
        public void Primitive_Null()
        {
            // Arrange
            string? tmp = null;
            var node = DSToonNode.ToNode(tmp);
            string toonString = node.Stringify();

            // Act
            var result = DSToonNode.ToObject<string>(toonString);

            // Assert
            Assert.Equal(tmp, result);         
        }      

        [Fact]
        public void Primitive_Empty()
        {
            // Arrange
            string? tmp = string.Empty;
            var node = DSToonNode.ToNode(tmp);
            string toonString = node.Stringify();

            // Act
            var result = DSToonNode.ToObject<string>(toonString);

            // Assert
            Assert.Equal(tmp, result);         
        }               

        [Fact]
        public void List()
        {
            // Arrange
            double[] tmp = [1.1, 2.2, 3.3, 4.4];
            var node = DSToonNode.ToNode(tmp);
            string toonString = node.Stringify();

            // Act
            var result = DSToonNode.ToObject<double[]>(toonString);

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
            var node = DSToonNode.ToNode(tmp);
            string toonString = node.Stringify();

            // Act
            var result = DSToonNode.ToObject<double[]>(toonString);

            // Assert
            Assert.Null(result);           
        }        

        [Fact]
        public void List_Empty()
        {
            // Arrange
            double[]? tmp = [];
            var node = DSToonNode.ToNode(tmp);
            string toonString = node.Stringify();

            // Act
            var result = DSToonNode.ToObject<double[]>(toonString);

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
            var node = DSToonNode.ToNode(tmp);
            string toonString = node.Stringify();

            // Act
            var result = DSToonNode.ToObject<Dictionary<string, double>?>(toonString);

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
            var node = DSToonNode.ToNode(tmp);
            string toonString = node.Stringify();

            // Act
            var result = DSToonNode.ToObject<Dictionary<string, double>?>(toonString);

            // Assert
            Assert.Null(result);              
        } 

        [Fact]
        public void Dictionary_Empty()
        {
            // Arrange
            Dictionary<string, double> tmp = [];
            var node = DSToonNode.ToNode(tmp);
            string toonString = node.Stringify();

            // Act
            var result = DSToonNode.ToObject<Dictionary<string, double>>(toonString);

            // Assert
            Assert.Empty(result);              
        }                   

        [Fact]
        public void ToObject_ExampleClass()
        {   
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<ExampleClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_EmptyClass()
        {   
            // Arrange
            var example = new EmptyClass();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<EmptyClass>(toonString);

            // Assert
            Assert.NotNull(result);
        }   

        [Fact]
        public void ToObject_ListFirstElementNull()
        {   
            // Arrange
            var example = ListFirstElementNull.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<ListFirstElementNull>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }          

        [Fact]
        public void ToObject_NoAttributeClass()
        {   
            // Arrange
            var example = NoAttributeClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<NoAttributeClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }  

        [Fact]
        public void ToObject_AccessModifierClass()
        {   
            // Arrange
            var example = AccessModifierClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<AccessModifierClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }  

        [Fact]
        public void ToObject_SimpleClass()
        {   
            // Arrange
            var example = SimpleClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<SimpleClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }      

        [Fact]
        public void ToObject_NullClass()
        {   
            // Arrange
            var example = NullClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<NullClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }  

        [Fact]
        public void ToObject_IEnumerableClass()
        {   
            // Arrange
            var example = IEnumerableClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();
            
            // Act
            var result = DSToonNode.ToObject<IEnumerableClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }      

        [Fact]
        public void ToObject_NestedClasss()
        {   
            // Arrange
            var example = NestedClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<NestedClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }    

        [Fact]
        public void ToObject_NestedNestedClass()
        {   
            // Arrange
            var example = NestedNestedClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<NestedNestedClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }     

        [Fact]
        public void ToObject_PrimitiveClass()
        {   
            // Arrange
            var example = PrimitiveClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<PrimitiveClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }           

        [Fact]
        public void ToObject_DictionaryClass()
        {   
            // Arrange
            var example = DictionaryClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<DictionaryClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }      

        [Fact]
        public void ToObject_EnumClass()
        {   
            // Arrange
            var example = EnumClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<EnumClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }     

        [Fact]
        public void ToObject_DateTimeClass()
        {   
            // Arrange
            var example = DateTimeClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<DateTimeClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }   

        [Fact]
        public void ToObject_StructClass()
        {   
            // Arrange
            var example = StructClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<StructClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }     

        [Fact]
        public void ToObject_RecordClass()
        {   
            // Arrange
            var example = RecordClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<RecordClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }     

        [Fact]
        public void ToObject_ParsableClass()
        {   
            // Arrange
            var example = ParsableClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<ParsableClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }  

        [Fact]
        public void ToObject_PathClass()
        {   
            // Arrange
            var example = PathClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<PathClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }    

        [Fact]
        public void ToObject_ClassWithoutParameterlessConstructor()
        {   
            // Arrange
            var example = ClassWithoutParameterlessConstructor.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<ClassWithoutParameterlessConstructor>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }   

        [Fact]
        public void ToObject_ClassRecordNoParameterlessConstructor()
        {   
            // Arrange
            var example = ClassRecordNoParameterlessConstructor.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<ClassRecordNoParameterlessConstructor>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }       

        [Fact]
        public void ToObject_PrimitiveClassIEnumarable()
        {   
            // Arrange
            var example = PrimitiveClassIEnumarable.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<PrimitiveClassIEnumarable>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }    

        [Fact]
        public void ToObject_MultiDimClassIEnumarble()
        {   
            // Arrange
            var example = MultiDimClassIEnumarble.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<MultiDimClassIEnumarble>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }            

        [Fact]
        public void ToObject_EmptyObjectClass()
        {   
            // Arrange
            var example = EmptyObjectClass.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<EmptyObjectClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }              

        [Fact]
        public void ToObject_ClassWithOneList()
        {   
            // Arrange
            var example = ClassWithOneList.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<ClassWithOneList>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }      

        [Fact]
        public void ToObject_ClassWithOneDictionary()
        {   
            // Arrange
            var example = ClassWithOneDictionary.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<ClassWithOneDictionary>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }              

        [Fact]
        public void ToObject_ClassWithOnePrimitive()
        {   
            // Arrange
            var example = ClassWithOnePrimitive.CreateTestDefault();
            var tmp = DSToonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSToonNode.ToObject<ClassWithOnePrimitive>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }           
    }
}