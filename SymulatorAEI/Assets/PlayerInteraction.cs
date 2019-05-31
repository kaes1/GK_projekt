using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public Camera camera;
    GameObject act_obj;
    GameObject prev_obj;

    public Canvas player_ui;
    private CanvasGroup canvasGroup;
    public Text itemName;
    
    bool interacted;
    // Start is called before the first frame update
    void Start()
    {
        //camera = GetComponent<Camera>();
        prev_obj = null;
        act_obj = null;
        interacted = false;
        canvasGroup = player_ui.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("DRAWING");
        //Debug.DrawRay(camera.transform.position, camera.transform.forward *100, Color.blue);
        act_obj = MyOwnRaycasting.CastRay(camera.transform.position, camera.transform.forward, 5.0f);
        if(act_obj)
        {
            itemName.text = act_obj.name;
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.alpha = 0;
        }
        Debug.Log(act_obj.name);
        if (act_obj && Input.GetKeyDown(KeyCode.E))
        {
            if (!interacted)
            {
                act_obj.GetComponent<IInteractable>().StartInteraction();
                interacted = true;
            }
            else
            {
                act_obj.GetComponent<IInteractable>().EndInteraction();
                interacted = false;
            }
        }
    }
}
