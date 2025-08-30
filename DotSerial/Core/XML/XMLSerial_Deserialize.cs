using System.Collections;
using System.Reflection;
using System.Xml;

namespace DotSerial.Core.XML
{
    public class XMLSerial_Deserialize
    {
        /// <summary> Deserialize Object
        /// </summary>
        /// <param name="classObj">Object</param>
        /// <param name="node">XmlNode</param>
        public static void Deserialize(object? classObj, XmlNode node)
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
                    foreach (XmlNode para in node.ChildNodes)
                    {
                        if (null == para || null == para.Attributes)
                        {
                            throw new NotSupportedException();
                        }

                        // Read AttribteID from xmlnode and cast it to int.
                        string? t = para.Attributes[Constants.IdAttribute]?.Value;
                        if (false == int.TryParse(t, out int idXML))
                        {
                            throw new NotSupportedException("ID could not be deserialized.");
                        }

                        // If AttributeID and ID from xml node match => deserialze.
                        if (id == idXML)
                        {
                            // String
                            if (prop.PropertyType == typeof(string))
                            {
                                // Note: String case MUST become before IEnumerable
                                // otherwise it will not be serialized correct.

                                // If string equals NullString
                                // => Object was null when it was serialzed.
                                if (para.InnerText.Equals(Constants.NullString))
                                {
                                    prop.SetValue(classObj, null);
                                    break;
                                }
                                prop.SetValue(classObj, para.InnerText);
                            }
                            // Char
                            else if (prop.PropertyType == typeof(char))
                            {
                                char tmp = char.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // Byte
                            else if (prop.PropertyType == typeof(byte))
                            {
                                byte tmp = byte.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // SByte
                            else if (prop.PropertyType == typeof(sbyte))
                            {
                                sbyte tmp = sbyte.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // Decimal
                            else if (prop.PropertyType == typeof(double))
                            {
                                double tmp = double.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // Float
                            else if (prop.PropertyType == typeof(float))
                            {
                                float tmp = float.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // Double
                            else if (prop.PropertyType == typeof(decimal))
                            {
                                decimal tmp = decimal.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // Int
                            else if (prop.PropertyType == typeof(int))
                            {
                                int tmp = int.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // UInt
                            else if (prop.PropertyType == typeof(uint))
                            {
                                uint tmp = uint.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // NInt
                            else if (prop.PropertyType == typeof(nint))
                            {
                                nint tmp = nint.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // NUInt
                            else if (prop.PropertyType == typeof(nuint))
                            {
                                nuint tmp = nuint.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // Long
                            else if (prop.PropertyType == typeof(long))
                            {
                                long tmp = long.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // ULong
                            else if (prop.PropertyType == typeof(ulong))
                            {
                                ulong tmp = ulong.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // Short
                            else if (prop.PropertyType == typeof(short))
                            {
                                short tmp = short.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // UShort
                            else if (prop.PropertyType == typeof(ushort))
                            {
                                ushort tmp = ushort.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // Boolean
                            else if (prop.PropertyType == typeof(bool))
                            {
                                // Special case bool
                                // Was casted to int in serialze.

                                int tmp = int.Parse(para.InnerText);
                                bool tmpBool = Misc.HelperMethods.IntToBool(tmp);
                                prop.SetValue(classObj, tmpBool);
                            }
                            // DateTime
                            else if (prop.PropertyType == typeof(DateTime))
                            {
                                DateTime tmp = DateTime.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // Enum
                            else if (true == prop.PropertyType.IsEnum)
                            {
                                int tmp = int.Parse(para.InnerText);
                                prop.SetValue(classObj, tmp);
                            }
                            // Generic List
                            else if (Misc.HelperMethods.ImplementsIEnumerable(prop.PropertyType))
                            {
                                if (para.InnerText.Equals(Constants.NullString))
                                {
                                    prop.SetValue(classObj, null);
                                    break;
                                }

                                Type itemType = Misc.HelperMethods.GetItemTypeOfIEnumerable(prop.PropertyType);
                                var tmpList = DeserializeList(para.ChildNodes, itemType);

                                if (null == tmpList)
                                {
                                    throw new NotSupportedException();
                                }

                                // Convert deserialzed list.
                                object? tmpValue = ConvertDeserializedList(tmpList, prop.PropertyType);
                                prop.SetValue(classObj, tmpValue);

                            }
                            else if (prop.PropertyType.IsClass)
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
                            else if (prop.PropertyType.IsValueType && !prop.PropertyType.IsPrimitive && !prop.PropertyType.IsEnum && prop.PropertyType != typeof(decimal))
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
                                throw new NotSupportedException("Typ could not be deserialized.");
                            }
                            break;
                        }
                    }
                }
            }
        }

        /// <summary> Converts the serialzed list to object so "PropertyInfo.SetValue" can
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
                    if (itemType == typeof(string))
                    {
                        // Note: String case MUST become before IEnumerable
                        // otherwise it will not be serialized correct.

                        if (isArray)
                            castedListResult[i] = castedList[i];
                        else
                            castedListResult.Add(castedList[i]);
                    }
                    else if (Misc.HelperMethods.ImplementsIEnumerable(itemType))
                    {
                        if (castedList[i] is not List<object?> castedListItemObj)
                        {
                            throw new NotSupportedException();
                        }

                        object? itemResult = ConvertDeserializedList(castedListItemObj, itemType);
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
                            throw new NotSupportedException();
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

            return null;
        }

        /// <summary> Deserialize a list
        /// </summary>
        /// <param name="xnodeList">XmlNodeList</param>
        /// <param name="type">Type</param>
        /// <returns>List of objects</returns>
        private static List<object?> DeserializeList(XmlNodeList xnodeList, Type type)
        {
            ArgumentNullException.ThrowIfNull(xnodeList);
            ArgumentNullException.ThrowIfNull(type);

            List<object?> result = [];

            foreach (XmlNode node in xnodeList)
            {
                if (type == typeof(string))
                {
                    // Note: String case MUST become before IEnumerable
                    // otherwise it will not be serialized correct.

                    DeserializeString(out string? tmpString, node);
                    result.Add(tmpString);
                }
                else if (Misc.HelperMethods.ImplementsIEnumerable(type))
                {
                    // If xml text equals NullString
                    // => Object was null when it was serialzed.
                    if (node.InnerText.Equals(Constants.NullString))
                    {
                        result.Add(null);
                        continue;
                    }

                    Type itemType = Misc.HelperMethods.GetItemTypeOfIEnumerable(type);
                    var tmpList = DeserializeList(node.ChildNodes, itemType);
                    result.Add(tmpList);

                }
                else
                {
                    object? tmp = Activator.CreateInstance(type);
                    if (null == tmp)
                    {
                        throw new NullReferenceException();
                    }

                    if (Misc.HelperMethods.IsPrimitive(type))
                    {
                        DeserializePrimitive(ref tmp, node);
                    }
                    else if (true == type.IsClass)
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
                    else if (type.IsValueType && !type.IsPrimitive && !type.IsEnum && type != typeof(decimal))
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
                        throw new NotSupportedException();
                    }

                    result.Add(tmp);
                }
            }

            return result;
        }

        /// <summary> Derserialize an string
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
                throw new NotSupportedException();
            }

            // If string equals NullString
            // => Object was null when it was serialzed.
            if (innerText.Equals(Constants.NullString))
            {
                innerText = null;
            }

            strObj = innerText;
        }

        /// <summary> Deserialze an primitive type
        /// </summary>
        /// <param name="primObj">Primitive Object</param>
        /// <param name="node">XMLNode</param>
        private static void DeserializePrimitive(ref object primObj, XmlNode node)
        {
            ArgumentNullException.ThrowIfNull(primObj);

            // Get type
            Type typeObj = primObj.GetType();

            // Check if primitive type
            if (!Misc.HelperMethods.IsPrimitive(typeObj))
            {
                throw new NotSupportedException();
            }

            if (node.ChildNodes == null || node.ChildNodes.Count != 1)
            {
                throw new NotSupportedException();
            }

            // Get text of xmlnode
            string? innerText = node.ChildNodes[0]?.InnerText;

            if (null == innerText)
            {
                throw new NotSupportedException();
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
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
