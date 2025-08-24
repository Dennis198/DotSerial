using System.Collections;
using System.ComponentModel.Design;
using System.Reflection;
using System.Xml;

namespace DotSerial.Core.XML
{
    public class XMLSerial_Deserialize
    {
        private static void DeserializeString(out string classObj, XmlNode node)
        {
            if (node.ChildNodes == null || node.ChildNodes.Count != 1)
            {
                throw new NotSupportedException();
            }

            string innerText = node.ChildNodes[0].InnerText;

            if (innerText.Equals(Constants.NullString))
            {
                innerText = null;
            }

            classObj = innerText;
        }

        private static void DeserializePrimitive(ref object classObj, XmlNode node)
        {
            ArgumentNullException.ThrowIfNull(classObj);
            Type typeObj = classObj.GetType();

            if (!Misc.HelperMethods.IsPrimitive(typeObj))
            {
                throw new NotSupportedException();
            }

            if (node.ChildNodes == null || node.ChildNodes.Count != 1)
            {
                throw new NotSupportedException();
            }

            string innerText = node.ChildNodes[0].InnerText;

            // Char
            if (typeObj == typeof(char))
            {
                char tmp = char.Parse(innerText);
                classObj = tmp;
            }
            // Byte
            else if (typeObj == typeof(byte))
            {
                byte tmp = byte.Parse(innerText);
                classObj = tmp;
            }
            // SByte
            else if (typeObj == typeof(sbyte))
            {
                sbyte tmp = sbyte.Parse(innerText);
                classObj = tmp;
            }
            // Decimal
            else if (typeObj == typeof(double))
            {
                double tmp = double.Parse(innerText);
                classObj = tmp;
            }
            // Float
            else if (typeObj == typeof(float))
            {
                float tmp = float.Parse(innerText);
                classObj = tmp;
            }
            // Double
            else if (typeObj == typeof(decimal))
            {
                decimal tmp = decimal.Parse(innerText);
                classObj = tmp;
            }
            // Int
            else if (typeObj == typeof(int))
            {
                int tmp = int.Parse(innerText);
                classObj = tmp;
            }
            // UInt
            else if (typeObj == typeof(uint))
            {
                uint tmp = uint.Parse(innerText);
                classObj = tmp;
            }
            // NInt
            else if (typeObj == typeof(nint))
            {
                nint tmp = nint.Parse(innerText);
                classObj = tmp;
            }
            // NUInt
            else if (typeObj == typeof(nuint))
            {
                nuint tmp = nuint.Parse(innerText);
                classObj = tmp;
            }
            // Long
            else if (typeObj == typeof(long))
            {
                long tmp = long.Parse(innerText);
                classObj = tmp;
            }
            // ULong
            else if (typeObj == typeof(ulong))
            {
                ulong tmp = ulong.Parse(innerText);
                classObj = tmp;
            }
            // Short
            else if (typeObj == typeof(short))
            {
                short tmp = short.Parse(innerText);
                classObj = tmp;
            }
            // UShort
            else if (typeObj == typeof(ushort))
            {
                ushort tmp = ushort.Parse(innerText);
                classObj = tmp;
            }
            // Boolean
            else if (typeObj == typeof(bool))
            {
                int tmp = int.Parse(innerText);
                bool tmpBool = Misc.HelperMethods.IntToBool(tmp);
                classObj = tmpBool;
            }
            // DateTime
            else if (typeObj == typeof(DateTime))
            {
                DateTime tmp = DateTime.Parse(innerText);
                classObj = tmp;
            }
            // Enum
            else if (true == typeObj.IsEnum)
            {
                int tmp = int.Parse(innerText);
                classObj = tmp;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        /// <summary> Deserialize Object
        /// </summary>
        /// <param name="classObj">Object</param>
        /// <param name="node">XmlNode</param>
        public static void Deserialize(object? classObj, XmlNode node)
        {
            if (node.InnerText.Equals(Constants.NullString))
            {
                classObj = null;
                return;
            }

            ArgumentNullException.ThrowIfNull(classObj);
            Type typeObj = classObj.GetType();

            // Get all properties for class
            PropertyInfo[] props = typeObj.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                int id = Attributes.HelperMethods.GetPropertyID(prop);

                if (-1 != id)
                {
                    foreach (XmlNode para in node.ChildNodes)
                    {
                        var t = para.Attributes[Constants.IdAttribute]?.Value;
                        if (false == int.TryParse(t, out int idXML))
                        {
                            throw new NotSupportedException("ID could not be deserialized.");
                        }
                        if (id == idXML)
                        {
                            // String
                            if (prop.PropertyType == typeof(string))
                            {
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
                                object? tmpValue = prop.GetValue(classObj);

                                if (null != tmpValue)
                                {
                                    if (tmpList is IList castedList)
                                    {
                                        if (tmpValue is IList castedListTarget)
                                        {
                                            foreach (var entry in castedList)
                                            {
                                                castedListTarget.Add(entry);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        throw new InvalidCastException();
                                    }
                                }
                                else
                                {
                                    if (prop.PropertyType.IsArray)
                                    {
                                        tmpValue = Array.CreateInstanceFromArrayType(prop.PropertyType, tmpList.Count);

                                        if (tmpList is IList castedList)
                                        {
                                            if (tmpValue is IList castedListTarget)
                                            {
                                                for (int i = 0; i < castedList.Count; i++)
                                                {
                                                    if (itemType == typeof(string))
                                                    {
                                                        //castedListTarget.Add(castedList[i]);
                                                        castedListTarget[i] = castedList[i];
                                                    }
                                                    else if (Misc.HelperMethods.ImplementsIEnumerable(itemType))
                                                    {
                                                        int h = 0;
                                                        if (castedList[i] is IList hhhhh)
                                                        {
                                                            object? tmpValue2 = Array.CreateInstanceFromArrayType(itemType, hhhhh.Count);
                                                            
                                                            if (tmpValue2 is IList gggg)
                                                            {
                                                                Type itemitemType = Misc.HelperMethods.GetItemTypeOfIEnumerable(gggg);

                                                                for (int j = 0; j < hhhhh.Count; j++)
                                                                {
                                                                    if (false == itemitemType.IsEnum)
                                                                    {
                                                                        gggg[j] = hhhhh[j];
                                                                    }
                                                                    else
                                                                    {
                                                                        gggg[j] = Enum.ToObject(itemitemType, hhhhh[j]);
                                                                    }
                                                                    
                                                                }
                                                                castedListTarget[i] = gggg;
                                                            }
                                                            else
                                                            {
                                                                throw new NotSupportedException();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            castedListTarget[i] = null;
                                                        }
                                                    }
                                                    else if (itemType.IsEnum)
                                                    {
                                                        castedListTarget[i] = Enum.ToObject(itemType, castedList[i]);
                                                    }
                                                    else
                                                    {
                                                        castedListTarget[i] = castedList[i];
                                                    }
                                                }

                                                prop.SetValue(classObj, castedListTarget);
                                            }
                                        }
                                        else
                                        {
                                            throw new InvalidCastException();
                                        }
                                    }
                                    else
                                    {
                                        tmpValue = Activator.CreateInstance(prop.PropertyType);

                                        if (tmpList is IList castedList)
                                        {
                                            if (tmpValue is IList castedListTarget)
                                            {
                                                for (int i = 0; i < castedList.Count; i++)
                                                {
                                                    if (itemType == typeof(string))
                                                    {
                                                        castedListTarget.Add(castedList[i]);
                                                    }
                                                    else if (Misc.HelperMethods.ImplementsIEnumerable(itemType))
                                                    {
                                                        int h = 0;
                                                        if (castedList[i] is IList hhhhh)
                                                        {
                                                            object? tmpValue2 = Activator.CreateInstance(itemType);
                                                            if (tmpValue2 is IList gggg)
                                                            {
                                                                for (int j = 0; j < hhhhh.Count; j++)
                                                                {
                                                                    gggg.Add(hhhhh[j]);
                                                                    //gggg[j] = hhhhh[j];
                                                                }
                                                                //castedListTarget[i] = gggg;
                                                                castedListTarget.Add(gggg);
                                                            }
                                                            else
                                                            {
                                                                throw new NotSupportedException();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            castedListTarget.Add(null);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //castedListTarget[i] = castedList[i];
                                                        castedListTarget.Add(castedList[i]);
                                                    }

                                                }

                                                prop.SetValue(classObj, castedListTarget);
                                            }
                                        }
                                        else
                                        {
                                            throw new InvalidCastException();
                                        }
                                    }
                                        
                                }
                            }
                            else if (prop.PropertyType.IsClass)
                            {
                                object? tmp = Activator.CreateInstance(prop.PropertyType);
                                if (null != tmp)
                                {
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

        /// <summary> Deserialize a list
        /// </summary>
        /// <param name="xnodeList">XmlNodeList</param>
        /// <param name="type">Type</param>
        /// <returns>List of objects</returns>
        private static List<object> DeserializeList(XmlNodeList xnodeList, Type type)
        {
            ArgumentNullException.ThrowIfNull(xnodeList);
            ArgumentNullException.ThrowIfNull(type);

            List<object> result = [];

            foreach (XmlNode node in xnodeList)
            {
                if (type == typeof(string))
                {
                    DeserializeString(out string tmpString, node);
                    result.Add(tmpString);
                    continue;
                }
                else if (Misc.HelperMethods.ImplementsIEnumerable(type))
                {
                    if (node.InnerText.Equals(Constants.NullString))
                    {
                        result.Add(null);
                        continue;
                    }

                    Type itemType = Misc.HelperMethods.GetItemTypeOfIEnumerable(type);
                    var tmpList = DeserializeList(node.ChildNodes, itemType);
                    result.Add(tmpList);
                    continue;
                }

                // TODO: Sonder für string
                object tmp = Activator.CreateInstance(type);
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
                else
                {
                    throw new NotSupportedException();
                }

                result.Add(tmp);
            }

            return result;
        }
    }
}
