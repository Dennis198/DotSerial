using System.Reflection;
using DotSerial.Common;

namespace DotSerial.Attributes
{
    /// <summary>
    /// Helper Methods for the custom attributes
    /// </summary>
    internal static class AttributesMethods
    {
        /// <summary>
        /// Gets the name for serialization.
        /// </summary>
        /// <param name="prop">PropertyInfo</param>
        /// <returns>Name for serialization</returns>
        internal static string? GetSerializeName(PropertyInfo prop)
        {
            ArgumentNullException.ThrowIfNull(prop);

            if (ShouldPropertyBeIgnored(prop))
            {
                return null;
            }

            string? name = GetCustomPropertyName(prop);

            if (null == name)
            {
                name = prop.Name;
            }

            return name;
        }

        /// <summary> Get the parameter ID
        /// </summary>
        /// <param name="prop">PropertyInfo</param>
        /// <returns>ID</returns>
        private static string? GetCustomPropertyName(PropertyInfo prop)
        {
            object[] attrs = prop.GetCustomAttributes(true);
            foreach (object att in attrs)
            {
                if (att is DotSerialNameAttribute saveAtt)
                {
                    string result = saveAtt.Name;

                    ArgumentOutOfRangeException.ThrowIfZero(result.Length, result);

                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Check if the property should be ignored.
        /// </summary>
        /// <param name="prop">PropertyInfo</param>
        /// <returns>True, if ignored.</returns>
        private static bool ShouldPropertyBeIgnored(PropertyInfo prop)
        {
            object[] attrs = prop.GetCustomAttributes(true);
            foreach (object att in attrs)
            {
                if (att is DotSerialIgnoreAttribute)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
