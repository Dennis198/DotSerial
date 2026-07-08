using System.Collections;
using System.Collections.ObjectModel;

namespace DotSerial.Tests.Utilities
{
    public class TypeCheckMethodsTests
    {
        [Fact]
        public void DictionaryArrayT_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollectionKeyValuePair(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DictionaryArray_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollectionKeyValuePair(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DictionaryClass_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollectionKeyValuePair(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DictionaryDictionaryT_True()
        {
            // Arrange
            Dictionary<int, string> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollectionKeyValuePair(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DictionaryDictionary_True()
        {
            // Arrange
            Dictionary<int, string> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollectionKeyValuePair(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DictionaryHashSet_False()
        {
            // Arrange
            HashSet<int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollectionKeyValuePair(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DictionaryList_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollectionKeyValuePair(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DictionaryNull_False()
        {
            // Arrange
            List<int>? tmp = null;

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollectionKeyValuePair(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DictionaryRecord_False()
        {
            // Arrange
            TestRecord tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollectionKeyValuePair(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DictionaryStruct_False()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollectionKeyValuePair(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ImplementsICollection_ArrayT_False()
        {
            // Arange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollection(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ImplementsICollection_Array_True()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollection(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ImplementsICollection_Class_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollection(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ImplementsICollection_Dictionary_False()
        {
            // Arrange
            Dictionary<int, int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollection(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ImplementsICollection_HashSet_True()
        {
            // Arrange
            HashSet<int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollection(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ImplementsICollection_ListT_True()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollection(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ImplementsICollection_List_True()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollection(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ImplementsICollection_Null_False()
        {
            // Arrange
            List<int>? tmp = null;

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollection(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ImplementsICollection_Record_False()
        {
            // Arrange
            TestRecord tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollection(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ImplementsICollection_Struct_False()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.ImplementsICollection(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ImplementsIEnumerable_Int_Object()
        {
            // Arrange
            int tmp = 5;

            // Act
            var result = DotSerial.Utilities.TypeCheckMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ImplementsIEnumerable_Int_Type()
        {
            // Arrange
            Type tmp = typeof(int);

            // Act
            var result = DotSerial.Utilities.TypeCheckMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ImplementsIEnumerable_ListInt_Object()
        {
            // Arrange
            var tmp = new List<int>();

            // Act
            var result = DotSerial.Utilities.TypeCheckMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ImplementsIEnumerable_ListInt_Type()
        {
            // Arrange
            Type tmp = typeof(List<int>);

            // Act
            var result = DotSerial.Utilities.TypeCheckMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsArray_Class_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_Dictionary_False()
        {
            // Arrange
            Dictionary<int, int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_HashSet_False()
        {
            // Arrange
            HashSet<int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_IntArrayT_True()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsArray_IntArray_True()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsArray(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsArray_ListT_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_List_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsArray(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_Null_False()
        {
            // Arrange
            List<int>? tmp = null;

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsArray(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_Record_False()
        {
            // Arrange
            TestRecord tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_Struct_False()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsClass_Array_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsClass_ClassT_True()
        {
            // Arrange
            object? tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsClass_Class_True()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsClass_DateTimet_False()
        {
            // Arrange
            DateTime tmp = DateTime.Now;

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsClass_Dictionary_False()
        {
            // Arrange
            Dictionary<int, int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsClass_HashSet_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsClass_List_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsClass_Record_True()
        {
            // Arrange
            TestRecord tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsClass_Struct_False()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(typeof(object))]
        [InlineData(typeof(EmptyClass))]
        [InlineData(typeof(List<int>))]
        [InlineData(typeof(Array))]
        [InlineData(typeof(Dictionary<int, int>))]
        [InlineData(typeof(TestStruct))]
        [InlineData(typeof(TestRecord))]
        [InlineData(typeof(HashSet<int>))]
        [InlineData(typeof(DateTime))]
        public void IsPrimitive_False(Type t)
        {
            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsPrimitive(t);

            // Assert
            Assert.False(result);
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
        public void IsPrimitive_True(Type t)
        {
            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsPrimitive(t);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSpecialParsableObject_Class_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsSpecialParsableObject(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSpecialParsableObject_DateTimet_True()
        {
            // Arrange
            DateTime tmp = DateTime.Now;

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsSpecialParsableObject(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSpecialParsableObject_Record_False()
        {
            // Arrange
            TestRecord tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsSpecialParsableObject(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSpecialParsableObject_Struct_False()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsSpecialParsableObject(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_ArrayT_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsStruct(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Array_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_ClassT_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsStruct(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Class_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_DateTime_False()
        {
            // Arrange
            DateTime tmp = DateTime.Now;

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Decimal_False()
        {
            // Arrange
            decimal tmp = 4;

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Dictionary_False()
        {
            // Arrange
            Dictionary<int, int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_HashSet_False()
        {
            // Arrange
            HashSet<int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_List_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Null_False()
        {
            // Arrange
            List<int>? tmp = null;

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Record_False()
        {
            // Arrange
            TestRecord tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_StructT_True()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsStruct(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsStruct_Struct_True()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Utilities.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(typeof(Hashtable))]
        [InlineData(typeof(IList))]
        public void IsTypeSupported_False(Type t)
        {
            bool result = DotSerial.Utilities.TypeCheckMethods.IsTypeSupported(t);
            Assert.False(result);
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
        [InlineData(typeof(Stack<int>))]
        [InlineData(typeof(Queue<int>))]
        [InlineData(typeof(LinkedList<int>))]
        [InlineData(typeof(HashSet<int>))]
        [InlineData(typeof(SortedSet<int>))]
        [InlineData(typeof(SortedList<int, int>))]
        [InlineData(typeof(ISet<int>))]
        [InlineData(typeof(ObservableCollection<int>))]
        [InlineData(typeof(Collection<int>))]
        public void IsTypeSupported_True(Type t)
        {
            bool result = DotSerial.Utilities.TypeCheckMethods.IsTypeSupported(t);
            Assert.True(result);
        }
    }
}
