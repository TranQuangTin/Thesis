using UnityEngine;
using UnityEngine.Networking;

public class ChatMessage : MessageBase
{
    public string Name;
    public string Content;
}

public enum GameType
{
    Online,
    LAN,
    Offline,
    None
}
public enum MsType
{
    IN, OUT
}
public delegate void ColorChange(Color color);
public delegate void OnClientConnected(NetworkMessage msg);