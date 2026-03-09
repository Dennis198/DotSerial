using DotSerial.Common;
using DotSerial.Toon;

namespace DotSerial.Tests.Toon
{
    public class DotSerialToonTests
    {
        [Fact]
        public void Save_True()
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();
            var toonDocument = DotSerialToon.Serialize(testDefault);

            using var file = new TemporaryFile();
            // Act
            DotSerialToon.SaveToFile(file.FileInfo.FullName, toonDocument);
        }

        [Fact]
        public void Save_Direct_True()
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();

            using var file = new TemporaryFile();
            // Act
            DotSerialToon.SaveToFile(file.FileInfo.FullName, testDefault);
        }

        [Fact]
        public void Load_True()
        {
            // Arrange
            PrimitiveClass? tmp = null;
            var expected = PrimitiveClass.CreateTestDefault();
            string path = Directory.GetCurrentDirectory();
            path = Path.GetFullPath(Path.Combine(path, string.Format("..{0}..{0}..", Path.DirectorySeparatorChar)));
            path = Path.Combine(path, string.Format("Resources{0}ToonTest.toon", Path.DirectorySeparatorChar));

            try
            {
                // Act
                tmp = DotSerialToon.LoadFromFile<PrimitiveClass>(path);
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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<double>(toonDocument);

            // Assert
            Assert.Equal(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_Primitive_Null()
        {
            // Arrange
            string? tmp = null;

            // Act
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<string?>(toonDocument);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void List()
        {
            // Arrange
            double[] tmp = [1.1, 2.2, 3.3, 4.4];

            // Act
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<double[]>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<double[]>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<Dictionary<string, double>>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<Dictionary<string, double>?>(toonDocument);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateSerializedObject_EmptyClass()
        {
            // Arrange
            var tmp = new EmptyClass();

            // Act
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<EmptyClass>(toonDocument);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateSerializedObject_AccessModifierClass()
        {
            // Arrange
            var tmp = AccessModifierClass.CreateTestDefault();

            // Act
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<AccessModifierClass>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<DictionaryClass>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<StructClass>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<RecordClass>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<ParsableClass>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<PathClass>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<ClassWithoutParameterlessConstructor>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<PrimitiveClass>(toonDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_NestedClass()
        {
            // Arrange
            var tmp = NestedClass.CreateTestDefault();

            // Act
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<NestedClass>(toonDocument);

            // Assert
            Assert.NotNull(result);
            Assert.True(tmp.AssertTest(result));
        }

        [Fact]
        public void CreateSerializedObject_NestedNestedClass()
        {
            var tmp = NestedNestedClass.CreateTestDefault();

            // Act
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<NestedNestedClass>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<EnumClass>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<NoAttributeClass>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<MultiDimClassIEnumarble>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<NullClass>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<PrimitiveClassIEnumarable>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<DateTimeClass>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<IEnumerableClass>(toonDocument);

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
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<ClassRecordNoParameterlessConstructor>(toonDocument);

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
            Assert.Throws<DotSerialException>(() => DotSerialToon.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeHashSet()
        {
            // Arrange
            var tmp = new HashSetClassNotSupported();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialToon.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_StackClass()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassStack();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialToon.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassHashTable()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassHashTable { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialToon.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassStack()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassStack { Value0 = new Stack<int>() };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialToon.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassQueue()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassQueue { Value0 = new Queue<int>() };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialToon.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassLinkedList()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassLinkedList { Value0 = new LinkedList<int>() };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialToon.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassObservableCollection()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassObservableCollection { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialToon.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassSortedList()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassSortedList { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialToon.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassSortedSet()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassSortedSet { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialToon.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_Null()
        {
            // Arrange
            object? tmp = null;

            // Act
            var toonDocument = DotSerialToon.Serialize(tmp);
            var result = DotSerialToon.Deserialize<object?>(toonDocument);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void CreateSerializedObject_InvalidIDException()
        {
            // Arrange
            InvalidIDClass tmp = new();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => DotSerialToon.Serialize(tmp));
        }

        [Fact]
        public void ToString_Content()
        {
            // Arrange
            var tmp = ExampleClass.CreateTestDefault();
            var toon = DotSerialToon.Serialize(tmp);

            // Act
            string result = toon.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public void ToString_NoContent()
        {
            // Arrange
            var toon = new DotSerialToon();

            // Act
            string result = toon.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrWhiteSpace(result));
        }
    }
}
