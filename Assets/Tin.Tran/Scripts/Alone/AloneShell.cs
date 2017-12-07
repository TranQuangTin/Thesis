using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AloneShell : MonoBehaviour
{
    public GameObject Parent;
    public GameObject Trail;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Parent) return;
        if (collision.gameObject.tag == "Tank")
            collision.gameObject.GetComponent<AloneBlood>().TakeDamage(Map_Manager.Global.level.EnemyDame);
        if (collision.gameObject.tag == "Enemy")
            collision.gameObject.GetComponent<AloneBlood>().TakeDamage(Map_Manager.Global.level.PlayerDame);
        //Trail.SetActive(false);
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
        if (systems != null)
        {
            for (int i = 0; i < systems.Length; i++)
            {
                systems[i].Play();
            }
        }
        Destroy(gameObject, 0.3f);
    }
}
