using DotSerial.Core.Exceptions;
using DotSerial.Core.Misc;
using DotSerial.Core.XML;

using System.Collections;
using System.Reflection;

namespace DotSerial.Core.General
{
    internal class DSSerialize
    {
        /// <summary>
        /// Serialize object
        /// </summary>
        /// <param name="classObj">Object</param>
        /// <param name="objectID">Object-ID</param>
        /// <returns>DSNode</returns>
        internal static DSNode Serialize(object? classObj, int objectID)
        {
            // If classObj is null, create Null node
            if (classObj == null)
            {
                var nullNode = new DSNode(objectID, null, "Class", DSNodeType.Leaf, DSNodePropertyType.Class);
                return nullNode;
            }

            Type typeObj = classObj.GetType();

            // Create node
            var node = new DSNode(objectID, typeObj.Name, DSNodeType.InnerNode, DSNodePropertyType.Class);

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
                if (-1 != id)
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

                    if (TypeCheckMethods.IsPrimitive(prop.PropertyType))
                    {
                        // Primitive types || String
                        DSNode childNode = new(id, value, propName, DSNodeType.Leaf, DSNodePropertyType.Primitive);
                        node.AppendChild(id, childNode);
                    }
                    else if (TypeCheckMethods.IsDictionary(prop.PropertyType))
                    {
                        // Dictionary
                        var dicNode = SerializeDictionary(value, id);
                        node.AppendChild(id, dicNode);

                    }
                    else if (TypeCheckMethods.IsList(prop.PropertyType) ||
                             TypeCheckMethods.IsArray(prop.PropertyType))
                    {
                        // List || Array
                        var dicNode = SerializeList(value, id);
                        node.AppendChild(id, dicNode);
                    }
                    else if (TypeCheckMethods.IsClass(prop.PropertyType) || TypeCheckMethods.IsStruct(prop.PropertyType))
                    {
                        // Class || Struct
                        var dicNode = Serialize(value, id);
                        node.AppendChild(id, dicNode);
                    }
                    else if (value == null)
                    {
                        // Null
                        DSNode nullNode = new(id, null, "Null", DSNodeType.InnerNode, DSNodePropertyType.Class);
                        node.AppendChild(id, nullNode);
                    }
                    else
                    {
                        throw new DSNotSupportedTypeException(prop.PropertyType);
                    }
                }
            }

            return node;
        }

        /// <summary>
        /// Serialize dictionary
        /// </summary>
        /// <param name="dic">Dictioanry</param>
        /// <param name="id">Object-ID</param>
        /// <returns>DSNode</returns>
        private static DSNode SerializeDictionary(object? dic, int id)
        {
            if (dic == null)
            {
                // Null
                DSNode nullNode = new(id, null, "Dictioanry", DSNodeType.Leaf, DSNodePropertyType.Dictionary);
                return nullNode;
            }

            var node = new DSNode(id, "Dictioanry", DSNodeType.InnerNode, DSNodePropertyType.Dictionary);

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

                    int dicID = 0;
                    foreach (DictionaryEntry keyValuePair in castedDic)
                    {
                        var key = keyValuePair.Key;
                        var value = keyValuePair.Value;
                        DSNode keyValuepair = new(dicID, "KeyVlauePair", DSNodeType.InnerNode, DSNodePropertyType.KeyValuePair);
                        DSNode keyNode;
                        DSNode keyValue;

                        #region Key

                        // Key
                        if (null == key)
                        {
                            throw new NullReferenceException();
                        }

                        if (TypeCheckMethods.IsPrimitive(keyType))
                        {
                            keyNode = new (0, key, "Key", DSNodeType.Leaf, DSNodePropertyType.KeyValuePairKey);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                        #endregion

                        #region Value

                        if (TypeCheckMethods.IsPrimitive(valueType))
                        {
                            keyValue = new (1, value, "Value", DSNodeType.Leaf, DSNodePropertyType.KeyValuePairValue);
                        }
                        else if (TypeCheckMethods.IsDictionary(value))
                        {
                            // Dictionary
                            if (value is IEnumerable castedValue)
                            {
                                keyValue = new(1, value, "Value", DSNodeType.InnerNode, DSNodePropertyType.KeyValuePairValue);
                                int innerDicID = 0;
                                foreach (var str in castedValue)
                                {
                                    var listNode = SerializeDictionary(str, innerDicID);
                                    keyValue.AppendChild(innerDicID, listNode);
                                    innerDicID++;
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

                            if (value is IEnumerable castedValue)
                            {
                                keyValue = new(1, value, "Value", DSNodeType.InnerNode, DSNodePropertyType.KeyValuePairValue);
                                int listID = 0;
                                foreach (var str in castedValue)
                                {
                                    var listNode = SerializeList(str, listID);
                                    keyValue.AppendChild(listID, listNode);
                                    listID++;
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
                            keyValue = new(1, "Value", DSNodeType.InnerNode, DSNodePropertyType.KeyValuePairValue);
                            var classNode = Serialize(value, 0);
                            keyValue.AppendChild(0, classNode);
                        }
                        else
                        {
                            throw new DSNotSupportedTypeException(valueType);
                        }

                        #endregion

                        keyValuepair.AppendChild(0, keyNode);
                        keyValuepair.AppendChild(1, keyValue);
                        node.AppendChild(dicID, keyValuepair);
                        dicID++;
                    }

                    return node;
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
        /// <param name="id">Object-ID</param>
        /// <returns>DSNode</returns>
        private static DSNode SerializeList(object list, int id)
        {
            if (list == null)
            {
                // Null
                DSNode nullNode = new(id, null, "List", DSNodeType.Leaf, DSNodePropertyType.List);
                return nullNode;
            }

            // Create node
            var node = new DSNode(id, "List", DSNodeType.InnerNode, DSNodePropertyType.List);

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
                    int listID = 0;
                    foreach (var str in castedList)
                    {
                        DSNode childNode = new(listID, str, string.Empty, DSNodeType.Leaf, DSNodePropertyType.Primitive);
                        node.AppendChild(listID, childNode);
                        listID++;
                    }
                }
                else if (TypeCheckMethods.IsList(type) ||
                         TypeCheckMethods.IsArray(type))
                {
                    // List || Array
                    int listID = 0;
                    foreach (var str in castedList)
                    {
                        var listNode = SerializeList(str, listID);
                        node.AppendChild(listID, listNode);
                        listID++;
                    }
                }
                else if (TypeCheckMethods.IsDictionary(type))
                {
                    // Dictionary
                    int listID = 0;
                    foreach (var str in castedList)
                    {
                        var listNode = SerializeDictionary(str, listID);
                        node.AppendChild(listID, listNode);
                        listID++;
                    }
                }
                else if (TypeCheckMethods.IsClass(type) ||
                         TypeCheckMethods.IsStruct(type))
                {
                    // Class || Struct
                    int listID = 0;
                    foreach (var entry in castedList)
                    {
                        var classNode = Serialize(entry, listID);
                        node.AppendChild(listID, classNode);
                        listID++;
                    }
                }
                else
                {
                    throw new DSNotSupportedTypeException(type);
                }

                return node;
            }
            else
            {
                throw new InvalidCastException();
            }
        }
    }
}
