
namespace DotSerial.Core.Exceptions
{
    [Serializable()]
    public class DuplicateIDException : Exception
    {
        public DuplicateIDException() : base("An id must be unique within an object") { }

        public DuplicateIDException(int id) : base(string.Format("Id {0} must not exist more than once inside an object.", id))
        {}
    }
}
