using DotSerial.Common;
using DotSerial.Tree.Creation;
using DotSerial.Tree.Serialize;

namespace DotSerial
{
    public static class DSConverter
    {
        public static U Deserialize<U>(ReadOnlySpan<char> str, StategyType strategy)
        {
            try
            {
                return DSNode.ToObject<U>(str, strategy);
            }
            catch
            {
                throw;
            }
        }

        public static U LoadFromFile<U>(string path, StategyType strategy)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            try
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException(path);
                }

                if (false == FileMethods.LoadFileContent(path, out string content))
                {
                    throw new NotSupportedException();
                }

                var result = Deserialize<U>(content, strategy);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public static void SaveToFile(string path, object? obj, StategyType strategy)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            try
            {
                var content = Serialize(obj, strategy);
                FileMethods.SaveContentToFile(path, content);
            }
            catch
            {
                throw;
            }
        }

        public static string Serialize(object? obj, StategyType strategy)
        {
            var rootNode = SerializeObject.Serialize(obj, CommonConstants.MainObjectKey, strategy);
            var dsNode = new DSNode(rootNode);
            return dsNode.Stringify(strategy);
        }

        public static DSNode ToNode(object? obj, StategyType strategy)
        {
            var rootNode = SerializeObject.Serialize(obj, CommonConstants.MainObjectKey, strategy);
            var dsNode = new DSNode(rootNode);
            return dsNode;
        }

        public static DSNode ToNodeFromString(ReadOnlySpan<char> str, StategyType strategy)
        {
            var root = DSNode.FromString(str, strategy);
            return root;
        }
    }
}
