using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class GameRoom : MonoBehaviour
{
    private GameType gameType;
    private string LANAdress;
    private MatchInfoSnapshot netInfor;
    public void Init(string name, string ip, MatchInfoSnapshot infor, GameType type)
    {
        gameType = type;
        LANAdress = ip;
        netInfor = infor;
        gameObject.GetComponentInChildren<Text>().text = name + (string.IsNullOrEmpty(ip) ? "" : "(" + ip + ")");
    }
    public void OnClick()
    {
        if (gameType == GameType.LAN)
            if (!string.IsNullOrEmpty(LANAdress))
                T_BroadCastController.Instance.JoinRoom(LANAdress);
        if (gameType == GameType.Online)
            if (netInfor != null)
                T_Host_Join.Instance.JoinRoom(netInfor);
    }

}
public enum GameType
{
    Online,
    LAN,
    Offline
}
