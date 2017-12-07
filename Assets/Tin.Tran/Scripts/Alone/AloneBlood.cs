using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AloneBlood : MonoBehaviour
{
    public float Total;
    public GameObject BloodCube;
    public GameObject Destroyed;
    private Transform[] spawnPoints;
    private void Start()
    {
        Total = 100;
    }
    public void TakeDamage(float number)
    {
        Total -= number;
        if (Total <= 0)
        {
            BloodCube.SetActive(false);
            Dead();
            return;
        }
        BloodCube.transform.localScale = new Vector3(Total * 1.0f / 100f, 0.05f, 0.05f);
    }
    void Dead()
    {
        GameObject go = Instantiate(Destroyed, transform.localPosition, transform.rotation);
        if (gameObject.tag == "Tank")
        {
            gameObject.GetComponent<AloneMove>().ReturnCamera();
            WinLose.Global.ShowResult(false);
        }
        else
        {
            Map_Manager.Global.OnEnemyDestroyed(this);
        }
        Destroy(gameObject);
    }
    IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(true);
    }

    void RpcRespawn()
    {
        Vector3 spawnPoint = Vector3.zero;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        }
        Total = 100;
        transform.position = spawnPoint;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Medical")
        {
            collision.gameObject.SetActive(false);
            Total = 100;
            BloodCube.transform.localScale = new Vector3(Total * 1.0f / 100f, 0.05f, 0.05f);
        }
    }
}
