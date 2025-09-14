using System.Collections;

namespace DotSerial.Core.Misc
{
    internal static class HelperMethods
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
        /// Gets the type of an object which implements IEnumarble
        /// </summary>
        /// <param name="objType">Object Type</param>
        /// <returns>Type of object in IEnumerable.</returns>
        internal static Type GetItemTypeOfIEnumerable(Type objType)
        {
            if (false == ImplementsIEnumerable(objType))
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
            if (false == ImplementsIEnumerable(obj))
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
        /// Get key and value type of dictionary
        /// </summary>
        /// <param name="obj">Dictionary</param>
        /// <param name="typeKey">Key type</param>
        /// <param name="typeValue">Value type</param>
        /// <returns>True, if succeeded</returns>
        /// <exception cref="NotSupportedException"></exception>
        internal static bool GetKeyValueTypeOfDictionary(object obj, out Type typeKey, out Type typeValue)
        {
            if (false == IsDictionary(obj))
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
        /// <exception cref="NotSupportedException"></exception>
        internal static bool GetKeyValueTypeOfDictionary(Type type, out Type typeKey, out Type typeValue)
        {
            if (false == IsDictionary(type))
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
        /// Gets the type of an object which implements IEnumarble
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Type of object in IEnumerable.</returns>
        internal static Type? GetItemTypeOfArray(object obj)
        {
            if (false == ImplementsIEnumerable(obj))
            {
                throw new NotSupportedException();
            }

            return obj.GetType().GetElementType();
        }

        /// <summary>
        /// Gets the type of an object which implements IEnumarble
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Type of object in IEnumerable.</returns>
        internal static Type? GetItemTypeOfArray(Type objType)
        {
            if (false == ImplementsIEnumerable(objType))
            {
                throw new NotSupportedException();
            }

            return objType.GetElementType();
        }

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
            if (ImplementsIEnumerable(type)) return false;
            return type.IsClass;
        }
    }
}
