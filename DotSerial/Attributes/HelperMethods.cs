using System.Reflection;
using DotSerial.Core.Exceptions;
using DotSerial.Core.XML;

namespace DotSerial.Attributes
{
    public static class HelperMethods
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
                if (att is DSPropertyIDAttribute saveAtt)
                {
                    int result = (int)saveAtt.PropertyID;

                    if (result == Constants.NoAttributeID)
                    {
                        throw new InvalidIDException(result);
                    }

                    return result;
                }
            }
            return Constants.NoAttributeID;
        }

    }
}
