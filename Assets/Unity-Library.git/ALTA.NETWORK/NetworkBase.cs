using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Alta.Plugin;

namespace Alta.INetwork
{
    [RequireComponent(typeof(NetworkView))]
    public  abstract partial class NetworkBase : MonoBehaviour
    {
        public int port = 6789;
        protected NetworkView networkViewBase;
        public bool status;

        public event EventHandler<StringNetwork> OnReceiverStringEvent;
        public event EventHandler<DataNetwork> OnRecevicerDataEvent;

        public bool Connected
        {
            get
            {
                return Network.peerType != NetworkPeerType.Disconnected;
            }
        }

        public void Awake()
        {
            this.networkViewBase = GetComponent<NetworkView>();
        }



        public void Update()
        {
            this.status = Network.peerType != NetworkPeerType.Disconnected;
        }

        public virtual void DisConnect()
        {
            Network.Disconnect();
        }

        #region Send Message
        /// <summary>
        /// Send string cross network socket
        /// </summary>
        /// <param name="msg"> string data</param>
        /// <param name="Rpc"> send to other </param>
        /// <param name="methodName"> method recevier data</param>
        public void SendMsg(string msg, RPCMode Rpc, string methodName = "OnReceiverString")
        {
            this.networkViewBase.RPC(methodName, Rpc, msg);
        }

        /// <summary>
        /// send delay string to the network socket
        /// </summary>
        /// <param name="msg"> string data</param>
        /// <param name="Rpc"> send to other</param>
        /// <param name="methodName">method recevier data </param>
        /// <param name="timeDelay">time delay</param>
        /// <returns></returns>

        public IEnumerator SendMsg(string msg, RPCMode Rpc, string methodName, float timeDelay)
        {
            yield return new WaitForSeconds(timeDelay);
            this.SendMsg(msg, Rpc, methodName);
        }
        /// <summary>
        /// Send object data to network socket
        /// </summary>
        /// <typeparam name="T"> Serialbase object</typeparam>
        /// <param name="data">object data</param>
        /// <param name="Rpc"> send to other </param>
        /// <param name="methodName">method recevier data</param>

        public void SendObj<T>(T data, RPCMode Rpc, string methodName = "OnReceiverData")
        {
            PackageTranfer package = new PackageTranfer() { DataTranfer = data.SerializeObjectByte(), TYPE = data.GetType().ToString() };
            this.networkViewBase.RPC(methodName, Rpc, package.SerializeObjectByte());
        }

        #endregion

        #region OnReceiver
        /// <summary>
        /// receiver string in the network
        /// </summary>
        /// <param name="msg">data string</param>
        [RPC]
        public void OnReceiverString(string msg)
        {
#if UNITY_EDITOR
            Debug.LogWarning("S->C: " + msg);
#endif
            if (OnReceiverStringEvent != null)
            {
                OnReceiverStringEvent(this, new StringNetwork() { MSG = msg });
            }
            transform.parent.SendMessage("OnListenString", msg);

        }
        /// <summary>
        /// receiver byte array in the network
        /// </summary>
        /// <param name="data"> byte array data</param>
        [RPC]
        public void OnReceiverData(byte[] data)
        {
#if UNITY_EDITOR
            Debug.LogWarning("S->C: " + data.Length);
#endif
            PackageTranfer netData = data.DeserializeByte<PackageTranfer>();
            DataNetwork sender = new DataNetwork() { Data = netData };
            if (OnRecevicerDataEvent != null)
            {
                OnRecevicerDataEvent(this, sender);
            }
            else
            {
                transform.parent.SendMessage("OnListenData", sender);
            }
        }

        /// <summary>
        /// recivier byte array in the network
        /// </summary>
        /// <param name="data"> byte array data</param>
        /// <param name="info"> infomation of sender</param>
        [RPC]
        public void OnReceiverData(byte[] data, NetworkMessageInfo info)
        {
#if UNITY_EDITOR
            Debug.LogWarning("S->C: " + data.Length);
#endif
            PackageTranfer netData = data.DeserializeByte<PackageTranfer>();            
            DataNetwork sender = new DataNetwork() { Data = netData, Info = info.sender};
            this.OnReceiverData(sender);
           
        }
        /// <summary>
        /// Recevier datanetwork in the network
        /// </summary>
        /// <param name="data"> a data network</param>
        public void OnReceiverData(DataNetwork data)
        {
            if (OnRecevicerDataEvent != null)
            {
                OnRecevicerDataEvent(this, data);
            }
            else
            {
                transform.parent.SendMessage("OnListenData", data);
            }
        }
        /// <summary>
        /// Receiver string in the network
        /// </summary>
        /// <param name="msg"> string data receiver </param>
        /// <param name="info"> infomation of sender</param>
        [RPC]
        public void OnReceiverString(string msg, NetworkMessageInfo info)
        {
#if UNITY_EDITOR
            Debug.LogWarning("S->C: " + msg);
#endif
            if (OnReceiverStringEvent != null)
            {
                OnReceiverStringEvent(this, new StringNetwork() { MSG = msg, Info = info.sender });
            }
            else
            {
                if(transform.parent!=null)
                    transform.parent.SendMessage("OnListenString", msg);
                else
                    SendMessage("OnListenString", msg);
            }
        }

        /// <summary>
        /// receiver string data from network
        /// </summary>
        /// <param name="msg">string data</param>
        /// <param name="c">client sender</param>
        public void OnReceiverString(string msg, Client c)
        {
#if UNITY_EDITOR
            Debug.LogWarning("C->S: " + msg);
#endif
            if (OnReceiverStringEvent != null)
            {
                OnReceiverStringEvent(this, new StringNetwork() { MSG = msg, Info = c.player, type = c.Type });
            }

        }
        #endregion
        private void OnDestroy()
        {
            this.DisConnect();
        }

    }
    public class NetworkEvent : EventArgs
    {
        public NetworkPlayer Info { get; set; }
        public TypeClient type { get; set; }
    }

    public class StringNetwork : NetworkEvent
    {
        public string MSG { get; set; }
    }

    public class DataNetwork : NetworkEvent
    {
        public PackageTranfer Data;
    }
}
