using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlaque : MonoBehaviour, Interactable
{
    public string roomID;

    private GameController GameController;
    private GameObject Player;
    private GameObject RaycastingPlane;

    public void Start()
    {
        foreach (Transform t in transform)
            if (t.tag == "RaycastingPlane")
                RaycastingPlane = t.gameObject;
        GameController = FindObjectOfType<GameController>();
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) > 5)
            RaycastingPlane.SetActive(false);
        else
            RaycastingPlane.SetActive(true);
    }

    public void Interact()
    {
        GameController.DisplayDetailsForRoom(roomID);
    }

    public string GetInteractPromptText()
    {
        return "Press E to Read";
    }
}
