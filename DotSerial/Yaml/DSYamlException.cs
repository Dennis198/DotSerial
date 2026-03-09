using DotSerial.Common;

namespace DotSerial.Yaml
{
    /// <summary>
    /// Exception for yaml.
    /// </summary>
    [Serializable()]
    public class DSYamlException : DotSerialException
    {
        public DSYamlException(string message) : base (message){}
    }
}