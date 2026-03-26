using System.Collections;
using System.Reflection;
using DotSerial.Attributes;
using DotSerial.Common;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Tree.Serialize
{
    /// <summary>
    /// Class for serialization of an object
    /// </summary>
    internal class SerializeObject
    {
        /// <summary>
        /// Node factory;
        /// </summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <summary>
        /// Serialize object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="objectID">Object-ID</param>
        /// <returns>Node</returns>
        internal static IDSNode Serialize(object? obj, string objectID, SerializeStrategy strategyType)
        {
            ///      (node) (Class)
            ///        |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (Properties)
            // If classObj is null, create Null node
            if (obj == null)
            {
                return _nodeFactory.CreateNode(strategyType, objectID, null, TreeNodeType.Leaf, null);
            }

            Type typeObj = obj.GetType();

            // Check if type is supported
            if (false == TypeCheckMethods.IsTypeSupported(typeObj))
            {
                ThrowHelper.ThrowTypeIsNotSupportedException(typeObj);
            }

            IDSNode? result;

            if (TypeCheckMethods.IsPrimitive(typeObj) || TypeCheckMethods.IsSpecialParsableObject(typeObj))
            {
                result = _nodeFactory.CreateNode(strategyType, objectID, obj, TreeNodeType.Leaf, null);
            }
            else if (TypeCheckMethods.IsDictionary(typeObj))
            {
                result = SerializeDictionary(obj, objectID, strategyType);
            }
            else if (TypeCheckMethods.IsList(typeObj) || TypeCheckMethods.IsArray(typeObj))
            {
                result = SerializeList(obj, objectID, strategyType);
            }
            else if (TypeCheckMethods.IsClass(typeObj) || TypeCheckMethods.IsStruct(typeObj))
            {
                result = SerializeClass(obj, objectID, strategyType);
            }
            else
            {
                ThrowHelper.ThrowTypeIsNotSupportedException(typeObj);
                throw new Exception("Unreachable code.");
            }

            if (null == result)
            {
                ThrowHelper.ThrowIfNullException(result);
                throw new Exception("Unreachable code.");
            }

            return result;
        }

        /// <summary>
        /// Serialize class object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="objectID">Object-ID</param>
        /// <returns>Node</returns>
        private static IDSNode SerializeClass(object? classObj, string objectID, SerializeStrategy strategyType)
        {
            ///      (node) (Class)
            ///        |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (Properties)
            // If classObj is null, create Null node
            if (classObj == null)
            {
                return _nodeFactory.CreateNode(strategyType, objectID, null, TreeNodeType.Leaf, null);
            }

            // Create node
            var result = _nodeFactory.CreateNode(strategyType, objectID, null, TreeNodeType.InnerNode, null);

            Type typeObj = classObj.GetType();

            // Create datastructur to check if every id in a class is only
            // used once.
            Dictionary<string, string> dicIdName = [];

            // Get all Properties and iterate threw
            PropertyInfo[] props = typeObj.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                // Get ID attribute
                string? dsPropName = AttributesMethods.GetSerializeName(prop);

                // Check if property got the attribute
                if (null != dsPropName)
                {
                    // Check if type is supported
                    if (false == TypeCheckMethods.IsTypeSupported(prop.PropertyType))
                    {
                        ThrowHelper.ThrowTypeIsNotSupportedException(prop.PropertyType);
                    }

                    // Check if id was already used.
                    // If yes throw exception.
                    if (dicIdName.ContainsKey(dsPropName))
                    {
                        ThrowHelper.ThrowDuplicateNodeKeyTypeException(dsPropName);
                    }

                    // Get Value of property
                    object? value = prop.GetValue(classObj);
                    // Get name of property
                    string propName = prop.Name;

                    // Add ID and prop name to datastrcuture.
                    dicIdName.Add(dsPropName, propName);

                    if (
                        TypeCheckMethods.IsPrimitive(prop.PropertyType)
                        || TypeCheckMethods.IsSpecialParsableObject(prop.PropertyType)
                    )
                    {
                        // Primitive types || String
                        var childNode = _nodeFactory.CreateNode(
                            strategyType,
                            dsPropName,
                            value,
                            TreeNodeType.Leaf,
                            result
                        );
                        result.AddChild(childNode);
                    }
                    else if (TypeCheckMethods.IsDictionary(prop.PropertyType))
                    {
                        // Dictionary
                        var childNode = SerializeDictionary(value, dsPropName, strategyType);
                        childNode.Parent = result;
                        result.AddChild(childNode);
                    }
                    else if (TypeCheckMethods.IsList(prop.PropertyType) || TypeCheckMethods.IsArray(prop.PropertyType))
                    {
                        // List || Array
                        var childNode = SerializeList(value, dsPropName, strategyType);
                        childNode.Parent = result;
                        result.AddChild(childNode);
                    }
                    else if (
                        TypeCheckMethods.IsClass(prop.PropertyType) || TypeCheckMethods.IsStruct(prop.PropertyType)
                    )
                    {
                        // Class || Struct
                        var childNode = SerializeClass(value, dsPropName, strategyType);
                        childNode.Parent = result;
                        result.AddChild(childNode);
                    }
                    else
                    {
                        ThrowHelper.ThrowTypeIsNotSupportedException(prop.PropertyType);
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
        private static IDSNode SerializeDictionary(object? dic, string id, SerializeStrategy strategyType)
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
                return _nodeFactory.CreateNode(strategyType, id, null, TreeNodeType.Leaf, null);
            }

            // Create node
            var result = _nodeFactory.CreateNode(strategyType, id, null, TreeNodeType.DictionaryNode, null);

            if (dic is IDictionary castedDic)
            {
                if (GetTypeMethods.GetKeyValueTypeOfDictionary(dic, out Type keyType, out Type valueType))
                {
                    // Check if type is supported
                    if (false == TypeCheckMethods.IsTypeSupported(keyType))
                    {
                        ThrowHelper.ThrowTypeIsNotSupportedException(keyType);
                    }
                    // Check if type is supported
                    if (false == TypeCheckMethods.IsTypeSupported(valueType))
                    {
                        ThrowHelper.ThrowTypeIsNotSupportedException(valueType);
                    }

                    foreach (DictionaryEntry keyValuePair in castedDic)
                    {
                        var key = keyValuePair.Key;
                        var value = keyValuePair.Value;

                        IDSNode keyValue;
                        string? keyString = null;
                        #region Key

                        // Key
                        ThrowHelper.ThrowIfNullException(key);

                        if (TypeCheckMethods.IsPrimitive(keyType) || TypeCheckMethods.IsSpecialParsableObject(keyType))
                        {
                            keyString = HelperMethods.PrimitiveToString(key);
                        }
                        else
                        {
                            throw new DotSerialException(
                                $"Serialize: Key type must be primitive for dictionary but was {keyType}."
                            );
                        }

                        if (null == keyString)
                        {
                            ThrowHelper.ThrowKeyNodeNullException();
                        }

                        #endregion

                        #region Value

                        if (null == value)
                        {
                            keyValue = _nodeFactory.CreateNode(
                                strategyType,
                                keyString,
                                null,
                                TreeNodeType.Leaf,
                                result
                            );
                        }
                        else if (
                            TypeCheckMethods.IsPrimitive(valueType)
                            || TypeCheckMethods.IsSpecialParsableObject(valueType)
                        )
                        {
                            keyValue = _nodeFactory.CreateNode(
                                strategyType,
                                keyString,
                                value,
                                TreeNodeType.Leaf,
                                result
                            );
                        }
                        else if (TypeCheckMethods.IsDictionary(value))
                        {
                            // Dictionary
                            if (value is IDictionary castedValue)
                            {
                                if (GetTypeMethods.GetKeyValueTypeOfDictionary(dic, out Type innerKeyType, out Type _))
                                {
                                    keyValue = _nodeFactory.CreateNode(
                                        strategyType,
                                        keyString,
                                        null,
                                        TreeNodeType.DictionaryNode,
                                        result
                                    );
                                    foreach (DictionaryEntry str in castedValue)
                                    {
                                        string? innerDicID =
                                            HelperMethods.PrimitiveToString(str.Key)
                                            ?? throw new DotSerialException(
                                                $"Serialize: Can't convert {str.Key} to string."
                                            );
                                        var childNode = SerializeDictionary(str, innerDicID, strategyType);
                                        childNode.Parent = keyValue;
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
                        else if (TypeCheckMethods.IsList(value) || TypeCheckMethods.IsArray(value))
                        {
                            // List || Array

                            if (value is IEnumerable castedValue)
                            {
                                keyValue = _nodeFactory.CreateNode(
                                    strategyType,
                                    keyString,
                                    null,
                                    TreeNodeType.ListNode,
                                    result
                                );
                                int listID = 0;
                                foreach (var str in castedValue)
                                {
                                    string listIDString = listID.ToString();
                                    var childNode = SerializeList(str, listIDString, strategyType);
                                    childNode.Parent = keyValue;
                                    keyValue.AddChild(childNode);
                                    listID++;
                                }
                            }
                            else
                            {
                                throw new InvalidCastException();
                            }
                        }
                        else if (TypeCheckMethods.IsClass(valueType) || TypeCheckMethods.IsStruct(valueType))
                        {
                            keyValue = SerializeClass(value, keyString, strategyType);
                        }
                        else
                        {
                            ThrowHelper.ThrowTypeIsNotSupportedException(valueType);
                            throw new Exception("Unreachable code.");
                        }

                        #endregion

                        keyValue.Parent = result;
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
        private static IDSNode SerializeList(object? list, string id, SerializeStrategy strategyType)
        {
            ///      (node) (List)
            ///        |
            ///  -------------
            ///  |     |     |
            /// (A)   (B)   (C) (Items)
            // If classObj is list, create Null node
            if (list == null)
            {
                return _nodeFactory.CreateNode(strategyType, id, null, TreeNodeType.Leaf, null);
            }

            // Create node
            var result = _nodeFactory.CreateNode(strategyType, id, null, TreeNodeType.ListNode, null);

            if (list is IEnumerable castedList)
            {
                Type type = GetTypeMethods.GetItemTypeOfIEnumerable(castedList);

                // Check if type is supported
                if (false == TypeCheckMethods.IsTypeSupported(type))
                {
                    ThrowHelper.ThrowTypeIsNotSupportedException(type);
                }

                if (TypeCheckMethods.IsPrimitive(type) || TypeCheckMethods.IsSpecialParsableObject(type))
                {
                    // Primitive types | special parsable objects
                    int listID = 0;
                    foreach (var str in castedList)
                    {
                        string listIDString = listID.ToString();
                        var childNode = _nodeFactory.CreateNode(
                            strategyType,
                            listIDString,
                            str,
                            TreeNodeType.Leaf,
                            result
                        );
                        result.AddChild(childNode);
                        listID++;
                    }
                }
                else if (TypeCheckMethods.IsList(type) || TypeCheckMethods.IsArray(type))
                {
                    // List || Array
                    int listID = 0;
                    foreach (var str in castedList)
                    {
                        string listIDString = listID.ToString();
                        var childNode = SerializeList(str, listIDString, strategyType);
                        childNode.Parent = result;
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
                        var childNode = SerializeDictionary(str, listIDString, strategyType);
                        childNode.Parent = result;
                        result.AddChild(childNode);
                        listID++;
                    }
                }
                else if (TypeCheckMethods.IsClass(type) || TypeCheckMethods.IsStruct(type))
                {
                    // Class || Struct
                    int listID = 0;
                    foreach (var entry in castedList)
                    {
                        string listIDString = listID.ToString();
                        var childNode = SerializeClass(entry, listIDString, strategyType);
                        childNode.Parent = result;
                        result.AddChild(childNode);
                        listID++;
                    }
                }
                else
                {
                    ThrowHelper.ThrowTypeIsNotSupportedException(type);
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
