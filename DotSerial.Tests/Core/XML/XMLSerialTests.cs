using DotSerial.Core.XML;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace DotSerial.Tests.Core.XML
{
    public class XMLSerialTests
    {

        //using var fileStream = File.Open(@"C:\Users\Dennis\Downloads\unitTest.xml", FileMode.Create);
        //xmlDocument.Save(fileStream);

        [Fact]
        public void CreateSerializedObject_EmptyClass()
        {
            var tmp = new EmptyClass();
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);

            var output = new EmptyClass();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);
        }

        [Fact]
        public void CreateSerializedObject_PrimitiveClass()
        {
            var testDefault = PrimitiveClass.CreateTestDefault();

            var xmlDocument = XMLSerial.CreateSerializedObject(testDefault);

            var output = new PrimitiveClass();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);

            Assert.True(output.Boolean);
            AssertDefaultPrimitiveClass(output);
        }

        [Fact]
        public void CreateSerializedObject_NestedClass()
        {
            var tmp2 = PrimitiveClass.CreateTestDefault();

            var tmp = new NestedClass
            {
                Boolean = true,
                PrimitiveClass = tmp2
            };

            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);

            var output = new NestedClass();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);

            Assert.True(output.Boolean);

            Assert.True(output.Boolean);
            AssertDefaultPrimitiveClass(output.PrimitiveClass);
        }

        [Fact]
        public void CreateSerializedObject_NestedNestedClass()
        {
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
                UShort = 13
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

            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);

            var output = new NestedNestedClass();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);

            Assert.True(output.Boolean);

            Assert.True(output.Boolean);
            Assert.NotNull(output.PrimitiveClass);
            Assert.Equal(1, output.PrimitiveClass.Byte);
            Assert.Equal(2, output.PrimitiveClass.SByte);
            Assert.Equal('e', output.PrimitiveClass.Char);
            Assert.Equal(3, output.PrimitiveClass.Decimal);
            Assert.Equal(4.9, output.PrimitiveClass.Double);
            Assert.Equal(5.8F, output.PrimitiveClass.Float);
            Assert.Equal(6, output.PrimitiveClass.Int);
            Assert.Equal(7, (int)output.PrimitiveClass.UInt);
            Assert.Equal(8, output.PrimitiveClass.NInt);
            Assert.Equal(9, (int)output.PrimitiveClass.NUInt);
            Assert.Equal(10, output.PrimitiveClass.Long);
            Assert.Equal(11, (long)output.PrimitiveClass.ULong);
            Assert.Equal(12, output.PrimitiveClass.Short);
            Assert.Equal(13, output.PrimitiveClass.UShort);

            Assert.NotNull(output.NestedClass);
            Assert.True(output.NestedClass.Boolean);
            AssertDefaultPrimitiveClass(output.NestedClass.PrimitiveClass);
        }

        [Fact]
        public void CreateSerializedObject_EnumClass()
        {
            var tmp = new EnumClass
            {
                TestEnum0 = TestEnum.Fourth,
                TestEnum1 = TestEnum.Undefined,
                TestEnum2 = TestEnum.First
            };

            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);

            var output = new EnumClass();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);

            Assert.Equal(TestEnum.Fourth, output.TestEnum0);
            Assert.Equal(TestEnum.Undefined, output.TestEnum1);
            Assert.Equal(TestEnum.First, output.TestEnum2);
        }

        [Fact]
        public void CreateSerializedObject_IEnumerableClass()
        {
            var tmp = new IEnumerableClass
            {
                //Array = new SimpleClass[10],
                List = [],
                Collection = []
            };

            for (int i = 0; i < 10; i++)
            {
                var d = new SimpleClass
                {
                    Boolean = true
                };

                //tmp.Array[i] = d;
                tmp.List.Add(d);
                tmp.Collection.Add(d);
            }
            
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);

            using var fileStream = File.Open(@"C:\Users\Dennis\Downloads\unitTest.xml", FileMode.Create);
            xmlDocument.Save(fileStream);

            var output = new IEnumerableClass();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);
            //Assert.NotNull(output.Array);
            //Assert.Equal(10, output.Array.Length);
            Assert.Equal(10, output.List.Count);
            Assert.Equal(10, output.Collection.Count);

            for (int i = 0; i < 10; i++)
            {

                //Assert.True(tmp.Array[i].Boolean);
                Assert.True(tmp.List[i].Boolean);
                Assert.True(tmp.Collection[i].Boolean);
            }

        }

        private void AssertDefaultPrimitiveClass(PrimitiveClass? pClass)
        {
            var testDefault = PrimitiveClass.CreateTestDefault();

            Assert.NotNull(pClass);
            Assert.Equal(testDefault.Byte, pClass.Byte);
            Assert.Equal(testDefault.SByte, pClass.SByte);
            Assert.Equal(testDefault.Char, pClass.Char);
            Assert.Equal(testDefault.Decimal, pClass.Decimal);
            Assert.Equal(testDefault.Double, pClass.Double);
            Assert.Equal(testDefault.Float, pClass.Float);
            Assert.Equal(testDefault.Int, pClass.Int);
            Assert.Equal((int)testDefault.UInt, (int)pClass.UInt);
            Assert.Equal(testDefault.NInt, pClass.NInt);
            Assert.Equal((int)testDefault.NUInt, (int)pClass.NUInt);
            Assert.Equal(testDefault.Long, pClass.Long);
            Assert.Equal((long)testDefault.ULong, (long)pClass.ULong);
            Assert.Equal(testDefault.Short, pClass.Short);
            Assert.Equal(testDefault.UShort, pClass.UShort);
        }
    }
}
