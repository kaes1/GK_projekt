using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlaque : MonoBehaviour, Interactable
{
    public RoomInformation roomInformation;
    private GameController GameController;
    public void Awake()
    {
        GameController = FindObjectOfType<GameController>();
    }

    public void Interact()
    {
        GameController.DisplayDetails(roomInformation.plaqueText, "");
    }

    public string GetInteractPromptText()
    {
        return "Press E to Read";
    }
}
