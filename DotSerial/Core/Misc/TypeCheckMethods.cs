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

namespace DotSerial.Core.Misc
{
    /// <summary>
    /// Class is used to check Types/objects if there are of a specific
    /// type.
    /// </summary>
    internal static class TypeCheckMethods
    {
        /// <summary>
        /// Returns true, if Type is a primitive
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>True if primitive</returns>
        internal static bool IsPrimitive(Type type)
        {
            // null => false
            if (type == null)
                return false;

            // Special Cases 
            if (type == typeof(string) ||
                type.IsEnum)
            {
                return true;
            }

            // c# Primitive class
            if (type == typeof(bool) ||
                type == typeof(byte) ||
                type == typeof(sbyte) ||
                type == typeof(char) ||
                type == typeof(decimal) ||
                type == typeof(double) ||
                type == typeof(float) ||
                type == typeof(int) ||
                type == typeof(uint) ||
                type == typeof(nint) ||
                type == typeof(nuint) ||
                type == typeof(long) ||
                type == typeof(ulong) ||
                type == typeof(short) ||
                type == typeof(ushort)
               )
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if object is array
        /// </summary>
        /// <param name="o">object</param>
        /// <returns>True if array</returns>
        internal static bool IsArray(object? o)
        {
            if (o == null) return false;
            return IsArray(o.GetType());
        }

        /// <summary>
        /// Check if type is array
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>True if array</returns>
        internal static bool IsArray(Type type)
        {
            if (type == null) return false;
            return type.IsArray;
        }

        /// <summary>
        /// Check if object is list
        /// </summary>
        /// <param name="o">object</param>
        /// <returns>True if list</returns>
        internal static bool IsList(object? o)
        {
            if (o == null) return false;
            return o is IList && IsList(o.GetType());
        }

        /// <summary>
        /// Check if type is list
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>True if list</returns>
        internal static bool IsList(Type type)
        {
            if (type == null) return false;
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        /// <summary>
        /// Check if object is dictioanry
        /// </summary>
        /// <param name="o">object</param>
        /// <returns>True if list</returns>
        internal static bool IsDictionary(object? o)
        {
            if (o == null) return false;
            return o is IDictionary && IsDictionary(o.GetType());
        }

        /// <summary>
        /// Check if type is dictionary
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>True if dictionary</returns>
        internal static bool IsDictionary(Type type)
        {
            if (type == null) return false;
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }

        /// <summary>
        /// Check if obj is struct
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>True if struct</returns>
        internal static bool IsStruct(object? obj)
        {
            if (obj == null) return false;
            return IsStruct(obj.GetType());
        }

        /// <summary>
        /// Check if type is struct
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>True if struct</returns>
        internal static bool IsStruct(Type type)
        {
            return (type.IsValueType && !type.IsPrimitive && !type.IsEnum && type != typeof(decimal));
        }

        /// <summary>
        /// Check if type is class
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>True if class</returns>
        internal static bool IsClass(Type type)
        {
            if (type == null) return false;
            if (type == typeof(string)) return false;
            if (HelperMethods.ImplementsIEnumerable(type)) return false;
            return type.IsClass;
        }
    }
}
