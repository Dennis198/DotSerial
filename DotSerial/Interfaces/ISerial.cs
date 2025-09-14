
namespace DotSerial.Interfaces
{
    public interface ISerial<T>
    {
        /// <summary> 
        /// Creates from an object an object T which can be serialzed.
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>T</returns>
        public abstract static T Serialize(object obj);

        /// <summary>
        /// Deserialze Object
        /// </summary>
        /// <param name="obj">Object to dezerialize</param>
        /// <param name="serialObj">Serialized object</param>
        /// <returns>True, if deserialiation is succesfull.</returns>
        //public abstract static bool Deserialize(object obj, T serialObj);

        /// <summary>
        /// Deserialze Object
        /// </summary>
        /// <typeparam name="U">Type of the serialized object</typeparam>
        /// <param name="serialObj">Serialized object.</param>
        /// <returns>Deserialized object</returns>
        public abstract static U Deserialize<U>(T serialObj);

        /// <summary>
        /// Saves the serialized object to file.
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="serialObj">Serialzed object</param>
        /// <returns>True if succeeded</returns>
        public abstract static bool SaveToFile(string path, T serialObj);

        /// <summary>
        /// Saves the serialized object to file.
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="obj">Object to serialze</param>
        /// <returns>True if succeeded</returns>
        public abstract static bool SaveToFile(string path, object? obj);
        
        /// <summary>
        /// Loads the deserialized object from file
        /// </summary>
        /// <typeparam name="U">Type of the object</typeparam>
        /// <param name="path">Path to the file</param>
        /// <returns>Deserilized object</returns>
        public abstract static U LoadFromFile<U>(string path);

    }
}
