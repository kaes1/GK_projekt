using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public enum NPCBehaviour
    {
        Stand,
        StandAndWave,
        Wander
    }

    [SerializeField]
    public NPCBehaviour CurrentBehaviour = NPCBehaviour.StandAndWave;

    //WANDER
    private float distanceToSee = 1.0f;
    private Quaternion WalkingRotation;

    //STAND AND WAVE
    [SerializeField]
    public double ReactToPlayerRange = 5.0;

   

    //The NPC's default Transform.
    private Quaternion DefaultRotation;
    //The player GameObject.
    private GameObject Player;

    //Speed factor used in rotation SLERP.
    private float SLERP_speed = 4.5f;
    //Max rotation speed, in degrees per second.
    private float maxRotationSpeed = 180.0f;
    //Min rotation speed, in degrees per second.
    private float minRotationSpeed = 22.2f;



    private Animator Animator;


    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        DefaultRotation = this.transform.rotation;
        WalkingRotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentBehaviour)
        {
            case NPCBehaviour.Stand:
                Animator.SetBool("Waving", false);
                Animator.SetBool("Walking", false);
                break;
            case NPCBehaviour.StandAndWave:
                if (DistanceToPlayer() <= ReactToPlayerRange)
                {
                    TurnToPlayer();
                    Animator.SetBool("Waving", true);
                }
                else
                {
                    TurnTo(DefaultRotation);
                    Animator.SetBool("Waving", false);
                }
                break;
            case NPCBehaviour.Wander:
                
                //Turn towards walking direction.
                TurnTo(WalkingRotation);

                //Walk forward if no blockade in the way.
                GameObject objectHit = CustomRaycasting.Raycast(transform.position, transform.forward, 1.0f);
                if (objectHit == null || !objectHit.name.Contains("Blockade"))
                {
                    Animator.SetBool("Walking", true);
                    Vector3 movement = transform.forward * 1.2f * Time.deltaTime;
                    this.transform.position += movement;
                }

                //If walking in the chosen direction, check if walking into blockade.
                if (this.transform.rotation == WalkingRotation)
                {
                    objectHit = CustomRaycasting.Raycast(transform.position, transform.forward, distanceToSee);
                    if (objectHit != null && (objectHit.name.Contains("Blockade") || objectHit.transform.parent.name.Contains("Blockade")))
                    {
                        CurrentBehaviour = NPCBehaviour.Stand;
                        Invoke("PickNewDirectionAndWalk", 2.0f);
                    }
                }

                break;

        }

    }

    double DistanceToPlayer()
    {
        return Vector3.Distance(Player.transform.position, this.transform.position);
    }

    void TurnTo(Quaternion targetRotation)
    {
        if (Time.deltaTime > 0)
        {
            //Slerp between current rotation and target rotation.
            Quaternion slerpResult = Quaternion.Slerp(transform.rotation, targetRotation, SLERP_speed * Time.deltaTime);
            //See what the speed of slerp rotation is.
            float rotationSpeed = Quaternion.Angle(this.transform.rotation, slerpResult) / Time.deltaTime;
            //Clamp the rotation speed using max rotation and min rotation speed.
            if (rotationSpeed > maxRotationSpeed)
                rotationSpeed = maxRotationSpeed;
            else if (rotationSpeed < minRotationSpeed)
                rotationSpeed = minRotationSpeed;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void TurnToPlayer()
    {
        //Get target rotation from player's position.
        Vector3 lookPos = Player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(lookPos);
        TurnTo(targetRotation);
    }

    void PickNewDirectionAndWalk()
    {
        Quaternion newWalkingRotation;

        while (true)
        {
            newWalkingRotation = WalkingRotation * Quaternion.Euler(0, Random.Range(120.0f, 240.0f), 0);
            GameObject objectHit = CustomRaycasting.Raycast(transform.position, newWalkingRotation * Vector3.forward, distanceToSee + 1.0f);
            if (objectHit != null && objectHit.name.Contains("Blockade"))
                continue;
            else
                break;
        }

        WalkingRotation = newWalkingRotation;
        CurrentBehaviour = NPCBehaviour.Wander;
    }

}
