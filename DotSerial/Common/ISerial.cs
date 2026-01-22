#region License
//Copyright (c) 2025 Dennis Sölch

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
#endregion

namespace DotSerial.Common
{
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

        /// <summary>
        /// Check if Type is supprted for serialization and deserialization.
        /// </summary>
        /// <param name="t">Type</param>
        /// <returns>True, if supported</returns>
        public abstract static bool IsTypeSupported(Type t);

    }
}
