#region License
//Copyright (c) 2026 Dennis Sölch

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

using System.Reflection;

using DotSerial.Utilities;
using DotSerial.Tree.Nodes;
using DotSerial.XML;
using DotSerial.Common;

namespace DotSerial.Tree.Deserialize
{
    /// <summary>
    /// Class for deserialiation of object
    /// </summary>
    public class DeserializeObject : INodeDeserializeVisitor
    {
        /// <inheritdoc/>
        public object? VisitLeafNode(LeafNode node, Type? type)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(type);

            string? strValue = node.GetValue();

            if (null == strValue)
            {
                return null;
            }

            object? result;

            if (TypeCheckMethods.IsPrimitive(type))
            {
                result = ConverterMethods.ConvertStringToPrimitive(strValue, type);
            }
            else if (TypeCheckMethods.IsSpecialParsableObject(type))
            {
                result = ConverterMethods.ConvertStringToSpecialParsableObject(strValue, type);
            }
            else
            {
                throw new DotSerialException($"Deserialize: Type {type} is not a leaf");
            }

            return result;

        }

        /// <inheritdoc/>
        public object? VisitInnerNode(InnerNode node, Type? type)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(type);

            // Get type
            // Type typeObj = obj.GetType();

            var result = CreateInstanceMethods.CreateInstanceGeneric(type);

            // Get all Properties and iterate threw
            PropertyInfo[] props = type.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                // Get ID attribute
                string? propDSName = Attributes.AttributesMethods.GetCustomPropertyName(prop);

                if (null != propDSName)
                {
                    // Check if type is supported
                    if (false == DotSerialXML.IsTypeSupported(prop.PropertyType))
                    {
                        throw new NotImplementedException();
                    }

                    var child = node.GetChild(propDSName);

                    // Special case: In must formats (json, yaml, ..) there is no
                    // difference in classes or dictionarys when parsing. So
                    // the dictionary case must also be handles here.
                    if (TypeCheckMethods.IsDictionary(prop.PropertyType))
                    {
                        var tmpList = DeserializeDictionary(child, prop.PropertyType);

                        // Convert deserialzed dictionary.
                        object? tmpValue = ConverterMethods.ConvertDeserializedDictionary(tmpList, prop.PropertyType);

                        prop.SetValue(result, tmpValue);
                        continue;
                    }

                    if (null == child)
                    {
                        throw new DotSerialException($"Deserialize: Child with ID {propDSName} not found in node {node.Key}");
                    }                    

                    var tmp = child.DeserializeAccept(this, prop.PropertyType);
                    prop.SetValue(result, tmp);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public object? VisitListNode(ListNode node, Type? type)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(type);

            Type itemType = GetTypeMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            if (false == DotSerialXML.IsTypeSupported(itemType))
            {
                throw new DotSerialException($"Deserialize: Type {itemType} is not supported.");
            }

            List<object?> tmpList = [];

            var children = node.GetChildren();

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];

                var tmp = child.DeserializeAccept(this, itemType);
                tmpList.Add(tmp);
            }

            var result = ConverterMethods.ConvertDeserializedList(tmpList, type);

            return result;
        }

        /// <inheritdoc/>
        private Dictionary<object, object?>? DeserializeDictionary(IDSNode node, Type type)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(type);

            if (GetTypeMethods.GetKeyValueTypeOfDictionary(type, out Type keyType, out Type valueType))
            {
                 // Check if type is supported
                if (false == DotSerialXML.IsTypeSupported(keyType))
                {
                    throw new DotSerialException($"Deserialize: Type {keyType} is not supported.");
                }
                // Check if type is supported
                if (false == DotSerialXML.IsTypeSupported(valueType))
                {
                    throw new DotSerialException($"Deserialize: Type {valueType} is not supported.");
                }

                if (node is LeafNode leaf)
                {
                    if (leaf.GetValue() == null)
                    {
                        return null;
                    }
                    else
                    {
                        throw new DotSerialException($"Deserialize: Node is a leaf but dictionary expected.");
                    }
                }

                Dictionary<object, object?> result = [];
                var children = node.GetChildren();

                foreach(var child in children)
                {
                    string key = child.Key;
                    object? value;
                    if (child is LeafNode)
                    {
                        value = child.DeserializeAccept(this, valueType);
                    }
                    else
                    {
                        value = child.DeserializeAccept(this, valueType);
                    }

                    result.Add(key, value);
                }

                return result;
            }
            else
            {
                throw new DotSerialException("Type is not a Dictionary.");
            }

        }

        /// <inheritdoc/>
        public object? VisitDictionaryNode(DictionaryNode node, Type? type)
        {
            // Currenlty not needed
            // Will be procced in the VisitInnerNode
            throw new NotImplementedException();
        }
    }
}