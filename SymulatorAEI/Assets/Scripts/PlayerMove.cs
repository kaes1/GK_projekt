using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //Moving
    [SerializeField] private float movementSpeed;
    private float slopeForce = 5f;
    private float slopeForceRayLength = 1.5f;
    
    //Jumping
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private AudioSource stepSound;
    private bool isJumping;
    

    private CharacterController charController;


    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (charController.enabled == true)
            PlayerMovement();
    }

    private void PlayerMovement()
    {
        //Regular movement.
        float vertInput = Input.GetAxis("Vertical"); //No need to multiply * Time.deltaTime because SimpleMove does that automatically.
        float horizInput = Input.GetAxis("Horizontal"); //No need to multiply * Time.deltaTime because SimpleMove does that automatically.
        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;
        
        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        //Play walking sound
        if((forwardMovement.sqrMagnitude != 0 || rightMovement.sqrMagnitude != 0) && !isJumping)
        {
            if (!stepSound.isPlaying)
                stepSound.Play();
        }

        //When walking down slopes apply force downwards.
        if ((vertInput != 0 || horizInput != 0) && OnSlope())
            charController.Move(Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime);

        //Jumping.
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private bool OnSlope()
    {
        if (isJumping)
            return false;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, charController.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
   
        return false;
    }


    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        do
        {
            charController.Move(Vector3.up * jumpFallOff.Evaluate(timeInAir) * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }
}
