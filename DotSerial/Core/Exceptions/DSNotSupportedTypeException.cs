
namespace DotSerial.Core.Exceptions
{
    [Serializable()]
    public class DSNotSupportedTypeException : Exception
    {
        public DSNotSupportedTypeException() : base("Type is not supported.") { }

        public DSNotSupportedTypeException(string str) : base(str) { }

        public DSNotSupportedTypeException(Type t) : base(string.Format("Type {0} is not supported.", t.Name))
        { }

        public DSNotSupportedTypeException(Type t, Type itemT) : base(string.Format("Item type {0} is not supported in type {1}.", itemT.Name, t.Name))
        { }
    }
}
