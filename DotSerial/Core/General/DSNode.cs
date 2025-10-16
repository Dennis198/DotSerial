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

using DotSerial.Core.Exceptions;
using DotSerial.Core.Exceptions.Node;
using DotSerial.Core.Misc;
using System.Diagnostics;

namespace DotSerial.Core.General
{
    /// <summary>
    /// Node of a tree
    ///     (node)
    ///       |
    ///  -------------
    ///  |     |     |
    /// (A)   (B)   (C) (Children)
    /// </summary>
    [DebuggerDisplay("Key = {Key}, Value = {Value}, IsNull = {IsNull}, IsEmpty = {IsEmpty}, Children = {Count}")]
    internal class DSNode(string key)
    {
        /// <summary>
        /// Key of the node
        /// </summary>
        public string Key { get; private set; } = key;

        /// <summary>
        /// Value of the node
        /// </summary>
        public string? Value { get; private set; }

        /// <summary>
        /// Type node
        /// </summary>
        public DSNodeType Type { get; private set; }

        /// <summary>
        /// Type of Proprty which is represented with this node.
        /// </summary>
        public DSNodePropertyType PropType { get; private set; }

        /// <summary>
        /// Children
        /// </summary>
        public List<DSNode> Children { get; private set; } = [];

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="type">Type</param>
        public DSNode(string key, DSNodeType type, DSNodePropertyType propType) : this(key)
        {
            Type = type;
            SetPropertyType(propType);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="type">Type</param>
        public DSNode(string key, object? value, DSNodeType type, DSNodePropertyType propType) : this(key, type, propType)
        {
            SetValue(value);
        }

        /// <summary>
        /// True, if Node has Null Value and is a leaf
        /// </summary>
        public bool IsNull 
        {
            get
            {
                return null == Value && Type == DSNodeType.Leaf;
            }
        }

        /// <summary>
        /// Truem if node no children but is NOT a leaf
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Children.Count == 0 && Type != DSNodeType.Leaf;
            }
        }

        /// <summary>
        /// True, if Node has children
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return Children.Count > 0;
            }
        }

        /// <summary>
        /// Get number of children
        /// </summary>
        public int Count
        {
            get
            {
                return Children.Count;
            }
        }

        /// <summary>
        /// Returns the child nodes of this child
        /// </summary>
        /// <returns>Dictionary<int, DSNode></returns>
        public List<DSNode> GetChildren()
        {
            return Children;
        }

        /// <summary>
        /// Returns the first child node
        /// </summary>
        /// <returns>DSNode</returns>
        public DSNode GetFirstChild()
        {
            return GetNthChild(0);
        }

        /// <summary>
        /// Returns the n-th child node
        /// </summary>
        /// <param name="n">Nth</param>
        /// <returns>DSNode</returns>       
        public DSNode GetNthChild(int n)
        {
            if (Children.Count < n + 1)
            {
                throw new IndexOutOfRangeException();
            }

            return Children[n];
        }

        /// <summary>
        /// Changes the property type of the node
        /// </summary>
        /// <param name="propType">DSNodePropertyType</param>
        public void SetPropertyType(DSNodePropertyType propType)
        {
            if (Type == DSNodeType.Leaf)
            {
                // Make sure type is suitable for node
                if (propType != DSNodePropertyType.Primitive &&
                    propType != DSNodePropertyType.Null &&
                    propType != DSNodePropertyType.KeyValuePairValue)
                {
                    throw new DSInvalidNodeTypeException(propType);
                }

                // If node is leaf and value is null proptype
                // will be automatically put to Null.
                if (Type == DSNodeType.Leaf && Value == null)
                {
                    PropType = DSNodePropertyType.Null;
                }
                else
                {
                    PropType = propType;
                }
            }
            else
            {
                // Make sure type is suitable for node
                if (propType == DSNodePropertyType.Primitive || propType == DSNodePropertyType.Null)
                {
                    throw new DSInvalidNodeTypeException(propType);
                }

                PropType = propType;
            }

        }

        /// <summary>
        /// Converts the value of the node to a specific type.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Object?</returns>
        public object? ConvertValue(Type type)
        {
            if (HasChildren)
            {
                throw new DSInvalidNodeTypeException(this.Type);
            }

            if (TypeCheckMethods.IsPrimitive(type))
            {
                return ConverterMethods.ConvertStringToPrimitive(Value, type);
            }
            else if (TypeCheckMethods.IsSpecialParsableObject(type))
            {
                return ConverterMethods.ConvertStringToSpecialParsableObject(Value, type);
            }
            else
            {
                throw new DSNotSupportedTypeException(type);
            }
        }

        /// <summary>
        /// Gets child node
        /// </summary>
        /// <param name="key">Key of the child node</param>
        /// <returns>Node</returns>
        public DSNode GetChild(string key)
        {
            foreach(var child in Children)
            {
                if (child.Key.Equals(key))
                {
                    return child;
                }
            }

            throw new DSNodeKeyNotFoundException(key);
        }

        /// <summary>
        /// Append child node
        /// </summary>
        /// <param name="node">Child node</param>
        public void AppendChild(DSNode node)
        {
            // Can't append child to a leaf node
            if (Value != null)
            {
                throw new DSInvalidNodeTypeException(this.Type);
            }

            string key = node.Key;

            // Key is already taken
            foreach (var child in Children)
            {
                if (child.Key.Equals(key))
                {
                    throw new DSDuplicateNodeKeyException(key);
                }
            }

            Children.Add(node);
        }

        /// <summary>
        /// Gets the height of the node
        /// </summary>
        /// <returns>Height</returns>
        public int GetHeight()
        {
            if (Children.Count == 0)
            {
                return 1;
            }

            int max = -1;
            foreach(var tmp in Children)
            {
                DSNode child = tmp;
                int tmpHeight = child.GetHeight();
                if (tmpHeight > max)
                {
                    max = tmpHeight;
                }
            }

            if (max < 1)
            {
                throw new InvalidOperationException();
            }

            return max + 1;
        }

        /// <summary>
        /// Returns true, if List represents list of primitives
        /// </summary>
        /// <returns>Bool</returns>
        public bool IsPrimitiveList()
        {
            if (PropType != DSNodePropertyType.List)
            {
                return false;
            }

            if (IsNull || IsEmpty || !HasChildren)
            {
                return false;
            }

            var child = Children[0];

            if (child.IsNull || child.Type == DSNodeType.Leaf)
            {
                return true;
            }

            return false;

        }

        /// <summary>
        /// Returns true, if List represents dictionary of primitives
        /// </summary>
        /// <returns>Bool</returns>
        public bool IsPrimitiveDictionary()
        {
            if (PropType != DSNodePropertyType.Dictionary)
            {
                return false;
            }

            if (IsNull || IsEmpty || !HasChildren)
            {
                return false;
            }

            var keyValuePair = Children[0];
            var value = keyValuePair.Children[0];

            if (value.IsNull || value.Type == DSNodeType.Leaf)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the value
        /// </summary>
        /// <param name="value">Value</param>
        private void SetValue(object? value)
        {
            if (value == null)
            {
                Value = null;
            }
            else
            {
                string strValue;
                Type type = value.GetType();
                if (type == typeof(bool))
                {
                    int tmp = HelperMethods.BoolToInt((bool)value);
                    strValue = tmp.ToString();
                }
                else if (type.IsEnum)
                {
                    strValue = Convert.ToString((int)value);
                }
                else if (type == typeof(float))
                {
                    strValue = ((float)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (type == typeof(double))
                {
                    strValue = ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (type == typeof(decimal))
                {
                    strValue = ((decimal)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    strValue = value.ToString();
                }

                Value = strValue;
            }
        }
    }
}
