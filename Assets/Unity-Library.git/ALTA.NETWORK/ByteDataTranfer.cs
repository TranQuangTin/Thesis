using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alta.Plugin;
using System;

namespace Alta.INetwork
{
    public class ByteNetwork : NetworkEvent
    {
        public byte[] Data;
    }
    public abstract partial class NetworkBase
    {
        public event EventHandler<ByteNetwork> RawByteNetworkEvent;

        #region Byte Data Tranfer

        /// <summary>
        /// Send byte array to server
        /// </summary>
        /// <param name="methodName"> method to listen on the server </param>
        /// <param name="data"> data byte array</param>
        public void SendBytes(string methodName, params byte[] data)
        {
            this.networkViewBase.RPC(methodName, RPCMode.Server, data);
        }

        /// <summary>
        /// Send byte array to server method listen RawDataTranfer
        /// </summary>
        /// <param name="data"> byte array</param>
        public void SendBytes(params byte[] data)
        {
            this.networkViewBase.RPC("RawDataTranfer", RPCMode.Server, data);
        }
        /// <summary>
        /// listen byte array 
        /// </summary>
        /// <param name="data"> byte array recevier</param>
        [RPC]
        public void RawDataTranfer(byte[] data)
        {
            if (data == null || data.Length == 0)
                return;
#if UNITY_EDITOR
            Debug.LogWarning("S->C: " + data.ConvertByteToHexString());
#endif
            if (RawByteNetworkEvent != null)
            {
                this.RawByteNetworkEvent(this, new ByteNetwork() { Data = data });
            }
        }

        /// <summary>
        /// listen byte array 
        /// </summary>
        /// <param name="data"> byte array</param>
        /// <param name="info"> sender</param>
        [RPC]
        public void RawDataTranfer(byte[] data, NetworkMessageInfo info)
        {
            if (data == null || data.Length == 0)
                return;
#if UNITY_EDITOR
            Debug.LogWarning("S->C: " + data.ConvertByteToHexString());
#endif
            if (RawByteNetworkEvent != null)
            {
                this.RawByteNetworkEvent(this, new ByteNetwork() { Data = data, Info = info.sender });
            }
        }

        #endregion
    }
}
