using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public class ElevatorDoor
    {
        public ElevatorDoor(GameObject leftDoor, GameObject rightDoor)
        {
            this.leftDoor = leftDoor;
            this.rightDoor = rightDoor;
        }
        public GameObject leftDoor;
        public GameObject rightDoor;
        public bool open = false;
    }


    public float speed = 0.025f;

    private GameObject Elevator;
    private float originalElevatorPosition;

    public List<ElevatorDoor> ElevatorDoorsList;

    //The floor elevator is currently moving to.
    public int targetFloor;

    private bool moving = false;
    private bool carryingPlayer = false;


    private GameObject Player;
    private Collider ElevatorInside;

    private void Awake()
    {
        Elevator = transform.Find("Elevator").gameObject;
        originalElevatorPosition = Elevator.transform.localPosition.y;
        targetFloor = 0;

        ElevatorInside = Elevator.GetComponent<BoxCollider>();
        Player = GameObject.FindWithTag("Player");
        ElevatorDoorsList = new List<ElevatorDoor>();
        for (int i = 0; i < 10; i++)
        {
            Transform floor = transform.Find("Floor" + i);
            ElevatorDoor door = new ElevatorDoor(floor.GetChild(0).gameObject, floor.GetChild(1).gameObject);
            ElevatorDoorsList.Add(door);
        }
    }

    void Update()
    {
        if (moving)
        {
            MoveTowardsTargetFloor();
        }
    }

    private void MoveTowardsTargetFloor()
    {
        float currentPositionGlobal = Elevator.transform.position.y;

        float targetPosition = originalElevatorPosition + targetFloor * 0.03f;
        float currentPosition = Elevator.transform.localPosition.y;

        //If at target position return.
        if (Mathf.Abs(currentPosition - targetPosition) < 0.000001)
        {
            OpenDoorsOnFloor(targetFloor);
            moving = false;
            Player.GetComponent<CharacterController>().enabled = true;
            carryingPlayer = false;
        }
            

        bool movingUp = targetPosition - currentPosition > 0;
        float movement = (movingUp ? speed : -speed) * Time.deltaTime;
        float newPosition = currentPosition + movement;

        if (movingUp && (newPosition > targetPosition))
            newPosition = targetPosition;
        else if (!movingUp && (newPosition < targetPosition))
            newPosition = targetPosition;

        

        Elevator.transform.localPosition = new Vector3(Elevator.transform.localPosition.x, newPosition, Elevator.transform.localPosition.z);

        if (carryingPlayer)
        {
            float newPositionGlobal = Elevator.transform.position.y;

            float movementGlobal = newPositionGlobal - currentPositionGlobal;
            Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + movementGlobal, Player.transform.position.z);

        }

    }

    public void CallElevatorToFloor(int floor)
    {
        CloseDoorsOnAllFloors();
        targetFloor = floor;

        if (ElevatorInside.bounds.Contains(Player.transform.position))
        {
            Player.GetComponent<CharacterController>().enabled = false;
            carryingPlayer = true;
        }

        moving = true;
    }

    private void CloseDoorsOnAllFloors()
    {
        foreach (ElevatorDoor door in ElevatorDoorsList)
        {
            if (door.open)
            {
                door.leftDoor.transform.localPosition =
                    new Vector3(door.leftDoor.transform.localPosition.x / 4, door.leftDoor.transform.localPosition.y, door.leftDoor.transform.localPosition.z);
                door.rightDoor.transform.localPosition =
                    new Vector3(door.rightDoor.transform.localPosition.x / 4, door.rightDoor.transform.localPosition.y, door.rightDoor.transform.localPosition.z);
                door.open = false;
            }
        }

    }


    private void OpenDoorsOnFloor(int floor)
    {
        ElevatorDoor door = ElevatorDoorsList[floor];
        if (!door.open)
        {
            door.leftDoor.transform.localPosition =
                new Vector3(door.leftDoor.transform.localPosition.x * 4, door.leftDoor.transform.localPosition.y, door.leftDoor.transform.localPosition.z);
            door.rightDoor.transform.localPosition =
                new Vector3(door.rightDoor.transform.localPosition.x * 4, door.rightDoor.transform.localPosition.y, door.rightDoor.transform.localPosition.z);
            door.open = true;
        }
    }
}
