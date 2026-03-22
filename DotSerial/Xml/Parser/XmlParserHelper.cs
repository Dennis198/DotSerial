using DotSerial.Common;
using DotSerial.Utilities;

namespace DotSerial.Xml.Parser
{
    /// <summary>
    /// /// Helper class with methode for parsing xml.
    /// </summary>
    internal static class XmlParserHelper
    {
        /// <summary>
        /// Extracts all key value pairs from an xml object string builder.
        /// </summary>
        /// <param name="content">XMl content</param>
        /// <returns>Dictionary<XmlTagKeyPair, ParserBookmark?> </returns>
        internal static Dictionary<XmlTagKeyPair, ParserBookmark> ExtractKeyValuePairsFromXmlObject(
            ParserBookmark bookmark,
            ReadOnlySpan<char> content
        )
        {
            var tmpContent = bookmark.GetContent(content);
            int offSet = bookmark.Start;

            var result = new Dictionary<XmlTagKeyPair, ParserBookmark>();

            for (int i = 0; i < tmpContent.Length; i++)
            {
                char c = tmpContent[i];

                if (c == XmlConstants.XmlTagOpening)
                {
                    (int start, int end) = ExtractKeyValuePair(tmpContent, i, out XmlTagKeyPair tagKeyPair, out int j);

                    if (end != -1)
                    {
                        var tmpBookmark = new ParserBookmark(start + offSet, end + offSet);
                        result.Add(tagKeyPair, tmpBookmark);
                    }
                    else
                    {
                        var tmpBookmark = new ParserBookmark(start + offSet, -1);
                        result.Add(tagKeyPair, tmpBookmark);
                    }

                    i = j;
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts a key value pair from an xml object string builder.
        /// </summary>
        /// <param name="content">XMl content</param>
        /// <param name="startIndex">StartIndex</param>
        /// <param name="tagKeyPair">XmlTagKeyPair</param>
        /// <param name="value">Extracted Value</param>
        /// <returns>End Index of the object</returns>
        private static (int, int) ExtractKeyValuePair(
            ReadOnlySpan<char> content,
            int startIndex,
            out XmlTagKeyPair tagKeyPair,
            out int endIndex
        )
        {
            int start = ReadOnlySpanMethods.SkipEnclosingValue(
                content,
                startIndex,
                XmlConstants.XmlTagOpening,
                XmlConstants.XmlTagClosing
            );
            var tmp = ReadOnlySpanMethods.SliceFromTo(content, startIndex, start);

            // Extract tag and key
            tagKeyPair = ExtractTagAndKey(tmp);

            // Check for empty tag
            if (IsEmptyXmlTag(tmp))
            {
                endIndex = start;
                return (start, -1);
            }

            // Extract end index of object
            var startAndEnd = FindIndexEndOfXmlTag(content, start, tagKeyPair.Tag);

            // Extract value
            int len = startAndEnd.start - 1 - (start + 1) + 1;
            endIndex = startAndEnd.end;

            if (len == 0)
            {
                return (start + 1, -1);
            }

            return (start + 1, start + len);
        }

        /// <summary>
        /// Extracts the tag and key from an xml object string builder.
        /// </summary>
        /// <param name="content">XMl content</param>
        /// <returns>XmlTagKeyPair</returns>
        private static XmlTagKeyPair ExtractTagAndKey(ReadOnlySpan<char> content)
        {
            if (content[0] != XmlConstants.XmlTagOpening)
            {
                throw new DotSerialException("Parse: Start char is not '<'.");
            }

            if (content[^1] != XmlConstants.XmlTagClosing)
            {
                throw new DotSerialException("Parse: End char is not '>'.");
            }

            string tagBuilder = string.Empty;
            string keyBuilder = string.Empty;

            // Tag
            bool tagStartFound = false;
            int indexTag = 1;
            for (; indexTag < content.Length - 1; indexTag++)
            {
                char c = content[indexTag];
                if (char.IsWhiteSpace(c))
                {
                    if (tagStartFound)
                    {
                        var tmp = ReadOnlySpanMethods.SliceFromTo(content, 1, indexTag - 1);
                        tagBuilder = tmp.ToString();
                        break;
                    }
                }
                else
                {
                    tagStartFound = true;
                }
            }

            // Key
            int indexKey = content.IndexOf(XmlConstants.XmlAttributeKey);

            if (-1 == indexKey)
            {
                throw new NotImplementedException();
            }

            indexKey += XmlConstants.XmlAttributeKey.Length;

            for (; indexKey < content.Length - 1; indexKey++)
            {
                char c = content[indexKey];
                if (char.IsWhiteSpace(c) || c == XmlConstants.XmlAttributeAssignment)
                {
                    continue;
                }
                else if (c == CommonConstants.Quote)
                {
                    int j = ReadOnlySpanMethods.SkipQuotedValue(content, indexKey);
                    var tmp = ReadOnlySpanMethods.SliceFromTo(content, indexKey, j);
                    keyBuilder = tmp.ToString();
                    break;
                }
                else
                {
                    throw new DotSerialException("Parse: Invalid key format.");
                }
            }

            var result = new XmlTagKeyPair(tagBuilder, keyBuilder);
            return result;
        }

        /// <summary>
        /// Finds the end index of an xml tag.
        /// </summary>
        /// <param name="content">XMl content</param>
        /// <param name="indexStartSearch">Index to start searching/param>
        /// <param name="tag"></param>
        /// <returns>(Start index of end tag and end index of end tag)</returns>
        private static (int start, int end) FindIndexEndOfXmlTag(
            ReadOnlySpan<char> content,
            int indexStartSearch,
            string tag
        )
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException(nameof(tag));
            }

            var endTagBuilder2 =
                $"{XmlConstants.XmlTagOpening}{XmlConstants.XmlTagEnd}{tag}{XmlConstants.XmlTagClosing}".AsSpan();

            // Count new objects
            int numberNewObjects = 0;

            int startIndex = -1;
            int endIndex = -1;

            for (int i = indexStartSearch; i < content.Length; i++)
            {
                char c = content[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                // Skip quoted values
                if (c == CommonConstants.Quote)
                {
                    i = ReadOnlySpanMethods.SkipQuotedValue(content, i);
                    continue;
                }
                else if (c == XmlConstants.XmlTagOpening)
                {
                    // Check for the end tag
                    if (content.Slice(i, endTagBuilder2.Length).SequenceEqual(endTagBuilder2))
                    {
                        if (numberNewObjects == 0)
                        {
                            startIndex = i;

                            // Find end index
                            for (int j = i; j < content.Length; j++)
                            {
                                char d = content[j];
                                if (d == CommonConstants.Quote)
                                {
                                    j = ReadOnlySpanMethods.SkipQuotedValue(content, j);
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
                        int j = ReadOnlySpanMethods.SkipEnclosingValue(
                            content,
                            i,
                            XmlConstants.XmlTagOpening,
                            XmlConstants.XmlTagClosing
                        );
                        var tmp = ReadOnlySpanMethods.SliceFromTo(content, i, j);

                        // Check if it is a closing or empty tag
                        if (false == IsClosingXmlTag(tmp) && false == IsEmptyXmlTag(tmp))
                        {
                            var tagKeyPair = ExtractTagAndKey(tmp);

                            // Check if it is the same tag
                            if (tagKeyPair.Tag.Equals(tag))
                            {
                                numberNewObjects++;
                            }
                        }

                        // Move index to end of extracted object
                        i = j;
                    }
                }
            }

            if (-1 == startIndex || -1 == endIndex)
            {
                throw new DotSerialException("Parse: Could not find end of xml tag.");
            }

            return (startIndex, endIndex);
        }

        /// <summary>
        /// Checks if the string builder is a closing xml tag.
        /// </summary>
        /// <param name="content">XMl content</param>
        /// <returns>True, if closing tag</returns>
        private static bool IsClosingXmlTag(ReadOnlySpan<char> content)
        {
            if (content[0] != XmlConstants.XmlTagOpening)
            {
                throw new DotSerialException("Parse: Start char is not '<'.");
            }

            if (content[^1] != XmlConstants.XmlTagClosing)
            {
                throw new DotSerialException("Parse: End char is not '>'.");
            }

            for (int i = 1; i < content.Length - 1; i++)
            {
                char c = content[i];
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

            throw new DotSerialException("Parse: Unkown error.");
        }

        /// <summary>
        /// Checks if the string builder is an empty xml tag.
        /// </summary>
        /// <param name="content">XMl content</param>
        /// <returns>True, if emtpy tag</returns>
        private static bool IsEmptyXmlTag(ReadOnlySpan<char> content)
        {
            if (content[0] != XmlConstants.XmlTagOpening)
            {
                throw new DotSerialException("Parse: Start char is not '<'.");
            }

            if (content[^1] != XmlConstants.XmlTagClosing)
            {
                throw new DotSerialException("Parse: End char is not '>'.");
            }

            for (int i = content.Length - 2; i > 0; i--)
            {
                char c = content[i];
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

            throw new DotSerialException("Parse: Unkown error.");
        }
    }

    /// <summary>
    /// Class representing a tag key pair in xml.
    /// </summary>
    internal class XmlTagKeyPair
    {
        internal string Key;
        internal string Tag;

        internal XmlTagKeyPair(string tag, string key)
        {
            Tag = tag;
            Key = key;
        }

        /// <summary>
        /// Checks if the tag key pair represents an xml list.
        /// </summary>
        /// <returns>True, if list</returns>
        internal bool IsXmlList()
        {
            return Tag == XmlConstants.XmlListProp;
        }

        /// <summary>
        /// Checks if the tag key pair represents an xml object.
        /// </summary>
        /// <returns>True, if object</returns>
        internal bool IsXmlObject()
        {
            return Tag == XmlConstants.XmlInnerNodeProp;
        }

        /// <summary>
        /// Checks if the tag key pair represents an xml primitive.
        /// </summary>
        /// <returns>True, if primitive</returns>
        internal bool IsXmlPrimitive()
        {
            return Tag == XmlConstants.XmlLeafProp;
        }
    }
}
