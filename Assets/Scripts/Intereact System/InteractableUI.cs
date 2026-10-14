using UnityEngine;

public class InteractableUI : AnimatorHandler
{
    [SerializeField] private InteractableObject correspondingInteractableObject;

    private const string IsInInteractRangeAnimatorBoolName = "isInRange";
    private const string IsInteractableAnimatorBoolName = "isInteractable";

    private void OnEnable()
    {
        correspondingInteractableObject.onIsInteractableChanged += OnIsInteractable;
        correspondingInteractableObject.onIsInInteractRangeChanged += OnIsInInteractRange;
    }

    private void OnDisable()
    {
        correspondingInteractableObject.onIsInteractableChanged -= OnIsInteractable;
        correspondingInteractableObject.onIsInInteractRangeChanged -= OnIsInInteractRange;
    }

    private void OnIsInInteractRange(bool isInInteractRange)
    {
        SetAnimatorBool(IsInInteractRangeAnimatorBoolName, isInInteractRange);
    }

    private void OnIsInteractable(bool isInteractable)
    {
        SetAnimatorBool(IsInteractableAnimatorBoolName, isInteractable);
    }
}