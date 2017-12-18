using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TankBlood : NetworkBehaviour
{
    [SyncVar(hook = "OnChangeHealth")]
    public int Total;
    [SyncVar(hook = "OnChangeScore")]
    public int Score = 0;
    public GameObject BloodCube;
    public GameObject Destroyed;
    public Text TxtScore;
    private void Start()
    {
        Total = 100;
        TxtScore = GameObject.Find("TQTText").GetComponent<Text>();
    }
    public void OnChangeScore(int score)
    {
        if (isLocalPlayer)
        {
            Score++;
            TxtScore.text = Score.ToString();
        }
    }
    public void TakeDamage(GameObject parent)
    {
        Total -= 20;
        if (Total <= 0)
        {
            //BloodCube.SetActive(false);
            Dead();
            parent.GetComponent<TankBlood>().Score++;
        }
    }
    void OnChangeHealth(int helth)
    {
        Total = helth;
        BloodCube.transform.localScale = new Vector3(Total * 1.0f / 100f, 0.05f, 0.05f);
    }
    void Dead()
    {
        if (isLocalPlayer)
        {
            //gameObject.GetComponent<TankMove>().ReturnCamera();
            Total = 100;
            if (isServer)
                RpcRespawn();
            else Cmdabc();
            //TankController.Global.Respawn(gameObject);
        }
        gameObject.GetComponent<Rigidbody>().MovePosition(Vector3.zero);
        GameObject go = Instantiate(Destroyed, transform.localPosition, transform.rotation);
    }
    [Command]
    void Cmdabc()
    {
        RpcRespawn(); Total = 100;
    }
    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            Total = 100;
            Vector3 spawnPoint = Vector3.zero;
            if (TankController.Global.spawnPoints != null && TankController.Global.spawnPoints.Length > 0)
            {
                spawnPoint = TankController.Global.spawnPoints[Random.Range(0, TankController.Global.spawnPoints.Length)].transform.position;
            }
            gameObject.GetComponent<Rigidbody>().MovePosition(spawnPoint);
            gameObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(Vector3.zero));
            GameObject go = Instantiate(Destroyed, transform.localPosition, transform.rotation);
        }
    }
}
