
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
        //[SerialzePropertyID(0)]
        //public SimpleClass[] Array { get; set; }
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
                UShort = 30
            };

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