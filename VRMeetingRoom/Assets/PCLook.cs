using UnityEngine;
using UnityEngine.InputSystem;

public class PCLook : MonoBehaviour
{
    public InputActionReference lookAction; // Assign this in the Inspector
    public float sensitivity = 2.0f;

    private Transform cameraTransform;
    private float rotationX = 0f;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lookAction.action.Enable();
    }

    void Update()
    {
        // Only rotate when the right mouse button is held down
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 lookDelta = lookAction.action.ReadValue<Vector2>();

            float mouseX = lookDelta.x * sensitivity;
            float mouseY = lookDelta.y * sensitivity;

            // Rotate horizontally
            transform.Rotate(Vector3.up * mouseX);

            // Rotate vertically (limit up/down rotation)
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -80f, 80f);
            cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        }
    }
}
