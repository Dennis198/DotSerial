using DotSerial.Common;

namespace DotSerial.Json
{
    /// <summary>
    /// Exception for json.
    /// </summary>
    [Serializable()]
    public class DSJsonException : DotSerialException
    {
        public DSJsonException(string message)
            : base(message) { }
    }
}
