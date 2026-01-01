using DotSerial.Common;

namespace DotSerial.Json
{
    [Serializable()]
    public abstract class DSJsonException : DotSerialException
    {
        public DSJsonException(string message) : base (message){}
    }
}