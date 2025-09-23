
namespace DotSerial.Core.XML
{
    public static class Constants
    {

        #region Attribute

        /// <summary>
        /// XML id attribute display
        /// </summary>
        public const string XmlAttributeID = "id";
        /// <summary>
        /// XML display attribute display
        /// </summary>
        public const string XmlAttributeName = "name";
        /// <summary>
        /// XML id attribute version
        /// </summary>
        public const string XmlAttributeVersion = "version";
        /// <summary>
        /// XML id attribute version
        /// </summary>
        public const string XmlAttributeProducer = "producer";

        #endregion

        /// <summary>
        /// XML Object display
        /// </summary>
        public const string DotSerial = "DotSerial";
        /// <summary>
        /// XML Object display
        /// </summary>
        public const string Object = "Object";
        /// <summary>
        /// XML Parameter display
        /// </summary>
        public const string Property = "Parameter";
        /// <summary>
        /// XML List Value display
        /// </summary>
        public const string List = "List";
        /// <summary>
        /// XML List Value display
        /// </summary>
        public const string Null = "Null";
        /// <summary>
        /// XML Dictionary display
        /// </summary>
        public const string Dictionary = "Dictionary";
        /// <summary>
        /// XML Dictionary KeyValuepair display
        /// </summary>
        public const string KeyValuePair = "KeyValuePair";
        /// <summary>
        /// XML Dictionary Key display
        /// </summary>
        public const string Key = "Key";
        /// <summary>
        /// XML Dictionary Value display
        /// </summary>
        public const string Value = "Value";

        #region Default Values

        /// <summary>
        /// String for null objects
        /// </summary>
        public const string NullString = "{null}";
        /// <summary>
        /// MainObject ID
        /// </summary>
        public const int MainObjectID = 0;
        /// <summary>
        /// ID for property without id
        /// </summary>
        public const int NoAttributeID = -1;
        /// <summary>
        /// ID for key in dictionary
        /// </summary>
        public const int DicKeyID = 0;
        /// <summary>
        /// ID for key in value
        /// </summary>
        public const int DicKeyValueID = 1;

        #endregion       
    }
}
