using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


//multy
public class TankBlood : MonoBehaviour
{
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
        else
        {
            BloodCube.transform.localScale = new Vector3(Total * 1.0f / 100f, 0.05f, 0.05f);
        }
    }
    void Dead()
    {
        Destroy(gameObject);
        Instantiate(Destroyed, transform.localPosition, transform.rotation);
    }
}
