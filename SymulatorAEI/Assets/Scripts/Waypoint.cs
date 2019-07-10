using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> Neighbours;
    public string roomID;

    void Start()
    {
        foreach(Waypoint neighbour in Neighbours)
            if (neighbour != null && !neighbour.Neighbours.Contains(this))
                neighbour.Neighbours.Add(this);
    }

}
