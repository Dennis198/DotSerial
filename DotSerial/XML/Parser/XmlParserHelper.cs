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
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.XML.Parser
{
    // TODO interface internal machen.
    public class XmlTagKeyPair
    {
        internal string Tag;
        internal string Key;

        internal XmlTagKeyPair(string tag, string key)
        {
            Tag = tag;
            Key = key;
        }

        internal bool IsXmlObject()
        {
            return Tag == XmlConstants.XmlInnerNodeProp;
        }

        internal bool IsXmlList()
        {
            return Tag == XmlConstants.XmlListProp;
        }

        internal bool IsXmlPrimitive()
        {
            return Tag == XmlConstants.XmlLeafProp;
        }
    }

    /// <summary>
    /// Helper class with methode for parsing xml.
    /// </summary>
    internal static class XmlParserHelper
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;
        
        internal static Dictionary<XmlTagKeyPair, StringBuilder?> ExtractKeyValuePairsFromXmlObject(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            var result = new Dictionary<XmlTagKeyPair, StringBuilder?>();

            for (int i = 0; i < sb.Length; i++)
            {  
                char c = sb[i];

                if (c == XmlConstants.XmlTagOpening)
                {
                    // TODO extract key value pair
                    // i = ExtractKeyValuePair(sb, i, out XmlTagKeyPair tagKeyPair);
                    // result.Add(tagKeyPair, null);
                    i = ExtractKeyValuePair(sb, i, out XmlTagKeyPair tagKeyPair, out StringBuilder? value);
                    result.Add(tagKeyPair, value);
                }
            }

            return result;
        }

        private static int ExtractKeyValuePair(StringBuilder sb, int startIndex, out XmlTagKeyPair tagKeyPair, out StringBuilder? value)
        {
            ArgumentNullException.ThrowIfNull(sb);

            int start = ExtractObjectStart(sb, startIndex, out StringBuilder tmp);
            tagKeyPair = ExtractTagAndKey(tmp);

            if (IsEmptyXmlTag(tmp))
            {
                value = null;
                return start;
            }

            var startAndEnd = FindIndexEndOfXmlTag(sb, start, tagKeyPair.Tag);

            int len = (startAndEnd.start - 1) - (start + 1) + 1;

            value = sb.SubString(start + 1, len);

            return startAndEnd.end;
        }

        private static (int start, int end) FindIndexEndOfXmlTag(StringBuilder sb, int tmpIndex, string tag)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException(nameof(tag));
            }

            StringBuilder endTagBuilder = new();
            endTagBuilder.Append(XmlConstants.XmlTagOpening);
            endTagBuilder.Append(XmlConstants.XmlTagEnd);
            endTagBuilder.Append(tag);
            endTagBuilder.Append(XmlConstants.XmlTagClosing);


            int numberNewObjects = 0;

            int startIndex = -1;
            int endIndex = -1;

            for (int i = tmpIndex; i < sb.Length; i++)
            {
                char c = sb[i];
                if (c == CommonConstants.Quote)
                {
                    StringBuilder _ = new ();
                    i = ParseMethods.AppendStringValue(_, i, sb.ToString());
                    continue;
                }
                else if (c == XmlConstants.XmlTagOpening)
                {
                    if (sb.EqualsContent(endTagBuilder, i))
                    {
                        if (numberNewObjects == 0)
                        {
                            startIndex = i;
                            // Find end index
                            for (int j = i; j < sb.Length; j++)
                            {
                                char d = sb[j];
                                if (d == CommonConstants.Quote)
                                {
                                    StringBuilder __ = new ();
                                    j = ParseMethods.AppendStringValue(__, j, sb.ToString());
                                    continue;
                                }
                                else if (d == XmlConstants.XmlTagClosing)
                                {
                                    endIndex = j;
                                    break;
                                }
                            }

                            break;
                        }
                        else
                        {
                            numberNewObjects--;
                        }
                    }
                    else
                    {
                        int start = ExtractObjectStart(sb, i, out StringBuilder tmp);

                        if (false == IsClosingXmlTag(tmp) && false == IsEmptyXmlTag(tmp))
                        {
                            var tagKeyPair = ExtractTagAndKey(tmp);

                            if (tagKeyPair.Tag.Equals(tag))
                            {
                                numberNewObjects++;
                            }
                        }

                        i = start;
                    }
                }
            }

            if (-1 == startIndex || -1 == endIndex)
            {
                throw new DSXmlException("Parse: Could not find end of xml tag.");
            }

            return (startIndex, endIndex);
        }

        // internal static StringBuilder RemoveXmlDeclaration(StringBuilder? sb)
        // {
        //     ArgumentNullException.ThrowIfNull(sb);

        //     if (sb.EqualsNullString())
        //     {
        //         return null;
        //     }

        //     return sb;
        // }

        // internal static string? ExtractNodeValue(StringBuilder? sb)
        // {
        //     ArgumentNullException.ThrowIfNull(sb);

        //     if (sb.EqualsNullString())
        //     {
        //         return null;
        //     }

        //     return sb.ToString();
        // }

        /// <summary>
        /// Parses primitive node without a key, e.g "3.14"
        /// </summary>
        /// <param name="sb">Stringbuilder</param>
        /// <param name="startIndex">StartIndex</param>
        /// <param name="key">Key of the node</param>
        /// <returns>Leafnode</returns>

        // internal static IDSNode ParsePrimitiveNode(StringBuilder sb, int startIndex, string key)
        // {
        //     ArgumentNullException.ThrowIfNull(sb);

        //     if (sb.IsNullOrWhiteSpace() || sb.EqualsNullString())
        //     {
        //         return _nodeFactory.CreateNode(key, null, NodeType.Leaf);
        //     }

        //     StringBuilder sbPrim = new();

        //     int i = ParseMethods.AppendStringValue(sbPrim, startIndex, sb.ToString());
        //     if (i != sb.Length -1)
        //     {
        //         throw new DotSerialException("Parse: Can't parse single value.");
        //     }

        //     // Remove opening and closing quote
        //     sbPrim.Remove(0, 1);
        //     sbPrim.Remove(sbPrim.Length - 1, 1);
        //     string nodeValue = sbPrim.ToString();
            
        //     return _nodeFactory.CreateNode(key, nodeValue, NodeType.Leaf);
        // }           

        private static bool IsClosingXmlTag(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (sb[0] != XmlConstants.XmlTagOpening)
            {
                throw new DSXmlException("Parse: Start char is not '<'.");
            }

            if (sb[^1] != XmlConstants.XmlTagClosing)
            {
                throw new DSXmlException("Parse: End char is not '>'.");
            }

            for (int i = 1; i < sb.Length - 1; i++)
            {
                char c = sb[i];
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == XmlConstants.XmlTagEnd)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            throw new DSXmlException("Parse: Unkown error.");
        }

        private static bool IsEmptyXmlTag(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (sb[0] != XmlConstants.XmlTagOpening)
            {
                throw new DSXmlException("Parse: Start char is not '<'.");
            }

            if (sb[^1] != XmlConstants.XmlTagClosing)
            {
                throw new DSXmlException("Parse: End char is not '>'.");
            }

            for (int i = sb.Length - 2; i > 0; i--)
            {
                char c = sb[i];
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == XmlConstants.XmlTagEnd)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
            throw new DSXmlException("Parse: Unkown error.");
        }

        private static XmlTagKeyPair ExtractTagAndKey(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (sb[0] != XmlConstants.XmlTagOpening)
            {
                throw new DSXmlException("Parse: Start char is not '<'.");
            }

            if (sb[^1] != XmlConstants.XmlTagClosing)
            {
                throw new DSXmlException("Parse: End char is not '>'.");
            }

            StringBuilder tagBuilder = new();
            StringBuilder keyBuilder = new();

            // Tag
            bool tagStartFound = false;
            int indexTag = 1;
            for(; indexTag < sb.Length - 1; indexTag++)
            {
                char c = sb[indexTag];
                if (char.IsWhiteSpace(c))
                {
                    if (tagStartFound)
                    {
                        break;
                    }
                }
                else
                {
                    tagStartFound = true;
                    tagBuilder.Append(c);
                }
            }

            // Key
            int indexKey = sb.IndexOf(XmlConstants.XmlAttributeKey);

            if (-1 == indexKey)
            {
                throw new NotImplementedException();
            }

            indexKey += XmlConstants.XmlAttributeKey.Length;

            for (; indexKey < sb.Length - 1; indexKey++)
            {
                char c = sb[indexKey];
                if (char.IsWhiteSpace(c) || c == XmlConstants.XmlAttributeAssignment)
                {
                    continue;
                }
                else if (c == CommonConstants.Quote)
                {
                    _ = ParseMethods.AppendStringValue(keyBuilder, indexKey, sb.ToString());
                    // Remove opening and closing quote
                    keyBuilder.Remove(0, 1);
                    keyBuilder.Remove(keyBuilder.Length - 1, 1);
                    break;
                }
                else
                {
                    throw new DSXmlException("Parse: Invalid key format.");
                }
            }

            var result = new XmlTagKeyPair(tagBuilder.ToString(), keyBuilder.ToString());
            return result;
        }

        private static int ExtractObjectStart(StringBuilder sb, int startIndex, out StringBuilder result)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (sb[startIndex] != XmlConstants.XmlTagOpening)
            {
                throw new DSXmlException("Parse: Start char is not '<'.");
            }

            result = new();
            result.Append(XmlConstants.XmlTagOpening);
            int i = startIndex + 1;
            for (; i < sb.Length; i++)
            {   
                char c = sb[i];
                if (c == CommonConstants.Quote)
                {
                    StringBuilder attValue = new();
                    i = ParseMethods.AppendStringValue(attValue, i, sb.ToString());
                    result.Append(attValue);
                }
                else
                {
                    result.Append(c);

                    if (c == XmlConstants.XmlTagClosing)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}