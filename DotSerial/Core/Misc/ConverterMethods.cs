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
using DotSerial.Core.XML;
using System.Collections;

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
        internal static object? ConvertDeserializedList(List<object?>? list, Type type)
        {
            if (null == list)
            {
                return null;
            }
            
            // Get Item type of list
            Type itemType = GetTypeMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            // TODO vom Interace abhänhgig machen:
            // https://stackoverflow.com/questions/196661/calling-a-static-method-on-a-generic-type-parameter
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
                    else if (TypeCheckMethods.IsList(itemType) ||
                             TypeCheckMethods.IsArray(itemType))
                    {
                        object? itemResult;
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

#pragma warning disable CS8604
                        object enumObj = ConvertEnumToObject(itemType, castedList[i]);
                        if (isArray)
                            castedListResult[i] = enumObj;
                        else
                            castedListResult.Add(enumObj);
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
                Type resultType = GetTypeMethods.GetDictionaryTypeFromKeyValue(keyType, valueType);
                object? result = CreateInstanceMethods.CreateInstanceGeneric(resultType);

                if (null == result)
                {
                    throw new NullReferenceException();
                }

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
                        else if (TypeCheckMethods.IsList(valueType) ||
                                 TypeCheckMethods.IsArray(valueType))
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

#pragma warning disable CS8604
                            castedDicResult.Add(keyValuePair.Key, ConvertEnumToObject(valueType, castedDic[keyValuePair.Key]));
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
            catch(Exception)
            {
                throw;
            }
        }

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
                throw new DSNotSupportedTypeException(typeObj);
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
                double tmp = double.Parse(str);
                primObj = tmp;
            }
            // Float
            else if (typeObj == typeof(float))
            {
                float tmp = float.Parse(str);
                primObj = tmp;
            }
            // Double
            else if (typeObj == typeof(decimal))
            {
                decimal tmp = decimal.Parse(str);
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
                int tmp = int.Parse(str);

                // Special case bool
                // Was casted to int in serialze.
                bool tmpBool = HelperMethods.IntToBool(tmp);
                primObj = tmpBool;
            }
            // DateTime
            else if (typeObj == typeof(DateTime))
            {
                DateTime tmp = DateTime.Parse(str);
                primObj = tmp;
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
                if (str.Equals(Constants.NullString))
                {
                    primObj = null;
                }
                else
                {
                    primObj = str;
                }
            }
            else
            {
                throw new DSNotSupportedTypeException(typeObj);
            }

            return primObj;
        }
    }
}
