
namespace DotSerial.Core.Exceptions
{
    [Serializable()]
    public class NoParameterlessConstructorDefinedException : Exception
    {
        public NoParameterlessConstructorDefinedException() : base("Type need a parameterless constructor for deserialization") { }

        public NoParameterlessConstructorDefinedException(string type) : base(string.Format("{0} type need a parameterless constructor for deserialization", type))
        { }
    }
}
