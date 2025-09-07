
using DotSerial.Attributes;

namespace DotSerial.Core.Exceptions
{
    [Serializable()]
    public class DuplicateIDException : Exception
    {
        public DuplicateIDException() : base(string.Format("{0} must be unique within an object", nameof(DSPropertyIDAttribute))) { }

        public DuplicateIDException(int id) : base(string.Format("{0} {1} must not exist more than once inside an object.", nameof(DSPropertyIDAttribute), id))
        {}
    }
}
