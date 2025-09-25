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

namespace DotSerial.Core.Misc
{
    /// <summary>
    /// Class is used to identify the type of verious objects.
    /// </summary>
    internal static class GetTypeMethods
    {
        /// <summary>
        /// Gets the type of an object which implements IEnumarble
        /// </summary>
        /// <param name="objType">Object Type</param>
        /// <returns>Type of object in IEnumerable.</returns>
        internal static Type GetItemTypeOfIEnumerable(Type objType)
        {
            if (false == HelperMethods.ImplementsIEnumerable(objType))
            {
                throw new NotSupportedException();
            }

            if (objType.IsArray)
            {
                var tmp = GetItemTypeOfArray(objType);
                return tmp ?? throw new NullReferenceException();
            }

            return objType.GetGenericArguments().Single();
        }

        /// <summary>
        /// Gets the type of an object which implements IEnumarble
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Type of object in IEnumerable.</returns>
        internal static Type GetItemTypeOfIEnumerable(object obj)
        {
            if (false == HelperMethods.ImplementsIEnumerable(obj))
            {
                throw new NotSupportedException();
            }

            if (obj is Array)
            {
                var tmp = GetItemTypeOfArray(obj);
                return tmp ?? throw new NullReferenceException();
            }

            return obj.GetType().GetGenericArguments().Single();
        }

        /// <summary>
        /// Gets the type of an object which implements IEnumarble
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Type of object in IEnumerable.</returns>
        internal static Type? GetItemTypeOfArray(object obj)
        {
            if (false == HelperMethods.ImplementsIEnumerable(obj))
            {
                throw new NotSupportedException();
            }

            return obj.GetType().GetElementType();
        }

        /// <summary>
        /// Gets the type of an object which implements IEnumarble
        /// </summary>
        /// <param name="objType">Object</param>
        /// <returns>Type of object in IEnumerable.</returns>
        internal static Type? GetItemTypeOfArray(Type objType)
        {
            if (false == HelperMethods.ImplementsIEnumerable(objType))
            {
                throw new NotSupportedException();
            }

            return objType.GetElementType();
        }
      
        /// <summary>
        /// Get key and value type of dictionary
        /// </summary>
        /// <param name="obj">Dictionary</param>
        /// <param name="typeKey">Key type</param>
        /// <param name="typeValue">Value type</param>
        /// <returns>True, if succeeded</returns>
        internal static bool GetKeyValueTypeOfDictionary(object obj, out Type typeKey, out Type typeValue)
        {
            if (false == TypeCheckMethods.IsDictionary(obj))
            {
                throw new NotSupportedException();
            }

            Type[] arguments = obj.GetType().GetGenericArguments();

            if (arguments.Length != 2)
            {
                throw new NotSupportedException();
            }

            typeKey = arguments[0];
            typeValue = arguments[1];

            return true;
        }

        /// <summary>
        /// Get key and value type of dictionary
        /// </summary>
        /// <param name="type">Dictionary-type</param>
        /// <param name="typeKey">Key type</param>
        /// <param name="typeValue">Value type</param>
        /// <returns>True, if succeeded</returns>
        internal static bool GetKeyValueTypeOfDictionary(Type type, out Type typeKey, out Type typeValue)
        {
            if (false == TypeCheckMethods.IsDictionary(type))
            {
                throw new NotSupportedException();
            }

            Type[] arguments = type.GetGenericArguments();

            if (arguments.Length != 2)
            {
                throw new NotSupportedException();
            }

            typeKey = arguments[0];
            typeValue = arguments[1];

            return true;
        }

        /// <summary>
        /// Create the Type of a dictionary with the given key, value types.
        /// </summary>
        /// <param name="typeKey">Key type</param>
        /// <param name="typeValue">Value type</param>
        /// <returns>Type</returns>
        internal static Type GetDictionaryTypeFromKeyValue(Type typeKey, Type typeValue)
        {
            ArgumentNullException.ThrowIfNull(typeKey);
            ArgumentNullException.ThrowIfNull(typeValue);

            return typeof(Dictionary<,>).MakeGenericType(typeKey, typeValue);
        }
    }
}
