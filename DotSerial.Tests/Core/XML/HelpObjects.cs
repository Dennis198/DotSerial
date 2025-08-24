using System.Collections.ObjectModel;

using DotSerial.Attributes;

namespace DotSerial.Tests.Core.XML
{
    /// <summary>
    /// Empty Class
    /// </summary>
    public class EmptyClass
    {}

    /// <summary>
    /// No Attribute Class
    /// </summary>
    public class NoAttributeClass
    {
        [SerialzePropertyID(0)]
        public PrimitiveClass? PrimitiveClass { get; set; }
        [SerialzePropertyID(1)]
        public bool Boolean { get; set; }

        public bool BooleanNoAttribute { get; set; }
        public int IntNoAttribute { get; set; }
        public string StringNoAttribute { get; set; }
        public SimpleClass SimpleClassNoAttribute { get; set; }
        public PrimitiveClass? PrimitiveClassNoAttribute { get; set; }
        public TestEnum EnumNoAttribute { get; set; }
        public int[] ArrayNoAttribute { get; set; }
        public List<int> ListNoAttribute { get; set; }

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
                ListNoAttribute = [9, 8, 7, 6]
            };

            return tmp;
        }

    }

    /// <summary>
    /// Null Class
    /// </summary>
    public class NullClass
    {
        [SerialzePropertyID(0)]
        public SimpleClass SimpleClass { get; set; }
        [SerialzePropertyID(1)]
        public SimpleClass[] Array { get; set; }
        [SerialzePropertyID(2)]
        public List<SimpleClass> List { get; set; }
        [SerialzePropertyID(3)]
        public SimpleClass[] ArrayWithNulls { get; set; }
        [SerialzePropertyID(4)]
        public List<SimpleClass> ListWithNulls { get; set; }
        [SerialzePropertyID(5)]
        public string String { get; set; }
        [SerialzePropertyID(6)]
        public bool[] BooleanArray { get; set; }
        [SerialzePropertyID(7)]
        public List<bool> BooleanList { get; set; }
        [SerialzePropertyID(8)]
        public string[] StringArray { get; set; }
        [SerialzePropertyID(9)]
        public List<string> StringList { get; set; }
        [SerialzePropertyID(10)]
        public string[] StringArrayWithNulls { get; set; }
        [SerialzePropertyID(11)]
        public List<string> StringListWithNulls { get; set; }
        [SerialzePropertyID(12)]
        public TestEnum[] EnumArray { get; set; }
        [SerialzePropertyID(13)]
        public List<TestEnum> EnumList { get; set; }

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
            };

            return tmp;
        }
    }

    /// <summary>
    /// Simple Class
    /// </summary>
    [SerialzeClassID(0)]
    public class SimpleClass
    {
        [SerialzePropertyID(0)]
        public bool Boolean { get; set; }
    }

    /// <summary>
    /// Genric IEnumerable Class
    /// </summary>
    public class IEnumerableClass
    {
        [SerialzePropertyID(0)]
        public SimpleClass[] Array { get; set; }
        [SerialzePropertyID(1)]
        public List<SimpleClass> List { get; set; }
        [SerialzePropertyID(2)]
        public Collection<SimpleClass> Collection { get; set; }
    }

    /// <summary>
    /// Nested Class
    /// </summary>
    public class NestedClass()
    {
        [SerialzePropertyID(0)]
        public PrimitiveClass? PrimitiveClass { get; set; }
        [SerialzePropertyID(1)]
        public bool Boolean { get; set; }
    }

    /// <summary>
    /// Nested Nested Class
    /// </summary>
    public class NestedNestedClass()
    {
        [SerialzePropertyID(0)]
        public NestedClass? NestedClass { get; set; }
        [SerialzePropertyID(1)]
        public PrimitiveClass? PrimitiveClass { get; set; }
        [SerialzePropertyID(2)]
        public bool Boolean { get; set; }
    }
    
    /// <summary>
    /// Class with all primitives
    /// </summary>
    public class PrimitiveClass
    {
        [SerialzePropertyID(0)]
        public bool Boolean { get; set; }
        [SerialzePropertyID(1)]
        public byte Byte { get; set; }
        [SerialzePropertyID(2)]
        public sbyte SByte { get; set; }
        [SerialzePropertyID(3)]
        public char Char { get; set; }
        [SerialzePropertyID(4)]
        public decimal Decimal { get; set; }
        [SerialzePropertyID(5)]
        public double Double { get; set; }
        [SerialzePropertyID(6)]
        public float Float { get; set; }
        [SerialzePropertyID(7)]
        public int Int { get; set; }
        [SerialzePropertyID(8)]
        public uint UInt { get; set; }
        [SerialzePropertyID(9)]
        public nint NInt { get; set; }
        [SerialzePropertyID(10)]
        public nuint NUInt { get; set; }
        [SerialzePropertyID(11)]
        public long Long { get; set; }
        [SerialzePropertyID(12)]
        public ulong ULong { get; set; }
        [SerialzePropertyID(13)]
        public short Short { get; set; }
        [SerialzePropertyID(14)]
        public ushort UShort { get; set; }
        [SerialzePropertyID(15)]
        public string String { get; set; }
        [SerialzePropertyID(16)]
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
                Enum = TestEnum.First
            };

            return tmp;
        }
    }

    /// <summary>
    /// Class with all primitives in arrays/list
    /// </summary>
    public class PrimitiveClassIEnumarable
    {
        [SerialzePropertyID(0)]
        public bool[] Boolean { get; set; }
        [SerialzePropertyID(1)]
        public List<bool> BooleanList { get; set; }
        [SerialzePropertyID(2)]
        public byte[] Byte { get; set; }
        [SerialzePropertyID(3)]
        public List<byte> ByteList { get; set; }
        [SerialzePropertyID(4)]
        public sbyte[] SByte { get; set; }
        [SerialzePropertyID(5)]
        public List<sbyte> SByteList { get; set; }
        [SerialzePropertyID(6)]
        public char[] Char { get; set; }
        [SerialzePropertyID(7)]
        public List<char> CharList { get; set; }
        [SerialzePropertyID(8)]
        public decimal[] Decimal { get; set; }
        [SerialzePropertyID(9)]
        public List<decimal> DecimalList { get; set; }
        [SerialzePropertyID(10)]
        public double[] Double { get; set; }
        [SerialzePropertyID(11)]
        public List<double> DoubleList { get; set; }
        [SerialzePropertyID(12)]
        public float[] Float { get; set; }
        [SerialzePropertyID(13)]
        public List<float> FloatList { get; set; }
        [SerialzePropertyID(14)]
        public int[] Int { get; set; }
        [SerialzePropertyID(15)]
        public List<int> IntList { get; set; }
        [SerialzePropertyID(16)]
        public uint[] UInt { get; set; }
        [SerialzePropertyID(17)]
        public List<uint> UIntList { get; set; }
        [SerialzePropertyID(18)]
        public nint[] NInt { get; set; }
        [SerialzePropertyID(19)]
        public List<nint> NIntList { get; set; }
        [SerialzePropertyID(20)]
        public nuint[] NUInt { get; set; }
        [SerialzePropertyID(21)]
        public List<nuint> NUIntList { get; set; }
        [SerialzePropertyID(22)]
        public long[] Long { get; set; }
        [SerialzePropertyID(23)]
        public List<long> LongList { get; set; }
        [SerialzePropertyID(24)]
        public ulong[] ULong { get; set; }
        [SerialzePropertyID(25)]
        public List<ulong> ULongList { get; set; }
        [SerialzePropertyID(26)]
        public short[] Short { get; set; }
        [SerialzePropertyID(27)]
        public List<short> ShortList { get; set; }
        [SerialzePropertyID(28)]
        public ushort[] UShort { get; set; }
        [SerialzePropertyID(29)]
        public List<ushort> UShortList { get; set; }
        [SerialzePropertyID(30)]
        public string[] String { get; set; }
        [SerialzePropertyID(31)]
        public List<string> StringList { get; set; }
        [SerialzePropertyID(32)]
        public TestEnum[] Enum { get; set; }
        [SerialzePropertyID(33)]
        public List<TestEnum> EnumList { get; set; }

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
    }

    public class MultiDimClassIEnumarble
    {
        [SerialzePropertyID(0)]
        public int[][] Int { get; set; }
        [SerialzePropertyID(1)]
        public List<List<int>> IntList{ get; set; }
        [SerialzePropertyID(2)]
        public string[][] String { get; set; }
        [SerialzePropertyID(3)]
        public List<List<string>> StringList { get; set; }
        [SerialzePropertyID(4)]
        public PrimitiveClass[][] PrimitiveClassArray { get; set; }
        [SerialzePropertyID(5)]
        public List<List<PrimitiveClass>> PrimitiveClassList { get; set; }
        [SerialzePropertyID(6)]
        public TestEnum[][] Enum { get; set; }
        [SerialzePropertyID(7)]
        public List<List<TestEnum>> EnumList { get; set; }

        public static MultiDimClassIEnumarble CreateTestDefault()
        {
            var tmp = new MultiDimClassIEnumarble();

            tmp.Int = new int[3][];
            tmp.Int[0] = [1, 2, 3];
            tmp.Int[1] = [3, 4, 5];
            tmp.Int[2] = [6, 7, 8];

            tmp.IntList = [[], [], []];
            tmp.IntList[2] = [1, 2, 3];
            tmp.IntList[1] = [3, 4, 5];
            tmp.IntList[0] = [6, 7, 8];

            tmp.String = new string[3][];
            tmp.String[0] = ["1", "2", "3"];
            tmp.String[1] = ["11", "22", "33"];
            tmp.String[2] = ["111", "222", "333"];

            tmp.StringList = [[], [], []];
            tmp.StringList[2] = ["13", "23", "33"];
            tmp.StringList[1] = ["11", "21", "31"];
            tmp.StringList[0] = ["15", "25", "35"];

            PrimitiveClass pDefault = PrimitiveClass.CreateTestDefault();
            tmp.PrimitiveClassArray = new PrimitiveClass[2][];
            tmp.PrimitiveClassArray[0] = [pDefault, pDefault, pDefault, pDefault];
            tmp.PrimitiveClassArray[1] = [pDefault, pDefault, pDefault, pDefault];

            tmp.PrimitiveClassList = [[], []];
            tmp.PrimitiveClassList[0] = [pDefault, pDefault, pDefault, pDefault];
            tmp.PrimitiveClassList[1] = [pDefault, pDefault, pDefault, pDefault];

            tmp.Enum = new TestEnum[3][];
            tmp.Enum[0] = [TestEnum.Second, TestEnum.First, TestEnum.Fourth];
            tmp.Enum[1] = [TestEnum.Undefined, TestEnum.Fourth, TestEnum.First];
            tmp.Enum[2] = [TestEnum.First, TestEnum.Fourth, TestEnum.Second];

            tmp.EnumList = [[], [], []];
            tmp.EnumList[2] = [TestEnum.First, TestEnum.Fourth, TestEnum.First];
            tmp.EnumList[1] = [TestEnum.First, TestEnum.First, TestEnum.Second];
            tmp.EnumList[0] = [TestEnum.First, TestEnum.First, TestEnum.Second];

            return tmp;
        }
    }

    /// <summary>
    /// Class with an enum
    /// </summary>
    public class EnumClass()
    {
        [SerialzePropertyID(0)]
        public TestEnum TestEnum0 { get; set; }
        [SerialzePropertyID(1)]
        public TestEnum TestEnum1 { get; set; }
        [SerialzePropertyID(2)]
        public TestEnum TestEnum2 { get; set; }
    }

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
}