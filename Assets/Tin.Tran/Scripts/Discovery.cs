using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Discovery : MonoBehaviour
{
    public delegate void OnreceiveConnectReques(string message, string ip);
    public event OnreceiveConnectReques OnreceiveRequest;

    public bool Server_isServer;
    public string Server_Message;
    public float Server_TimeRepeat;

    public bool Client_ReListen;


    UdpClient sender;
    UdpClient receiver;
    int remotePort = 19784;
    string currentMsg;
    string currentIp;
    bool send = false;
    public void Discover()
    {
        if (Server_isServer)
        {
            sender = new UdpClient(6789, AddressFamily.InterNetwork);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, remotePort);
            sender.Connect(groupEP);
            InvokeRepeating("SendData", 0, Server_TimeRepeat);
        }
        else
            StartReceivingIP();
    }
    void SendData()
    {
        if (!string.IsNullOrEmpty(Server_Message))
        {
            sender.Send(Encoding.ASCII.GetBytes(Server_Message), Server_Message.Length);
            Debug.Log(Server_Message);
        }
    }
    public void StopReceiveData()
    {
        receiver = null;
    }
    public void StartReceivingIP()
    {
        try
        {
            if (receiver == null)
            {
                receiver = new UdpClient(remotePort);
                receiver.BeginReceive(new AsyncCallback(ReceiveData), null);
                Debug.Log("****** Start Receiving IP ******");
            }
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message);
        }
    }
    private void ReceiveData(IAsyncResult result)
    {
        IPEndPoint receiveIPGroup = new IPEndPoint(IPAddress.Any, remotePort);
        byte[] received;
        if (receiver != null)
        {
            received = receiver.EndReceive(result, ref receiveIPGroup);
        }
        else
        {
            return;
        }
        if (Client_ReListen)
            receiver.BeginReceive(new AsyncCallback(ReceiveData), null);
        string receivedString = Encoding.ASCII.GetString(received);
        currentIp = receiveIPGroup.Address.ToString();
        currentMsg = receivedString;
#if UNITY_EDITOR
        Debug.Log("Receive data: " + receivedString);
#endif
        send = true;
    }
    private void Update()
    {
        if (send == true)
        {
            send = false;
            if (OnreceiveRequest != null) OnreceiveRequest(currentMsg, currentIp);
            return;
        }
    }
}
