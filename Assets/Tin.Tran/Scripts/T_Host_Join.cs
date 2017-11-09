﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class T_Host_Join : MonoBehaviour
{
    public List<MatchInfoSnapshot> ListInternetMatch;

    [SerializeField]
    private uint roomSize = 6;
    private string roomName;
    private NetworkManager networkManager;
    public static T_Host_Join Instance
    {
        get;
        protected set;
    }
    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }
    #region Host
    public void SetRoomName(string _name)
    {
        roomName = _name;
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Creating Room: \"" + roomName + "\" for " + roomSize + " players.");
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, OnCreateMatch);
        }
    }

    private void OnCreateMatch(bool success, string extendedInfo, MatchInfo responseData)
    {
        networkManager.OnMatchCreate(success, extendedInfo, responseData);
        if (!success)
            LobbyController.Instance.CreateOnlineRoomFail();
    }
    #endregion
    #region Join
    public void RefreshRoomList()
    {
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
        // status.text = "Loading...";
    }
    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        if (!success || matchList == null)
        {
            Debug.Log("Nothing found!");
            ListInternetMatch = null;
            return;
        }
        if (matchList.Count > 0)
        {
            Debug.Log("Found " + matchList.Count + " matchs on Internet!");
            ListInternetMatch = matchList;
        }
    }
    public void JoinRoom(MatchInfoSnapshot _match)
    {
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        StartCoroutine(WaitForJoin());
    }

    IEnumerator WaitForJoin()
    {
        yield return new WaitForSeconds(10);
        MatchInfo matchInfo = networkManager.matchInfo;
        if (matchInfo != null)
        {
            Debug.Log("Fail to connect!");
            networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
            networkManager.StopHost();
        }
    }
    #endregion
}