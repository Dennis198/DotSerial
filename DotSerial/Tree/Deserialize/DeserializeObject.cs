using System.Reflection;
using DotSerial.Attributes;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Tree.Deserialize
{
    /// <summary>
    /// Class for deserialiation of object
    /// </summary>
    internal class DeserializeObject : INodeDeserializeVisitor
    {
        /// <inheritdoc/>
        /// <remarks>
        /// Only use this methode if the deserialized object itself is a dictionary.
        /// </remarks>
        public object? VisitDictionaryNode(DictionaryNode node, Type? type)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(type);

            var tmpList = DeserializeDictionaryNode(node, type);

            // Convert deserialzed dictionary.
            object? tmpValue = ConverterMethods.ConvertDeserializedDictionary(tmpList, type);

            return tmpValue;
        }

        /// <inheritdoc/>
        public object? VisitInnerNode(InnerNode node, Type? type)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(type);

            // Get type
            // Type typeObj = obj.GetType();

            var result = CreateInstanceMethods.CreateInstanceGeneric(type);

            // Get all Properties and iterate threw
            PropertyInfo[] props = type.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                // Get ID attribute
                string? propDSName = AttributesMethods.GetSerializeName(prop);

                if (null != propDSName)
                {
                    // Check if type is supported
                    if (false == TypeCheckMethods.IsTypeSupported(prop.PropertyType))
                    {
                        ThrowHelper.ThrowTypeIsNotSupportedException(prop.PropertyType);
                    }

                    var child = node.GetChild(propDSName);

                    // Special case: In must formats (json, yaml, ..) there is no
                    // difference in classes or dictionarys when parsing. So
                    // the dictionary case must also be handles here.
                    if (TypeCheckMethods.ImplementsICollectionKeyValuePair(prop.PropertyType))
                    {
                        var tmpList = DeserializeDictionaryNode(child, prop.PropertyType);

                        // Convert deserialzed dictionary.
                        object? tmpValue = ConverterMethods.ConvertDeserializedDictionary(tmpList, prop.PropertyType);

                        prop.SetValue(result, tmpValue);
                        continue;
                    }

                    var tmp = child.DeserializeAccept(this, prop.PropertyType);
                    prop.SetValue(result, tmp);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public object? VisitLeafNode(LeafNode node, Type? type)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(type);

            string? strValue = node.GetValue();

            if (null == strValue)
            {
                return null;
            }

            object? result;

            if (TypeCheckMethods.IsPrimitive(type))
            {
                result = ConverterMethods.ConvertStringToPrimitive(strValue, type);
            }
            else if (TypeCheckMethods.IsSpecialParsableObject(type))
            {
                result = ConverterMethods.ConvertStringToSpecialParsableObject(strValue, type);
            }
            else
            {
                ThrowHelper.ThrowWrongNodeTypeException();
                throw new Exception("Unreachable code.");
            }

            return result;
        }

        /// <inheritdoc/>
        public object? VisitListNode(ListNode node, Type? type)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(type);

            Type itemType = GetTypeMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            if (false == TypeCheckMethods.IsTypeSupported(itemType))
            {
                ThrowHelper.ThrowTypeIsNotSupportedException(itemType);
            }

            List<object?> tmpList = [];

            var children = node.GetChildren();

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];

                var tmp = child.DeserializeAccept(this, itemType);
                tmpList.Add(tmp);
            }

            var result = ConverterMethods.ConvertDeserializedList(tmpList, type);

            return result;
        }

        /// <summary>
        /// Deserialize dictionary
        /// </summary>
        /// <param name="node">Node</param>
        /// <param name="type">Type</param>
        /// <returns>Dictionary</returns>
        private Dictionary<object, object?>? DeserializeDictionaryNode(IDSNode node, Type type)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(type);

            if (GetTypeMethods.GetKeyValueTypeOfDictionary(type, out Type keyType, out Type valueType))
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

                if (node is LeafNode leaf)
                {
                    if (leaf.GetValue() == null)
                    {
                        return null;
                    }
                    else
                    {
                        ThrowHelper.ThrowWrongNodeTypeException();
                    }
                }

                Dictionary<object, object?> result = [];
                var children = node.GetChildren();

                foreach (var child in children)
                {
                    string key = child.Key;
                    object? value;
                    if (child is LeafNode)
                    {
                        value = child.DeserializeAccept(this, valueType);
                    }
                    else
                    {
                        value = child.DeserializeAccept(this, valueType);
                    }

                    result.Add(key, value);
                }

                return result;
            }
            else
            {
                ThrowHelper.ThrowTypeIsNotSupportedException(type);
                throw new Exception("Unreachable code.");
            }
        }
    }
}
