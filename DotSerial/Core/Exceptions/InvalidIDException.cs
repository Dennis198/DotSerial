
using DotSerial.Attributes;

namespace DotSerial.Core.Exceptions
{
    [Serializable()]
    public class InvalidIDException : Exception
    {
        public InvalidIDException() : base(string.Format("Invalid {0} id.", nameof(DSPropertyIDAttribute))) { }

        public InvalidIDException(int id) : base(string.Format("Invalid {0} value. ({1})", nameof(DSPropertyIDAttribute), id))
        { }
    }
}
