using DotSerial.Common;

namespace DotSerial.Xml
{
    /// <summary>
    /// Exception for xml.
    /// </summary>
    [Serializable()]
    public class DSXmlException : DotSerialException
    {
        public DSXmlException(string message)
            : base(message) { }
    }
}
