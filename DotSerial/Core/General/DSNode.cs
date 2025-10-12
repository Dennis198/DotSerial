using DotSerial.Core.Misc;
using System.Diagnostics;

namespace DotSerial.Core.General
{
    /// <summary>
    /// Node of a tree
    /// </summary>
    [DebuggerDisplay("Key = {Key}, Value = {Value}, IsNull = {IsNull}, IsEmpty = {IsEmpty}, Children = {Count}")]
    internal class DSNode(int key)
    {
        /// <summary>
        /// Key of the node
        /// </summary>
        public int Key { get; private set; } = key;

        /// <summary>
        /// Value of the node
        /// </summary>
        public string? Value { get; private set; }

        /// <summary>
        /// Type node
        /// </summary>
        public DSNodeType Type { get; private set; }

        /// <summary>
        /// Type of Proprty which is represented with this node.
        /// </summary>
        public DSNodePropertyType PropType { get; private set; }

        /// <summary>
        /// Children
        /// </summary>
        /// TODO: NUR ALS LISTE IMPLEMENTIEREN vlt wegen Liste nicht möglich?
        public Dictionary<int, DSNode> Children { get; private set; } = [];

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="type">Type</param>
        public DSNode(int key, DSNodeType type, DSNodePropertyType propType) : this(key)
        {
            Type = type;
            SetPropertyType(propType);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="type">Type</param>
        public DSNode(int key, object? value, DSNodeType type, DSNodePropertyType propType) : this(key, type, propType)
        {
            SetValue(value);
        }

        /// <summary>
        /// True, if Node has Null Value and is a leaf
        /// </summary>
        public bool IsNull 
        {
            get
            {
                return null == Value && Type == DSNodeType.Leaf;
            }
        }

        /// <summary>
        /// Truem if node no children but is NOT a leaf
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return Children.Count == 0 && Type != DSNodeType.Leaf;
            }
        }

        /// <summary>
        /// True, if Node has children
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return Children.Count > 0;
            }
        }

        /// <summary>
        /// Get number of children
        /// </summary>
        public int Count
        {
            get
            {
                return Children.Count;
            }
        }

        /// <summary>
        /// Changes the property type of the node
        /// </summary>
        /// <param name="propType">DSNodePropertyType</param>
        public void SetPropertyType(DSNodePropertyType propType)
        {
            if (Type == DSNodeType.Leaf)
            {
                // Make sure type is suitable for node
                if (propType != DSNodePropertyType.Primitive &&
                    propType != DSNodePropertyType.Null &&
                    propType != DSNodePropertyType.KeyValuePairKey &&
                    propType != DSNodePropertyType.KeyValuePairValue)
                {
                    throw new NotImplementedException();
                }

                // If node is leaf and value is null proptype
                // will be automatically put to Null.
                if (Type == DSNodeType.Leaf && Value == null)
                {
                    PropType = DSNodePropertyType.Null;
                }
                else
                {
                    PropType = propType;
                }
            }
            else
            {
                // Make sure type is suitable for node
                if (propType == DSNodePropertyType.Primitive || propType == DSNodePropertyType.Null)
                {
                    throw new NotImplementedException();
                }

                PropType = propType;
            }

        }

        /// <summary>
        /// Returns the child nodes of this child
        /// </summary>
        /// <returns>Dictionary<int, DSNode></returns>
        public Dictionary<int, DSNode> GetChildren()
        {
            return Children;
        }

        /// <summary>
        /// Sets the value
        /// </summary>
        /// <param name="value">Value</param>
        private void SetValue(object? value)
        {
            if (value == null)
            {
                Value = null;
            }
            else
            {
                string strValue;
                Type type = value.GetType();
                if (type == typeof(bool))
                {
                    int tmp = HelperMethods.BoolToInt((bool)value);
                    strValue = tmp.ToString();
                }
                else if (type.IsEnum)
                {
                    strValue = Convert.ToString((int)value);
                }
                else if (type == typeof(float))
                {
                    strValue = ((float)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (type == typeof(double))
                {
                    strValue = ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (type == typeof(decimal))
                {
                    strValue = ((decimal)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    strValue = value.ToString();
                }

                Value = strValue;
            }
        }

        /// <summary>
        /// Converts the value of the node to a specific type.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Object?</returns>
        public object? ConvertValue(Type type)
        {
            if (HasChildren)
            {
                throw new NotImplementedException();
            }
            return ConverterMethods.ConvertStringToPrimitive(Value, type);
        }

        /// <summary>
        /// Gets child node
        /// </summary>
        /// <param name="key">Key of the child node</param>
        /// <returns>Node</returns>
        public DSNode GetChild(int key)
        {
            // Key is already taken
            if (false == Children.ContainsKey(key))
            {
                throw new NotImplementedException();
            }

            return Children[key];
        }

        /// <summary>
        /// Append child node
        /// </summary>
        /// <param name="key">Key of the child node</param>
        /// <param name="node">Child node</param>
        public void AppendChild(int key, DSNode node)
        {
            // Can't append child to a leaf node
            if (Value != null)
            {
                throw new NotImplementedException();
            }

            // Key is already taken
            if (Children.ContainsKey(key))
            {
                throw new NotImplementedException();
            }

            Children.Add(key, node);
        }

        /// <summary>
        /// Gets the height of the node
        /// </summary>
        /// <returns>Height</returns>
        public int GetHeight()
        {
            if (Children.Count == 0)
            {
                return 1;
            }

            int max = -1;
            foreach(var tmp in Children)
            {
                DSNode child = tmp.Value;
                int tmpHeight = child.GetHeight();
                if (tmpHeight > max)
                {
                    max = tmpHeight;
                }
            }

            if (max < 1)
            {
                throw new NotImplementedException();
            }

            return max + 1;
        }

        public DSNode Clone(int newKey = -1)
        {
            // TODO Überarbeiten
            int key = newKey != -1 ? newKey : this.Key;
            DSNode clone = new DSNode(key, this.Value, this.Type, this.PropType);
            //var ttt = this._children[1];
            clone.Children.Add(key, this.Children[0]);
            return clone;
        }

        /// <summary>
        /// Returns true, if List represents list of primitives
        /// </summary>
        /// <returns>Bool</returns>
        public bool IsPrimitiveList()
        {
            if (PropType != DSNodePropertyType.List)
            {
                throw new NotImplementedException();
            }

            if (IsNull || IsEmpty || !HasChildren)
            {
                return false;
            }

            var child = Children[0];

            if (child.IsNull || child.Type == DSNodeType.Leaf)
            {
                return true;
            }

            return false;

        }

        /// <summary>
        /// Returns true, if List represents dictionary of primitives
        /// </summary>
        /// <returns>Bool</returns>
        public bool IsPrimitiveDictionary()
        {
            if (PropType != DSNodePropertyType.Dictionary)
            {
                throw new NotImplementedException();
            }

            if (IsNull || IsEmpty || !HasChildren)
            {
                return false;
            }

            var keyValuePair = Children[0];
            var value = keyValuePair.Children[1];

            if (value.IsNull || value.Type == DSNodeType.Leaf)
            {
                return true;
            }

            return false;
        }

        public List<DSNode> GetDictionaryKeyValuePairs()
        {
            if (PropType != DSNodePropertyType.Dictionary)
            {
                throw new NotImplementedException();
            }

            if (IsNull || IsEmpty || !HasChildren)
            {
                return [];
            }

            List<DSNode> result = [];

            foreach (var child in Children)
            {
                var tmp = child.Value;
                DSNode val = tmp;
                result.Add(val);
            }

            return result;
        }

        public List<string> GetDicionaryNodeKeys()
        {
            if (PropType != DSNodePropertyType.Dictionary)
            {
                throw new NotImplementedException();
            }

            if (IsNull || IsEmpty || !HasChildren)
            {
                return [];
            }

            List<string> result = [];

            foreach (var child in Children)
            {
                var tmp = child.Value.GetChildren();
                string key = tmp[0].Value;
                result.Add(key);
            }

            return result;
        }

        public List<DSNode> GetDicionaryNodeVales()
        {
            if (PropType != DSNodePropertyType.Dictionary)
            {
                throw new NotImplementedException();
            }

            if (IsNull || IsEmpty || !HasChildren)
            {
                return [];
            }

            List<DSNode> result = [];

            foreach (var child in Children)
            {
                var tmp = child.Value.GetChildren();
                DSNode val = tmp[1];
                result.Add(val);
            }

            return result;
        }


    }
}
