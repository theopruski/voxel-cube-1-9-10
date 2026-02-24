using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float verticalSpeed = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;
    private Rigidbody rb;
    float xRotation = 0f;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }
    void Update()
    {
        if (Keyboard.current == null || Mouse.current == null) return;

        HandleMovement();
        HandleMouseLook();
    }
    void HandleMovement()
    {
        Vector3 direction = Vector3.zero;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.zKey.isPressed)
            direction += transform.forward;
        if (Keyboard.current.sKey.isPressed)
            direction -= transform.forward;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.qKey.isPressed)
            direction -= transform.right;
        if (Keyboard.current.dKey.isPressed)
            direction += transform.right;
        if (Keyboard.current.spaceKey.isPressed)
            direction += Vector3.up * verticalSpeed;
        if (Keyboard.current.leftCtrlKey.isPressed)
            direction -= Vector3.up * verticalSpeed;
        Vector3 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(direction.x * speed, direction.y, direction.z * speed);
    }
    void HandleMouseLook()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;
        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseDelta.x);
    }
}