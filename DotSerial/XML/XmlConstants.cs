#region License
//Copyright (c) 2025 Dennis Sölch

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
#endregion

namespace DotSerial.XML
{
    public static class XmlConstants
    {
        /// <summary>
        /// Indentation Size
        /// </summary>
        internal const int IndentationSize = 2;
        /// <summary>
        /// XML Declaration
        /// </summary>
        internal const string XmlDeclaration = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
        /// <summary>
        /// XMl Tag opening char
        /// </summary>
        internal const char XmlTagOpening = '<';
        /// <summary>
        /// XML Tag closing char
        /// </summary>
        internal const char XmlTagClosing = '>';
        /// <summary>
        /// XML Tag end char
        /// </summary>
        internal const char XmlTagEnd = '/';
        /// <summary>
        /// XML Attribute assignment char
        /// </summary>
        internal const char XmlAttributeAssignment = '=';    
        /// <summary>
        /// XML display attribute key
        /// </summary>
        public const string XmlAttributeKey = "key";
        /// <summary>
        /// XML InnerNode display
        /// </summary>
        public const string XmlInnerNodeProp = "InnerNode";
        /// <summary>
        /// XML Leaf display
        /// </summary>
        public const string XmlLeafProp = "Leaf";
        /// <summary>
        /// XML List Value display
        /// </summary>
        public const string XmlListProp = "List";    
    }
}
