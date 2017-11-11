using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml;
namespace Alta.Plugin
{
    public static class XmlExtention
    {
        private static Dictionary<int, string> mapDatabase;
       /// <summary>
       /// convert string xml to object
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="content">string xml input</param>
       /// <returns>object</returns>
        public static T ReadContent<T>(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                try
                {
                    using (Stream s = content.GenerateStreamFromString())
                    {
                        System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(T));
                        return (T)reader.Deserialize(s);
                    }
                }
                catch (Exception)
                {
                    return (T)Activator.CreateInstance(typeof(T));
                }
            }
        }
        /// <summary>
        /// convert file xml to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">file xml path</param>
        /// <returns>object</returns>
        public static T Read<T>(string file)
        {
            if (mapDatabase == null)
            {
                mapDatabase = new Dictionary<int, string>();
            }

            if (!File.Exists(file))
            {
                T tmp = (T)Activator.CreateInstance(typeof(T));
                if (!mapDatabase.ContainsKey(tmp.GetHashCode()))
                    mapDatabase.Add(tmp.GetHashCode(), file);
                Write(tmp, file);
                return tmp;
            }
            else
            {
                try
                {
                    using (Stream s = File.Open(file, FileMode.Open))
                    {
                        System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(T));                      
                        T tmp= (T)reader.Deserialize(s);
                        if(!mapDatabase.ContainsKey(tmp.GetHashCode()))
                            mapDatabase.Add(tmp.GetHashCode(), file);
                        return tmp;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Lỗi không đọc được file XML: " + ex.ToString());
                    return (T)Activator.CreateInstance(typeof(T));
                }

            }
        }
        /// <summary>
        /// convert file xml to object with decryption key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file">file xml path</param>
        /// <param name="key">decryption key</param>
        /// <returns></returns>
        public static T Read<T>(string file, string key)
        {
            if (mapDatabase == null)
            {
                mapDatabase = new Dictionary<int, string>();
            }

            if (!File.Exists(file))
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
            T data = default(T);
            using (RijndaelManaged RMCrypto = new RijndaelManaged())
            {
                byte[] Key = ("Alta@2015" + key).md5().Clone(0.5f).toBytes();
                byte[] IV = { 0x01, 0x02, 0x03, 0x04, 0x32, 0x06, 0x07, 0x08, 0x92, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x76 };
                RMCrypto.Mode = CipherMode.CBC;
                RMCrypto.Padding = PaddingMode.Zeros;
                var keyRMC = RMCrypto.CreateDecryptor(Key, IV);
                using (Stream s = File.Open(file, FileMode.Open))
                {
                    if (s.Length > 0)
                    {
                        using (CryptoStream CryptStream = new CryptoStream(s, keyRMC, CryptoStreamMode.Read))
                        {
                            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(T));

                            try
                            {
                                data = (T)reader.Deserialize(CryptStream);
                                if (!mapDatabase.ContainsKey(data.GetHashCode()))
                                    mapDatabase.Add(data.GetHashCode(), file);
                            }
                            catch (Exception ex)
                            {
                                Debug.Log(ex.GetBaseException().ToString());
                                data = default(T);
                            }
                        }
                        s.Close();
                    }
                    else
                    {
                        s.Close();
                        File.Delete(file);
                    }


                }
            }
            return data;
        }

        /// <summary>
        /// convert object to string xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="overview">object input</param>
        /// <returns>string xml </returns>

        public static string SerializeObject<T>(this T overview)
        {
            System.Xml.XmlWriterSettings setting = new System.Xml.XmlWriterSettings();
            setting.Encoding = Encoding.UTF8;
            setting.CloseOutput = true;
            setting.NewLineChars = "\r\n";
            setting.Indent = true;
            setting.OmitXmlDeclaration = true;
            XmlSerializer xmlSerializer = new XmlSerializer(overview.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            using (StringWriter textWriter = new StringWriter())
            {
                XmlWriter xmlWriter = XmlWriter.Create(textWriter, setting);
                xmlSerializer.Serialize(xmlWriter, overview, ns);
                return textWriter.ToString();
            }
        }

        /// <summary>
        /// write object to file 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="overview">object input</param>
        /// <param name="file">file path to write</param>
        public static void Write<T>(this T overview, string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new Exception("File Not Empty");
            FileInfo inf = new FileInfo(file);
            if (!inf.Directory.Exists)
            {
                Directory.CreateDirectory(inf.Directory.FullName);
            }
            XmlSerializer writer = new XmlSerializer(typeof(T));
            XmlWriterSettings setting = new XmlWriterSettings();
            setting.Encoding = Encoding.UTF8;
            setting.CloseOutput = true;
            setting.NewLineChars = "\r\n";
            setting.Indent = true;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            if (!File.Exists(file))
            {
                using (Stream s = File.Open(file, FileMode.OpenOrCreate))
                {
                    XmlWriter tmp = System.Xml.XmlWriter.Create(s, setting);
                    writer.Serialize(tmp, overview, ns);
                }
            }
            else
            {
                using (Stream s = File.Open(file, FileMode.Truncate))
                {
                    XmlWriter tmp = System.Xml.XmlWriter.Create(s, setting);
                    writer.Serialize(tmp, overview, ns);
                }
            }
        }

        /// <summary>
        /// write object to file with file cache. if not found file path in cache throw exption
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="overview">object input</param>
        public static void Write<T>(this T overview)
        {
            string file = string.Empty;
            if (mapDatabase == null)
                return;
            if (mapDatabase.ContainsKey(overview.GetHashCode()))
            {
                file = mapDatabase[overview.GetHashCode()];
            }
#if UNITY_EDITOR
            Debug.LogWarning(file);
#endif
            if (!string.IsNullOrEmpty(file))
                Write(overview, file);
            else throw new Exception("file not found");
        }

        /// <summary>
        /// write object to xml file with encryption key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">object input</param>
        /// <param name="key">encryption key</param>
        /// <param name="filename"> file path</param>
        public static void Write<T>(this T data,  string key, string filename)
        {
            using (RijndaelManaged RMCrypto = new RijndaelManaged())
            {
                byte[] Key = ("Alta@2015" + key).md5().Clone(0.5f).toBytes();
                byte[] IV = { 0x01, 0x02, 0x03, 0x04, 0x32, 0x06, 0x07, 0x08, 0x92, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x76 };
                RMCrypto.Mode = CipherMode.CBC;
                RMCrypto.Padding = PaddingMode.Zeros;

                System.Xml.XmlWriterSettings setting = new System.Xml.XmlWriterSettings();
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                setting.Encoding = Encoding.UTF8;
                setting.CloseOutput = true;
                setting.NewLineChars = "\r\n";
                if (!File.Exists(filename))
                {

                    using (Stream s = File.Open(filename, FileMode.OpenOrCreate))
                    {
                        using (CryptoStream CryptStream = new CryptoStream(s, RMCrypto.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                        {
                            System.Xml.XmlWriter formatter = System.Xml.XmlWriter.Create(CryptStream, setting);
                            writer.Serialize(formatter, data);
                            CryptStream.FlushFinalBlock();
                            CryptStream.Close();
                        }
                        s.Close();
                    }
                }
                else
                {
                    using (Stream s = File.Open(filename, FileMode.Truncate))
                    {
                        using (CryptoStream CryptStream = new CryptoStream(s, RMCrypto.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                        {
                            System.Xml.XmlWriter formatter = System.Xml.XmlWriter.Create(CryptStream, setting);
                            writer.Serialize(formatter, data);
                            CryptStream.FlushFinalBlock();
                            CryptStream.Close();
                        }
                        s.Close();
                    }
                }
            }
        }
    }
}