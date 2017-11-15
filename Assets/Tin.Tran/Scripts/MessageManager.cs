using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Alta.Plugin;

public class MessageManager : NetworkBehaviour
{
    public InputField NewMessage;
    public GameObject MessagePrefab;
    public Transform MessagesParent;
    T_NetworkManager manager;
    private void Start()
    {
        manager = T_NetworkManager._singleton;
        manager.OnReceiveMessage += OnreceiveMessage;
    }

    private void OnreceiveMessage(NetworkMessage obj)
    {
        ChatMessage msg = obj.ReadMessage<ChatMessage>();
        if (isServer && msg.Name != manager.Name)
        {
            NetworkServer.SendToAll(99, msg);
            return;
        }
        Debug.Log("OnReceiveMessage ==> Name: " + msg.Name + "  // Content: " + msg.Content);
        if (msg.Name == manager.Name)
        {
            DisplayMessage(msg.Name, msg.Content, MsType.OUT);
        }
        else
        {
            DisplayMessage(msg.Name, msg.Content, MsType.IN);
        }
    }
    public void SendMS(string name, string content)
    {
        ChatMessage msg = new ChatMessage();
        msg.Name = name;
        msg.Content = content;
        if (isClient)
            manager.client.Send(99, msg);
        else if (isServer)
            NetworkServer.SendToAll(99, msg);
    }
    public void Send()
    {
        if (string.IsNullOrEmpty(NewMessage.text)) return;
        SendMS(manager.Name, NewMessage.text);
        // DisplayMessage("", NewMessage.text, MsType.OUT);
        NewMessage.text = "";
    }
    public void DisplayMessage(string name, string content, MsType type)
    {
        GameObject go = MessagesParent.CreateChild(MessagePrefab);
        Text txt = go.GetComponent<Text>();
        string msg = "";
        if (type == MsType.OUT)
        {
            txt.alignment = TextAnchor.MiddleRight;
            msg = content;
        }
        else
        {
            txt.alignment = TextAnchor.MiddleLeft;
            msg = name + ": " + content;
        }
        txt.text = content;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Send();
        }
    }
}
