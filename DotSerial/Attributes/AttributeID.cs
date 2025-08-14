namespace DotSerial.Attributes
{ 
    /// <summary> Parameter ID (class)
    /// </summary>
    /// <param name="id">ID</param>
    [AttributeUsage(AttributeTargets.Class)]
    public class SerialzeClassIDAttribute(int id) : Attribute
    {
        public int ClassID { get; private set; } = id;
    }

    /// <summary> Parameter ID (Property)
    /// </summary>
    /// <param name="id">ID</param>
    [AttributeUsage(AttributeTargets.Property)]
    public class SerialzeParaIDAttribute(int id) : Attribute
    {
        public int ParaID { get; private set; } = id;
    }

}
