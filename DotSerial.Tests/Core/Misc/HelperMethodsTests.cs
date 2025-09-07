namespace DotSerial.Tests.Core.Misc
{
    public class HelperMethodsTests
    {
        [Fact]
        public void GetItemTypeOfIEnumerable_ListInt_Type()
        {            
            // Arrange
            Type tmp = typeof(List<int>);

            // Act
            var result = DotSerial.Core.Misc.HelperMethods.GetItemTypeOfIEnumerable(tmp);

            // Assert
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetItemTypeOfIEnumerable_ListInt_Object()
        {
            // Arrange
            var tmp = new List<int>();

            // Act
            var result = DotSerial.Core.Misc.HelperMethods.GetItemTypeOfIEnumerable(tmp);

            // Assert
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void ImplementsIEnumerable_ListInt_Object()
        {
            // Arrange
            var tmp = new List<int>();

            // Act
            var result = DotSerial.Core.Misc.HelperMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ImplementsIEnumerable_Int_Object()
        {
            // Arrange
            int tmp = 5;

            // Act
            var result = DotSerial.Core.Misc.HelperMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ImplementsIEnumerable_ListInt_Type()
        {
            // Arrange
            Type tmp = typeof(List<int>);

            // Act
            var result = DotSerial.Core.Misc.HelperMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ImplementsIEnumerable_Int_Type()
        {
            // Arrange
            Type tmp = typeof(int);

            // Act
            var result = DotSerial.Core.Misc.HelperMethods.ImplementsIEnumerable(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void BoolToInt_True()
        {
            // Arrange
            bool tmp = true;

            // Act
            int result = DotSerial.Core.Misc.HelperMethods.BoolToInt(tmp);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void BoolToInt_False()
        {
            // Arrange
            bool tmp = false;

            // Act
            int result = DotSerial.Core.Misc.HelperMethods.BoolToInt(tmp);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void IntToBool_False()
        {
            // Arrange
            int tmp = 0;

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IntToBool(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IntToBool_True()
        {
            // Arrange
            int tmp = 1;

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IntToBool(tmp);

            // Assert
            Assert.True(result);
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
            bool result = DotSerial.Core.Misc.HelperMethods.IsPrimitive(t);

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
        public void IsPrimitive_False(Type t)
        {
            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsPrimitive(t);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetKeyValueTypeOfDictionary_Object()
        {
            // Arrange
            Dictionary<int, string> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.GetKeyValueTypeOfDictionary(tmp, out Type typeKey, out Type typeValue);

            // Assert
            Assert.True(result);
            Assert.NotNull(typeKey);
            Assert.NotNull(typeValue);
            Assert.Equal(typeof(int), typeKey);
            Assert.Equal(typeof(string), typeValue);
        }

        [Fact]
        public void GetKeyValueTypeOfDictionary_Type()
        {
            // Arrange
            Dictionary<int, string> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.GetKeyValueTypeOfDictionary(tmp.GetType(), out Type typeKey, out Type typeValue);

            // Assert
            Assert.True(result);
            Assert.NotNull(typeKey);
            Assert.NotNull(typeValue);
            Assert.Equal(typeof(int), typeKey);
            Assert.Equal(typeof(string), typeValue);
        }

        [Fact]
        public void GetItemTypeOfArray_Object()
        {
            // Arrange
            int[] tmp = [];

            // Act
            Type? result = DotSerial.Core.Misc.HelperMethods.GetItemTypeOfArray(tmp);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(int), result);            
        }

        [Fact]
        public void GetItemTypeOfArray_Type()
        {
            // Arrange
            int[] tmp = [];

            // Act
            Type? result = DotSerial.Core.Misc.HelperMethods.GetItemTypeOfArray(tmp.GetType());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void IsArray_Object_True()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsArray(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsArray_Type_True()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsArray(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsArray_Object_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsArray(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_Type_False()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsArray(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsArray_Null_False()
        {
            List<int>? tmp = null;
            bool result = DotSerial.Core.Misc.HelperMethods.IsArray(tmp);
            Assert.False(result);
        }

        [Fact]
        public void IsList_Object_True()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsList(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsList_Type_True()
        {
            // Arrange
            List<int> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsList(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsList_Object_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsList(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsList_Type_False()
        {
            // Arange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsList(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsList_Null_False()
        {
            // Arrange
            List<int>? tmp = null;

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsList(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDictionary_Object_True()
        {
            // Arrange
            Dictionary<int, string> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsDictionary(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDictionary_Type_True()
        {
            // Arrange
            Dictionary<int, string> tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsDictionary(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsDictionary_Object_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsDictionary(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDictionary_Type_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsDictionary(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsDictionary_Null_False()
        {
            // Arrange
            List<int>? tmp = null;

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsDictionary(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Object_True()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsStruct_Type_True()
        {
            // Arrange
            TestStruct tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp.GetType());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsStruct_Object_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Type_False()
        {
            // Arrange
            int[] tmp = [];

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Class_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_ClassType_False()
        {
            // Arrange
            SimpleClass tmp = new();

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp.GetType());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Decimal_False()
        {
            // Arrange
            decimal tmp = 4;

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Null_False()
        {
            // Arrange
            List<int>? tmp = null;

            // Act
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp);

            // Assert
            Assert.False(result);
        }






        [Fact]
        public void IsClass_Type_True()
        {
            SimpleClass tmp = new();
            bool result = DotSerial.Core.Misc.HelperMethods.IsClass(tmp.GetType());
            Assert.True(result);
        }

        [Fact]
        public void IsClass_Object_True()
        {
            object? tmp = new();
            bool result = DotSerial.Core.Misc.HelperMethods.IsClass(tmp.GetType());
            Assert.True(result);
        }

        [Fact]
        public void IsClass_Type_False()
        {
            int[] tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsClass(tmp.GetType());
            Assert.False(result);
        }

        [Fact]
        public void IsClass_Class_False()
        {
            TestStruct tmp = new();
            bool result = DotSerial.Core.Misc.HelperMethods.IsClass(tmp.GetType());
            Assert.False(result);
        }

    }
}
