using DotSerial.Core.Exceptions;
using DotSerial.Core.Misc;
using DotSerial.Core.XML;
using System.Reflection;

namespace DotSerial.Core.General
{
    internal class DSDeserialize
    {

        /// <summary>
        /// Deserialize Object
        /// </summary>
        /// <param name="classObj">Object</param>
        /// <param name="node">DSNode</param>
        internal static void Deserialize(object? classObj, DSNode? node)
        {
            // XmlNode text equals NullString
            // => Object was null when it was serialzed.
            if (node == null || node.IsNull || node.IsEmpty) // isNull weg amchen?
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

                if (-1 != id)
                {
                    // Check if type is supported
                    if (false == DotSerialXML.IsTypeSupported(prop.PropertyType))
                    {
                        throw new DSNotSupportedTypeException(prop.PropertyType);
                    }

                    DSNode value = node.GetChild(id);

                    if (TypeCheckMethods.IsPrimitive(prop.PropertyType))
                    {
                        object? tmp = value.ConvertValue(prop.PropertyType);
                        prop.SetValue(classObj, tmp);
                    }
                    else if (TypeCheckMethods.IsDictionary(prop.PropertyType))
                    {
                        var tmpList = DeserializeDictionary(value, prop.PropertyType);

                        object? tmpValue;
                        // Convert deserialzed dictionary.
                        tmpValue = ConverterMethods.ConvertDeserializedDictionary(tmpList, prop.PropertyType);

                        prop.SetValue(classObj, tmpValue);
                    }
                    else if (TypeCheckMethods.IsList(prop.PropertyType) ||
                             TypeCheckMethods.IsArray(prop.PropertyType))
                    {

                        var tmpList = DeserializeList(value, prop.PropertyType);

                        object? tmpValue;
                        // Convert deserialzed list.
                        tmpValue = ConverterMethods.ConvertDeserializedList(tmpList, prop.PropertyType);

                        prop.SetValue(classObj, tmpValue);
                    }
                    else if (TypeCheckMethods.IsClass(prop.PropertyType) ||
                             TypeCheckMethods.IsStruct(prop.PropertyType))
                    {
                        if (value.IsNull || value.IsEmpty)
                        {
                            prop.SetValue(classObj, null);
                        }
                        else
                        {
                            object tmp = CreateInstanceMethods.CreateInstanceGeneric(prop.PropertyType);
                            Deserialize(tmp, value);
                            prop.SetValue(classObj, tmp);
                        }
                    }
                    else if (value.IsNull)
                    {
                        prop.SetValue(classObj, null);
                        continue;
                    }
                    else
                    {
                        throw new DSNotSupportedTypeException(prop.PropertyType);
                    }
                }
            }
        }

        /// <summary>
        /// Deserialze Dictionary
        /// </summary>
        /// <param name="node">DSNode</param>
        /// <param name="type">Type</param>
        /// <returns>Dictionary of objects</returns>
        private static Dictionary<object, object?>? DeserializeDictionary(DSNode node, Type type)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(type);

            if (node.IsNull)
            {
                return null;
            }

            if (node.IsEmpty)
            {
                return [];
            }

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

                Dictionary<object, object?> result = [];

                for (int i = 0; i < node.Count; i++)
                {
                    DSNode tmp = node.GetChild(i);
                    DSNode keyValuePairNode = tmp;
                    DSNode keyNode = keyValuePairNode.GetChild(0);
                    DSNode valueNode = keyValuePairNode.GetChild(1);
                    object key;
                    object? value;

                    #region Key

                    if (TypeCheckMethods.IsPrimitive(keyType))
                    {
                        key = keyNode.ConvertValue(keyType) ?? throw new NullReferenceException();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    #endregion

                    #region Value

                    if (TypeCheckMethods.IsPrimitive(valueType))
                    {
                        value = valueNode.ConvertValue(valueType);
                    }
                    else if (TypeCheckMethods.IsList(valueType) ||
                             TypeCheckMethods.IsArray(valueType))
                    {
                        var tmpList = DeserializeList(valueNode, valueType);
                        value = tmpList;
                    }
                    else if (TypeCheckMethods.IsDictionary(valueType))
                    {
                        var tmpDic = DeserializeDictionary(valueNode, valueType);
                        value = tmpDic;
                    }
                    else if (TypeCheckMethods.IsClass(valueType) || TypeCheckMethods.IsStruct(valueType))
                    {
                        object? tmpInstance = CreateInstanceMethods.CreateInstanceGeneric(valueType);
                        DSNode ttt = valueNode.GetChild(0);
                        if (ttt.IsNull || ttt.IsEmpty)
                        {
                            tmpInstance = null;
                        }
                        else
                        {
                            Deserialize(tmpInstance, ttt);
                        }

                        value = tmpInstance;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    #endregion

                    result.Add(key, value);
                }

                return result;
            }
            else
            {
                throw new TypeAccessException();
            }
        }

        /// <summary> 
        /// Deserialize a list
        /// </summary>
        /// <param name="node">DSNode</param>
        /// <param name="type">Type</param>
        /// <returns>List of objects</returns>
        private static List<object?>? DeserializeList(DSNode node, Type type)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(type);

            if (node.IsNull)
            {
                return null;
            }

            if (node.IsEmpty)
            {
                return [];
            }

            Type itemType = GetTypeMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            if (false == DotSerialXML.IsTypeSupported(itemType))
            {
                throw new DSNotSupportedTypeException(itemType);
            }

            List<object?> result = [];

            for (int i = 0; i < node.Count; i++)
            {
                DSNode tmp = node.GetChild(i);

                if (tmp.IsNull)
                {
                    result.Add(null);
                }
                else if (TypeCheckMethods.IsPrimitive(itemType))
                {
                    object? tmp2 = tmp.ConvertValue(itemType);
                    result.Add(tmp2);
                }
                else if (TypeCheckMethods.IsList(itemType) ||
                         TypeCheckMethods.IsArray(itemType))
                {
                    var tmpList = DeserializeList(tmp, itemType);
                    result.Add(tmpList);
                }
                else if (TypeCheckMethods.IsDictionary(itemType))
                {
                    var tmpDic = DeserializeDictionary(tmp, itemType);
                    result.Add(tmpDic);
                }
                else if (TypeCheckMethods.IsClass(itemType) ||
                         TypeCheckMethods.IsStruct(itemType))
                {
                    if (tmp.IsNull || tmp.IsEmpty)
                    {
                        result.Add(null);
                    }
                    else
                    {
                        object? tmpClass = CreateInstanceMethods.CreateInstanceGeneric(itemType);
                        Deserialize(tmpClass, tmp);

                        result.Add(tmpClass);
                    }
                }
                else
                {
                    throw new DSNotSupportedTypeException(itemType);
                }
            }

            return result;
        }
    }
}
