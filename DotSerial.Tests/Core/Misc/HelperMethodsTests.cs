namespace DotSerial.Tests.Core.Misc
{
    public class HelperMethodsTests
    {
        [Fact]
        public void GetItemTypeOfIEnumerable_ListInt_Type()
        {            
            Type tmp = typeof(List<int>);
            var result = DotSerial.Core.Misc.HelperMethods.GetItemTypeOfIEnumerable(tmp);
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetItemTypeOfIEnumerable_ListInt_Object()
        {
            var tmp = new List<int>();
            var result = DotSerial.Core.Misc.HelperMethods.GetItemTypeOfIEnumerable(tmp);
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void ImplementsIEnumerable_ListInt_Object()
        {
            var tmp = new List<int>();
            var result = DotSerial.Core.Misc.HelperMethods.ImplementsIEnumerable(tmp);
            Assert.True(result);
        }

        [Fact]
        public void ImplementsIEnumerable_Int_Object()
        {
            int tmp = 5;
            var result = DotSerial.Core.Misc.HelperMethods.ImplementsIEnumerable(tmp);
            Assert.False(result);
        }

        [Fact]
        public void ImplementsIEnumerable_ListInt_Type()
        {
            Type tmp = typeof(List<int>);
            var result = DotSerial.Core.Misc.HelperMethods.ImplementsIEnumerable(tmp);
            Assert.True(result);
        }

        [Fact]
        public void ImplementsIEnumerable_Int_Type()
        {
            Type tmp = typeof(int);
            var result = DotSerial.Core.Misc.HelperMethods.ImplementsIEnumerable(tmp);
            Assert.False(result);
        }

        [Fact]
        public void BoolToInt_True()
        {
            bool tmp = true;
            int result = DotSerial.Core.Misc.HelperMethods.BoolToInt(tmp);
            Assert.Equal(1, result);
        }

        [Fact]
        public void BoolToInt_False()
        {
            bool tmp = false;
            int result = DotSerial.Core.Misc.HelperMethods.BoolToInt(tmp);
            Assert.Equal(0, result);
        }

        [Fact]
        public void IntToBool_False()
        {
            int tmp = 0;
            bool result = DotSerial.Core.Misc.HelperMethods.IntToBool(tmp);
            Assert.False(result);
        }

        [Fact]
        public void IntToBool_True()
        {
            int tmp = 1;
            bool result = DotSerial.Core.Misc.HelperMethods.IntToBool(tmp);
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
            bool result = DotSerial.Core.Misc.HelperMethods.IsPrimitive(t);
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
            bool result = DotSerial.Core.Misc.HelperMethods.IsPrimitive(t);
            Assert.False(result);
        }

        [Fact]
        public void GetKeyValueTypeOfDictionary_Object()
        {
            Dictionary<int, string> tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.GetKeyValueTypeOfDictionary(tmp, out Type typeKey, out Type typeValue);
            Assert.True(result);
            Assert.NotNull(typeKey);
            Assert.NotNull(typeValue);
            Assert.Equal(typeof(int), typeKey);
            Assert.Equal(typeof(string), typeValue);
        }

        [Fact]
        public void GetKeyValueTypeOfDictionary_Type()
        {
            Dictionary<int, string> tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.GetKeyValueTypeOfDictionary(tmp.GetType(), out Type typeKey, out Type typeValue);
            Assert.True(result);
            Assert.NotNull(typeKey);
            Assert.NotNull(typeValue);
            Assert.Equal(typeof(int), typeKey);
            Assert.Equal(typeof(string), typeValue);
        }

        [Fact]
        public void GetItemTypeOfArray_Object()
        {
            int[] tmp = [];
            Type? result = DotSerial.Core.Misc.HelperMethods.GetItemTypeOfArray(tmp);
            
            Assert.NotNull(result);
            Assert.Equal(typeof(int), result);            
        }

        [Fact]
        public void GetItemTypeOfArray_Type()
        {
            int[] tmp = [];
            Type? result = DotSerial.Core.Misc.HelperMethods.GetItemTypeOfArray(tmp.GetType());

            Assert.NotNull(result);
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void IsArray_Object_True()
        {
            int[] tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsArray(tmp);
            Assert.True(result);
        }

        [Fact]
        public void IsArray_Type_True()
        {
            int[] tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsArray(tmp.GetType());
            Assert.True(result);
        }

        [Fact]
        public void IsArray_Object_False()
        {
            List<int> tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsArray(tmp);
            Assert.False(result);
        }

        [Fact]
        public void IsArray_Type_False()
        {
            List<int> tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsArray(tmp.GetType());
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
            List<int> tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsList(tmp);
            Assert.True(result);
        }

        [Fact]
        public void IsList_Type_True()
        {
            List<int> tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsList(tmp.GetType());
            Assert.True(result);
        }

        [Fact]
        public void IsList_Object_False()
        {
            int[] tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsList(tmp);
            Assert.False(result);
        }

        [Fact]
        public void IsList_Type_False()
        {
            int[] tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsList(tmp.GetType());
            Assert.False(result);
        }

        [Fact]
        public void IsList_Null_False()
        {
            List<int>? tmp = null;
            bool result = DotSerial.Core.Misc.HelperMethods.IsList(tmp);
            Assert.False(result);
        }

        [Fact]
        public void IsDictionary_Object_True()
        {
            Dictionary<int, string> tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsDictionary(tmp);
            Assert.True(result);
        }

        [Fact]
        public void IsDictionary_Type_True()
        {
            Dictionary<int, string> tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsDictionary(tmp.GetType());
            Assert.True(result);
        }

        [Fact]
        public void IsDictionary_Object_False()
        {
            int[] tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsDictionary(tmp);
            Assert.False(result);
        }

        [Fact]
        public void IsDictionary_Type_False()
        {
            int[] tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsDictionary(tmp.GetType());
            Assert.False(result);
        }

        [Fact]
        public void IsDictionary_Null_False()
        {
            List<int>? tmp = null;
            bool result = DotSerial.Core.Misc.HelperMethods.IsDictionary(tmp);
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Object_True()
        {
            TestStruct tmp = new();
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp);
            Assert.True(result);
        }

        [Fact]
        public void IsStruct_Type_True()
        {
            TestStruct tmp = new();
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp.GetType());
            Assert.True(result);
        }

        [Fact]
        public void IsStruct_Object_False()
        {
            int[] tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp);
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Type_False()
        {
            int[] tmp = [];
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp.GetType());
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Class_False()
        {
            SimpleClass tmp = new();
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp);
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_ClassType_False()
        {
            SimpleClass tmp = new();
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp.GetType());
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Decimal_False()
        {
            decimal tmp = 4;
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp);
            Assert.False(result);
        }

        [Fact]
        public void IsStruct_Null_False()
        {
            List<int>? tmp = null;
            bool result = DotSerial.Core.Misc.HelperMethods.IsStruct(tmp);
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
