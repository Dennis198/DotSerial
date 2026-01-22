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

using System.Runtime.Serialization;
using DotSerial.Common;

namespace DotSerial.Utilities
{
    internal static class CreateInstanceMethods
    {
        /// <summary>
        /// Creates a instance of the given type
        /// </summary>
        /// <param name="type">Type of instance</param>
        /// <returns>Object</returns>
        public static object CreateInstanceGeneric(Type type)
        {
            try
            {
                object? tmp = Activator.CreateInstance(type);

                return tmp ?? throw new NullReferenceException();
            }
            catch (MissingMethodException)
            {
                object? unTmp = CreateUninitializedObject(type);
                return unTmp ?? throw new NullReferenceException();
            }
            catch (Exception) 
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a instance of the given type
        /// </summary>
        /// <typeparam name="T">Object to create</typeparam>
        /// <returns>Object</returns>
        public static T CreateInstanceGeneric<T>()
        {
            try
            {
                T tmp = Activator.CreateInstance<T>();

                return tmp ?? throw new NullReferenceException();
            }
            catch (MissingMethodException)
            {
                T unTmp = CreateUninitializedObject<T>();
                return unTmp ?? throw new NullReferenceException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Create an Array instance of the given type with the given
        /// number of elements
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="count">Number of elements</param>
        /// <returns>Array</returns>
        public static Array CreateInstanceArray(Type type, int count)
        {
            try
            {

#if NET9_0
                var tmp = Array.CreateInstanceFromArrayType(type, count);
#elif NET8_0
                Type? itemType = GetTypeMethods.GetItemTypeOfArray(type);
                if (null == itemType)
                {
                    throw new NullReferenceException();
                }
                var tmp = Array.CreateInstance(itemType, count);
#endif
                return tmp ?? throw new NullReferenceException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates an HashSet instacne of the given type.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>HashSet</returns>
        public static object CreateInstanceHashSet(Type type)
        {
            try
            {
                var tmp = Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(type));
                return tmp ?? throw new NullReferenceException();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates an unintialized object
        /// </summary>
        /// <typeparam name="T">Object to create</typeparam>
        /// <returns>Object</returns>
        private static T CreateUninitializedObject<T>()
        {
            try
            {
#pragma warning disable SYSLIB0050
                // Try create instance without a constructor.
                return (T)FormatterServices.GetUninitializedObject(typeof(T));
#pragma warning restore SYSLIB0050
            }
            catch
            {
                throw new DotSerialException("Type need a parameterless constructor for deserialization");
            }
        }

        /// <summary>
        /// Creates an unintialized object
        /// </summary>
        /// <param name="type">Type of instance</param>
        /// <returns>Object</returns>
        private static object CreateUninitializedObject(Type type)
        {
            try
            {
#pragma warning disable SYSLIB0050
                // Try create instance without a constructor.
                return FormatterServices.GetUninitializedObject(type);
#pragma warning restore SYSLIB0050
            }
            catch
            {
                throw new DotSerialException("Type need a parameterless constructor for deserialization");
            }
        }
    }
}
