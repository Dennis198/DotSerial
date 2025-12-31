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
using DotSerial.Core.Tree;
using DotSerial.Core.Tree.Nodes;
using DotSerial.Core.XML;

using System.Collections;
using System.Reflection;

namespace DotSerial.Core.General
{
    internal class DSSerialize
    {
        /// <summary>
        /// Node factory;
        /// </summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <summary>
        /// Serialize object
        /// </summary>
        /// <param name="classObj">Object</param>
        /// <param name="objectID">Object-ID</param>
        /// <returns>DSNode</returns>
        internal static IDSNode Serialize2(object? classObj, string objectID)
        {
            ///      (node) (Class)
            ///        |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (Properties)        
            
            // If classObj is null, create Null node
            if (classObj == null)
            {
                return _nodeFactory.CreateNode(objectID, null, NodeType.Leaf);
            }

            // Create node
            var result = _nodeFactory.CreateNode(objectID, null, NodeType.InnerNode);

            Type typeObj = classObj.GetType();

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

                    if (TypeCheckMethods.IsPrimitive(prop.PropertyType) || TypeCheckMethods.IsSpecialParsableObject(prop.PropertyType))
                    {
                        // Primitive types || String
                        string? strValue = HelperMethods.PrimitiveToString(value);
                        var childNode = _nodeFactory.CreateNode(idString, strValue, NodeType.Leaf);
                        result.AddChild(childNode);
                    }
                    else if (TypeCheckMethods.IsDictionary(prop.PropertyType))
                    {
                        // Dictionary
                        var childNode = SerializeDictionary2(value, idString);
                        result.AddChild(childNode);

                    }
                    else if (TypeCheckMethods.IsList(prop.PropertyType) ||
                             TypeCheckMethods.IsArray(prop.PropertyType))
                    {
                        // List || Array
                        var childNode = SerializeList2(value, idString);
                        result.AddChild(childNode);
                    }
                    else if (TypeCheckMethods.IsClass(prop.PropertyType) || TypeCheckMethods.IsStruct(prop.PropertyType))
                    {
                        // Class || Struct
                        var childNode = Serialize2(value, idString);
                        result.AddChild(childNode);
                    }
                    else if (value == null)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        throw new DSNotSupportedTypeException(prop.PropertyType);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Serialize dictionary
        /// </summary>
        /// <param name="dic">Dictioanry</param>
        /// <param name="id">Object-ID</param>
        /// <returns>DSNode</returns>
        private static IDSNode SerializeDictionary2(object? dic, string id)
        {
            ///      (node) (Dictionary)
            ///        |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (KeyValuePairs)
            ///  :     |     :
            ///  :    (D)    :  (Value of KeyvaluePairs)            
            
            // If classObj is list, create Null node
            if (dic == null)
            {
                return _nodeFactory.CreateNode(id, null, NodeType.Leaf);
            }

            // Create node
            var result = _nodeFactory.CreateNode(id, null, NodeType.DictionaryNode);            

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
                        // IDSNode keyValuepair;
                        IDSNode keyValue;
                        string? keyString = null;
                        #region Key

                        // Key
                        if (null == key)
                        {
                            throw new NullReferenceException();
                        }

                        if (TypeCheckMethods.IsPrimitive(keyType) ||
                            TypeCheckMethods.IsSpecialParsableObject(keyType))
                        {
                            keyString = HelperMethods.PrimitiveToString(key);
                            // string? tmpKey = HelperMethods.PrimitiveToString(key);
                            // if (tmpKey == null)
                            // {
                            //     throw new ArgumentException(tmpKey);
                            // }
                            // keyValuepair = _nodeFactory.CreateNode(tmpKey, null, NodeType.InnerNode);
                        }
                        else
                        {
                            throw new DSNotSupportedTypeException(valueType);
                        }

                        if (null == keyString)
                        {
                            throw new ArgumentException(keyString);
                        }

                        // string? keyString = keyValuepair.Key;

                        #endregion

                        #region Value

                        if (null == value)
                        {
                            keyValue = _nodeFactory.CreateNode(keyString, null, NodeType.Leaf);
                        }
                        else if (TypeCheckMethods.IsPrimitive(valueType) ||
                            TypeCheckMethods.IsSpecialParsableObject(valueType))
                        {
                            string? tmpValue = HelperMethods.PrimitiveToString(value);
                            keyValue = _nodeFactory.CreateNode(keyString, tmpValue, NodeType.Leaf);
                        }
                        else if (TypeCheckMethods.IsDictionary(value))
                        {
                            // Dictionary
                            if (value is IDictionary castedValue)
                            {
                                if (GetTypeMethods.GetKeyValueTypeOfDictionary(dic, out Type innerKeyType, out Type _))
                                {
                                    if (!TypeCheckMethods.IsPrimitive(keyType) &&
                                        !TypeCheckMethods.IsSpecialParsableObject(keyType))
                                    {
                                        throw new DSNotSupportedTypeException(valueType);
                                    }

                                    keyValue = _nodeFactory.CreateNode(keyString, null, NodeType.DictionaryNode);
                                    foreach (DictionaryEntry str in castedValue)
                                    {
                                        string? innerDicID = HelperMethods.PrimitiveToString(str.Key);
                                        if (innerDicID == null)
                                        {
                                            throw new ArgumentException(innerDicID);
                                        }
                                        var childNode = SerializeDictionary2(str, innerDicID);
                                        keyValue.AddChild(childNode);
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
                        else if (TypeCheckMethods.IsList(value) ||
                                 TypeCheckMethods.IsArray(value))
                        {
                            // List || Array

                            if (value is IEnumerable castedValue)
                            {
                                keyValue = _nodeFactory.CreateNode(keyString, null, NodeType.ListNode);
                                int listID = 0;
                                foreach (var str in castedValue)
                                {
                                    string listIDString = listID.ToString();
                                    var childNode = SerializeList2(str, listIDString);
                                    keyValue.AddChild(childNode);
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
                            // keyValue = _nodeFactory.CreateNode(keyString, null, NodeType.InnerNode);
                            // var classNode = Serialize2(value, keyString);
                            // keyValue.AddChild(classNode);

                            keyValue = Serialize2(value, keyString);
                        }
                        else
                        {
                            throw new DSNotSupportedTypeException(valueType);
                        }

                        #endregion

                        // keyValuepair.AddChild(keyValue);
                        result.AddChild(keyValue);
                    }

                    return result;
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
        private static IDSNode SerializeList2(object? list, string id)
        {
            ///      (node) (List)
            ///        |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (Items)        
            
            // If classObj is list, create Null node
            if (list == null)
            {
                return _nodeFactory.CreateNode(id, null, NodeType.Leaf);
            }

            // Create node
            var result = _nodeFactory.CreateNode(id, null, NodeType.ListNode);

            if (list is IEnumerable castedList)
            {
                Type type = GetTypeMethods.GetItemTypeOfIEnumerable(castedList);

                // Check if type is supported
                if (false == DotSerialXML.IsTypeSupported(type))
                {
                    throw new DSNotSupportedTypeException(type);
                }

                if (TypeCheckMethods.IsPrimitive(type) || TypeCheckMethods.IsSpecialParsableObject(type))
                {
                    // Primitive types | special parsable objects
                    int listID = 0;
                    foreach (var str in castedList)
                    {
                        string listIDString = listID.ToString();
                        string? strValue = HelperMethods.PrimitiveToString(str);
                        var childNode = _nodeFactory.CreateNode(listIDString, strValue, NodeType.Leaf);
                        result.AddChild(childNode);
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
                        var childNode = SerializeList2(str, listIDString);
                        result.AddChild(childNode);
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
                        var childNode = SerializeDictionary2(str, listIDString);
                        result.AddChild(childNode);
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
                        var childNode = Serialize2(entry, listIDString);
                        result.AddChild(childNode);
                        listID++;
                    }
                }
                else
                {
                    throw new DSNotSupportedTypeException(type);
                }

                return result;
            }
            else
            {
                throw new InvalidCastException();
            }
        }        
    }
}
