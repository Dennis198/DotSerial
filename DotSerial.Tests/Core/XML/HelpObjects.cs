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
        [SerialzePropertyID(0)]
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
    [SerialzeClassID(42)]
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

    /// <summary>
    /// Multidim IEnumarable Class
    /// </summary>
    public class MultiDimClassIEnumarble
    {
        [SerialzePropertyID(0)]
        public int[][] Int { get; set; }
        [SerialzePropertyID(1)]
        public List<List<int>> IntList{ get; set; }
        [SerialzePropertyID(2)]
        public int[][][] IntThree { get; set; }
        [SerialzePropertyID(3)]
        public List<List<List<int>>> IntListThree { get; set; }
        [SerialzePropertyID(4)]
        public List<int>[] IntMix { get; set; }
        [SerialzePropertyID(5)]
        public List<int[]> IntListMix { get; set; }

        [SerialzePropertyID(6)]
        public string[][] String { get; set; }
        [SerialzePropertyID(7)]
        public List<List<string>> StringList { get; set; }
        [SerialzePropertyID(8)]
        public string[][][] StringThree { get; set; }
        [SerialzePropertyID(9)]
        public List<List<List<string>>> StringListThree { get; set; }
        [SerialzePropertyID(10)]
        public List<string>[] StringMix { get; set; }
        [SerialzePropertyID(11)]
        public List<string[]> StringListMix { get; set; }

        [SerialzePropertyID(12)]
        public PrimitiveClass[][] PrimitiveClassArray { get; set; }
        [SerialzePropertyID(13)]
        public List<List<PrimitiveClass>> PrimitiveClassList { get; set; }
        [SerialzePropertyID(14)]
        public PrimitiveClass[][][] PrimitiveClassArrayThree { get; set; }
        [SerialzePropertyID(15)]
        public List<List<List<PrimitiveClass>>> PrimitiveClassListThree { get; set; }
        [SerialzePropertyID(16)]
        public List<PrimitiveClass>[] PrimitiveClassArrayMix { get; set; }
        [SerialzePropertyID(17)]
        public List<PrimitiveClass[]> PrimitiveClassListMix { get; set; }

        [SerialzePropertyID(18)]
        public TestEnum[][] Enum { get; set; }
        [SerialzePropertyID(19)]
        public List<List<TestEnum>> EnumList { get; set; }
        [SerialzePropertyID(20)]
        public TestEnum[][][] EnumThree { get; set; }
        [SerialzePropertyID(21)]
        public List<List<List<TestEnum>>> EnumListThree { get; set; }
        [SerialzePropertyID(22)]
        public List<TestEnum>[] EnumMix { get; set; }
        [SerialzePropertyID(23)]
        public List<TestEnum[]> EnumListMix { get; set; }

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

            tmp.IntListMix = [[77, 888, 9999], [7, 88, 999]];

            tmp.String = new string[3][];
            tmp.String[0] = ["1", "2", "3"];
            tmp.String[1] = ["11", "22", "33"];
            tmp.String[2] = ["111", "222", "333"];

            tmp.StringList = [[], [], []];
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

            tmp.StringListMix = [["77", "888", "9999"], ["7", "88", "2"]];

            PrimitiveClass pDefault = PrimitiveClass.CreateTestDefault();
            tmp.PrimitiveClassArray = new PrimitiveClass[2][];
            tmp.PrimitiveClassArray[0] = [pDefault, pDefault, pDefault, pDefault];
            tmp.PrimitiveClassArray[1] = [pDefault, pDefault, pDefault, pDefault];

            tmp.PrimitiveClassList = [[], []];
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

            tmp.PrimitiveClassListMix = [[pDefault, pDefault], [pDefault, pDefault, pDefault]];

            tmp.Enum = new TestEnum[3][];
            tmp.Enum[0] = [TestEnum.Second, TestEnum.First, TestEnum.Fourth];
            tmp.Enum[1] = [TestEnum.Undefined, TestEnum.Fourth, TestEnum.First];
            tmp.Enum[2] = [TestEnum.First, TestEnum.Fourth, TestEnum.Second];

            tmp.EnumList = [[], [], []];
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

            tmp.EnumListMix = [[TestEnum.None, TestEnum.Second], [TestEnum.Fourth, TestEnum.Second, TestEnum.Fourth]];

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