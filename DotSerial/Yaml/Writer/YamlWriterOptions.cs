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

namespace DotSerial.Yaml.Writer
{
    /// <summary>
    /// Additional options for node visitors.
    /// </summary>
    /// <param name="level">Level of indication</param>
    /// <param name="addKey">Add key</param>
    /// <param name="numberOfListPrefix">Number of listitem indicators to add.</param>
    public struct YamlWriterOptions(int level, bool addKey = true, int numberOfListPrefix = 0)
    {
        /// <summary>
        /// Indentation level
        /// </summary>
        internal int Level { get; private set; } = level;
        /// <summary>
        /// Add Key to object
        /// </summary>
        internal bool AddKey { get; private set; } = addKey;
        /// <summary>
        /// Number of ListItem Indicator to add.
        /// </summary>
        internal int NumberOfPrefix = numberOfListPrefix;
        private static readonly string? s_ListItemIndicator = YamlConstants.ListItemIndicator.ToString();

        /// <summary>
        /// Returns the ListItemIndicator(s) Prefix.
        /// </summary>
        /// <returns>ListItemIndicator(s)</returns>
        public readonly string? GetPrefix()
        {
            if (NumberOfPrefix < 1)
            {
                return null;
            }

            string result = string.Empty;

            for (int i = 0; i < NumberOfPrefix; i++)
            {
                result += string.Format("{0} ", s_ListItemIndicator);
            }

            return result;
        }

        /// <summary>
        /// Decreases the number of ListItemIndicator(s)
        /// </summary>
        public void DecreasePrefixCount()
        {
            NumberOfPrefix--;
                
            if (NumberOfPrefix < 0)
            {
                NumberOfPrefix = 0;
            }

        }
    }
}