
using DotSerial.Attributes;

namespace DotSerial.Tests.Core.XML
{
    /// <summary>
    /// Empty Class
    /// </summary>
    public class EmptyClass
    {}
    
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
    }
}