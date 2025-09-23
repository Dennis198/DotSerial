
namespace DotSerial.Interfaces
{
    public interface ISerial<T>
    {
        /// <summary> 
        /// Serializes the specific object and creates the an object T which contains
        /// the serialized object.
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>Object which contains the serialized object</returns>
        /// <exception cref="DSNotSupportedTypeException">Object type not supported.</exception>
        /// <exception cref="DSDuplicateIDException">Duplicate ids.</exception>
        /// <exception cref="DSInvalidIDException">Invalid id.</exception>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="InvalidCastException">Invalid cast.</exception>
        /// <exception cref="TypeAccessException">Type access.</exception>
        /// <exception cref="NullReferenceException">Null reference.</exception>
        /// <exception cref="NotSupportedException">Not supported.</exception>
        public abstract static T Serialize(object obj);

        /// <summary>
        /// Deserialzes the object to the specific type.
        /// </summary>
        /// <typeparam name="U">Type of the serialized object</typeparam>
        /// <param name="serialObj">Serialized object.</param>
        /// <returns>Deserialized object</returns>
        /// <exception cref="DSNotSupportedTypeException">Object type not supported.</exception>
        /// <exception cref="DSInvalidIDException">Invalid id.</exception>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="InvalidCastException">Invalid cast.</exception>
        /// <exception cref="TypeAccessException">Type access.</exception>
        /// <exception cref="NullReferenceException">Null reference.</exception>
        /// <exception cref="NotSupportedException">Not supported.</exception>
        public abstract static U Deserialize<U>(T serialObj);

        /// <summary>
        /// Saves the serialized object to file.
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="serialObj">Serialzed object</param>
        /// <exception cref="DSNotSupportedTypeException">Object type not supported.</exception>
        /// <exception cref="DSDuplicateIDException">Duplicate ids.</exception>
        /// <exception cref="DSInvalidIDException">Invalid id.</exception>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="InvalidCastException">Invalid cast.</exception>
        /// <exception cref="TypeAccessException">Type access.</exception>
        /// <exception cref="NullReferenceException">Null reference.</exception>
        /// <exception cref="NotSupportedException">Not supported.</exception>
        /// <exception cref="XmlException">Not supported.</exception>
        public abstract static void SaveToFile(string path, T serialObj);

        /// <summary>
        /// Saves the serialized object to file.
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="obj">Object to serialze</param>
        /// <exception cref="DSNotSupportedTypeException">Object type not supported.</exception>
        /// <exception cref="DSDuplicateIDException">Duplicate ids.</exception>
        /// <exception cref="DSInvalidIDException">Invalid id.</exception>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="InvalidCastException">Invalid cast.</exception>
        /// <exception cref="TypeAccessException">Type access.</exception>
        /// <exception cref="NullReferenceException">Null reference.</exception>
        /// <exception cref="NotSupportedException">Not supported.</exception>
        /// <exception cref="XmlException">Not supported.</exception>
        public abstract static void SaveToFile(string path, object? obj);

        /// <summary>
        /// Loads the deserialized object from file
        /// </summary>
        /// <typeparam name="U">Type of the object</typeparam>
        /// <param name="path">Path to the file</param>
        /// <returns>Deserilized object</returns>
        /// <exception cref="DSNotSupportedTypeException">Object type not supported.</exception>
        /// <exception cref="DSInvalidIDException">Invalid id.</exception>
        /// <exception cref="ArgumentNullException">Argument null.</exception>
        /// <exception cref="InvalidCastException">Invalid cast.</exception>
        /// <exception cref="TypeAccessException">Type access.</exception>
        /// <exception cref="NullReferenceException">Null reference.</exception>
        /// <exception cref="NotSupportedException">Not supported.</exception>
        /// <exception cref="XmlException">Not supported.</exception>
        /// <exception cref="FileNotFoundException">Not supported.</exception>
        public abstract static U LoadFromFile<U>(string path);

        /// <summary>
        /// Check if Type is supprted for serialization and deserialization.
        /// </summary>
        /// <param name="t">Type</param>
        /// <returns>True, if supported</returns>
        public abstract static bool IsTypeSupported(Type t);

    }
}
