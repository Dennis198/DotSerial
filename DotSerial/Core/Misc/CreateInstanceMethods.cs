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
