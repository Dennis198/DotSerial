using System.ComponentModel;
using System.Net.Mail;
using System.Numerics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using DotSerial.Common;
using DotSerial.Tree;
using DotSerial.Tree.Nodes;
using DotSerial.Utilities;

namespace DotSerial.Toon.Parser
{
    internal static  class ToonParserHelper
    {
        /// <summary>Node factory</summary>
        private static readonly NodeFactory _nodeFactory = NodeFactory.Instance;

        internal static Dictionary<string, ToonMulitLineStringBuilder> ExtractKeyValuePairsFromToonObject(ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var result = new Dictionary<string, ToonMulitLineStringBuilder>();
            int objLevel = LineLevel(lines.GetLine(0));

            for (int i = 0; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                var line = lines.GetLine(i);
                int currLevel = LineLevel(line);
                if (currLevel < objLevel)
                {
                    break;
                }

                // "Key":
                if (IsKeyLine(line))
                {
                    string key = ExtractKeyFromLine(line);

                    int sIndex = i + 1;
                    int eIndex = -1;
                    if (sIndex>= lines.Count || currLevel >= LineLevel(lines.GetLine(sIndex)))
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
                    helpObj.KeyLine = line.ToString();

                    result.Add(key, helpObj);
                    i = eIndex;                    

                }
                // "Key": "Value"
                else
                {
                    string key = ExtractKeyFromLine(line);
                    var helpObj = lines.Slice(i, i);                    
                   
                    result.Add(key, helpObj);
                }
            }

            return result;            
        }

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

        internal static List<string?> ExtractPrimitiveListFromLine(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            List<string?> result = [];
            bool seperatorFound = false;

            for(int i = 0; i < line.Length; i++)
            {
                var c = line[i];

                if (c == CommonConstants.Quote)
                {
                    if (seperatorFound)
                    {
                        StringBuilder todo = new();
                        i = ParseMethods.AppendStringValue(todo, i, line);
                        // Remove ending quote
                        todo.Remove(todo.Length - 1, 1);
                        // Remove starting quote
                        todo.Remove(0, 1);

                        result.Add(todo.ToString());
                    }
                    else
                    {
                        i = line.SkipStringValue(i);
                    }
                }
                else if (seperatorFound && c == CommonConstants.N)
                {
                    if (false == line.EqualsNullString(i))
                    {
                        throw new DSToonException("Invalid toon");
                    }

                    i += 3;
                    result.Add(null);
                }
                else if (c == ToonConstants.KeyValueSeperator)
                {
                      if (seperatorFound)
                    {
                        throw new DSToonException("Invalid toon");
                    }
                    seperatorFound = true;
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
                if (c == CommonConstants.Quote)
                {
                    if (keyWasFound)
                    {
                        _ = ParseMethods.AppendStringValue(keyBuilder, i, line);
                        // Remove ending quote
                        keyBuilder.Remove(keyBuilder.Length - 1, 1);
                        // Remove starting quote
                        keyBuilder.Remove(0, 1);
                        
                        return keyBuilder.ToString();
                    }
                    else
                    {
                        i = line.SkipStringValue(i);
                        keyWasFound = true;
                    }
                }
                else if (keyWasFound && c == CommonConstants.N)
                {
                    if (false == line.EqualsNullString(i))
                    {
                        throw new DSToonException("Invalid toon");
                    }
                    
                    return null;
                }
            }

            throw new NotImplementedException();
        }    

        internal static List<ToonMulitLineStringBuilder> ExtractObjectList(ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            var result = new List<ToonMulitLineStringBuilder>();
            int objLevel = LineLevel(lines.GetLine(0));

            for (int i = 0; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                var line = lines.GetLine(i);
                int currLevel = LineLevel(line);
                if (currLevel < objLevel)
                {
                    break;
                }

                // if (true == line.EqualFirstNoWhiteSpaceChar(ToonConstants.ListItemIndicator))
                if (-1 != ParseListCount(line))
                {
                    // "Key":
                    if (IsKeyLine(line))
                    {
                        int sIndex = i + 1;
                        int eIndex = -1;
                        if (sIndex>= lines.Count || currLevel >= LineLevel(lines.GetLine(sIndex)))
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

                        // Remove List indicator of the objects
                        // RemoveListItemIndicator(lines, i, endIndex); TODO AUCH MACHEN????

                        var helpObj = lines.Slice(i, endIndex);
                        result.Add(helpObj);
                        i = endIndex;
                    }
                }
                else if (line.EqualFirstNoWhiteSpaceChar(CommonConstants.Minus))
                {
                    // How to be an object!!
                    // TODO
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

        private static void RemoveListItemIndicator(ToonMulitLineStringBuilder lines)
        {
            var startLine = lines.GetLine(0);
            int index = LineLevel(startLine) * ToonConstants.IndentationSize;

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
                
                if (false == char.IsWhiteSpace(c))
                {
                    if (keyWasFound && valueWasFound)
                    {
                        throw new NotImplementedException();
                    }
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
                else if (keyWasFound && c == CommonConstants.N)
                {
                    if (false == firstLine.EqualsNullString(i))
                    {
                        throw new DSToonException("Invalid toon");
                    }

                    i += 3;
                    valueWasFound = true;
                }
            }

            return keyWasFound && valueWasFound;
        }        

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

        internal static void ParsePrimitiveList(ListNode node, StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(node);
            ArgumentNullException.ThrowIfNull(line);

            int count = ParseListCount(line);
            var gg = ExtractPrimitiveListFromLine(line);

            if (count != gg.Count)
            {
                throw new NotImplementedException();
            }

            for (int i = 0; i < count; i++)
            {
                var listNode = _nodeFactory.CreateNode(i.ToString(), gg[i], NodeType.Leaf);
                node.AddChild(listNode);
            }
        }

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

            if (1 == firstLine.CountQuotedValues())
            {
                return true;
            }

            return false;
        }

        internal static bool IsToonList(ToonMulitLineStringBuilder lines, bool isRootElement = false)
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
                    return true;
                }
                else
                {
                    return false;
                }

                // No Key Line
                // string? key = ExtractKeyFromLine(firstLine);
                // if (string.IsNullOrWhiteSpace(key))
                // {
                //     return true;
                // }
                // else
                // {
                //     if (isRootElement)
                //     {
                //         return false;
                //     }
                //     else
                //     {
                //         return true;
                //     }
                // }
            }




            // var firstLine = lines.GetLine(0);

            // if (null == firstLine)
            // {
            //     throw new NotImplementedException();
            // }

            // // 1. "'key'[0]:"
            // // 2. "[0]:"
            // if (IsEmptyList(firstLine))
            // {
            //     return true;
            // }

            //  TODO TEST, WENN Im Object NUR eine List ist, was dann??

            // Empty List
            // if (-1 != ParseListCount(firstLine))
            // {
            //     string? key = ExtractKeyFromLine(firstLine);

            //     if (string.IsNullOrWhiteSpace(key))
            //     {
            //         return true;
            //     }
            //     else
            //     {
            //         if (isRootElement)
            //         {
            //             return false;
            //         }
            //         else
            //         {
            //             return true;
            //         }
            //     }

            //     // if (lines.Count == 1)
            //     // {
            //     //     string? key = ExtractKeyFromLine(firstLine);
            //     //     if (string.IsNullOrWhiteSpace(key))
            //     //     {
            //     //         return false;
            //     //     }
            //     //     else
            //     //     {
            //     //         return true;
            //     //     }
            //     // }
            //     // else
            //     // {
            //     //     int levelFirst = LineLevel(firstLine);
            //     //     bool ggg = false;
            //     //     for (int i = 1; i < lines.Count; i++)
            //     //     {
            //     //         var level = LineLevel(lines.GetLine(i));
            //     //         if (level == levelFirst)
            //     //         {
            //     //             ggg = true;
            //     //             break;
            //     //         }
            //     //     }
            //     //     // var secondLine = lines.GetLine(1);
            //     //     // int levelSecond = LineLevel(secondLine);

            //     //     return levelFirst != levelSecond;
            //     // }
            // }

            // 1. "- 'item'"
            // 2. "- 'key' : 'item'"
            if (firstLine.EqualFirstNoWhiteSpaceChar(ToonConstants.ListItemIndicator))
            {
                return true;
            }

            return false;
        }

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
                var child = _nodeFactory.CreateNode((i).ToString(), null, NodeType.InnerNode);
                var values = ParseCommaSeperateValues(line, 0);

                if (keys.Count != values.Count)
                {
                    throw new NotImplementedException();
                }

                for (int j = 0; j < keys.Count; j++)
                {
                    string key = keys[j];
                    string value = values[j];
                    var childChild = _nodeFactory.CreateNode(key, value, NodeType.Leaf);
                    child.AddChild(childChild);
                }

                node.AddChild(child);
            }
        }

        internal static List<string> ParseCommaSeperateValues(StringBuilder sb, int startIndex = 0)
        {
            ArgumentNullException.ThrowIfNull(sb);

            List<string> result = [];
            for (int i = startIndex; i < sb.Length; i++)
            {
                char c = sb[i];

                if (c == CommonConstants.Quote)
                {
                    StringBuilder tmp = new();
                    i = ParseMethods.AppendStringValue(tmp, i, sb);
                    // Remove ending quote
                    tmp.Remove(tmp.Length - 1, 1);
                    // Remove starting quote
                    tmp.Remove(0, 1);
                    result.Add(tmp.ToString());
                }
            }

            return result;
        }

        internal static List<string> ParseSchemaKeys(ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (string.IsNullOrWhiteSpace(lines.KeyLine))
            {
                throw new NotImplementedException();
            }

            var firstLine = new StringBuilder(lines.KeyLine);//lines.GetLine(0);

            int start = firstLine.IndexOf("{", 0);

            if (start == -1 || firstLine[start] != CommonConstants.BracesStart)
            {
                throw new NotImplementedException();
            }

            List<string> result = [];

            for (int i = start; i < firstLine.Length; i++)
            {   
                char c = firstLine[i];

                if (c == ToonConstants.KeyValueSeperator)
                {
                    break;
                }
                else if (c == CommonConstants.Quote)
                {
                    StringBuilder tmp = new();
                    i = ParseMethods.AppendStringValue(tmp, i, firstLine);
                    // Remove ending quote
                    tmp.Remove(tmp.Length - 1, 1);
                    // Remove starting quote
                    tmp.Remove(0, 1);
                    result.Add(tmp.ToString());
                }
            }

            return result;
        }

        internal static bool IsSchemaList(ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (string.IsNullOrWhiteSpace(lines.KeyLine))
            {
                return false;
            }

            var firstLine = new StringBuilder(lines.KeyLine);//lines.GetLine(0);
            if (1 > ParseListCount(firstLine))
            {
                return false;
            }

            bool startSchemaFound = false;                        
            bool endSchemaFound = false;   

            for (int i = 0; i < firstLine.Length; i++)
            {
                char c = firstLine[i];

                if(char.IsWhiteSpace(c))
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
            }

            return startSchemaFound && endSchemaFound;
        }

        internal static bool IsEmptyList(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            int count = ParseListCount(line);

            return 0 == count;

            // bool countIndicatorFound =  false;
            // string countAsString = string.Empty;

            // for(int i = 0; i < line.Length - 1; i++)
            // {
            //     char c = line[i];

            //     if (char.IsWhiteSpace(c))
            //     {
            //         continue;
            //     }
            //     else if (c == CommonConstants.Quote)
            //     {
            //         if (true == countIndicatorFound)
            //         {
            //             throw new NotImplementedException();
            //         }
            //         i = line.SkipStringValue(i);
            //     }
            //     else if (c == CommonConstants.BracketsStart)
            //     {
            //         if (true == countIndicatorFound)
            //         {
            //             throw new NotImplementedException();
            //         }
            //         countIndicatorFound = true;
            //     }
            //     else if (c == CommonConstants.BracketsEnd)
            //     {
            //         if (false == countIndicatorFound)
            //         {
            //             throw new NotImplementedException();
            //         }
            //         break;
            //     }
            //     else if (char.IsDigit(c))
            //     {
            //         if (true == countIndicatorFound)
            //         {
            //             countAsString += c;
            //         }
            //     }
            //     else if (c == ToonConstants.KeyValueSeperator)
            //     {
            //         return false;
            //     }
            // }

            // if (false == int.TryParse(countAsString, out int listCount))
            // {
            //     return false;
                
            // }

        }

        internal static int ParseListCount(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            bool countIndicatorStartFound =  false;
            bool countIndicatorEndFound =  false;
            bool firstDigitFromCountFound = false;
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
                        firstDigitFromCountFound = true;
                    }
                }    
                else
                {
                    if (true == countIndicatorStartFound)
                    {
                        throw new NotImplementedException();
                    }
                }            
            }

            if (false == countIndicatorStartFound || false == countIndicatorEndFound)
            {
                return -1;
            }

            // TODO TESTEN OB "22 33" geht?
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


        internal static bool IsEmptyObject(ToonMulitLineStringBuilder lines)
        {
            ArgumentNullException.ThrowIfNull(lines);

            if (lines.Count != 1)
            {
                return false;
            }

            var line = lines.GetLine(0);

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
        /// Returns the indentation level of a line
        /// </summary>
        /// <param name="lines">Stringbuilder-List</param>
        /// <returns>indentation level of a line</returns>
        private static int LineLevel(StringBuilder line)
        {
            ArgumentNullException.ThrowIfNull(line);

            int level = 0;
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == CommonConstants.WhiteSpace)
                {
                    level++;
                }
                else
                {
                    break;
                }
            }

            return level / ToonConstants.IndentationSize;
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
                if (c == CommonConstants.Quote)
                {
                    _ = ParseMethods.AppendStringValue(keyBuilder, i, line);
                    // Remove ending quote
                    keyBuilder.Remove(keyBuilder.Length - 1, 1);
                     // Remove starting quote
                    keyBuilder.Remove(0, 1);
                    return keyBuilder.ToString();
                }
                else if (c == CommonConstants.BracketsStart)
                {
                    return null;
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
            int objLevel = LineLevel(lines.GetLine(startIndex));

            for (int i = startIndex + 1; i < lines.Count; i++)
            {
                // Check if we reached the end of the object
                int currLevel = LineLevel(lines.GetLine(i));
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
