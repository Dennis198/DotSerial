using DotSerial.Core.Misc;

namespace DotSerial.Tests.Core.Misc
{
    public class HelperMethodsTests
    {
        [Fact]
        public void GetItemTypeOfIEnumerable_ListInt_Type()
        {            
            Type tmp = typeof(List<int>);
            var result = HelperMethods.GetItemTypeOfIEnumerable(tmp);
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void GetItemTypeOfIEnumerable_ListInt_Object()
        {
            var tmp = new List<int>();
            var result = HelperMethods.GetItemTypeOfIEnumerable(tmp);
            Assert.Equal(typeof(int), result);
        }

        [Fact]
        public void ImplementsIEnumerable_ListInt_Object()
        {
            var tmp = new List<int>();
            var result = HelperMethods.ImplementsIEnumerable(tmp);
            Assert.True(result);
        }

        [Fact]
        public void ImplementsIEnumerable_Int_Object()
        {
            int tmp = 5;
            var result = HelperMethods.ImplementsIEnumerable(tmp);
            Assert.False(result);
        }

        [Fact]
        public void ImplementsIEnumerable_ListInt_Type()
        {
            Type tmp = typeof(List<int>);
            var result = HelperMethods.ImplementsIEnumerable(tmp);
            Assert.True(result);
        }

        [Fact]
        public void ImplementsIEnumerable_Int_Type()
        {
            Type tmp = typeof(int);
            var result = HelperMethods.ImplementsIEnumerable(tmp);
            Assert.False(result);
        }

        [Fact]
        public void BoolToInt_True()
        {
            bool tmp = true;
            int result = HelperMethods.BoolToInt(tmp);
            Assert.Equal(1, result);
        }

        [Fact]
        public void BoolToInt_False()
        {
            bool tmp = false;
            int result = HelperMethods.BoolToInt(tmp);
            Assert.Equal(0, result);
        }

        [Fact]
        public void IntToBool_False()
        {
            int tmp = 0;
            bool result = HelperMethods.IntToBool(tmp);
            Assert.False(result);
        }

        [Fact]
        public void IntToBool_True()
        {
            int tmp = 1;
            bool result = HelperMethods.IntToBool(tmp);
            Assert.True(result);
        }
    }
}
