using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorsController : MonoBehaviour
{
    private List<ElevatorShaft> ElevatorShafts = new List<ElevatorShaft>();

    private void Awake()
    {
        ElevatorShafts.Add(transform.Find("ElevatorShaftLeft").gameObject.GetComponent<ElevatorShaft>());
        ElevatorShafts.Add(transform.Find("ElevatorShaftRight").gameObject.GetComponent<ElevatorShaft>());
    }

    void Update()
    {

    }

    public void CallElevatorToFloorGoingUp(int floor)
    {
        ElevatorShafts[0].CallElevatorToFloor(floor);
        //ElevatorShafts[1].CallElevatorToFloor(floor);
    }

    public void CallElevatorToFloorGoingDown(int floor)
    {
        ElevatorShafts[0].CallElevatorToFloor(floor);
        //ElevatorShafts[1].CallElevatorToFloor(floor);
    }

}
