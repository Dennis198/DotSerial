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

using DotSerial.Core.Exceptions;
using DotSerial.Core.JSON;
using System.Collections;
using System.Collections.ObjectModel;

namespace DotSerial.Tests.Core.JSON
{
    public class DotSerialJSONTests
    {

        [Fact]
        public void Save_True()
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();
            var xmlDocument = DotSerialJSON.Serialize(testDefault);

            using var file = new TemporaryFile();
            // Act
            DotSerialJSON.SaveToFile(file.FileInfo.FullName, xmlDocument);
        }

        [Fact]
        public void Save_Direct_True()
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();

            using var file = new TemporaryFile();
            // Act
            DotSerialJSON.SaveToFile(file.FileInfo.FullName, testDefault);
        }

        [Fact]
        public void Load_True()
        {
            // Arrange
            PrimitiveClass? tmp = null;
            var expected = PrimitiveClass.CreateTestDefault();
            string path = Directory.GetCurrentDirectory();
            path = Path.GetFullPath(Path.Combine(path, @"..\..\.."));
            path = Path.Combine(path, @"Resources\JsonTest.json");

            try
            {
                // Act
                tmp = DotSerialJSON.LoadFromFile<PrimitiveClass>(path);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            // Assert
            Assert.NotNull(tmp);
            EqualCheck.AssertClassEqual(tmp, expected);
        }

        [Fact]
        public void CreateSerializedObject_EmptyClass()
        {
            // Arrange
            var tmp = new EmptyClass();

            // Act
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<EmptyClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_AccessModifierClass()
        {
            // Arrange
            var tmp = AccessModifierClass.CreateTestDefault();

            // Act
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<AccessModifierClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_DictionaryClass()
        {
            // Arrange
            var tmp = DictionaryClass.CreateTestDefault();


            // Act
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<DictionaryClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_StructClass()
        {
            // Arrange
            var tmp = StructClass.CreateTestDefault();

            // Act
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<StructClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_RecordClass()
        {
            // Arrange
            var tmp = RecordClass.CreateTestDefault();

            // Act
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<RecordClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_PrimitiveClass()
        {
            // Arrange
            var tmp = PrimitiveClass.CreateTestDefault();

            // Act
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<PrimitiveClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
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
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<NestedClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
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
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<NestedNestedClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_EnumClass()
        {
            // Arrange
            var tmp = EnumClass.CreateTestDefault();

            // Act
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<EnumClass>(xmlDocument);


            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_NoAttribute()
        {
            // Arrange
            var tmp = NoAttributeClass.CreateTestDefault();

            // Act
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<NoAttributeClass>(xmlDocument);

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
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<MultiDimClassIEnumarble>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_NullClass()
        {
            // Arrange
            var tmp = NullClass.CreateTestDefault();

            // Act
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<NullClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);

        }

        [Fact]
        public void CreateSerializedObject_PrimitiveClassIEnumarable()
        {
            // Arrange
            var tmp = PrimitiveClassIEnumarable.CreateTestDefault();

            // Act
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<PrimitiveClassIEnumarable>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
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
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<DateTimeClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_IEnumerableClass()
        {
            // Arrange
            var tmp = new IEnumerableClass
            {
                Array = new SimpleClass[10],
                List = [],
                Dic = []
            };
            for (int i = 0; i < 10; i++)
            {
                var d = new SimpleClass
                {
                    Boolean = true
                };

                tmp.Array[i] = d;
                tmp.List.Add(d);
                tmp.Dic.Add(i, d);
            }

            // Act
            var xmlDocument = DotSerialJSON.Serialize(tmp);
            var result = DotSerialJSON.Deserialize<IEnumerableClass>(xmlDocument);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_DuplicateIDClass()
        {
            // Arrange
            var tmp = new DuplicateIDClass();

            // Act & Assert
            Assert.Throws<DSDuplicateIDException>(() => DotSerialJSON.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeHashSet()
        {
            // Arrange
            var tmp = new HashSetClassNotSupported();

            // Act & Assert
            Assert.Throws<DSNotSupportedTypeException>(() => DotSerialJSON.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_StackClass()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassStack();

            // Act & Assert
            Assert.Throws<DSNotSupportedTypeException>(() => DotSerialJSON.Serialize(tmp));
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
            Assert.Throws<DSNotSupportedTypeException>(() => DotSerialJSON.Serialize(tmp));
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
            Assert.Throws<DSNotSupportedTypeException>(() => DotSerialJSON.Serialize(tmp));
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
            Assert.Throws<DSNotSupportedTypeException>(() => DotSerialJSON.Serialize(tmp));
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
            Assert.Throws<DSNotSupportedTypeException>(() => DotSerialJSON.Serialize(tmp));
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
            Assert.Throws<DSNotSupportedTypeException>(() => DotSerialJSON.Serialize(tmp));
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
            Assert.Throws<DSNotSupportedTypeException>(() => DotSerialJSON.Serialize(tmp));
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
            Assert.Throws<DSNotSupportedTypeException>(() => DotSerialJSON.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassRecordNoParameterlessConstructor()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassRecordNoParameterlessConstructor
            {
                Value0 = new TestRecordNoParameterlessConstructor(5, 7)
            };
            var xmlDocument = DotSerialJSON.Serialize(tmp);

            // Act & Assert
            Assert.Throws<DSNoParameterlessConstructorDefinedException>(() => DotSerialJSON.Deserialize<NotSupportedTypeClassRecordNoParameterlessConstructor>(xmlDocument));
        }

        [Fact]
        public void CreateSerializedObject_NullRefernceException()
        {
            // Arrange
            object? tmp = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => DotSerialJSON.Serialize(tmp));
        }

        [Fact]
        public void CreateSerializedObject_InvalidIDException()
        {
            // Arrange
            InvalidIDClass tmp = new();

            // Act & Assert
            Assert.Throws<DSInvalidIDException>(() => DotSerialJSON.Serialize(tmp));
        }

        [Theory]
        [InlineData(typeof(bool))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(sbyte))]
        [InlineData(typeof(char))]
        [InlineData(typeof(decimal))]
        [InlineData(typeof(double))]
        [InlineData(typeof(float))]
        [InlineData(typeof(int))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(nint))]
        [InlineData(typeof(nuint))]
        [InlineData(typeof(long))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(short))]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(string))]
        [InlineData(typeof(TestEnum))]
        [InlineData(typeof(SimpleClass))]
        [InlineData(typeof(TestStruct))]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(Dictionary<int, int>))]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(int[]))]
        [InlineData(typeof(RecordClass))]
        public void IsTypeSupported_True(Type t)
        {
            bool result = DotSerialJSON.IsTypeSupported(t);
            Assert.True(result);
        }

        [Theory]
        [InlineData(typeof(Collection<int>))]
        [InlineData(typeof(ISet<int>))]
        [InlineData(typeof(Hashtable))]
        [InlineData(typeof(Stack<int>))]
        [InlineData(typeof(Queue<int>))]
        [InlineData(typeof(LinkedList<int>))]
        [InlineData(typeof(ObservableCollection<int>))]
        [InlineData(typeof(SortedList<int, int>))]
        [InlineData(typeof(SortedSet<int>))]
        [InlineData(typeof(HashSet<int>))]
        public void IsTypeSupported_False(Type t)
        {
            bool result = DotSerialJSON.IsTypeSupported(t);
            Assert.False(result);
        }

        [Fact]
        public void ToString_Content()
        {
            // Arrange
            var tmp = ExampleClass.CreateTestDefault();
            var xml = DotSerialJSON.Serialize(tmp);

            // Act
            string result = xml.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
        }

        [Fact]
        public void ToString_NoContent()
        {
            // Arrange
            var xml = new DotSerialJSON();

            // Act
            string result = xml.ToString();

            // Assert
            Assert.NotNull(result);
            Assert.True(string.IsNullOrWhiteSpace(result));
        }
    }
}
