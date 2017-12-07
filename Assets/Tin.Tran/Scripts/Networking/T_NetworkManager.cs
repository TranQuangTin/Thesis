using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class T_NetworkManager : NetworkManager
{
    public string Name;
    public event Action<NetworkMessage> OnReceiveMessage;
    public static T_NetworkManager _singleton
    {
        get;
        protected set;
    }
    private void OnEnable()
    {
        _singleton = this;
    }
    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        client.RegisterHandler(99, OnMessage);
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler(99, OnMessage);
    }
    private void OnMessage(NetworkMessage netMsg)
    {
        if (OnReceiveMessage != null)
        {
            OnReceiveMessage(netMsg);
        }
    }
}
