using System.Collections;

namespace DotSerial.Core.Misc
{
    public static class HelperMethods
    {

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

            return obj.GetType().GetGenericArguments().Single();
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
    }
}
