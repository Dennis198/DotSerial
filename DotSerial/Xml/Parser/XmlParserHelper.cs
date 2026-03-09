using System.Text;
using DotSerial.Common;
using DotSerial.Utilities;

namespace DotSerial.Xml.Parser
{
    /// <summary>
    /// Class representing a tag key pair in xml.
    /// </summary>
    internal class XmlTagKeyPair
    {
        internal string Tag;
        internal string Key;

        internal XmlTagKeyPair(string tag, string key)
        {
            Tag = tag;
            Key = key;
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
        /// Checks if the tag key pair represents an xml list.
        /// </summary>
        /// <returns>True, if list</returns>
        internal bool IsXmlList()
        {
            return Tag == XmlConstants.XmlListProp;
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

    /// <summary>
    /// Helper class with methode for parsing xml.
    /// </summary>
    internal static class XmlParserHelper
    {
        /// <summary>
        /// Extracts all key value pairs from an xml object string builder.
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <returns>Dictionary<XmlTagKeyPair, StringBuilder?> </returns>
        internal static Dictionary<XmlTagKeyPair, StringBuilder?> ExtractKeyValuePairsFromXmlObject(StringBuilder sb)
        {
            ArgumentNullException.ThrowIfNull(sb);

            var result = new Dictionary<XmlTagKeyPair, StringBuilder?>();

            for (int i = 0; i < sb.Length; i++)
            {
                char c = sb[i];

                if (c == XmlConstants.XmlTagOpening)
                {
                    i = ExtractKeyValuePair(sb, i, out XmlTagKeyPair tagKeyPair, out StringBuilder? value);
                    result.Add(tagKeyPair, value);
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts a key value pair from an xml object string builder.
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="startIndex">StartIndex</param>
        /// <param name="tagKeyPair">XmlTagKeyPair</param>
        /// <param name="value">Extracted Value</param>
        /// <returns>End Index of the object</returns>
        private static int ExtractKeyValuePair(
            StringBuilder sb,
            int startIndex,
            out XmlTagKeyPair tagKeyPair,
            out StringBuilder? value
        )
        {
            ArgumentNullException.ThrowIfNull(sb);

            // Extract opening tag
            StringBuilder tmp = new();
            int start = ParseMethods.AppendEnclosingValue(
                tmp,
                startIndex,
                sb,
                XmlConstants.XmlTagOpening,
                XmlConstants.XmlTagClosing
            );

            // Extract tag and key
            tagKeyPair = ExtractTagAndKey(tmp);

            // Check for empty tag
            if (IsEmptyXmlTag(tmp))
            {
                value = null;
                return start;
            }

            // Extract end index of object
            var startAndEnd = FindIndexEndOfXmlTag(sb, start, tagKeyPair.Tag);

            // Extract value
            int len = startAndEnd.start - 1 - (start + 1) + 1;
            value = sb.SubString(start + 1, len);

            return startAndEnd.end;
        }

        /// <summary>
        /// Finds the end index of an xml tag.
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="indexStartSearch">Index to start searching/param>
        /// <param name="tag"></param>
        /// <returns>(Start index of end tag and end index of end tag)</returns>
        private static (int start, int end) FindIndexEndOfXmlTag(StringBuilder sb, int indexStartSearch, string tag)
        {
            ArgumentNullException.ThrowIfNull(sb);

            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException(nameof(tag));
            }

            // Build end tag
            StringBuilder endTagBuilder = new();
            endTagBuilder.Append(XmlConstants.XmlTagOpening);
            endTagBuilder.Append(XmlConstants.XmlTagEnd);
            endTagBuilder.Append(tag);
            endTagBuilder.Append(XmlConstants.XmlTagClosing);

            // Count new objects
            int numberNewObjects = 0;

            int startIndex = -1;
            int endIndex = -1;

            for (int i = indexStartSearch; i < sb.Length; i++)
            {
                char c = sb[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                // Skip quoted values
                if (c == CommonConstants.Quote)
                {
                    i = sb.SkipStringValue(i);
                    continue;
                }
                else if (c == XmlConstants.XmlTagOpening)
                {
                    // Check for the end tag
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
                                    j = sb.SkipStringValue(j);
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
                        // Extract full object to check its tag
                        StringBuilder tmp = new();
                        int start = ParseMethods.AppendEnclosingValue(
                            tmp,
                            i,
                            sb,
                            XmlConstants.XmlTagOpening,
                            XmlConstants.XmlTagClosing
                        );

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

        /// <summary>
        /// Removes all whitespace from an xml string, except for whitespace within quoted strings.
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>Xml String without whitespaces.</returns>
        internal static StringBuilder RemoveWhiteSpaceXmlString(string str)
        {
            // Check if value has value
            if (string.IsNullOrWhiteSpace(str))
            {
                return new StringBuilder();
            }

            StringBuilder sb = new();
            int stringLength = str.Length;
            StringBuilder strAsStringBuilder = new(str);

            for (int i = 0; i < stringLength; i++)
            {
                var c = str[i];

                if (c == CommonConstants.Quote)
                {
                    i = ParseMethods.AppendStringValue(sb, i, strAsStringBuilder);
                }
                else if (c == XmlConstants.XmlTagOpening)
                {
                    i = ParseMethods.AppendEnclosingValue(
                        sb,
                        i,
                        strAsStringBuilder,
                        XmlConstants.XmlTagOpening,
                        XmlConstants.XmlTagClosing
                    );
                }
                else
                {
                    i = ParseMethods.AppendTillStopChar(sb, i, strAsStringBuilder, XmlConstants.XmlTagClosing);
                }
            }

            return sb;
        }

        /// <summary>
        /// Checks if the string builder is a closing xml tag.
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <returns>True, if closing tag</returns>
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

        /// <summary>
        /// Checks if the string builder is an empty xml tag.
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <returns>True, if emtpy tag</returns>
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

        /// <summary>
        /// Extracts the tag and key from an xml object string builder.
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <returns>XmlTagKeyPair</returns>
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
            for (; indexTag < sb.Length - 1; indexTag++)
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
                    _ = ParseMethods.AppendStringValue(keyBuilder, indexKey, sb);
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
    }
}
