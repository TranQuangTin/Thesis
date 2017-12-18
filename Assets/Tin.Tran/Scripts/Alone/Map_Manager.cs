using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Map_Manager : MonoBehaviour
{
    public static Map_Manager Global;

    public LevelInfor level;


    public Transform Player;
    public GameObject SpawnPositionParent;
    public GameObject EnemyPrefab;
    public GameObject CoudownObj;
    public GameObject InforPanel;
    public Text Score;

    private List<Transform> ListStartSpawn;
    private List<GameObject> ListEnemy;
    private int CurrentSpawn = 0;
    private int score;
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
        {
            CoudownObj.SetActive(true);
        }
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
        //level.TotalEnemyAtTime: lấy số lượng địch tối đa
        for (int i = 0; i < level.TotalEnemyAtTime; i++)
        {
            Spawn();
        }
        InforPanel.SetActive(false);
    }
    public void Spawn()
    {
        if (ListEnemy.Count >= level.TotalEnemy) return;
        CurrentSpawn++;
        int s = Random.Range(0, ListStartSpawn.Count);
        // tạo đối tượng địch tại một vị trí ngẫu nhiên
        GameObject enemy = Instantiate(EnemyPrefab, ListStartSpawn[s].position, ListStartSpawn[s].rotation);
        ListEnemy.Add(enemy);
    }
    public void OnEnemyDestroyed(GameObject go)
    {
        score++;
        if (ListEnemy.Contains(go)) ListEnemy.Remove(go);
        Score.text = score.ToString() + "/" + level.TotalEnemy;
        // kiểm tra xem đã tạo hết số lượng địch trong một level chưa
        if (CurrentSpawn < level.TotalEnemy && level.Mode == GameMode.EndLife)
        {
            // nếu chứ hết thì tiếp tục tạo
            Spawn();
        }
        else if (level.Mode == GameMode.Time)
            Spawn();
        // nếu không còn đối thủ nào thì người chơi đã chiến thắng
        if (score == level.TotalEnemy)
        {
            Debug.Log("Win");
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            WinLose.Global.ShowResult(true);
        }
    }
}
