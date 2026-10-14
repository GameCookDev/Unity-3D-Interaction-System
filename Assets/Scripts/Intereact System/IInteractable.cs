using UnityEngine;

public interface IInteractable
{
    public void Interact() { }

    public void SetIsInInteractRange(bool isInInteractRange) { }
    public void SetIsInteractable(bool isInteractable) { }
}
