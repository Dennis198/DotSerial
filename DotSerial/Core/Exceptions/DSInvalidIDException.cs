
using DotSerial.Attributes;

namespace DotSerial.Core.Exceptions
{
    [Serializable()]
    public class DSInvalidIDException : Exception
    {
        public DSInvalidIDException() : base(string.Format("Invalid {0} id.", nameof(DSPropertyIDAttribute))) { }

        public DSInvalidIDException(int id) : base(string.Format("Invalid {0} value. ({1})", nameof(DSPropertyIDAttribute), id))
        { }
    }
}
