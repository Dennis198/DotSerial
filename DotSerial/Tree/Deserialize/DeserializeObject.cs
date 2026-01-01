using System.Reflection;
using DotSerial.Utilities;
using DotSerial.Tree.Nodes;
using DotSerial.XML;

namespace DotSerial.Tree.Deserialize
{
    /// <summary>
    /// TODO
    /// </summary>
    public class DeserializeObject : INodeDeserializeVisitor
    {
        public object? VisitLeafNode(LeafNode node, Type? type)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(type);

            string? strValue = node.GetValue();
            object? tmp = null;

            if (null == strValue)
            {
                return null;
            }

            if (TypeCheckMethods.IsPrimitive(type))
            {
                tmp = ConverterMethods.ConvertStringToPrimitive(strValue, type);
            }
            else if (TypeCheckMethods.IsSpecialParsableObject(type))
            {
                tmp = ConverterMethods.ConvertStringToSpecialParsableObject(strValue, type);
            }
            else
            {
                throw new NotImplementedException();
            }

            return tmp;

        }

        public object? VisitInnerNode(InnerNode node, Type? type)
        {
            ArgumentNullException.ThrowIfNull(node);
            
            if (null == type)
            {
                throw new NotImplementedException();
            }        

            // Get type
            // Type typeObj = obj.GetType();

            var result = CreateInstanceMethods.CreateInstanceGeneric(type);

            // Get all Properties and iterate threw
            PropertyInfo[] props = type.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                // Get ID attribute
                int id = Attributes.HelperMethods.GetPropertyID(prop);

                if (-1 != id)
                {
                    // Check if type is supported
                    if (false == DotSerialXML.IsTypeSupported(prop.PropertyType))
                    {
                        throw new NotImplementedException();
                    }

                    var child = node.GetChild(id.ToString());

                    // TODO SONDERFAQLL
                    if (TypeCheckMethods.IsDictionary(prop.PropertyType))
                    {
                        var tmpList = DeserializeDictionary(child, prop.PropertyType);

                        object? tmpValue;
                        // Convert deserialzed dictionary.
                        tmpValue = ConverterMethods.ConvertDeserializedDictionary(tmpList, prop.PropertyType);

                        prop.SetValue(result, tmpValue);
                        continue;
                    }

                    if (null == child)
                    {
                        throw new InvalidOperationException($"Child with ID {id} not found in node {node.Key}");
                    }

                    // var tmp = CreateInstanceMethods.CreateInstanceGeneric(prop.PropertyType);

                    // TODO als ref machen?
                    

                    var tmp = child.DeserializeAccept(this, prop.PropertyType);
                    prop.SetValue(result, tmp);
                }
            }

            return result;
        }

        public object? VisitListNode(ListNode node, Type? type)
        {
            ArgumentNullException.ThrowIfNull(node);

            Type itemType = GetTypeMethods.GetItemTypeOfIEnumerable(type);

            // Check if type is supported
            if (false == DotSerialXML.IsTypeSupported(itemType))
            {
                throw new NotImplementedException();
            }

            List<object?> result = [];

            var children = node.GetChildren();

            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];

                var tmp = child.DeserializeAccept(this, itemType);
                result.Add(tmp);
            }

            var gg = ConverterMethods.ConvertDeserializedList(result, type);

            return gg;
        }

        private Dictionary<object, object?>? DeserializeDictionary(IDSNode node, Type type)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(type);

            if (GetTypeMethods.GetKeyValueTypeOfDictionary(type, out Type keyType, out Type valueType))
            {
                 // Check if type is supported
                if (false == DotSerialXML.IsTypeSupported(keyType))
                {
                    throw new NotImplementedException();
                }
                // Check if type is supported
                if (false == DotSerialXML.IsTypeSupported(valueType))
                {
                    throw new NotImplementedException();
                }

                if (node is LeafNode leaf)
                {
                    if (leaf.GetValue() == null)
                    {
                        return null;
                    }
                    else
                    {
                        throw new InvalidOperationException("Node is a leaf but dictionary expected");
                    }
                }

                Dictionary<object, object?> result = [];
                var children = node.GetChildren();

                foreach(var child in children)
                {
                    string key = child.Key;
                    object? value;
                    if (child is LeafNode)
                    {
                        value = child.DeserializeAccept(this, valueType);
                    }
                    else
                    {
                        // var ggg = child.GetChild(key); // TODO????
                        value = child.DeserializeAccept(this, valueType);
                    }

                    result.Add(key, value);
                }

                return result;
            }
            else
            {
                throw new InvalidOperationException("Type is not a Dictionary");
            }

        }

        public object? VisitDictionaryNode(DictionaryNode node, Type? type)
        {
            throw new NotImplementedException();
        }
    }
}