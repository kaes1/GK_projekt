using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorShaft : MonoBehaviour
{

    public enum ElevatorState
    {
        Moving,
        DoorsOpening,
        DoorsClosing,
        DoorsOpen,
        DoorsClosed,
    }

    //Current state of the elevator.
    private ElevatorState CurrentElevatorState = ElevatorState.DoorsClosed;


    //GameObject of the elevator, needed for moving.
    private GameObject Elevator;
    //GameObject of the player, for checking if player is inside.
    private GameObject Player;

    //Elevator AudioPlayer
    private AudioSource elevatorArrivedAudioSource;
    private AudioSource elevatorMovesAudioSource;

    //The floor elevator is currently moving to.
    private int targetFloor = 0;
    //The floor elevator will move to when finished with current movement.
    private int nextTargetFloor = -1;
    //The floor elevator is currently as. Float instead of integer to show position between floors.
    private float currentFloor = 0;
    //Text UI where current floor will be displayed.
    private TMPro.TextMeshProUGUI[] currentFloorDisplayTexts = new TMPro.TextMeshProUGUI[10];
    //Time left before doors close.
    private float doorsOpenTimeLeft = 0.0f;


    private float originalElevatorPosition;
    private float distanceBetweenFloors;
    private bool carryingPlayer = false;
    //Speed of movement of the elevator.
    public float speed = 1.6f;

    private void Awake()
    {
        CurrentElevatorState = ElevatorState.DoorsClosed;

        Elevator = transform.Find("Elevator").gameObject;
        Player = GameObject.FindWithTag("Player");

        originalElevatorPosition = Elevator.transform.localPosition.y;
        distanceBetweenFloors = transform.Find("Floor2").localPosition.y - transform.Find("Floor1").localPosition.y;

        for (int i = 0; i < 10; i++)
            currentFloorDisplayTexts[i] = transform.Find("Floor" + i).Find("ElevatorFloorDisplay").Find("Canvas").Find("TextMeshPro Text").GetComponent<TMPro.TextMeshProUGUI>();

        var audioSources = Elevator.GetComponents<AudioSource>();
        elevatorArrivedAudioSource = audioSources[0];
        elevatorMovesAudioSource = audioSources[1];
    }

    void Update()
    {
        //Pause audio when time is frozen and don't do anything.
        if (Time.deltaTime == 0)
        {
            elevatorMovesAudioSource.Pause();
            elevatorArrivedAudioSource.Pause();
            return;
        }
        else
        {
            elevatorMovesAudioSource.UnPause();
            elevatorArrivedAudioSource.UnPause();
        }

        //Calculate current floor.
        currentFloor = (Elevator.transform.localPosition.y - originalElevatorPosition) / distanceBetweenFloors;
        //Display current floor on all floor displays.
        for (int i = 0; i < 10; i++)
            currentFloorDisplayTexts[i].text = Mathf.RoundToInt(currentFloor).ToString();
        //Act based on current state.
        switch (CurrentElevatorState)
        {
            case ElevatorState.Moving:
                //Move towards target floor.
                MoveTowardsTargetFloor();
                //If arrived at target floor, open the doors.
                if (Mathf.Abs(currentFloor - targetFloor) < 0.0001)
                {
                    CurrentElevatorState = ElevatorState.DoorsOpening;
                    Player.GetComponent<CharacterController>().enabled = true;
                    carryingPlayer = false;
                    elevatorMovesAudioSource.Stop();
                    elevatorArrivedAudioSource.Play();
                }
                else if(!elevatorMovesAudioSource.isPlaying)
                {
                    elevatorMovesAudioSource.Play();
                }
                break;

            case ElevatorState.DoorsOpening:
                //Play doors opening animation.
                Animator openDoorsAnimator = transform.Find("Floor" + Mathf.RoundToInt(currentFloor)).GetComponent<Animator>();
                openDoorsAnimator.SetBool("open", true);
                //If animation is finished, go to next state.
                if (openDoorsAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Open"))
                {
                    CurrentElevatorState = ElevatorState.DoorsOpen;
                    doorsOpenTimeLeft = 2.5f; 
                }
                break;

            case ElevatorState.DoorsClosing:
                //Play doors closing animation.
                Animator closeDoorsAnimator = transform.Find("Floor" + Mathf.RoundToInt(currentFloor)).GetComponent<Animator>();
                closeDoorsAnimator.SetBool("open", false);
                //If animation is finished, go to next state.
                if (closeDoorsAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Closed"))
                    CurrentElevatorState = ElevatorState.DoorsClosed;
                break;

            case ElevatorState.DoorsOpen:
                //If doors open time is still > 0 then stay open, otherwise close the doors.
                if (doorsOpenTimeLeft > 0)
                    doorsOpenTimeLeft -= Time.deltaTime;
                else
                {
                    CurrentElevatorState = ElevatorState.DoorsClosing;
                }
                break;

            case ElevatorState.DoorsClosed:
                //Doors are closed, elevator is ready to proceed to another floor.
                if (nextTargetFloor == -1)
                    break;
                targetFloor = nextTargetFloor;
                nextTargetFloor = -1;
                if (targetFloor == Mathf.RoundToInt(currentFloor))
                {
                    CurrentElevatorState = ElevatorState.DoorsOpening;
                    elevatorArrivedAudioSource.Play();
                }
                else
                {
                    CurrentElevatorState = ElevatorState.Moving;
                    if (Elevator.GetComponent<BoxCollider>().bounds.Contains(Player.transform.position))
                    {
                        Player.GetComponent<CharacterController>().enabled = false;
                        carryingPlayer = true;
                    }
                }
                break;
        }
    }

    private void MoveTowardsTargetFloor()
    {
        float currentPositionGlobal = Elevator.transform.position.y;

        float targetPosition = originalElevatorPosition + targetFloor * distanceBetweenFloors;
        float currentPosition = Elevator.transform.localPosition.y;

        bool movingUp = targetPosition - currentPosition > 0;
        float newPosition = currentPosition + (movingUp ? speed : -speed) * Time.deltaTime;
        
        //Clamp position to not overshoot.
        if (movingUp && (newPosition > targetPosition))
            newPosition = targetPosition;
        else if (!movingUp && (newPosition < targetPosition))
            newPosition = targetPosition;

        //Change elevator position.
        Elevator.transform.localPosition = new Vector3(Elevator.transform.localPosition.x, newPosition, Elevator.transform.localPosition.z);

        //Move player if neccesary.
        if (carryingPlayer)
        {
            float newPositionGlobal = Elevator.transform.position.y;
            float movementGlobal = newPositionGlobal - currentPositionGlobal;
            Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + movementGlobal, Player.transform.position.z);
        }
    }

    public void CallElevatorToFloor(int floor)
    {
        nextTargetFloor = floor;
    }
}
