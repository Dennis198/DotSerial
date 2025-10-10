
namespace DotSerial.Core.General
{
    internal enum DSNodeType
    {
        Root = 0,
        InnerNode = 1,
        Leaf = 2,
    }

    internal enum DSNodePropertyType
    {
        Undefined = -1,
        Primitive = 0,
        Class = 1,
        List = 2,
        Dictionary = 3,
        KeyValuePair = 4,
        KeyValuePairKey = 5,
        KeyValuePairValue = 6,
        Null = 7
    }
}
