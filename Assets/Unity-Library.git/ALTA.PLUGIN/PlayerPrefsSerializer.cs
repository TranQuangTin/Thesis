using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
namespace Alta.Plugin
{
    public static class PlayerPrefsSerializer
    {
        private static BinaryFormatter bf = new BinaryFormatter();

        public static void Save<T>(this T serializableObject, string prefKey)
        {
            if (serializableObject == null)
                return;
            Debug.Log(string.Format("Saving: {0} => {1}", prefKey, typeof(T)));
            MemoryStream memoryStream = new MemoryStream();
            bf.Serialize(memoryStream, serializableObject);
            Debug.Log(memoryStream.Length);
            string serializedData = System.Convert.ToBase64String(memoryStream.ToArray());
            PlayerPrefs.SetString(prefKey, serializedData);
            PlayerPrefs.Save();
            // Debug.Log(string.Format("Saved serialized data {0}", serializedData));
        }

        public static T Load<T>(string prefKey)
        {
            if (!PlayerPrefs.HasKey(prefKey))
                return (T)Activator.CreateInstance(typeof(T));

            string serializedData = PlayerPrefs.GetString(prefKey, string.Empty);
            if (serializedData == string.Empty)
                return (T)Activator.CreateInstance(typeof(T));
            MemoryStream dataStream = new MemoryStream(System.Convert.FromBase64String(serializedData));
            T result = (T)bf.Deserialize(dataStream);
            //Debug.Log(string.Format("Loaded: {0} => {1}", prefKey, result.GetType()));
            return result;
        }
    }

}