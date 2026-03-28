namespace DotSerial.Utilities
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
            if (false == TypeCheckMethods.ImplementsIEnumerable(objType))
            {
                ThrowHelper.ThrowWrongTypeException(objType);
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
            if (false == TypeCheckMethods.ImplementsIEnumerable(obj))
            {
                ThrowHelper.ThrowWrongTypeException(obj.GetType());
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
            if (false == TypeCheckMethods.ImplementsIEnumerable(obj))
            {
                ThrowHelper.ThrowWrongTypeException(obj.GetType());
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
            if (false == TypeCheckMethods.ImplementsIEnumerable(objType))
            {
                ThrowHelper.ThrowWrongTypeException(objType);
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
            if (false == TypeCheckMethods.ImplementsICollectionKeyValuePair(obj))
            {
                ThrowHelper.ThrowWrongTypeException(obj.GetType());
            }

            Type[] arguments = obj.GetType().GetGenericArguments();

            if (arguments.Length != 2)
            {
                ThrowHelper.ThrowWrongTypeException(obj.GetType());
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
            if (false == TypeCheckMethods.ImplementsICollectionKeyValuePair(type))
            {
                ThrowHelper.ThrowWrongTypeException(type);
            }

            Type[] arguments = type.GetGenericArguments();

            if (arguments.Length != 2)
            {
                ThrowHelper.ThrowWrongTypeException(type);
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

        /// <summary>
        /// Create the Type of an IDictionary with the given key, value types.
        /// </summary>
        /// <param name="typeKey">Key type</param>
        /// <param name="typeValue">Value type</param>
        /// <returns>Type</returns>
        internal static Type GetIDictionaryTypeFromKeyValue(Type typeKey, Type typeValue)
        {
            ArgumentNullException.ThrowIfNull(typeKey);
            ArgumentNullException.ThrowIfNull(typeValue);

            return typeof(IDictionary<,>).MakeGenericType(typeKey, typeValue);
        }
    }
}
