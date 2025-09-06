using DotSerial.Core.XML;

namespace DotSerial.Tests.Core.XML
{
    public class XMLSerialTests
    {

        // TODO Code COverage

        //using var fileStream = File.Open(@"C:\Users\Dennis\Downloads\unitTest.xml", FileMode.Create);
        //xmlDocument.Save(fileStream);

        [Fact]
        public void CreateSerializedObject_EmptyClass()
        {
            // Arrange
            var tmp = new EmptyClass();
            var output = new EmptyClass();

            // Act
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            // Assert
            Assert.True(result);
            HelperMethods.AssertClassEqual(tmp, output);
        }

        [Fact]
        public void CreateSerializedObject_DictionaryClass()
        {
            // Arrange
            var tmp = DictionaryClass.CreateTestDefault();
            var output = new DictionaryClass();

            // Act
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);           
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            // Assert
            Assert.True(result);
            HelperMethods.AssertClassEqual(tmp, output);
        }

        [Fact]
        public void CreateSerializedObject_StructClass()
        {
            // Arrange
            var tmp = StructClass.CreateTestDefault();
            var output = new StructClass();

            // Act
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            // Assert
            Assert.True(result);
            HelperMethods.AssertClassEqual(tmp, output);
        }

        [Fact]
        public void CreateSerializedObject_PrimitiveClass()
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();
            var output = new PrimitiveClass();

            // Act
            var xmlDocument = XMLSerial.CreateSerializedObject(testDefault);
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            // Assert
            Assert.True(result);
            HelperMethods.AssertClassEqual(testDefault, output);
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
            var output = new NestedClass();

            // Act
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            // Assert
            Assert.True(result);
            HelperMethods.AssertClassEqual(tmp, output);
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
            var output = new NestedNestedClass();

            // Act
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            // Assert
            Assert.True(result);
            HelperMethods.AssertClassEqual(tmp, output);
        }

        [Fact]
        public void CreateSerializedObject_EnumClass()
        {
            // Arrange
            var tmp = new EnumClass
            {
                TestEnum0 = TestEnum.Fourth,
                TestEnum1 = TestEnum.Undefined,
                TestEnum2 = TestEnum.First
            };
            var output = new EnumClass();

            // Act
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);
            var result = XMLSerial.DeserializeObject(output, xmlDocument);


            // Assert
            Assert.True(result);
            HelperMethods.AssertClassEqual(tmp, output);
        }

        [Fact]
        public void CreateSerializedObject_NoAttribute()
        {
            // Arrange
            var tmp = NoAttributeClass.CreateTestDefault();
            var output = new NoAttributeClass();

            // Act
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            // Arrange
            Assert.True(result);
            HelperMethods.AssertClassEqual(tmp, output);
        }

        [Fact]
        public void CreateSerializedObject_MultiDimClassIEnumarble()
        {
            // Arrange
            var tmp = MultiDimClassIEnumarble.CreateTestDefault();
            var output = new MultiDimClassIEnumarble();

            // Act
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            // Assert
            Assert.True(result);
            HelperMethods.AssertClassEqual(tmp, output);
        }

        [Fact]
        public void CreateSerializedObject_NullClass()
        {
            // Arrange
            var tmp = NullClass.CreateTestDefault();
            var output = new NullClass();

            // Act
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            // Assert
            Assert.True(result);
            HelperMethods.AssertClassEqual(tmp, output);

        }

        [Fact]
        public void CreateSerializedObject_PrimitiveClassIEnumarable()
        {
            // Arrange
            var tmp = PrimitiveClassIEnumarable.CreateTestDefault();
            var output = new PrimitiveClassIEnumarable();

            // Act
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            // Assert
            Assert.True(result);
            HelperMethods.AssertClassEqual(tmp, output);
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
            var output = new IEnumerableClass();

            // Act
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            // Assert
            Assert.True(result);
            HelperMethods.AssertClassEqual(tmp, output);
        }
       
    }
}
