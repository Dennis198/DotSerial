using DotSerial.Core.XML;

namespace DotSerial.Tests.Core.XML
{
    public class XMLSerialTests
    {
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
            var tmp = new PrimitiveClass
            {
                Boolean = true,
                Byte = 42,
                SByte = 41,
                Char = 'd',
                Decimal = 40,
                Double = 39.9,
                Float = 38.8F,
                Int = 37,
                UInt = 36,
                Long = 35,
                ULong = 34,
                Short = 33,
                UShort = 31
            };

            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);

            var output = new PrimitiveClass();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);
        }
    }
}
