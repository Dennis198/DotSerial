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

namespace DotSerial.Core.YAML.Writer
{
    /// <summary>
    /// Additional options for node visitors.
    /// </summary>
    public struct YamlWriterOptions
    {
        internal int Level { get; private set; }
        internal bool AddKey { get; private set; }
        private string? _prefix;
        internal int NumberOfPrefix = 0;

        public YamlWriterOptions(int level, bool addKey = true, int numberOfPrefix = 0)
        {
            Level = level;
            AddKey = addKey;

            _prefix = YAMLConstants.ListItemIndicator.ToString();
            NumberOfPrefix = numberOfPrefix;
    }

        public string? GetPrefix()
        {
            if (string.IsNullOrWhiteSpace(_prefix))
            {
                return null;
            }
            if (NumberOfPrefix < 0)
            {
                return null;
            }

            string result = string.Empty;

            for (int i = 0; i < NumberOfPrefix; i++)
            {
                result += string.Format("{0} ", _prefix);
            }

            return result;
        }

        public void IncreasePrefixCount()
        {
            NumberOfPrefix++;
        }

        public void DecreasePrefixCount()
        {
            NumberOfPrefix--;
                
            if (NumberOfPrefix < 0)
            {
                NumberOfPrefix = 0;
                // throw new NotImplementedException();
            }

        }

        // internal static YamlWriterOptions CreateHigher(YamlWriterOptions opt)
        // {
        //     var result = new YamlWriterOptions(opt.Level + 1, opt.AddKey, opt.Prefix);
        //     return result;
        // }
    }
}