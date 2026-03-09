namespace DotSerial.Common
{
    /// <summary>
    /// Universal excpetion
    /// </summary>
    [Serializable()]
    public class DotSerialException : Exception
    {
        internal DotSerialException(string message)
            : base(message) { }
    }
}
