using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMovement : MonoBehaviour
{

    //Actual, max and min floor
    public int floor = 0;
    public int min_floor = 0;
    public int max_floor = 5;

    //Y starting pos of elevator
    public float starting_level = 1.4f;

    //Up and Down movement vectors
    private Vector3 up;
    private Vector3 down;

    //Boolean indicating the movement direction and whether the elevator stopped or not.
    private bool up_direction = true;
    private bool moving = false;

    //List of objects in elevator
    private List<GameObject> inElevator;

    // Start is called before the first frame update
    void Start()
    {
        up = new Vector3(0, 1, 0);
        down = new Vector3(0, -1, 0);
        inElevator = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //Changing elevator floor with U and J key
        if(Input.GetKeyDown(KeyCode.U))
        {
            floor += 1;
            floor = floor > max_floor ? max_floor : floor;
            up_direction = true;
            moving = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            floor -= 1;
            floor = floor < min_floor ? min_floor : floor;
            up_direction = false;
            moving = true;
        }

        //Moving up and down elevator and all objects inside
        if (transform.position.y < floor * 10 + starting_level && up_direction)
        {
            transform.position += up * Time.deltaTime;
            foreach(GameObject obj in inElevator)
            {
                CharacterController ch = obj.GetComponent<CharacterController>();
                ch.Move(up * Time.deltaTime);
            }
        }
        else
        {
            if(transform.position.y > floor * 10 + starting_level && !up_direction)
            {
                transform.position += down * Time.deltaTime;
                foreach (GameObject obj in inElevator)
                {
                    CharacterController ch = obj.GetComponent<CharacterController>();
                    ch.Move(down * Time.deltaTime);
                }
            }
            else
            {
                moving = false;
            }
        }
    }

    //Adding and deleting object to/from inElevator list
    private void OnTriggerEnter(Collider other)
    {
        inElevator.Add(other.gameObject);
        Debug.Log("IN: " + inElevator.Count.ToString());
    }
    private void OnTriggerExit(Collider other)
    {
        inElevator.Remove(other.gameObject);
        Debug.Log("IN: " + inElevator.Count.ToString());
    }
}
