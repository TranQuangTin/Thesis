using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    public static Map_Manager Global;

    public LevelInfor level;


    public Transform Player;
    public GameObject SpawnPositionParent;
    public GameObject EnemyPrefab;
    public GameObject CoudownObj;
    public GameObject InforPanel;

    private List<Transform> ListStartSpawn;
    private List<GameObject> ListEnemy;
    private int CurrentSpawn = 0;
    private void Awake()
    {
        Global = this;
    }
    // Use this for initialization
    void Start()
    {
        if (Preload1.Global == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }
        InforPanel.SetActive(true);
        level = Preload1.Global.GetCurrentLevel();
        if (level.Mode == GameMode.Time)
            CoudownObj.SetActive(true);
        else
        {
            Debug.Log("vao day");
            CoudownObj.SetActive(false);
        }
        ListStartSpawn = SpawnPositionParent.GetComponentsInChildren<Transform>().ToList();
        ListEnemy = new List<GameObject>();
    }
    public void Play()
    {
        for (int i = 0; i < level.TotalEnemyAtTime; i++)
        {
            Spawn();
        }
        InforPanel.SetActive(false);
    }

    public void OnEnemyDestroyed(AloneBlood go)
    {
        if (CurrentSpawn < level.TotalEnemy)
        {
            Spawn();
        }
        if (ListEnemy.Count == 0)
        {
            // win
            Debug.Log("Thang");
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            WinLose.Global.ShowResult(true);
        }
    }
    public void Spawn()
    {
        CurrentSpawn++;
        int s = Random.Range(0, ListStartSpawn.Count);
        GameObject enemy = Instantiate(EnemyPrefab, ListStartSpawn[s].position, ListStartSpawn[s].rotation);
        ListEnemy.Add(enemy);
    }
}
