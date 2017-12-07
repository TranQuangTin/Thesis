using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankController : NetworkBehaviour
{
    private GameValues values;

    [SyncVar(hook = "OnMyName")]
    private string m_PlayerName = "";
    [SyncVar(hook = "OnMyColor")]
    public Color m_PlayerColor = Color.clear;
    private void OnMyColor(Color newColor)
    {
       // if (!isServer) return;
        m_PlayerColor = newColor;
        foreach (Material mt in gameObject.GetComponent<Renderer>().materials)
        {
            if (mt.name.IndexOf("TankColour") == 0)
            {
                mt.color = m_PlayerColor;
                break;
            }
        }
        foreach (Renderer rd in gameObject.GetComponentsInChildren<Renderer>())
        {
            foreach (Material mt in rd.materials)
            {
                if (mt.name.IndexOf("TankColour") == 0)
                {
                    mt.color = m_PlayerColor;
                    break;
                }
            }
        }
    }
    private void OnMyName(string msg)
    {
        m_PlayerName = msg;
    }
    private void Start()
    {
        values = T_NetworkManager._singleton.gameObject.GetComponent<GameValues>();
        CmdFire(values.TankColor);
    }
    [Command]
    void CmdFire(Color cl)
    {
        m_PlayerColor = cl;
        foreach (Material mt in gameObject.GetComponent<Renderer>().materials)
        {
            if (mt.name.IndexOf("TankColour") == 0)
            {
                mt.color = m_PlayerColor;
                break;
            }
        }
        foreach (Renderer rd in gameObject.GetComponentsInChildren<Renderer>())
        {
            foreach (Material mt in rd.materials)
            {
                if (mt.name.IndexOf("TankColour") == 0)
                {
                    mt.color = m_PlayerColor;
                    break;
                }
            }
        }
    }
}
