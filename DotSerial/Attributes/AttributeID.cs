namespace DotSerial.Attributes
{ 
    /// <summary> Parameter ID (Property)
    /// </summary>
    /// <param name="id">ID</param>
    [AttributeUsage(AttributeTargets.Property)]
    public class DSPropertyIDAttribute(int id) : Attribute
    {
        public int PropertyID { get; private set; } = id;
    }

}
