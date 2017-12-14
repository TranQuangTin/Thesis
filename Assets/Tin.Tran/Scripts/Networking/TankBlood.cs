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
            BloodCube.SetActive(false);
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
        GameObject go = Instantiate(Destroyed, transform.localPosition, transform.rotation);
        gameObject.GetComponent<TankMove>().ReturnCamera();
        gameObject.SetActive(false);
        if (isLocalPlayer)
            TankController.Global.RpcRespawn(gameObject);
    }
}
