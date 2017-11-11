using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Alta.Plugin;

namespace Alta.INetwork
{
    /// <summary>
    /// a server on the network
    /// </summary>
    [RequireComponent(typeof(NetworkView))]
    public class SimpleNetworkSever : NetworkBase
    {
        private new NetworkView networkView;
        public bool autoRun;
        public bool canRun = true;
        /// <summary>
        /// list of the clients connected to server
        /// </summary>
        public List<Client> m_aryClients;

        public int currentNumberClient
        {
            get
            {
                if (this.m_aryClients == null)
                    return -1;
                return m_aryClients.Count;
            }
        }


        /// <summary>
        /// Check kind of client connected to server
        /// </summary>
        /// <param name="type">kind of client</param>
        /// <returns>true: kind of client connectd
        /// false: if no client of this kind connect
        /// </returns>
        internal bool hasType(TypeClient type)
        {
            if (this.m_aryClients == null || this.m_aryClients.Count == 0)
                return false;
            return this.m_aryClients.FindIndex(c => c.Type == type) >= 0;
        }

        new void Awake()
        {
            m_aryClients = new List<Client>();
            networkView = GetComponent<NetworkView>();
        }
        // Use this for initialization
        void Start()
        {
            
            if (this.autoRun) // auto start server
            {
                this.InitializeServer();
            }
        }
        /// <summary>
        /// start server
        /// </summary>
        public void InitializeServer()
        {
            this.canRun = true;
            if (Network.peerType == NetworkPeerType.Disconnected)
            {
                Network.InitializeServer(20, port, false);
            }

        }

        /// <summary>
        /// send string message to all client
        /// </summary>
        /// <param name="msg">string message</param>
        public void SendTo(string msg)
        {
            Debug.LogWarning("S->C: " + msg);
            if (this.m_aryClients.Count > 0)
            {
                networkView.RPC("OnReceiverString", RPCMode.Others, msg);
            }
        }

        /// <summary>
        /// Send string message to a client
        /// </summary>
        /// <param name="msg">string message</param>
        /// <param name="player">infomation of client </param>
        public void SendTo(string msg, NetworkPlayer info)
        {
            if (this.m_aryClients.Count > 0)
            {
                foreach (Client c in this.m_aryClients)
                {
                    if (c.player == info)
                    {
                        networkView.RPC("OnReceiverString", c.player, msg);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// delay send message to a client 
        /// </summary>
        /// <param name="msg">string message</param>
        /// <param name="info">infomation of client</param>
        /// <param name="timeDelay">time delay</param>
        /// <returns></returns>

        public IEnumerator SendTo(string msg, NetworkPlayer info, float timeDelay)
        {
            yield return new WaitForSeconds(timeDelay);
            this.SendTo(msg, info);
        }

        /// <summary>
        /// Send string message to group client
        /// </summary>
        /// <param name="msg">string message</param>
        /// <param name="Type">kind of client</param>
        public void SendTo(string msg, TypeClient Type)
        {
            if (this.m_aryClients.Count > 0)
            {
                foreach (Client c in this.m_aryClients)
                {
	                if (Type.has(c.Type))
                    {
                        this.networkView.RPC("OnReceiverString", c.player, msg);
                    }
                }
            }
        }

        /// <summary>
        /// send object to group client
        /// </summary>
        /// <typeparam name="T">serializebase object</typeparam>
        /// <param name="data">object message</param>
        /// <param name="type">kind of client</param>

        public void SendObj<T>(T data, TypeClient type)
        {
            foreach (Client c in this.m_aryClients)
            {
	            if (type.has(c.Type))
                {
                    this.SendObj<T>(data, c);
                }
            }
        }
        /// <summary>
        /// delay send object to a client
        /// </summary>
        /// <typeparam name="T">serializebase object</typeparam>
        /// <param name="data">object message</param>
        /// <param name="client">client receiver</param>
        public void SendObj<T>(T data, Client client)
        {
            PackageTranfer pdata = new PackageTranfer();
            pdata.TYPE = data.GetType().ToString();
            pdata.DataTranfer = data.SerializeObjectByte();
#if UNITY_EDITOR
            Debug.LogWarning("S->C: " + pdata.DataTranfer.Length);
#endif
            this.networkView.RPC("OnReceiverData", client.player, pdata.SerializeObjectByte());
        }


        /// <summary>
        /// delay send object to client
        /// </summary>
        /// <typeparam name="T">serializebase object</typeparam>
        /// <param name="data"> object message</param>
        /// <param name="client">client receiver </param>
        /// <param name="timeDelay">time delay</param>
        /// <returns></returns>
        public IEnumerator SendObj<T>(T data, Client client, float timeDelay)
        {
            yield return new WaitForSeconds(timeDelay);
            this.SendObj(data, client);
        }

        /// <summary>
        /// send object to all client in the network
        /// </summary>
        /// <typeparam name="T">serializebase object</typeparam>
        /// <param name="data">object message</param>
        public void SendObj<T>(T data)
        {
            PackageTranfer pdata = new PackageTranfer();
            pdata.TYPE = data.GetType().ToString();
            pdata.DataTranfer = data.SerializeObjectByte();
#if UNITY_EDITOR
            Debug.LogWarning("S->C: " + pdata.DataTranfer.Length);
#endif
            this.networkView.RPC("OnReceiverData", RPCMode.Others, pdata.SerializeObjectByte());
        }


        /// <summary>
        /// delay send string message to group in network
        /// </summary>
        /// <param name="msg">string message</param>
        /// <param name="Type">kind of client</param>
        /// <param name="TimeDelay">time delay</param>
        /// <returns></returns>
        public IEnumerator SendTo(string msg, TypeClient Type, float TimeDelay)
        {
            yield return new WaitForSeconds(TimeDelay);
            this.SendTo(msg, Type);
        }
        [RPC]
        public void OnReceiverVector(Vector2 v)
        {
#if UNITY_EDITOR
            Debug.LogWarning(v);
#endif
        }
        /// <summary>
        /// method listen string message of client
        /// </summary>
        /// <param name="msg">string message</param>
        /// <param name="info">infomation of client</param>
        [RPC]
        public void OnReceiverString(string msg, NetworkMessageInfo info)
        {
            TypeClient type = msg.ToUpper().Trim().ToEnum<TypeClient>(TypeClient.None);
            string[] cmds = msg.Split('_', '|');
            
            if (type!= TypeClient.None)            {
                if (this.m_aryClients.Count > 0)
                {
                    int index = this.m_aryClients.FindIndex(cI => cI.player == info.sender);                    
                    if(index<0)
                        return;
                    this.m_aryClients[index].Type = type;  
                }
            }
            base.OnReceiverString(msg, info);
        }

        /// <summary>
        /// method listen object message of client
        /// </summary>
        /// <param name="data">object message</param>
        /// <param name="info">infomation of client</param>
        [RPC]
        public void OnReceiverData(byte[] data, NetworkMessageInfo info)
        {
#if UNITY_EDITOR
            Debug.LogWarning("S->C: " + data.Length);
#endif
            PackageTranfer netData = data.DeserializeByte<PackageTranfer>();
            Client cInfo = this.m_aryClients.Where(c => c.player == info.sender).FirstOrDefault();
            DataNetwork sender = new DataNetwork() { Data = netData, Info = info.sender };
            if (cInfo != null)
            {
                sender.type = cInfo.Type;
            }
            OnReceiverData(sender);
        }

        /// <summary>
        /// Called on the server whenever a new client has successfully connected.
        /// </summary>
        /// <param name="player">client connected</param>
        void OnPlayerConnected(NetworkPlayer player)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Client Connect: " + player.ipAddress);
#endif
            m_aryClients.Add(new Client() { player = player });
        }
        /// <summary>
        /// Called on the server whenever a player disconnected from the server.
        /// </summary>
        /// <param name="player"></param>
        void OnPlayerDisconnected(NetworkPlayer player)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Client Disconnect: " + player.ipAddress);
#endif
            if (this.m_aryClients != null && this.m_aryClients.Count > 0)
            {
                
                foreach (Client c in this.m_aryClients)
                {
                    if (c.player == player)
                    {
                        this.m_aryClients.Remove(c);
                        return;
                    }
                }
            }

        }
       
    }
}
