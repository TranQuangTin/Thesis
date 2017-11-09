using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class T_Controller : NetworkBehaviour
{
    public bool m_Ready;
    public int m_PlayerId;
    public TinNetwork m_TinNetwork;
    [SerializeField]
    protected GameObject m_TankPrefab;

    public event Action<T_Controller> becameReady;
    public static T_Controller s_LocalPlayer
    {
        get;
        private set;
    }
    // Use this for initialization
    void Start()
    {
        m_TinNetwork = TinNetwork.s_Instance;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (hasAuthority)
            {
                Debug.Log("hasAuthority");
                CmdClientReadyInGameScene();
            }
        }
    }
    [Client]
    public override void OnStartLocalPlayer()
    {
        //if (m_Settings == null)
        //{
        //    m_Settings = GameSettings.s_Instance;
        //}

        base.OnStartLocalPlayer();
        Debug.Log("Local Network Player start");
        // UpdatePlayerSelections();

        s_LocalPlayer = this;
    }
    [ClientRpc]
    public void RpcPrepareForLoad()
    {
        if (isLocalPlayer)
        {
            // Show loading screen

        }
    }
    [Server]
    public void ClearReady()
    {
        m_Ready = false;
    }
    [Server]
    public void SetPlayerId(int playerId)
    {
        this.m_PlayerId = playerId;
    }
    public override void OnNetworkDestroy()
    {
        if (m_TinNetwork != null)
        {
            m_TinNetwork.DeregisterNetworkPlayer(this);
        }
        base.OnNetworkDestroy();
    }
    [Client]
    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);

        //if (m_Settings == null)
        //{
        //    m_Settings = GameSettings.s_Instance;
        //}
        if (m_TinNetwork == null)
        {
            m_TinNetwork = TinNetwork.s_Instance;
        }

        base.OnStartClient();
        Debug.Log("Client Network Player start");

        m_TinNetwork.RegisterNetworkPlayer(this);
    }

    protected void AddClientToServer()
    {
        //quangtin
        Debug.Log("CmdClientReadyInScene");
        GameObject tankObject = Instantiate(m_TankPrefab);
        NetworkServer.SpawnWithClientAuthority(tankObject, connectionToClient);
        //tank = tankObject.GetComponent<TankManager>();
        //tank.SetPlayerId(playerId);
        //if (lateSetupOfClientPlayer)
        //{
        //    lateSetupOfClientPlayer = false;
        //    SpawnManager.InstanceSet -= AddClientToServer;
        //}
    }
    [Command]
    public void CmdClientReadyInGameScene()
    {
        //if (SpawnManager.s_InstanceExists)
        //{
        AddClientToServer();
        //}
        //else
        //{
        //    lateSetupOfClientPlayer = true;
        //    SpawnManager.InstanceSet += AddClientToServer;
        //}
    }
    [Command]
    public void CmdSetReady()
    {
        m_Ready = true;

        if (becameReady != null)
        {
            becameReady(this);
        }
    }
}
