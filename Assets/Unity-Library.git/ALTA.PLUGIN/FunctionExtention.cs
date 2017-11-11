using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System;
using System.Reflection;
using System.Collections;
using UnityEngine;

namespace Alta.Plugin
{
    public delegate void FuncDelay();
    public static class FunctionExtention
    {
     
        public static int getIndex(this Enum _enum)
        {
            return Array.IndexOf(Enum.GetValues(_enum.GetType()), _enum);
        }

        /// <summary>
        /// convert object to byte array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerialize">object</param>
        /// <returns>byte array</returns>
        public static byte[] SerializeObjectByte<T>(this T objectToSerialize)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream memStr = new MemoryStream();
            bf.Serialize(memStr, objectToSerialize);
            memStr.Position = 0;
            return memStr.ToArray();
        }

        /// <summary>
        /// convert byte array to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataStream">byte array</param>
        /// <returns>object</returns>
        public static T DeserializeByte<T>(this byte[] dataStream)
        {
            MemoryStream stream = new MemoryStream(dataStream);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            bf.Binder = new VersionFixer();
            try
            {

                T newInfo = (T)bf.Deserialize(stream);
                return newInfo;
            }
            catch (Exception)
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
        }
        public static object DeserializeByte(this byte[] dataStream)
        {
            MemoryStream stream = new MemoryStream(dataStream);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            bf.Binder = new VersionFixer();
            try
            {

                object newInfo = bf.Deserialize(stream);
                return newInfo;
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
    sealed class VersionFixer : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type typeToDeserialize = null;

            // For each assemblyName/typeName that you want to deserialize to
            // a different type, set typeToDeserialize to the desired type.
            string assemVer1 = Assembly.GetExecutingAssembly().FullName;
            if (assemblyName != assemVer1)
            {
                // To use a type from a different assembly version, 
                // change the version number.
                // To do this, uncomment the following line of code.
                assemblyName = assemVer1;
                // To use a different type from the same assembly, 
                // change the type name.
            }
            // The following line of code returns the type.
            typeToDeserialize = System.Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
            return typeToDeserialize;
        }

    } 
}
