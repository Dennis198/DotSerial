namespace DotSerial.Common
{
    /// <summary>
    /// Interface for the main class
    /// </summary>
    /// <typeparam name="T">Type of the class (Json, Yaml, ...)</typeparam>
    public interface ISerial<T>
    {
        /// <summary> 
        /// Serializes the specific object and creates the an object T which contains
        /// the serialized object.
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>Object which contains the serialized object</returns>
        public abstract static T Serialize(object obj);

        /// <summary>
        /// Deserialzes the object to the specific type.
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
        public abstract static void SaveToFile(string path, T serialObj);

        /// <summary>
        /// Saves the serialized object to file.
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="obj">Object to serialze</param>
        public abstract static void SaveToFile(string path, object? obj);

        /// <summary>
        /// Loads the deserialized object from file
        /// </summary>
        /// <typeparam name="U">Type of the object</typeparam>
        /// <param name="path">Path to the file</param>
        /// <returns>Deserilized object</returns>
        public abstract static U LoadFromFile<U>(string path);

    }
}
