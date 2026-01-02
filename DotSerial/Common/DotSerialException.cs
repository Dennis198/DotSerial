namespace DotSerial.Common
{
    [Serializable()]
    public class DotSerialException : Exception
    {
        public DotSerialException(string message) : base (message){}
    }
}