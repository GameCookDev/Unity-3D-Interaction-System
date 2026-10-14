using UnityEngine;

public class FirstPersonCamea : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 10f;

    [SerializeField] private Transform player;

    private float xRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = InputManager.instance.GetMouseDelta().x * mouseSensitivity * Time.deltaTime;
        float mouseY = InputManager.instance.GetMouseDelta().y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        player.Rotate(Vector3.up * mouseX);
    }
}
