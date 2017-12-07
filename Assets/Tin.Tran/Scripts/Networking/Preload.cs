using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Preload : MonoBehaviour
{
    public static Preload Global;
    public Text TxtDebug;



    public TinNetwork manager;
    Discovery dc;



    private void Awake()
    {
        Global = this;
        manager = gameObject.GetComponent<TinNetwork>();
        dc = gameObject.GetComponent<Discovery>();
    }

    void Start()
    {
        Debug.Log("editor");
        dc.Server_isServer = false;
        dc.Client_ReListen = true;
        dc.OnreceiveRequest += OnListenServer;

        dc.Discover();
        StartCoroutine(WaitForConnect());
    }
    private IEnumerator WaitForConnect()
    {
        yield return new WaitForSeconds(2);
        if (manager.isNetworkActive == true && manager.IsClientConnected() == true) yield break;
        Debug.Log("*** Start server ***");
        dc.StopReceiveData();
        try
        {
            manager.StartHost();
        }
        catch
        {
            OnListenServer("aa", Network.player.ipAddress);
        }
        dc.Server_isServer = true;
        dc.Server_TimeRepeat = 2;
        dc.Server_Message = manager.IsClientConnected().ToString();
        dc.Discover();
    }
    private void OnListenServer(string message, string ip)
    {
        // listen from discovery
        if (manager.client != null && manager.client.isConnected)
        {
            return;
        }
        Debug.Log("Connect to server! ip: \"" + ip + "\"");
        manager.networkAddress = ip;
        NetworkClient cl = manager.StartClient();
        cl.RegisterHandler(MsgType.Connect, OnConnected);
        cl.RegisterHandler(99, OnScore);
    }
    public void OnScore(NetworkMessage netMsg)
    {
        ChatMessage msg = netMsg.ReadMessage<ChatMessage>();
        Debug.Log("OnReceiveMessage ==> Name: " + msg.Name + "  // Content: " + msg.Content);
    }
    public void SendMS(string name, string content)
    {
        ChatMessage msg = new ChatMessage();
        msg.Name = name;
        msg.Content = content;
        NetworkServer.SendToAll(99, msg);
    }
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("*********Connected to server");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (manager.isNetworkActive)
            {
                Debug.Log("Connected: " + manager.client.isConnected);
            }
            else
                Debug.Log("tab");
        }
    }
}