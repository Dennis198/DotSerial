using System;
using System.Collections;
using System.Reflection;
using System.Xml;

using DotSerial.Core.Exceptions;

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
                        throw new NotSupportedTypeException(prop.PropertyType);
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
                            if (Misc.HelperMethods.IsPrimitive(prop.PropertyType))
                            {
                                object? tmp = null;
                                DeserializePrimitive(ref tmp, prop.PropertyType, para);
                                prop.SetValue(classObj, tmp);
                            }
                            // DateTime
                            else if (prop.PropertyType == typeof(DateTime))
                            {
                                DateTime tmp = DateTime.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            else if (Misc.HelperMethods.IsDictionary(prop.PropertyType))
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
                                object? tmpValue = ConvertDeserializedDictionary(tmpDic, prop.PropertyType);
                                prop.SetValue(classObj, tmpValue);
                            }
                            else if (Misc.HelperMethods.IsList(prop.PropertyType) || Misc.HelperMethods.IsArray(prop.PropertyType))
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

                                // Convert deserialzed list.
                                object? tmpValue = ConvertDeserializedList(tmpList, prop.PropertyType);
                                prop.SetValue(classObj, tmpValue);
                            }
                            else if (Misc.HelperMethods.IsClass(prop.PropertyType) || Misc.HelperMethods.IsStruct(prop.PropertyType))
                            {
                                object? tmp = Activator.CreateInstance(prop.PropertyType);
                                if (null != tmp)
                                {
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
                            }
                            else
                            {
                                throw new NotSupportedTypeException(prop.PropertyType);
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary> 
        /// Converts the serialzed list to object so "PropertyInfo.SetValue" can
        /// set the value properly
        /// </summary>
        /// <param name="list">Deserialzed List</param>
        /// <param name="type">Type</param>
        /// <returns>Converted list</returns>
        private static object? ConvertDeserializedList(List<object?> list, Type type)
        {
            if (null == list)
            {
                return null;
            }

            // Get Item type of list
            Type itemType = Misc.HelperMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            if (false == DotSerialXML.IsTypeSupported(itemType))
            {
                throw new NotSupportedTypeException(itemType);
            }

            // Check if type is array
            bool isArray = type.IsArray;

            // result object
            object? result;

            // Create initial object to fill.
            if (isArray)
            {
                result = Array.CreateInstanceFromArrayType(type, list.Count);
            }
            else
            {
                result = Activator.CreateInstance(type);
            }

            if (list is IList castedList && result is IList castedListResult)
            {
                for (int i = 0; i < castedList.Count; i++)
                {
                    if (Misc.HelperMethods.IsDictionary(itemType))
                    {
                        object? itemResult = null;
                        if (castedList[i] is not Dictionary<object, object?> castedDictionaryItemObj)
                        {
                            throw new InvalidCastException();
                        }
                        itemResult = ConvertDeserializedDictionary(castedDictionaryItemObj, itemType);

                        if (itemResult != null)
                        {
                            if (isArray)
                                castedListResult[i] = itemResult;
                            else
                                castedListResult.Add(itemResult);
                        }
                    }
                    else if (Misc.HelperMethods.IsList(itemType) || Misc.HelperMethods.IsArray(itemType))
                    {
                        object? itemResult = null;
                        if (castedList[i] is not List<object?> castedListItemObj)
                        {
                            throw new InvalidCastException();
                        }

                        itemResult = ConvertDeserializedList(castedListItemObj, itemType);

                        if (itemResult != null)
                        {
                            if (isArray)
                                castedListResult[i] = itemResult;
                            else
                                castedListResult.Add(itemResult);
                        }
                    }
                    else if (itemType.IsEnum)
                    {
                        if (null == castedList[i])
                        {
                            throw new NullReferenceException();
                        }

                        if (isArray)
                            castedListResult[i] = Enum.ToObject(itemType, castedList[i]);
                        else
                            castedListResult.Add(Enum.ToObject(itemType, castedList[i]));
                    }
                    else
                    {
                        if (isArray)
                            castedListResult[i] = castedList[i];
                        else
                            castedListResult.Add(castedList[i]);
                    }
                }

                return castedListResult;
            }
            else
            {
                throw new InvalidCastException();
            }

        }

        /// <summary> 
        /// Converts the serialzed dictionary to object so "PropertyInfo.SetValue" can
        /// set the value properly
        /// </summary>
        /// <param name="dic">Deserialzed Dictionary</param>
        /// <param name="type">Type</param>
        /// <returns>Converted dictionary</returns>
        private static object? ConvertDeserializedDictionary(Dictionary<object, object?> dic, Type type)
        {
            if (null == dic)
            {
                return null;
            }

            // Get Item type of dictionary            
            if (Misc.HelperMethods.GetKeyValueTypeOfDictionary(type, out Type keyType, out Type valueType))
            {
                // Check if type is supported
                if (false == DotSerialXML.IsTypeSupported(keyType))
                {
                    throw new NotSupportedTypeException(keyType);
                }
                // Check if type is supported
                if (false == DotSerialXML.IsTypeSupported(valueType))
                {
                    throw new NotSupportedTypeException(valueType);
                }

                // result object
                Type resultType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
                object? result = Activator.CreateInstance(resultType);

                if (null == result)
                {
                    throw new NullReferenceException();
                }

                if (dic is IDictionary castedDic && result is IDictionary castedDicResult)
                {
                    foreach (DictionaryEntry keyValuePair in castedDic)
                    {
                        if (Misc.HelperMethods.IsDictionary(valueType))
                        {
                            object? itemResult = null;
                            if (castedDic[keyValuePair.Key] is not Dictionary<object, object?> castedDictionaryItemObj)
                            {
                                throw new InvalidCastException();
                            }
                            itemResult = ConvertDeserializedDictionary(castedDictionaryItemObj, valueType);

                            castedDicResult.Add(keyValuePair.Key, itemResult);
                        }
                        else if (Misc.HelperMethods.IsList(valueType) || Misc.HelperMethods.IsArray(valueType))
                        {
                            object? itemResult = null;
                            if (castedDic[keyValuePair.Key] is not List<object?> castedListItemObj)
                            {
                                throw new InvalidCastException();
                            }

                            itemResult = ConvertDeserializedList(castedListItemObj, valueType);

                            castedDicResult.Add(keyValuePair.Key, itemResult);
                        }
                        else if (valueType.IsEnum)
                        {
                            if (null == castedDic[keyValuePair.Key])
                            {
                                throw new NullReferenceException();
                            }
                            
                            castedDicResult.Add(keyValuePair.Key, Enum.ToObject(valueType, castedDic[keyValuePair.Key]));
                        }
                        else
                        {
                            castedDicResult.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                    }

                    return castedDicResult;
                }
                else
                {
                    throw new InvalidCastException();
                }
            }
            else
            {
                throw new TypeAccessException();
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

            Type itemType = Misc.HelperMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            if (false == DotSerialXML.IsTypeSupported(itemType))
            {
                throw new NotSupportedTypeException(itemType);
            }

            List<object?> result = [];

            foreach (XmlNode node in xnodeList)
            {
                if (itemType == typeof(string))
                {
                    DeserializeString(out string? tmpString, node);
                    result.Add(tmpString);
                }
                else if (Misc.HelperMethods.IsDictionary(itemType))
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
                else if (Misc.HelperMethods.IsList(itemType) || Misc.HelperMethods.IsArray(itemType))
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
                    object? tmp = Activator.CreateInstance(itemType);
                    if (null == tmp)
                    {
                        throw new NullReferenceException();
                    }

                    if (Misc.HelperMethods.IsPrimitive(itemType))
                    {
                        DeserializePrimitive(ref tmp, itemType, node);
                    }
                    else if (Misc.HelperMethods.IsClass(itemType) || Misc.HelperMethods.IsStruct(itemType))
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
                        throw new NotSupportedTypeException(itemType);
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
            if (Misc.HelperMethods.GetKeyValueTypeOfDictionary(type, out Type keyType, out Type valueType))
            {
                // Check if type is supported
                if (false == DotSerialXML.IsTypeSupported(keyType))
                {
                    throw new NotSupportedTypeException(keyType);
                }
                // Check if type is supported
                if (false == DotSerialXML.IsTypeSupported(valueType))
                {
                    throw new NotSupportedTypeException(valueType);
                }
                Dictionary< object, object ?> result = [];

                foreach (XmlNode node in xnodeList)
                {
                    if (null != node && null != node?.ChildNodes && node.ChildNodes.Count == 2)
                    {
                        #region Key
                        XmlNode keyNode = node.ChildNodes[0];
                        XmlNode keyNodeInner = keyNode.ChildNodes[0];
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
                        else if (Misc.HelperMethods.IsDictionary(keyType))
                        {
                            // If xml text equals NullString
                            // => Object was null when it was serialzed.
                            if (keyNodeInner.InnerText.Equals(Constants.NullString))
                            {
                                throw new NullReferenceException();
                            }
                            var tmpDic = DeserializeDictionary(keyNodeInner.ChildNodes, keyType);

                            if (null == tmpDic)
                            {
                                throw new NullReferenceException();
                            }

                            key = tmpDic;
                        }
                        else if (Misc.HelperMethods.IsList(keyType) || Misc.HelperMethods.IsArray(keyType))
                        {
                            // If xml text equals NullString
                            // => Object was null when it was serialzed.
                            if (keyNodeInner.InnerText.Equals(Constants.NullString))
                            {
                                throw new NullReferenceException();
                            }
                            var tmpList = DeserializeList(keyNodeInner.ChildNodes, keyType);

                            if (null == tmpList)
                            {
                                throw new NullReferenceException();
                            }

                            key = tmpList;
                        }
                        else
                        {
                            object? tmp = Activator.CreateInstance(keyType);
                            if (null == tmp)
                            {
                                throw new NullReferenceException();
                            }

                            if (Misc.HelperMethods.IsPrimitive(keyType))
                            {
                                DeserializePrimitive(ref tmp, keyType, keyNodeInner);
                            }
                            else if (Misc.HelperMethods.IsClass(keyType) || Misc.HelperMethods.IsStruct(keyType))
                            {
                                if (false == string.IsNullOrWhiteSpace(keyNodeInner.InnerText))
                                {
                                    Deserialize(tmp, keyNodeInner);
                                }
                                else
                                {
                                    tmp = null;
                                }

                            }
                            else
                            {
                                throw new NotSupportedTypeException(keyType);
                            }

                            if (null == tmp)
                            {
                                throw new NullReferenceException();
                            }

                            key = tmp;
                            
                        }

                        #endregion

                        #region Value

                        XmlNode valueNode = node.ChildNodes[1];
                        XmlNode valueNodeInner = valueNode.ChildNodes[0];
                        object? value;

                        if (valueType == typeof(string))
                        {
                            // Note: String case MUST become before IEnumerable
                            // otherwise it will not be serialized correct.

                            DeserializeString(out string? tmpString, valueNodeInner);
                            value = tmpString;
                        }
                        else if (Misc.HelperMethods.IsDictionary(valueType))
                        {
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
                        }
                        else if (Misc.HelperMethods.IsList(valueType) || Misc.HelperMethods.IsArray(valueType))
                        {
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

                        }
                        else
                        {
                            object? tmp = Activator.CreateInstance(valueType);
                            if (null == tmp)
                            {
                                throw new NullReferenceException();
                            }

                            if (Misc.HelperMethods.IsPrimitive(valueType))
                            {
                                DeserializePrimitive(ref tmp, valueType, valueNodeInner);
                            }
                            else if (Misc.HelperMethods.IsClass(valueType) || Misc.HelperMethods.IsStruct(valueType))
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
                                throw new NotSupportedTypeException(valueType);
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
            if (!Misc.HelperMethods.IsPrimitive(typeObj))
            {
                throw new NotSupportedTypeException(typeObj);
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

            // Char
            if (typeObj == typeof(char))
            {
                char tmp = char.Parse(innerText);
                primObj = tmp;
            }
            // Byte
            else if (typeObj == typeof(byte))
            {
                byte tmp = byte.Parse(innerText);
                primObj = tmp;
            }
            // SByte
            else if (typeObj == typeof(sbyte))
            {
                sbyte tmp = sbyte.Parse(innerText);
                primObj = tmp;
            }
            // Decimal
            else if (typeObj == typeof(double))
            {
                double tmp = double.Parse(innerText);
                primObj = tmp;
            }
            // Float
            else if (typeObj == typeof(float))
            {
                float tmp = float.Parse(innerText);
                primObj = tmp;
            }
            // Double
            else if (typeObj == typeof(decimal))
            {
                decimal tmp = decimal.Parse(innerText);
                primObj = tmp;
            }
            // Int
            else if (typeObj == typeof(int))
            {
                int tmp = int.Parse(innerText);
                primObj = tmp;
            }
            // UInt
            else if (typeObj == typeof(uint))
            {
                uint tmp = uint.Parse(innerText);
                primObj = tmp;
            }
            // NInt
            else if (typeObj == typeof(nint))
            {
                nint tmp = nint.Parse(innerText);
                primObj = tmp;
            }
            // NUInt
            else if (typeObj == typeof(nuint))
            {
                nuint tmp = nuint.Parse(innerText);
                primObj = tmp;
            }
            // Long
            else if (typeObj == typeof(long))
            {
                long tmp = long.Parse(innerText);
                primObj = tmp;
            }
            // ULong
            else if (typeObj == typeof(ulong))
            {
                ulong tmp = ulong.Parse(innerText);
                primObj = tmp;
            }
            // Short
            else if (typeObj == typeof(short))
            {
                short tmp = short.Parse(innerText);
                primObj = tmp;
            }
            // UShort
            else if (typeObj == typeof(ushort))
            {
                ushort tmp = ushort.Parse(innerText);
                primObj = tmp;
            }
            // Boolean
            else if (typeObj == typeof(bool))
            {
                int tmp = int.Parse(innerText);

                // Special case bool
                // Was casted to int in serialze.
                bool tmpBool = Misc.HelperMethods.IntToBool(tmp);
                primObj = tmpBool;
            }
            // DateTime
            else if (typeObj == typeof(DateTime))
            {
                DateTime tmp = DateTime.Parse(innerText);
                primObj = tmp;
            }
            // Enum
            else if (true == typeObj.IsEnum)
            {
                int tmp = int.Parse(innerText);
                primObj = tmp;
            }
            // String
            else if (typeObj == typeof(string))
            {
                if (innerText.Equals(Constants.NullString))
                {
                    primObj = null;
                }
                else
                {
                    primObj = innerText;
                }
            }
            else
            {
                throw new NotSupportedTypeException(typeObj);
            }
        }
    }
}
