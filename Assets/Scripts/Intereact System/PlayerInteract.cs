using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Tooltip("how often do you want the player to check for interactables nearby")]
    [SerializeField] private float checkForInteractablesUpdateRate;
    private Coroutine checkForInteractablesRoutine;

    [Tooltip("the camera through which we look to see if anything interactable is in front of the player")]
    [SerializeField] private Transform playerCamera;

    [SerializeField] private float interactRadius;
    [Tooltip("a value from 0 to 1 that controls how direct the player has to look into an object to be able to interact with it (the lower the number, the wider the range)")]
    [SerializeField, Range(0, 1)] private float minInteractDotProduct;

    [Tooltip("if the distance weight is higher, the player always chooses to interact with the nearest interactable object")]
    [SerializeField, Range(0, 1)] private float distanceWeight = 0.1f;
    [Tooltip("if the dot product weight is higher, the player always chooses to interact with the interactable object that it's directly looking at, whether it's close or not")]
    [SerializeField, Range(0, 1)] private float dotProductWeight = 1f;

    [Tooltip("both obstacles and interactables layers")]
    [SerializeField] private LayerMask considerableLayers;
    [SerializeField] private LayerMask interactablesLayerMask;

    private List<IInteractable> interactablesInRangeList = new List<IInteractable>();
    private IInteractable currentBestInteractableObjectToInteractWith;

    [SerializeField] private bool showGizmos = true;

    private void Start()
    {
        InputManager.instance.onInteract += Interact;
    }

    private void OnEnable()
    {
        if (InputManager.instance != null)
            InputManager.instance.onInteract += Interact;

        if (checkForInteractablesRoutine != null)
            StopCoroutine(checkForInteractablesRoutine);

        checkForInteractablesRoutine = StartCoroutine(CheckForInteractablesCoRoutine());
    }

    private void OnDisable()
    {
        StopCoroutine(checkForInteractablesRoutine);
        checkForInteractablesRoutine = null;

        InputManager.instance.onInteract -= Interact;
    }

    private void Interact()
    {
        currentBestInteractableObjectToInteractWith?.Interact();
    }

    private void CheckForInteractable()
    {
        bool hasFoundNewInteractable = false;

        List<IInteractable> newInteractablesFound = new List<IInteractable>();

        //check and find all the interactables in close to the player
        Collider[] nearbyInteractablesArray = Physics.OverlapSphere(playerCamera.transform.position, interactRadius, interactablesLayerMask);

        //to find which interactable object is most valid for interaction (either it's the closest, or the player is directly looking at it)
        float bestInteractableScore = float.MinValue;

        //check if any interactables were even found
        if (nearbyInteractablesArray != null && nearbyInteractablesArray.Length > 0)
        {
            //check each interactable and find the best one
            for (int i = 0; i < nearbyInteractablesArray.Length; i++)
            {
                //if the object we detected doesn't have an interactable object script on it the move on to the next object
                if (!nearbyInteractablesArray[i].TryGetComponent<IInteractable>(out IInteractable interactable))
                    continue;

                //Debug.Log(interactable);

                newInteractablesFound.Add(interactable);
                if (!interactablesInRangeList.Contains(interactable))
                    interactablesInRangeList.Add(interactable);

                //calculate the dot product to the obejct
                Vector3 directionToInteractable = nearbyInteractablesArray[i].transform.position - playerCamera.transform.position;
                float dotProduct = Vector3.Dot(playerCamera.transform.forward, directionToInteractable.normalized);

                //if the camera/player is slightly pointing towards the interactable object
                if (dotProduct > minInteractDotProduct)
                {
                    //cast a ray to see if there are any obstacles that prevent the player from interacting with the object (eg. a wall)
                    RaycastHit interactableRaycastHit;
                    Physics.Raycast(playerCamera.transform.position, directionToInteractable.normalized, out interactableRaycastHit, directionToInteractable.magnitude, considerableLayers);

                    //for debugging purposes
                    Debug.DrawRay(playerCamera.transform.position, directionToInteractable.normalized * interactRadius);

                    //if the raycast hits the interactable object (meaning there's no obstacles in the way)
                    if (interactableRaycastHit.collider == nearbyInteractablesArray[i])
                    {
                        //calculate the interactable object's weight to see which interactable object is more valid for interaction
                        float distanceScore = (1 - (directionToInteractable.magnitude / interactRadius)) * distanceWeight;
                        float dotScore = dotProduct * dotProductWeight;

                        float interactableScore = distanceScore + dotScore;

                        //Debug.Log(interactable + $": Distance Score: {distanceScore}, Dot Score: {dotScore}, Final Score: {interactableScore}");

                        if (interactableScore > bestInteractableScore)
                        {
                            bestInteractableScore = interactableScore;

                            //if we our best interactable object was some other object, then set that object to not interactable
                            currentBestInteractableObjectToInteractWith?.SetIsInteractable(false);

                            //set the best interactable object variable to the newly found best interactable object
                            currentBestInteractableObjectToInteractWith = interactable;
                            //set the newly found best interactable object to is interactable
                            currentBestInteractableObjectToInteractWith.SetIsInteractable(true);

                            hasFoundNewInteractable = true;
                        }
                    }
                }
            }
        }

        //if we find interactable objects that are near us, then we the them to is in range, so that the interactable ui attached to them can play the corresponding animation
        foreach (IInteractable iInteractable in interactablesInRangeList)
        {
            if (newInteractablesFound.Contains(iInteractable))
                iInteractable.SetIsInInteractRange(true);
            else
                iInteractable.SetIsInInteractRange(false);
        }

        //if no interactable objects were found/chosen, then set the previous best interactable object to not interactable
        if (!hasFoundNewInteractable)
        {
            currentBestInteractableObjectToInteractWith?.SetIsInteractable(false);
        }
    }

    //update the check for interactable logic every few seconds/miliseconds to save performance
    private IEnumerator CheckForInteractablesCoRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkForInteractablesUpdateRate);

            CheckForInteractable();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;

        Gizmos.DrawWireSphere(playerCamera.position, interactRadius);
    }
}
