using System.Collections;
using System.Reflection;
using System.Xml;

namespace DotSerial.Core.XML
{
    public class XMLSerial_Serialize
    {
        /// <summary> Serialize object
        /// </summary>
        /// <param name="classObj">Object</param>
        /// <param name="xmlDoc">XmlDocument</param>
        /// <param name="xnode">XmlNode</param>
        public static void Serialize(object classObj, XmlDocument xmlDoc, XmlNode xnode)
        {
            ArgumentNullException.ThrowIfNull(classObj);

            Type typeObj = classObj.GetType();
            int objectID = Attributes.HelperMethods.GetClassID(typeObj);

            var xnodeEntry = xmlDoc.CreateElement(Constants.Object);

            CreateAttributes(xmlDoc, xnodeEntry, objectID, typeObj.Name);

            PropertyInfo[] props = typeObj.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                int id = Attributes.HelperMethods.GetPropertyID(prop);

                if (-1 != id)
                {
                    object? value = prop.GetValue(classObj);
                    string propName = prop.Name;

                    if (null == value)
                    {
                        throw new NotImplementedException();
                    }

                    if (prop.PropertyType == typeof(string))
                    {
                        AddParaXmlNode(xmlDoc, xnodeEntry, id, value, propName);
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        int tmp = Misc.HelperMethods.BoolToInt((bool)value);
                        AddParaXmlNode(xmlDoc, xnodeEntry, id, tmp, propName);
                    }
                    else if (true == Misc.HelperMethods.ImplementsIEnumerable(value))
                    {
                        var xnodeVersion = xmlDoc.CreateElement(Constants.List);

                        CreateAttributes(xmlDoc, xnodeVersion, id, propName);
                        SerializeList(value, xmlDoc, xnodeVersion);

                        xnodeEntry.AppendChild(xnodeVersion);
                    }
                    else
                    {
                        AddParaXmlNode(xmlDoc, xnodeEntry, id, value, propName);
                    }

                }
            }

            xnode.AppendChild(xnodeEntry);
        }

        /// <summary> Serialize a list.
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="xmlDoc">XmlDocument</param>
        /// <param name="xnode">XmlNode</param>
        private static void SerializeList(object list, XmlDocument xmlDoc, XmlNode xnode)
        {
            ArgumentNullException.ThrowIfNull(list);

            if (list is IEnumerable castedList)
            {
                Type type = Misc.HelperMethods.GetItemTypeOfIEnumerable(castedList);
                if (type == typeof(string) || type == typeof(int))
                {
                    int id = 0;
                    foreach (var str in castedList)
                    {
                        AddParaXmlNode(xmlDoc, xnode, id, str);
                        id++;
                    }
                }
                else if (true == type.IsClass)
                {
                    foreach (var entry in castedList)
                    {
                        Serialize(entry, xmlDoc, xnode);
                    }
                }
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        /// <summary> Adds an Parameter Block to an Xml node
        /// </summary>
        /// <param name="xmlDoc">XmlDocument</param>
        /// <param name="xnode">XmlNode</param>
        /// <param name="id">Parameter id</param>
        /// <param name="value">value</param>
        /// <param name="displayName">display</param>
        private static void AddParaXmlNode(XmlDocument xmlDoc, XmlNode xnode, int id, object value, string? displayName = null)
        {
            ArgumentNullException.ThrowIfNull(value);

            var xnodeParameter = xmlDoc.CreateElement(Constants.Parameter);
            xnodeParameter.InnerText = value.GetType().IsEnum ? Convert.ToString((int)value) : value.ToString();

            CreateAttributes(xmlDoc, xnodeParameter, id, displayName);

            xnode.AppendChild(xnodeParameter);
        }

        /// <summary> Creates the attributes id and display (optional)
        /// </summary>
        /// <param name="xmlDoc">XmlDocument</param>
        /// <param name="xmlElement">XmlElement</param>
        /// <param name="id">id</param>
        /// <param name="displayName">display name</param>
        private static void CreateAttributes(XmlDocument xmlDoc, XmlElement xmlElement, int id, string? displayName = null)
        {
            // ID
            var xnodeParaID = xmlDoc.CreateAttribute(Constants.IdAttribute);
            xnodeParaID.InnerText = id.ToString();
            xmlElement.Attributes?.Append(xnodeParaID);

            // Display
            if (false == string.IsNullOrWhiteSpace(displayName))
            {
                var xnodeParaDisplay = xmlDoc.CreateAttribute(Constants.DisplayAttribute);
                xnodeParaDisplay.InnerText = displayName;
                xmlElement.Attributes?.Append(xnodeParaDisplay);
            }
        }
    }
}
