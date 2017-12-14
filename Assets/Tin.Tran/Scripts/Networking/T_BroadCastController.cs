using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class T_BroadCastController : NetworkDiscovery
{
    public Dictionary<string, string> ListGames;
    private Dictionary<string, Coroutine> ListIPs;
    private T_NetworkManager manager;
    public static T_BroadCastController _singleton
    {
        get;
        protected set;
    }
    private void OnEnable()
    {
        _singleton = this;
    }
    private void Start()
    {
        Initialize();
        manager = T_NetworkManager._singleton;
        ListIPs = new Dictionary<string, Coroutine>();
        ListGames = new Dictionary<string, string>();
        StartAsClient();
    }
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log("Receive from NetworkDiscovery(" + fromAddress + "): " + data);
        base.OnReceivedBroadcast(fromAddress, data);
        if (!ListIPs.ContainsKey(fromAddress))
        {
            ListIPs.Add(fromAddress, StartCoroutine(RemoveTimeOut(fromAddress)));
            ListGames.Add(fromAddress, data);
        }
        else
        {
            StopCoroutine(ListIPs[fromAddress]);
            ListIPs[fromAddress] = StartCoroutine(RemoveTimeOut(fromAddress));
        }
    }
    IEnumerator RemoveTimeOut(string key)
    {
        yield return new WaitForSeconds(3);
        ListIPs.Remove(key);
        ListGames.Remove(key);
    }
    public void CreateRoom(string roomName)
    {// tạo phòng mới
        if (string.IsNullOrEmpty(LobbyController._singleton.GetName()))
        {
            Debug.Log("Haven't enter name");
            return;
        }
        StopBroadcast();
        broadcastData = roomName;
        StartAsServer();
        Debug.Log("start server");
        manager.StartHost();
    }
    public void JoinRoom(string address)
    {// gia nhập phòng có sẵn
        if (string.IsNullOrEmpty(LobbyController._singleton.GetName()))
        {
            Debug.Log("Haven't enter name");
            return;  }
        manager.Name = LobbyController._singleton.GetName();
        manager.networkAddress = address;
        manager.StartClient();
        StopBroadcast();
    }
}
