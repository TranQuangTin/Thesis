using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankController : NetworkBehaviour
{
    #region old
    //private GameValues values;

    //[SyncVar(hook = "OnMyName")]
    //private string m_PlayerName = "";
    //[SyncVar(hook = "OnMyColor")]
    //public Color m_PlayerColor = Color.clear;
    //private void OnMyColor(Color newColor)
    //{
    //   // if (!isServer) return;
    //    m_PlayerColor = newColor;
    //    foreach (Material mt in gameObject.GetComponent<Renderer>().materials)
    //    {
    //        if (mt.name.IndexOf("TankColour") == 0)
    //        {
    //            mt.color = m_PlayerColor;
    //            break;
    //        }
    //    }
    //    foreach (Renderer rd in gameObject.GetComponentsInChildren<Renderer>())
    //    {
    //        foreach (Material mt in rd.materials)
    //        {
    //            if (mt.name.IndexOf("TankColour") == 0)
    //            {
    //                mt.color = m_PlayerColor;
    //                break;
    //            }
    //        }
    //    }
    //}
    //private void OnMyName(string msg)
    //{
    //    m_PlayerName = msg;
    //}
    //private void Start()
    //{
    //    values = T_NetworkManager._singleton.gameObject.GetComponent<GameValues>();
    //    CmdFire(values.TankColor);
    //}
    //[Command]
    //void CmdFire(Color cl)
    //{
    //    m_PlayerColor = cl;
    //    foreach (Material mt in gameObject.GetComponent<Renderer>().materials)
    //    {
    //        if (mt.name.IndexOf("TankColour") == 0)
    //        {
    //            mt.color = m_PlayerColor;
    //            break;
    //        }
    //    }
    //    foreach (Renderer rd in gameObject.GetComponentsInChildren<Renderer>())
    //    {
    //        foreach (Material mt in rd.materials)
    //        {
    //            if (mt.name.IndexOf("TankColour") == 0)
    //            {
    //                mt.color = m_PlayerColor;
    //                break;
    //            }
    //        }
    //    }
    //}
    #endregion

    public static TankController Global;
    public GameObject cd;
    private GameObject Tank;
    public NetworkStartPosition[] spawnPoints;
    private void Awake()
    {
        Global = this;
    }
    private void Start()
    {
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
    }
    public void Respawn(GameObject Tank)
    {
        Debug.Log("Vo day");
        this.Tank = Tank;
        cd.SetActive(true);
        StartCoroutine(abc());
    }
    IEnumerator abc()
    {
        yield return new WaitForSeconds(0.1f);
        cd.GetComponent<CountDown>().Count(5, Onlife);
    }
    public void Onlife()
    {
        Vector3 spawnPoint = Vector3.zero;
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        }
        Tank.transform.position = spawnPoint;
        Tank.SetActive(true);
    }
    public IEnumerator ABC(GameObject go)
    {
        yield return new WaitForSeconds(5);
        go.SetActive(true);
    }
}
