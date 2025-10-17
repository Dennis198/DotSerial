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
        [InlineData(typeof(DateTime))]
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

        [Fact]
        public void IsStruct_DateTime_False()
        {
            // Arrange
            DateTime tmp = DateTime.Now;

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
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsClass_ClassT_True()
        {
            // Arrange
            object? tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsClass_Array_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsClass_Struct_False()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsClass_Record_True()
        {
            // Arrange
            TestRecord tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsClass_List_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsClass_Dictionary_False()
        {
            // Arrange
            Dictionary<int, int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsClass_HashSet_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsClass_DateTimet_False()
        {
            // Arrange
            DateTime tmp = DateTime.Now;

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsClass(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        #endregion

        #region Special Parsable

        [Fact]
        public void IsSpecialParsableObject_DateTimet_True()
        {
            // Arrange
            DateTime tmp = DateTime.Now;

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsSpecialParsableObject(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSpecialParsableObject_Struct_False()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsSpecialParsableObject(tmp.GetType());

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void IsSpecialParsableObject_Class_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsSpecialParsableObject(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSpecialParsableObject_Record_False()
        {
            // Arrange
            TestRecord tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.TypeCheckMethods.IsSpecialParsableObject(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        #endregion
    }
}
