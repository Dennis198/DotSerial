using DotSerial.Common;

namespace DotSerial.Xml
{
    /// <summary>
    /// Xml constants
    /// </summary>
    internal static class XmlConstants
    {
        /// <summary>
        /// Indentation Size
        /// </summary>
        internal const int IndentationSize = 2;
        /// <summary>
        /// Xml Declaration
        /// </summary>
        internal const string XmlDeclaration = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        /// <summary>
        /// XMl Tag opening char
        /// </summary>
        internal const char XmlTagOpening = '<';
        /// <summary>
        /// Xml Tag closing char
        /// </summary>
        internal const char XmlTagClosing = '>';
        /// <summary>
        /// Xml Tag end char
        /// </summary>
        internal const char XmlTagEnd = '/';
        /// <summary>
        /// Xml Attribute assignment char
        /// </summary>
        internal const char XmlAttributeAssignment = '=';    
        /// <summary>
        /// Xml display attribute key
        /// </summary>
        internal const string XmlAttributeKey = "key";
        /// <summary>
        /// Xml InnerNode display
        /// </summary>
        internal const string XmlInnerNodeProp = "InnerNode";
        /// <summary>
        /// Xml Leaf display
        /// </summary>
        internal const string XmlLeafProp = "Leaf";
        /// <summary>
        /// Xml List Value display
        /// </summary>
        internal const string XmlListProp = "List";    
        /// <summary>
        /// Chars which must be escaped
        /// </summary>
        internal readonly static char[] CharsToEscape = [CommonConstants.Quote, CommonConstants.Backslash];                
    }
}
