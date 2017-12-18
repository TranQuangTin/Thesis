using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alta.Plugin;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public Text Mode;
    public Text EnemyDistance;
    public Text TimeBetween2Shoot;
    public Text TimeBetween2EnemyShoot;
    public Text PlayerDame;
    public Text EnemyDame;
    public Text PlayerSpeed;
    public Text EnemySpeed;
    public Text TotalEnemy;
    public CountDown CountDown;
    LevelInfor lv;
    private void Start()
    {
        if (Preload1.Global == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }
        lv = Preload1.Global.GetCurrentLevel();
        if (lv != null)
        {
            if (lv.Mode == GameMode.Time)
            {
                Mode.text = "Best killer in time.";
                //CountDown.gameObject.SetActive(true);
            }
            else
            {
                Mode.text = "The last stand.";
                //CountDown.gameObject.SetActive(false);
            }
            EnemyDistance.text = lv.EnemyDistance.ToString() + "m";
            TimeBetween2Shoot.text = lv.TimeBetween2Shoot.ToString() + "s";
            TimeBetween2EnemyShoot.text = lv.TimeBetween2EnemyShoot.ToString() + "s";
            PlayerDame.text = lv.PlayerDame + "/" + lv.PlayerBlood;
            EnemyDame.text = lv.EnemyDame + "/" + lv.EnemyBlood;
            PlayerSpeed.text = lv.Speed + "/" + lv.RotateSpeed;
            EnemySpeed.text = lv.EnemySpeed + "/" + lv.EnemyRotateSpeed;
            TotalEnemy.text = lv.TotalEnemy.ToString();
        }
    }
    public void StartClick()
    {
        CountDown.Count(lv.Time, null);
        Map_Manager.Global.Play();
    }
}
