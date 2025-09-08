using System.Collections.ObjectModel;

using DotSerial.Core.Exceptions;
using DotSerial.Core.XML;

namespace DotSerial.Tests.Core.XML
{
    public class XMLSerialTests
    {

        // TODO Code COverage

        //using var fileStream = File.Open(@"C:\Users\Dennis\Downloads\unitTest.xml", FileMode.Create);
        //xmlDocument.Save(fileStream);

        [Fact]
        public void Save_True()
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();
            var xmlDocument = DotSerialXML.CreateSerializedObject(testDefault);
            bool result;

            using (var file = new TemporaryFile())
            {
                // Act
                result = DotSerialXML.Save(file.FileInfo.FullName, xmlDocument);
            }

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Save_Diect_True()
        {
            // Arrange
            var testDefault = PrimitiveClass.CreateTestDefault();            
            bool result;

            using (var file = new TemporaryFile())
            {
                // Act
                result = DotSerialXML.Save(file.FileInfo.FullName, testDefault);
            }

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Load_True()
        {
            // Arrange
            var tmp = new PrimitiveClass();
            var expected = PrimitiveClass.CreateTestDefault();
            string path = Directory.GetCurrentDirectory();
            path = Path.GetFullPath(Path.Combine(path, @"..\..\.."));
            path = Path.Combine(path, @"Resources\XmlTest.xml");
            bool result = false;

            try
            {
                // Act
                result = DotSerialXML.Load(path, tmp);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            // Assert
            Assert.True(result);
            HelperMethods.AssertClassEqual(tmp, expected);

        }


        [Fact]
        public void CreateSerializedObject_EmptyClass()
        {
            // Arrange
            var tmp = new EmptyClass();
            var output = new EmptyClass();

            // Act
            var xmlDocument = DotSerialXML.CreateSerializedObject(tmp);
            var result = DotSerialXML.DeserializeObject(output, xmlDocument);

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
            var xmlDocument = DotSerialXML.CreateSerializedObject(tmp);           
            var result = DotSerialXML.DeserializeObject(output, xmlDocument);

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
            var xmlDocument = DotSerialXML.CreateSerializedObject(tmp);
            var result = DotSerialXML.DeserializeObject(output, xmlDocument);

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
            var xmlDocument = DotSerialXML.CreateSerializedObject(testDefault);
            var result = DotSerialXML.DeserializeObject(output, xmlDocument);

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
            var xmlDocument = DotSerialXML.CreateSerializedObject(tmp);
            var result = DotSerialXML.DeserializeObject(output, xmlDocument);

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
            var xmlDocument = DotSerialXML.CreateSerializedObject(tmp);
            var result = DotSerialXML.DeserializeObject(output, xmlDocument);

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
            var xmlDocument = DotSerialXML.CreateSerializedObject(tmp);
            var result = DotSerialXML.DeserializeObject(output, xmlDocument);


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
            var xmlDocument = DotSerialXML.CreateSerializedObject(tmp);
            var result = DotSerialXML.DeserializeObject(output, xmlDocument);

            // Arrange
            Assert.True(result);
            //HelperMethods.AssertClassEqual(tmp, output); // TODO
        }

        [Fact]
        public void CreateSerializedObject_MultiDimClassIEnumarble()
        {
            // Arrange
            var tmp = MultiDimClassIEnumarble.CreateTestDefault();
            var output = new MultiDimClassIEnumarble();

            // Act
            var xmlDocument = DotSerialXML.CreateSerializedObject(tmp);
            var result = DotSerialXML.DeserializeObject(output, xmlDocument);

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
            var xmlDocument = DotSerialXML.CreateSerializedObject(tmp);
            var result = DotSerialXML.DeserializeObject(output, xmlDocument);

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
            var xmlDocument = DotSerialXML.CreateSerializedObject(tmp);
            var result = DotSerialXML.DeserializeObject(output, xmlDocument);

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
            var xmlDocument = DotSerialXML.CreateSerializedObject(tmp);
            var result = DotSerialXML.DeserializeObject(output, xmlDocument);

            // Assert
            Assert.True(result);
            HelperMethods.AssertClassEqual(tmp, output);
        }

        [Fact]
        public void CreateSerializedObject_DuplicateIDClass()
        {
            // Arrange
            var tmp = new DuplicateIDClass();

            // Act & Assert
            Assert.Throws<DuplicateIDException>(() => DotSerialXML.CreateSerializedObject(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClass()
        {
            // Arrange
            var tmp = new NotSupportedTypeClass();
            tmp.Value0 = new HashSet<int>();

            // Act & Assert
            Assert.Throws<NotSupportedTypeException>(() => DotSerialXML.CreateSerializedObject(tmp));
        }

        [Fact]
        public void CreateSerializedObject_NullRefernceException()
        {
            // Arrange
            object? tmp = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => DotSerialXML.CreateSerializedObject(tmp));
        }

        [Fact]
        public void CreateSerializedObject_InvalidIDException()
        {
            // Arrange
            InvalidIDClass tmp = new();

            // Act & Assert
            Assert.Throws<InvalidIDException>(() => DotSerialXML.CreateSerializedObject(tmp));
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
        public void IsTypeSupported_True(Type t)
        {
            bool result = DotSerialXML.IsTypeSupported(t);
            Assert.True(result);
        }

        [Theory]        
        [InlineData(typeof(HashSet<int>))]
        [InlineData(typeof(Collection<int>))]
        [InlineData(typeof(ISet<int>))]
        public void IsTypeSupported_False(Type t)
        {
            bool result = DotSerialXML.IsTypeSupported(t);
            Assert.False(result);
        }

        [Fact]
        public void AsString()
        {
            var tmp = PrimitiveClass.CreateTestDefault();
            var xml = DotSerialXML.CreateSerializedObject(tmp);
            string result = DotSerialXML.AsString(xml);

            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result));
        }
    }
}
