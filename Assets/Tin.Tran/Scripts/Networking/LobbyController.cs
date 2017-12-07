using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Alta.Plugin;
using UnityEngine.Networking.Match;
using System;

public class LobbyController : MonoBehaviour
{
    [SerializeField]
    public GameObject Tank;
    [SerializeField]
    public ColorPickerTriangle Colorpicker;
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
    [SerializeField]
    private InputField Name;

    public Text TxtLevel;


    private T_BroadCastController broadcast;
    private T_Host_Join hostControll;
    private GameValues values;


    public static LobbyController _singleton
    {
        get;
        protected set;
    }
    private void Awake()
    {
        _singleton = this;
    }
    private void Start()
    {
        TxtLevel.text = "Your current level: " + PlayerPrefs.GetInt("Level");
        values = T_NetworkManager._singleton.gameObject.GetComponent<GameValues>();
        broadcast = T_BroadCastController._singleton;
        hostControll = T_Host_Join._singleton;
        Colorpicker.OnColorChange += OnChangeColor;
    }
    public void PlayOffline()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Preload1.Global.GetCurrentLevel().SceneName);
    }
    private void OnChangeColor(Color color)
    {
        values.TankColor = color;
        foreach (Material mt in Tank.GetComponent<Renderer>().materials)
        {
            if (mt.name.IndexOf("TankColour") == 0)
            {
                mt.color = color;
                break;
            }
        }
        foreach (Renderer rd in Tank.GetComponentsInChildren<Renderer>())
        {
            foreach (Material mt in rd.materials)
            {
                if (mt.name.IndexOf("TankColour") == 0)
                {
                    mt.color = color;
                    break;
                }
            }
        }
    }
    private void OnDestroy()
    {
        Colorpicker.OnColorChange -= OnChangeColor;
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
    public string GetName()
    {
        // Debug.Log(Environment.MachineName);
        return Environment.MachineName;
    }
}
