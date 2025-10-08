
using DotSerial.Core.Misc;
using System.Diagnostics;

namespace DotSerial.Core.General
{
    /// <summary>
    /// Node of a tree
    /// </summary>
    [DebuggerDisplay("Key = {Key}, Name = {Name}, Value = {Value}")]
    internal class DSNode
    {
        /// <summary>
        /// Key of the node
        /// </summary>
        public int Key { get; private set; }

        /// <summary>
        /// Value of the node
        /// </summary>
        public string? Value { get; private set; }

        /// <summary>
        /// Custom name of node
        /// </summary>
        public string? Name { get; private set; }

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
        private Dictionary<int, DSNode> _children = [];

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="name">Name</param>
        /// <param name="type">Type</param>
        public DSNode(int key, object? value, string name, DSNodeType type, DSNodePropertyType propType) : this(key, name, type, propType)
        {
            SetValue(value);
        }

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="name">Name</param>
        /// <param name="type">Type</param>
        public DSNode(int key, string name, DSNodeType type, DSNodePropertyType propType)
        {
            Key = key;
            Name = name;
            Type = type;
            PropType = propType;
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
                return _children.Count == 0 && Type != DSNodeType.Leaf;
            }
        }

        /// <summary>
        /// True, if Node has children
        /// </summary>
        public bool HasChildren
        {
            get
            {
                return _children.Count > 0;
            }
        }

        /// <summary>
        /// Get number of children
        /// </summary>
        public int Count
        {
            get
            {
                return _children.Count;
            }
        }

        public Dictionary<int, DSNode> GetChildren()
        {
            return _children;
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

            var child = _children[0];

            if (child.IsNull || child.Type == DSNodeType.Leaf)
            {
                return true;
            }

            return false;
            
        }

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

            var keyValuePair = _children[0];

            //if (keyValuePair.IsNull)
            //{
            //    return true;
            //}

            var value = keyValuePair._children[1];

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

            foreach (var child in _children)
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

            foreach (var child in _children)
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

            foreach (var child in _children)
            {
                var tmp = child.Value.GetChildren();
                DSNode val = tmp[1];
                result.Add(val);
            }

            return result;
        }

        /// <summary>
        /// Gets child node
        /// </summary>
        /// <param name="key">Key of the child node</param>
        /// <returns>Node</returns>
        public DSNode GetChild(int key)
        {
            // Key is already taken
            if (false == _children.ContainsKey(key))
            {
                throw new NotImplementedException();
            }

            return _children[key];
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
            if (_children.ContainsKey(key))
            {
                throw new NotImplementedException();
            }

            _children.Add(key, node);
        }

        /// <summary>
        /// Gets the height of the node
        /// </summary>
        /// <returns>Height</returns>
        public int GetHeight()
        {
            if (_children.Count == 0)
            {
                return 1;
            }

            int max = -1;
            foreach(var tmp in _children)
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
            clone._children.Add(key, this._children[0]);
            return clone;
        }
    }
}
