using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
namespace Alta.Plugin
{
    [Flags]
    public enum FileType
    {
        PNG = 1, JPG = 2
    }
    public static class TextureExtension
    {
        /// <summary>
        /// convert RenderTexture to Texture2D
        /// </summary>
        /// <param name="rt">RenderTexture input</param>
        /// <param name="format">TextureFormat output</param>
        /// <returns>Texture2D</returns>
        public static Texture2D CopyRenderTexture(this RenderTexture rt, TextureFormat format = TextureFormat.ARGB32)
        {
            RenderTexture prevRT = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D texture = new Texture2D(rt.width, rt.height, format, false);
           
            texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            texture.Apply(false, false);
            RenderTexture.active = prevRT;
            return texture;
        }
        /// <summary>
        /// save Texture2D to file
        /// </summary>
        /// <param name="texture">input Texture2D</param>
        /// <param name="filePrefixName">prefix of file</param>
        /// <param name="type">file type</param>
        /// <returns>list file path</returns>
        public static List<string> SaveTextureToFile(this Texture2D texture, string filePrefixName, FileType type = FileType.JPG)
        {
            List<string> fileNames = new List<string>();
            List<FileType> types = new List<FileType>();
            if (EnumerationExtensions.has(type, FileType.PNG))
            {
                fileNames.Add(string.Format("{0}.png", filePrefixName));
                types.Add(FileType.PNG);
            }
            if (EnumerationExtensions.has(type, FileType.JPG))
            {
                fileNames.Add(string.Format("{0}.jpg", filePrefixName));
                types.Add(FileType.JPG);
            }
            for (int i = 0; i < fileNames.Count; i++)
            {
                using (Stream s = File.Open(fileNames[i], FileMode.Create))
                {
                    byte[] bytes;
                    if (types[i] == FileType.PNG)
                    {
                        bytes = texture.EncodeToPNG();
                    }
                    else
                    {
                        bytes = texture.EncodeToJPG();
                    }
                    BinaryWriter binary = new BinaryWriter(s);
                    binary.Write(bytes);
                    s.Close();
                }
            }
            return fileNames;
        }
        /// <summary>
        /// flip texture2d
        /// </summary>
        /// <param name="original">input Texture2D</param>
        /// <param name="flipX">hozizontal flip</param>
        /// <param name="flipY">vertical flip</param>
        /// <returns>output Texture2D</returns>
        public static Texture2D FlipTexture(this Texture2D original, bool flipX, bool flipY)
        {
            Texture2D flipped = new Texture2D(original.width, original.height);

            int xN = original.width;
            int yN = original.height;


            if (flipY)
            {
                for (int x = 0; x < xN; x++)
                {
                    for (int y = 0; y < yN; y++)
                    {
                        if (flipY && !flipX)
                            flipped.SetPixel(x, yN - (y + 1), original.GetPixel(x, y));
                        else if (flipX && !flipY)
                            flipped.SetPixel(xN - (x + 1), y, original.GetPixel(x, y));
                        else if (flipX && flipY)
                            flipped.SetPixel(xN - (x + 1), yN - (y + 1), original.GetPixel(x, y));
                    }

                }
            }
            flipped.Apply();

            return flipped;
        }
    }
}