using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsBehaviour : MonoBehaviour, IInteractable
{
    bool opened = false;
    bool startOpening = false;
    bool startClosing = false;
    bool stateChanged = false;
    float act_rotation = 0;

    public void EndInteraction()
    {
        //throw new System.NotImplementedException();
    }

    public void StartInteraction()
    {
       if(opened)
        {
            startClosing = true;
            startOpening = false;
            opened = false;
        }
       else
        {
            startClosing = false;
            startOpening = true;
            opened = true;
        }
        stateChanged = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stateChanged)
        {
            if (startClosing)
            {
                CloseDoors();
            }
            if (startOpening)
            {
                OpenDoors();
            }
        }
    }

    void CloseDoors()
    {
        if (act_rotation != 0)
        {
            act_rotation -= 1;
            transform.Rotate(0, 0, -1);
        }
        else
        {
            stateChanged = false;
            startClosing = false;
        }
    }

    void OpenDoors()
    {
        if (act_rotation != 90)
        {
            act_rotation += 1;
            transform.Rotate(0, 0, 1);
        }
        else
        {
            stateChanged = false;
            startOpening = false;
        }
    }
}
