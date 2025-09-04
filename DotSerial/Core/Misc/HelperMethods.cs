using System.Collections;

namespace DotSerial.Core.Misc
{
    public static class HelperMethods
    {

        /// <summary> Returns true, if Type is a primitive
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>True if primitive</returns>
        public static bool IsPrimitive(Type type)
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

        /// <summary> Gets the type of an object which implements IEnumarble
        /// </summary>
        /// <param name="objType">Object Type</param>
        /// <returns>Type of object in IEnumerable.</returns>
        public static Type GetItemTypeOfIEnumerable(Type objType)
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

        /// <summary> Gets the type of an object which implements IEnumarble
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Type of object in IEnumerable.</returns>
        public static Type GetItemTypeOfIEnumerable(object obj)
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

        public static bool GetKeyValueTypeOfDictionary(object obj, out Type typeKey, out Type typeValue)
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

        public static bool GetKeyValueTypeOfDictionary(Type type, out Type typeKey, out Type typeValue)
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

        /// <summary> Gets the type of an object which implements IEnumarble
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Type of object in IEnumerable.</returns>
        public static Type? GetItemTypeOfArray(object obj)
        {
            if (false == ImplementsIEnumerable(obj))
            {
                throw new NotSupportedException();
            }

            return obj.GetType().GetElementType();
        }

        /// <summary> Gets the type of an object which implements IEnumarble
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Type of object in IEnumerable.</returns>
        public static Type? GetItemTypeOfArray(Type objType)
        {
            if (false == ImplementsIEnumerable(objType))
            {
                throw new NotSupportedException();
            }

            return objType.GetElementType();
        }

        /// <summary> Check if Object implements IEnumerable
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>True, if type implements IEnumerable</returns>
        public static bool ImplementsIEnumerable(object obj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            var oType = obj.GetType();
            return ImplementsIEnumerable(oType);
        }

        /// <summary> Check if Type implements IEnumerable
        /// </summary>
        /// <param name="objType">Type</param>
        /// <returns>True, if type implements IEnumerable</returns>
        public static bool ImplementsIEnumerable(Type objType)
        {
            return typeof(IEnumerable).IsAssignableFrom(objType);
        }

        /// <summary> Converts Bool to Integer
        /// </summary>
        /// <param name="b">Bool</param>
        /// <returns>Integer</returns>
        public static int BoolToInt(bool b)
        {
            return b ? 1 : 0;
        }

        /// <summary> Converts Integer to Bool
        /// </summary>
        /// <param name="i">Integer</param>
        /// <returns>Bool</returns>
        public static bool IntToBool(int i)
        {
            return i == 1;
        }

        public static bool IsArray(object o)
        {
            if (o == null) return false;
            return o.GetType().IsArray;
        }

        public static bool IsArray(Type type)
        {
            if (type == null) return false;
            return type.IsArray;
        }

        public static bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public static bool IsList(Type type)
        {
            if (type == null) return false;
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public static bool IsDictionary(object o)
        {
            if (o == null) return false;
            return o is IDictionary &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }

        public static bool IsDictionary(Type type)
        {
            if (type == null) return false;
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }
    }
}
