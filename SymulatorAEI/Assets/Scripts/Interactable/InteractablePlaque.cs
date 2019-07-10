using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlaque : MonoBehaviour, Interactable
{
    public string roomID;

    private GameController GameController;

    public void Awake()
    {
        GameController = FindObjectOfType<GameController>();
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
