using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycasting : MonoBehaviour
{
    //Distance of the ray used in raycasting.
    public float distanceToSee;
    //Player's camera.
    public Camera PlayerCamera;

    public GameObject InteractPromptPanel;
    public GameObject InteractDetailsPanel;

    //private DataController dataController;

    private RaycastHit raycastHitInfo;
    private GameObject lookedAtObject = null;
    private GameObject highlightedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        //dataController = FindObjectOfType<DataController>();
    }

    // Update is called once per frame
    void Update()
    {
        //---
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(MyOwnRaycasting.CastRay(transform.position, PlayerCamera.transform.forward, 5.0f).name);
        }

        //Draw the ray used for raycasting.
        Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.forward * distanceToSee, Color.magenta);
        //Use Raycasting to get object hit.
        GameObject objectHit = null;
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out raycastHitInfo, distanceToSee))
            objectHit = raycastHitInfo.collider.gameObject;

        //If something new is being looked at.
        if (objectHit != lookedAtObject)
        {
            lookedAtObject = objectHit;
            //Stop current highlighting.
            if (highlightedObject)
            {
                highlightedObject.GetComponent<Highlight>().HighlightEnd();
                //Hide interact prompt.
                InteractPromptPanel.SetActive(false);
                //Hide details panel.
                InteractDetailsPanel.SetActive(false);
                highlightedObject = null;
            }

            //Highlight new object if needed.
            if (objectHit != null && objectHit.tag == "InteractablePlaque")
            {
                objectHit.GetComponent<Highlight>().HighlightStart();
                //Display interact prompt.
                InteractPromptPanel.SetActive(true);
                highlightedObject = objectHit;
            }
        }
        
        //Check if player interacts with interactable objects.
        if (lookedAtObject != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Player interacts with Plaque.
                if (lookedAtObject.tag == "InteractablePlaque")
                {
                    //Display details if they weren't displayed.
                    if (InteractDetailsPanel.activeSelf == false)
                    {
                        InteractPromptPanel.SetActive(false);
                        InteractDetailsPanel.SetActive(true);
                        RoomInformation roomInfo = lookedAtObject.GetComponent<RoomInformation>();
                        if (roomInfo != null)
                            InteractDetailsPanel.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = roomInfo.plaqueText;
                        else
                            InteractDetailsPanel.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "No description found";
                    }
                    //Hide details if displayed already.
                    else
                    {
                        InteractDetailsPanel.SetActive(false);
                    }

                }
            }
        }
    }
}
