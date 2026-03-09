using DotSerial.Common;
using DotSerial.Xml;

namespace DotSerial.Tests.Xml
{
    public class DotSerialXmlTests
    {
        [Fact]
        public void Save_True()
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();
            var xmlDocument = DotSerialXml.Serialize(testDefault);

            using var file = new TemporaryFile();
            // Act
            DotSerialXml.SaveToFile(file.FileInfo.FullName, xmlDocument);
        }

        [Fact]
        public void Save_Direct_True()
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();

            using var file = new TemporaryFile();
            // Act
            DotSerialXml.SaveToFile(file.FileInfo.FullName, testDefault);
        }

        [Fact]
        public void Load_True()
        {
            // Arrange
            PrimitiveClass? tmp = null;
            var expected = PrimitiveClass.CreateTestDefault();
            string path = Directory.GetCurrentDirectory();
            path = Path.GetFullPath(Path.Combine(path, string.Format("..{0}..{0}..", Path.DirectorySeparatorChar)));
            path = Path.Combine(path, string.Format("Resources{0}XmlTest.xml", Path.DirectorySeparatorChar));

            try
            {
                // Act
                tmp = DotSerialXml.LoadFromFile<PrimitiveClass>(path);
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
        public void CreateSerializedObject_EmptyClass()
        {
            // Arrange
            var tmp = new EmptyClass();

            // Act
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<EmptyClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateSerializedObject_AccessModifierClass()
        {
            // Arrange
            var tmp = AccessModifierClass.CreateTestDefault();

            // Act
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<AccessModifierClass>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<DictionaryClass>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<StructClass>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<RecordClass>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<ParsableClass>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<PathClass>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<ClassWithoutParameterlessConstructor>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<PrimitiveClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_NestedClass()
        {
            // Arrange
            var tmp2 = PrimitiveClass.CreateTestDefault();
            var tmp = new NestedClass { Boolean = true, PrimitiveClass = tmp2 };

            // Act
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<NestedClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_NestedNestedClass()
        {
            //Arrange
            var tmp = NestedNestedClass.CreateTestDefault();

            // Act
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<NestedNestedClass>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<EnumClass>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<NoAttributeClass>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<MultiDimClassIEnumarble>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<NullClass>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<PrimitiveClassIEnumarable>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<DateTimeClass>(xmlDocument);

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
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<IEnumerableClass>(xmlDocument);

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
                Value0 = new TestRecordNoParameterlessConstructor(5, 7),
            };

            // Act
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<ClassRecordNoParameterlessConstructor>(xmlDocument);

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
            Assert.Throws<DotSerialException>(() => DotSerialXml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeHashSet()
        {
            // Arrange
            var tmp = new HashSetClassNotSupported();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialXml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_StackClass()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassStack();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialXml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassHashTable()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassHashTable { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialXml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassStack()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassStack { Value0 = new Stack<int>() };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialXml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassQueue()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassQueue { Value0 = new Queue<int>() };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialXml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassLinkedList()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassLinkedList { Value0 = new LinkedList<int>() };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialXml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassObservableCollection()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassObservableCollection { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialXml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassSortedList()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassSortedList { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialXml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassSortedSet()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassSortedSet { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialXml.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_Null()
        {
            // Arrange
            object? tmp = null;

            // Act
            var xmlDocument = DotSerialXml.Serialize(tmp);
            var result = DotSerialXml.Deserialize<object?>(xmlDocument);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateSerializedObject_InvalidIDException()
        {
            // Arrange
            InvalidIDClass tmp = new();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialXml.Serialize(tmp));
        }

        [Fact]
        public void ToString_Content()
        {
            var tmp = ExampleClass.CreateTestDefault();
            var xml = DotSerialXml.Serialize(tmp);
            string result = xml.ToString();

            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public void ToString_NoContent()
        {
            var xml = new DotSerialXml();
            string result = xml.ToString();

            Assert.NotNull(result);
            Assert.True(string.IsNullOrWhiteSpace(result));
        }
    }
}
