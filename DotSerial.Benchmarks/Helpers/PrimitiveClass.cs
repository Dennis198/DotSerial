namespace DotSerial.Benchmarks.Helpers
{
    internal class PrimitiveClass
    {
        public bool Boolean { get; set; }
        public byte Byte { get; set; }
        public sbyte SByte { get; set; }
        public char Char { get; set; }
        public decimal Decimal { get; set; }
        public double Double { get; set; }
        public float Float { get; set; }
        public int Int { get; set; }
        public uint UInt { get; set; }
        public nint NInt { get; set; }
        public nuint NUInt { get; set; }
        public long Long { get; set; }
        public ulong ULong { get; set; }
        public short Short { get; set; }
        public ushort UShort { get; set; }
        public string? String { get; set; }

        internal static PrimitiveClass Create()
        {
            var result = new PrimitiveClass
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
            };

            return result;
        }
    }
}
