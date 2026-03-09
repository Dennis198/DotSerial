using DotSerial.Yaml;
using DotSerial.Tree;
using DotSerial.Tree.Creation;

namespace DotSerial.Tests.Yaml
{
    public class DSYamlNodeTests
    {
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;
        
        [Fact]
        public void Create()
        {
            // Arrange
            var tmp = _nodeFactory.CreateNode(StategyType.Yaml, "key", "value", NodeType.Leaf);

            // Act
            var result = new DSYamlNode(tmp);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("key", result.Key);
            Assert.False(result.HasChildren);
        }

        [Fact]
        public void GetChild()
        {
            // Arrange
            var tmp = _nodeFactory.CreateNode(StategyType.Yaml, "child", "value", NodeType.Leaf);
            var tmp2 = _nodeFactory.CreateNode(StategyType.Yaml, "key", null, NodeType.InnerNode);
            tmp2.AddChild(tmp);
            var resultNode = new DSYamlNode(tmp2);

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
            var result = DSYamlNode.ToNode(example);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Stringify()
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);

            // Act
            var result = tmp.Stringify();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void FromYamlString()
        {
            // Arrange
            var example = PrimitiveClassIEnumarable.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.FromString(yamlString);

            // Assert
            Assert.NotNull(result);
        }        

       [Fact]
        public void Primitive()
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSYamlNode.ToNode(tmp);
            string yamlString = node.Stringify();

            // Act
            var result = DSYamlNode.ToObject<double>(yamlString);

            // Assert
            Assert.Equal(tmp, result);         
        }

        [Fact]
        public void Primitive_Null()
        {
            // Arrange
            string? tmp = null;
            var node = DSYamlNode.ToNode(tmp);
            string yamlString = node.Stringify();

            // Act
            var result = DSYamlNode.ToObject<string>(yamlString);

            // Assert
            Assert.Equal(tmp, result);         
        }      

        [Fact]
        public void Primitive_Empty()
        {
            // Arrange
            string? tmp = string.Empty;
            var node = DSYamlNode.ToNode(tmp);
            string yamlString = node.Stringify();

            // Act
            var result = DSYamlNode.ToObject<string>(yamlString);

            // Assert
            Assert.Equal(tmp, result);         
        }        

        [Fact]
        public void PrimitiveSpecialChar()
        {
            // Arrange
            string tmp = "{{}<><:;[[]-?!#!";
            var node = DSYamlNode.ToNode(tmp);
            string yamlString = node.Stringify();

            // Act
            var result = DSYamlNode.ToObject<string>(yamlString);

            // Assert
            Assert.Equal(tmp, result);         
        }                 

        [Fact]
        public void List()
        {
            // Arrange
            double[] tmp = [1.1, 2.2, 3.3, 4.4];
            var node = DSYamlNode.ToNode(tmp);
            string yamlString = node.Stringify();

            // Act
            var result = DSYamlNode.ToObject<double[]>(yamlString);

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
            var node = DSYamlNode.ToNode(tmp);
            string yamlString = node.Stringify();

            // Act
            var result = DSYamlNode.ToObject<double[]>(yamlString);

            // Assert
            Assert.Null(result);           
        }        

        [Fact]
        public void List_Empty()
        {
            // Arrange
            double[]? tmp = [];
            var node = DSYamlNode.ToNode(tmp);
            string yamlString = node.Stringify();

            // Act
            var result = DSYamlNode.ToObject<double[]>(yamlString);

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
            var node = DSYamlNode.ToNode(tmp);
            string yamlString = node.Stringify();

            // Act
            var result = DSYamlNode.ToObject<Dictionary<string, double>?>(yamlString);

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
            var node = DSYamlNode.ToNode(tmp);
            string yamlString = node.Stringify();

            // Act
            var result = DSYamlNode.ToObject<Dictionary<string, double>?>(yamlString);

            // Assert
            Assert.Null(result);              
        } 

        [Fact]
        public void Dictionary_Empty()
        {
            // Arrange
            Dictionary<string, double> tmp = [];
            var node = DSYamlNode.ToNode(tmp);
            string yamlString = node.Stringify();

            // Act
            var result = DSYamlNode.ToObject<Dictionary<string, double>>(yamlString);

            // Assert
            Assert.Empty(result);              
        }                 

        [Fact]
        public void ToObject_ExampleClass()
        {   
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<ExampleClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }

        [Fact]
        public void ToObject_EmptyClass()
        {   
            // Arrange
            var example = new EmptyClass();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<EmptyClass>(yamlString);

            // Assert
            Assert.NotNull(result);
        }   

        [Fact]
        public void ToObject_ListFirstElementNull()
        {   
            // Arrange
            var example = ListFirstElementNull.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<ListFirstElementNull>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }           

        [Fact]
        public void ToObject_NoAttributeClass()
        {   
            // Arrange
            var example = NoAttributeClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<NoAttributeClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }  

        [Fact]
        public void ToObject_AccessModifierClass()
        {   
            // Arrange
            var example = AccessModifierClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<AccessModifierClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }  

        [Fact]
        public void ToObject_SimpleClass()
        {   
            // Arrange
            var example = SimpleClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<SimpleClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }      

        [Fact]
        public void ToObject_NullClass()
        {   
            // Arrange
            var example = NullClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<NullClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }  

        [Fact]
        public void ToObject_IEnumerableClass()
        {   
            // Arrange
            var example = IEnumerableClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();
            
            // Act
            var result = DSYamlNode.ToObject<IEnumerableClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }      

        [Fact]
        public void ToObject_NestedClasss()
        {   
            // Arrange
            var example = NestedClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<NestedClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }    

        [Fact]
        public void ToObject_NestedNestedClass()
        {   
            // Arrange
            var example = NestedNestedClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<NestedNestedClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }     

        [Fact]
        public void ToObject_PrimitiveClass()
        {   
            // Arrange
            var example = PrimitiveClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<PrimitiveClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }           

        [Fact]
        public void ToObject_DictionaryClass()
        {   
            // Arrange
            var example = DictionaryClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<DictionaryClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }      

        [Fact]
        public void ToObject_EnumClass()
        {   
            // Arrange
            var example = EnumClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<EnumClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }     

        [Fact]
        public void ToObject_DateTimeClass()
        {   
            // Arrange
            var example = DateTimeClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<DateTimeClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }   

        [Fact]
        public void ToObject_StructClass()
        {   
            // Arrange
            var example = StructClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<StructClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }     

        [Fact]
        public void ToObject_RecordClass()
        {   
            // Arrange
            var example = RecordClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<RecordClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }     

        [Fact]
        public void ToObject_ParsableClass()
        {   
            // Arrange
            var example = ParsableClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<ParsableClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }  

        [Fact]
        public void ToObject_PathClass()
        {   
            // Arrange
            var example = PathClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<PathClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }    

        [Fact]
        public void ToObject_ClassWithoutParameterlessConstructor()
        {   
            // Arrange
            var example = ClassWithoutParameterlessConstructor.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<ClassWithoutParameterlessConstructor>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }   

        [Fact]
        public void ToObject_ClassRecordNoParameterlessConstructor()
        {   
            // Arrange
            var example = ClassRecordNoParameterlessConstructor.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<ClassRecordNoParameterlessConstructor>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }       

        [Fact]
        public void ToObject_PrimitiveClassIEnumarable()
        {   
            // Arrange
            var example = PrimitiveClassIEnumarable.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<PrimitiveClassIEnumarable>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }    

        [Fact]
        public void ToObject_MultiDimClassIEnumarble()
        {   
            // Arrange
            var example = MultiDimClassIEnumarble.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<MultiDimClassIEnumarble>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }   

        [Fact]
        public void ToObject_EmptyObjectClass()
        {   
            // Arrange
            var example = EmptyObjectClass.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<EmptyObjectClass>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }              

        [Fact]
        public void ToObject_ClassWithOneList()
        {   
            // Arrange
            var example = ClassWithOneList.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<ClassWithOneList>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }      

        [Fact]
        public void ToObject_ClassWithOneDictionary()
        {   
            // Arrange
            var example = ClassWithOneDictionary.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<ClassWithOneDictionary>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }              

        [Fact]
        public void ToObject_ClassWithOnePrimitive()
        {   
            // Arrange
            var example = ClassWithOnePrimitive.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var yamlString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<ClassWithOnePrimitive>(yamlString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }               

        [Fact]
        public void ToObject_ClassSpecialCharsKeys()
        {   
            // Arrange
            var example = ClassSpecialCharsKeys.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var resultString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<ClassSpecialCharsKeys>(resultString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }   

        [Fact]
        public void ToObject_ClassSpecialCharsValue()
        {   
            // Arrange
            var example = ClassSpecialCharsValue.CreateTestDefault();
            var tmp = DSYamlNode.ToNode(example);
            var resultString = tmp.Stringify();

            // Act
            var result = DSYamlNode.ToObject<ClassSpecialCharsValue>(resultString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }                                                                                                                             
     
    }
}