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

using DotSerial.Json;
using DotSerial.Tree;
using DotSerial.Tree.Creation;

namespace DotSerial.Tests.Json
{
    public class DSJsonNodeTests
    {
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        [Fact]
        public void Create()
        {
            // Arrange
            var tmp = _nodeFactory.CreateNode(StategyType.Json, "key", "value", NodeType.Leaf);

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
            var tmp = _nodeFactory.CreateNode(StategyType.Json, "child", "value", NodeType.Leaf);
            var tmp2 = _nodeFactory.CreateNode(StategyType.Json, "key", null, NodeType.InnerNode);
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
        public void Stringify()
        {
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);

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
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.Stringify();

            // Act
            var result = DSJsonNode.FromString(jsonString);

            // Assert
            Assert.NotNull(result);
        }            

        [Fact]
        public void Primitive()
        {
            // Arrange
            double tmp = 1234.45;
            var node = DSJsonNode.ToNode(tmp);
            string jsonString = node.Stringify();

            // Act
            var result = DSJsonNode.ToObject<double>(jsonString);

            // Assert
            Assert.Equal(tmp, result);         
        }

        [Fact]
        public void Primitive_Null()
        {
            // Arrange
            string? tmp = null;
            var node = DSJsonNode.ToNode(tmp);
            string jsonString = node.Stringify();

            // Act
            var result = DSJsonNode.ToObject<string>(jsonString);

            // Assert
            Assert.Equal(tmp, result);         
        }      

        [Fact]
        public void Primitive_Empty()
        {
            // Arrange
            string? tmp = string.Empty;
            var node = DSJsonNode.ToNode(tmp);
            string jsonString = node.Stringify();

            // Act
            var result = DSJsonNode.ToObject<string>(jsonString);

            // Assert
            Assert.Equal(tmp, result);         
        }               

        [Fact]
        public void List()
        {
            // Arrange
            double[] tmp = [1.1, 2.2, 3.3, 4.4];
            var node = DSJsonNode.ToNode(tmp);
            string jsonString = node.Stringify();

            // Act
            var result = DSJsonNode.ToObject<double[]>(jsonString);

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
            var node = DSJsonNode.ToNode(tmp);
            string jsonString = node.Stringify();

            // Act
            var result = DSJsonNode.ToObject<double[]>(jsonString);

            // Assert
            Assert.Null(result);           
        }        

        [Fact]
        public void List_Empty()
        {
            // Arrange
            double[]? tmp = [];
            var node = DSJsonNode.ToNode(tmp);
            string jsonString = node.Stringify();

            // Act
            var result = DSJsonNode.ToObject<double[]>(jsonString);

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
            var node = DSJsonNode.ToNode(tmp);
            string jsonString = node.Stringify();

            // Act
            var result = DSJsonNode.ToObject<Dictionary<string, double>?>(jsonString);

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
            var node = DSJsonNode.ToNode(tmp);
            string jsonString = node.Stringify();

            // Act
            var result = DSJsonNode.ToObject<Dictionary<string, double>?>(jsonString);

            // Assert
            Assert.Null(result);              
        } 

        [Fact]
        public void Dictionary_Empty()
        {
            // Arrange
            Dictionary<string, double> tmp = [];
            var node = DSJsonNode.ToNode(tmp);
            string jsonString = node.Stringify();

            // Act
            var result = DSJsonNode.ToObject<Dictionary<string, double>>(jsonString);

            // Assert
            Assert.Empty(result);              
        }                   

        [Fact]
        public void ToObject_ExampleClass()
        {   
            // Arrange
            var example = ExampleClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

            // Act
            var result = DSJsonNode.ToObject<EmptyClass>(jsonString);

            // Assert
            Assert.NotNull(result);
        }   

        [Fact]
        public void ToObject_ListFirstElementNull()
        {   
            // Arrange
            var example = ListFirstElementNull.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.Stringify();

            // Act
            var result = DSJsonNode.ToObject<ListFirstElementNull>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }          

        [Fact]
        public void ToObject_NoAttributeClass()
        {   
            // Arrange
            var example = NoAttributeClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();
            
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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

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
            var jsonString = tmp.Stringify();

            // Act
            var result = DSJsonNode.ToObject<MultiDimClassIEnumarble>(jsonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }    

        [Fact]
        public void ToObject_EmptyObjectClass()
        {   
            // Arrange
            var example = EmptyObjectClass.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSJsonNode.ToObject<EmptyObjectClass>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }              

        [Fact]
        public void ToObject_ClassWithOneList()
        {   
            // Arrange
            var example = ClassWithOneList.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSJsonNode.ToObject<ClassWithOneList>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }      

        [Fact]
        public void ToObject_ClassWithOneDictionary()
        {   
            // Arrange
            var example = ClassWithOneDictionary.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSJsonNode.ToObject<ClassWithOneDictionary>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }              

        [Fact]
        public void ToObject_ClassWithOnePrimitive()
        {   
            // Arrange
            var example = ClassWithOnePrimitive.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var toonString = tmp.Stringify();

            // Act
            var result = DSJsonNode.ToObject<ClassWithOnePrimitive>(toonString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }    

        [Fact]
        public void ToObject_ClassSpecialCharsKeys()
        {   
            // Arrange
            var example = ClassSpecialCharsKeys.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var resultString = tmp.Stringify();

            // Act
            var result = DSJsonNode.ToObject<ClassSpecialCharsKeys>(resultString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }   

        [Fact]
        public void ToObject_ClassSpecialCharsValue()
        {   
            // Arrange
            var example = ClassSpecialCharsValue.CreateTestDefault();
            var tmp = DSJsonNode.ToNode(example);
            var resultString = tmp.Stringify();

            // Act
            var result = DSJsonNode.ToObject<ClassSpecialCharsValue>(resultString);

            // Assert
            Assert.NotNull(result);
            Assert.True(example.AssertTest(result));
        }                                                                                                                                                 
     
    }
}