using System.Text;
using DotSerial.Tree.Nodes;

namespace DotSerial.YAML.Writer
{
    public class YamlWriterVisitor : IYamlNodeWriterVisitor
    {
        /// <inheritdoc/>
        public static string Write(DSYamlNode node)
        {
            ArgumentNullException.ThrowIfNull(node);

            StringBuilder sb = new();

            // Add document start
            sb.Append(YAMLConstants.YAMLDocumentStart);

            var internalNode = node.GetInternalData();
            WriterAccept(internalNode, new YamlWriterVisitor(), sb, new YamlWriterOptions(0, false));

            // Add document end
            sb.AppendLine();
            sb.Append(YAMLConstants.YAMLDocumentEnd);

            return sb.ToString();
        }

        /// <summary>
        /// Writer for yaml
        /// </summary>
        /// <param name="node">IDSNode</param>
        /// <param name="visitor">Visitor</param>
        /// <param name="sb">Stringbuild</param>
        /// <param name="options">Additional options</param>
        private static void WriterAccept(IDSNode node, YamlWriterVisitor visitor, StringBuilder sb, YamlWriterOptions options)
        {
            if (node is LeafNode leafNode)
            {
                visitor.VisitLeafNode(leafNode, sb, options);    
            }
            else if (node is InnerNode innerNode)
            {
                visitor.VisitInnerNode(innerNode, sb, options);    
            }
            else if (node is ListNode listNode)
            {
                visitor.VisitListNode(listNode, sb, options);    
            }
            else if (node is DictionaryNode dicNode)
            {
                visitor.VisitDictionaryNode(dicNode, sb, options);
            }
            else
            {
                throw new NotImplementedException();
            }            
        }       

        /// <inheritdoc/>
        public void VisitLeafNode(LeafNode node, StringBuilder sb, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            YamlWriterHelper.KeyValuePairToYaml(sb, node.Key.ToString(), node.GetValue(), level, options.GetPrefix());
        }           

        /// <inheritdoc/>
        public void VisitInnerNode(InnerNode node, StringBuilder sb, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                if (options.AddKey)
                {
                    YamlWriterHelper.AddObjectStart(sb, node.Key.ToString(), level, options.GetPrefix());
                    level++;
                }

                var children = node.GetChildren();
                // string tmpPrefix = options.Prefix;
                foreach(var keyValue in children)
                {
                    WriterAccept(keyValue, this, sb, new YamlWriterOptions(level, true, options.NumberOfPrefix));
                    if (keyValue is LeafNode)
                    {
                        level += options.NumberOfPrefix;
                        options.NumberOfPrefix = 0;
                    }
                    if(0 != options.NumberOfPrefix)
                    {
                        options.DecreasePrefixCount();
                        level++;
                    }
                    
                    // if (false == string.IsNullOrWhiteSpace(options.Prefix))
                    // {
                    //     tmpPrefix = " ";
                    // }                    
                }
            }
            else
            {
                if (options.AddKey)
                {
                    YamlWriterHelper.AddObjectStart(sb, node.Key.ToString(), level, options.GetPrefix());
                    sb.Append(" {}");
                }
                // else
                // {
                //     throw new NotImplementedException();    
                // }
            }
        } 


        /// <inheritdoc/>
        public void VisitListNode(ListNode node, StringBuilder sb, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                if (false == node.IsPrimitiveList())
                {    
                    var children = node.GetChildren();  
                    string prefix = YAMLConstants.ListItemIndicator.ToString();
                    if (options.AddKey)
                    {
                        YamlWriterHelper.AddObjectStart(sb, node.Key.ToString(), level, options.GetPrefix());
                        level++;
                    }
                    else if (!string.IsNullOrWhiteSpace(options.GetPrefix()))
                    {
                        // options.IncreasePrefixCount();
                        // prefix += " -";
                        // level++;
                    }

                    StringBuilder sb2 = new();
                    foreach (var keyValue in children)
                    {
                        StringBuilder sb3 = new();
                        WriterAccept(keyValue, this, sb3, new YamlWriterOptions(level, false, options.NumberOfPrefix + 1));
                        if(0 != options.NumberOfPrefix)
                        {
                            options.DecreasePrefixCount();
                            level++;
                        }
                        sb2.Append(sb3);
                    }

                    sb.Append(sb2);
                }
                else
                {
                    if (options.AddKey)
                    {
                        YamlWriterHelper.AddObjectStart(sb, node.Key.ToString(), level, options.GetPrefix());
                        level++;
                    }

                    // List to yaml
                    YamlWriterHelper.PrimitiveListToYaml(sb, node, level, options);
                }
            }
            else
            {
                YamlWriterHelper.AddObjectStart(sb, node.Key.ToString(), level, options.GetPrefix());
                sb.Append(" []");
            }
        }

        /// <inheritdoc/>
        public void VisitDictionaryNode(DictionaryNode node, StringBuilder sb, YamlWriterOptions options)
        {
            ArgumentNullException.ThrowIfNull(sb);
            ArgumentNullException.ThrowIfNull(node);

            int level = options.Level;

            if (node.HasChildren())
            {
                var children = node.GetChildren();  

                if (options.AddKey)
                {
                    YamlWriterHelper.AddObjectStart(sb, node.Key.ToString(), level, options.GetPrefix());
                    level++;
                }

                foreach(var keyValue in children)
                {
                    WriterAccept(keyValue, this, sb, new YamlWriterOptions(level, true));
                }
            }
            else
            {
                YamlWriterHelper.AddObjectStart(sb, node.Key.ToString(), level, options.GetPrefix());
                sb.Append(" {}");
            }
        }   

        // ==================================================================================================       
    }
}