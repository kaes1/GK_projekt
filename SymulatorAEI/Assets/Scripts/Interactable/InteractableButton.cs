using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : MonoBehaviour, Interactable
{
    public UnityEngine.Events.UnityEvent OnInteract;

    public void Interact()
    {
        Debug.Log("Interact() in button called!");
        OnInteract.Invoke();
    }

    public string GetInteractPromptText()
    {
        return "Press E to Interact";
    }
}
