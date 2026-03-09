namespace DotSerial.Attributes
{
    /// <summary> Attribute "DotSerialName"
    /// </summary>
    /// <param name="name">Custom name for the property</param>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DotSerialNameAttribute(string name) : Attribute
    {
        /// <summary>
        /// Custom name of the property.
        /// </summary>
        public string Name { get; private set; } = name;
    }
}
