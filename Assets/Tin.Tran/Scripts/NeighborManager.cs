using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborManager : MonoBehaviour
{
    public static NeighborManager Global;
    private Dictionary<string, int> Connections;


    private void Awake()
    {
        Global = this;
        Connections = new Dictionary<string, int>();
    }
    public void Add(string Name, int ConnectionID)
    {
        if (!Connections.ContainsKey(Name))
            Connections.Add(Name, ConnectionID);
    }
    public void Remove(string Name)
    {
        if (Connections.ContainsKey(name))
            Connections.Remove(name);
    }
    public bool CheckKey(string Name)
    {
        return Connections.ContainsKey(Name);
    }
}
