using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Xml;

namespace DotSerial.Core.XML
{
    public class XMLSerial_Serialize
    {
        public static void SerializePrimitive(object classObj, XmlDocument xmlDoc, XmlNode xnode, int tmpID = -1)
        {
            int objectID = tmpID;

            if (null == classObj)
            {
                AddParaXmlNode(xmlDoc, xnode, objectID, classObj);
                return;
            }

            Type typeObj = classObj.GetType();

            if (false == Misc.HelperMethods.IsPrimitive(typeObj))
            {
                throw new NotSupportedException();
            }

            if (typeObj == typeof(bool))
            {
                int tmp = Misc.HelperMethods.BoolToInt((bool)classObj);
                AddParaXmlNode(xmlDoc, xnode, objectID, tmp);
            }
            else
            {
                AddParaXmlNode(xmlDoc, xnode, objectID, classObj);
            }
        }

        /// <summary> Serialize object
        /// </summary>
        /// <param name="classObj">Object</param>
        /// <param name="xmlDoc">XmlDocument</param>
        /// <param name="xnode">XmlNode</param>
        public static void Serialize(object classObj, XmlDocument xmlDoc, XmlNode xnode, int tmpID = -1)
        {
            //ArgumentNullException.ThrowIfNull(classObj);

            if (classObj == null)
            {
                var xnodeNullEntry = xmlDoc.CreateElement(Constants.Object);
                CreateAttributes(xmlDoc, xnodeNullEntry, tmpID, "Todo");
                xnode.AppendChild(xnodeNullEntry);
                return;
            }

            Type typeObj = classObj.GetType();
            int objectID = Attributes.HelperMethods.GetClassID(typeObj);

            if (-1 == objectID)
            {
                objectID = tmpID;
            }

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
                        AddParaXmlNode(xmlDoc, xnodeEntry, id, value, propName);
                    }
                    else if (prop.PropertyType == typeof(string))
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
                    else if (prop.PropertyType.IsClass)
                    {
                        Serialize(value, xmlDoc, xnodeEntry, id);
                    }
                    else if (Misc.HelperMethods.IsPrimitive(prop.PropertyType))
                    {
                        AddParaXmlNode(xmlDoc, xnodeEntry, id, value, propName);
                    }
                    else
                    {
                        throw new NotImplementedException();
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
                if (Misc.HelperMethods.IsPrimitive(type))
                {
                    int id = 0;
                    foreach (var str in castedList)
                    {
                        // TODO SeiralizePrimitive
                        //AddParaXmlNode(xmlDoc, xnode, id, str);
                        SerializePrimitive(str, xmlDoc, xnode, id);
                        id++;
                    }
                }
                else if (Misc.HelperMethods.ImplementsIEnumerable(type))
                {

                    int id = 0;
                    foreach (var str in castedList)
                    {
                        var xnodeVersion = xmlDoc.CreateElement(Constants.List);
                        CreateAttributes(xmlDoc, xnodeVersion, id);
                        SerializeList(str, xmlDoc, xnodeVersion);

                        xnode.AppendChild(xnodeVersion);
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
        private static void AddParaXmlNode(XmlDocument xmlDoc, XmlNode xnode, int id, object? value, string? displayName = null)
        {
            ArgumentNullException.ThrowIfNull(xmlDoc);
            ArgumentNullException.ThrowIfNull(xnode);

            var xnodeParameter = xmlDoc.CreateElement(Constants.Parameter);

            if (value != null)
            {
                xnodeParameter.InnerText = value.GetType().IsEnum ? Convert.ToString((int)value) : value.ToString();
            }
            else
            {
                xnodeParameter.InnerText = Constants.NullString;
            }

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
