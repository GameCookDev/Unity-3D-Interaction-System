using System;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    protected bool isInteractable;

    // a delegate void (event) that gets invoked every time this object is set to being interactable or not
    //for example when this object is interactable, a certain UI plays it's "is interactable" animation
    public delegate void OnIsInteractableChanged(bool isInteractable);
    public OnIsInteractableChanged onIsInteractableChanged;

    public delegate void OnIsInInteractRangeChanged(bool isInInteractRange);
    public OnIsInInteractRangeChanged onIsInInteractRangeChanged;

    public void SetIsInInteractRange(bool isInInteractRange)
    {
        onIsInInteractRangeChanged?.Invoke(isInInteractRange);
    }

    public virtual void SetIsInteractable(bool isInteractable)
    {
        this.isInteractable = isInteractable;

        onIsInteractableChanged?.Invoke(isInteractable);
    }

    public virtual void Interact()
    {
        //write your interaction logic here
        Debug.Log($"Interacting with {gameObject.name}");
    }
}
