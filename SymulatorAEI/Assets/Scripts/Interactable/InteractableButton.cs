using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : MonoBehaviour, Interactable
{
    public UnityEngine.Events.UnityEvent OnInteract;
    public string interactPromtText = "Press E to Interact";

    public void Interact()
    {
        OnInteract.Invoke();
    }

    public string GetInteractPromptText()
    {
        return interactPromtText;
    }
}
