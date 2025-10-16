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

using DotSerial.Core.General;
using DotSerial.Core.JSON;

namespace DotSerial.Tests.Core.JSON
{
    public class JSONParserTests
    {
        [Fact]
        public void Convert_ExampleClass()
        {
            // Arrange
            var tmp = ExampleClass.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");
            string jsonString = JSONWriter.ToJsonString(node);

            // Act
            DSNode result = JSONParser.ToNode(jsonString);

            // Assert
            EqualCheck.AssertClassEqual(node, result);
        }

        [Fact]
        public void Convert_EmptyClass()
        {
            // Arrange
            var tmp = new EmptyClass();
            var node = DSSerialize.Serialize(tmp, "0");
            string jsonString = JSONWriter.ToJsonString(node);

            //Act
            DSNode result = JSONParser.ToNode(jsonString);

            // Assert
            EqualCheck.AssertClassEqual(node, result);
        }

        [Fact]
        public void Convert_PrimitiveClass()
        {
            // Arrange
            var tmp = PrimitiveClass.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");
            string jsonString = JSONWriter.ToJsonString(node);

            // Act
            DSNode result = JSONParser.ToNode(jsonString);

            // Assert
            EqualCheck.AssertClassEqual(node, result);
        }

        [Fact]
        public void Convert_PrimitiveClassIEnumarable()
        {
            // Arrange
            var tmp = PrimitiveClassIEnumarable.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");
            string jsonString = JSONWriter.ToJsonString(node);

            // Act
            DSNode result = JSONParser.ToNode(jsonString);

            // Assert
            EqualCheck.AssertClassEqual(node, result);
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
            string jsonString = JSONWriter.ToJsonString(node);

            // Act
            DSNode result = JSONParser.ToNode(jsonString);

            // Assert
            EqualCheck.AssertClassEqual(node, result);
        }

        [Fact]
        public void Convert_DictionaryClass()
        {
            // Arrange
            var tmp = DictionaryClass.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");
            string jsonString = JSONWriter.ToJsonString(node);

            // Act
            DSNode result = JSONParser.ToNode(jsonString);

            // Assert
            EqualCheck.AssertClassEqual(node, result);
        }

        [Fact]
        public void Convert_NullClass()
        {
            // Arrange
            var tmp = NullClass.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");
            string jsonString = JSONWriter.ToJsonString(node);

            // Act
            DSNode result = JSONParser.ToNode(jsonString);

            // Assert
            EqualCheck.AssertClassEqual(node, result);
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
            string jsonString = JSONWriter.ToJsonString(node);

            // Act
            DSNode result = JSONParser.ToNode(jsonString);

            // Arrange
            EqualCheck.AssertClassEqual(node, result);
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
            string jsonString = JSONWriter.ToJsonString(node);

            // Act
            DSNode result = JSONParser.ToNode(jsonString);

            // Assert
            EqualCheck.AssertClassEqual(node, result);
        }

        [Fact]
        public void Convert_MultiDimClassIEnumarble()
        {
            // Arrange
            var tmp = MultiDimClassIEnumarble.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");
            string jsonString = JSONWriter.ToJsonString(node);

            // Act
            DSNode result = JSONParser.ToNode(jsonString);


            // Assert
            EqualCheck.AssertClassEqual(node, result);
        }

        [Fact]
        public void Convert_DateTimeClass()
        {
            // Arrange
            var tmp = DateTimeClass.CreateTestDefault();
            var node = DSSerialize.Serialize(tmp, "0");
            string jsonString = JSONWriter.ToJsonString(node);

            // Act
            DSNode result = JSONParser.ToNode(jsonString);


            // Assert
            EqualCheck.AssertClassEqual(node, result);
        }
    }
}
