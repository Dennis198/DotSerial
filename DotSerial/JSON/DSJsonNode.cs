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

using DotSerial.Core.General;
using DotSerial.JSON.Parser;
using DotSerial.JSON.Writer;
using DotSerial.Core.Tree;
using DotSerial.Core.Tree.Deserialize;
using DotSerial.Core.Tree.Nodes;
using DotSerial.Core.Tree.Serialize;

namespace DotSerial.JSON
{
    /// <summary>
    /// JSON Node
    /// </summary>
    public class DSJsonNode
    {
        /// <summary>Internal node </summary>
        private readonly IDSNode _node;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">Node</param>
        internal DSJsonNode(IDSNode node)
        {
            _node = node;
        }

        /// <summary>
        /// Key of the node
        /// </summary>
        public string Key => _node.Key;

        /// <summary>
        /// True, if node contains children
        /// </summary>
        public bool HasChildren => _node.HasChildren();

        /// <summary>
        /// Returns the internal node
        /// </summary>
        /// <returns>IDSNode</returns>
        internal IDSNode GetInternalData()
        {
            return _node;
        }

        /// <summary>
        /// Returns the child with the given key, otherwise null.
        /// </summary>
        /// <param name="key">Key of child</param>
        /// <returns>Child node</returns>
        public DSJsonNode? GetChild(string key)
        {
            var child = _node.GetChild(key);
            return new DSJsonNode(child);
        }

        /// <summary>
        /// Returns all children of the node.
        /// </summary>
        /// <returns>All chidlren</returns>
        public List<DSJsonNode>? GetChildren()
        {
            if (false == HasChildren)
                return null;

            var children = new List<DSJsonNode>();
            var tmp = _node.GetChildren();
            foreach (var c in tmp)
            {
                children.Add(new DSJsonNode(c));
            }

            return children;
        }

        /// <summary>
        /// Creates the node structure for a given obj
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="key">Key of the object</param>
        /// <returns>DSJsonNode</returns>
        public static DSJsonNode ToNode(object? obj, string? key = null)
        {
            // Determine key
            string currKey = key ?? GeneralConstants.MainObjectKey;
            
            // Serialize object
            var rootNode = SerializeObject.Serialize(obj, currKey);

            return new DSJsonNode(rootNode);
        }

        /// <summary>
        /// Converts the node to a JSON string.
        /// </summary>
        /// <returns>Json string</returns>
        public string ToJsonString()
        {
            if (null == _node)
            {
                throw new NotImplementedException();
            }

            // Convert
            string jsonString = JSONWriterVisitor.Write(this);

            return jsonString;
        }

        /// <summary>
        /// Parses a json string to a DSJsonNode
        /// </summary>
        /// <param name="jsonString">Json string</param>
        /// <returns>DSJsonNode</returns>
        public static DSJsonNode FromJsonString(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new NotImplementedException();
            }   

            var root = JSONParserVisitor.Parse(jsonString);        

            return root;
        }

        /// <summary>
        /// Deserializes a json string to an object of type U
        /// </summary>
        /// <param name="jsonString">Json string</param>
        /// <typeparam name="U">Type fo the object</typeparam>
        /// <returns>Deserilized object</returns>
        public static U ToObject<U>(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new NotImplementedException();
            }

            // Parse json string to node
            DSJsonNode dsNode = FromJsonString(jsonString);

            return dsNode.ToObject<U>();
        }

        /// <summary>
        /// Deserializes the node to an object of type U
        /// </summary>
        /// <typeparam name="U">Type fo the object</typeparam>
        /// <returns>Deserilized object</returns>
        public U ToObject<U>()
        {
            if (null == _node)
            {
                throw new NotImplementedException();
            }

            // Deserilize object
            var resultObj = _node.DeserializeAccept(new DeserializeObject(), typeof(U));

            if (null == resultObj)
            {
                throw new NotImplementedException();
            }
            
            // cast object to U
            if (resultObj is U result)
            {
                return result;
            }
            else
            {
                throw new NotImplementedException();
            }    
        }        
    }
}