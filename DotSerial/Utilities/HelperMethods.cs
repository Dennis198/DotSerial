using System.Collections;
using DotSerial.Common;

namespace DotSerial.Utilities
{
    /// <summary>
    /// Class is used to verious helper methods.
    /// </summary>
    internal static class HelperMethods
    {
        /// <summary>
        /// Converts Bool to Integer
        /// </summary>
        /// <param name="b">Bool</param>
        /// <returns>Integer</returns>
        internal static string BoolToString(bool b)
        {
            return b ? CommonConstants.TrueString : CommonConstants.FalseString;
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
        /// Check if Object implements IDictionary
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>True, if type implements IDictionary</returns>
        internal static bool ImplementsIDictionary(object obj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            var oType = obj.GetType();
            return ImplementsIDictionary(oType);
        }

        /// <summary>
        /// Check if Type implements IDictionary
        /// </summary>
        /// <param name="objType">Type</param>
        /// <returns>True, if type implements IDictionary</returns>
        internal static bool ImplementsIDictionary(Type objType)
        {
            return typeof(IDictionary).IsAssignableFrom(objType);
        }

        /// <summary>
        /// Check if Object implements IDictionary<TKey, TValue>
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>True, if type implements IDictionary&lt;TKey, TValue&gt;</returns>
        internal static bool ImplementsIDictionaryKeyValue(object obj)
        {
            ArgumentNullException.ThrowIfNull(obj);
            var oType = obj.GetType();
            return ImplementsIDictionaryKeyValue(oType);
        }

        /// <summary>
        /// Check if Type implements IDictionary&lt;TKey, TValue&gt;
        /// </summary>
        /// <param name="objType">Type</param>
        /// <returns>True, if type implements IDictionary&lt;TKey, TValue&gt;</returns>
        internal static bool ImplementsIDictionaryKeyValue(Type objType)
        {
            return objType
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        /// <summary>
        /// Converts primitive to string
        /// </summary>
        /// <param name="value">Primitive obj</param>
        /// <returns>String</returns>
        internal static string? PrimitiveToString(object? value)
        {
            if (null == value)
            {
                return null;
            }

            string? strValue;
            Type type = value.GetType();
            if (type == typeof(bool))
            {
                strValue = BoolToString((bool)value);
            }
            else if (type.IsEnum)
            {
                strValue = Convert.ToString((int)value);
            }
            else if (type == typeof(float))
            {
                strValue = ((float)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (type == typeof(double))
            {
                strValue = ((double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (type == typeof(decimal))
            {
                strValue = ((decimal)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                strValue = value.ToString();
            }

            if (null == strValue)
            {
                ThrowHelper.ThrowWrongTypeException(type);
            }

            return strValue;
        }

        /// <summary>
        /// Converts Integer to Bool
        /// </summary>
        /// <param name="str">Integer</param>
        /// <returns>Bool</returns>
        internal static bool StringToBool(string str)
        {
            if (str.Equals(CommonConstants.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (str.Equals(CommonConstants.FalseString, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else
            {
                throw new InvalidOperationException("String is not a valid boolean representation.");
            }
        }
    }
}
