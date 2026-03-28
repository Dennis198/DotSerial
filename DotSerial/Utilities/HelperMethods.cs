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
