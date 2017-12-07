using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//multy
public class TankShoot : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        };
    }
    //multy
    [Command]
    void CmdFire()
    {
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;
        bullet.GetComponent<ShellScript>().Parent = gameObject;
        //multy
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 2.0f);
    }
}
