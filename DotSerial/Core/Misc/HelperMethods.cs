using System.Collections;

namespace DotSerial.Core.Misc
{
    internal static class HelperMethods
    {




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

        internal static bool IsHashTable(object? o)
        {
            if (o == null) return false;
            return o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Hashtable));
        }

        internal static bool IsHashTable(Type type)
        {
            if (type == null) return false;
            return type.IsGenericType &&
                 type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Hashtable));
        }

        internal static bool IsStack(object? o)
        {
            if (o == null) return false;
            return o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Stack<>));
        }

        internal static bool IsStack(Type type)
        {
            if (type == null) return false;
            return type.IsGenericType &&
                 type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Stack<>));
        }

        internal static bool IsQueue(object? o)
        {
            if (o == null) return false;
            return o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Queue<>));
        }

        internal static bool IsQueue(Type type)
        {
            if (type == null) return false;
            return type.IsGenericType &&
                 type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Queue<>));
        }

    }
}
