
namespace DotSerial.Interfaces
{
    public interface ISerial<T>
    {
        /// <summary> 
        /// Creates from an object an object T which can be serialzed.
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>T</returns>
        public abstract static T CreateSerializedObject(object obj);

        /// <summary>
        /// DeserialzeObject
        /// </summary>
        /// <param name="obj">Object to dezerialize</param>
        /// <param name="serialObj">Serialized object</param>
        /// <returns>True, if deserialiation is succesfull.</returns>
        public abstract static bool DeserializeObject(object obj, T serialObj);

        /// <summary>
        /// Converts the serialized object to a string
        /// </summary>
        /// <param name="serialObj">erialized object</param>
        /// <returns>String</returns>
        public abstract static string AsString(T serialObj);

        /// <summary>
        /// Check if Type is supprted for serialization and deserialization.
        /// </summary>
        /// <param name="t">Type</param>
        /// <returns>True, if supported</returns>
        public abstract static bool IsTypeSupported(Type t);
    }
}
