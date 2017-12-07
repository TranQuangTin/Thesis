using System;
using System.Collections.Generic;
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
[Serializable]
public enum GameMode
{
    Time, EndLife
}
public delegate void ColorChange(Color color);
public delegate void OnClientConnected(NetworkMessage msg);

[Serializable]
public class LevelSetting
{
    public List<LevelInfor> ListLevel;
    //public LevelSetting()
    //{
    //    ListLevel = new List<LevelInfor>();
    //    ListLevel.Add(new LevelInfor());
    //}
}
[Serializable]
public class LevelInfor
{
    public int Number = 1;
    public string SceneName = "aaa";
    public int Time = 30;
    public GameMode Mode = GameMode.Time;
    public float EnemyDistance = 50;
    public float TimeBetween2Shoot = 1;
    public float TimeBetween2EnemyShoot = 2;
    public float PlayerDame = 20;
    public float EnemyDame = 10;
    public float PlayerBlood = 100;
    public float EnemyBlood = 100;
    public int TotalEnemy = 20;
    public int TotalEnemyAtTime = 5;
    public int TimeSpawnPharmacy = 10;
    public int Speed = 4;
    public int RotateSpeed = 70;
    public int EnemySpeed = 4;
    public int EnemyRotateSpeed = 70;
    public string Describe = "";
}