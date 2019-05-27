using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float velocityY;

    private CharacterController charController;
    [SerializeField] private bool jumped;
 
    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        jumped = false;
        velocityY = 0;
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;
        float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        //Movement vector in total
        Vector3 movement = forwardMovement + rightMovement;
        movement.y += velocityY;    //Movement in Y axis
        //if jumped add force to Y axis
        if (Input.GetKeyDown("space") && jumped == false)
        {
            velocityY += 5;
            Debug.Log("JUMP");
            jumped = true;
        }
        //after jump add gravity to Y movement
        if (jumped == true)
        {
            velocityY += Physics.gravity.y * Time.fixedDeltaTime;
        }

        charController.Move(movement * Time.deltaTime);
    }

    //reset jump on collision with ground
    //doesnt work on stairs
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Ground")
        {
            jumped = false;
            velocityY = 0;
        }
    }
}
