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
