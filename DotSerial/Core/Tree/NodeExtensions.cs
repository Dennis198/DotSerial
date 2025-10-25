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

using System.Text;
using DotSerial.Core.Exceptions.Node;

namespace DotSerial.Core.Tree
{
    internal static  class NodeExtensions
    {
        /// <summary>
        /// Connvert enum to string
        /// </summary>
        /// <param name="type">node type</param>
        /// <returns>String</returns>
        internal static string ConvertToString(this DSNodeType type)
        {
            return type switch
            {
                DSNodeType.Root => "Root",
                DSNodeType.InnerNode => "Inner node",
                DSNodeType.Leaf => "Leaf",
                _ => throw new DSInvalidNodeTypeException(type),
            };
        }

        /// <summary>
        /// Convert string to enum
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>DSNodePropertyType</returns>
        internal static DSNodePropertyType ConvertToDSNodePropertyType(this string str)
        {
            return str switch
            {
                "Undefined" => DSNodePropertyType.Undefined,
                "Primitive" => DSNodePropertyType.Primitive,
                "Class" => DSNodePropertyType.Class,
                "List" => DSNodePropertyType.List,
                "Dictionary" => DSNodePropertyType.Dictionary,
                "KeyValuePair" => DSNodePropertyType.KeyValuePair,
                "KeyValuePairValue" => DSNodePropertyType.KeyValuePairValue,
                "Null" => DSNodePropertyType.Null,
                _ => throw new ArgumentException(nameof(ConvertToString)),
            };
        }

        /// <summary>
        /// Convert node to string
        /// </summary>
        /// <param name="propType"></param>
        /// <returns>String</returns>
        internal static string ConvertToString(this DSNodePropertyType propType)
        {
            return propType switch
            {
                DSNodePropertyType.Undefined => "Undefined",
                DSNodePropertyType.Primitive => "Primitive",
                DSNodePropertyType.Class => "Class",
                DSNodePropertyType.List => "List",
                DSNodePropertyType.Dictionary => "Dictionary",
                DSNodePropertyType.KeyValuePair => "KeyValuePair",
                DSNodePropertyType.KeyValuePairValue => "KeyValuePairValue",
                DSNodePropertyType.Null => "Null",
                _ => throw new DSInvalidNodeTypeException(propType),
            };
        }

    }
}
