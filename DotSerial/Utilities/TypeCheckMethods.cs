using System.Collections;
using System.Globalization;
using System.Net;

namespace DotSerial.Utilities
{
    /// <summary>
    /// Class is used to check Types/objects if there are of a specific
    /// type.
    /// </summary>
    internal static class TypeCheckMethods
    {
        /// <summary>
        /// Check if Type is supprted for serialization and deserialization.
        /// </summary>
        /// <param name="t">Type</param>
        /// <returns>True, if supported</returns>
        internal static bool IsTypeSupported(Type t)
        {
            // Primitive + string.
            if (IsPrimitive(t))
            {
                return true;
            }

            if (ImplementsIEnumerable(t))
            {
                if (IsListNodeCompatible(t) || IsDictionaryNodeCompatible(t))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (IsInnerNodeCompatible(t) || IsSpecialParsableObject(t))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Types that can be serialized to a leaf node (primitive types, string and special parsable objects)
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if the type can be serialized to a leaf node</returns>
        internal static bool IsLeafNodeCompatible(Type type)
        {
            if (type == null)
                return false;
            return IsPrimitive(type) || IsSpecialParsableObject(type);
        }

        /// <summary>
        /// Types that can be serialized to a leaf node (primitive types, string and special parsable objects)
        /// </summary>
        /// <param name="o">Object to check</param>
        /// <returns>True if the object can be serialized to a leaf node</returns>
        internal static bool IsLeafNodeCompatible(object? o)
        {
            if (o == null)
                return false;
            Type type = o.GetType();
            return IsLeafNodeCompatible(type);
        }

        /// <summary>
        /// Types that can be serialized to a list node.
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if the type can be serialized to a list node</returns>
        internal static bool IsListNodeCompatible(Type type)
        {
            if (type == null)
                return false;
            return ImplementsICollection(type) || IsArray(type) || IsStack(type) || IsQueue(type);
        }

        /// <summary>
        /// Types that can be serialized to a list node.
        /// </summary>
        /// <param name="o">Object to check</param>
        /// <returns>True if the object can be serialized to a list node</returns>
        internal static bool IsListNodeCompatible(object? o)
        {
            if (o == null)
                return false;
            Type type = o.GetType();
            return IsListNodeCompatible(type);
        }

        /// <summary>
        /// Types that can be serialized to a dictionary node.
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if the type can be serialized to a dictionary node</returns>
        internal static bool IsDictionaryNodeCompatible(Type type)
        {
            if (type == null)
                return false;
            return ImplementsICollectionKeyValuePair(type);
        }

        /// <summary>
        /// Types that can be serialized to a dictionary node.
        /// </summary>
        /// <param name="o">Object to check</param>
        /// /// <returns>True if the object can be serialized to a dictionary node</returns>
        internal static bool IsDictionaryNodeCompatible(object? o)
        {
            if (o == null)
                return false;
            Type type = o.GetType();
            return IsDictionaryNodeCompatible(type);
        }

        /// <summary>
        /// Types that can be serialized to an inner node (class or struct)
        /// </summary>
        /// <param name="type">Type to check</param>
        /// <returns>True if the type can be serialized to an inner node</returns>
        internal static bool IsInnerNodeCompatible(Type type)
        {
            if (type == null)
                return false;
            return IsClass(type) || IsStruct(type);
        }

        /// <summary>
        /// Types that can be serialized to an inner node (class or struct)
        /// </summary>
        /// <param name="o">Object to check</param>
        /// <returns>True if the object can be serialized to an inner node</returns>
        internal static bool IsInnerNodeCompatible(object? o)
        {
            if (o == null)
                return false;
            Type type = o.GetType();
            return IsInnerNodeCompatible(type);
        }

        /// <summary>
        /// Determines if a type is a numeric type.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>True, if the type is a numeric type</returns>
        internal static bool IsNumericType(Type type)
        {
            return type == typeof(byte)
                || type == typeof(byte?)
                || type == typeof(sbyte)
                || type == typeof(sbyte?)
                || type == typeof(int)
                || type == typeof(int?)
                || type == typeof(uint)
                || type == typeof(uint?)
                || type == typeof(nint)
                || type == typeof(nint?)
                || type == typeof(nuint)
                || type == typeof(nuint?)
                || type == typeof(long)
                || type == typeof(long?)
                || type == typeof(ulong)
                || type == typeof(ulong?)
                || type == typeof(short)
                || type == typeof(short?)
                || type == typeof(ushort)
                || type == typeof(ushort?)
                || type == typeof(float)
                || type == typeof(float?)
                || type == typeof(double)
                || type == typeof(double?)
                || type == typeof(decimal)
                || type == typeof(decimal?)
                || type.IsEnum;
        }

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
            if (type == typeof(string) || type.IsEnum)
            {
                return true;
            }

            // c# Primitive class
            if (
                type == typeof(bool)
                || type == typeof(byte)
                || type == typeof(sbyte)
                || type == typeof(char)
                || type == typeof(decimal)
                || type == typeof(double)
                || type == typeof(float)
                || type == typeof(int)
                || type == typeof(uint)
                || type == typeof(nint)
                || type == typeof(nuint)
                || type == typeof(long)
                || type == typeof(ulong)
                || type == typeof(short)
                || type == typeof(ushort)
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
            if (o == null)
                return false;
            return IsArray(o.GetType());
        }

        /// <summary>
        /// Check if type is array
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>True if array</returns>
        internal static bool IsArray(Type type)
        {
            if (type == null)
                return false;
            return type.IsArray;
        }

        /// <summary>
        /// Check if type is stack
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>True if stack</returns>
        internal static bool IsStack(Type type)
        {
            if (type == null)
                return false;
            return type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Stack<>));
        }

        /// <summary>
        /// Check if type is queue
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>True if queue</returns>
        internal static bool IsQueue(Type type)
        {
            if (type == null)
                return false;
            return type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Queue<>));
        }

        /// <summary>
        /// Check if obj is struct
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>True if struct</returns>
        internal static bool IsStruct(object? obj)
        {
            if (obj == null)
                return false;
            return IsStruct(obj.GetType());
        }

        /// <summary>
        /// Check if type is struct
        /// </summary>
        /// <param name="type">type</param>
        /// <returns>True if struct</returns>
        internal static bool IsStruct(Type type)
        {
            if (IsSpecialParsableObject(type))
                return false;
            return (type.IsValueType && !type.IsPrimitive && !type.IsEnum && type != typeof(decimal));
        }

        /// <summary>
        /// Check if type is class
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>True if class</returns>
        internal static bool IsClass(Type type)
        {
            if (type == null)
                return false;
            if (type == typeof(string))
                return false;
            if (ImplementsIEnumerable(type))
                return false;
            if (IsSpecialParsableObject(type))
                return false;
            return type.IsClass;
        }

        /// <summary>
        /// Check if type is a special parsable type like DateTime
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>True, if special parsable type.</returns>
        internal static bool IsSpecialParsableObject(object? obj)
        {
            if (obj == null)
                return false;
            return IsSpecialParsableObject(obj.GetType());
        }

        /// <summary>
        /// Check if type is a special parsable type like DateTime
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>True, if special parsable type.</returns>
        internal static bool IsSpecialParsableObject(Type type)
        {
            if (type == typeof(DateTime))
            {
                return true;
            }
            else if (type == typeof(Guid))
            {
                return true;
            }
            else if (type == typeof(TimeSpan))
            {
                return true;
            }
            else if (type == typeof(Uri))
            {
                return true;
            }
            else if (type == typeof(IPAddress))
            {
                return true;
            }
            else if (type == typeof(Version))
            {
                return true;
            }
            else if (type == typeof(CultureInfo))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if Object implements IEnumerable
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>True, if type implements IEnumerable</returns>
        internal static bool ImplementsIEnumerable(object? obj)
        {
            if (null == obj)
                return false;
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
        /// Check if Object implements ICollection&lt;T&gt;
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>True, if type implements ICollection&lt;T&gt;</returns>
        internal static bool ImplementsICollection(object? obj)
        {
            if (null == obj)
                return false;
            var oType = obj.GetType();
            return ImplementsICollection(oType);
        }

        /// <summary>
        /// Check if Type implements ICollection&lt;T&gt;
        /// </summary>
        /// <param name="objType">Type</param>
        /// <returns>True, if type implements ICollection&lt;T&gt;</returns>
        internal static bool ImplementsICollection(Type objType)
        {
            if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                Type itemType = objType.GetGenericArguments()[0];
                return !(itemType.IsGenericType && itemType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>));
            }

            return objType
                .GetInterfaces()
                .Any(i =>
                    i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(ICollection<>)
                    && !(
                        i.GetGenericArguments()[0] is Type arg
                        && arg.IsGenericType
                        && arg.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)
                    )
                );
        }

        /// <summary>
        /// Check if Object implements ICollection&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>True, if type implements ICollection&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;</returns>
        internal static bool ImplementsICollectionKeyValuePair(object? obj)
        {
            if (null == obj)
                return false;
            var oType = obj.GetType();
            return ImplementsICollectionKeyValuePair(oType);
        }

        /// <summary>
        /// Check if Type implements ICollection&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;
        /// </summary>
        /// <param name="objType">Type</param>
        /// <returns>True, if type implements ICollection&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;</returns>
        internal static bool ImplementsICollectionKeyValuePair(Type objType)
        {
            if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                Type itemType = objType.GetGenericArguments()[0];
                return itemType.IsGenericType && itemType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>);
            }

            return objType
                .GetInterfaces()
                .Any(i =>
                    i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(ICollection<>)
                    && i.GetGenericArguments()[0] is Type arg
                    && arg.IsGenericType
                    && arg.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)
                );
        }
    }
}
