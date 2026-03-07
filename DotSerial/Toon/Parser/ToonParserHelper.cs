using System.Text;
using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Toon.Parser
{
    /// <summary>
    /// Helper class with methode for parsing toon.
    /// </summary>
    internal static  class ToonParserHelper
    {

        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        /// <summary>
        /// Extracts key value pairs from yaml object
        /// </summary>
        /// <param name="lines">ToonMulitLineStringBuilder</param>
        /// <returns>Dictionary<string, ToonMulitLineStringBuilder></returns>
        internal static Dictionary<string, ToonMulitLineStringBuilder> ExtractKeyValuePairsFromToonObject(ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var result = new Dictionary<string, ToonMulitLineStringBuilder>();
            int objLevel = ParseMethods.LineLevel(lines.GetLine(0), ToonConstants.IndentationSize);

            for (int i = 0; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                var line = lines.GetLine(i);
                int currLevel = ParseMethods.LineLevel(line, ToonConstants.IndentationSize);
                if (currLevel < objLevel)
                {
                    break;
                }

                // "Key":
                if (IsKeyLine(line))
                {
                    string? key = ExtractKeyFromLine(line);

                    if  (string.IsNullOrWhiteSpace(key))
                    {
                        throw new DSToonException("Invalid toon");
                    }

                    int sIndex = i + 1;
                    int eIndex;
                    if (sIndex>= lines.Count || currLevel >= ParseMethods.LineLevel(lines.GetLine(sIndex), ToonConstants.IndentationSize))
                    {
                        sIndex = i;
                        eIndex = i;
                    }
                    else 
                    {                
                        eIndex = GetEndIndexOfToonObject(lines, i);
                    }

                    var helpObj = lines.Slice(sIndex, eIndex);
                    
                    if (sIndex == eIndex && !IsToonList(helpObj))
                    {
                        // Special case an object with exaclty one item.
                        // Must be marked, otherwise there is no way to
                        // differentiated between an object or a simple
                        // Key, Value pair for an primitive.
                        helpObj.IsOneLineObject = true;
                    }

                    // Set Key line
                    // Needed for list parsing
                    helpObj.KeyLine = line.ToString();

                    result.Add(key, helpObj);
                    i = eIndex;                    

                }
                // "Key": "Value"
                else
                {
                    string? key = ExtractKeyFromLine(line);

                    if  (string.IsNullOrWhiteSpace(key))
                    {
                        throw new DSToonException("Invalid toon");
                    }

                    var helpObj = lines.Slice(i, i);                    
                   
                    result.Add(key, helpObj);
                }
            }

            return result;            
        }

        /// <summary>
        /// Check if ToonMulitLineStringBuilder is a toon object
        /// </summary>
        /// <param name="lines">MultiLineStringBuilder</param>
        /// <returns>True, if yaml object</returns>
        internal static bool IsToonObject(ToonMulitLineStringBuilder lines, bool isRootElement = false)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var firstLine = lines.GetLine(0);

            if (null == firstLine)
            {
                throw new NotImplementedException();
            }

            // "'key':"
            if (IsEmptyObject(lines))
            {
                return true;
            }

            if (IsToonList(lines, isRootElement))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Parses a primitive list from a line
        /// </summary>
        /// <param name="line">StringBuilder</param>
        /// <returns>Values in a list</returns>
        private static List<string?> ExtractPrimitiveListFromLine(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            List<string?> result = [];
            bool seperatorFound = false;

            for(int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (char.IsWhiteSpace(c) || c == CommonConstants.Comma)
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    if (seperatorFound)
                    {
                        StringBuilder tmp = new();
                        i = ParseMethods.AppendStringValue(tmp, i, line);

                        result.Add(tmp.ToString());
                    }
                    else
                    {
                        i = line.SkipStringValue(i);
                    }
                }
                else if (c == ToonConstants.KeyValueSeperator)
                {
                    if (seperatorFound)
                    {
                        throw new DSToonException("Invalid toon");
                    }
                    seperatorFound = true;
                }
                else
                {
                    if (seperatorFound)
                    {
                        StringBuilder sb2 = new();
                        i = ParseMethods.AppendTillStopChars(sb2, i, line, [CommonConstants.Comma]);
                        result.Add(sb2.ToString());
                    }
                    else
                    {
                        i = line.SkipTillStopChars(i, [CommonConstants.Comma, ToonConstants.KeyValueSeperator]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts the value from a line
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <returns>Value of the line</returns>
        internal static string? ExtractValueFromLine(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            StringBuilder keyBuilder = new();
            bool keyWasFound = false;

            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    if (keyWasFound)
                    {
                        _ = ParseMethods.AppendStringValue(keyBuilder, i, line);
                        return keyBuilder.ToString();
                    }
                    else
                    {
                        i = line.SkipStringValue(i);
                        keyWasFound = true;
                    }
                }
                else if (c == ToonConstants.KeyValueSeperator)
                {
                    if (!keyWasFound)
                    {
                        throw new DSToonException("Invalid toon");
                    }
                }
                else
                {
                    if (keyWasFound)
                    {
                        _ = ParseMethods.AppendTillStopChar(keyBuilder, i, line, null);
                        return keyBuilder.ToString();
                    }
                    else
                    {
                        i = line.SkipTillStopChar(i, ToonConstants.KeyValueSeperator);
                        keyWasFound = true;
                    }
                }
            }

            throw new NotImplementedException();
        }    

        /// <summary>
        /// Extracts list of objcts from toon string
        /// </summary>
        /// <param name="lines">ToonMulitLineStringBuilder</param>
        /// <returns>List<ToonMulitLineStringBuilder></returns>
        internal static List<ToonMulitLineStringBuilder> ExtractObjectList(ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var result = new List<ToonMulitLineStringBuilder>();
            int objLevel = ParseMethods.LineLevel(lines.GetLine(0), ToonConstants.IndentationSize);

            for (int i = 0; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                var line = lines.GetLine(i);
                int currLevel = ParseMethods.LineLevel(line, ToonConstants.IndentationSize);
                if (currLevel < objLevel)
                {
                    break;
                }

                if (-1 != ParseListCount(line))
                {
                    // "Key":
                    if (IsKeyLine(line))
                    {
                        int sIndex = i + 1;
                        int eIndex;
                        if (sIndex>= lines.Count || currLevel >= ParseMethods.LineLevel(lines.GetLine(sIndex), ToonConstants.IndentationSize))
                        {
                            sIndex = i;
                            eIndex = i;
                        }
                        else 
                        {                
                            eIndex = GetEndIndexOfToonObject(lines, i);
                        }

                        var helpObj = lines.Slice(sIndex, eIndex);
                         // Set Key line
                        helpObj.KeyLine = line.ToString();

                        result.Add(helpObj);
                        i = eIndex; 
                    }
                    else
                    {
                        int endIndex = GetEndIndexOfToonObject(lines, i);
                        var helpObj = lines.Slice(i, endIndex);
                        result.Add(helpObj);
                        i = endIndex;
                    }
                }
                else if (line.EqualFirstNoWhiteSpaceChar(CommonConstants.Minus))
                {
                    // Has no list count, => Must be an object

                    int endIndex = GetEndIndexOfToonObject(lines, i);
                    var helpObj = lines.Slice(i, endIndex);

                    if (i == endIndex)
                    {
                        // Special case an object with exaclty one item.
                        // Must be marked, otherwise there is no way to
                        // differentiated between an object or a simple
                        // Key, Value pair for an primitive.
                        helpObj.IsOneLineObject = true;
                    }

                    RemoveListItemIndicator(helpObj);
                    result.Add(helpObj);
                    i = endIndex;
                }
                
            }        

            return result;
        }        

       /// <summary>
        /// Removes the List indicator for the objects
        /// </summary>
        /// <param name="lines">ToonMulitLineStringBuilder</param>
        private static void RemoveListItemIndicator(ToonMulitLineStringBuilder lines)
        {
            var startLine = lines.GetLine(0);
            int index = ParseMethods.LineLevel(startLine, ToonConstants.IndentationSize) * ToonConstants.IndentationSize;

            if (startLine[index] != ToonConstants.ListItemIndicator)
            {
                throw new NotImplementedException();
            }

            startLine.Remove(index, 1);
        }        

        /// <summary>
        /// Check if ToonMulitLineStringBuilder is a key only a key value pair
        /// </summary>
        /// <param name="lines">ToonMulitLineStringBuilder</param>
        /// <returns>True, if yaml key value pair</returns>
        internal static bool IsToonPrimitiveLine(ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count != 1)
            {
                return false;
            }

            if (lines.IsOneLineObject)
            {
                return false;
            }

            if (IsToonList(lines))
            {
                return false;
            }

            if (IsToonSingleValue(lines))
            {
                return false;
            }

            var firstLine = lines.GetLine(0);
            bool keyWasFound = false;
            bool valueWasFound =  false;

            for (int i = 0; i < firstLine.Length; i++)
            {
                var c = firstLine[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }            

                if (c == CommonConstants.Quote)
                {
                    if (keyWasFound)
                    {
                        i = firstLine.SkipStringValue(i);                        
                        valueWasFound = true;
                    }
                    else
                    {
                        i = firstLine.SkipStringValue(i);   
                        keyWasFound = true;
                    }
                }
                else
                {
                    if (keyWasFound)
                    {
                        i = firstLine.SkipTillStopChar(i, null);                        
                        valueWasFound = true;
                    }
                    else
                    {
                        i = firstLine.SkipTillStopChar(i, ToonConstants.KeyValueSeperator);   
                        keyWasFound = true;
                    }
                }
            }

            return keyWasFound && valueWasFound;
        }        

        /// <summary>
        /// Check if string is a primitive list
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        internal static bool IsPrimitiveList(ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count != 1)
            {
                return false;
            }

            var firstLine = lines.GetLine(0);

            if (firstLine.EqualLastNoWhiteSpaceChar(ToonConstants.KeyValueSeperator))
            {
                return false;
            }

            if (0 < ParseListCount(firstLine))
            {
                return true;
            }
            else
            {
                return false;
            }
        } 

        /// <summary>
        /// Parse primitive list
        /// </summary>
        /// <param name="node">Listnode</param>
        /// <param name="line">StringBuilder</param>
        internal static void ParsePrimitiveList(ListNode node, StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(line);

            int count = ParseListCount(line);
            var lItems = ExtractPrimitiveListFromLine(line);

            if (count != lItems.Count)
            {
                throw new NotImplementedException();
            }

            for (int i = 0; i < count; i++)
            {
                var listNode = _nodeFactory.CreateNodeFromString(StategyType.Toon, i.ToString(), lItems[i], NodeType.Leaf);
                node.AddChild(listNode);
            }
        }

        /// <summary>
        /// Check if ToonMulitLineStringBuilder is a single value
        /// </summary>
        /// <param name="lines">ToonMulitLineStringBuilder</param>
         /// <returns>True, if toon single value</returns>
        internal static bool IsToonSingleValue(ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count != 1)
            {
                return false;
            }

            var firstLine = lines.GetLine(0);

            // "null"
            if (firstLine.EqualsNullString())
            {
                return true;
            }

            // "'Key': "
            if (IsKeyLine(firstLine))
            {
                return false;
            }            

            bool seperatorFound = false;
            bool keyFound = false;

            for (int i = 0; i < firstLine.Length; i++)
            {
                char c = firstLine[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (true == keyFound && c == ToonConstants.KeyValueSeperator)
                {
                    seperatorFound = true;
                    continue;
                }

                if (seperatorFound)
                {
                    return false;
                }

                if (c == CommonConstants.Quote)
                {
                    i = firstLine.SkipStringValue(i);
                    keyFound = true;
                }
                else
                {
                    i = firstLine.SkipTillStopChar(i, ToonConstants.KeyValueSeperator);
                    keyFound = true;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if ToonMulitLineStringBuilder is a list
        /// </summary>
        /// <param name="lines">ToonMulitLineStringBuilder</param>
        /// <returns>True, if list</returns>
        internal static bool IsToonList(ToonMulitLineStringBuilder lines, bool rootElement = false)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var firstLine = lines.GetLine(0);

            if (false == string.IsNullOrWhiteSpace(lines.KeyLine))
            {
                if (-1 != ParseListCount(new StringBuilder(lines.KeyLine)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (lines.Count != 1)
                {
                    return false;
                }

                if (-1 != ParseListCount(firstLine))
                {                    
                    if(rootElement)
                    {
                        if (null == ExtractKeyFromLine(firstLine))
                        {
                            return true;
                        }
                        else
                        {
                            // Root element must be a list, otherwise there would
                            // be no way to differentiate between a list and an object with one item.
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Parses a toon schema list and adds the objects to the node
        /// </summary>
        /// <param name="node">ListNode</param>
        /// <param name="lines">ToonMulitLineStringBuilder</param>
        internal static void ParseSchemaList(ListNode node, ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(lines);

            var keys = ParseSchemaKeys(lines);

            if (keys.Count == 0)
            {
                throw new NotImplementedException();
            }

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines.GetLine(i);
                var child = _nodeFactory.CreateNodeFromString(StategyType.Toon, i.ToString(), null, NodeType.InnerNode);
                var values = ParseCommaSeperateValues(line, 0);

                if (keys.Count != values.Count)
                {
                    throw new NotImplementedException();
                }

                for (int j = 0; j < keys.Count; j++)
                {
                    string key = keys[j];
                    string value = values[j];
                    var childChild = _nodeFactory.CreateNodeFromString(StategyType.Toon, key, value, NodeType.Leaf);
                    child.AddChild(childChild);
                }

                node.AddChild(child);
            }
        }

        /// <summary>
        /// Parses values which are sepearted by comma.
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <param name="startIndex">StartIndex for parsing</param>
        /// <returns>List of the values</returns>
        internal static List<string> ParseCommaSeperateValues(StringBuilder sb, int startIndex = 0)
        {
            ArgumentNullException.ThrowIfNull(sb);

            List<string> result = [];
            for (int i = startIndex; i < sb.Length; i++)
            {
                char c = sb[i];

                if (char.IsWhiteSpace(c) || c == CommonConstants.Comma)
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    StringBuilder tmp = new();
                    i = ParseMethods.AppendStringValue(tmp, i, sb);
                    result.Add(tmp.ToString());
                }
                else
                {
                    StringBuilder tmp = new();
                    i = ParseMethods.AppendTillStopChar(tmp, i, sb, CommonConstants.Comma);
                    result.Add(tmp.ToString());
                }
            }

            return result;
        }

        /// <summary>
        /// Parses the schema keys from the key line of a toon schema list
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private static List<string> ParseSchemaKeys(ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (string.IsNullOrWhiteSpace(lines.KeyLine))
            {
                throw new NotImplementedException();
            }

            var firstLine = new StringBuilder(lines.KeyLine);

            int start = firstLine.IndexOf(CommonConstants.BracesStart.ToString(), 0);

            if (start == -1 || firstLine[start] != CommonConstants.BracesStart)
            {
                throw new NotImplementedException();
            }

            List<string> result = [];

            for (int i = start + 1; i < firstLine.Length; i++)
            {   
                char c = firstLine[i];

                if (char.IsWhiteSpace(c) || c == CommonConstants.Comma)
                {
                    continue;
                }

                if (c == ToonConstants.KeyValueSeperator || c == CommonConstants.BracesEnd)
                {
                    break;
                }
                else if (c == CommonConstants.Quote)
                {
                    StringBuilder tmp = new();
                    i = ParseMethods.AppendStringValue(tmp, i, firstLine);
                    result.Add(tmp.ToString());
                }
                else
                {
                    StringBuilder tmp = new();
                    i = ParseMethods.AppendTillStopChars(tmp, i, firstLine, [CommonConstants.Comma, CommonConstants.BracesStart, CommonConstants.BracesEnd]);
                    result.Add(tmp.ToString());

                }
            }

            return result;
        }

        /// <summary>
        /// Check if ToonMulitLineStringBuilder is a toon schema list
        /// </summary>
        /// <param name="lines">ToonMulitLineStringBuilder</param>
        /// <returns>True, if list wioth schema</returns>
        internal static bool IsSchemaList(ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (string.IsNullOrWhiteSpace(lines.KeyLine))
            {
                return false;
            }

            var firstLine = new StringBuilder(lines.KeyLine);
            if (1 > ParseListCount(firstLine))
            {
                return false;
            }

            bool startSchemaFound = false;                        
            bool endSchemaFound = false;   

            for (int i = 0; i < firstLine.Length; i++)
            {
                char c = firstLine[i];

                if(char.IsWhiteSpace(c) || c == CommonConstants.Comma)
                {
                    continue;
                }

                if (c == ToonConstants.KeyValueSeperator)
                {
                    break;
                }
                else if (c == CommonConstants.Quote)
                {
                    i = firstLine.SkipStringValue(i);
                }
                else if (c == CommonConstants.BracesStart)
                {
                    if (startSchemaFound)
                    {
                        throw new NotImplementedException();
                    }
                    startSchemaFound = true;
                }
                else if (c == CommonConstants.BracesEnd)
                {
                    if (false == startSchemaFound)
                    {
                        throw new NotImplementedException();
                    }
                    endSchemaFound = true;
                }
                else
                {
                    i = firstLine.SkipTillStopChars(i, [CommonConstants.Comma, CommonConstants.BracesStart, CommonConstants.BracesEnd]);
                }
            }

            return startSchemaFound && endSchemaFound;
        }

        /// <summary>
        /// Check if String is a empty toon list.
        /// </summary>
        /// <param name="line">StringBuilder</param>
        /// <returns>true, if empty toon list</returns>
        internal static bool IsEmptyList(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            int count = ParseListCount(line);

            return 0 == count;
        }

        /// <summary>
        /// Parses the list count of a toon list from a string
        /// </summary>
        /// <param name="line">StringBuilder</param>
        /// <returns>Count, if the toon list count indicator is there, -1 otherwise.</returns>
        internal static int ParseListCount(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            bool countIndicatorStartFound =  false;
            bool countIndicatorEndFound =  false;
            string countAsString = string.Empty;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c == CommonConstants.Quote)
                {
                    if (true == countIndicatorStartFound)
                    {
                        throw new NotImplementedException();
                    }
                    i = line.SkipStringValue(i);
                }
                else if (c == CommonConstants.BracketsStart)
                {
                    if (true == countIndicatorStartFound)
                    {
                        throw new NotImplementedException();
                    }
                    countIndicatorStartFound = true;
                }
                else if (c == CommonConstants.BracketsEnd)
                {
                    if (false == countIndicatorStartFound)
                    {
                        throw new NotImplementedException();
                    }
                    countIndicatorEndFound = true;
                    break;
                }
                else if (char.IsDigit(c))
                {
                    if (true == countIndicatorStartFound)
                    {
                        countAsString += c;
                    }
                    else
                    {
                        i = line.SkipTillStopChars(i, [ToonConstants.KeyValueSeperator, CommonConstants.BracketsStart]);
                    }
                }    
                else if (c == ToonConstants.KeyValueSeperator)
                {
                    break;
                }
                else
                {
                    if (true == countIndicatorStartFound)
                    {
                        throw new NotImplementedException();
                    }

                    i = line.SkipTillStopChars(i, [ToonConstants.KeyValueSeperator, CommonConstants.BracketsStart]);
                }            
            }

            if (false == countIndicatorStartFound || false == countIndicatorEndFound)
            {
                return -1;
            }

            if (false == int.TryParse(countAsString, out int listCount))
            {
                return -1;
            }

            if (listCount < 0)
            {
                return -1;
            }

            return listCount;
        }

        /// <summary>
        /// Check if ToonMulitLineStringBuilder is an empty toon object.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>True, if empty objcet</returns>
        internal static bool IsEmptyObject(ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count != 1)
            {
                return false;
            }

            var line = lines.GetLine(0);

            if (-1 != ParseListCount(line))
            {
                return false;
            }

            if (false == line.EqualLastNoWhiteSpaceChar(ToonConstants.KeyValueSeperator))
            {
                return false;
            }

            bool seperatorFound = false;

            for (int i = line.Length - 1; i >= 0; i--)
            {   
                char c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else if (c ==  ToonConstants.KeyValueSeperator)
                {
                    if (true == seperatorFound)
                    {
                        throw new NotImplementedException();
                    }
                    seperatorFound = true;
                }
                else if (c == CommonConstants.BracketsEnd)
                {
                    return false;
                }
                else if (c == CommonConstants.BracesEnd)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if line is a key line
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <returns>True, if line is key line</returns>
        private static bool IsKeyLine(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            return line.EqualLastNoWhiteSpaceChar(ToonConstants.KeyValueSeperator);             
        }                   

        /// <summary>
        /// Extracts the key from a line
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <returns>Key of the line</returns>
        private static string? ExtractKeyFromLine(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            StringBuilder keyBuilder = new();
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];

                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                if (c == CommonConstants.Quote)
                {
                    _ = ParseMethods.AppendStringValue(keyBuilder, i, line);
                    return keyBuilder.ToString();
                }
                else if (c == CommonConstants.BracketsStart)
                {
                    return null;
                }
                else
                {
                    _ = ParseMethods.AppendTillStopChars(keyBuilder, i, line, [ToonConstants.KeyValueSeperator, CommonConstants.BracketsStart]);
                    return keyBuilder.ToString();
                }
            }

            throw new NotImplementedException();
        }          

        /// <summary>
        /// Gets the end index of an yaml object.
        /// </summary>
        /// <param name="lines">ToonMulitLineStringBuilder</param>
        /// <param name="startIndex">Start index of the object</param>
        /// <returns>End index</returns>
        private static int GetEndIndexOfToonObject(ToonMulitLineStringBuilder lines, int startIndex)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count - 1 < startIndex)
            {
                throw new NotImplementedException();
            }

            int endIndex = -1;
            int objLevel = ParseMethods.LineLevel(lines.GetLine(startIndex), ToonConstants.IndentationSize);

            for (int i = startIndex + 1; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                int currLevel = ParseMethods.LineLevel(lines.GetLine(i), ToonConstants.IndentationSize);
                if (currLevel <= objLevel)
                {
                    endIndex = i - 1;
                    break;
                }
            }

            if (endIndex == -1)
            {
                endIndex = lines.Count - 1;
            }

            return endIndex;
        }                    
    }
}
