using System.Reflection;

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
            return -1;
        }

        /// <summary> Get the class ID
        /// </summary>
        /// <param name="t">Type</param>
        /// <returns>ID</returns>
        public static int GetClassID(Type t)
        {
            var attrs = t.GetCustomAttributes();
            foreach (object att in attrs)
            {
                if (att is SerialzeClassIDAttribute classID)
                {
                    return (int)classID.ClassID;
                }
            }
            return -1;
        }
    }
}
