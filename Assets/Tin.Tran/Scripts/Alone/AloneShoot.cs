using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AloneShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;


    private bool flag = true;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!flag) return;
            CmdFire();
        };
    }
    void CmdFire()
    {
        StartCoroutine(ResetFlag());
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;
        bullet.GetComponent<AloneShell>().Parent = gameObject;
        //multy
        //NetworkServer.Spawn(bullet);
        Destroy(bullet, 2.0f);
    }
    IEnumerator ResetFlag()
    {
        flag = false;
        yield return new WaitForSeconds(Map_Manager.Global.level.TimeBetween2Shoot);
        flag = true;
    }
}
