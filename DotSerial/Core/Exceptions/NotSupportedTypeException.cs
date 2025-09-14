
namespace DotSerial.Core.Exceptions
{
    [Serializable()]
    public class NotSupportedTypeException : Exception
    {
        public NotSupportedTypeException() : base("Type is not supported.") { }

        public NotSupportedTypeException(Type t) : base(string.Format("Type {0} is not supported.", t.Name))
        { }
    }
}
