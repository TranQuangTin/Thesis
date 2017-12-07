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
    private NetworkStartPosition[] spawnPoints;
    private void Start()
    {
        Total = 100;
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
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
        Destroy(go, 5);
        StartCoroutine(WaitForRespawn());
        gameObject.SetActive(false);
        RpcRespawn();
    }
    IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(true);
    }
    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            Vector3 spawnPoint = Vector3.zero;

            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }
            Total = 100;
            transform.position = spawnPoint;
        }
    }
}
