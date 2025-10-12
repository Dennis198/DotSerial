using DotSerial.Core.Exceptions;
using DotSerial.Core.General;
using DotSerial.Core.Misc;

namespace DotSerial.Tests.Core.General
{
    public class DSDeserializeTests
    {

        [Fact]
        public void Deserialize_EmptyClass()
        {
            // Arrange
            var tmp = new EmptyClass();
            var result = CreateInstanceMethods.CreateInstanceGeneric<EmptyClass>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_AccessModifierClass()
        {
            // Arrange
            var tmp = AccessModifierClass.CreateTestDefault();
            var result = CreateInstanceMethods.CreateInstanceGeneric<AccessModifierClass>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_DictionaryClass()
        {
            // Arrange
            var tmp = DictionaryClass.CreateTestDefault();
            var result = CreateInstanceMethods.CreateInstanceGeneric<DictionaryClass>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_StructClass()
        {
            // Arrange
            var tmp = StructClass.CreateTestDefault();
            var result = CreateInstanceMethods.CreateInstanceGeneric<StructClass>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_RecordClass()
        {
            // Arrange
            var tmp = RecordClass.CreateTestDefault();
            var result = CreateInstanceMethods.CreateInstanceGeneric<RecordClass>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_PrimitiveClass()
        {
            // Arrange
            var tmp = PrimitiveClass.CreateTestDefault();
            var result = CreateInstanceMethods.CreateInstanceGeneric<PrimitiveClass>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
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
            var result = CreateInstanceMethods.CreateInstanceGeneric<NestedClass>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
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
            var result = CreateInstanceMethods.CreateInstanceGeneric<NestedNestedClass>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_EnumClass()
        {
            // Arrange
            var tmp = EnumClass.CreateTestDefault();
            var result = CreateInstanceMethods.CreateInstanceGeneric<EnumClass>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);


            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        //[Fact]
        //public void CreateSerializedObject_DateTimeClass()
        //{
        //    // TODO!!!!!!!!!!!!!!!!!!!!

        //    // Arrange
        //    var tmp = DateTimeClass.CreateTestDefault();
        //    var result = CreateInstanceMethods.CreateInstanceGeneric<DateTimeClass>();

        //    // Act
        //    var node = DSSerialize.Serialize(tmp, "0");
        //    DSDeserialize.Deserialize(result, node);


        //    // Assert
        //    Assert.NotNull(result);
        //    EqualCheck.AssertClassEqual(tmp, result);
        //}

        [Fact]
        public void CreateSerializedObject_NoAttribute()
        {
            // Arrange
            var tmp = NoAttributeClass.CreateTestDefault();
            var result = CreateInstanceMethods.CreateInstanceGeneric<NoAttributeClass>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);

            // Arrange
            Assert.NotNull(result);
            //HelperMethods.AssertClassEqual(tmp, output); // TODO
        }

        [Fact]
        public void CreateSerializedObject_MultiDimClassIEnumarble()
        {
            // Arrange
            var tmp = MultiDimClassIEnumarble.CreateTestDefault();
            var result = CreateInstanceMethods.CreateInstanceGeneric<MultiDimClassIEnumarble>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_NullClass()
        {
            // Arrange
            var tmp = NullClass.CreateTestDefault();
            var result = CreateInstanceMethods.CreateInstanceGeneric<NullClass>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);

        }

        [Fact]
        public void CreateSerializedObject_PrimitiveClassIEnumarable()
        {
            // Arrange
            var tmp = PrimitiveClassIEnumarable.CreateTestDefault();
            var result = CreateInstanceMethods.CreateInstanceGeneric<PrimitiveClassIEnumarable>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
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
            var result = CreateInstanceMethods.CreateInstanceGeneric<IEnumerableClass>();

            // Act
            var node = DSSerialize.Serialize(tmp, "0");
            DSDeserialize.Deserialize(result, node);

            // Assert
            Assert.NotNull(result);
            EqualCheck.AssertClassEqual(tmp, result);
        }

        [Fact]
        public void CreateSerializedObject_DuplicateIDClass()
        {
            // Arrange
            var tmp = new DuplicateIDClass();

            // Act & Assert
            Assert.Throws<DSDuplicateIDException>(() => DSSerialize.Serialize(tmp, "0"));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeHashSet()
        {
            // Arrange
            var tmp = new HashSetClassNotSupported();

            // Act & Assert
            Assert.Throws<DSNotSupportedTypeException>(() => DSSerialize.Serialize(tmp, "0"));
        }

        [Fact]
        public void CreateSerializedObject_StackClass()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassStack();

            // Act & Assert
            Assert.Throws<DSNotSupportedTypeException>(() => DSSerialize.Serialize(tmp, "0"));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassHashTable()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassHashTable
            {
                Value0 = []
            };

            // Act & Assert
            Assert.Throws<DSNotSupportedTypeException>(() => DSSerialize.Serialize(tmp, "0"));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassStack()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassStack
            {
                Value0 = new Stack<int>()
            };

            // Act & Assert
            Assert.Throws<DSNotSupportedTypeException>(() => DSSerialize.Serialize(tmp, "0"));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassQueue()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassQueue
            {
                Value0 = new Queue<int>()
            };

            // Act & Assert
            Assert.Throws<DSNotSupportedTypeException>(() => DSSerialize.Serialize(tmp, "0"));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassLinkedList()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassLinkedList
            {
                Value0 = new LinkedList<int>()
            };

            // Act & Assert
            Assert.Throws<DSNotSupportedTypeException>(() => DSSerialize.Serialize(tmp, "0"));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassObservableCollection()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassObservableCollection
            {
                Value0 = []
            };

            // Act & Assert
            Assert.Throws<DSNotSupportedTypeException>(() => DSSerialize.Serialize(tmp, "0"));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassSortedList()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassSortedList
            {
                Value0 = []
            };

            // Act & Assert
            Assert.Throws<DSNotSupportedTypeException>(() => DSSerialize.Serialize(tmp, "0"));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassSortedSet()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassSortedSet
            {
                Value0 = []
            };

            // Act & Assert
            Assert.Throws<DSNotSupportedTypeException>(() => DSSerialize.Serialize(tmp, "0"));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassRecordNoParameterlessConstructor()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassRecordNoParameterlessConstructor
            {
                Value0 = new TestRecordNoParameterlessConstructor(5, 7)
            };
            var result = CreateInstanceMethods.CreateInstanceGeneric<NotSupportedTypeClassRecordNoParameterlessConstructor>();
            var xmlDocument = DSSerialize.Serialize(tmp, "0");

            // Act & Assert
            Assert.Throws<DSNoParameterlessConstructorDefinedException>(() => DSDeserialize.Deserialize(result, xmlDocument));

        }

        [Fact]
        public void CreateSerializedObject_NullRefernceException()
        {
            // Arrange
            object? tmp = null;

            // Act & Assert
            // TODO
            //Assert.Throws<ArgumentNullException>(() => DSSerialize.Serialize(tmp, "0"));
        }

        [Fact]
        public void CreateSerializedObject_InvalidIDException()
        {
            // Arrange
            InvalidIDClass tmp = new();

            // Act & Assert
            Assert.Throws<DSInvalidIDException>(() => DSSerialize.Serialize(tmp, "0"));
        }

    }
}
