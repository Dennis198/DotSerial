using DotSerial.Common;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Deserialize;
using DotSerial.Tree.Serialize;

namespace DotSerial.Tests.Tree.Serialize
{
    public class SerializeObjectTests
    {
        [Fact]
        public void Deserialize_EmptyClass()
        {
            // Arrange
            var tmp = new EmptyClass();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(EmptyClass));
            if (result is EmptyClass)
            {
                // Assert
                Assert.NotNull(result);
            }
            else
            {
                Assert.Fail();
            }
        }

        // [Fact]
        // public void CreateSerializedObject_AccessModifierClass()
        // {
        //     // Arrange
        //     var tmp = AccessModifierClass.CreateTestDefault();
        //     var result = CreateInstanceMethods.CreateInstanceGeneric<AccessModifierClass>();

        //     // Act
        //     var node = SerializeObject.Serialize(tmp, "0");
        //     DSDeserialize.Deserialize(result, node);

        //     // Assert
        //     Assert.NotNull(result);
        //     EqualCheck.AssertClassEqual(tmp, result);
        // }

        [Fact]
        public void CreateSerializedObject_DictionaryClass()
        {
            // Arrange
            var tmp = DictionaryClass.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(DictionaryClass));
            if (result is DictionaryClass castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_StructClass()
        {
            // Arrange
            var tmp = StructClass.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(StructClass));
            if (result is StructClass castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_RecordClass()
        {
            // Arrange
            var tmp = RecordClass.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(RecordClass));
            if (result is RecordClass castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_ParsableClass()
        {
            // Arrange
            var tmp = ParsableClass.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(ParsableClass));
            if (result is ParsableClass castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_PathClass()
        {
            // Arrange
            var tmp = PathClass.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(PathClass));
            if (result is PathClass castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_ClassWithoutParameterlessConstructor()
        {
            // Arrange
            var tmp = ClassWithoutParameterlessConstructor.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(ClassWithoutParameterlessConstructor));
            if (result is ClassWithoutParameterlessConstructor castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_PrimitiveClass()
        {
            // Arrange
            var tmp = PrimitiveClass.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(PrimitiveClass));
            if (result is PrimitiveClass castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_NestedClass()
        {
            // Arrange
            var tmp = NestedClass.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(NestedClass));
            if (result is NestedClass castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_NestedNestedClass()
        {
            // Arrange
            var tmp = NestedNestedClass.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(NestedNestedClass));
            if (result is NestedNestedClass castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_EnumClass()
        {
            // Arrange
            var tmp = EnumClass.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(EnumClass));
            if (result is EnumClass castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_DateTimeClass()
        {
            // Note: Test is failing because DateTime is not parsed correctly (last digits differ)
            // Expected: 9999-12-31T23:59:59.0000000
            // Actual: 9999 - 12 - 31T23: 59:59.9999999

            // Arrange
            var tmp = DateTimeClass.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(DateTimeClass));
            if (result is DateTimeClass castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_NoAttribute()
        {
            // Arrange
            var tmp = NoAttributeClass.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(NoAttributeClass));
            if (result is NoAttributeClass)
            {
                // Assert
                Assert.NotNull(result);
                // tmp.AssertTest(castedResult); // TODO
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_MultiDimClassIEnumarble()
        {
            // Arrange
            var tmp = MultiDimClassIEnumarble.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(MultiDimClassIEnumarble));
            if (result is MultiDimClassIEnumarble castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_NullClass()
        {
            // Arrange
            var tmp = NullClass.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(NullClass));
            if (result is NullClass castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_PrimitiveClassIEnumarable()
        {
            // Arrange
            var tmp = PrimitiveClassIEnumarable.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(PrimitiveClassIEnumarable));
            if (result is PrimitiveClassIEnumarable castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_IEnumerableClass()
        {
            // Arrange
            var tmp = IEnumerableClass.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(IEnumerableClass));
            if (result is IEnumerableClass castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_ClassRecordNoParameterlessConstructor()
        {
            // Arrange
            var tmp = ClassRecordNoParameterlessConstructor.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(ClassRecordNoParameterlessConstructor));
            if (result is ClassRecordNoParameterlessConstructor castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_DuplicateIDClass()
        {
            // Arrange
            var tmp = new DuplicateIDClass();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeHashSet()
        {
            // Arrange
            var tmp = new HashSetClassNotSupported();

            // Act & Assert
            Assert.Throws<DotSerialException>(() => SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json));
        }

        [Fact]
        public void CreateSerializedObject_StackClass()
        {
            // Arrange
            var tmp = ClassStack.CreateTestDefault();

            // Act
            var node = SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json);
            var result = node.DeserializeAccept(new DeserializeObject(), typeof(ClassStack));
            if (result is ClassStack castedResult)
            {
                // Assert
                Assert.NotNull(result);
                tmp.AssertTest(castedResult);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassHashTable()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassHashTable { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassQueue()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassQueue { Value0 = new Queue<int>() };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassLinkedList()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassLinkedList { Value0 = new LinkedList<int>() };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassObservableCollection()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassObservableCollection { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassSortedList()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassSortedList { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json));
        }

        [Fact]
        public void CreateSerializedObject_NotSupportedTypeClassSortedSet()
        {
            // Arrange
            var tmp = new NotSupportedTypeClassSortedSet { Value0 = [] };

            // Act & Assert
            Assert.Throws<DotSerialException>(() => SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json));
        }

        [Fact]
        public void CreateSerializedObject_InvalidIDException()
        {
            // Arrange
            InvalidIDClass tmp = new();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                SerializeObject.Serialize(tmp, "0", SerializeStrategy.Json)
            );
        }
    }
}
