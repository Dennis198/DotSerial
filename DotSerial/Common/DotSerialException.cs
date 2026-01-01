namespace DotSerial.Common
{
    [Serializable()]
    public abstract class DotSerialException : Exception
    {
        public DotSerialException(string message) : base (message){}
    }
}