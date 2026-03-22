using DotSerial.Common;
using DotSerial.Tree.Creation;

namespace DotSerial.Tests
{
    public class DSConverterTests
    {
        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void Save_True(StategyType strategy)
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();
            var tmp = DSConverter.Serialize(testDefault, strategy);

            using var file = new TemporaryFile();
            // Act
            DSConverter.SaveToFile(file.FileInfo.FullName, tmp, strategy);
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void Save_Direct_True(StategyType strategy)
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();

            using var file = new TemporaryFile();
            // Act
            DSConverter.SaveToFile(file.FileInfo.FullName, testDefault, strategy);
        }

        [Theory]
        [InlineData(StategyType.Json, "Resources{0}JsonTest.json")]
        [InlineData(StategyType.Toon, "Resources{0}ToonTest.toon")]
        [InlineData(StategyType.Xml, "Resources{0}XmlTest.xml")]
        [InlineData(StategyType.Yaml, "Resources{0}YamlTest.yml")]
        public void Load_True(StategyType strategy, string name)
        {
            // Arrange
            PrimitiveClass? tmp = null;
            var expected = PrimitiveClass.CreateTestDefault();
            string path = Directory.GetCurrentDirectory();
            path = Path.GetFullPath(Path.Combine(path, string.Format("..{0}..{0}..", Path.DirectorySeparatorChar)));
            path = Path.Combine(path, string.Format(name, Path.DirectorySeparatorChar));

            try
            {
                // Act
                tmp = DSConverter.LoadFromFile<PrimitiveClass>(path, strategy);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            // Assert
            Assert.NotNull(tmp);
            Assert.True(tmp.AssertTest(expected));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_Primitive(StategyType strategy)
        {
            // Arrange
            double tmp = 1234.45;

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<double>(serializedObject, strategy);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_Primitive_Null(StategyType strategy)
        {
            // Arrange
            string? tmp = null;

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<string?>(serializedObject, strategy);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void List(StategyType strategy)
        {
            // Arrange
            double[] tmp = [1.1, 2.2, 3.3, 4.4];

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<double[]>(serializedObject, strategy);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(tmp.Length, result.Length);
            Assert.Equal(tmp[0], result[0]);
            Assert.Equal(tmp[1], result[1]);
            Assert.Equal(tmp[2], result[2]);
            Assert.Equal(tmp[3], result[3]);
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void List_Null(StategyType strategy)
        {
            // Arrange
            double[]? tmp = null;

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<double[]>(serializedObject, strategy);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void Dictionary(StategyType strategy)
        {
            // Arrange
            Dictionary<string, double> tmp = [];
            tmp.Add("test1", 1.1);
            tmp.Add("test2", 2.2);

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<Dictionary<string, double>>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(tmp.Count, result.Count);
            Assert.Equal(tmp["test1"], result["test1"]);
            Assert.Equal(tmp["test2"], result["test2"]);
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void Dictionary_Null(StategyType strategy)
        {
            // Arrange
            Dictionary<string, double>? tmp = null;

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<Dictionary<string, double>?>(serializedObject, strategy);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_EmptyClass(StategyType strategy)
        {
            // Arrange
            var tmp = new EmptyClass();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<EmptyClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_AccessModifierClass(StategyType strategy)
        {
            // Arrange
            var tmp = AccessModifierClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<AccessModifierClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_DictionaryClass(StategyType strategy)
        {
            // Arrange
            var tmp = DictionaryClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<DictionaryClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_StructClass(StategyType strategy)
        {
            // Arrange
            var tmp = StructClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<StructClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_RecordClass(StategyType strategy)
        {
            // Arrange
            var tmp = RecordClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<RecordClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_ParsableClass(StategyType strategy)
        {
            // Arrange
            var tmp = ParsableClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<ParsableClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_PathClass(StategyType strategy)
        {
            // Arrange
            var tmp = PathClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<PathClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_ClassWithoutParameterlessConstructor(StategyType strategy)
        {
            // Arrange
            var tmp = ClassWithoutParameterlessConstructor.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<ClassWithoutParameterlessConstructor>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_PrimitiveClass(StategyType strategy)
        {
            // Arrange
            var tmp = PrimitiveClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<PrimitiveClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_NestedClass(StategyType strategy)
        {
            // Arrange
            var tmp = NestedClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<NestedClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_NestedNestedClass(StategyType strategy)
        {
            var tmp = NestedNestedClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<NestedNestedClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_EnumClass(StategyType strategy)
        {
            // Arrange
            var tmp = EnumClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<EnumClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_NoAttribute(StategyType strategy)
        {
            // Arrange
            var tmp = NoAttributeClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<NoAttributeClass>(serializedObject, strategy);

            // Arrange
            Assert.NotNull(result);
            //HelperMethods.AssertClassEqual(tmp, output); // TODO
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_MultiDimClassIEnumarble(StategyType strategy)
        {
            // Arrange
            var tmp = MultiDimClassIEnumarble.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<MultiDimClassIEnumarble>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_NullClass(StategyType strategy)
        {
            // Arrange
            var tmp = NullClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<NullClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_PrimitiveClassIEnumarable(StategyType strategy)
        {
            // Arrange
            var tmp = PrimitiveClassIEnumarable.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<PrimitiveClassIEnumarable>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_DateTimeClass(StategyType strategy)
        {
            // Note: Test is failing because DateTime is not parsed correctly (last digits differ)
            // Expected: 9999-12-31T23:59:59.0000000
            // Actual: 9999 - 12 - 31T23: 59:59.9999999

            // Arrange
            var tmp = DateTimeClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<DateTimeClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_IEnumerableClass(StategyType strategy)
        {
            // Arrange
            var tmp = IEnumerableClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<IEnumerableClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_ClassRecordNoParameterlessConstructor(StategyType strategy)
        {
            // Arrange
            var tmp = new ClassRecordNoParameterlessConstructor
            {
                Value0 = new TestRecordNoParameterlessConstructor(5, 7),
            };

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<ClassRecordNoParameterlessConstructor>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_DuplicateIDClass(StategyType strategy)
        {
            // Arrange
            var tmp = new DuplicateIDClass();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DSConverter.Serialize(tmp, strategy));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_NotSupportedTypeHashSet(StategyType strategy)
        {
            // Arrange
            var tmp = new HashSetClassNotSupported();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DSConverter.Serialize(tmp, strategy));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_StackClass(StategyType strategy)
        {
            // Arrange
            var tmp = new NotSupportedTypeClassStack();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DSConverter.Serialize(tmp, strategy));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_NotSupportedTypeClassHashTable(StategyType strategy)
        {
            // Arrange
            var tmp = new NotSupportedTypeClassHashTable { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DSConverter.Serialize(tmp, strategy));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_NotSupportedTypeClassStack(StategyType strategy)
        {
            // Arrange
            var tmp = new NotSupportedTypeClassStack { Value0 = new Stack<int>() };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DSConverter.Serialize(tmp, strategy));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_NotSupportedTypeClassQueue(StategyType strategy)
        {
            // Arrange
            var tmp = new NotSupportedTypeClassQueue { Value0 = new Queue<int>() };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DSConverter.Serialize(tmp, strategy));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_NotSupportedTypeClassLinkedList(StategyType strategy)
        {
            // Arrange
            var tmp = new NotSupportedTypeClassLinkedList { Value0 = new LinkedList<int>() };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DSConverter.Serialize(tmp, strategy));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_NotSupportedTypeClassObservableCollection(StategyType strategy)
        {
            // Arrange
            var tmp = new NotSupportedTypeClassObservableCollection { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DSConverter.Serialize(tmp, strategy));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_NotSupportedTypeClassSortedList(StategyType strategy)
        {
            // Arrange
            var tmp = new NotSupportedTypeClassSortedList { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DSConverter.Serialize(tmp, strategy));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_NotSupportedTypeClassSortedSet(StategyType strategy)
        {
            // Arrange
            var tmp = new NotSupportedTypeClassSortedSet { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DSConverter.Serialize(tmp, strategy));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_Null(StategyType strategy)
        {
            // Arrange
            object? tmp = null;

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<object?>(serializedObject, strategy);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void CreateSerializedObject_InvalidIDException(StategyType strategy)
        {
            // Arrange
            InvalidIDClass tmp = new();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DSConverter.Serialize(tmp, strategy));
        }

        [Theory]
        [InlineData(StategyType.Json)]
        [InlineData(StategyType.Toon)]
        [InlineData(StategyType.Xml)]
        [InlineData(StategyType.Yaml)]
        public void ToString_Content(StategyType strategy)
        {
            // Arrange
            var tmp = ExampleClass.CreateTestDefault();
            var serializedObject = DSConverter.Serialize(tmp, strategy);

            // Act
            string result = serializedObject.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        //         [Theory]
        // [InlineData(StategyType.Json)]
        // [InlineData(StategyType.Toon)]
        // [InlineData(StategyType.Xml)]
        // [InlineData(StategyType.Yaml)]
        // public void ToString_NoContent(StategyType strategy)
        // {
        //     // Arrange
        //     var json = new DotSerialJson();

        //     // Act
        //     string result = json.ToString();

        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.True(string.IsNullOrWhiteSpace(result));
        // }
    }
}
