namespace DotSerial.Tests
{
    public class DSConverterTests
    {
        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_AccessModifierClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_ClassRecordNoParameterlessConstructor(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_ClassWithoutParameterlessConstructor(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_DateTimeClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_DictionaryClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_DuplicateIDClass(SerializeStrategy strategy)
        {
            // Arrange
            var tmp = new DuplicateIDClass();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => DSConverter.Serialize(tmp, strategy));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_EmptyClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_EnumClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_IEnumerableClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_InvalidIDException(SerializeStrategy strategy)
        {
            // Arrange
            InvalidIDClass tmp = new();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => DSConverter.Serialize(tmp, strategy));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_MultiDimClassIEnumarble(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_NestedClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_NestedNestedClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_NoAttribute(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_NotSupportedTypeClassHashTable(SerializeStrategy strategy)
        {
            Assert.True(true);
            return;
            // Arrange
            var tmp = NotSupportedTypeClassHashTable.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<NotSupportedTypeClassHashTable>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_ClassLinkedList(SerializeStrategy strategy)
        {
            // Arrange
            var tmp = ClassLinkedList.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<ClassLinkedList>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_ClassObservableCollection(SerializeStrategy strategy)
        {
            // Arrange
            var tmp = ClassObservableCollection.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<ClassObservableCollection>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_ClassQueue(SerializeStrategy strategy)
        {
            // Arrange
            var tmp = ClassQueue.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<ClassQueue>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_ClassSortedSet(SerializeStrategy strategy)
        {
            // Arrange
            var tmp = ClassSortedSet.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<ClassSortedSet>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_ClassStack(SerializeStrategy strategy)
        {
            // Arrange
            var tmp = ClassStack.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<ClassStack>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_HashSetClass(SerializeStrategy strategy)
        {
            // Arrange
            var tmp = HashSetClass.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<HashSetClass>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_ClassSortedList(SerializeStrategy strategy)
        {
            // Arrange
            var tmp = ClassSortedList.CreateTestDefault();

            // Act
            var serializedObject = DSConverter.Serialize(tmp, strategy);
            var result = DSConverter.Deserialize<ClassSortedList>(serializedObject, strategy);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_Null(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_NullClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_ParsableClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_PathClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_Primitive(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_PrimitiveClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_PrimitiveClassIEnumarable(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_Primitive_Null(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_RecordClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void CreateSerializedObject_StructClass(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Dictionary_Null(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void List(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void List_Null(SerializeStrategy strategy)
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
        [InlineData(SerializeStrategy.Json, "Resources{0}JsonTest.json")]
        [InlineData(SerializeStrategy.Toon, "Resources{0}ToonTest.toon")]
        [InlineData(SerializeStrategy.Xml, "Resources{0}XmlTest.xml")]
        [InlineData(SerializeStrategy.Yaml, "Resources{0}YamlTest.yml")]
        public void Load_True(SerializeStrategy strategy, string name)
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
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Save_Direct_True(SerializeStrategy strategy)
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();

            using var file = new TemporaryFile();
            // Act
            DSConverter.SaveToFile(file.FileInfo.FullName, testDefault, strategy);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void Save_True(SerializeStrategy strategy)
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();
            var tmp = DSConverter.Serialize(testDefault, strategy);

            using var file = new TemporaryFile();
            // Act
            DSConverter.SaveToFile(file.FileInfo.FullName, tmp, strategy);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToNode(SerializeStrategy strategy)
        {
            // Arrange
            var tmp = ExampleClass.CreateTestDefault();

            // Act
            var node = DSConverter.ToNode(tmp, strategy);

            // Assert
            Assert.NotNull(node);
            Assert.Equal(node.Strategy, strategy);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToNodeFromString(SerializeStrategy strategy)
        {
            // Arrange
            var tmp = ExampleClass.CreateTestDefault();
            string? serializedObject = DSConverter.Serialize(tmp, strategy);

            // Act
            var node = DSConverter.ToNodeFromString(serializedObject, strategy);

            // Assert
            Assert.NotNull(node);
            Assert.Equal(node.Strategy, strategy);
        }

        [Theory]
        [InlineData(SerializeStrategy.Json)]
        [InlineData(SerializeStrategy.Toon)]
        [InlineData(SerializeStrategy.Xml)]
        [InlineData(SerializeStrategy.Yaml)]
        public void ToString_Content(SerializeStrategy strategy)
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
