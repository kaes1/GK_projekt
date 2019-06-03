using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    
    [SerializeField]
    public bool TurnAndWaveBehaviour = true;
    [SerializeField]
    public double ReactToPlayerRange = 5.0;

    //The NPC's default Transform.
    private Quaternion DefaultRotation;
    //The player GameObject.
    private GameObject Player;



    //Speed factor used in rotation SLERP.
    private float SLERP_speed = 4.2f;
    //Max rotation speed, in degrees per second.
    private float maxRotationSpeed = 180.0f;
    //Min rotation speed, in degrees per second.
    private float minRotationSpeed = 22.2f;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        DefaultRotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        if (TurnAndWaveBehaviour && DistanceToPlayer() <= ReactToPlayerRange)
        {
            TurnToPlayer();
            WaveToPlayer();
        }
        else
        {
            TurnToDefaultPosition();
        }
            
    }

    double DistanceToPlayer()
    {
        return Vector3.Distance(Player.transform.position, this.transform.position);
    }


    void TurnToPlayer()
    {
        //Get target rotation from player's position.
        Vector3 lookPos = Player.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(lookPos);

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
    
    void TurnToDefaultPosition()
    {
        //Get target rotation from DefaultRotation.
        Quaternion targetRotation = DefaultRotation;
        
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

    void WaveToPlayer()
    {
        //NOTHING YET
    }


}
