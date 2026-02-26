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

using System.Text;
using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;

namespace DotSerial.Utilities
{
    internal static class ParseMethods
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;  

        /// <summary>
        /// Apends the whole string from starting quote to end quote to
        /// the stringbuilder.
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="startIndex">Index of the opeing quote</param>
        /// <param name="str">string</param>
        /// <returns>Index of the closing quote</returns>
        internal static int AppendStringValue(StringBuilder sb, int startIndex, StringBuilder str)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (str.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(str.ToString());
            }

            if (str.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (str[startIndex] != CommonConstants.Quote)
            {
                throw new ArgumentException(str.ToString());
            }

            sb.Append(CommonConstants.Quote);

            for (int j = startIndex + 1; j < str.Length; j++)
            {
                var c2 = str[j];
                if (c2 == '\\')
                {
                    sb.Append(c2);
                    sb.Append(str[j + 1]);
                    j++;
                }
                else if (c2 == CommonConstants.Quote)
                {
                    sb.Append(c2);
                    return j;
                }
                else
                {
                    sb.Append(c2);
                }
            }

            return str.Length - 1;
        }      

        internal static int AppendTillStopChar(StringBuilder sb, int startIndex, StringBuilder str, char[] stopChars)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (str.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(str.ToString());
            }

            if (str.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }
            
            sb.Append(str[startIndex]);
            for (int i = startIndex + 1; i < str.Length; i++)
            {
                var c = str[i];
                
                if (stopChars.Contains(c))
                {
                    return i - 1;
                }
                sb.Append(c);
            }

            return str.Length - 1;
        }

        /// <summary>
        /// Appends an enclosed value from starting openChar to closing closeChar to
        /// the stringbuilder.
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="startIndex">Index of the opeing quote</param>
        /// <param name="str">Stringbuilder</param>
        /// <param name="openChar">Open char</param>
        /// <param name="closeChar">Closing char</param>
        /// <returns>Index of the closing char</returns>
        internal static int AppendEnclosingValue(StringBuilder sb, int startIndex, StringBuilder str, char openChar, char closeChar)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (str.IsNullOrWhiteSpace())
            {
                throw new ArgumentException(str.ToString());
            }

            if (str.Length < startIndex)
            {
                throw new IndexOutOfRangeException();
            }

            if (str[startIndex] != openChar)
            {
                throw new ArgumentException(str.ToString());
            }

            sb.Append(openChar);

            int numOpen = 0;

            for (int i = startIndex + 1; i < str.Length; i++)
            {
                var c = str[i];

                if (c == closeChar)
                {
                    if (numOpen == 0)
                    {
                        sb.Append(c);
                        return i;
                    }
                    else
                    {
                        numOpen--;
                        sb.Append(c);
                    }
                }
                else if (c == openChar)
                {
                    numOpen++;
                    sb.Append(c);
                }
                else
                {
                    sb.Append(c);
                }
            }

            return str.Length - 1;
        }  

        /// <summary>
        /// Removes all whitespaces inside a string
        /// except whitespace is between quotes.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>String without whitespaces.</returns>
        internal static StringBuilder RemoveWhiteSpace(string str)
        {
            // Check if value has value
            if (string.IsNullOrWhiteSpace(str))
            {
                return new StringBuilder();
            }

            StringBuilder sb = new();
            int stringLength = str.Length;
            StringBuilder strBuilder = new(str);

            for (int i = 0; i < stringLength; i++)
            {
                var c = str[i];

                // If char is a quoto extract everything
                // till the closing quote is reached
                if (c == CommonConstants.Quote)
                {
                    i = AppendStringValue(sb, i, strBuilder);
                    continue;
                }
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                sb.Append(c);
            }

            return sb;
        }       

        internal static string RemoveStartAndEndQuotes(string str)
        {
             return str[1..^1]; 
        }
        
        /// <summary>
        /// Parses primitive node without a key, e.g "3.14"
        /// </summary>
        /// <param name="strategyType">Strategy type</param>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="startIndex">StartIndex</param>
        /// <param name="key">Key of the node</param>
        /// <returns>Leafnode</returns>

        internal static IDSNode ParsePrimitiveNode(StategyType strategyType, StringBuilder sb, int startIndex, string key)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (sb.IsNullOrWhiteSpace() || sb.EqualsNullString())
            {
                return _nodeFactory.CreateNodeFromString(strategyType, key, null, NodeType.Leaf);
            }

            StringBuilder sbPrim = new();

            int i = AppendStringValue(sbPrim, startIndex, sb);
            if (i != sb.Length -1)
            {
                throw new DotSerialException("Parse: Can't parse single value.");
            }

            string nodeValue = sbPrim.ToString();
            
            return _nodeFactory.CreateNodeFromString(strategyType, key, nodeValue, NodeType.Leaf);
        }              
    }
}