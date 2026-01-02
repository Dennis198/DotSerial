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

namespace DotSerial.Common
{
    /// <summary>
    /// Interface for the public nodes.
    /// </summary>
    /// <typeparam name="T">Type of the node (Json, Yaml, ...)</typeparam>
    public interface IDSSerialNode<T>
    {
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
        /// <typeparam name="U">Type of the object</typeparam>
        /// <returns>Deserilized object</returns>
        public abstract U ToObject<U>();        
    }
}