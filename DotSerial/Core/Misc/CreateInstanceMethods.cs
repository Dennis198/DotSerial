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

using DotSerial.Core.Exceptions;

namespace DotSerial.Core.Misc
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
                // Hack, currently if a "record" without a parameterless constructor is
                // deserialized it will crash with "Activator.CreateInstance"
                throw new DSNoParameterlessConstructorDefinedException(type.Name);
            }
            catch (Exception) 
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a instance of the given type
        /// </summary>
        /// <param name="type">Type of instance</param>
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
                // Hack, currently if a "record" without a parameterless constructor is
                // deserialized it will crash with "Activator.CreateInstance"
                throw new DSNoParameterlessConstructorDefinedException();
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
                var tmp = Array.CreateInstanceFromArrayType(type, count);
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
    }
}
