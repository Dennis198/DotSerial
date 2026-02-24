#region License
//Copyright (c) 2026 Dennis Sölch

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

using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Deserialize;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Common
{
    /// <summary>
    /// Interface for the public nodes.
    /// </summary>
    /// <typeparam name="T">Type of the node (Json, Yaml, ...)</typeparam>
    public interface IDSSerialNode<T>
    {
        /// <summary>Node factory</summary>
        // private static readonly NodeFactoryObsolete _nodeFactory = NodeFactoryObsolete.Instance;
        
        /// <summary>
        /// Key of the node.
        /// </summary>
        public string Key {get;}

        /// <summary>
        /// Check if node has children.
        /// </summary>
        public bool HasChildren {get;}

        /// <summary>
        /// Gets the child with the key.
        /// </summary>
        /// <param name="key">Key of the child</param>
        /// <returns>Child node</returns>
        public abstract T? GetChild(string key);

        /// <summary>
        /// Creates the node/tree structure for a given object.
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="key">Key of the object</param>
        /// <returns>Node</returns>
        public abstract static T ToNode(object? obj, string? key = null);

        /// <summary>
        /// Converts the node to a string.
        /// </summary>
        /// <returns>String</returns>
        public abstract string Stringify();

        /// <summary>
        /// Create the node/tree from the given string.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>Node</returns>
        public abstract static T FromString(string str);

        /// <summary>
        /// Deserializes a string to an object of type U.
        /// </summary>
        /// <param name="str">String</param>
        /// <typeparam name="U">Type of the object</typeparam>
        /// <returns>Deserilized object</returns>
        public abstract static U ToObject<U>(string str);

        /// <summary>
        /// Deserializes the node to an object of type U.
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <typeparam name="U">Type of the object</typeparam>
        /// <returns>Deserilized object</returns>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DotSerialException">DotSerial Exception.</exception>
        public static U ToObject<U>(IDSNode node)
        {
            try
            {
                if (null == node)
                {
                    throw new DotSerialException($"{node} can't be null.");
                }

                IDSNode nodeToSerialize = node;

                // Special case: In must formats (json, yaml, ..) there is no
                // difference in classes or dictionarys when parsing. So
                // in parsing an dictionary node will be created as an inner node.
                // So for the deserialization of an dictionaty type we habe to create an
                // dictionary node which wraps the inner node.
                if (TypeCheckMethods.IsDictionary(typeof(U)) && nodeToSerialize is InnerNode)
                {
                    nodeToSerialize = NodeFactory.WrappNode(nodeToSerialize, NodeType.DictionaryNode);
                }

                // Deserilize object
                var resultObj = nodeToSerialize.DeserializeAccept(new DeserializeObject(), typeof(U));

                if (null == resultObj)
                {
#pragma warning disable CS8603
                    return default;
#pragma warning restore CS8603                    
                }
                
                // cast object to U
                if (resultObj is U result)
                {
                    return result;
                }
                else
                {
                    throw new DotSerialException($"{resultObj} is not of type {nameof(U)}.");
                }  
            }
            catch(DotSerialException ex)
            {
                throw new DotSerialException(ex.Message);
            }
            catch
            {
                throw;
            }               
        }        
    }
}