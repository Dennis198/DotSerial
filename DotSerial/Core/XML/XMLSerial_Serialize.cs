using System.Collections;
using System.ComponentModel.Design;
using System.Reflection;
using System.Xml;

namespace DotSerial.Core.XML
{
    public class XMLSerial_Serialize
    {
        /// <summary> 
        /// Serialize object
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
                    else if (prop.PropertyType == typeof(bool))
                    {
                        // Bool
                        // Special case bool
                        // => casted to int.
                        int tmp = Misc.HelperMethods.BoolToInt((bool)value);
                        AddParaXmlNode(xmlDoc, xnodeEntry, id, tmp, propName);
                    }
                    if (Misc.HelperMethods.IsDictionary(value))
                    {
                        // Dictionary
                        var xnodeVersion = xmlDoc.CreateElement(Constants.Dictionary);

                        CreateAttributes(xmlDoc, xnodeVersion, id, propName);
                        SerializeDictionary(value, xmlDoc, xnodeVersion);

                        xnodeEntry.AppendChild(xnodeVersion);
                    }
                    else if (Misc.HelperMethods.IsList(value) || Misc.HelperMethods.IsArray(value))
                    {
                        // List/Array
                        var xnodeVersion = xmlDoc.CreateElement(Constants.List);

                        CreateAttributes(xmlDoc, xnodeVersion, id, propName);
                        SerializeList(value, xmlDoc, xnodeVersion);

                        xnodeEntry.AppendChild(xnodeVersion);
                    }
                    else if (Misc.HelperMethods.IsClass(prop.PropertyType))
                    {
                        // Class
                        Serialize(value, xmlDoc, xnodeEntry, id);
                    }
                    else if (Misc.HelperMethods.IsStruct(prop.PropertyType))
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

        /// <summary>
        /// Adds an Parameter Block to an Xml node
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

        /// <summary>
        /// Serialite dictionary
        /// </summary>
        /// <param name="dic">Dictioanry</param>
        /// <param name="xmlDoc">XmlDocument</param>
        /// <param name="xnode">XmlNode</param>
        private static void SerializeDictionary(object dic, XmlDocument xmlDoc, XmlNode xnode)
        {
            ArgumentNullException.ThrowIfNull(dic);

            if (dic is IDictionary castedDic)
            {
                if (Misc.HelperMethods.GetKeyValueTypeOfDictionary(dic, out Type keyType, out Type valueType))
                {
                    int id = 0;
                    foreach (DictionaryEntry keyValuePair in castedDic)
                    {
                        var key = keyValuePair.Key;
                        var value = keyValuePair.Value;

                        var xnodeKeyValuePair = xmlDoc.CreateElement(Constants.KeyValuePair);
                        CreateAttributes(xmlDoc, xnodeKeyValuePair, id);

                        #region Key

                        // Key
                        if (null == key)
                        {
                            throw new NotSupportedException();
                        }

                        var xnodeKey = xmlDoc.CreateElement(Constants.Key);
                        CreateAttributes(xmlDoc, xnodeKey, 0);

                        if (Misc.HelperMethods.IsPrimitive(keyType))
                        {
                            SerializePrimitive(key, xmlDoc, xnodeKey, 0);
                        }
                        else if (Misc.HelperMethods.IsDictionary(key))
                        {
                            if (key is IEnumerable castedKey)
                            {
                                int idInner = 0;
                                foreach (var str in castedKey)
                                {
                                    var xnodeVersionInner = xmlDoc.CreateElement(Constants.Dictionary);
                                    CreateAttributes(xmlDoc, xnodeVersionInner, idInner);
                                    SerializeDictionary(str, xmlDoc, xnodeVersionInner);

                                    xnodeKey.AppendChild(xnodeVersionInner);
                                    idInner++;
                                }
                            }
                            else
                            {
                                throw new NotSupportedException();
                            }
                        }
                        else if (Misc.HelperMethods.IsList(key) || Misc.HelperMethods.IsArray(key))
                        {
                            if (key is IEnumerable castedKey)
                            {
                                int idInner = 0;
                                foreach (var str in castedKey)
                                {
                                    var xnodeVersionInner = xmlDoc.CreateElement(Constants.List);
                                    CreateAttributes(xmlDoc, xnodeVersionInner, idInner);
                                    SerializeList(str, xmlDoc, xnodeVersionInner);

                                    xnodeKey.AppendChild(xnodeVersionInner);
                                    idInner++;
                                }
                            }
                            else
                            {
                                throw new NotSupportedException();
                            }
                        }
                        else if (Misc.HelperMethods.IsClass(keyType))
                        {
                            // Class

                            Serialize(key, xmlDoc, xnodeKey, 0);
                        }
                        else if (Misc.HelperMethods.IsStruct(keyType))
                        {
                            // Struct

                            Serialize(key, xmlDoc, xnodeKey, 0);
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }

                        #endregion

                        #region Value

                        // Value
                        var xnodeValue = xmlDoc.CreateElement(Constants.Value);
                        CreateAttributes(xmlDoc, xnodeValue, 1);

                        if (Misc.HelperMethods.IsPrimitive(valueType))
                        {
                            SerializePrimitive(value, xmlDoc, xnodeValue, 0);
                        }
                        else if (Misc.HelperMethods.IsDictionary(value))
                        {
                            if (value is IEnumerable castedKey)
                            {
                                int idInner = 0;
                                foreach (var str in castedKey)
                                {
                                    var xnodeVersionInner = xmlDoc.CreateElement(Constants.Dictionary);
                                    CreateAttributes(xmlDoc, xnodeVersionInner, idInner);
                                    SerializeDictionary(str, xmlDoc, xnodeVersionInner);

                                    xnodeValue.AppendChild(xnodeVersionInner);
                                    idInner++;
                                }
                            }
                            else
                            {
                                throw new NotSupportedException();
                            }

                        }
                        else if (Misc.HelperMethods.IsList(value) || Misc.HelperMethods.IsArray(value))
                        {
                            if (value is IEnumerable castedKey)
                            {
                                int idInner = 0;
                                foreach (var str in castedKey)
                                {
                                    var xnodeVersionInner = xmlDoc.CreateElement(Constants.List);
                                    CreateAttributes(xmlDoc, xnodeVersionInner, idInner);
                                    SerializeList(str, xmlDoc, xnodeVersionInner);

                                    xnodeValue.AppendChild(xnodeVersionInner);
                                    idInner++;
                                }
                            }
                            else
                            {
                                throw new NotSupportedException();
                            }
                        }
                        else if (Misc.HelperMethods.IsClass(valueType))
                        {
                            // Class

                            Serialize(value, xmlDoc, xnodeValue, 0);
                        }
                        else if (Misc.HelperMethods.IsStruct(valueType))
                        {
                            // Struct

                            Serialize(value, xmlDoc, xnodeValue, 0);
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }

                        #endregion

                        // Add KeyValuePair
                        xnodeKeyValuePair.AppendChild(xnodeKey);
                        xnodeKeyValuePair.AppendChild(xnodeValue);
                        xnode.AppendChild(xnodeKeyValuePair);
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

        /// <summary> 
        /// Serialize a list.
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
                else if (Misc.HelperMethods.IsDictionary(type))
                {
                    int id = 0;
                    foreach (var str in castedList)
                    {
                        var xnodeVersion = xmlDoc.CreateElement(Constants.Dictionary);
                        CreateAttributes(xmlDoc, xnodeVersion, id);
                        SerializeList(str, xmlDoc, xnodeVersion);

                        xnode.AppendChild(xnodeVersion);
                        id++;
                    }
                }
                else if (Misc.HelperMethods.IsList(type) || Misc.HelperMethods.IsArray(type))
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
                else if (Misc.HelperMethods.IsClass(type))
                {
                    // Class

                    int id = 0;
                    foreach (var entry in castedList)
                    {
                        Serialize(entry, xmlDoc, xnode, id);
                        id++;
                    }
                }
                else if (Misc.HelperMethods.IsStruct(type))
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
