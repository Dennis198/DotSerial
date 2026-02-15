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

using DotSerial.Common;
using DotSerial.Tree.Nodes;
using DotSerial.Tree.Serialize;
using DotSerial.Yaml.Parser;
using DotSerial.Yaml.Writer;

namespace DotSerial.Yaml
{
    /// <summary>
    /// Yaml Node
    /// </summary>
    public class DSYamlNode : IDSSerialNode<DSYamlNode>
    {
        /// <summary>Internal node </summary>
        private readonly IDSNode _node;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">Node</param>
        internal DSYamlNode(IDSNode node)
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
        public DSYamlNode? GetChild(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            var child = _node.GetChild(key);
            return new DSYamlNode(child);
        }

        /// <inheritdoc/>
        public List<DSYamlNode>? GetChildren()
        {
            if (false == HasChildren)
                return null;

            var children = new List<DSYamlNode>();
            var tmp = _node.GetChildren();
            foreach (var c in tmp)
            {
                children.Add(new DSYamlNode(c));
            }

            return children;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSYamlException">DotSerial Exception.</exception>
        public static DSYamlNode ToNode(object? obj, string? key = null)
        {
            try
            {
                // Determine key
                string currKey = key ?? CommonConstants.MainObjectKey;
                
                // Serialize object
                var rootNode = SerializeObject.Serialize(obj, currKey);

                return new DSYamlNode(rootNode);
            }
            catch (DotSerialException ex)
            {
                throw new DSYamlException(ex.Message);
            }
            catch
            {
                throw;
            }
        }       

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSYamlException">DotSerial Exception.</exception>
        public string Stringify()
        {            
            try
            {
                if (null == _node)
                {
                    throw new DSYamlException($"{_node} can't be null.");
                }

                // Convert
                string yamlString = YamlWriterVisitor.Write(this);

                return yamlString;
            }
            catch (DotSerialException ex)
            {
                throw new DSYamlException(ex.Message);
            }
            catch
            {
                throw;
            }            
        }         

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSYamlException">DotSerial Exception.</exception>
        public static DSYamlNode FromString(string str)
        {
            try
            {
                if (null == str)
                {
                    throw new DSYamlException($"{str} can't be null.");
                }      

                var root = YamlParserVisitor.Parse(str);        

                return root;
            }
            catch (DotSerialException ex)
            {
                throw new DSYamlException(ex.Message);
            }
            catch
            {
                throw;
            }               
        }        

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="DSYamlException">DotSerial Exception.</exception>
        public static U ToObject<U>(string str)
        {
            try
            {
                if (null == str)
                {
                    throw new DSYamlException($"{str} can't be null.");
                }                

                // Parse yaml string to node
                DSYamlNode dsNode = FromString(str);

                return IDSSerialNode<U>.ToObject<U>(dsNode.GetInternalData());
            }
            catch (DotSerialException ex)
            {
                throw new DSYamlException(ex.Message);
            }
            catch
            {
                throw;
            }              
        }                
    }
}