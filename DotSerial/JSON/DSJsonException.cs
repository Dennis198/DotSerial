using DotSerial.Common;

namespace DotSerial.Json
{
    [Serializable()]
    public class DSJsonException : DotSerialException
    {
        public DSJsonException(string message) : base (message){}
    }
}