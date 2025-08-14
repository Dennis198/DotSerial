using System.Collections;
using System.Reflection;

namespace DotSerial.Interfaces
{
    public interface ISerial<T>
    {
        /// <summary> Creates from an object an object T which can be serialzed.
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>T</returns>
        public abstract static T CreateSerializedObject(object obj);

        /// <summary> DeserialzeObject
        /// </summary>
        /// <param name="obj">Object to dezerialize</param>
        /// <param name="serialObj">Serialized object</param>
        /// <returns>True, if deserialiation is succesfull.</returns>
        public abstract static bool DeserializeObject(object obj, T serialObj);
    }
}
