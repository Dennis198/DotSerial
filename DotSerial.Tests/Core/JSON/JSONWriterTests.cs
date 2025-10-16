using DotSerial.Core.General;
using DotSerial.Core.JSON;

namespace DotSerial.Tests.Core.JSON
{
    public class JSONWriterTests
    {
        [Fact]
        public void Convert_ExampleClass()
        {
            // Arrange
            var tmp = ExampleClass.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");

            // Act
            string result = JSONWriter.ToJsonString(node);

            // Assert
            Assert.Equal(JSONConstants.ExampleClassAsJson, result);
        }

        [Fact]
        public void Convert_EmptyClass()
        {
            // Arrange
            var tmp = new EmptyClass();
            var node = DSSerialize.Serialize(tmp, "0");

            // Act
            string result = JSONWriter.ToJsonString(node);

            // Assert
            Assert.Equal(JSONConstants.EmptyClassAsJson, result);
        }

        [Fact]
        public void Convert_PrimitiveClass()
        {
            // Arrange
            var tmp = PrimitiveClass.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");

            // Act
            string result = JSONWriter.ToJsonString(node);

            // Assert
            Assert.Equal(JSONConstants.PrimitiveClassAsJson, result);
        }

        [Fact]
        public void Convert_PrimitiveClassIEnumarable()
        {
            // Arrange
            var tmp = PrimitiveClassIEnumarable.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");

            // Act
            string result = JSONWriter.ToJsonString(node);

            // Assert
            Assert.Equal(JSONConstants.PrimitiveClassIEnumarableAsJson, result);
        }

        [Fact]
        public void Convert_IEnumerableClass()
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
            var node = DSSerialize.Serialize(tmp, "0");

            // Act
            string result = JSONWriter.ToJsonString(node);

            // Assert
            Assert.Equal(JSONConstants.IEnumerableClassAsJson, result);
        }

        [Fact]
        public void Convert_DictionaryClass()
        {
            // Arrange
            var tmp = DictionaryClass.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");

            // Act
            string result = JSONWriter.ToJsonString(node);

            // Assert
            Assert.Equal(JSONConstants.DictionaryClassAsJson, result);
        }

        [Fact]
        public void Convert_NullClass()
        {
            // Arrange
            var tmp = NullClass.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");

            // Act
            string result = JSONWriter.ToJsonString(node);

            // Assert
            Assert.Equal(JSONConstants.NullClassAsJson, result);
        }

        [Fact]
        public void Convert_NestedClass()
        {
            // Arrange
            var tmp2 = PrimitiveClass.CreateTestDefault();
            var tmp = new NestedClass
            {
                Boolean = true,
                PrimitiveClass = tmp2
            };
            var node = DSSerialize.Serialize(tmp, "0");

            // Act
            string result = JSONWriter.ToJsonString(node);

            // Assert
            Assert.Equal(JSONConstants.NestedClassAsJson, result);
        }

        [Fact]
        public void Convert_NestedNestedClass()
        {
            // Arrange
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
            var node = DSSerialize.Serialize(tmp, "0");

            // Act
            string result = JSONWriter.ToJsonString(node);

            // Assert
            Assert.Equal(JSONConstants.NestedNestedClassAsJson, result);
        }

        [Fact]
        public void Convert_MultiDimClassIEnumarble()
        {
            // Arrange
            var tmp = MultiDimClassIEnumarble.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");

            // Act
            string result = JSONWriter.ToJsonString(node);

            // Assert
            Assert.Equal(JSONConstants.MultiDimClassIEnumarbleAsJson, result);
        }

        [Fact]
        public void Convert_DateTimeClass()
        {
            // Arrange
            var tmp = DateTimeClass.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");

            // Act
            string result = JSONWriter.ToJsonString(node);

            // Assert
            Assert.Equal(JSONConstants.DateTimeClassAsJson, result);
        }
    }
}
