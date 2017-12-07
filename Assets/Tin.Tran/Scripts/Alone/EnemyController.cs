using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Distance;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private Transform Target;
    private Rigidbody rigidbody;
    private bool flag = true;
    private float speed;
    private float rotatespeed = 3f;
    private void Start()
    {
        if (Preload1.Global == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }
        rigidbody = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(CheckDistance());
        rigidbody.velocity = transform.forward * 10;
        StartCoroutine(Fire());
        speed = Map_Manager.Global.level.Speed;
        rotatespeed = Map_Manager.Global.level.RotateSpeed;
    }
    private void Update()
    {
        if (Target == null) return;
        transform.LookAt(Target);
        Vector3 movement = transform.forward * speed * Time.deltaTime;
        Vector3 target = rigidbody.position + movement;
        rigidbody.MovePosition(target);
    }
    IEnumerator CheckDistance()
    {
        yield return new WaitForSeconds(2);
        float dis;
        try
        {
            dis = Vector3.Distance(Map_Manager.Global.Player.position, transform.position);
        }
        catch
        {
            yield break;
        }
        if (dis < Distance && dis > 10)
            Target = Map_Manager.Global.Player;
        else Target = null;
        StartCoroutine(CheckDistance());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!flag || collision.gameObject.tag != "Tank") return;
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(0f, 100, 0f));
        StartCoroutine(ChangeFlag());
    }
    IEnumerator ChangeFlag()
    {
        flag = false;
        yield return new WaitForSeconds(3);
        flag = true;
    }
    void CmdFire()
    {
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
    IEnumerator Fire()
    {
        yield return new WaitForSeconds(Map_Manager.Global.level.TimeBetween2EnemyShoot);
        CmdFire();
        StartCoroutine(Fire());
    }
}
