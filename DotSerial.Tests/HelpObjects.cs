using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net;
using DotSerial.Attributes;

namespace DotSerial.Tests
{
    public interface ITestable<T>
    {
        public static abstract T CreateTestDefault();
        public abstract bool AssertTest(T actual);
    }

#pragma warning disable CS8602
#pragma warning disable CS8604

    public class DebugClass : ITestable<DebugClass>
    {
        [DotSerialName("pairs")]
        public List<List<int>>? Tmp { get; set; }

        public static DebugClass CreateTestDefault()
        {
            List<int> tmp1 = [1, 2];
            List<int> tmp2 = [3, 4];
            List<List<int>> tmp = [tmp1, tmp2];

            var result = new DebugClass { Tmp = tmp };

            return result;
        }

        public bool AssertTest(DebugClass actual)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Example class (ReadMe.md)
    /// </summary>
    public class ExampleClass : ITestable<ExampleClass>
    {
        [DotSerialName("0")]
        public bool Boolean { get; set; }

        [DotSerialName("1")]
        public int Number { get; set; }

        [DotSerialName("2")]
        public string? Text { get; set; }

        public static ExampleClass CreateTestDefault()
        {
            var tmp = new ExampleClass
            {
                Boolean = true,
                Number = 42,
                Text = "Hello DotSerial!",
            };

            return tmp;
        }

        public bool AssertTest(ExampleClass actual)
        {
            var expected = this;

            if (expected.Boolean != actual.Boolean)
                return false;
            if (expected.Number != actual.Number)
                return false;
            if (false == expected?.Text?.Equals(actual.Text))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Empty Class
    /// </summary>
    public class EmptyClass { }

    /// <summary>
    /// No Attribute Class
    /// </summary>
    public class NoAttributeClass : ITestable<NoAttributeClass>
    {
        [DotSerialName("0")]
        public PrimitiveClass? PrimitiveClass { get; set; }

        [DotSerialName("1")]
        public bool Boolean { get; set; }

        [DotSerialIgnore]
        public bool BooleanNoAttribute { get; set; }

        [DotSerialIgnore]
        public int IntNoAttribute { get; set; }

        [DotSerialIgnore]
        public string? StringNoAttribute { get; set; }

        [DotSerialIgnore]
        public SimpleClass? SimpleClassNoAttribute { get; set; }

        [DotSerialIgnore]
        public PrimitiveClass? PrimitiveClassNoAttribute { get; set; }

        [DotSerialIgnore]
        public TestEnum EnumNoAttribute { get; set; }

        [DotSerialIgnore]
        public int[]? ArrayNoAttribute { get; set; }

        [DotSerialIgnore]
        public List<int>? ListNoAttribute { get; set; }

        public static NoAttributeClass CreateTestDefault()
        {
            var tmp = new NoAttributeClass
            {
                PrimitiveClass = PrimitiveClass.CreateTestDefault(),
                Boolean = true,
                BooleanNoAttribute = true,
                IntNoAttribute = 55,
                StringNoAttribute = "HelloWorld",
                SimpleClassNoAttribute = new SimpleClass(),
                PrimitiveClassNoAttribute = PrimitiveClass.CreateTestDefault(),
                EnumNoAttribute = TestEnum.Second,
                ArrayNoAttribute = [1, 2, 3, 4],
                ListNoAttribute = [9, 8, 7, 6],
            };

            return tmp;
        }

        public bool AssertTest(NoAttributeClass actual)
        {
            var expected = this;

            if (expected.Boolean != actual.Boolean)
                return false;

            if (false == expected.PrimitiveClass.AssertTest(actual.PrimitiveClass))
            {
                return false;
            }

            if (actual.BooleanNoAttribute != false)
            {
                return false;
            }

            if (actual.IntNoAttribute != 0)
            {
                return false;
            }

            if (actual.StringNoAttribute != null)
            {
                return false;
            }

            if (actual.SimpleClassNoAttribute != null)
            {
                return false;
            }

            if (actual.PrimitiveClassNoAttribute != null)
            {
                return false;
            }

            if (actual.EnumNoAttribute != TestEnum.None)
            {
                return false;
            }

            if (actual.ArrayNoAttribute != null)
            {
                return false;
            }

            if (actual.ListNoAttribute != null)
            {
                return false;
            }

            return true;
        }
    }

    public class AccessModifierClass : ITestable<AccessModifierClass>
    {
        [DotSerialName("0")]
        public int PublicBoolean { get; set; }

        [DotSerialName("1")]
        private int PrivateBoolean { get; set; }

        [DotSerialName("2")]
        internal int InternalBoolean { get; set; }

        [DotSerialName("3")]
        protected int ProtectedBoolean { get; set; }

        [DotSerialName("4")]
        public static int StaticBoolean { get; set; }

        public static AccessModifierClass CreateTestDefault()
        {
            var tmp = new AccessModifierClass
            {
                PublicBoolean = 11,
                PrivateBoolean = 12,
                InternalBoolean = 13,
                ProtectedBoolean = 14,
            };

            AccessModifierClass.StaticBoolean = 15;

            return tmp;
        }

        public bool AssertTest(AccessModifierClass actual)
        {
            var expected = this;

            if (expected.PublicBoolean != actual.PublicBoolean)
                return false;
            if (AccessModifierClass.StaticBoolean != 15)
                return false;

            return true;
        }
    }

    /// <summary>
    /// Null Class
    /// </summary>
    public class NullClass : ITestable<NullClass>
    {
        [DotSerialName("0")]
        public SimpleClass? SimpleClass { get; set; }

        [DotSerialName("1")]
        public SimpleClass[]? Array { get; set; }

        [DotSerialName("2")]
        public List<SimpleClass>? List { get; set; }

        [DotSerialName("3")]
        public SimpleClass?[]? ArrayWithNulls { get; set; }

        [DotSerialName("4")]
        public List<SimpleClass?>? ListWithNulls { get; set; }

        [DotSerialName("5")]
        public string? String { get; set; }

        [DotSerialName("6")]
        public bool[]? BooleanArray { get; set; }

        [DotSerialName("7")]
        public List<bool>? BooleanList { get; set; }

        [DotSerialName("8")]
        public string[]? StringArray { get; set; }

        [DotSerialName("9")]
        public List<string>? StringList { get; set; }

        [DotSerialName("10")]
        public string?[]? StringArrayWithNulls { get; set; }

        [DotSerialName("11")]
        public List<string?>? StringListWithNulls { get; set; }

        [DotSerialName("12")]
        public TestEnum[]? EnumArray { get; set; }

        [DotSerialName("13")]
        public List<TestEnum>? EnumList { get; set; }

        [DotSerialName("14")]
        public Dictionary<int, SimpleClass>? Dictionary { get; set; }

        [DotSerialName("15")]
        public Dictionary<int, SimpleClass?>? DictionaryWithNulls { get; set; }

        [DotSerialName("16")]
        public string? StringAsText { get; set; }

        [DotSerialName("17")]
        public string? StringEmpty { get; set; }

        public static NullClass CreateTestDefault()
        {
            var tmp = new NullClass
            {
                SimpleClass = null,
                Array = null,
                List = null,
                ArrayWithNulls = [null, null, null],
                ListWithNulls = [null, null, null],
                String = null,
                BooleanArray = null,
                BooleanList = null,
                StringArray = null,
                StringList = null,
                StringArrayWithNulls = [null, null],
                StringListWithNulls = [null, null],
                EnumArray = null,
                EnumList = null,
                Dictionary = null,
                DictionaryWithNulls = [],
                StringAsText = "null",
                StringEmpty = String.Empty,
            };
            tmp.DictionaryWithNulls.Add(2, null);
            tmp.DictionaryWithNulls.Add(4, null);

            return tmp;
        }

        public bool AssertTest(NullClass actual)
        {
            var expected = this;

            if (expected.SimpleClass != null || actual.SimpleClass != null)
                return false;
            if (expected.Array != null || actual.Array != null)
                return false;
            if (expected.List != null || actual.List != null)
                return false;
            if (expected.String != null || actual.String != null)
                return false;
            if (expected.BooleanArray != null || actual.BooleanArray != null)
                return false;
            if (expected.BooleanList != null || actual.BooleanList != null)
                return false;
            if (expected.StringArray != null || actual.StringArray != null)
                return false;
            if (expected.StringList != null || actual.StringList != null)
                return false;
            if (expected.EnumArray != null || actual.EnumArray != null)
                return false;
            if (expected.EnumList != null || actual.EnumList != null)
                return false;
            if (expected.Dictionary != null || actual.Dictionary != null)
                return false;
            if (!expected.StringAsText.Equals(actual.StringAsText))
                return false;
            if (!expected.StringEmpty.Equals(actual.StringEmpty))
                return false;

            if (expected.ArrayWithNulls.Length != actual.ArrayWithNulls.Length)
                return false;
            for (int i = 0; i < expected.ArrayWithNulls.Length; i++)
            {
                if (expected.ArrayWithNulls[0] != null || actual.ArrayWithNulls[0] != null)
                    return false;
            }

            if (expected.ListWithNulls.Count != actual.ListWithNulls.Count)
                return false;
            for (int i = 0; i < expected.ListWithNulls.Count; i++)
            {
                if (expected.ListWithNulls[0] != null || actual.ListWithNulls[0] != null)
                    return false;
            }

            if (expected.StringArrayWithNulls.Length != actual.StringArrayWithNulls.Length)
                return false;
            for (int i = 0; i < expected.StringArrayWithNulls.Length; i++)
            {
                if (expected.StringArrayWithNulls[0] != null || actual.StringArrayWithNulls[0] != null)
                    return false;
            }

            if (expected.StringListWithNulls.Count != actual.StringListWithNulls.Count)
                return false;
            for (int i = 0; i < expected.StringListWithNulls.Count; i++)
            {
                if (expected.StringListWithNulls[0] != null || actual.StringListWithNulls[0] != null)
                    return false;
            }

            if (expected.DictionaryWithNulls.Count != actual.DictionaryWithNulls.Count)
                return false;

            foreach (var keyValue in expected.DictionaryWithNulls)
            {
                if (actual.DictionaryWithNulls.TryGetValue(keyValue.Key, out var simple))
                {
                    if (keyValue.Value != null || simple != null)
                        return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class ListFirstElementNull : ITestable<ListFirstElementNull>
    {
        [DotSerialName("1")]
        public List<SimpleClass?>? List { get; set; }

        public static ListFirstElementNull CreateTestDefault()
        {
            int num = 5;
            var tmp = new ListFirstElementNull { List = [] };
            tmp.List.Add(null);
            for (int i = 1; i < num; i++)
            {
                var d = new SimpleClass { Boolean = true };

                tmp.List.Add(d);
            }

            return tmp;
        }

        public bool AssertTest(ListFirstElementNull actual)
        {
            var expected = this;

            if (expected.List.Count != actual.List.Count)
                return false;
            if (actual.List[0] != null)
                return false;
            for (int i = 1; i < expected.List.Count; i++)
            {
                if (false == expected.List[i].AssertTest(actual.List[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Simple Class
    /// </summary>
    public class SimpleClass : ITestable<SimpleClass>
    {
        [DotSerialName("0")]
        public bool Boolean { get; set; }

        public static SimpleClass CreateTestDefault()
        {
            var tmp = new SimpleClass { Boolean = true };

            return tmp;
        }

        public bool AssertTest(SimpleClass actual)
        {
            var expected = this;

            if (null == expected && null == actual)
                return true;

            if (expected.Boolean != actual.Boolean)
                return false;

            return true;
        }
    }

    public class ClassSortedList : ITestable<ClassSortedList>
    {
        [DotSerialName("1893")]
        public SortedList<int, int>? Value0 { get; set; }

        public static ClassSortedList CreateTestDefault()
        {
            var tmp = new ClassSortedList { Value0 = new SortedList<int, int>() };
            tmp.Value0.Add(1, 10);
            tmp.Value0.Add(2, 20);
            tmp.Value0.Add(3, 30);
            return tmp;
        }

        public bool AssertTest(ClassSortedList actual)
        {
            var expected = this;
            if (expected.Value0.Count != actual.Value0.Count)
                return false;
            if (expected.Value0[1] != actual.Value0[1])
                return false;
            if (expected.Value0[2] != actual.Value0[2])
                return false;
            if (expected.Value0[3] != actual.Value0[3])
                return false;
            return true;
        }
    }

    public class ClassObservableCollection : ITestable<ClassObservableCollection>
    {
        [DotSerialName("1893")]
        public ObservableCollection<int>? Value0 { get; set; }

        public static ClassObservableCollection CreateTestDefault()
        {
            var tmp = new ClassObservableCollection { Value0 = new ObservableCollection<int>() };
            tmp.Value0.Add(1);
            tmp.Value0.Add(2);
            tmp.Value0.Add(3);
            return tmp;
        }

        public bool AssertTest(ClassObservableCollection actual)
        {
            var expected = this;
            if (expected.Value0.Count != actual.Value0.Count)
                return false;
            if (expected.Value0[0] != actual.Value0[0])
                return false;
            if (expected.Value0[1] != actual.Value0[1])
                return false;
            if (expected.Value0[2] != actual.Value0[2])
                return false;
            return true;
        }
    }

    public class ClassStack : ITestable<ClassStack>
    {
        [DotSerialName("1893")]
        public Stack<int>? Value0 { get; set; }

        public static ClassStack CreateTestDefault()
        {
            var tmp = new ClassStack { Value0 = new Stack<int>([1, 2, 3]) };
            return tmp;
        }

        public bool AssertTest(ClassStack actual)
        {
            var expected = this;
            if (expected.Value0.Count != actual.Value0.Count)
                return false;
            if (expected.Value0.Pop() != actual.Value0.Pop())
                return false;
            if (expected.Value0.Pop() != actual.Value0.Pop())
                return false;
            if (expected.Value0.Pop() != actual.Value0.Pop())
                return false;
            return true;
        }
    }

    public class ClassQueue : ITestable<ClassQueue>
    {
        [DotSerialName("1893")]
        public Queue<int>? Value0 { get; set; }

        public static ClassQueue CreateTestDefault()
        {
            var tmp = new ClassQueue { Value0 = new Queue<int>([1, 2, 3]) };
            return tmp;
        }

        public bool AssertTest(ClassQueue actual)
        {
            var expected = this;
            if (expected.Value0.Count != actual.Value0.Count)
                return false;
            if (expected.Value0.Dequeue() != actual.Value0.Dequeue())
                return false;
            if (expected.Value0.Dequeue() != actual.Value0.Dequeue())
                return false;
            if (expected.Value0.Dequeue() != actual.Value0.Dequeue())
                return false;
            return true;
        }
    }

    public class ClassLinkedList : ITestable<ClassLinkedList>
    {
        [DotSerialName("1893")]
        public LinkedList<int>? Value0 { get; set; }

        public static ClassLinkedList CreateTestDefault()
        {
            var tmp = new ClassLinkedList { Value0 = new LinkedList<int>([1, 2, 3]) };
            return tmp;
        }

        public bool AssertTest(ClassLinkedList actual)
        {
            var expected = this;
            if (expected.Value0.Count != actual.Value0.Count)
                return false;
            if (expected.Value0.First.Value != actual.Value0.First.Value)
                return false;
            if (expected.Value0.First.Next.Value != actual.Value0.First.Next.Value)
                return false;
            if (expected.Value0.First.Next.Next.Value != actual.Value0.First.Next.Next.Value)
                return false;
            return true;
        }
    }

    public class HashSetClass : ITestable<HashSetClass>
    {
        [DotSerialName("1893")]
        public HashSet<int>? Value0 { get; set; }

        public static HashSetClass CreateTestDefault()
        {
            var tmp = new HashSetClass { Value0 = new HashSet<int>([1, 2, 3]) };
            return tmp;
        }

        public bool AssertTest(HashSetClass actual)
        {
            var expected = this;
            if (expected.Value0.Count != actual.Value0.Count)
                return false;
            if (expected.Value0.First() != actual.Value0.First())
                return false;
            if (expected.Value0.ElementAt(1) != actual.Value0.ElementAt(1))
                return false;
            if (expected.Value0.ElementAt(2) != actual.Value0.ElementAt(2))
                return false;
            return true;
        }
    }

    public class ClassSortedSet : ITestable<ClassSortedSet>
    {
        [DotSerialName("1893")]
        public SortedSet<int>? Value0 { get; set; }

        public static ClassSortedSet CreateTestDefault()
        {
            var tmp = new ClassSortedSet { Value0 = new SortedSet<int>([1, 2, 3]) };
            return tmp;
        }

        public bool AssertTest(ClassSortedSet actual)
        {
            var expected = this;
            if (expected.Value0.Count != actual.Value0.Count)
                return false;
            if (expected.Value0.First() != actual.Value0.First())
                return false;
            if (expected.Value0.ElementAt(1) != actual.Value0.ElementAt(1))
                return false;
            if (expected.Value0.ElementAt(2) != actual.Value0.ElementAt(2))
                return false;
            return true;
        }
    }

    /// <summary>
    /// Genric IEnumerable Class
    /// </summary>
    public class IEnumerableClass : ITestable<IEnumerableClass>
    {
        [DotSerialName("0")]
        public SimpleClass[]? Array { get; set; }

        [DotSerialName("1")]
        public List<SimpleClass>? List { get; set; }

        [DotSerialName("2")]
        public Dictionary<int, SimpleClass>? Dic { get; set; }

        public static IEnumerableClass CreateTestDefault()
        {
            int num = 2;
            var tmp = new IEnumerableClass
            {
                Array = new SimpleClass[num],
                List = [],
                Dic = [],
            };
            for (int i = 0; i < num; i++)
            {
                var d = new SimpleClass { Boolean = true };

                tmp.Array[i] = d;
                tmp.List.Add(d);
                tmp.Dic.Add(i, d);
            }

            return tmp;
        }

        public bool AssertTest(IEnumerableClass actual)
        {
            var expected = this;

            if (expected.Array.Length != actual.Array.Length)
                return false;
            for (int i = 0; i < expected.Array.Length; i++)
            {
                if (false == expected.Array[i].AssertTest(actual.Array[i]))
                {
                    return false;
                }
            }

            if (expected.List.Count != actual.List.Count)
                return false;
            for (int i = 0; i < expected.List.Count; i++)
            {
                if (false == expected.List[i].AssertTest(actual.List[i]))
                {
                    return false;
                }
            }

            if (expected.Dic.Count != actual.Dic.Count)
                return false;
            foreach (var keyValue in expected.Dic)
            {
                if (actual.Dic.TryGetValue(keyValue.Key, out var simple))
                {
                    if (false == keyValue.Value.AssertTest(simple))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Nested Class
    /// </summary>
    public class NestedClass() : ITestable<NestedClass>
    {
        [DotSerialName("0")]
        public PrimitiveClass? PrimitiveClass { get; set; }

        [DotSerialName("1")]
        public bool Boolean { get; set; }

        public static NestedClass CreateTestDefault()
        {
            var tmp = new NestedClass { PrimitiveClass = PrimitiveClass.CreateTestDefault(), Boolean = true };

            return tmp;
        }

        public bool AssertTest(NestedClass actual)
        {
            var expected = this;

            if (false == expected.PrimitiveClass.AssertTest(actual.PrimitiveClass))
                return false;
            if (expected.Boolean != actual.Boolean)
                return false;

            return true;
        }
    }

    /// <summary>
    /// Nested Nested Class
    /// </summary>
    public class NestedNestedClass() : ITestable<NestedNestedClass>
    {
        [DotSerialName("0")]
        public NestedClass? NestedClass { get; set; }

        [DotSerialName("1")]
        public PrimitiveClass? PrimitiveClass { get; set; }

        [DotSerialName("2")]
        public bool Boolean { get; set; }

        public static NestedNestedClass CreateTestDefault()
        {
            var tmp = new NestedNestedClass
            {
                NestedClass = NestedClass.CreateTestDefault(),
                PrimitiveClass = PrimitiveClass.CreateTestDefault(),
                Boolean = true,
            };

            return tmp;
        }

        public bool AssertTest(NestedNestedClass actual)
        {
            var expected = this;

            if (false == expected.NestedClass.AssertTest(actual.NestedClass))
                return false;
            if (false == expected.PrimitiveClass.AssertTest(actual.PrimitiveClass))
                return false;
            if (expected.Boolean != actual.Boolean)
                return false;

            return true;
        }
    }

    /// <summary>
    /// Class with all primitives
    /// </summary>
    public class PrimitiveClass : ITestable<PrimitiveClass>
    {
        [DotSerialName("0")]
        public bool Boolean { get; set; }

        [DotSerialName("1")]
        public byte Byte { get; set; }

        [DotSerialName("2")]
        public sbyte SByte { get; set; }

        [DotSerialName("3")]
        public char Char { get; set; }

        [DotSerialName("4")]
        public decimal Decimal { get; set; }

        [DotSerialName("5")]
        public double Double { get; set; }

        [DotSerialName("6")]
        public float Float { get; set; }

        [DotSerialName("7")]
        public int Int { get; set; }

        [DotSerialName("8")]
        public uint UInt { get; set; }

        [DotSerialName("9")]
        public nint NInt { get; set; }

        [DotSerialName("10")]
        public nuint NUInt { get; set; }

        [DotSerialName("11")]
        public long Long { get; set; }

        [DotSerialName("12")]
        public ulong ULong { get; set; }

        [DotSerialName("13")]
        public short Short { get; set; }

        [DotSerialName("14")]
        public ushort UShort { get; set; }

        [DotSerialName("15")]
        public string? String { get; set; }

        [DotSerialName("16")]
        public TestEnum Enum { get; set; }

        public static PrimitiveClass CreateTestDefault()
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
                NInt = 35,
                NUInt = 34,
                Long = 33,
                ULong = 32,
                Short = 31,
                UShort = 30,
                String = "HelloWorld",
                Enum = TestEnum.First,
            };

            return tmp;
        }

        public bool AssertTest(PrimitiveClass actual)
        {
            var expected = this;

            if (expected.Boolean != actual.Boolean)
                return false;
            if (expected.Byte != actual.Byte)
                return false;
            if (expected.SByte != actual.SByte)
                return false;
            if (expected.Char != actual.Char)
                return false;
            if (expected.Decimal != actual.Decimal)
                return false;
            if (expected.Double != actual.Double)
                return false;
            if (expected.Float != actual.Float)
                return false;
            if (expected.Int != actual.Int)
                return false;
            if (expected.UInt != actual.UInt)
                return false;
            if (expected.NInt != actual.NInt)
                return false;
            if (expected.NUInt != actual.NUInt)
                return false;
            if (expected.Long != actual.Long)
                return false;
            if (expected.Short != actual.Short)
                return false;
            if (expected.UShort != actual.UShort)
                return false;
            if (null == expected.String && null != actual.String)
            {
                return false;
            }
            if (false == expected.String.Equals(actual.String))
                return false;
            if (expected.Enum != actual.Enum)
                return false;

            return true;
        }
    }

    /// <summary>
    /// Class with all primitives in arrays/list
    /// </summary>
    public class PrimitiveClassIEnumarable : ITestable<PrimitiveClassIEnumarable>
    {
        [DotSerialName("0")]
        public bool[]? Boolean { get; set; }

        [DotSerialName("1")]
        public List<bool>? BooleanList { get; set; }

        [DotSerialName("2")]
        public byte[]? Byte { get; set; }

        [DotSerialName("3")]
        public List<byte>? ByteList { get; set; }

        [DotSerialName("4")]
        public sbyte[]? SByte { get; set; }

        [DotSerialName("5")]
        public List<sbyte>? SByteList { get; set; }

        [DotSerialName("6")]
        public char[]? Char { get; set; }

        [DotSerialName("7")]
        public List<char>? CharList { get; set; }

        [DotSerialName("8")]
        public decimal[]? Decimal { get; set; }

        [DotSerialName("9")]
        public List<decimal>? DecimalList { get; set; }

        [DotSerialName("10")]
        public double[]? Double { get; set; }

        [DotSerialName("11")]
        public List<double>? DoubleList { get; set; }

        [DotSerialName("12")]
        public float[]? Float { get; set; }

        [DotSerialName("13")]
        public List<float>? FloatList { get; set; }

        [DotSerialName("14")]
        public int[]? Int { get; set; }

        [DotSerialName("15")]
        public List<int>? IntList { get; set; }

        [DotSerialName("16")]
        public uint[]? UInt { get; set; }

        [DotSerialName("17")]
        public List<uint>? UIntList { get; set; }

        [DotSerialName("18")]
        public nint[]? NInt { get; set; }

        [DotSerialName("19")]
        public List<nint>? NIntList { get; set; }

        [DotSerialName("20")]
        public nuint[]? NUInt { get; set; }

        [DotSerialName("21")]
        public List<nuint>? NUIntList { get; set; }

        [DotSerialName("22")]
        public long[]? Long { get; set; }

        [DotSerialName("23")]
        public List<long>? LongList { get; set; }

        [DotSerialName("24")]
        public ulong[]? ULong { get; set; }

        [DotSerialName("25")]
        public List<ulong>? ULongList { get; set; }

        [DotSerialName("26")]
        public short[]? Short { get; set; }

        [DotSerialName("27")]
        public List<short>? ShortList { get; set; }

        [DotSerialName("28")]
        public ushort[]? UShort { get; set; }

        [DotSerialName("29")]
        public List<ushort>? UShortList { get; set; }

        [DotSerialName("30")]
        public string[]? String { get; set; }

        [DotSerialName("31")]
        public List<string>? StringList { get; set; }

        [DotSerialName("32")]
        public TestEnum[]? Enum { get; set; }

        [DotSerialName("33")]
        public List<TestEnum>? EnumList { get; set; }

        public static PrimitiveClassIEnumarable CreateTestDefault()
        {
            var tmp = new PrimitiveClassIEnumarable
            {
                Boolean = [true, true, false, true, false],
                BooleanList = [false, true, false, true, false],
                Byte = [1, 2, 3, 4],
                ByteList = [4, 3, 2, 1],
                SByte = [1, 2, 3],
                SByteList = [3, 2, 1],
                Char = ['b', 'x', 'g'],
                CharList = ['a', '1', '!'],
                Decimal = [-44, 55, 66],
                DecimalList = [88, 99, 111],
                Double = [44.5, 33, 33.2, -888.9],
                DoubleList = [44.5, 33, 33.2, -888.9],
                Float = [4.5f, 33f, 33.266f, -88.9f],
                FloatList = [44.5f, 33f, 323.2f, -888.9f],
                Int = [-446, 55, 66],
                IntList = [88, 996, 111],
                UInt = [446, 55, 66],
                UIntList = [88, 996, 111],
                NInt = [-4426, 55, 66],
                NIntList = [882, 9196, 111],
                NUInt = [446, 545, 66],
                NUIntList = [88, 996, 1141],
                Long = [-446, 55, 66, 999999999],
                LongList = [88, 996, 111, 90909090909],
                ULong = [446, 55, 66, 999999999],
                ULongList = [88, 996, 111, 90909090909],
                Short = [-446, 55, 66],
                ShortList = [88, 996, 111],
                UShort = [446, 55, 66],
                UShortList = [88, 996, 111],
                String = ["HelloWorld", "xxx", ""],
                StringList = ["yyy", "hsdasd", string.Empty],
                Enum = [TestEnum.First, TestEnum.Fourth],
                EnumList = [TestEnum.None, TestEnum.Undefined],
            };

            return tmp;
        }

        public bool AssertTest(PrimitiveClassIEnumarable actual)
        {
            var expected = this;

            if (expected.Boolean.Length != actual.Boolean.Length)
                return false;
            for (int i = 0; i < expected.Boolean.Length; i++)
            {
                if (expected.Boolean[i] != actual.Boolean[i])
                {
                    return false;
                }
            }

            if (expected.BooleanList.Count != actual.BooleanList.Count)
                return false;
            for (int i = 0; i < expected.BooleanList.Count; i++)
            {
                if (expected.BooleanList[i] != actual.BooleanList[i])
                {
                    return false;
                }
            }

            if (expected.Byte.Length != actual.Byte.Length)
                return false;
            for (int i = 0; i < expected.Byte.Length; i++)
            {
                if (expected.Byte[i] != actual.Byte[i])
                {
                    return false;
                }
            }

            if (expected.ByteList.Count != actual.ByteList.Count)
                return false;
            for (int i = 0; i < expected.ByteList.Count; i++)
            {
                if (expected.ByteList[i] != actual.ByteList[i])
                {
                    return false;
                }
            }

            if (expected.SByte.Length != actual.SByte.Length)
                return false;
            for (int i = 0; i < expected.SByte.Length; i++)
            {
                if (expected.SByte[i] != actual.SByte[i])
                {
                    return false;
                }
            }

            if (expected.SByteList.Count != actual.SByteList.Count)
                return false;
            for (int i = 0; i < expected.SByteList.Count; i++)
            {
                if (expected.SByteList[i] != actual.SByteList[i])
                {
                    return false;
                }
            }

            if (expected.Char.Length != actual.Char.Length)
                return false;
            for (int i = 0; i < expected.Char.Length; i++)
            {
                if (expected.Char[i] != actual.Char[i])
                {
                    return false;
                }
            }

            if (expected.CharList.Count != actual.CharList.Count)
                return false;
            for (int i = 0; i < expected.CharList.Count; i++)
            {
                if (expected.CharList[i] != actual.CharList[i])
                {
                    return false;
                }
            }

            if (expected.Decimal.Length != actual.Decimal.Length)
                return false;
            for (int i = 0; i < expected.Decimal.Length; i++)
            {
                if (expected.Decimal[i] != actual.Decimal[i])
                {
                    return false;
                }
            }

            if (expected.DecimalList.Count != actual.DecimalList.Count)
                return false;
            for (int i = 0; i < expected.DecimalList.Count; i++)
            {
                if (expected.DecimalList[i] != actual.DecimalList[i])
                {
                    return false;
                }
            }

            if (expected.Double.Length != actual.Double.Length)
                return false;
            for (int i = 0; i < expected.Double.Length; i++)
            {
                if (expected.Double[i] != actual.Double[i])
                {
                    return false;
                }
            }

            if (expected.DoubleList.Count != actual.DoubleList.Count)
                return false;
            for (int i = 0; i < expected.DoubleList.Count; i++)
            {
                if (expected.DoubleList[i] != actual.DoubleList[i])
                {
                    return false;
                }
            }

            if (expected.Float.Length != actual.Float.Length)
                return false;
            for (int i = 0; i < expected.Float.Length; i++)
            {
                if (expected.Float[i] != actual.Float[i])
                {
                    return false;
                }
            }

            if (expected.FloatList.Count != actual.FloatList.Count)
                return false;
            for (int i = 0; i < expected.FloatList.Count; i++)
            {
                if (expected.FloatList[i] != actual.FloatList[i])
                {
                    return false;
                }
            }

            if (expected.Int.Length != actual.Int.Length)
                return false;
            for (int i = 0; i < expected.Int.Length; i++)
            {
                if (expected.Int[i] != actual.Int[i])
                {
                    return false;
                }
            }

            if (expected.IntList.Count != actual.IntList.Count)
                return false;
            for (int i = 0; i < expected.IntList.Count; i++)
            {
                if (expected.IntList[i] != actual.IntList[i])
                {
                    return false;
                }
            }

            if (expected.UInt.Length != actual.UInt.Length)
                return false;
            for (int i = 0; i < expected.UInt.Length; i++)
            {
                if (expected.UInt[i] != actual.UInt[i])
                {
                    return false;
                }
            }

            if (expected.UIntList.Count != actual.UIntList.Count)
                return false;
            for (int i = 0; i < expected.UIntList.Count; i++)
            {
                if (expected.UIntList[i] != actual.UIntList[i])
                {
                    return false;
                }
            }

            if (expected.NInt.Length != actual.NInt.Length)
                return false;
            for (int i = 0; i < expected.NInt.Length; i++)
            {
                if (expected.NInt[i] != actual.NInt[i])
                {
                    return false;
                }
            }

            if (expected.NIntList.Count != actual.NIntList.Count)
                return false;
            for (int i = 0; i < expected.NIntList.Count; i++)
            {
                if (expected.NIntList[i] != actual.NIntList[i])
                {
                    return false;
                }
            }

            if (expected.NUInt.Length != actual.NUInt.Length)
                return false;
            for (int i = 0; i < expected.NUInt.Length; i++)
            {
                if (expected.NUInt[i] != actual.NUInt[i])
                {
                    return false;
                }
            }

            if (expected.NUIntList.Count != actual.NUIntList.Count)
                return false;
            for (int i = 0; i < expected.NUIntList.Count; i++)
            {
                if (expected.NUIntList[i] != actual.NUIntList[i])
                {
                    return false;
                }
            }

            if (expected.Long.Length != actual.Long.Length)
                return false;
            for (int i = 0; i < expected.Long.Length; i++)
            {
                if (expected.Long[i] != actual.Long[i])
                {
                    return false;
                }
            }

            if (expected.LongList.Count != actual.LongList.Count)
                return false;
            for (int i = 0; i < expected.LongList.Count; i++)
            {
                if (expected.LongList[i] != actual.LongList[i])
                {
                    return false;
                }
            }

            if (expected.ULong.Length != actual.ULong.Length)
                return false;
            for (int i = 0; i < expected.ULong.Length; i++)
            {
                if (expected.ULong[i] != actual.ULong[i])
                {
                    return false;
                }
            }

            if (expected.ULongList.Count != actual.ULongList.Count)
                return false;
            for (int i = 0; i < expected.ULongList.Count; i++)
            {
                if (expected.ULongList[i] != actual.ULongList[i])
                {
                    return false;
                }
            }

            if (expected.Short.Length != actual.Short.Length)
                return false;
            for (int i = 0; i < expected.Short.Length; i++)
            {
                if (expected.Short[i] != actual.Short[i])
                {
                    return false;
                }
            }

            if (expected.ShortList.Count != actual.ShortList.Count)
                return false;
            for (int i = 0; i < expected.ShortList.Count; i++)
            {
                if (expected.ShortList[i] != actual.ShortList[i])
                {
                    return false;
                }
            }

            if (expected.UShort.Length != actual.UShort.Length)
                return false;
            for (int i = 0; i < expected.UShort.Length; i++)
            {
                if (expected.UShort[i] != actual.UShort[i])
                {
                    return false;
                }
            }

            if (expected.UShortList.Count != actual.UShortList.Count)
                return false;
            for (int i = 0; i < expected.UShortList.Count; i++)
            {
                if (expected.UShortList[i] != actual.UShortList[i])
                {
                    return false;
                }
            }

            if (expected.String.Length != actual.String.Length)
                return false;
            for (int i = 0; i < expected.String.Length; i++)
            {
                if (!expected.String[i].Equals(actual.String[i]))
                {
                    return false;
                }
            }

            if (expected.StringList.Count != actual.StringList.Count)
                return false;
            for (int i = 0; i < expected.StringList.Count; i++)
            {
                if (!expected.StringList[i].Equals(actual.StringList[i]))
                {
                    return false;
                }
            }

            if (expected.Enum.Length != actual.Enum.Length)
                return false;
            for (int i = 0; i < expected.Enum.Length; i++)
            {
                if (expected.Enum[i] != actual.Enum[i])
                {
                    return false;
                }
            }

            if (expected.EnumList.Count != actual.EnumList.Count)
                return false;
            for (int i = 0; i < expected.EnumList.Count; i++)
            {
                if (expected.EnumList[i] != actual.EnumList[i])
                {
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Multidim IEnumarable Class
    /// </summary>
    public class MultiDimClassIEnumarble : ITestable<MultiDimClassIEnumarble>
    {
        [DotSerialName("0")]
        public int[][]? Int { get; set; }

        [DotSerialName("1")]
        public List<List<int>>? IntList { get; set; }

        [DotSerialName("2")]
        public int[][][]? IntThree { get; set; }

        [DotSerialName("3")]
        public List<List<List<int>>>? IntListThree { get; set; }

        [DotSerialName("4")]
        public List<int>[]? IntMix { get; set; }

        [DotSerialName("5")]
        public List<int[]>? IntListMix { get; set; }

        [DotSerialName("6")]
        public string[][]? String { get; set; }

        [DotSerialName("7")]
        public List<List<string>>? StringList { get; set; }

        [DotSerialName("8")]
        public string[][][]? StringThree { get; set; }

        [DotSerialName("9")]
        public List<List<List<string>>>? StringListThree { get; set; }

        [DotSerialName("10")]
        public List<string>[]? StringMix { get; set; }

        [DotSerialName("11")]
        public List<string[]>? StringListMix { get; set; }

        [DotSerialName("12")]
        public PrimitiveClass[][]? PrimitiveClassArray { get; set; }

        [DotSerialName("13")]
        public List<List<PrimitiveClass>>? PrimitiveClassList { get; set; }

        [DotSerialName("14")]
        public PrimitiveClass[][][]? PrimitiveClassArrayThree { get; set; }

        [DotSerialName("15")]
        public List<List<List<PrimitiveClass>>>? PrimitiveClassListThree { get; set; }

        [DotSerialName("16")]
        public List<PrimitiveClass>[]? PrimitiveClassArrayMix { get; set; }

        [DotSerialName("17")]
        public List<PrimitiveClass[]>? PrimitiveClassListMix { get; set; }

        [DotSerialName("18")]
        public TestEnum[][]? Enum { get; set; }

        [DotSerialName("19")]
        public List<List<TestEnum>>? EnumList { get; set; }

        [DotSerialName("20")]
        public TestEnum[][][]? EnumThree { get; set; }

        [DotSerialName("21")]
        public List<List<List<TestEnum>>>? EnumListThree { get; set; }

        [DotSerialName("22")]
        public List<TestEnum>[]? EnumMix { get; set; }

        [DotSerialName("23")]
        public List<TestEnum[]>? EnumListMix { get; set; }

        public static MultiDimClassIEnumarble CreateTestDefault()
        {
            var tmp = new MultiDimClassIEnumarble { Int = new int[3][] };
            tmp.Int[0] = [1, 2, 3];
            tmp.Int[1] = [3, 4, 5];
            tmp.Int[2] = [6, 7, 8];

            tmp.IntList =
            [
                [],
                [],
                [],
            ];
            tmp.IntList[2] = [1, 2, 3];
            tmp.IntList[1] = [3, 4, 5];
            tmp.IntList[0] = [6, 7, 8];

            tmp.IntThree = new int[3][][];
            tmp.IntThree[0] = new int[2][];
            tmp.IntThree[0][0] = [5, 7];
            tmp.IntThree[0][1] = [2, 7];
            tmp.IntThree[1] = new int[3][];
            tmp.IntThree[1][0] = [5, 67];
            tmp.IntThree[1][1] = [2, 17];
            tmp.IntThree[1][2] = [22, 7];
            tmp.IntThree[2] = new int[1][];
            tmp.IntThree[2][0] = [5, 6777];

            tmp.IntListThree = [];
            List<List<int>> tt0 = [];
            List<int> t0 = [5, 7];
            List<int> t1 = [2, 7];
            tt0.Add(t0);
            tt0.Add(t1);

            List<List<int>> tt1 = [];
            List<int> t2 = [5, 67];
            List<int> t3 = [2, 17];
            List<int> t4 = [22, 7];
            tt1.Add(t2);
            tt1.Add(t3);
            tt1.Add(t4);

            List<List<int>> tt2 = [];
            List<int> t5 = [5, 6777];
            tt2.Add(t5);

            tmp.IntListThree.Add(tt0);
            tmp.IntListThree.Add(tt1);
            tmp.IntListThree.Add(tt2);

            tmp.IntMix = new List<int>[2];
            tmp.IntMix[0] = [3, 4, 6666];
            tmp.IntMix[1] = [3222, 4, 6666, 664545];

            tmp.IntListMix =
            [
                [77, 888, 9999],
                [7, 88, 999],
            ];

            tmp.String = new string[3][];
            tmp.String[0] = ["1", "2", "3"];
            tmp.String[1] = ["11", "22", "33"];
            tmp.String[2] = ["111", "222", "333"];

            tmp.StringList =
            [
                [],
                [],
                [],
            ];
            tmp.StringList[2] = ["13", "23", "33"];
            tmp.StringList[1] = ["11", "21", "31"];
            tmp.StringList[0] = ["15", "25", "35"];

            tmp.StringThree = new string[3][][];
            tmp.StringThree[0] = new string[2][];
            tmp.StringThree[0][0] = ["asds", "asds"];
            tmp.StringThree[0][1] = ["asds", "asd333s"];
            tmp.StringThree[1] = new string[3][];
            tmp.StringThree[1][0] = ["a666sds", "asds"];
            tmp.StringThree[1][1] = ["asd434s", "asd22s"];
            tmp.StringThree[1][2] = ["asds", "asds"];
            tmp.StringThree[2] = new string[1][];
            tmp.StringThree[2][0] = ["asd2s", "a1sds"];

            tmp.StringListThree = [];
            List<List<string>> stt0 = [];
            List<string> st0 = ["as34s", "asd221s"];
            List<string> st1 = ["asd434s", "as111d22s"];
            stt0.Add(st0);
            stt0.Add(st1);

            List<List<string>> stt1 = [];
            List<string> st2 = ["asd434s", "asd122s"];
            List<string> st3 = ["assd434s", "as1d22s"];
            List<string> st4 = ["asd4d34s", "asd22s"];
            stt1.Add(st2);
            stt1.Add(st3);
            stt1.Add(st4);

            List<List<string>> stt2 = [];
            List<string> st5 = ["asssd434s", "asd22s"];
            stt2.Add(st5);

            tmp.StringListThree.Add(stt0);
            tmp.StringListThree.Add(stt1);
            tmp.StringListThree.Add(stt2);

            tmp.StringMix = new List<string>[2];
            tmp.StringMix[0] = ["3", "4", "6666"];
            tmp.StringMix[1] = ["3222", "4", "6666", "664545"];

            tmp.StringListMix =
            [
                ["77", "888", "9999"],
                ["7", "88", "2"],
            ];

            PrimitiveClass pDefault = PrimitiveClass.CreateTestDefault();
            tmp.PrimitiveClassArray = new PrimitiveClass[2][];
            tmp.PrimitiveClassArray[0] = [pDefault, pDefault, pDefault, pDefault];
            tmp.PrimitiveClassArray[1] = [pDefault, pDefault, pDefault, pDefault];

            tmp.PrimitiveClassList =
            [
                [],
                [],
            ];
            tmp.PrimitiveClassList[0] = [pDefault, pDefault, pDefault, pDefault];
            tmp.PrimitiveClassList[1] = [pDefault, pDefault, pDefault, pDefault];

            tmp.PrimitiveClassArrayThree = new PrimitiveClass[3][][];
            tmp.PrimitiveClassArrayThree[0] = new PrimitiveClass[2][];
            tmp.PrimitiveClassArrayThree[0][0] = [pDefault, pDefault];
            tmp.PrimitiveClassArrayThree[0][1] = [pDefault, pDefault];
            tmp.PrimitiveClassArrayThree[1] = new PrimitiveClass[3][];
            tmp.PrimitiveClassArrayThree[1][0] = [pDefault, pDefault];
            tmp.PrimitiveClassArrayThree[1][1] = [pDefault, pDefault];
            tmp.PrimitiveClassArrayThree[1][2] = [pDefault, pDefault];
            tmp.PrimitiveClassArrayThree[2] = new PrimitiveClass[1][];
            tmp.PrimitiveClassArrayThree[2][0] = [pDefault, pDefault];

            tmp.PrimitiveClassListThree = [];
            List<List<PrimitiveClass>> ptt0 = [];
            List<PrimitiveClass> pt0 = [pDefault, pDefault];
            List<PrimitiveClass> pt1 = [pDefault, pDefault];
            ptt0.Add(pt0);
            ptt0.Add(pt1);

            List<List<PrimitiveClass>> ptt1 = [];
            List<PrimitiveClass> pt2 = [pDefault, pDefault];
            List<PrimitiveClass> pt3 = [pDefault, pDefault];
            List<PrimitiveClass> pt4 = [pDefault, pDefault];
            ptt1.Add(pt2);
            ptt1.Add(pt3);
            ptt1.Add(pt4);

            List<List<PrimitiveClass>> ptt2 = [];
            List<PrimitiveClass> pt5 = [pDefault, pDefault];
            ptt2.Add(pt5);

            tmp.PrimitiveClassListThree.Add(ptt0);
            tmp.PrimitiveClassListThree.Add(ptt1);
            tmp.PrimitiveClassListThree.Add(ptt2);

            tmp.PrimitiveClassArrayMix = new List<PrimitiveClass>[2];
            tmp.PrimitiveClassArrayMix[0] = [pDefault, pDefault, pDefault];
            tmp.PrimitiveClassArrayMix[1] = [pDefault, pDefault, pDefault, pDefault];

            tmp.PrimitiveClassListMix =
            [
                [pDefault, pDefault],
                [pDefault, pDefault, pDefault],
            ];

            tmp.Enum = new TestEnum[3][];
            tmp.Enum[0] = [TestEnum.Second, TestEnum.First, TestEnum.Fourth];
            tmp.Enum[1] = [TestEnum.Undefined, TestEnum.Fourth, TestEnum.First];
            tmp.Enum[2] = [TestEnum.First, TestEnum.Fourth, TestEnum.Second];

            tmp.EnumList =
            [
                [],
                [],
                [],
            ];
            tmp.EnumList[2] = [TestEnum.First, TestEnum.Fourth, TestEnum.First];
            tmp.EnumList[1] = [TestEnum.First, TestEnum.First, TestEnum.Second];
            tmp.EnumList[0] = [TestEnum.First, TestEnum.First, TestEnum.Second];

            tmp.EnumThree = new TestEnum[3][][];
            tmp.EnumThree[0] = new TestEnum[2][];
            tmp.EnumThree[0][0] = [TestEnum.None, TestEnum.First];
            tmp.EnumThree[0][1] = [TestEnum.First, TestEnum.Fourth];
            tmp.EnumThree[1] = new TestEnum[3][];
            tmp.EnumThree[1][0] = [TestEnum.First, TestEnum.First];
            tmp.EnumThree[1][1] = [TestEnum.Second, TestEnum.First];
            tmp.EnumThree[1][2] = [TestEnum.First, TestEnum.First];
            tmp.EnumThree[2] = new TestEnum[1][];
            tmp.EnumThree[2][0] = [TestEnum.First, TestEnum.Undefined];

            tmp.EnumListThree = [];
            List<List<TestEnum>> ett0 = [];
            List<TestEnum> et0 = [TestEnum.Second, TestEnum.Second];
            List<TestEnum> et1 = [TestEnum.Second, TestEnum.Second];
            ett0.Add(et0);
            ett0.Add(et1);

            List<List<TestEnum>> ett1 = [];
            List<TestEnum> et2 = [TestEnum.Second, TestEnum.Second];
            List<TestEnum> et3 = [TestEnum.Second, TestEnum.Second];
            List<TestEnum> et4 = [TestEnum.Second, TestEnum.Second];
            ett1.Add(et2);
            ett1.Add(et3);
            ett1.Add(et4);

            List<List<TestEnum>> ett2 = [];
            List<TestEnum> et5 = [TestEnum.Second, TestEnum.Second];
            ett2.Add(et5);

            tmp.EnumListThree.Add(ett0);
            tmp.EnumListThree.Add(ett1);
            tmp.EnumListThree.Add(ett2);

            tmp.EnumMix = new List<TestEnum>[2];
            tmp.EnumMix[0] = [TestEnum.Second, TestEnum.Second, TestEnum.Second];
            tmp.EnumMix[1] = [TestEnum.First, TestEnum.Second, TestEnum.None, TestEnum.Second];

            tmp.EnumListMix =
            [
                [TestEnum.None, TestEnum.Second],
                [TestEnum.Fourth, TestEnum.Second, TestEnum.Fourth],
            ];

            return tmp;
        }

        public bool AssertTest(MultiDimClassIEnumarble actual)
        {
            // TODO...
            return true;
        }
    }

    /// <summary>
    /// Dictionary Class
    /// </summary>
    public class DictionaryClass() : ITestable<DictionaryClass>
    {
        [DotSerialName("0")]
        public Dictionary<int, int>? DicIntInt { get; set; }

        [DotSerialName("1")]
        public Dictionary<int, string?>? DicIntString { get; set; }

        [DotSerialName("2")]
        public Dictionary<string, int>? DicStringInt { get; set; }

        [DotSerialName("3")]
        public Dictionary<string, string>? DicStringString { get; set; }

        [DotSerialName("4")]
        public Dictionary<string, string>? DicEmpty { get; set; }

        [DotSerialName("5")]
        public Dictionary<string, string>? DicNull { get; set; }

        public static DictionaryClass CreateTestDefault()
        {
            var result = new DictionaryClass { DicIntInt = [] };
            result.DicIntInt.Add(33, 44);
            result.DicIntInt.Add(-33, 88);

            result.DicIntString = [];
            result.DicIntString.Add(3, "Moin");
            result.DicIntString.Add(2, null);
            result.DicIntString.Add(0, "asdasdasd");

            result.DicStringInt = [];
            result.DicStringInt.Add("sadasd", 44);
            result.DicStringInt.Add("sss", 311);

            result.DicStringString = [];
            result.DicStringString.Add("ssasdasd", "998989");
            result.DicStringString.Add("ssaaaasdasd", "998982229");

            result.DicEmpty = [];

            result.DicNull = null;

            return result;
        }

        public bool AssertTest(DictionaryClass actual)
        {
            var expected = this;

            if (expected.DicIntInt.Count != actual.DicIntInt.Count)
                return false;
            foreach (var keyValue in expected.DicIntInt)
            {
                if (actual.DicIntInt.TryGetValue(keyValue.Key, out var value))
                {
                    if (keyValue.Value != value)
                        return false;
                }
                else
                {
                    return false;
                }
            }

            if (expected.DicIntString.Count != actual.DicIntString.Count)
                return false;
            foreach (var keyValue in expected.DicIntString)
            {
                if (actual.DicIntString.TryGetValue(keyValue.Key, out var value))
                {
                    if (keyValue.Value == null)
                    {
                        if (value != null)
                            return false;

                        continue;
                    }
                    if (false == keyValue.Value.Equals(value))
                        return false;
                }
                else
                {
                    return false;
                }
            }

            if (expected.DicStringInt.Count != actual.DicStringInt.Count)
                return false;
            foreach (var keyValue in expected.DicStringInt)
            {
                if (actual.DicStringInt.TryGetValue(keyValue.Key, out var value))
                {
                    if (keyValue.Value != value)
                        return false;
                }
                else
                {
                    return false;
                }
            }

            if (expected.DicStringString.Count != actual.DicStringString.Count)
                return false;
            foreach (var keyValue in expected.DicStringString)
            {
                if (actual.DicStringString.TryGetValue(keyValue.Key, out var value))
                {
                    if (false == keyValue.Value.Equals(value))
                        return false;
                }
                else
                {
                    return false;
                }
            }

            if (expected.DicEmpty.Count != actual.DicEmpty.Count)
                return false;

            if (expected.DicNull != null || actual.DicNull != null)
                return false;

            return true;
        }
    }

    /// <summary>
    /// Class with an enum
    /// </summary>
    public class EnumClass() : ITestable<EnumClass>
    {
        [DotSerialName("0")]
        public TestEnum TestEnum0 { get; set; }

        [DotSerialName("1")]
        public TestEnum TestEnum1 { get; set; }

        [DotSerialName("2")]
        public TestEnum TestEnum2 { get; set; }

        [DotSerialName("3")]
        public TestEnum[]? TestEnumArray { get; set; }

        public static EnumClass CreateTestDefault()
        {
            var tmp = new EnumClass
            {
                TestEnum0 = TestEnum.Fourth,
                TestEnum1 = TestEnum.Undefined,
                TestEnum2 = TestEnum.First,
                TestEnumArray = [TestEnum.Fourth, TestEnum.Fourth, TestEnum.First],
            };

            return tmp;
        }

        public bool AssertTest(EnumClass actual)
        {
            var expected = this;

            if (expected.TestEnum0 != actual.TestEnum0)
                return false;
            if (expected.TestEnum1 != actual.TestEnum1)
                return false;
            if (expected.TestEnum2 != actual.TestEnum2)
                return false;
            if (expected.TestEnumArray.Length != actual.TestEnumArray.Length)
                return false;
            for (int i = 0; i < expected.TestEnumArray.Length; i++)
            {
                if (expected.TestEnumArray[i] != actual.TestEnumArray[i])
                    return false;
            }

            return true;
        }
    }

    public class DateTimeClass() : ITestable<DateTimeClass>
    {
        [DotSerialName("0")]
        public DateTime Date1 { get; set; }

        [DotSerialName("1")]
        public DateTime Date2 { get; set; }

        [DotSerialName("2")]
        public DateTime Date3 { get; set; }

        [DotSerialName("3")]
        public DateTime[]? DateArray { get; set; }

        [DotSerialName("4")]
        public Dictionary<int, DateTime>? DateDic { get; set; }

        public static DateTimeClass CreateTestDefault()
        {
            var tmp = new DateTimeClass
            {
                Date1 = DateTime.MinValue,
                Date2 = DateTime.MaxValue,
                Date3 = DateTime.MinValue,
                DateArray = [DateTime.MaxValue, DateTime.MinValue, DateTime.MinValue],
                DateDic = [],
            };
            tmp.DateDic.Add(5, DateTime.MaxValue);
            tmp.DateDic.Add(10, DateTime.MaxValue);

            return tmp;
        }

        public bool AssertTest(DateTimeClass actual)
        {
            var expected = this;

            if (!expected.Date1.ToString().Equals(actual.Date1.ToString()))
                return false;
            if (!expected.Date2.ToString().Equals(actual.Date2.ToString()))
                return false;
            if (!expected.Date3.ToString().Equals(actual.Date3.ToString()))
                return false;
            if (expected.DateArray.Length != actual.DateArray.Length)
                return false;
            for (int i = 0; i < expected.DateArray.Length; i++)
            {
                if (!expected.DateArray[i].ToString().Equals(actual.DateArray[i].ToString()))
                    return false;
            }

            if (expected.DateDic.Count != actual.DateDic.Count)
                return false;
            foreach (var keyValue in expected.DateDic)
            {
                if (actual.DateDic.TryGetValue(keyValue.Key, out var value))
                {
                    if (!keyValue.Value.ToString().Equals(value.ToString()))
                        return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Struct Class
    /// </summary>
    public class StructClass() : ITestable<StructClass>
    {
        [DotSerialName("0")]
        public TestStruct TestStruct0 { get; set; }

        [DotSerialName("1")]
        public TestStruct TestStruct1 { get; set; }

        [DotSerialName("2")]
        public TestStruct TestStruct2 { get; set; }

        [DotSerialName("3")]
        public TestStruct[]? TestStructArray { get; set; }

        public static StructClass CreateTestDefault()
        {
            var tmp = new StructClass
            {
                TestStruct0 = new TestStruct(55, 3),
                TestStruct1 = new TestStruct(5, 3),
                TestStruct2 = new TestStruct(55, 32),
                TestStructArray = [new TestStruct(6, 5), new TestStruct(4, 3), new TestStruct(2, 1)],
            };

            return tmp;
        }

        public bool AssertTest(StructClass actual)
        {
            var expected = this;

            if (expected.TestStruct0.Value0 != actual.TestStruct0.Value0)
                return false;
            if (expected.TestStruct0.Value1 != actual.TestStruct0.Value1)
                return false;
            if (expected.TestStruct1.Value0 != actual.TestStruct1.Value0)
                return false;
            if (expected.TestStruct1.Value1 != actual.TestStruct1.Value1)
                return false;
            if (expected.TestStruct2.Value0 != actual.TestStruct2.Value0)
                return false;
            if (expected.TestStruct2.Value1 != actual.TestStruct2.Value1)
                return false;
            if (expected.TestStructArray.Length != actual.TestStructArray.Length)
                return false;
            for (int i = 0; i < expected.TestStructArray.Length; i++)
            {
                if (expected.TestStructArray[i].Value0 != actual.TestStructArray[i].Value0)
                    return false;
                if (expected.TestStructArray[i].Value1 != actual.TestStructArray[i].Value1)
                    return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Duplicate ID class
    /// </summary>
    public class DuplicateIDClass
    {
        [DotSerialName("1893")]
        public int Value0 { get; set; }

        [DotSerialName("1893")]
        public int Value1 { get; set; }
    }

    /// <summary>
    /// Class with invalid id
    /// </summary>
    public class InvalidIDClass()
    {
        [DotSerialName("")]
        public int Value0 { get; set; }
    }

    /// <summary>
    /// Record Class
    /// </summary>
    public class RecordClass() : ITestable<RecordClass>
    {
        [DotSerialName("0")]
        public TestRecord? TestRecord0 { get; set; }

        [DotSerialName("1")]
        public TestRecord? TestRecord1 { get; set; }

        [DotSerialName("2")]
        public TestRecord? TestRecord2 { get; set; }

        [DotSerialName("3")]
        public TestRecord[]? TestRecordArray { get; set; }

        public static RecordClass CreateTestDefault()
        {
            var tmp = new RecordClass
            {
                TestRecord0 = new TestRecord(55, 3),
                TestRecord1 = new TestRecord(5, 3),
                TestRecord2 = new TestRecord(55, 32),
                TestRecordArray = [new TestRecord(6, 5), new TestRecord(4, 3), new TestRecord(2, 1)],
            };

            return tmp;
        }

        public bool AssertTest(RecordClass actual)
        {
            var expected = this;

            if (expected.TestRecord0.Value0 != actual.TestRecord0.Value0)
                return false;
            if (expected.TestRecord0.Value1 != actual.TestRecord0.Value1)
                return false;
            if (expected.TestRecord1.Value0 != actual.TestRecord1.Value0)
                return false;
            if (expected.TestRecord1.Value1 != actual.TestRecord1.Value1)
                return false;
            if (expected.TestRecord2.Value0 != actual.TestRecord2.Value0)
                return false;
            if (expected.TestRecord2.Value1 != actual.TestRecord2.Value1)
                return false;
            if (expected.TestRecordArray.Length != actual.TestRecordArray.Length)
                return false;
            for (int i = 0; i < expected.TestRecordArray.Length; i++)
            {
                if (expected.TestRecordArray[i].Value0 != actual.TestRecordArray[i].Value0)
                    return false;
                if (expected.TestRecordArray[i].Value1 != actual.TestRecordArray[i].Value1)
                    return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Class with parsable objects
    /// </summary>
    public class ParsableClass() : ITestable<ParsableClass>
    {
        [DotSerialName("0")]
        public DateTime DateTime0 { get; set; }

        [DotSerialName("1")]
        public Guid Guid0 { get; set; }

        [DotSerialName("2")]
        public TimeSpan TimeSpan0 { get; set; }

        [DotSerialName("3")]
        public Uri? Uri0 { get; set; }

        [DotSerialName("4")]
        public IPAddress? IPAddress0 { get; set; }

        [DotSerialName("5")]
        public Version? Version0 { get; set; }

        [DotSerialName("6")]
        public CultureInfo? CultureInfo0 { get; set; }

        public static ParsableClass CreateTestDefault()
        {
            var tmp = new ParsableClass
            {
                DateTime0 = DateTime.MaxValue,
                Guid0 = Guid.NewGuid(),
                TimeSpan0 = TimeSpan.Zero,
                Uri0 = new Uri("http://www.contoso.com/"),
                IPAddress0 = IPAddress.Parse("127.0.0.1"),
                Version0 = new Version(1, 2, 3),
                CultureInfo0 = new CultureInfo("en-US"),
            };

            return tmp;
        }

        public bool AssertTest(ParsableClass actual)
        {
            var expected = this;

            if (!expected.DateTime0.ToString().Equals(actual.DateTime0.ToString()))
                return false;
            if (!expected.Guid0.ToString().Equals(actual.Guid0.ToString()))
                return false;
            if (!expected.TimeSpan0.ToString().Equals(actual.TimeSpan0.ToString()))
                return false;
            if (!expected.Uri0.ToString().Equals(actual.Uri0.ToString()))
                return false;
            if (!expected.IPAddress0.ToString().Equals(actual.IPAddress0.ToString()))
                return false;
            if (!expected.Version0.ToString().Equals(actual.Version0.ToString()))
                return false;
            if (!expected.CultureInfo0.ToString().Equals(actual.CultureInfo0.ToString()))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Path class
    /// </summary>
    public class PathClass : ITestable<PathClass>
    {
        [DotSerialName("0")]
        public string? Path1 { get; set; }

        [DotSerialName("1")]
        public string? Path2 { get; set; }

        [DotSerialName("2")]
        public string? Path3 { get; set; }

        public static PathClass CreateTestDefault()
        {
            var tmp = new PathClass
            {
                Path1 = @"C:\Users\Dennis\AppData\Roaming\cbkit\cbkit.json",
                Path2 = "C:\\Users\\Dennis\\AppData\\Roaming\\cbkit\\cbkit.json",
                Path3 = "C:/Users/Dennis/AppData/Roaming/cbkit/cbkit.json",
            };
            return tmp;
        }

        public bool AssertTest(PathClass actual)
        {
            var expected = this;

            if (!expected.Path1.ToString().Equals(actual.Path1.ToString()))
                return false;
            if (!expected.Path2.ToString().Equals(actual.Path2.ToString()))
                return false;
            if (!expected.Path3.ToString().Equals(actual.Path3.ToString()))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Class without a parameterless constructor.
    /// </summary>
    public class ClassWithoutParameterlessConstructor(string name) : ITestable<ClassWithoutParameterlessConstructor>
    {
        [DotSerialName("0")]
        public string Name { get; set; } = name;

        public static ClassWithoutParameterlessConstructor CreateTestDefault()
        {
            var tmp = new ClassWithoutParameterlessConstructor("test");
            return tmp;
        }

        public bool AssertTest(ClassWithoutParameterlessConstructor actual)
        {
            var expected = this;

            if (!expected.Name.ToString().Equals(actual.Name.ToString()))
                return false;

            return true;
        }
    }

    /// <summary>
    /// Class with struct without parameterless constructor.
    /// </summary>
    public class ClassRecordNoParameterlessConstructor : ITestable<ClassRecordNoParameterlessConstructor>
    {
        [DotSerialName("1893")]
        public TestRecordNoParameterlessConstructor? Value0 { get; set; }

        public static ClassRecordNoParameterlessConstructor CreateTestDefault()
        {
            var tmp = new ClassRecordNoParameterlessConstructor
            {
                Value0 = new TestRecordNoParameterlessConstructor(5, 33),
            };

            return tmp;
        }

        public bool AssertTest(ClassRecordNoParameterlessConstructor actual)
        {
            var expected = this;

            if (expected.Value0.Value0 != actual.Value0.Value0)
                return false;
            if (expected.Value0.Value1 != actual.Value0.Value1)
                return false;

            return true;
        }
    }

    public class EmptyObjectClass : ITestable<EmptyObjectClass>
    {
        [DotSerialName("1")]
        public SimpleClass[]? EmptyArray { get; set; }

        [DotSerialName("2")]
        public Dictionary<int, SimpleClass>? EmptyDictionary { get; set; }

        [DotSerialName("3")]
        public string? StringEmpty { get; set; }

        public static EmptyObjectClass CreateTestDefault()
        {
            var tmp = new EmptyObjectClass
            {
                EmptyArray = [],
                EmptyDictionary = [],
                StringEmpty = string.Empty,
            };
            return tmp;
        }

        public bool AssertTest(EmptyObjectClass actual)
        {
            if (actual.EmptyArray == null || actual.EmptyArray.Length != 0)
                return false;
            if (actual.EmptyDictionary == null || actual.EmptyDictionary.Count != 0)
                return false;
            if (actual.StringEmpty == null || !actual.StringEmpty.Equals(string.Empty))
                return false;

            return true;
        }
    }

    public class ClassWithOneList : ITestable<ClassWithOneList>
    {
        [DotSerialName("0")]
        public bool[]? Boolean { get; set; }

        public static ClassWithOneList CreateTestDefault()
        {
            var tmp = new ClassWithOneList { Boolean = [true, true, false, true] };
            return tmp;
        }

        public bool AssertTest(ClassWithOneList actual)
        {
            var expected = this;

            if (expected.Boolean.Length != actual.Boolean.Length)
                return false;

            for (int i = 0; i < expected.Boolean.Length; i++)
            {
                if (expected.Boolean[i] != actual.Boolean[i])
                    return false;
            }

            return true;
        }
    }

    public class ClassWithOneDictionary : ITestable<ClassWithOneDictionary>
    {
        [DotSerialName("0")]
        public Dictionary<int, int>? DicIntInt { get; set; }

        public static ClassWithOneDictionary CreateTestDefault()
        {
            var tmp = new ClassWithOneDictionary { DicIntInt = [] };

            tmp.DicIntInt.Add(33, 44);
            tmp.DicIntInt.Add(-33, 88);

            return tmp;
        }

        public bool AssertTest(ClassWithOneDictionary actual)
        {
            var expected = this;

            if (expected.DicIntInt.Count != actual.DicIntInt.Count)
                return false;
            foreach (var keyValue in expected.DicIntInt)
            {
                if (actual.DicIntInt.TryGetValue(keyValue.Key, out var value))
                {
                    if (keyValue.Value != value)
                        return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }

    public class ClassWithOnePrimitive : ITestable<ClassWithOnePrimitive>
    {
        [DotSerialName("0")]
        public int Value { get; set; }

        public static ClassWithOnePrimitive CreateTestDefault()
        {
            var tmp = new ClassWithOnePrimitive { Value = 55 };

            return tmp;
        }

        public bool AssertTest(ClassWithOnePrimitive actual)
        {
            var expected = this;

            if (expected.Value != actual.Value)
                return false;

            return true;
        }
    }

    public class ClassSpecialCharsKeys : ITestable<ClassSpecialCharsKeys>
    {
        public int Value { get; set; }

        [DotSerialName("1234")]
        public int KeyAsInt { get; set; }

        [DotSerialName("12.34")]
        public int KeyAsDouble { get; set; }

        [DotSerialName("-1234")]
        public int KeyAsIntNegative { get; set; }

        [DotSerialName("-12.34")]
        public int KeyAsDoubleNegative { get; set; }

        [DotSerialName("true")]
        public int KeyAsTrue { get; set; }

        [DotSerialName("false")]
        public int KeyAsFalse { get; set; }

        [DotSerialName("null")]
        public int KeyAsNull { get; set; }

        [DotSerialName("Hello World")]
        public int KeyWithWhiteSpaceMiddle { get; set; }

        [DotSerialName(" HelloWorld")]
        public int KeyWithWhiteSpaceStart { get; set; }

        [DotSerialName("HelloWorld ")]
        public int KeyWithWhiteSpaceEnd { get; set; }

        [DotSerialName(" HelloWorld ")]
        public int KeyWithWhiteSpaceStartEnd { get; set; }

        [DotSerialName("[{;,!? \\§$%&'\"|-.}]")]
        public int KeyWithSpecialChars1 { get; set; }

        [DotSerialName(";,.-'!")]
        public int KeyWithSpecialChars2 { get; set; }

        [DotSerialName("<><:;-?!#!")]
        public int KeyWithSpecialChars3 { get; set; }

        [DotSerialName("{}<><:;-?!#!")]
        public int KeyWithSpecialChars4 { get; set; }

        [DotSerialName("   ")]
        public int KeyWithOnlyWhiteSpaces { get; set; }

        [DotSerialName("[ \"\\[]")]
        public string[]? Array { get; set; }

        [DotSerialName("{}<><:;[]-?!#!")]
        public Dictionary<string, string>? Dictionary { get; set; }

        // [DotSerialName("")]
        // public string? StringEmpty { get; set; }

        public static ClassSpecialCharsKeys CreateTestDefault()
        {
            var tmp = new ClassSpecialCharsKeys
            {
                Value = 55,
                KeyAsInt = 1,
                KeyAsDouble = 2,
                KeyAsIntNegative = 3,
                KeyAsDoubleNegative = 4,
                KeyAsTrue = 5,
                KeyAsFalse = 6,
                KeyAsNull = 7,
                KeyWithWhiteSpaceMiddle = 8,
                KeyWithWhiteSpaceStart = 9,
                KeyWithWhiteSpaceEnd = 10,
                KeyWithWhiteSpaceStartEnd = 11,
                KeyWithSpecialChars1 = 12,
                KeyWithSpecialChars2 = 13,
                KeyWithSpecialChars3 = 14,
                KeyWithSpecialChars4 = 15,
                KeyWithOnlyWhiteSpaces = 16,
                Array = ["[{;,! \\§$%&'\"|-.}]", ";,.-'!", "{}<><:;-?!#!", "   ", "[ \"\\[]"],
                Dictionary = [],
                // StringEmpty = ""
            };

            tmp.Dictionary.Add("[{;,!? \\§$%&'\"|-.}]", ";,.-'!");
            tmp.Dictionary.Add("   ", ";,.-'!");
            tmp.Dictionary.Add("[ \"\\[]", "[ \"\\[]");

            return tmp;
        }

        public bool AssertTest(ClassSpecialCharsKeys actual)
        {
            var expected = this;

            if (expected.Value != actual.Value)
                return false;
            if (expected.KeyAsInt != actual.KeyAsInt)
                return false;
            if (expected.KeyAsDouble != actual.KeyAsDouble)
                return false;
            if (expected.KeyAsIntNegative != actual.KeyAsIntNegative)
                return false;
            if (expected.KeyAsDoubleNegative != actual.KeyAsDoubleNegative)
                return false;
            if (expected.KeyAsTrue != actual.KeyAsTrue)
                return false;
            if (expected.KeyAsFalse != actual.KeyAsFalse)
                return false;
            if (expected.KeyAsNull != actual.KeyAsNull)
                return false;
            if (expected.KeyWithWhiteSpaceMiddle != actual.KeyWithWhiteSpaceMiddle)
                return false;
            if (expected.KeyWithWhiteSpaceStart != actual.KeyWithWhiteSpaceStart)
                return false;
            if (expected.KeyWithWhiteSpaceEnd != actual.KeyWithWhiteSpaceEnd)
                return false;
            if (expected.KeyWithWhiteSpaceStartEnd != actual.KeyWithWhiteSpaceStartEnd)
                return false;
            if (expected.KeyWithSpecialChars1 != actual.KeyWithSpecialChars1)
                return false;
            if (expected.KeyWithSpecialChars2 != actual.KeyWithSpecialChars2)
                return false;
            if (expected.KeyWithSpecialChars3 != actual.KeyWithSpecialChars3)
                return false;
            if (expected.KeyWithOnlyWhiteSpaces != actual.KeyWithOnlyWhiteSpaces)
                return false;
            if (expected.Array.Length != actual.Array.Length)
                return false;
            for (int i = 0; i < expected.Array.Length; i++)
            {
                if (!expected.Array[i].Equals(actual.Array[i]))
                    return false;
            }
            if (expected.Dictionary.Count != actual.Dictionary.Count)
                return false;
            foreach (var keyValue in expected.Dictionary)
            {
                if (actual.Dictionary.TryGetValue(keyValue.Key, out var value))
                {
                    if (keyValue.Value != value)
                        return false;
                }
                else
                {
                    return false;
                }
            }
            // if (expected.StringEmpty != actual.StringEmpty)
            //     return false;

            return true;
        }
    }

    public class ClassSpecialCharsValue : ITestable<ClassSpecialCharsValue>
    {
        public int Value { get; set; }
        public string? StringAsInt { get; set; }
        public string? StringAsDouble { get; set; }
        public string? StringAsIntNegative { get; set; }
        public string? StringAsDoubleNegative { get; set; }
        public string? StringAsTrue { get; set; }
        public string? StringAsFalse { get; set; }
        public string? StringAsNull { get; set; }
        public string? StringAsEmpty { get; set; }
        public string? StringWithWhiteSpaceMiddle { get; set; }
        public string? StringWithWhiteSpaceStart { get; set; }
        public string? StringWithWhiteSpaceEnd { get; set; }
        public string? StringWithWhiteSpaceStartEnd { get; set; }
        public string? SpecialChars1 { get; set; }
        public string? SpecialChars2 { get; set; }
        public string? SpecialChars3 { get; set; }
        public string? SpecialChars4 { get; set; }
        public string? StringWithOnlyWhiteSpaces { get; set; }
        public string[]? Array { get; set; }
        public Dictionary<string, string>? Dictionary { get; set; }
        public string? StringEmpty { get; set; }

        public static ClassSpecialCharsValue CreateTestDefault()
        {
            var tmp = new ClassSpecialCharsValue
            {
                Value = 55,
                StringAsInt = "1234",
                StringAsDouble = "12.34",
                StringAsIntNegative = "-1234",
                StringAsDoubleNegative = "-12.34",
                StringAsTrue = "true",
                StringAsFalse = "false",
                StringAsNull = "null",
                StringAsEmpty = string.Empty,
                StringWithWhiteSpaceMiddle = "Hello  World",
                StringWithWhiteSpaceStart = " HelloWorld",
                StringWithWhiteSpaceEnd = "HelloWorld ",
                StringWithWhiteSpaceStartEnd = " HelloWorld ",
                SpecialChars1 = "[{;,!? \\§$%&'\"|-.}]",
                SpecialChars2 = ";,.-'!",
                SpecialChars3 = "<><:;-?!#!",
                SpecialChars4 = "{}<><:;-?!#!",
                StringWithOnlyWhiteSpaces = "   ",
                Array = ["[{;,!? \\§$%&'\"|-.}]", ";,.-'!", "{}<><:;-?!#!", "   ", "[ \"\\[]"],
                Dictionary = [],
                StringEmpty = "",
            };

            tmp.Dictionary.Add("[{;,!? \\§$%&'\"|-.}]", ";,.-'!");
            tmp.Dictionary.Add("   ", ";,.-'!");
            tmp.Dictionary.Add("[ \"\\[]", "[ \"\\[]");

            return tmp;
        }

        public bool AssertTest(ClassSpecialCharsValue actual)
        {
            var expected = this;

            if (expected.Value != actual.Value)
                return false;
            if (!expected.StringAsInt.Equals(actual.StringAsInt))
                return false;
            if (!expected.StringAsDouble.Equals(actual.StringAsDouble))
                return false;
            if (!expected.StringAsIntNegative.Equals(actual.StringAsIntNegative))
                return false;
            if (!expected.StringAsDoubleNegative.Equals(actual.StringAsDoubleNegative))
                return false;
            if (!expected.StringAsTrue.Equals(actual.StringAsTrue))
                return false;
            if (!expected.StringAsFalse.Equals(actual.StringAsFalse))
                return false;
            if (!expected.StringAsNull.Equals(actual.StringAsNull))
                return false;
            if (!expected.StringAsEmpty.Equals(actual.StringAsEmpty))
                return false;
            if (!expected.StringWithWhiteSpaceMiddle.Equals(actual.StringWithWhiteSpaceMiddle))
                return false;
            if (!expected.StringWithWhiteSpaceStart.Equals(actual.StringWithWhiteSpaceStart))
                return false;
            if (!expected.StringWithWhiteSpaceEnd.Equals(actual.StringWithWhiteSpaceEnd))
                return false;
            if (!expected.StringWithWhiteSpaceStartEnd.Equals(actual.StringWithWhiteSpaceStartEnd))
                return false;
            if (!expected.SpecialChars1.Equals(actual.SpecialChars1))
                return false;
            if (!expected.SpecialChars2.Equals(actual.SpecialChars2))
                return false;
            if (!expected.SpecialChars3.Equals(actual.SpecialChars3))
                return false;
            if (!expected.SpecialChars4.Equals(actual.SpecialChars4))
                return false;
            if (!expected.StringWithOnlyWhiteSpaces.Equals(actual.StringWithOnlyWhiteSpaces))
                return false;
            if (expected.Array.Length != actual.Array.Length)
                return false;
            for (int i = 0; i < expected.Array.Length; i++)
            {
                if (!expected.Array[i].Equals(actual.Array[i]))
                    return false;
            }
            if (expected.Dictionary.Count != actual.Dictionary.Count)
                return false;
            foreach (var keyValue in expected.Dictionary)
            {
                if (actual.Dictionary.TryGetValue(keyValue.Key, out var value))
                {
                    if (keyValue.Value != value)
                        return false;
                }
                else
                {
                    return false;
                }
            }
            if (expected.StringEmpty != actual.StringEmpty)
                return false;

            return true;
        }
    }

    #region Helper Class, Enum, Struct, ...

    /// <summary>
    /// Enum for Tests
    /// </summary>
    public enum TestEnum
    {
        Undefined = -1,
        None = 0,
        First = 1,
        Second = 2,
        Fourth = 4,
    }

    /// <summary>
    /// Test Struct
    /// </summary>
    public struct TestStruct(int x, int y)
    {
        [DotSerialName("0")]
        public int Value0 { get; set; } = x;

        [DotSerialName("1")]
        public int Value1 { get; set; } = y;
    }

    /// <summary>
    /// Record
    /// </summary>
    public record TestRecord
    {
        [DotSerialName("0")]
        public int Value0 { get; set; }

        [DotSerialName("1")]
        public int Value1 { get; set; }

        public TestRecord() { }

        public TestRecord(int x, int y)
        {
            Value0 = x;
            Value1 = y;
        }
    }

    /// <summary>
    /// Record without parameterless constructor
    /// </summary>
    public record TestRecordNoParameterlessConstructor
    {
        [DotSerialName("0")]
        public int Value0 { get; set; }

        [DotSerialName("1")]
        public int Value1 { get; set; }

        public TestRecordNoParameterlessConstructor(int x, int y)
        {
            Value0 = x;
            Value1 = y;
        }
    }

    #endregion

    #region CurrentlyNotSupported

    public class NotSupportedTypeClassHashTable : ITestable<NotSupportedTypeClassHashTable>
    {
        [DotSerialName("1893")]
        public Hashtable? Value0 { get; set; }

        public static NotSupportedTypeClassHashTable CreateTestDefault()
        {
            var tmp = new NotSupportedTypeClassHashTable { Value0 = new Hashtable() };
            tmp.Value0.Add(1, 10);
            tmp.Value0.Add(2, 20);
            tmp.Value0.Add(3, 30);
            return tmp;
        }

        public bool AssertTest(NotSupportedTypeClassHashTable actual)
        {
            var expected = this;
            if (expected.Value0.Count != actual.Value0.Count)
                return false;
            if (expected.Value0[1] != actual.Value0[1])
                return false;
            if (expected.Value0[2] != actual.Value0[2])
                return false;
            if (expected.Value0[3] != actual.Value0[3])
                return false;
            return true;
        }
    }

    #endregion

#pragma warning restore CS8602
#pragma warning restore CS8604
}
