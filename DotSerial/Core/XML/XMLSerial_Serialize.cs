using System.Collections;
using System.Data.SqlTypes;
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
        /// <param name="objectID">ObjectID</param>
        public static void Serialize(object classObj, XmlDocument xmlDoc, XmlNode xnode, int objectID)
        {
            // If classObj is null, create empty node
            if (classObj == null)
            {
                var xnodeNullEntry = xmlDoc.CreateElement(Constants.Object);
                CreateAttributes(xmlDoc, xnodeNullEntry, objectID, "Todo");
                xnode.AppendChild(xnodeNullEntry);
                return;
            }

            Type typeObj = classObj.GetType();

            // Create Node
            var xnodeEntry = xmlDoc.CreateElement(Constants.Object);
            // Create Attributes
            CreateAttributes(xmlDoc, xnodeEntry, objectID, typeObj.Name);

            // Create datastructur to check if every id in a class is only
            // used once.
            Dictionary<int, string> dicIdName = [];

            // Get all Properties and iterate threw
            PropertyInfo[] props = typeObj.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                // Get ID attribute
                int id = Attributes.HelperMethods.GetPropertyID(prop);

                // Check if property got the attribute
                if (Constants.NoAttributeID != id)
                {
                    // Check if id was already used.
                    // If yes throw exception.
                    if (dicIdName.ContainsKey(id))
                    {
                        // TODO eigene Exception + Fehlermeldung
                        throw new NotSupportedException();
                    }

                    // Get Value of property
                    object? value = prop.GetValue(classObj);
                    // Get name of property
                    string propName = prop.Name;

                    // Add ID and prop name to datastrcuture.
                    dicIdName.Add(id, propName);

                    if (null == value)
                    {
                        // Null
                        AddParaXmlNode(xmlDoc, xnodeEntry, id, value, propName);
                    }
                    else if (prop.PropertyType == typeof(string))
                    {
                        // String
                        // Note: String case MUST become before IEnumerable
                        // otherwise it will not be serialized correct.
                        AddParaXmlNode(xmlDoc, xnodeEntry, id, value, propName);
                    }
                    else if (prop.PropertyType == typeof(bool))
                    {
                        // Bool
                        // Special case bool
                        // => casted to int.
                        int tmp = Misc.HelperMethods.BoolToInt((bool)value);
                        AddParaXmlNode(xmlDoc, xnodeEntry, id, tmp, propName);
                    }
                    else if (true == Misc.HelperMethods.ImplementsIEnumerable(value))
                    {
                        // IEnumarable
                        var xnodeVersion = xmlDoc.CreateElement(Constants.List);

                        CreateAttributes(xmlDoc, xnodeVersion, id, propName);
                        SerializeList(value, xmlDoc, xnodeVersion);

                        xnodeEntry.AppendChild(xnodeVersion);
                    }
                    else if (prop.PropertyType.IsClass)
                    {
                        // Class
                        Serialize(value, xmlDoc, xnodeEntry, id);
                    }
                    else if (prop.PropertyType.IsValueType && !prop.PropertyType.IsPrimitive && !prop.PropertyType.IsEnum && prop.PropertyType != typeof(decimal))
                    {
                        // Struct
                        Serialize(value, xmlDoc, xnodeEntry, id);
                    }
                    else if (Misc.HelperMethods.IsPrimitive(prop.PropertyType))
                    {
                        // Other supported primitive types
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
                Type type = value.GetType();
                if ( null == type)
                {
                    throw new NotSupportedException();
                }

                xnodeParameter.InnerText = type.IsEnum ? Convert.ToString((int)value) : value.ToString();
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

        /// <summary> Serialize Primitive type
        /// </summary>
        /// <param name="primObj">Primitive Object</param>
        /// <param name="xmlDoc">XmlDocument</param>
        /// <param name="xnode">XmlNode</param>
        /// <param name="primID">id</param>
        private static void SerializePrimitive(object primObj, XmlDocument xmlDoc, XmlNode xnode, int primID)
        {
            if (null == primObj)
            {
                // Null
                AddParaXmlNode(xmlDoc, xnode, primID, primObj);
                return;
            }

            // Get tyoe
            Type typeObj = primObj.GetType();

            // Check if object is a primitive
            if (false == Misc.HelperMethods.IsPrimitive(typeObj))
            {
                throw new NotSupportedException();
            }

            // Special case bool
            // => casted to int.
            if (typeObj == typeof(bool))
            {
                int tmp = Misc.HelperMethods.BoolToInt((bool)primObj);
                AddParaXmlNode(xmlDoc, xnode, primID, tmp);
            }
            else
            {
                AddParaXmlNode(xmlDoc, xnode, primID, primObj);
            }
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
                    // Primitive types

                    int id = 0;
                    foreach (var str in castedList)
                    {
                        SerializePrimitive(str, xmlDoc, xnode, id);
                        id++;
                    }
                }
                else if (Misc.HelperMethods.ImplementsIEnumerable(type))
                {
                    // IEnuramable

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
                    // Class

                    int id = 0;
                    foreach (var entry in castedList)
                    {
                        Serialize(entry, xmlDoc, xnode, id);
                        id++;
                    }
                }
                else if (type.IsValueType && !type.IsPrimitive && !type.IsEnum && type != typeof(decimal))
                {
                    // Struct

                    int id = 0;
                    foreach (var entry in castedList)
                    {
                        Serialize(entry, xmlDoc, xnode, id);
                        id++;
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
            else
            {
                throw new InvalidCastException();
            }
        }
    }
}
