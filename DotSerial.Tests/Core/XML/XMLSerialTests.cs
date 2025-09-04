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
            Assert.NotNull(output);
        }

        [Fact]
        public void CreateSerializedObject_DictionaryClass()
        {
            var tmp = DictionaryClass.CreateTestDefault();
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);

            var output = new DictionaryClass();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);

            Assert.NotNull(output.DicIntInt);
            Assert.Equal(output.DicIntInt.Count, tmp.DicIntInt?.Count);
            foreach(var keyValue in output.DicIntInt)
            {
                Assert.True(tmp.DicIntInt?.ContainsKey(keyValue.Key));
                Assert.Equal(tmp.DicIntInt?[keyValue.Key], keyValue.Value);
            }

            Assert.NotNull(output.DicIntString);
            Assert.Equal(output.DicIntString.Count, tmp.DicIntString?.Count);
            foreach (var keyValue in output.DicIntString)
            {
                Assert.True(tmp.DicIntString?.ContainsKey(keyValue.Key));
                Assert.Equal(tmp.DicIntString?[keyValue.Key], keyValue.Value);
            }

            Assert.NotNull(output.DicStringInt);
            Assert.Equal(output.DicStringInt.Count, tmp.DicStringInt?.Count);
            foreach (var keyValue in output.DicStringInt)
            {
                Assert.True(tmp.DicStringInt?.ContainsKey(keyValue.Key));
                Assert.Equal(tmp.DicStringInt?[keyValue.Key], keyValue.Value);
            }

            Assert.NotNull(output.DicStringString);
            Assert.Equal(output.DicStringString.Count, tmp.DicStringString?.Count);
            foreach (var keyValue in output.DicStringString)
            {
                Assert.True(tmp.DicStringString?.ContainsKey(keyValue.Key));
                Assert.Equal(tmp.DicStringString?[keyValue.Key], keyValue.Value);
            }

            Assert.NotNull(output.DicEmpty);
            Assert.Equal(output.DicEmpty.Count, tmp.DicEmpty?.Count);

            Assert.Null(output.DicNull);
        }

        [Fact]
        public void CreateSerializedObject_StructClass()
        {
            var tmp = StructClass.CreateTestDefault();
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);

            var output = new StructClass();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);

            Assert.Equal(tmp.TestStruct0.Value0, output.TestStruct0.Value0);
            Assert.Equal(tmp.TestStruct0.Value1, output.TestStruct0.Value1);

            Assert.Equal(tmp.TestStruct1.Value0, output.TestStruct1.Value0);
            Assert.Equal(tmp.TestStruct1.Value1, output.TestStruct1.Value1);

            Assert.Equal(tmp.TestStruct2.Value0, output.TestStruct2.Value0);
            Assert.Equal(tmp.TestStruct2.Value1, output.TestStruct2.Value1);

            Assert.NotNull(output.TestStructArray);
            Assert.Equal(tmp.TestStructArray.Length, output.TestStructArray.Length);
            for (int i = 0; i < tmp.TestStructArray.Length; i++)
            {
                Assert.Equal(tmp.TestStructArray[i].Value0, output.TestStructArray[i].Value0);
                Assert.Equal(tmp.TestStructArray[i].Value1, output.TestStructArray[i].Value1);
            }
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
            Assert.Equal("HelloWorld", output.PrimitiveClass.String);
            Assert.Equal(TestEnum.Second, output.PrimitiveClass.Enum);

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
        public void CreateSerializedObject_NoAttribute()
        {
            var tmp = NoAttributeClass.CreateTestDefault();
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);

            var output = new NoAttributeClass();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);

            AssertDefaultPrimitiveClass(output.PrimitiveClass);
            Assert.True(output.Boolean);

            Assert.False(output.BooleanNoAttribute);
            Assert.Equal(0, output.IntNoAttribute);
            Assert.Null(output.StringNoAttribute);
            Assert.Null(output.SimpleClassNoAttribute);
            Assert.Null(output.PrimitiveClassNoAttribute);
            Assert.Equal(TestEnum.None, output.EnumNoAttribute);
            Assert.Null(output.ArrayNoAttribute);
            Assert.Null(output.ListNoAttribute);
        }

        [Fact]
        public void CreateSerializedObject_MultiDimClassIEnumarble()
        {
            var tmp = MultiDimClassIEnumarble.CreateTestDefault();
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);

            var output = new MultiDimClassIEnumarble();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);

            Assert.NotNull(output.Int);
            Assert.Equal(tmp.Int?.Length, output.Int.Length);
            for (int i = 0; i < output.Int.Length; i++)
            {
                Assert.Equal(tmp.Int?[i].Length, output.Int[i].Length);
                for (int j = 0; j < tmp.Int?[i].Length; j++)
                {
                    Assert.Equal(tmp.Int[i][j], output.Int[i][j]);
                }
            }

            Assert.NotNull(output.IntList);
            Assert.Equal(tmp.IntList?.Count, output.IntList.Count);
            for (int i = 0; i < output.IntList.Count; i++)
            {
                Assert.Equal(tmp.IntList?[i].Count, output.IntList[i].Count);
                for (int j = 0; j < tmp.IntList?[i].Count; j++)
                {
                    Assert.Equal(tmp.IntList[i][j], output.IntList[i][j]);
                }
            }

            Assert.NotNull(output.IntThree);
            Assert.Equal(tmp.IntThree?.Length, output.IntThree.Length);
            for (int i = 0; i < output.IntThree.Length; i++)
            {
                Assert.Equal(tmp.IntThree?[i].Length, output.IntThree[i].Length);
                for (int j = 0; j < tmp.IntThree?[i].Length; j++)
                {
                    Assert.Equal(tmp.IntThree[i][j].Length, output.IntThree[i][j].Length);
                    for (int k = 0; k < tmp.IntThree[i][j].Length; k++)
                        Assert.Equal(tmp.IntListThree?[i][j][k], output.IntThree[i][j][k]);
                }
            }

            Assert.NotNull(output.IntListThree);
            Assert.Equal(tmp.IntListThree?.Count, output.IntListThree.Count);
            for (int i = 0; i < output.IntListThree.Count; i++)
            {
                Assert.Equal(tmp.IntListThree?[i].Count, output.IntListThree[i].Count);
                for (int j = 0; j < tmp.IntListThree?[i].Count; j++)
                {
                    Assert.Equal(tmp.IntListThree[i][j].Count, output.IntListThree[i][j].Count);
                    for (int k = 0; k < tmp.IntListThree[i][j].Count; k++)
                        Assert.Equal(tmp.IntListThree[i][j][k], output.IntListThree[i][j][k]);
                }
            }

            Assert.NotNull(output.IntMix);
            Assert.Equal(tmp.IntMix?.Length, output.IntMix.Length);
            for (int i = 0; i < output.IntMix.Length; i++)
            {
                Assert.Equal(tmp.IntMix?[i].Count, output.IntMix[i].Count);
                for (int j = 0; j < tmp.IntMix?[i].Count; j++)
                {
                    Assert.Equal(tmp.IntMix?[i][j], output.IntMix[i][j]);
                }
            }

            Assert.NotNull(output.IntListMix);
            Assert.Equal(tmp.IntListMix?.Count, output.IntListMix.Count);
            for (int i = 0; i < output.IntListMix.Count; i++)
            {
                Assert.Equal(tmp.IntListMix?[i].Length, output.IntListMix[i].Length);
                for (int j = 0; j < tmp.IntListMix?[i].Length; j++)
                {
                    Assert.Equal(tmp.IntListMix[i][j], output.IntListMix[i][j]);
                }
            }

            Assert.NotNull(output.String);
            Assert.Equal(tmp.String?.Length, output.String.Length);
            for (int i = 0; i < output.String.Length; i++)
            {
                Assert.Equal(tmp.String?[i].Length, output.String[i].Length);
                for (int j = 0; j < tmp.String?[i].Length; j++)
                {
                    Assert.Equal(tmp.String[i][j], output.String[i][j]);
                }
            }

            Assert.NotNull(output.StringList);
            Assert.Equal(tmp.StringList?.Count, output.StringList.Count);
            for (int i = 0; i < output.StringList.Count; i++)
            {
                Assert.Equal(tmp.StringList?[i].Count, output.StringList[i].Count);
                for (int j = 0; j < tmp.StringList?[i].Count; j++)
                {
                    Assert.Equal(tmp.StringList[i][j], output.StringList[i][j]);
                }
            }

            Assert.NotNull(output.StringThree);
            Assert.Equal(tmp.StringThree?.Length, output.StringThree.Length);
            for (int i = 0; i < output.StringThree.Length; i++)
            {
                Assert.Equal(tmp.StringThree?[i].Length, output.StringThree[i].Length);
                for (int j = 0; j < tmp.StringThree?[i].Length; j++)
                {
                    Assert.Equal(tmp.StringThree[i][j].Length, output.StringThree[i][j].Length);
                    for (int k = 0; k < tmp.StringThree[i][j].Length; k++)
                        Assert.Equal(tmp.StringThree[i][j][k], output.StringThree[i][j][k]);
                }
            }

            Assert.NotNull(output.StringListThree);
            Assert.Equal(tmp.StringListThree?.Count, output.StringListThree.Count);
            for (int i = 0; i < output.StringListThree.Count; i++)
            {
                Assert.Equal(tmp.StringListThree?[i].Count, output.StringListThree[i].Count);
                for (int j = 0; j < tmp.StringListThree?[i].Count; j++)
                {
                    Assert.Equal(tmp.StringListThree[i][j].Count, output.StringListThree[i][j].Count);
                    for (int k = 0; k < tmp.StringListThree[i][j].Count; k++)
                        Assert.Equal(tmp.StringListThree[i][j][k], output.StringListThree[i][j][k]);
                }
            }

            Assert.NotNull(output.StringMix);
            Assert.Equal(tmp.StringMix?.Length, output.StringMix.Length);
            for (int i = 0; i < output.StringMix.Length; i++)
            {
                Assert.Equal(tmp.StringMix?[i].Count, output.StringMix[i].Count);
                for (int j = 0; j < tmp.StringMix?[i].Count; j++)
                {
                    Assert.Equal(tmp.StringMix[i][j], output.StringMix[i][j]);
                }
            }

            Assert.NotNull(output.StringListMix);
            Assert.Equal(tmp.StringListMix?.Count, output.StringListMix.Count);
            for (int i = 0; i < output.StringListMix.Count; i++)
            {
                Assert.Equal(tmp.StringListMix?[i].Length, output.StringListMix[i].Length);
                for (int j = 0; j < tmp.StringListMix?[i].Length; j++)
                {
                    Assert.Equal(tmp.StringListMix[i][j], output.StringListMix[i][j]);
                }
            }

            Assert.NotNull(output.PrimitiveClassArray);
            Assert.Equal(tmp.PrimitiveClassArray?.Length, output.PrimitiveClassArray.Length);
            for (int i = 0; i < output.PrimitiveClassArray.Length; i++)
            {
                Assert.Equal(tmp.PrimitiveClassArray?[i].Length, output.PrimitiveClassArray[i].Length);
                for (int j = 0; j < tmp.PrimitiveClassArray?[i].Length; j++)
                {
                    AssertDefaultPrimitiveClass(output.PrimitiveClassList?[i][j]);
                }
            }

            Assert.NotNull(output.PrimitiveClassList);
            Assert.Equal(tmp.PrimitiveClassList?.Count, output.PrimitiveClassList.Count);
            for (int i = 0; i < output.PrimitiveClassList.Count; i++)
            {
                Assert.Equal(tmp.PrimitiveClassList?[i].Count, output.PrimitiveClassList[i].Count);
                for (int j = 0; j < tmp.PrimitiveClassList?[i].Count; j++)
                {
                    AssertDefaultPrimitiveClass(output.PrimitiveClassList[i][j]);
                }
            }

            Assert.NotNull(output.PrimitiveClassArrayThree);
            Assert.Equal(tmp.PrimitiveClassArrayThree?.Length, output.PrimitiveClassArrayThree.Length);
            for (int i = 0; i < output.PrimitiveClassArrayThree.Length; i++)
            {
                Assert.Equal(tmp.PrimitiveClassArrayThree?[i].Length, output.PrimitiveClassArrayThree[i].Length);
                for (int j = 0; j < tmp.PrimitiveClassArrayThree?[i].Length; j++)
                {
                    Assert.Equal(tmp.PrimitiveClassArrayThree[i][j].Length, output.PrimitiveClassArrayThree[i][j].Length);
                    for (int k = 0; k < tmp.PrimitiveClassArrayThree[i][j].Length; k++)
                        AssertDefaultPrimitiveClass(output.PrimitiveClassArrayThree[i][j][k]);
                }
            }

            Assert.NotNull(output.PrimitiveClassListThree);
            Assert.Equal(tmp.PrimitiveClassListThree?.Count, output.PrimitiveClassListThree.Count);
            for (int i = 0; i < output.PrimitiveClassListThree.Count; i++)
            {
                Assert.Equal(tmp.PrimitiveClassListThree?[i].Count, output.PrimitiveClassListThree[i].Count);
                for (int j = 0; j < tmp.PrimitiveClassListThree?[i].Count; j++)
                {
                    Assert.Equal(tmp.PrimitiveClassListThree[i][j].Count, output.PrimitiveClassListThree[i][j].Count);
                    for (int k = 0; k < tmp.PrimitiveClassListThree[i][j].Count; k++)
                        AssertDefaultPrimitiveClass(output.PrimitiveClassListThree[i][j][k]);
                }
            }

            Assert.NotNull(output.PrimitiveClassArrayMix);
            Assert.Equal(tmp.PrimitiveClassArrayMix?.Length, output.PrimitiveClassArrayMix.Length);
            for (int i = 0; i < output.PrimitiveClassArrayMix.Length; i++)
            {
                Assert.Equal(tmp.PrimitiveClassArrayMix?[i].Count, output.PrimitiveClassArrayMix[i].Count);
                for (int j = 0; j < tmp.PrimitiveClassArrayMix?[i].Count; j++)
                {
                    AssertDefaultPrimitiveClass(output.PrimitiveClassArrayMix[i][j]);
                }
            }

            Assert.NotNull(output.PrimitiveClassListMix);
            Assert.Equal(tmp.PrimitiveClassListMix?.Count, output.PrimitiveClassListMix.Count);
            for (int i = 0; i < output.PrimitiveClassListMix.Count; i++)
            {
                Assert.Equal(tmp.PrimitiveClassListMix?[i].Length, output.PrimitiveClassListMix[i].Length);
                for (int j = 0; j < tmp.PrimitiveClassListMix?[i].Length; j++)
                {
                    AssertDefaultPrimitiveClass(output.PrimitiveClassListMix[i][j]);
                }
            }

            Assert.NotNull(output.Enum);
            Assert.Equal(tmp.Enum?.Length, output.Enum.Length);
            for (int i = 0; i < output.Enum.Length; i++)
            {
                Assert.Equal(tmp.Enum?[i].Length, output.Enum[i].Length);
                for (int j = 0; j < tmp.Enum?[i].Length; j++)
                {
                    Assert.Equal(tmp.Enum[i][j], output.Enum[i][j]);
                }
            }

            Assert.NotNull(output.EnumList);
            Assert.Equal(tmp.EnumList?.Count, output.EnumList.Count);
            for (int i = 0; i < output.EnumList.Count; i++)
            {
                Assert.Equal(tmp.EnumList?[i].Count, output.EnumList[i].Count);
                for (int j = 0; j < tmp.EnumList?[i].Count; j++)
                {
                    Assert.Equal(tmp.EnumList[i][j], output.EnumList[i][j]);
                }
            }

            Assert.NotNull(output.EnumThree);
            Assert.Equal(tmp.EnumThree?.Length, output.EnumThree.Length);
            for (int i = 0; i < output.EnumThree.Length; i++)
            {
                Assert.Equal(tmp.EnumThree?[i].Length, output.EnumThree[i].Length);
                for (int j = 0; j < tmp.EnumThree?[i].Length; j++)
                {
                    Assert.Equal(tmp.EnumThree[i][j].Length, output.EnumThree[i][j].Length);
                    for (int k = 0; k < tmp.EnumThree[i][j].Length; k++)
                        Assert.Equal(tmp.EnumThree[i][j][k], output.EnumThree[i][j][k]);
                }
            }

            Assert.NotNull(output.EnumListThree);
            Assert.Equal(tmp.EnumListThree?.Count, output.EnumListThree.Count);
            for (int i = 0; i < output.EnumListThree.Count; i++)
            {
                Assert.Equal(tmp.EnumListThree?[i].Count, output.EnumListThree[i].Count);
                for (int j = 0; j < tmp.EnumListThree?[i].Count; j++)
                {
                    Assert.Equal(tmp.EnumListThree[i][j].Count, output.EnumListThree[i][j].Count);
                    for (int k = 0; k < tmp.EnumListThree[i][j].Count; k++)
                        Assert.Equal(tmp.EnumListThree[i][j][k], output.EnumListThree[i][j][k]);
                }
            }

            Assert.NotNull(output.EnumMix);
            Assert.Equal(tmp.EnumMix?.Length, output.EnumMix.Length);
            for (int i = 0; i < output.EnumMix.Length; i++)
            {
                Assert.Equal(tmp.EnumMix?[i].Count, output.EnumMix[i].Count);
                for (int j = 0; j < tmp.EnumMix?[i].Count; j++)
                {
                    Assert.Equal(tmp.EnumMix[i][j], output.EnumMix[i][j]);
                }
            }

            Assert.NotNull(output.EnumListMix);
            Assert.Equal(tmp.EnumListMix?.Count, output.EnumListMix.Count);
            for (int i = 0; i < output.EnumListMix.Count; i++)
            {
                Assert.Equal(tmp.EnumListMix?[i].Length, output.EnumListMix[i].Length);
                for (int j = 0; j < tmp.EnumListMix?[i].Length; j++)
                {
                    Assert.Equal(tmp.EnumListMix[i][j], output.EnumListMix[i][j]);
                }
            }
        }

        [Fact]
        public void CreateSerializedObject_NullClass()
        {
            var tmp = NullClass.CreateTestDefault();

            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);

            var output = new NullClass();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);

            Assert.Null(output.SimpleClass);
            Assert.Null(output.Array);
            Assert.Null(output.List);
            Assert.Null(output.String);
            Assert.Null(output.BooleanArray);
            Assert.Null(output.BooleanList);
            Assert.Null(output.StringArray);
            Assert.Null(output.StringList);
            Assert.Null(output.EnumArray);
            Assert.Null(output.EnumList);

            Assert.NotNull(output.ArrayWithNulls);
            Assert.Equal(tmp.ArrayWithNulls?.Length, output.ArrayWithNulls.Length);
            for (int i = 0; i < tmp.ArrayWithNulls?.Length; i++)
            {
                Assert.Null(output.ArrayWithNulls[i]);
            }

            Assert.NotNull(output.ListWithNulls);
            Assert.Equal(tmp.ListWithNulls?.Count, output.ListWithNulls.Count);
            for (int i = 0; i < tmp.ListWithNulls?.Count; i++)
            {
                Assert.Null(output.ListWithNulls[i]);
            }

            Assert.NotNull(output.StringArrayWithNulls);
            Assert.Equal(tmp.StringArrayWithNulls?.Length, output.StringArrayWithNulls.Length);
            for (int i = 0; i < tmp.StringArrayWithNulls?.Length; i++)
            {
                Assert.Null(output.StringArrayWithNulls[i]);
            }

            Assert.NotNull(output.StringListWithNulls);
            Assert.Equal(tmp.StringListWithNulls?.Count, output.StringListWithNulls.Count);
            for (int i = 0; i < tmp.StringListWithNulls?.Count; i++)
            {
                Assert.Null(output.StringListWithNulls[i]);
            }

        }

        [Fact]
        public void CreateSerializedObject_PrimitiveClassIEnumarable()
        {
            var tmp = PrimitiveClassIEnumarable.CreateTestDefault();

            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);

            var output = new PrimitiveClassIEnumarable();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);

            Assert.NotNull(output.Boolean);
            Assert.Equal(tmp.Boolean?.Length, output.Boolean.Length);
            Assert.NotNull(output.BooleanList);
            Assert.Equal(tmp.BooleanList?.Count, output.BooleanList.Count);
            for (int i = 0; i < tmp.Boolean?.Length; i++)
            {
                Assert.Equal(tmp.Boolean[i], output.Boolean[i]);
                Assert.Equal(tmp.BooleanList?[i], output.BooleanList[i]);
            }

            Assert.NotNull(output.Byte);
            Assert.Equal(tmp.Byte?.Length, output.Byte.Length);
            Assert.NotNull(output.ByteList);
            Assert.Equal(tmp.ByteList?.Count, output.ByteList.Count);
            for (int i = 0; i < tmp.Byte?.Length; i++)
            {
                Assert.Equal(tmp.Byte[i], output.Byte[i]);
                Assert.Equal(tmp.ByteList?[i], output.ByteList[i]);
            }

            Assert.NotNull(output.SByte);
            Assert.Equal(tmp.SByte?.Length, output.SByte.Length);
            Assert.NotNull(output.SByteList);
            Assert.Equal(tmp.SByteList?.Count, output.SByteList.Count);
            for (int i = 0; i < tmp.SByte?.Length; i++)
            {
                Assert.Equal(tmp.SByte[i], output.SByte[i]);
                Assert.Equal(tmp.SByteList?[i], output.SByteList[i]);
            }

            Assert.NotNull(output.Char);
            Assert.Equal(tmp.Char?.Length, output.Char.Length);
            Assert.NotNull(output.CharList);
            Assert.Equal(tmp.CharList?.Count, output.CharList.Count);
            for (int i = 0; i < tmp.Char?.Length; i++)
            {
                Assert.Equal(tmp.Char[i], output.Char[i]);
                Assert.Equal(tmp.CharList?[i], output.CharList[i]);
            }

            Assert.NotNull(output.Decimal);
            Assert.Equal(tmp.Decimal?.Length, output.Decimal.Length);
            Assert.NotNull(output.DecimalList);
            Assert.Equal(tmp.DecimalList?.Count, output.DecimalList.Count);
            for (int i = 0; i < tmp.Decimal?.Length; i++)
            {
                Assert.Equal(tmp.Decimal[i], output.Decimal[i]);
                Assert.Equal(tmp.DecimalList?[i], output.DecimalList[i]);
            }

            Assert.NotNull(output.Double);
            Assert.Equal(tmp.Double?.Length, output.Double.Length);
            Assert.NotNull(output.DoubleList);
            Assert.Equal(tmp.DoubleList?.Count, output.DoubleList.Count);
            for (int i = 0; i < tmp.Double?.Length; i++)
            {
                Assert.Equal(tmp.Double[i], output.Double[i]);
                Assert.Equal(tmp.DoubleList?[i], output.DoubleList[i]);
            }

            Assert.NotNull(output.Float);
            Assert.Equal(tmp.Float?.Length, output.Float.Length);
            Assert.NotNull(output.FloatList);
            Assert.Equal(tmp.FloatList?.Count, output.FloatList.Count);
            for (int i = 0; i < tmp.Float?.Length; i++)
            {
                Assert.Equal(tmp.Float[i], output.Float[i]);
                Assert.Equal(tmp.FloatList?[i], output.FloatList[i]);
            }

            Assert.NotNull(output.Int);
            Assert.Equal(tmp.Int?.Length, output.Int.Length);
            Assert.NotNull(output.IntList);
            Assert.Equal(tmp.IntList?.Count, output.IntList.Count);
            for (int i = 0; i < tmp.Int?.Length; i++)
            {
                Assert.Equal(tmp.Int[i], output.Int[i]);
                Assert.Equal(tmp.IntList?[i], output.IntList[i]);
            }

            Assert.NotNull(output.UInt);
            Assert.Equal(tmp.UInt?.Length, output.UInt.Length);
            Assert.NotNull(output.UIntList);
            Assert.Equal(tmp.UIntList?.Count, output.UIntList.Count);
            for (int i = 0; i < tmp.UInt?.Length; i++)
            {
                Assert.Equal(tmp.UInt?[i], output.UInt[i]);
                Assert.Equal(tmp.UIntList?[i], output.UIntList[i]);
            }

            Assert.NotNull(output.NInt);
            Assert.Equal(tmp.NInt?.Length, output.NInt.Length);
            Assert.NotNull(output.NIntList);
            Assert.Equal(tmp.NIntList?.Count, output.NIntList.Count);
            for (int i = 0; i < tmp.NInt?.Length; i++)
            {
                Assert.Equal(tmp.NInt[i], output.NInt[i]);
                Assert.Equal(tmp.NIntList?[i], output.NIntList[i]);
            }

            Assert.NotNull(output.NUInt);
            Assert.Equal(tmp.NUInt?.Length, output.NUInt.Length);
            Assert.NotNull(output.NUIntList);
            Assert.Equal(tmp.NUIntList?.Count, output.NUIntList.Count);
            for (int i = 0; i < tmp.NUInt?.Length; i++)
            {
                Assert.Equal(tmp.NUInt?[i], output.NUInt[i]);
                Assert.Equal(tmp.NUIntList?[i], output.NUIntList[i]);
            }

            Assert.NotNull(output.Long);
            Assert.Equal(tmp.Long?.Length, output.Long.Length);
            Assert.NotNull(output.LongList);
            Assert.Equal(tmp.LongList?.Count, output.LongList.Count);
            for (int i = 0; i < tmp.Long?.Length; i++)
            {
                Assert.Equal(tmp.Long?[i], output.Long[i]);
                Assert.Equal(tmp.LongList?[i], output.LongList[i]);
            }

            Assert.NotNull(output.ULong);
            Assert.Equal(tmp.ULong?.Length, output.ULong.Length);
            Assert.NotNull(output.ULongList);
            Assert.Equal(tmp.ULongList?.Count, output.ULongList.Count);
            for (int i = 0; i < tmp.ULong?.Length; i++)
            {
                Assert.Equal(tmp.ULong[i], output.ULong[i]);
                Assert.Equal(tmp.ULongList?[i], output.ULongList[i]);
            }

            Assert.NotNull(output.Short);
            Assert.Equal(tmp.Short?.Length, output.Short.Length);
            Assert.NotNull(output.ShortList);
            Assert.Equal(tmp.ShortList?.Count, output.ShortList.Count);
            for (int i = 0; i < tmp.Short?.Length; i++)
            {
                Assert.Equal(tmp.Short[i], output.Short[i]);
                Assert.Equal(tmp.ShortList?[i], output.ShortList[i]);
            }

            Assert.NotNull(output.UShort);
            Assert.Equal(tmp.UShort?.Length, output.UShort.Length);
            Assert.NotNull(output.UShortList);
            Assert.Equal(tmp.UShortList?.Count, output.UShortList.Count);
            for (int i = 0; i < tmp.UShort?.Length; i++)
            {
                Assert.Equal(tmp.UShort[i], output.UShort[i]);
                Assert.Equal(tmp.UShortList?[i], output.UShortList[i]);
            }

            Assert.NotNull(output.String);
            Assert.Equal(tmp.String?.Length, output.String.Length);
            Assert.NotNull(output.StringList);
            Assert.Equal(tmp.StringList?.Count, output.StringList.Count);
            for (int i = 0; i < tmp.String?.Length; i++)
            {
                Assert.Equal(tmp.String[i], output.String[i]);
                Assert.Equal(tmp.StringList?[i], output.StringList[i]);
            }

            Assert.NotNull(output.Enum);
            Assert.Equal(tmp.Enum?.Length, output.Enum.Length);
            Assert.NotNull(output.EnumList);
            Assert.Equal(tmp.EnumList?.Count, output.EnumList.Count);
            for (int i = 0; i < tmp.Enum?.Length; i++)
            {
                Assert.Equal(tmp.Enum[i], output.Enum[i]);
                Assert.Equal(tmp.EnumList?[i], output.EnumList[i]);
            }
        }

        [Fact]
        public void CreateSerializedObject_IEnumerableClass()
        {
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
            
            var xmlDocument = XMLSerial.CreateSerializedObject(tmp);

            var output = new IEnumerableClass();
            var result = XMLSerial.DeserializeObject(output, xmlDocument);

            Assert.True(result);
            Assert.NotNull(output);
            Assert.NotNull(output.Array);
            Assert.NotNull(output.List);
            Assert.NotNull(output.Dic);
            Assert.Equal(10, output.Array.Length);
            Assert.Equal(10, output.List?.Count);
            Assert.Equal(10, output.Dic?.Count);

            for (int i = 0; i < 10; i++)
            {
                Assert.True(output?.Array[i].Boolean);
                Assert.True(output?.List?[i].Boolean);
                Assert.True(output?.Dic?.ContainsKey(i));
                Assert.True(output?.Dic?[i].Boolean);
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
            Assert.Equal(testDefault.String, pClass.String);
            Assert.Equal(testDefault.Enum, pClass.Enum);
        }
    }
}
