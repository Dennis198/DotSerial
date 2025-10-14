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

using DotSerial.Core.General;

namespace DotSerial.Core.Exceptions.Node
{
    [Serializable()]
    public class DSInvalidNodeTypeException : Exception
    {
        public DSInvalidNodeTypeException() : base("Node type is not supported for this funtion.") { }
        public DSInvalidNodeTypeException(int children, int expected) : base(string.Format("'{0}' number of children, expected: '{1}'", children, expected)) { }

        public DSInvalidNodeTypeException(DSNodeType nodeType) : base(string.Format("Node type {0} is not supported for this funtion.", nodeType.ConvertToString())) { }

        public DSInvalidNodeTypeException(string str) : base(string.Format("Node prop type {0} is not supported for this funtion.", str)) { }
        public DSInvalidNodeTypeException(DSNodePropertyType nodePropType) : base(string.Format("Node prop type {0} is not supported for this funtion.", nodePropType.ConvertToString())) { }
    }
}
