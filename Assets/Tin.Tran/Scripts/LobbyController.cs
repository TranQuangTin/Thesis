using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}
