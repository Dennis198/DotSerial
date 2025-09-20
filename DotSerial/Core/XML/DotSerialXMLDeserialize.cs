using System.Collections;
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
                            if (Misc.TypeCheckMethods.IsPrimitive(prop.PropertyType))
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
                            else if (Misc.TypeCheckMethods.IsDictionary(prop.PropertyType))
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
                                object? tmpValue = Misc.ConverterMethods.ConvertDeserializedDictionary(tmpDic, prop.PropertyType);
                                prop.SetValue(classObj, tmpValue);
                            }
                            else if (Misc.TypeCheckMethods.IsList(prop.PropertyType) ||
                                     Misc.TypeCheckMethods.IsArray(prop.PropertyType))
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
                                tmpValue = Misc.ConverterMethods.ConvertDeserializedList(tmpList, prop.PropertyType);

                                prop.SetValue(classObj, tmpValue);
                            }
                            else if (Misc.TypeCheckMethods.IsHashSet(prop.PropertyType))
                            {
                                if (para.InnerText.Equals(Constants.NullString))
                                {
                                    prop.SetValue(classObj, null);
                                    break;
                                }

                                var tmpList = DeserializeHashSet(para.ChildNodes, prop.PropertyType);

                                if (null == tmpList)
                                {
                                    throw new NullReferenceException();
                                }

                                object? tmpValue;
                                // Convert deserialzed list.
                                tmpValue = Misc.ConverterMethods.ConvertDeserializeHashSet(tmpList, prop.PropertyType);

                                prop.SetValue(classObj, tmpValue);
                            }
                            else if (Misc.TypeCheckMethods.IsClass(prop.PropertyType) ||
                                     Misc.TypeCheckMethods.IsStruct(prop.PropertyType))
                            {
                                try
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
                                catch(MissingMethodException)
                                {
                                    // Hack, currently if a "record" without a parameterless constructor is
                                    // deserialized it will crash with "Activator.CreateInstance"
                                    throw new DSNoParameterlessConstructorDefinedException(prop.PropertyType.Name);
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

            Type itemType = Misc.GetTypeMethods.GetItemTypeOfIEnumerable(type);

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
                else if (Misc.TypeCheckMethods.IsDictionary(itemType))
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
                else if (Misc.TypeCheckMethods.IsList(itemType) ||
                         Misc.TypeCheckMethods.IsArray(itemType))
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
                else if (Misc.TypeCheckMethods.IsHashSet(itemType))
                {
                    // If xml text equals NullString
                    // => Object was null when it was serialzed.
                    if (node.InnerText.Equals(Constants.NullString))
                    {
                        result.Add(null);
                        continue;
                    }

                    var tmpList = DeserializeHashSet(node.ChildNodes, itemType);
                    result.Add(tmpList);
                }
                else
                {
                    object? tmp = Activator.CreateInstance(itemType);
                    if (null == tmp)
                    {
                        throw new NullReferenceException();
                    }

                    if (Misc.TypeCheckMethods.IsPrimitive(itemType))
                    {
                        DeserializePrimitive(ref tmp, itemType, node);
                    }
                    else if (Misc.TypeCheckMethods.IsClass(itemType) ||
                             Misc.TypeCheckMethods.IsStruct(itemType))
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
        /// Deserialize a HashSet
        /// </summary>
        /// <param name="xnodeList">XmlNodeList</param>
        /// <param name="type">Type</param>
        /// <returns>List of objects</returns>
        private static List<object?> DeserializeHashSet(XmlNodeList xnodeList, Type type)
        {
            ArgumentNullException.ThrowIfNull(xnodeList);
            ArgumentNullException.ThrowIfNull(type);

            Type itemType = Misc.GetTypeMethods.GetItemTypeOfIEnumerable(type);

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
                else
                {
                    object? tmp = Activator.CreateInstance(itemType);
                    if (null == tmp)
                    {
                        throw new NullReferenceException();
                    }

                    if (itemType.IsEnum)
                    {
                        throw new DSNotSupportedTypeException(type, itemType);
                    }

                    if (Misc.TypeCheckMethods.IsPrimitive(itemType))
                    {
                        DeserializePrimitive(ref tmp, itemType, node);
                    }
                    else
                    {
                        throw new DSNotSupportedTypeException(type, itemType);
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
            if (Misc.GetTypeMethods.GetKeyValueTypeOfDictionary(type, out Type keyType, out Type valueType))
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
                        else if (Misc.TypeCheckMethods.IsDictionary(keyType))
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
                        else if (Misc.TypeCheckMethods.IsList(keyType) ||
                                 Misc.TypeCheckMethods.IsArray(keyType))
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
                        else if (Misc.TypeCheckMethods.IsHashSet(keyType))
                        {
                            // If xml text equals NullString
                            // => Object was null when it was serialzed.
                            if (keyNodeInner.InnerText.Equals(Constants.NullString))
                            {
                                throw new NullReferenceException();
                            }
                            var tmpList = DeserializeHashSet(keyNodeInner.ChildNodes, keyType);

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

                            if (Misc.TypeCheckMethods.IsPrimitive(keyType))
                            {
                                DeserializePrimitive(ref tmp, keyType, keyNodeInner);
                            }
                            else if (Misc.TypeCheckMethods.IsClass(keyType) || Misc.TypeCheckMethods.IsStruct(keyType))
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
                        else if (Misc.TypeCheckMethods.IsDictionary(valueType))
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
                        else if (Misc.TypeCheckMethods.IsList(valueType) ||
                                 Misc.TypeCheckMethods.IsArray(valueType))
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
                        else if (Misc.TypeCheckMethods.IsHashSet(valueType))
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
                                var tmpList = DeserializeHashSet(valueNodeInner.ChildNodes, valueType);
                                value = tmpList;
                            }
#pragma warning restore CS8602
                        }
                        else
                        {
                            object? tmp = Activator.CreateInstance(valueType);
                            if (null == tmp)
                            {
                                throw new NullReferenceException();
                            }

                            if (Misc.TypeCheckMethods.IsPrimitive(valueType))
                            {
                                DeserializePrimitive(ref tmp, valueType, valueNodeInner);
                            }
                            else if (Misc.TypeCheckMethods.IsClass(valueType) || Misc.TypeCheckMethods.IsStruct(valueType))
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
            if (!Misc.TypeCheckMethods.IsPrimitive(typeObj))
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
                throw new DSNotSupportedTypeException(typeObj);
            }
        }
    }
}
