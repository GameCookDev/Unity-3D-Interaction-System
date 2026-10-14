using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField] private float speed = 7f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float x = InputManager.instance.GetPlayerMovement().x;
        float z = InputManager.instance.GetPlayerMovement().y;

        Vector3 movement = transform.right * x + transform.forward * z;

        characterController.Move(movement * speed * Time.deltaTime);
    }
}
