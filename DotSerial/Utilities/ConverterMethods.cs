using System.Collections;
using System.Globalization;
using System.Net;

namespace DotSerial.Utilities
{
    /// <summary>
    /// Class is used to verious methods to convert objects to a specific type.
    /// </summary>
    internal static class ConverterMethods
    {
        /// <summary>
        /// Converts the serialzed list to object so "PropertyInfo.SetValue" can
        /// set the value properly
        /// </summary>
        /// <param name="list">Deserialzed List</param>
        /// <param name="type">Type</param>
        /// <returns>Converted list</returns>
        internal static object? ConvertDeserializedList(List<object?>? list, Type type)
        {
            if (null == list)
            {
                return null;
            }

            // Handles case Stack.
            if (TypeCheckMethods.IsStack(type))
            {
                return ConvertDeserializedListFromStack(list, type);
            }
            // Handles case Queue.
            else if (TypeCheckMethods.IsQueue(type))
            {
                return ConvertDeserializedListFromQueue(list, type);
            }
            // Handles case LinkedList.
            else if (TypeCheckMethods.IsLinkedList(type))
            {
                return ConvertDeserializedListFromLinkedList(list, type);
            }

            // Get Item type of list
            Type itemType = GetTypeMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            if (false == TypeCheckMethods.IsTypeSupported(itemType))
            {
                ThrowHelper.ThrowTypeIsNotSupportedException(itemType);
            }

            // Check if type is array
            bool isArray = type.IsArray;

            // result object
            object? result;

            // Create initial object to fill.
            if (isArray)
            {
                result = CreateInstanceMethods.CreateInstanceArray(type, list.Count);
            }
            else
            {
                result = CreateInstanceMethods.CreateInstanceGeneric(type);
            }

            if (list is IList castedList && result is IList castedListResult)
            {
                for (int i = 0; i < castedList.Count; i++)
                {
                    if (TypeCheckMethods.IsDictionary(itemType))
                    {
                        object? itemResult;
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
                    else if (TypeCheckMethods.IsList(itemType) || TypeCheckMethods.IsArray(itemType))
                    {
                        object? itemResult;
                        if (castedList[i] is not List<object?> castedListItemObj)
                        {
                            if (castedList[i] != null)
                            {
                                if (isArray)
                                    castedListResult[i] = castedList[i];
                                else
                                    castedListResult.Add(castedList[i]);
                            }

                            continue;
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
                        ThrowHelper.ThrowIfNullException(castedList[i]);

#pragma warning disable CS8604
                        object enumObj = ConvertEnumToObject(itemType, castedList[i]);
                        if (isArray)
                            castedListResult[i] = enumObj;
                        else
                            castedListResult.Add(enumObj);
#pragma warning restore CS8604
                    }
                    else if (
                        TypeCheckMethods.IsClass(itemType)
                        || TypeCheckMethods.IsStruct(itemType)
                        || TypeCheckMethods.IsPrimitive(itemType)
                        || TypeCheckMethods.IsSpecialParsableObject(itemType)
                    )
                    {
                        if (isArray)
                            castedListResult[i] = castedList[i];
                        else
                            castedListResult.Add(castedList[i]);
                    }
                    else
                    {
                        ThrowHelper.ThrowTypeIsNotSupportedException(itemType);
                    }
                }

                return castedListResult;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        private static object? ConvertDeserializedListFromQueue(List<object?>? list, Type type)
        {
            if (null == list)
            {
                return null;
            }

            if (false == TypeCheckMethods.IsQueue(type))
            {
                throw new InvalidCastException();
            }

            // Get Item type of list
            Type itemType = GetTypeMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            if (false == TypeCheckMethods.IsTypeSupported(itemType))
            {
                ThrowHelper.ThrowTypeIsNotSupportedException(itemType);
            }

            // result object
            object? result;

            // Create initial object to fill.
            result = CreateInstanceMethods.CreateInstanceGeneric(type);

            var enqueueMethod = type.GetMethod("Enqueue") ?? throw new InvalidCastException();

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];

                if (TypeCheckMethods.IsDictionary(itemType))
                {
                    if (item is not Dictionary<object, object?> castedDictionaryItemObj)
                    {
                        throw new InvalidCastException();
                    }
                    object? itemResult = ConvertDeserializedDictionary(castedDictionaryItemObj, itemType);

                    if (itemResult != null)
                    {
                        _ = enqueueMethod.Invoke(result, [itemResult]);
                    }
                }
                else if (TypeCheckMethods.IsList(itemType) || TypeCheckMethods.IsArray(itemType))
                {
                    if (item is not List<object?> castedListItemObj)
                    {
                        if (item != null)
                        {
                            _ = enqueueMethod.Invoke(result, [item]);
                        }

                        continue;
                    }

                    object? itemResult = ConvertDeserializedList(castedListItemObj, itemType);

                    if (itemResult != null)
                    {
                        _ = enqueueMethod.Invoke(result, [itemResult]);
                    }
                }
                else if (itemType.IsEnum)
                {
                    ThrowHelper.ThrowIfNullException(item);

#pragma warning disable CS8604
                    object enumObj = ConvertEnumToObject(itemType, item);
                    _ = enqueueMethod.Invoke(result, [enumObj]);
#pragma warning restore CS8604
                }
                else if (
                    TypeCheckMethods.IsClass(itemType)
                    || TypeCheckMethods.IsStruct(itemType)
                    || TypeCheckMethods.IsPrimitive(itemType)
                    || TypeCheckMethods.IsSpecialParsableObject(itemType)
                )
                {
                    _ = enqueueMethod.Invoke(result, [item]);
                }
                else
                {
                    ThrowHelper.ThrowTypeIsNotSupportedException(itemType);
                }
            }

            return result;
        }

        private static object? ConvertDeserializedListFromLinkedList(List<object?>? list, Type type)
        {
            if (null == list)
            {
                return null;
            }

            if (false == TypeCheckMethods.IsLinkedList(type))
            {
                throw new InvalidCastException();
            }

            // Get Item type of list
            Type itemType = GetTypeMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            if (false == TypeCheckMethods.IsTypeSupported(itemType))
            {
                ThrowHelper.ThrowTypeIsNotSupportedException(itemType);
            }

            // result object
            object? result;

            // Create initial object to fill.
            result = CreateInstanceMethods.CreateInstanceGeneric(type);

            var addLastMethod = type.GetMethod("AddLast", [itemType]) ?? throw new InvalidCastException();

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];

                if (TypeCheckMethods.IsDictionary(itemType))
                {
                    if (item is not Dictionary<object, object?> castedDictionaryItemObj)
                    {
                        throw new InvalidCastException();
                    }
                    object? itemResult = ConvertDeserializedDictionary(castedDictionaryItemObj, itemType);

                    if (itemResult != null)
                    {
                        _ = addLastMethod.Invoke(result, [itemResult]);
                    }
                }
                else if (TypeCheckMethods.IsList(itemType) || TypeCheckMethods.IsArray(itemType))
                {
                    if (item is not List<object?> castedListItemObj)
                    {
                        if (item != null)
                        {
                            _ = addLastMethod.Invoke(result, [item]);
                        }

                        continue;
                    }

                    object? itemResult = ConvertDeserializedList(castedListItemObj, itemType);

                    if (itemResult != null)
                    {
                        _ = addLastMethod.Invoke(result, [itemResult]);
                    }
                }
                else if (itemType.IsEnum)
                {
                    ThrowHelper.ThrowIfNullException(item);

#pragma warning disable CS8604
                    object enumObj = ConvertEnumToObject(itemType, item);
                    _ = addLastMethod.Invoke(result, [enumObj]);
#pragma warning restore CS8604
                }
                else if (
                    TypeCheckMethods.IsClass(itemType)
                    || TypeCheckMethods.IsStruct(itemType)
                    || TypeCheckMethods.IsPrimitive(itemType)
                    || TypeCheckMethods.IsSpecialParsableObject(itemType)
                )
                {
                    _ = addLastMethod.Invoke(result, [item]);
                }
                else
                {
                    ThrowHelper.ThrowTypeIsNotSupportedException(itemType);
                }
            }

            return result;
        }

        private static object? ConvertDeserializedListFromStack(List<object?>? list, Type type)
        {
            if (null == list)
            {
                return null;
            }

            if (false == TypeCheckMethods.IsStack(type))
            {
                throw new InvalidCastException();
            }

            // Get Item type of list
            Type itemType = GetTypeMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            if (false == TypeCheckMethods.IsTypeSupported(itemType))
            {
                ThrowHelper.ThrowTypeIsNotSupportedException(itemType);
            }

            // result object
            object? result;

            // Create initial object to fill.
            result = CreateInstanceMethods.CreateInstanceGeneric(type);

            var pushMethod = type.GetMethod("Push") ?? throw new InvalidCastException();

            // Push in reverse order to maintain original stack order
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];

                if (TypeCheckMethods.IsDictionary(itemType))
                {
                    if (item is not Dictionary<object, object?> castedDictionaryItemObj)
                    {
                        throw new InvalidCastException();
                    }
                    object? itemResult = ConvertDeserializedDictionary(castedDictionaryItemObj, itemType);

                    if (itemResult != null)
                    {
                        _ = pushMethod.Invoke(result, [itemResult]);
                    }
                }
                else if (TypeCheckMethods.IsList(itemType) || TypeCheckMethods.IsArray(itemType))
                {
                    if (item is not List<object?> castedListItemObj)
                    {
                        if (item != null)
                        {
                            _ = pushMethod.Invoke(result, [item]);
                        }

                        continue;
                    }

                    object? itemResult = ConvertDeserializedList(castedListItemObj, itemType);

                    if (itemResult != null)
                    {
                        _ = pushMethod.Invoke(result, [itemResult]);
                    }
                }
                else if (itemType.IsEnum)
                {
                    ThrowHelper.ThrowIfNullException(item);

#pragma warning disable CS8604
                    object enumObj = ConvertEnumToObject(itemType, item);
                    _ = pushMethod.Invoke(result, [enumObj]);
#pragma warning restore CS8604
                }
                else if (
                    TypeCheckMethods.IsClass(itemType)
                    || TypeCheckMethods.IsStruct(itemType)
                    || TypeCheckMethods.IsPrimitive(itemType)
                    || TypeCheckMethods.IsSpecialParsableObject(itemType)
                )
                {
                    _ = pushMethod.Invoke(result, [item]);
                }
                else
                {
                    ThrowHelper.ThrowTypeIsNotSupportedException(itemType);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the serialzed dictionary to object so "PropertyInfo.SetValue" can
        /// set the value properly
        /// </summary>
        /// <param name="dic">Deserialzed Dictionary</param>
        /// <param name="type">Type</param>
        /// <returns>Converted dictionary</returns>
        internal static object? ConvertDeserializedDictionary(Dictionary<object, object?>? dic, Type type)
        {
            if (null == dic)
            {
                return null;
            }

            // Get Item type of dictionary
            if (GetTypeMethods.GetKeyValueTypeOfDictionary(type, out Type keyType, out Type valueType))
            {
                // Check if type is supported
                if (false == TypeCheckMethods.IsTypeSupported(keyType))
                {
                    ThrowHelper.ThrowTypeIsNotSupportedException(keyType);
                }
                // Check if type is supported
                if (false == TypeCheckMethods.IsTypeSupported(valueType))
                {
                    ThrowHelper.ThrowTypeIsNotSupportedException(valueType);
                }

                // result object
                Type resultType = GetTypeMethods.GetDictionaryTypeFromKeyValue(keyType, valueType);
                object? result = CreateInstanceMethods.CreateInstanceGeneric(resultType);

                ThrowHelper.ThrowIfNullException(result);

                if (dic is IDictionary castedDic && result is IDictionary castedDicResult)
                {
                    foreach (DictionaryEntry keyValuePair in castedDic)
                    {
                        if (TypeCheckMethods.IsDictionary(valueType))
                        {
                            object? itemResult = null;
                            if (castedDic[keyValuePair.Key] is not Dictionary<object, object?> castedDictionaryItemObj)
                            {
                                throw new InvalidCastException();
                            }
                            itemResult = ConvertDeserializedDictionary(castedDictionaryItemObj, valueType);

                            castedDicResult.Add(keyValuePair.Key, itemResult);
                        }
                        else if (TypeCheckMethods.IsList(valueType) || TypeCheckMethods.IsArray(valueType))
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
                            ThrowHelper.ThrowIfNullException(castedDic[keyValuePair.Key]);

#pragma warning disable CS8604
                            castedDicResult.Add(
                                keyValuePair.Key,
                                ConvertEnumToObject(valueType, castedDic[keyValuePair.Key])
                            );
#pragma warning restore CS8604
                        }
                        else if (
                            TypeCheckMethods.IsClass(valueType)
                            || TypeCheckMethods.IsStruct(valueType)
                            || TypeCheckMethods.IsPrimitive(valueType)
                        )
                        {
                            object? key = TypeCheckMethods.IsPrimitive(keyType)
                                ? ConvertStringToPrimitive(keyValuePair.Key.ToString(), keyType)
                                : ConvertStringToSpecialParsableObject(keyValuePair.Key.ToString(), keyType);
#pragma warning disable CS8604
                            castedDicResult.Add(key, keyValuePair.Value);
#pragma warning restore CS8604
                        }
                        else if (TypeCheckMethods.IsSpecialParsableObject(valueType))
                        {
                            object? key = TypeCheckMethods.IsPrimitive(keyType)
                                ? ConvertStringToPrimitive(keyValuePair.Key.ToString(), keyType)
                                : ConvertStringToSpecialParsableObject(keyValuePair.Key.ToString(), keyType);
#pragma warning disable CS8604
                            castedDicResult.Add(key, keyValuePair.Value);
#pragma warning restore CS8604
                        }
                        else
                        {
                            ThrowHelper.ThrowTypeIsNotSupportedException(valueType);
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
        /// Converts an enum object to an object
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="enumObj">Enum</param>
        /// <returns>Object</returns>
        internal static object ConvertEnumToObject(Type type, object enumObj)
        {
            ArgumentNullException.ThrowIfNull(type);
            ArgumentNullException.ThrowIfNull(enumObj);

            try
            {
                return Enum.ToObject(type, enumObj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Convert string to special parsable object
        /// </summary>
        /// <param name="str">string</param>
        /// <param name="type">type</param>
        /// <returns>Object</returns>
        internal static object? ConvertStringToSpecialParsableObject(string? str, Type type)
        {
            if (str == null)
            {
                return null;
            }

            // Get type
            Type typeObj = type;

            // Check if primitive type
            if (!TypeCheckMethods.IsSpecialParsableObject(typeObj))
            {
                ThrowHelper.ThrowTypeIsNotSupportedException(typeObj);
            }

            object? primObj;

            // DateTime
            if (typeObj == typeof(DateTime))
            {
                DateTime tmp = DateTime.Parse(str);
                primObj = tmp;
            }
            else if (type == typeof(Guid))
            {
                Guid tmp = Guid.Parse(str);
                primObj = tmp;
            }
            else if (type == typeof(TimeSpan))
            {
                TimeSpan tmp = TimeSpan.Parse(str);
                primObj = tmp;
            }
            else if (type == typeof(Uri))
            {
                Uri tmp = new(str);
                primObj = tmp;
            }
            else if (type == typeof(IPAddress))
            {
                IPAddress tmp = IPAddress.Parse(str);
                primObj = tmp;
            }
            else if (type == typeof(Version))
            {
                Version tmp = Version.Parse(str);
                primObj = tmp;
            }
            else if (type == typeof(CultureInfo))
            {
                CultureInfo tmp = new(str);
                primObj = tmp;
            }
            else
            {
                ThrowHelper.ThrowTypeIsNotSupportedException(typeObj);
                throw new Exception("Unreachable code.");
            }

            return primObj;
        }

        /// <summary>
        /// Convert string to primitive
        /// </summary>
        /// <param name="str">string</param>
        /// <param name="primType">type</param>
        /// <returns>Object</returns>
        internal static object? ConvertStringToPrimitive(string? str, Type primType)
        {
            if (str == null)
            {
                return null;
            }

            // Get type
            Type typeObj = primType;

            // Check if primitive type
            if (!TypeCheckMethods.IsPrimitive(typeObj))
            {
                ThrowHelper.ThrowTypeIsNotSupportedException(typeObj);
            }

            object? primObj;

            // Char
            if (typeObj == typeof(char))
            {
                char tmp = char.Parse(str);
                primObj = tmp;
            }
            // Byte
            else if (typeObj == typeof(byte))
            {
                byte tmp = byte.Parse(str);
                primObj = tmp;
            }
            // SByte
            else if (typeObj == typeof(sbyte))
            {
                sbyte tmp = sbyte.Parse(str);
                primObj = tmp;
            }
            // Decimal
            else if (typeObj == typeof(double))
            {
                double tmp = double.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
                primObj = tmp;
            }
            // Float
            else if (typeObj == typeof(float))
            {
                float tmp = float.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
                primObj = tmp;
            }
            // Double
            else if (typeObj == typeof(decimal))
            {
                decimal tmp = decimal.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
                primObj = tmp;
            }
            // Int
            else if (typeObj == typeof(int))
            {
                int tmp = int.Parse(str);
                primObj = tmp;
            }
            // UInt
            else if (typeObj == typeof(uint))
            {
                uint tmp = uint.Parse(str);
                primObj = tmp;
            }
            // NInt
            else if (typeObj == typeof(nint))
            {
                nint tmp = nint.Parse(str);
                primObj = tmp;
            }
            // NUInt
            else if (typeObj == typeof(nuint))
            {
                nuint tmp = nuint.Parse(str);
                primObj = tmp;
            }
            // Long
            else if (typeObj == typeof(long))
            {
                long tmp = long.Parse(str);
                primObj = tmp;
            }
            // ULong
            else if (typeObj == typeof(ulong))
            {
                ulong tmp = ulong.Parse(str);
                primObj = tmp;
            }
            // Short
            else if (typeObj == typeof(short))
            {
                short tmp = short.Parse(str);
                primObj = tmp;
            }
            // UShort
            else if (typeObj == typeof(ushort))
            {
                ushort tmp = ushort.Parse(str);
                primObj = tmp;
            }
            // Boolean
            else if (typeObj == typeof(bool))
            {
                // Special case bool
                primObj = HelperMethods.StringToBool(str);
            }
            // Enum
            else if (true == typeObj.IsEnum)
            {
                int tmp = int.Parse(str);
                primObj = tmp;
            }
            // String
            else if (typeObj == typeof(string))
            {
                primObj = str;
            }
            else
            {
                ThrowHelper.ThrowTypeIsNotSupportedException(typeObj);
                throw new Exception("Unreachable code.");
            }

            return primObj;
        }
    }
}
