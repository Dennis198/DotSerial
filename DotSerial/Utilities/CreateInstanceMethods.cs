using System.Runtime.Serialization;

namespace DotSerial.Utilities
{
    /// <summary>
    /// Class is used to verious methods to create instances of objects.
    /// </summary>
    internal static class CreateInstanceMethods
    {
        /// <summary>
        /// Creates a instance of the given type
        /// </summary>
        /// <param name="type">Type of instance</param>
        /// <returns>Object</returns>
        internal static object CreateInstanceGeneric(Type type)
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
        internal static T CreateInstanceGeneric<T>()
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
        internal static Array CreateInstanceArray(Type type, int count)
        {
            try
            {
#if NET9_0 || NET10_0
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
        internal static object CreateInstanceHashSet(Type type)
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
