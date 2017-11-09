using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TinNetwork : NetworkManager
{
    public static List<T_Controller> ListConnect;
    public event Action serverPlayersReadied;
    [SerializeField]
    protected T_Controller m_NetworkPlayerPrefab;
    public static bool IsServer
    {
        get
        {
            return NetworkServer.active;
        }
    }
    public static TinNetwork s_Instance
    {
        get;
        protected set;
    }
    protected virtual void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            s_Instance = this;

            ListConnect = new List<T_Controller>();
        }
    }
    protected virtual void OnDestroy()
    {
        if (s_Instance == this)
        {
            s_Instance = null;
        }
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Tin_OnClientConnect: " + conn.address);
        base.OnClientConnect(conn);
        ClientScene.Ready(conn);
        ClientScene.AddPlayer(0);

        //if (clientConnected != null)
        //{
        //    clientConnected(conn);
        //}
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("Tin_OnClientDisconnect");

        base.OnClientDisconnect(conn);

        //if (clientDisconnected != null)
        //{
        //    clientDisconnected(conn);
        //}
    }
    public void RegisterNetworkPlayer(T_Controller newPlayer)
    {
        Debug.Log("Player joined");

        //ListConnect.Add(newPlayer);
        //newPlayer.CmdClientReadyInGameScene();
        //newPlayer.becameReady += OnPlayerSetReady;
        //Debug.Log("Total connect: " + ListConnect.Count);
        //if (IsServer)
        //{
        //    Debug.Log("server here!");
        //    UpdatePlayerIDs();
        //}

        //newPlayer.OnEnterGameScene();
        // load scene cho nay
    }
    public virtual void OnPlayerSetReady(T_Controller player)
    {
        if (AllPlayersReady() && serverPlayersReadied != null)
        {
            serverPlayersReadied();
        }
    }
    public bool AllPlayersReady()
    {
        for (int i = 0; i < ListConnect.Count; ++i)
        {
            if (!ListConnect[i].m_Ready)
            {
                return false;
            }
        }
        return true;
    }
    protected virtual void UpdatePlayerIDs()
    {
        for (int i = 0; i < ListConnect.Count; ++i)
        {
            ListConnect[i].SetPlayerId(i);
        }
    }
    public void DeregisterNetworkPlayer(T_Controller removedPlayer)
    {
        Debug.Log("Player left");
        int index = ListConnect.IndexOf(removedPlayer);

        if (index >= 0)
        {
            ListConnect.RemoveAt(index);
        }
        UpdatePlayerIDs();
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        // Intentionally not calling base here - we want to control the spawning of prefabs
        Debug.Log("OnServerAddPlayer");
        //quangtin
        T_Controller newPlayer = Instantiate<T_Controller>(m_NetworkPlayerPrefab);
        //DontDestroyOnLoad(newPlayer);
        NetworkServer.AddPlayerForConnection(conn, newPlayer.gameObject, playerControllerId);
    }
    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {
        Debug.Log("OnServerRemovePlayer");
        base.OnServerRemovePlayer(conn, player);

        T_Controller connectedPlayer = GetPlayerForConnection(conn);
        if (connectedPlayer != null)
        {
            Destroy(connectedPlayer);
            ListConnect.Remove(connectedPlayer);
        }
    }
    public static T_Controller GetPlayerForConnection(NetworkConnection conn)
    {
        return conn.playerControllers[0].gameObject.GetComponent<T_Controller>();
    }
    public override void OnStopServer()
    {
        base.OnStopServer();
        Debug.Log("OnStopServer");

        for (int i = 0; i < ListConnect.Count; ++i)
        {
            T_Controller player = ListConnect[i];
            if (player != null)
            {
                NetworkServer.Destroy(player.gameObject);
            }
        }

        ListConnect.Clear();
    }
    public override void OnStopClient()
    {
        Debug.Log("OnStopClient");
        base.OnStopClient();

        for (int i = 0; i < ListConnect.Count; ++i)
        {
            T_Controller player = ListConnect[i];
            if (player != null)
            {
                Destroy(player.gameObject);
            }
        }
        ListConnect.Clear();
    }
    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        Debug.Log("OnClientError");

        base.OnClientError(conn, errorCode);
    }
    public override void OnStartHost()
    {
        base.OnStartHost();
        Debug.Log("Start host here!");
    }
}
