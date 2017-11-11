using UnityEngine;
using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Alta.Plugin
{
    /// <summary>
    /// custom playerpef
    /// </summary>
    public static class APlayerPrefs
    {
        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public static void Save()
        {
            PlayerPrefs.Save();
        }
        public static void DeleteKey(string key)
        {
            string sHeader = PlayerPrefs.GetString(key);
            if (!string.IsNullOrEmpty(sHeader))
            {
                Header h = JsonUtility.FromJson<Header>(sHeader);
                if (h != null)
                {
                    Type dataType = Type.GetType(h.TypeInfo);
                    FieldInfo[] typeInfos = dataType.GetFields();
                    foreach (FieldInfo f in typeInfos)
                    {
                        PlayerPrefs.DeleteKey(h.Code + "_" + f.Name);
                    }
                }
            }
            PlayerPrefs.DeleteKey(key);
        }
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }


        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
        public static int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
        public static string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public static float GetFloat(string key, float defaultValue = 0.0F)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        /// <summary>
        /// cache an object in unity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">object cache </param>
        /// <param name="key">key cache</param>
        /// <param name="clean">reset data </param>
        public static void Set<T>(this T data, string key, bool clean = false)
        {

            Type dataType = typeof(T);
            FieldInfo[] typeInfos = dataType.GetFields();

            if (typeInfos != null)
            {
                Header h = null;
                if (PlayerPrefs.HasKey(key))
                {
                    string sHeader = PlayerPrefs.GetString(key);
                    if (!string.IsNullOrEmpty(sHeader))
                    {
                        h = JsonUtility.FromJson<Header>(sHeader);
                    }
                }


                if (h == null)
                {
                    h = new Header() { Code = data.GetHashCode(), NumberFieldInfo = typeInfos.Length, TypeInfo = dataType.FullName };
                    PlayerPrefs.SetString(key, JsonUtility.ToJson(h));
                }


                foreach (FieldInfo f in typeInfos)
                {
                    var _mData = f.GetValue(data);
                    if (_mData is string)
                    {
                        PlayerPrefs.SetString(h.Code + "_" + f.Name, _mData.ToString());
                    }
                    else if (_mData is int)
                    {
                        PlayerPrefs.SetInt(h.Code + "_" + f.Name, Convert.ToInt32(_mData));
                    }
                    else if (_mData is float)
                    {
                        PlayerPrefs.SetFloat(h.Code + "_" + f.Name, Convert.ToSingle(_mData));
                    }
                    else if (!f.FieldType.IsClass)
                    {
                        PlayerPrefs.SetString(h.Code + "_" + f.Name, _mData.ToString());
                    }
                    else
                    {
                        string base64 = JsonConvert.SerializeObject(_mData);
                        PlayerPrefs.SetString(h.Code + "_" + f.Name, base64);

                    }
                }
                PlayerPrefs.Save();
            }
        }

        /// <summary>
        /// return an object corresponding to key 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key to cache</param>
        /// <returns>object</returns>

        public static T Get<T>(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                Type dataType = typeof(T);
                FieldInfo[] typeInfos = dataType.GetFields();
                T data = (T)Activator.CreateInstance(typeof(T));

                string sHeader = PlayerPrefs.GetString(key);
                if (!string.IsNullOrEmpty(sHeader))
                {
                    Header h = JsonUtility.FromJson<Header>(sHeader);
                    if (h != null)
                    {
                        foreach (FieldInfo f in typeInfos)
                        {
                            var _mData = f.GetValue(data);
                            if (_mData is string)
                            {
                                f.SetValue(data, PlayerPrefs.GetString(h.Code + "_" + f.Name));
                            }
                            else if (_mData is int)
                            {
                                f.SetValue(data, PlayerPrefs.GetInt(h.Code + "_" + f.Name));
                            }
                            else if (_mData is float)
                            {
                                f.SetValue(data, PlayerPrefs.GetFloat(h.Code + "_" + f.Name));
                            }
                            else if (!f.FieldType.IsClass)
                            {
                                f.SetValue(data, Convert.ChangeType(PlayerPrefs.GetString(h.Code + "_" + f.Name), f.FieldType));
                            }
                            else
                            {
                                f.SetValue(data, JsonConvert.DeserializeObject(PlayerPrefs.GetString(h.Code + "_" + f.Name), f.FieldType));
                            }
                        }
                        return data;
                    }
                }

            }
            return default(T);
        }

        private class Header
        {
            public int Code;
            public int NumberFieldInfo;
            public string TypeInfo;
        }
    }
}

