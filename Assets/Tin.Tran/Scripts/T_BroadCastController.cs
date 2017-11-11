using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class T_BroadCastController : NetworkDiscovery
{
    public Dictionary<string, string> ListGames;
    private Dictionary<string, Coroutine> ListIPs;
    private NetworkManager manager;
    public static T_BroadCastController Instance
    {
        get;
        protected set;
    }
    private void Start()
    {
        manager = NetworkManager.singleton;
        ListIPs = new Dictionary<string, Coroutine>();
        ListGames = new Dictionary<string, string>();
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
    {
        broadcastData = roomName;
        StartAsServer();
        manager.StartHost();
    }
    public void JoinRoom(string address)
    {
        manager.networkAddress = address;
        manager.StartClient();
    }
}
