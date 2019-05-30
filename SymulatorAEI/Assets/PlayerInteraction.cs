using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera camera;
    GameObject act_obj;
    GameObject prev_obj;
    bool interacted;
    // Start is called before the first frame update
    void Start()
    {
        //camera = GetComponent<Camera>();
        prev_obj = null;
        act_obj = null;
        interacted = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("DRAWING");
        Debug.DrawRay(camera.transform.position, camera.transform.forward *100, Color.blue);
        act_obj = MyOwnRaycasting.CastRay(camera.transform.position, camera.transform.forward, 5.0f);
        Debug.Log(act_obj.name);
        if (act_obj && Input.GetKeyDown(KeyCode.E))
        {
            if (!interacted)
            {
                act_obj.GetComponent<Interaction>().OpenDoor();
                interacted = true;
            }
            else
            {
                act_obj.GetComponent<Interaction>().CloseDoor();
                interacted = false;
            }
        }
    }
}
