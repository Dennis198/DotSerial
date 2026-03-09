using DotSerial.Common;
using DotSerial.Yaml;

namespace DotSerial.Tests.Yaml
{
    public class DotSerialYamlTests
    {

        [Fact]
        public void Save_True()
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();
            var yamlDocument = DotSerialYaml.Serialize(testDefault);

            using var file = new TemporaryFile();
            // Act
            DotSerialYaml.SaveToFile(file.FileInfo.FullName, yamlDocument);
        }

        [Fact]
        public void Save_Direct_True()
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();

            using var file = new TemporaryFile();
            // Act
            DotSerialYaml.SaveToFile(file.FileInfo.FullName, testDefault);
        }

        [Fact]
        public void Load_True()
        {
            // Arrange
            PrimitiveClass? tmp = null;
            var expected = PrimitiveClass.CreateTestDefault();
            string path = Directory.GetCurrentDirectory();
            path = Path.GetFullPath(Path.Combine(path, string.Format("..{0}..{0}..", Path.DirectorySeparatorChar) ));
            path = Path.Combine(path,string.Format("Resources{0}YamlTest.yml", Path.DirectorySeparatorChar));

            try
            {
                // Act
                tmp = DotSerialYaml.LoadFromFile<PrimitiveClass>(path);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            // Assert
            Assert.NotNull(tmp);
            Assert.True(tmp.AssertTest(expected));
        }

        [Fact]
        public void CreateSerializedObject_Primitive()
        {
            // Arrange
            double tmp = 1234.45;

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<double>(yamlDocument);

            // Assert
            Assert.Equal(tmp, result); 
        }    

        [Fact]
        public void CreateSerializedObject_Primitive_Null()
        {
            // Arrange
            string? tmp = null;

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<string?>(yamlDocument);

            // Assert
            Assert.Null(result); 
        }     

        [Fact]
        public void List()
        {
            // Arrange
            double[] tmp = [1.1, 2.2, 3.3, 4.4];

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<double[]>(yamlDocument);

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

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<double[]>(yamlDocument);

            // Assert
            Assert.Null(result);           
        }    

        [Fact]
        public void Dictionary()
        {
            // Arrange
            Dictionary<string, double> tmp = [];
            tmp.Add("test1", 1.1);
            tmp.Add("test2", 2.2);

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<Dictionary<string, double>>(yamlDocument);

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

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<Dictionary<string, double>?>(yamlDocument);

            // Assert
            Assert.Null(result);              
        }           

        [Fact]
        public void CreateSerializedObject_EmptyClass()
        {
            // Arrange
            var tmp = new EmptyClass();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<EmptyClass>(yamlDocument);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateSerializedObject_AccessModifierClass()
        {
            // Arrange
            var tmp = AccessModifierClass.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<AccessModifierClass>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_DictionaryClass()
        {
            // Arrange
            var tmp = DictionaryClass.CreateTestDefault();


            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<DictionaryClass>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_StructClass()
        {
            // Arrange
            var tmp = StructClass.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<StructClass>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_RecordClass()
        {
            // Arrange
            var tmp = RecordClass.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<RecordClass>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_ParsableClass()
        {
            // Arrange
            var tmp = ParsableClass.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<ParsableClass>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_PathClass()
        {
            // Arrange
            var tmp = PathClass.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<PathClass>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_ClassWithoutParameterlessConstructor()
        {
            // Arrange
            var tmp = ClassWithoutParameterlessConstructor.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<ClassWithoutParameterlessConstructor>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_PrimitiveClass()
        {
            // Arrange
            var tmp = PrimitiveClass.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<PrimitiveClass>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_NestedClass()
        {
            // Arrange
            var tmp2 = PrimitiveClass.CreateTestDefault();
            var tmp = new NestedClass
            {
                Boolean = true,
                PrimitiveClass = tmp2
            };

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<NestedClass>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_NestedNestedClass()
        {
            //Arrange
            var tmp3 = PrimitiveClass.CreateTestDefault();
            var tmp4 = new PrimitiveClass
            {
                Boolean = true,
                Byte = 1,
                SByte = 2,
                Char = 'e',
                Decimal = 3,
                Double = 4.9,
                Float = 5.8F,
                Int = 6,
                UInt = 7,
                NInt = 8,
                NUInt = 9,
                Long = 10,
                ULong = 11,
                Short = 12,
                UShort = 13,
                String = "HelloWorld",
                Enum = TestEnum.Second
            };
            var tmp2 = new NestedClass
            {
                Boolean = true,
                PrimitiveClass = tmp3
            };
            var tmp = new NestedNestedClass
            {
                NestedClass = tmp2,
                PrimitiveClass = tmp4,
                Boolean = true
            };

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<NestedNestedClass>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_EnumClass()
        {
            // Arrange
            var tmp = EnumClass.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<EnumClass>(yamlDocument);


            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_NoAttribute()
        {
            // Arrange
            var tmp = NoAttributeClass.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<NoAttributeClass>(yamlDocument);

            // Arrange
            Assert.NotNull(result);
            //HelperMethods.AssertClassEqual(tmp, output); // TODO
        }

        [Fact]
        public void CreateSerializedObject_MultiDimClassIEnumarble()
        {
            // Arrange
            var tmp = MultiDimClassIEnumarble.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<MultiDimClassIEnumarble>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_NullClass()
        {
            // Arrange
            var tmp = NullClass.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<NullClass>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));

        }

        [Fact]
        public void CreateSerializedObject_PrimitiveClassIEnumarable()
        {
            // Arrange
            var tmp = PrimitiveClassIEnumarable.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<PrimitiveClassIEnumarable>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_DateTimeClass()
        {
            // Note: Test is failing because DateTime is not parsed correctly (last digits differ)
            // Expected: 9999-12-31T23:59:59.0000000
            // Actual: 9999 - 12 - 31T23: 59:59.9999999

            // Arrange
            var tmp = DateTimeClass.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<DateTimeClass>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_IEnumerableClass()
        {
            // Arrange
            var tmp = IEnumerableClass.CreateTestDefault();

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<IEnumerableClass>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_ClassRecordNoParameterlessConstructor()
        {
            // Arrange
            var tmp = new ClassRecordNoParameterlessConstructor
            {
                Value0 = new TestRecordNoParameterlessConstructor(5, 7)
            };

            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<ClassRecordNoParameterlessConstructor>(yamlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_DuplicateIDClass()
        {
            // Arrange
            var tmp = new DuplicateIDClass();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialYaml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeHashSet()
        {
            // Arrange
            var tmp = new HashSetClassNotSupported();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialYaml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_StackClass()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassStack();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialYaml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassHashTable()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassHashTable
            {
                Value0 = []
            };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialYaml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassStack()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassStack
            {
                Value0 = new Stack<int>()
            };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialYaml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassQueue()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassQueue
            {
                Value0 = new Queue<int>()
            };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialYaml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassLinkedList()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassLinkedList
            {
                Value0 = new LinkedList<int>()
            };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialYaml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassObservableCollection()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassObservableCollection
            {
                Value0 = []
            };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialYaml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassSortedList()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassSortedList
            {
                Value0 = []
            };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialYaml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassSortedSet()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassSortedSet
            {
                Value0 = []
            };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialYaml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_Null()
        {
            // Arrange
            object? tmp = null;
           
            // Act
            var yamlDocument = DotSerialYaml.Serialize(tmp);
            var result = DotSerialYaml.Deserialize<object?>(yamlDocument);

            // Assert
            Assert.Null(result); 
        }

        [Fact]
        public void CreateSerializedObject_InvalidIDException()
        {
            // Arrange
            InvalidIDClass tmp = new();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialYaml.Serialize(tmp));
        }

        [Fact]
        public void ToString_Content()
        {
            // Arrange
            var tmp = ExampleClass.CreateTestDefault();
            var yaml = DotSerialYaml.Serialize(tmp);

            // Act
            string result = yaml.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public void ToString_NoContent()
        {
            // Arrange
            var yaml = new DotSerialYaml();

            // Act
            string result = yaml.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrWhiteSpace(result));
        }
    }
}