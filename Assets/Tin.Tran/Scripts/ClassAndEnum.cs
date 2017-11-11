
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