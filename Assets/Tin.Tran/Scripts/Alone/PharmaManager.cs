using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Alta.Plugin;

public class PharmaManager : MonoBehaviour
{
    public float TimeSpawn;
    private List<GameObject> ListTrans;
    // Use this for initialization
    void Start()
    {
        ListTrans = new List<GameObject>();
        foreach (Transform go in gameObject.GetComponentsInChildren<Transform>())
        {
            if (go.gameObject.tag == "Medical")
            {
                go.gameObject.SetActive(false);
                ListTrans.Add(go.gameObject);
            }
        }
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(TimeSpawn);
        int s = Random.Range(0, ListTrans.Count - 1);
        ListTrans[s].SetActive(true);
        StartCoroutine(Spawn());
    }
}
