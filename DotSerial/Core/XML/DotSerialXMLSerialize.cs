using System.Collections;
using System.Reflection;
using System.Xml;

using DotSerial.Core.Misc;
using DotSerial.Core.Exceptions;

namespace DotSerial.Core.XML
{
    internal class DotSerialXMLSerialize
    {
        /// <summary> 
        /// Serialize object
        /// </summary>
        /// <param name="classObj">Object</param>
        /// <param name="xmlDoc">XmlDocument</param>
        /// <param name="xnode">XmlNode</param>
        /// <param name="objectID">ObjectID</param>
        internal static void Serialize(object? classObj, XmlDocument xmlDoc, XmlNode xnode, int objectID)
        {
            // If classObj is null, create empty node
            if (classObj == null)
            {
                var xnodeNullEntry = xmlDoc.CreateElement(Constants.Object);
                CreateAttributes(xmlDoc, xnodeNullEntry, objectID, Constants.Null);
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
                    // Check if type is supported
                    if (false == DotSerialXML.IsTypeSupported(prop.PropertyType))
                    {
                        throw new DSNotSupportedTypeException(prop.PropertyType);
                    }

                    // Check if id was already used.
                    // If yes throw exception.
                    if (dicIdName.ContainsKey(id))
                    {
                        throw new DSDuplicateIDException(id);
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
                    else if (TypeCheckMethods.IsDictionary(value))
                    {
                        // Dictionary
                        var xnodeVersion = xmlDoc.CreateElement(Constants.Dictionary);

                        CreateAttributes(xmlDoc, xnodeVersion, id, propName);
                        SerializeDictionary(value, xmlDoc, xnodeVersion);

                        xnodeEntry.AppendChild(xnodeVersion);
                    }
                    else if (TypeCheckMethods.IsList(value) ||
                             TypeCheckMethods.IsArray(value))
                    {
                        // List || Array
                        var xnodeVersion = xmlDoc.CreateElement(Constants.List);

                        CreateAttributes(xmlDoc, xnodeVersion, id, propName);
                        SerializeList(value, xmlDoc, xnodeVersion);

                        xnodeEntry.AppendChild(xnodeVersion);
                    }
                    else if (TypeCheckMethods.IsClass(prop.PropertyType) || TypeCheckMethods.IsStruct(prop.PropertyType))
                    {
                        // Class || Struct
                        Serialize(value, xmlDoc, xnodeEntry, id);
                    }
                    else if (TypeCheckMethods.IsPrimitive(prop.PropertyType))
                    {
                        // Primitive types || String
                        SerializePrimitive(value, xmlDoc, xnodeEntry, id, propName);
                    }
                    else
                    {
                        throw new DSNotSupportedTypeException(prop.PropertyType);
                    }
                }
            }

            xnode.AppendChild(xnodeEntry);
        }

        /// <summary> Serialize Primitive type
        /// </summary>
        /// <param name="primObj">Primitive Object</param>
        /// <param name="xmlDoc">XmlDocument</param>
        /// <param name="xnode">XmlNode</param>
        /// <param name="primID">id</param>
        private static void SerializePrimitive(object? primObj, XmlDocument xmlDoc, XmlNode xnode, int primID, string? displayName = null)
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
            if (false == TypeCheckMethods.IsPrimitive(typeObj))
            {
                throw new DSNotSupportedTypeException(typeObj);
            }

            // Special case bool
            // => casted to int.
            if (typeObj == typeof(bool))
            {
                int tmp = HelperMethods.BoolToInt((bool)primObj);
                AddParaXmlNode(xmlDoc, xnode, primID, tmp, displayName);
            }
            else
            {
                AddParaXmlNode(xmlDoc, xnode, primID, primObj, displayName);
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
                if (GetTypeMethods.GetKeyValueTypeOfDictionary(dic, out Type keyType, out Type valueType))
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
                            throw new NullReferenceException();
                        }

                        var xnodeKey = xmlDoc.CreateElement(Constants.Key);
                        CreateAttributes(xmlDoc, xnodeKey, Constants.DicKeyID);

                        if (TypeCheckMethods.IsPrimitive(keyType))
                        {
                            SerializePrimitive(key, xmlDoc, xnodeKey, 0);
                        }
                        else if (TypeCheckMethods.IsDictionary(key))
                        {
                            // Dictionary

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
                                throw new InvalidCastException();
                            }
                        }
                        else if (TypeCheckMethods.IsList(key) ||
                                 TypeCheckMethods.IsArray(key))
                        {
                            // List || Array

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
                                throw new InvalidCastException();
                            }
                        }
                        else if (TypeCheckMethods.IsClass(keyType) ||
                                 TypeCheckMethods.IsStruct(keyType))
                        {
                            // Class || Struct
                            Serialize(key, xmlDoc, xnodeKey, 0);
                        }
                        else
                        {
                            throw new DSNotSupportedTypeException(keyType);
                        }

                        #endregion

                        #region Value

                        // Value
                        var xnodeValue = xmlDoc.CreateElement(Constants.Value);
                        CreateAttributes(xmlDoc, xnodeValue, Constants.DicKeyValueID);

                        if (TypeCheckMethods.IsPrimitive(valueType))
                        {
                            SerializePrimitive(value, xmlDoc, xnodeValue, 0);
                        }
                        else if (TypeCheckMethods.IsDictionary(value))
                        {
                            // Dictionary

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
                                throw new InvalidCastException();
                            }

                        }
                        else if (TypeCheckMethods.IsList(value) ||
                                 TypeCheckMethods.IsArray(value))
                        {
                            // List || Array

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
                                throw new InvalidCastException();
                            }
                        }
                        else if (TypeCheckMethods.IsClass(valueType) ||
                                 TypeCheckMethods.IsStruct(valueType))
                        {
                            // Class || Struct
                            Serialize(value, xmlDoc, xnodeValue, 0);
                        }
                        else
                        {
                            throw new DSNotSupportedTypeException(valueType);
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
                    throw new TypeAccessException();
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
                Type type = GetTypeMethods.GetItemTypeOfIEnumerable(castedList);
                // Check if type is supported
                if (false == DotSerialXML.IsTypeSupported(type))
                {
                    throw new DSNotSupportedTypeException(type);
                }

                if (TypeCheckMethods.IsPrimitive(type))
                {
                    // Primitive types

                    int id = 0;
                    foreach (var str in castedList)
                    {
                        SerializePrimitive(str, xmlDoc, xnode, id);
                        id++;
                    }
                }
                else if (TypeCheckMethods.IsDictionary(type))
                {
                    // Dictionary

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
                else if (TypeCheckMethods.IsList(type) ||
                         TypeCheckMethods.IsArray(type))
                {
                    // List || Array

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
                else if (TypeCheckMethods.IsClass(type) ||
                         TypeCheckMethods.IsStruct(type))
                {
                    // Class || Struct

                    int id = 0;
                    foreach (var entry in castedList)
                    {
                        Serialize(entry, xmlDoc, xnode, id);
                        id++;
                    }
                }
                else
                {
                    throw new DSNotSupportedTypeException(type);
                }
            }
            else
            {
                throw new InvalidCastException();
            }
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

            var xnodeParameter = xmlDoc.CreateElement(Constants.Property);

            if (value != null)
            {
                Type type = value.GetType();
                if (null == type)
                {
                    throw new NullReferenceException();
                }
#pragma warning disable CS8601
                xnodeParameter.InnerText = type.IsEnum ? Convert.ToString((int)value) : value.ToString();
#pragma warning restore CS8601
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
            DotSerialXML.CreateAttribute(xmlDoc, xmlElement, Constants.XmlAttributeID, id.ToString());

            // Display
            if (false == string.IsNullOrWhiteSpace(displayName))
            {
                DotSerialXML.CreateAttribute(xmlDoc, xmlElement, Constants.XmlAttributeName, displayName);
            }
        }
    }
}
