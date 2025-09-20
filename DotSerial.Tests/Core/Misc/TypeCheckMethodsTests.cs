
using Xunit.Sdk;

namespace DotSerial.Tests.Core.Misc
{
    public class TypeCheckMethodsTests
    {
        #region Primitive

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
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsPrimitive(t);

            // Assert
            Assert.True(result);
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
        public void IsPrimitive_False(Type t)
        {
            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsPrimitive(t);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Array

        [Fact]
        public void IsArray_IntArray_True()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsArray(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsArray_IntArrayT_True()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsArray_List_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsArray(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_ListT_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_Dictionary_False()
        {
            // Arrange
            Dictionary<int, int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_HashSet_False()
        {
            // Arrange
            HashSet<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_Class_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_Struct_False()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_Record_False()
        {
            // Arrange
            TestRecord tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsArray(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_Null_False()
        {
            // Arrange
            List<int>? tmp = null;

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsArray(tmp);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region List

        [Fact]
        public void IsList_List_True()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsList(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsList_ListT_True()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsList(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsList_Array_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsList(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsList_ArrayT_False()
        {
            // Arange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsList(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsList_Class_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsList(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsList_Struct_False()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsList(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsList_Record_False()
        {
            // Arrange
            TestRecord tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsList(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsList_Dictionary_False()
        {
            // Arrange
            Dictionary<int, int> tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsList(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsList_HashSet_False()
        {
            // Arrange
            HashSet<int> tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsList(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsList_Null_False()
        {
            // Arrange
            List<int>? tmp = null;

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsList(tmp);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Dictionary

        [Fact]
        public void IsDictionary_Dictionary_True()
        {
            // Arrange
            Dictionary<int, string> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsDictionary(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDictionary_DictionaryT_True()
        {
            // Arrange
            Dictionary<int, string> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsDictionary(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDictionary_Array_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsDictionary(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDictionary_ArrayT_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsDictionary(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDictionary_List_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsDictionary(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDictionary_Class_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsDictionary(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDictionary_Struct_False()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsDictionary(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDictionary_Record_False()
        {
            // Arrange
            TestRecord tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsDictionary(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDictionary_HashSet_False()
        {
            // Arrange
           HashSet<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsDictionary(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDictionary_Null_False()
        {
            // Arrange
            List<int>? tmp = null;

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsDictionary(tmp);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Struct

        [Fact]
        public void IsStruct_Struct_True()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsStruct_StructT_True()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsStruct(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsStruct_Array_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_ArrayT_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsStruct(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Class_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_ClassT_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsStruct(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Decimal_False()
        {
            // Arrange
            decimal tmp = 4;

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Record_False()
        {
            // Arrange
            TestRecord tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_List_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Dictionary_False()
        {
            // Arrange
            Dictionary<int, int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_HashSet_False()
        {
            // Arrange
            HashSet<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Null_False()
        {
            // Arrange
            List<int>? tmp = null;

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Class

        [Fact]
        public void IsClass_Class_True()
        {
            SimpleClass tmp = new();
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());
            Assert.True(result);
        }

        [Fact]
        public void IsClass_ClassT_True()
        {
            object? tmp = new();
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());
            Assert.True(result);
        }

        [Fact]
        public void IsClass_Array_False()
        {
            int[] tmp = [];
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());
            Assert.False(result);
        }

        [Fact]
        public void IsClass_Struct_False()
        {
            TestStruct tmp = new();
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());
            Assert.False(result);
        }

        [Fact]
        public void IsClass_Record_True()
        {
            TestRecord tmp = new();
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());
            Assert.True(result);
        }

        [Fact]
        public void IsClass_List_False()
        {
            List<int> tmp = [];
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());
            Assert.False(result);
        }

        [Fact]
        public void IsClass_Dictionary_False()
        {
            Dictionary<int, int> tmp = [];
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());
            Assert.False(result);
        }

        [Fact]
        public void IsClass_HashSet_False()
        {
            List<int> tmp = [];
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());
            Assert.False(result);
        }

        #endregion
    }
}
