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

using System.Collections;
using System.Reflection;

namespace DotSerial.Utilities
{
    internal static class HelperMethods
    {

        /// <summary>
        /// Check if Object implements IEnumerable
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>True, if type implements IEnumerable</returns>
        internal static bool ImplementsIEnumerable(object obj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            var oType = obj.GetType();
            return ImplementsIEnumerable(oType);
        }

        /// <summary>
        /// Check if Type implements IEnumerable
        /// </summary>
        /// <param name="objType">Type</param>
        /// <returns>True, if type implements IEnumerable</returns>
        internal static bool ImplementsIEnumerable(Type objType)
        {
            return typeof(IEnumerable).IsAssignableFrom(objType);
        }

        /// <summary>
        /// Converts Bool to Integer
        /// </summary>
        /// <param name="b">Bool</param>
        /// <returns>Integer</returns>
        internal static int BoolToInt(bool b)
        {
            return b ? 1 : 0;
        }

        /// <summary>
        /// Converts Integer to Bool
        /// </summary>
        /// <param name="i">Integer</param>
        /// <returns>Bool</returns>
        internal static bool IntToBool(int i)
        {
            return i == 1;
        }    

        /// <summary>
        /// Converts primitive to string
        /// </summary>
        /// <param name="value">Primitive obj</param>
        /// <returns>String</returns>
        internal static string? PrimitiveToString(object? value)
        {
            if (null == value)
            {
                return null;
            }

            string? strValue;
            Type type = value.GetType();
            if (type == typeof(bool))
            {
                int tmp = HelperMethods.BoolToInt((bool)value);
                strValue = tmp.ToString();
            }
            else if (type.IsEnum)
            {
                strValue = Convert.ToString((int)value);
            }
            else if (type == typeof(float))
            {
                strValue = ((float)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (type == typeof(double))
            {
                strValue = ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (type == typeof(decimal))
            {
                strValue = ((decimal)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                strValue = value.ToString();
            }

            if (null == strValue)
            {
                throw new InvalidOperationException("Could not convert primitive to string.");
            }

            return strValue;
        }

    }
}
