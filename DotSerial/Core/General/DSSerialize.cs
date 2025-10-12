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
        internal static DSNode Serialize(object? classObj, string objectID)
        {
            ///      (node) (Class)
            ///        |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (Properties)

            // If classObj is null, create Null node
            if (classObj == null)
            {
                var nullNode = new DSNode(objectID, null, DSNodeType.Leaf, DSNodePropertyType.Null);
                return nullNode;
            }

            Type typeObj = classObj.GetType();

            // Create node
            var node = new DSNode(objectID, DSNodeType.InnerNode, DSNodePropertyType.Class);

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

                    string idString = id.ToString();

                    if (TypeCheckMethods.IsPrimitive(prop.PropertyType))
                    {
                        // Primitive types || String
                        DSNode childNode = new(idString, value, DSNodeType.Leaf, DSNodePropertyType.Primitive);
                        node.AppendChild(childNode);
                    }
                    else if (TypeCheckMethods.IsDictionary(prop.PropertyType))
                    {
                        // Dictionary
                        var dicNode = SerializeDictionary(value, idString);
                        node.AppendChild(dicNode);

                    }
                    else if (TypeCheckMethods.IsList(prop.PropertyType) ||
                             TypeCheckMethods.IsArray(prop.PropertyType))
                    {
                        // List || Array
                        var dicNode = SerializeList(value, idString);
                        node.AppendChild(dicNode);
                    }
                    else if (TypeCheckMethods.IsClass(prop.PropertyType) || TypeCheckMethods.IsStruct(prop.PropertyType))
                    {
                        // Class || Struct
                        var dicNode = Serialize(value, idString);
                        node.AppendChild(dicNode);
                    }
                    else if (value == null)
                    {
                        // Null
                        DSNode nullNode = new(idString, null, DSNodeType.Leaf, DSNodePropertyType.Null);
                        node.AppendChild(nullNode);
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
        private static DSNode SerializeDictionary(object? dic, string id)
        {
            ///      (node) (Dictionary)
            ///        |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (KeyValuePairs)
            ///  :     |     :
            ///  :    (D)    :  (Value of KeyvaluePairs)

            if (dic == null)
            {
                // Null
                DSNode nullNode = new(id, null, DSNodeType.Leaf, DSNodePropertyType.Null);
                return nullNode;
            }

            var node = new DSNode(id, DSNodeType.InnerNode, DSNodePropertyType.Dictionary);

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

                    foreach (DictionaryEntry keyValuePair in castedDic)
                    {
                        var key = keyValuePair.Key;
                        var value = keyValuePair.Value;
                        DSNode keyValuepair;
                        DSNode keyValue;

                        #region Key

                        string? keyString = key.ToString();
                        if (string.IsNullOrWhiteSpace(keyString))
                        {
                            throw new NotImplementedException();
                        }

                        // Key
                        if (null == key)
                        {
                            throw new NullReferenceException();
                        }

                        if (TypeCheckMethods.IsPrimitive(keyType))
                        {
                            keyValuepair = new(keyString, DSNodeType.InnerNode, DSNodePropertyType.KeyValuePair);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                        #endregion

                        #region Value

                        if (TypeCheckMethods.IsPrimitive(valueType))
                        {
                            keyValue = new (keyString, value, DSNodeType.Leaf, DSNodePropertyType.KeyValuePairValue);
                        }
                        else if (TypeCheckMethods.IsDictionary(value))
                        {
                            // Dictionary
                            if (value is IEnumerable castedValue)
                            {
                                keyValue = new(keyString, value, DSNodeType.InnerNode, DSNodePropertyType.KeyValuePairValue);
                                int innerDicID = 0;
                                foreach (var str in castedValue)
                                {
                                    string innerDicIDString = innerDicID.ToString();
                                    var listNode = SerializeDictionary(str, innerDicIDString);
                                    keyValue.AppendChild(listNode);
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
                                keyValue = new(keyString, value, DSNodeType.InnerNode, DSNodePropertyType.KeyValuePairValue);
                                int listID = 0;
                                foreach (var str in castedValue)
                                {
                                    string listIDString = listID.ToString();
                                    var listNode = SerializeList(str, listIDString);
                                    keyValue.AppendChild(listNode);
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
                            keyValue = new(keyValuepair.Key, DSNodeType.InnerNode, DSNodePropertyType.KeyValuePairValue);
                            var classNode = Serialize(value, keyString);
                            keyValue.AppendChild(classNode);
                        }
                        else
                        {
                            throw new DSNotSupportedTypeException(valueType);
                        }

                        #endregion

                        keyValuepair.AppendChild(keyValue);
                        node.AppendChild(keyValuepair);
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
        private static DSNode SerializeList(object? list, string id)
        {
            ///      (node) (List)
            ///        |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (Items)
            
            if (list == null)
            {
                // Null
                DSNode nullNode = new(id, null, DSNodeType.Leaf, DSNodePropertyType.Null);
                return nullNode;
            }

            // Create node
            var node = new DSNode(id, DSNodeType.InnerNode, DSNodePropertyType.List);

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
                        string listIDString = listID.ToString();
                        DSNode childNode = new(listIDString, str, DSNodeType.Leaf, DSNodePropertyType.Primitive);
                        node.AppendChild(childNode);
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
                        string listIDString = listID.ToString();
                        var listNode = SerializeList(str, listIDString);
                        node.AppendChild(listNode);
                        listID++;
                    }
                }
                else if (TypeCheckMethods.IsDictionary(type))
                {
                    // Dictionary
                    int listID = 0;
                    foreach (var str in castedList)
                    {
                        string listIDString = listID.ToString();
                        var listNode = SerializeDictionary(str, listIDString);
                        node.AppendChild(listNode);
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
                        string listIDString = listID.ToString();
                        var classNode = Serialize(entry, listIDString);
                        node.AppendChild(classNode);
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
