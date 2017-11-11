using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alta.Plugin
{
    public static class ByteUltils
    {
        /// <summary>
        /// convert byte array to hex string
        /// </summary>
        /// <param name="sendData">byte array</param>
        /// <returns>hex string of byte array</returns>
        public static string ConvertByteToHexString(this byte[] sendData)
        {
            string lbStatus = "";
            for (int i = 0; i < sendData.Length; i++)
            {
                lbStatus += sendData[i].ToString("X2");
            }
            return lbStatus;
        }

        /// <summary>
        /// convert bool array to byte array
        /// </summary>
        /// <param name="bits">bool array true:1 false:0</param>
        /// <returns>byte array</returns>
        public static byte[] ToByteArray(this bool[] bits)
        {
            BitArray a = new BitArray(bits);
            return ToByteArray(a);
        }

        /// <summary>
        /// convert bit array to byte array
        /// </summary>
        /// <param name="bits">bit array</param>
        /// <returns>byte array</returns>
        public static byte[] ToByteArray(this BitArray bits)
        {
            byte[] bytes = new byte[bits.Length / 8];
            bits.CopyTo(bytes, 0);
            return bytes;
        }

    }
}
