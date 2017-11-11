using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Alta.Plugin;
using UnityEngine.Networking.Match;

public class LobbyController : MonoBehaviour
{

    [SerializeField]
    private InputField OnlineRoomName;
    [SerializeField]
    private InputField LANRoomName;
    [SerializeField]
    private GameRoom GameItemPrefabs;
    [SerializeField]
    private Transform ListOnlineGameListParent;
    [SerializeField]
    private Transform ListLANGameListParent;


    private T_BroadCastController broadcast;
    private T_Host_Join hostControll;


    public static LobbyController Instance
    {
        get;
        protected set;
    }

    private void Start()
    {
        broadcast = gameObject.GetComponent<T_BroadCastController>();
        hostControll = gameObject.GetComponent<T_Host_Join>();
    }
    public void CreateLANRoom()
    {
        if (string.IsNullOrEmpty(LANRoomName.text))
            return;
        broadcast.CreateRoom(LANRoomName.text);
    }
    public void CreateOnlineRoom()
    {
        if (string.IsNullOrEmpty(OnlineRoomName.text))
            return;
        broadcast.CreateRoom(OnlineRoomName.text);
    }
    public void CreateOnlineRoomFail()
    {
        // do something
    }
    public void RefreshOnlineRooms()
    {
        ListOnlineGameListParent.ClearChildren();
        if (hostControll.ListInternetMatch != null)
        {
            foreach (MatchInfoSnapshot match in hostControll.ListInternetMatch)
            {
                GameRoom room = ListOnlineGameListParent.CreateChild(GameItemPrefabs);
                room.Init(match.name, null, match, GameType.Online);
            }
            return;
        }
        GameRoom room1 = ListOnlineGameListParent.CreateChild(GameItemPrefabs);
        room1.Init("No existing rooms!", null, null, GameType.None);
    }
    public void RefreshLANRooms()
    {
        ListLANGameListParent.ClearChildren();
        if (broadcast.ListGames != null && broadcast.ListGames.Count > 0)
        {
            foreach (string key in broadcast.ListGames.Keys)
            {
                GameRoom room = ListLANGameListParent.CreateChild(GameItemPrefabs);
                room.Init(broadcast.ListGames[key], key, null, GameType.LAN);
            }
            return;
        }
        GameRoom room1 = ListLANGameListParent.CreateChild(GameItemPrefabs);
        room1.Init("No existing rooms!", null, null, GameType.None);
    }
}
