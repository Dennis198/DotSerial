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

using System.Reflection;
using System.Xml;

using DotSerial.Core.Exceptions;
using DotSerial.Core.Misc;

namespace DotSerial.Core.XML
{
    internal class DotSerialXMLDeserialize
    {
        /// <summary> 
        /// Deserialize Object
        /// </summary>
        /// <param name="classObj">Object</param>
        /// <param name="node">XmlNode</param>
        internal static void Deserialize(object? classObj, XmlNode node)
        {
            // XmlNode text equals NullString
            // => Object was null when it was serialzed.
            if (node.InnerText.Equals(Constants.NullString))
            {
                classObj = null;
                return;
            }

            ArgumentNullException.ThrowIfNull(classObj);

            // Get type
            Type typeObj = classObj.GetType();

            // Get all Properties and iterate threw
            PropertyInfo[] props = typeObj.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                // Get ID attribute
                int id = Attributes.HelperMethods.GetPropertyID(prop);

                if (Constants.NoAttributeID != id)
                {
                    // Check if type is supported
                    if (false == DotSerialXML.IsTypeSupported(prop.PropertyType))
                    {
                        throw new DSNotSupportedTypeException(prop.PropertyType);
                    }

                    foreach (XmlNode para in node.ChildNodes)
                    {
                        if (null == para || null == para.Attributes)
                        {
                            throw new NullReferenceException();
                        }

                        // Read AttribteID from xmlnode and cast it to int.
                        string? t = para.Attributes[Constants.XmlAttributeID]?.Value;
                        if (false == int.TryParse(t, out int idXML))
                        {
                            throw new InvalidCastException("ID could not be deserialized.");
                        }

                        // If AttributeID and ID from xml node match => deserialze.
                        if (id == idXML)
                        {
                            if (TypeCheckMethods.IsPrimitive(prop.PropertyType))
                            {
                                object? tmp = null;
                                DeserializePrimitive(ref tmp, prop.PropertyType, para);
                                prop.SetValue(classObj, tmp);
                            }
                            else if (TypeCheckMethods.IsSpecialParsableObject(prop.PropertyType))
                            {
                                object? tmp = null;
                                DeserializeSpecialParsableObject(ref tmp, prop.PropertyType, para);
                                prop.SetValue(classObj, tmp);
                            }
                            else if (TypeCheckMethods.IsDictionary(prop.PropertyType))
                            {
                                if (para.InnerText.Equals(Constants.NullString))
                                {
                                    prop.SetValue(classObj, null);
                                    break;
                                }

                                var tmpDic = DeserializeDictionary(para.ChildNodes, prop.PropertyType);

                                if (null == tmpDic)
                                {
                                    throw new NullReferenceException();
                                }

                                // Convert deserialzed dictionary.
                                object? tmpValue = ConverterMethods.ConvertDeserializedDictionary(tmpDic, prop.PropertyType);
                                prop.SetValue(classObj, tmpValue);
                            }
                            else if (TypeCheckMethods.IsList(prop.PropertyType) ||
                                     TypeCheckMethods.IsArray(prop.PropertyType))
                            {
                                if (para.InnerText.Equals(Constants.NullString))
                                {
                                    prop.SetValue(classObj, null);
                                    break;
                                }

                                var tmpList = DeserializeList(para.ChildNodes, prop.PropertyType);

                                if (null == tmpList)
                                {
                                    throw new NullReferenceException();
                                }

                                object? tmpValue;
                                // Convert deserialzed list.
                                tmpValue = ConverterMethods.ConvertDeserializedList(tmpList, prop.PropertyType);

                                prop.SetValue(classObj, tmpValue);
                            }
                            else if (TypeCheckMethods.IsClass(prop.PropertyType) ||
                                     TypeCheckMethods.IsStruct(prop.PropertyType))
                            {
                                object tmp = CreateInstanceMethods.CreateInstanceGeneric(prop.PropertyType);

                                // If xmlnode text equals NullString
                                // => Object was null when it was serialzed.
                                if (para.InnerText.Equals(Constants.NullString))
                                {
                                    prop.SetValue(classObj, null);
                                }
                                else
                                {
                                    Deserialize(tmp, para);
                                    prop.SetValue(classObj, tmp);
                                }
                            }
                            else
                            {
                                throw new DSNotSupportedTypeException(prop.PropertyType);
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary> 
        /// Deserialize a list
        /// </summary>
        /// <param name="xnodeList">XmlNodeList</param>
        /// <param name="type">Type</param>
        /// <returns>List of objects</returns>
        private static List<object?> DeserializeList(XmlNodeList xnodeList, Type type)
        {
            ArgumentNullException.ThrowIfNull(xnodeList);
            ArgumentNullException.ThrowIfNull(type);

            Type itemType = GetTypeMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            if (false == DotSerialXML.IsTypeSupported(itemType))
            {
                throw new DSNotSupportedTypeException(itemType);
            }

            List<object?> result = [];

            foreach (XmlNode node in xnodeList)
            {
                if (itemType == typeof(string))
                {
                    DeserializeString(out string? tmpString, node);
                    result.Add(tmpString);
                }
                else if (TypeCheckMethods.IsDictionary(itemType))
                {
                    // If xml text equals NullString
                    // => Object was null when it was serialzed.
                    if (node.InnerText.Equals(Constants.NullString))
                    {
                        result.Add(null);
                        continue;
                    }

                    var tmpDic = DeserializeDictionary(node.ChildNodes, itemType);
                    result.Add(tmpDic);
                }
                else if (TypeCheckMethods.IsList(itemType) ||
                         TypeCheckMethods.IsArray(itemType))
                {
                    // If xml text equals NullString
                    // => Object was null when it was serialzed.
                    if (node.InnerText.Equals(Constants.NullString))
                    {
                        result.Add(null);
                        continue;
                    }

                    var tmpList = DeserializeList(node.ChildNodes, itemType);
                    result.Add(tmpList);
                }
                else
                {
                    object? tmp = CreateInstanceMethods.CreateInstanceGeneric(itemType);

                    if (TypeCheckMethods.IsPrimitive(itemType))
                    {
                        DeserializePrimitive(ref tmp, itemType, node);
                    }
                    else if (TypeCheckMethods.IsSpecialParsableObject(itemType))
                    {
                        DeserializeSpecialParsableObject(ref tmp, itemType, node);
                    }
                    else if (TypeCheckMethods.IsClass(itemType) ||
                             TypeCheckMethods.IsStruct(itemType))
                    {
                        if (false == string.IsNullOrWhiteSpace(node.InnerText))
                        {
                            Deserialize(tmp, node);
                        }
                        else
                        {
                            tmp = null;
                        }

                    }
                    else
                    {
                        throw new DSNotSupportedTypeException(itemType);
                    }

                    result.Add(tmp);
                }
            }

            return result;
        }

        /// <summary>
        /// Deserialze Dictionary
        /// </summary>
        /// <param name="xnodeList">XmlNodeList</param>
        /// <param name="type">Type</param>
        /// <returns>Dictionary of objects</returns>
        private static Dictionary<object, object?> DeserializeDictionary(XmlNodeList xnodeList, Type type)
        {
            if (GetTypeMethods.GetKeyValueTypeOfDictionary(type, out Type keyType, out Type valueType))
            {
                // Check if type is supported
                if (false == DotSerialXML.IsTypeSupported(keyType))
                {
                    throw new DSNotSupportedTypeException(keyType);
                }
                // Check if type is supported
                if (false == DotSerialXML.IsTypeSupported(valueType))
                {
                    throw new DSNotSupportedTypeException(valueType);
                }

                Dictionary< object, object ?> result = [];

                foreach (XmlNode node in xnodeList)
                {
                    if (null != node && null != node?.ChildNodes && node.ChildNodes.Count == 2)
                    {

                        #region Key

#pragma warning disable CS8600, CS8602
                        XmlNode keyNode = node.ChildNodes[0];
                        XmlNode keyNodeInner = keyNode.ChildNodes[0];
#pragma warning restore CS8600, CS8602


                        object key;

                        if (null == keyNode || null == keyNodeInner)
                        {
                            throw new NullReferenceException();
                        }

                        if (keyType == typeof(string))
                        {
                            // Note: String case MUST become before IEnumerable
                            // otherwise it will not be serialized correct.

                            DeserializeString(out string? tmpString, keyNodeInner);

                            if (null == tmpString)
                            {
                                throw new NullReferenceException();
                            }

                            key = tmpString;
                        }
                        else
                        {
                            object? tmp = CreateInstanceMethods.CreateInstanceGeneric(keyType);

                            if (TypeCheckMethods.IsPrimitive(keyType))
                            {
                                DeserializePrimitive(ref tmp, keyType, keyNodeInner);
                            }
                            else if (TypeCheckMethods.IsSpecialParsableObject(keyType))
                            {
                                DeserializeSpecialParsableObject(ref tmp, keyType, keyNodeInner);
                            }
                            else
                            {
                                throw new DSNotSupportedTypeException(keyType);
                            }

                            if (null == tmp)
                            {
                                throw new NullReferenceException();
                            }

                            key = tmp;
                            
                        }

                        #endregion

                        #region Value

#pragma warning disable CS8600, CS8602
                        XmlNode valueNode = node.ChildNodes[1];
                        XmlNode valueNodeInner = valueNode.ChildNodes[0];
#pragma warning restore CS8600, CS8602

                        object? value;

                        if (valueType == typeof(string))
                        {
                            // Note: String case MUST become before IEnumerable
                            // otherwise it will not be serialized correct.

#pragma warning disable CS8604
                            DeserializeString(out string? tmpString, valueNodeInner);
#pragma warning restore CS8604
                            value = tmpString;
                        }
                        else if (TypeCheckMethods.IsDictionary(valueType))
                        {
#pragma warning disable CS8602
                            // If xml text equals NullString
                            // => Object was null when it was serialzed.
                            if (valueNodeInner.InnerText.Equals(Constants.NullString))
                            {
                                value = null;
                            }
                            else
                            {
                                var tmpDic = DeserializeDictionary(valueNodeInner.ChildNodes, valueType);
                                value = tmpDic;
                            }
#pragma warning restore CS8602
                        }
                        else if (TypeCheckMethods.IsList(valueType) ||
                                 TypeCheckMethods.IsArray(valueType))
                        {
#pragma warning disable CS8602
                            // If xml text equals NullString
                            // => Object was null when it was serialzed.
                            if (valueNodeInner.InnerText.Equals(Constants.NullString))
                            {
                                value = null;
                            }
                            else
                            {
                                var tmpList = DeserializeList(valueNodeInner.ChildNodes, valueType);
                                value = tmpList;
                            }
#pragma warning restore CS8602
                        }
                        else
                        {
                            object? tmp = CreateInstanceMethods.CreateInstanceGeneric(valueType);

                            if (TypeCheckMethods.IsPrimitive(valueType))
                            {
                                DeserializePrimitive(ref tmp, valueType, valueNodeInner);
                            }
                            else if (TypeCheckMethods.IsSpecialParsableObject(valueType))
                            {
                                DeserializeSpecialParsableObject(ref tmp, valueType, valueNodeInner);
                            }
                            else if (TypeCheckMethods.IsClass(valueType) || TypeCheckMethods.IsStruct(valueType))
                            {
                                if (false == string.IsNullOrWhiteSpace(valueNodeInner?.InnerText))
                                {
                                    Deserialize(tmp, valueNodeInner);
                                }
                                else
                                {
                                    tmp = null;
                                }

                            }
                            else
                            {
                                throw new DSNotSupportedTypeException(valueType);
                            }

                            value = tmp;

                        }

                        #endregion

                        result.Add(key, value);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }

                return result;
            }
            else
            {
                throw new TypeAccessException();
            }
                
        }

        /// <summary> 
        /// Derserialize an string
        /// </summary>
        /// <param name="strObj">String</param>
        /// <param name="node">XmlNode</param>
        private static void DeserializeString(out string? strObj, XmlNode node)
        {
            if (node.ChildNodes == null || node.ChildNodes.Count != 1)
            {
                throw new NotSupportedException();
            }

            // Get text of xmlnode
            string? innerText = node.ChildNodes[0]?.InnerText;

            if (null == innerText)
            {
                throw new NullReferenceException();
            }

            // If string equals NullString
            // => Object was null when it was serialzed.
            if (innerText.Equals(Constants.NullString))
            {
                innerText = null;
            }

            strObj = innerText;
        }

        /// <summary> 
        /// Deserialze an primitive type
        /// </summary>
        /// <param name="primObj">Primitive Object</param>
        /// <param name="node">XMLNode</param>
        private static void DeserializePrimitive(ref object? primObj, Type primType, XmlNode? node)
        {
            ArgumentNullException.ThrowIfNull(primType);
            ArgumentNullException.ThrowIfNull(node);

            // Get type
            Type typeObj = primType;

            // Check if primitive type
            if (!TypeCheckMethods.IsPrimitive(typeObj))
            {
                throw new DSNotSupportedTypeException(typeObj);
            }

            if (node.ChildNodes == null || node.ChildNodes.Count != 1)
            {
                throw new NotSupportedException();
            }

            // Get text of xmlnode
            string? innerText = node.ChildNodes[0]?.InnerText;

            if (null == innerText)
            {
                throw new NullReferenceException();
            }

            object? tmp = ConverterMethods.ConvertStringToPrimitive(innerText, typeObj);
            primObj = tmp;
        }

        /// <summary> 
        /// Deserialze an speical parsable type
        /// </summary>
        /// <param name="primObj">Primitive Object</param>
        /// <param name="node">XMLNode</param>
        private static void DeserializeSpecialParsableObject(ref object? primObj, Type primType, XmlNode? node)
        {
            ArgumentNullException.ThrowIfNull(primType);
            ArgumentNullException.ThrowIfNull(node);

            // Get type
            Type typeObj = primType;

            // Check if primitive type
            if (!TypeCheckMethods.IsSpecialParsableObject(typeObj))
            {
                throw new DSNotSupportedTypeException(typeObj);
            }

            if (node.ChildNodes == null || node.ChildNodes.Count != 1)
            {
                throw new NotSupportedException();
            }

            // Get text of xmlnode
            string? innerText = node.ChildNodes[0]?.InnerText;

            if (null == innerText)
            {
                throw new NullReferenceException();
            }

            object? tmp = ConverterMethods.ConvertStringToSpecialParsableObject(innerText, typeObj);
            primObj = tmp;
        }
    }
}
