
namespace DotSerial.Core.Exceptions
{
    [Serializable()]
    public class DSNoParameterlessConstructorDefinedException : Exception
    {
        public DSNoParameterlessConstructorDefinedException() : base("Type need a parameterless constructor for deserialization") { }

        public DSNoParameterlessConstructorDefinedException(string type) : base(string.Format("{0} type need a parameterless constructor for deserialization", type))
        { }
    }
}
