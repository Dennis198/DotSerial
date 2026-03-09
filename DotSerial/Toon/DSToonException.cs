using DotSerial.Common;

namespace DotSerial.Toon
{
     /// <summary>
    /// Exception for toon.
    /// </summary>
    [Serializable()]
    public class DSToonException : DotSerialException
    {
        public DSToonException(string message) : base (message){}
    }
}