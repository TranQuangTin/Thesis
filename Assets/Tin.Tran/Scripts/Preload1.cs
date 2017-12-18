using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alta.Plugin;

public class Preload1 : MonoBehaviour
{
    public static Preload1 Global;
    public Dictionary<int, LevelInfor> ListLevel;
    private void Awake()
    {
        Global = this;
        DontDestroyOnLoad(this);
        ListLevel = new Dictionary<int, LevelInfor>();
        LevelSetting tmp = XmlExtention.Read<LevelSetting>(Application.streamingAssetsPath + "/LevelController.xml");
        foreach (LevelInfor ll in tmp.ListLevel)
        {
            ListLevel.Add(ll.Number, ll);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby"); //PlayerPrefs.SetInt("Level", 2);
    }
    public LevelInfor GetCurrentLevel()
    {
        int s = PlayerPrefs.GetInt("Level");
        if (s == 0)
        {
            s = 1;
            PlayerPrefs.SetInt("Level", 1);
        }
        if (ListLevel.ContainsKey(s))
            return ListLevel[s];
        return null;
    }
}
