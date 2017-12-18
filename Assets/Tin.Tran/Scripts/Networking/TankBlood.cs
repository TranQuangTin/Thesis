using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class TankBlood : NetworkBehaviour
{
    [SyncVar(hook = "OnChangeHealth")]
    public int Total;
    public GameObject BloodCube;
    public GameObject Destroyed;
    private void Start()
    {
        Total = 100;
    }
    public void TakeDamage(int number)
    {
        Total -= number;
        if (Total <= 0)
        {
            //BloodCube.SetActive(false);
            Dead();
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
