using System.Reflection;
using DotSerial.Core.XML;

namespace DotSerial.Attributes
{
    internal static class HelperMethods
    {
        /// <summary> Get the parameter ID
        /// </summary>
        /// <param name="prop">PropertyInfo</param>
        /// <returns>ID</returns>
        public static int GetPropertyID(PropertyInfo prop)
        {
            object[] attrs = prop.GetCustomAttributes(true);
            foreach (object att in attrs)
            {
                if (att is SerialzePropertyIDAttribute saveAtt)
                {
                    return (int)saveAtt.PropertyID;
                }
            }
            return Constants.NoAttributeID;
        }

    }
}
