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
            return o.GetType().IsArray;
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
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
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
            return o is IDictionary &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
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
        /// Check if type is struct
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>True if struct</returns>
        internal static bool IsStruct(Type type)
        {
            return (type.IsValueType && !type.IsPrimitive && !type.IsEnum && type != typeof(decimal));
        }

        /// <summary>
        /// Check if obj is struct
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>True if struct</returns>
        internal static bool IsStruct(object? obj)
        {
            if (obj == null) return false;
            Type type = obj.GetType();
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

        /// <summary>
        /// Check if type is hashset
        /// </summary>
        /// <param name="o">object</param>
        /// <returns>True if hashset</returns>
        internal static bool IsHashSet(object? o)
        {
            if (o == null) return false;
            return o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(HashSet<>));
        }

        /// <summary>
        /// Check if type is hashset
        /// </summary>
        /// <param name="type">object</param>
        /// <returns>True if hashset</returns>
        internal static bool IsHashSet(Type type)
        {
            if (type == null) return false;
            return type.IsGenericType &&
                 type.GetGenericTypeDefinition().IsAssignableFrom(typeof(HashSet<>));
        }
    }
}
