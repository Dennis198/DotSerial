using System.Collections;

using DotSerial.Core.Exceptions;
using DotSerial.Core.XML;

namespace DotSerial.Core.Misc
{
    internal static class ConverterMethods
    {
        /// <summary> 
        /// Converts the serialzed list to object so "PropertyInfo.SetValue" can
        /// set the value properly
        /// </summary>
        /// <param name="list">Deserialzed List</param>
        /// <param name="type">Type</param>
        /// <returns>Converted list</returns>
        internal static object? ConvertDeserializedList(List<object?> list, Type type)
        {
            if (null == list)
            {
                return null;
            }

            // Get Item type of list
            Type itemType = Misc.GetTypeMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            if (false == DotSerialXML.IsTypeSupported(itemType))
            {
                throw new DSNotSupportedTypeException(itemType);
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
                    if (Misc.TypeCheckMethods.IsDictionary(itemType))
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
                    else if (Misc.TypeCheckMethods.IsList(itemType) ||
                             Misc.TypeCheckMethods.IsArray(itemType))
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
                    else if (Misc.TypeCheckMethods.IsHashSet(itemType))
                    {
                        object? itemResult = null;
                        if (castedList[i] is not List<object?> castedListItemObj)
                        {
                            throw new InvalidCastException();
                        }

                        itemResult = ConvertDeserializeHashSet(castedListItemObj, itemType);

                        if (itemResult != null)
                        {
                            castedListResult.Add(itemResult);
                        }
                    }
                    else if (itemType.IsEnum)
                    {
                        if (null == castedList[i])
                        {
                            throw new NullReferenceException();
                        }

#pragma warning disable CS8604
                        if (isArray)
                            castedListResult[i] = Enum.ToObject(itemType, castedList[i]);
                        else
                            castedListResult.Add(Enum.ToObject(itemType, castedList[i]));
#pragma warning restore CS8604
                    }
                    else if (TypeCheckMethods.IsClass(itemType) ||
                             TypeCheckMethods.IsStruct(itemType) ||
                             TypeCheckMethods.IsPrimitive(itemType))
                    {
                        if (isArray)
                            castedListResult[i] = castedList[i];
                        else
                            castedListResult.Add(castedList[i]);
                    }
                    else
                    {
                        throw new DSNotSupportedTypeException(itemType);
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
        internal static object? ConvertDeserializedDictionary(Dictionary<object, object?> dic, Type type)
        {
            if (null == dic)
            {
                return null;
            }

            // Get Item type of dictionary            
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
                        if (Misc.TypeCheckMethods.IsDictionary(valueType))
                        {
                            object? itemResult = null;
                            if (castedDic[keyValuePair.Key] is not Dictionary<object, object?> castedDictionaryItemObj)
                            {
                                throw new InvalidCastException();
                            }
                            itemResult = ConvertDeserializedDictionary(castedDictionaryItemObj, valueType);

                            castedDicResult.Add(keyValuePair.Key, itemResult);
                        }
                        else if (Misc.TypeCheckMethods.IsList(valueType) ||
                                 Misc.TypeCheckMethods.IsArray(valueType))
                        {
                            object? itemResult = null;
                            if (castedDic[keyValuePair.Key] is not List<object?> castedListItemObj)
                            {
                                throw new InvalidCastException();
                            }

                            itemResult = ConvertDeserializedList(castedListItemObj, valueType);

                            castedDicResult.Add(keyValuePair.Key, itemResult);
                        }
                        else if (Misc.TypeCheckMethods.IsHashSet(valueType))
                        {
                            object? itemResult = null;
                            if (castedDic[keyValuePair.Key] is not List<object?> castedListItemObj)
                            {
                                throw new InvalidCastException();
                            }

                            itemResult = ConvertDeserializeHashSet(castedListItemObj, valueType);

                            if (itemResult != null)
                            {
                                castedDicResult.Add(keyValuePair.Key, itemResult);
                            }
                        }
                        else if (valueType.IsEnum)
                        {
                            if (null == castedDic[keyValuePair.Key])
                            {
                                throw new NullReferenceException();
                            }

#pragma warning disable CS8604
                            castedDicResult.Add(keyValuePair.Key, Enum.ToObject(valueType, castedDic[keyValuePair.Key]));
#pragma warning restore CS8604
                        }
                        else if (TypeCheckMethods.IsClass(valueType) ||
                                 TypeCheckMethods.IsStruct(valueType) ||
                                 TypeCheckMethods.IsPrimitive(valueType))
                        {
                            castedDicResult.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                        else
                        {
                            throw new DSNotSupportedTypeException(valueType);
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
        /// Converts the serialzed hashset to object so "PropertyInfo.SetValue" can
        /// set the value properly
        /// </summary>
        /// <param name="hashSet">Deserialzed HashSet</param>
        /// <param name="type">Type</param>
        /// <returns>Converted hashset</returns>
        internal static object? ConvertDeserializeHashSet(List<object?> hashSet, Type type)
        {
            if (null == hashSet)
            {
                return null;
            }

            // Get Item type of list
            Type itemType = Misc.GetTypeMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            if (false == DotSerialXML.IsTypeSupported(itemType))
            {
                throw new DSNotSupportedTypeException(itemType);
            }


            dynamic? result = Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(itemType));

            if (null != result)
            {
                foreach (object? obj in hashSet)
                {
                    if (obj.GetType().IsEnum)
                    {
                        throw new DSNotSupportedTypeException(type, itemType);
                    }

                    if (TypeCheckMethods.IsPrimitive(obj.GetType()))
                    {
                        dynamic itemResult = obj;
                        result.Add(itemResult);
                    }
                    else 
                    {
                        throw new DSNotSupportedTypeException(type, itemType);
                    }


//                    if (Misc.TypeCheckMethods.IsDictionary(itemType))
//                    {
//                        object? itemResult = null;
//                        if (obj is not Dictionary<object, object?> castedDictionaryItemObj)
//                        {
//                            throw new InvalidCastException();
//                        }
//                        itemResult = ConvertDeserializedDictionary(castedDictionaryItemObj, itemType);

//                        if (itemResult != null)
//                        {
//                            result.Add(itemResult);
//                        }
//                    }
//                    else if (Misc.TypeCheckMethods.IsList(itemType) ||
//                             Misc.TypeCheckMethods.IsArray(itemType))
//                    {
//                        object? itemResult = null;
//                        if (obj is not List<object?> castedListItemObj)
//                        {
//                            throw new InvalidCastException();
//                        }

//                        itemResult = ConvertDeserializedList(castedListItemObj, itemType);

//                        if (itemResult != null)
//                        {
//                            result.Add(itemResult);
//                        }
//                    }
//                    else if (Misc.TypeCheckMethods.IsHashSet(itemType))
//                    {
//                        object? itemResult = null;
//                        if (obj is not List<object?> castedListItemObj)
//                        {
//                            throw new InvalidCastException();
//                        }

//                        itemResult = ConvertDeserializeHashSet(castedListItemObj, itemType);

//                        if (itemResult != null)
//                        {
//                            result.Add(itemResult);
//                        }
//                    }
//                    else if (itemType.IsEnum)
//                    {
//                        if (null == obj)
//                        {
//                            throw new NullReferenceException();
//                        }

//#pragma warning disable CS8604
//                        result.Add(Enum.ToObject(itemType, obj));
//#pragma warning restore CS8604
//                    }
//                    else if (TypeCheckMethods.IsPrimitive(obj.GetType()))
//                    {
//                        dynamic itemResult = obj;
//                        result.Add(itemResult);
//                    }
//                    else if (TypeCheckMethods.IsClass(obj.GetType()) ||
//                             TypeCheckMethods.IsStruct(obj.GetType()) ||
//                             TypeCheckMethods.IsPrimitive(obj.GetType()))
//                    {
//                        dynamic itemResult = obj;
//                        result.Add(itemResult);
//                    }
//                    else
//                    {
//                        throw new NotSupportedTypeException(itemType);
//                    }
                }
            }


            return result;
        }
    }
}
