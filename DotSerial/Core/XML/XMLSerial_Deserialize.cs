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
        public static void Deserialize(object classObj, XmlNode node)
        {
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
                                prop.SetValue(classObj, para.InnerText);
                            }
                            // Int
                            else if (prop.PropertyType == typeof(int))
                            {
                                int tmp = int.Parse(para.InnerText);
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
                                Type enumType = prop.PropertyType;
                                prop.SetValue(classObj, tmp);
                            }
                            // Generic List
                            else if (Misc.HelperMethods.ImplementsIEnumerable(prop.PropertyType))
                            {
                                Type itemType = Misc.HelperMethods.GetItemTypeOfIEnumerable(prop.PropertyType);
                                var tmpList = DeserializeList(para.ChildNodes, itemType);
                                object? tmpValue = prop.GetValue(classObj);

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
                object tmp = Activator.CreateInstance(type);
                if (null == tmp)
                {
                    throw new NullReferenceException();
                }
                Deserialize(tmp, node);
                result.Add(tmp);
            }

            return result;
        }
    }
}
