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

using System.Reflection;
using DotSerial.Common;

namespace DotSerial.Attributes
{
    internal static class AttributesMethods
    {
        /// <summary>
        /// Gets the name for serialization.
        /// </summary>
        /// <param name="prop">PropertyInfo</param>
        /// <returns>Name for serialization</returns>
        internal static string? GetSerializeName(PropertyInfo prop)
        {
            ArgumentNullException.ThrowIfNull(prop);

            if (ShouldPropertyBeIgnored(prop))
            {
                return null;
            }

            string? name = GetCustomPropertyName(prop);

            if (null == name)
            {
                name = prop.Name;
            }

            return name;
        }

        /// <summary> Get the parameter ID
        /// </summary>
        /// <param name="prop">PropertyInfo</param>
        /// <returns>ID</returns>
        private static string? GetCustomPropertyName(PropertyInfo prop)
        {
            object[] attrs = prop.GetCustomAttributes(true);
            foreach (object att in attrs)
            {
                if (att is DotSerialNameAttribute saveAtt)
                {
                    string result = saveAtt.Name;

                    if (result.Length == 0)
                    {
                        throw new DotSerialException($"{nameof(DotSerialNameAttribute)} can't be null.");
                    }

                    return result;
                }
            }

            return null;
        }       

        /// <summary>
        /// Check if the property should be ignored.
        /// </summary>
        /// <param name="prop">PropertyInfo</param>
        /// <returns>True, if ignored.</returns>
        private static bool ShouldPropertyBeIgnored(PropertyInfo prop)
        {
            object[] attrs = prop.GetCustomAttributes(true);
            foreach (object att in attrs)
            {
                if (att is DotSerialIgnoreAttribute)
                {
                    return true;
                }
            }

            return false;
        } 

    }
}
