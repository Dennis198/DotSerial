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

using DotSerial.Common;
using DotSerial.Json;
using DotSerial.JSON.Parser;
using DotSerial.JSON.Writer;
using DotSerial.Tree.Deserialize;
using DotSerial.Tree.Nodes;
using DotSerial.Tree.Serialize;

namespace DotSerial.JSON
{
    /// <summary>
    /// JSON Node
    /// </summary>
    public class DSJsonNode : IDSSerialNode<DSJsonNode>
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

        /// <inheritdoc/>
        public string Key => _node.Key;

        /// <inheritdoc/>
        public bool HasChildren => _node.HasChildren();

        /// <summary>
        /// Returns the internal node
        /// </summary>
        /// <returns>IDSNode</returns>
        internal IDSNode GetInternalData()
        {
            return _node;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        public DSJsonNode? GetChild(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            var child = _node.GetChild(key);
            return new DSJsonNode(child);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSJsonException">DotSerial Exception.</exception>
        public static DSJsonNode ToNode(object? obj, string? key = null)
        {
            try
            {
                // Determine key
                string currKey = key ?? CommonConstants.MainObjectKey;
                
                // Serialize object
                var rootNode = SerializeObject.Serialize(obj, currKey);

                return new DSJsonNode(rootNode);
            }
            catch(DotSerialException ex)
            {
                throw new DSJsonException(ex.Message);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSJsonException">DotSerial Exception.</exception>
        public string Stringify()
        {
            if (null == _node)
            {
                throw new DSJsonException($"{_node} can't be null.");
            }

            try
            {
                // Convert
                string jsonString = JSONWriterVisitor.Write(this);

                return jsonString;
            }
            catch(DotSerialException ex)
            {
                throw new DSJsonException(ex.Message);
            }
            catch
            {
                throw;
            }            
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSJsonException">DotSerial Exception.</exception>
        public static DSJsonNode FromString(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    throw new DSJsonException($"{str} can't be null or whitespace.");
                }   

                var root = JSONParserVisitor.Parse(str);        

                return root;
            }
            catch(DotSerialException ex)
            {
                throw new DSJsonException(ex.Message);
            }
            catch
            {
                throw;
            }             
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSJsonException">DotSerial Exception.</exception>
        public static U ToObject<U>(string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    throw new DSJsonException($"{str} can't be null or whitespace.");
                }

                // Parse json string to node
                DSJsonNode dsNode = FromString(str);

                return dsNode.ToObject<U>();
            }
            catch(DotSerialException ex)
            {
                throw new DSJsonException(ex.Message);
            }
            catch
            {
                throw;
            }              
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSJsonException">DotSerial Exception.</exception>
        public U ToObject<U>()
        {
            try
            {
                if (null == _node)
                {
                    throw new DSJsonException($"{_node} can't be null.");
                }

                // Deserilize object
                var resultObj = _node.DeserializeAccept(new DeserializeObject(), typeof(U));

                if (null == resultObj)
                {
                    throw new DSJsonException($"{resultObj} can't be null.");
                }
                
                // cast object to U
                if (resultObj is U result)
                {
                    return result;
                }
                else
                {
                    throw new DSJsonException($"{resultObj} is not of type {nameof(U)}.");
                }  
            }
            catch(DotSerialException ex)
            {
                throw new DSJsonException(ex.Message);
            }
            catch
            {
                throw;
            }               
        }        
    }
}