using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Security.Cryptography;
using System;
namespace Alta.Plugin
{
    public static class FileExtensiton
    {
        /// <summary>
        /// get md5 of file
        /// </summary>
        /// <param name="file"> file infomation</param>
        /// <returns>string md5</returns>
        public static string md5(this FileInfo file)
        {
            if (file.Exists)
            {
                using (var md5Hash = MD5.Create())
                {
                    using (var stream = File.OpenRead(file.FullName))
                    {
                        byte[] data = md5Hash.ComputeHash(stream);
                        string tmp = BitConverter.ToString(data).Replace("-", "");
                        return tmp.ToLower();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// save byte array to file
        /// </summary>
        /// <param name="data">byte array data</param>
        /// <param name="fileName">string file name</param>
        /// <param name="_override">override old file</param>
        public static void SaveFile(this byte[] data, string fileName, bool _override = false)
        {
            FileInfo file = new FileInfo(fileName);
            if (_override)
            {
                System.IO.File.WriteAllBytes(file.FullName, data);
            }
            else if (!File.Exists(fileName))
            {
                System.IO.File.WriteAllBytes(file.FullName, data);
            }
        }

        /// <summary>
        /// load file to teture
        /// </summary>
        /// <param name="file">file infomation</param>
        /// <param name="format">TextureFormat</param>
        /// <returns>texture 2D</returns>

        public static Texture2D LoadTexture2DNative(this FileInfo file, TextureFormat format = TextureFormat.RGB24)
        {
            Texture2D tex = new Texture2D(2, 2, format, false);
            byte[] data = File.ReadAllBytes(file.FullName);
            tex.LoadImage(data);
            return tex;
        }
    }
}
