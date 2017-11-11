using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Alta.Plugin;

namespace Alta.INetwork
{
    /// <summary>
    /// Client of the network
    /// </summary>
    public class SimpleNetworkClient : NetworkBase
    {
        /// <summary>
        /// ip of server
        /// </summary>
        public string IP = "127.0.0.1";
        /// <summary>
        /// Awake connect automatic
        /// </summary>
        public bool StartConnect = false;

        /// <summary>
        /// time delay of reconnect
        /// </summary>
        public float TimeDelayAutoConnect = 3.0f;
        /// <summary>
        /// Define kind of client
        /// </summary>
        public string DEFINE_CLIENT = "DISPLAY";
        private float _timeReConnect = 0f;
        private bool _manualConnect = false;
        private bool _flagSendDefine = false;

        protected virtual void Start()
        {
            if (this.StartConnect)
            {
                this.Connect();
            }
        }
        /// <summary>
        /// Connect to server
        /// </summary>
        public void Connect()
        {
            if (!this.status)
            {
                _manualConnect = true;
                _flagSendDefine = false;
                Network.Connect(this.IP, this.port);
            }
        }

        new void Update()
        {
            base.Update();
            
            if (!this.Connected && this._manualConnect) // if lost connect 
            {
                // wait time to connect
                this._timeReConnect += Time.deltaTime;
                if (this._timeReConnect >= TimeDelayAutoConnect)
                {
                    this._timeReConnect = 0f;
                    //reconnect
                    this.Connect();
                }
            }
            else if (this.Connected && !this._flagSendDefine)
            {
                //send define kind of client
                this.SendMsg(this.DEFINE_CLIENT, RPCMode.Server);
                this._flagSendDefine = true;
            }

        }
        /// <summary>
        /// client send to server a string message
        /// </summary>
        /// <param name="msg">string message</param>
        /// <param name="methodName"> method listen on server default: OnReceiverString</param>
        #region Send Message
        public void SendMsg(string msg, string methodName = "OnReceiverString")
        {
            if (!this.Connected)
                return;
#if UNITY_EDITOR
            Debug.LogWarning("C->S: " + msg);
#endif
            networkViewBase.RPC(methodName, RPCMode.Server, msg);
        }

        /// <summary>
        /// delay send string message to server
        /// </summary>
        /// <param name="msg">string message</param>
        /// <param name="methodName">method listen on server default: OnReceiverString</param>
        /// <param name="timeDelay">time delay</param>
        /// <returns></returns>
        public IEnumerator SendMsg(string msg, string methodName, float timeDelay)
        {
            yield return new WaitForSeconds(timeDelay);
            this.SendMsg(msg, RPCMode.Server, methodName);
        }

        /// <summary>
        /// Client Send object to server
        /// </summary>
        /// <typeparam name="T">seriallizebase object</typeparam>
        /// <param name="data"> object message</param>
        /// <param name="methodName"> method listen on server default: OnReceiverString</param>
        public void SendObj<T>(T data, string methodName = "OnReceiverData")
        {
#if UNITY_EDITOR

            PackageTranfer package = new PackageTranfer() { DataTranfer = data.SerializeObjectByte(), TYPE = data.GetType().ToString() };
            Debug.LogWarning(string.Format("C->S: {0}[{1}]", typeof(T).ToString(), package.DataTranfer.Length));
            if (!this.Connected)
                return;

#else
            if (!this.Connected)
                return;
            PackageTranfer package = new PackageTranfer() { DataTranfer = data.SerializeObjectByte(), TYPE = data.GetType().ToString() };
#endif
            this.networkViewBase.RPC(methodName, RPCMode.Server, package.SerializeObjectByte());
        }
        #endregion
    }
}