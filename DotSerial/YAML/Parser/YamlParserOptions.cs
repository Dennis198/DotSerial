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

namespace DotSerial.YAML.Parser
{
     /// <summary>
    /// Additional options for node visitors.
    /// </summary>
    public class YamlParserOptions
    {
        /// <summary>
        /// Key 
        /// </summary>
        internal string Key;
        /// <summary>
        /// Level of node 
        /// </summary>        
        internal int Level;
        /// <summary>
        /// StartIndex of Object 
        /// </summary>
        internal int StartLineIndex;
        /// <summary>
        /// EndIndex of OPbject
        /// </summary>
        internal int EndLineIndex;

        internal bool IsList;
        internal bool IsObject;

        internal bool IsEmptyList;
        internal bool IsEmptyObject;

        internal YamlParserOptions(string key, int level, int startLineIndex, int endLineIndex)
        {
            Key = key;
            Level = level;
            StartLineIndex = startLineIndex;
            EndLineIndex = endLineIndex;
        }

        // TODO SET ISList & IsObject extra

        internal void SetIsList()
        {   
            IsList = true;
        }

        internal void SetIsYamlObject()
        {
            IsObject = true;
        }

        /// <summary>
        /// Check if object is a yaml object or just a key value pair
        /// </summary>
        /// <returns>True, if object</returns>
        internal bool IsYamlObject()
        {
            return IsObject;
        }
    }
}