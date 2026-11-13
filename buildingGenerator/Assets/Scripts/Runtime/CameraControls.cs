using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

public class EditorFlyCamera : MonoBehaviour
{
    [Header("Movement Settings")]
    public float normalSpeed = 10f;
    public float fastSpeedMultiplier = 3f;
    public float mouseSensitivity = 2f;

    private float rotX;
    private float rotY;

    void Start()
    {
        rotX = transform.localEulerAngles.y;
        rotY = transform.localEulerAngles.x;
    }

    void Update()
    {
        // Ensure Keyboard and Mouse are connected
        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;
        if (keyboard == null || mouse == null) return;

        // Engage fly mode when holding the Right Mouse Button
        if (mouse.rightButton.isPressed)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Mouse look using new Input System delta
            Vector2 mouseDelta = mouse.delta.ReadValue();
            rotX += mouseDelta.x * mouseSensitivity * 0.1f;
            rotY -= mouseDelta.y * mouseSensitivity * 0.1f;
            rotY = Mathf.Clamp(rotY, -90f, 90f);

            transform.localRotation = Quaternion.Euler(rotY, rotX, 0f);

            // Speed boost with Shift
            float speed = normalSpeed;
            if (keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed)
            {
                speed *= fastSpeedMultiplier;
            }

            // WASD / QE movement
            float moveFB = 0f;
            float moveLR = 0f;
            float moveUD = 0f;

            if (keyboard.wKey.isPressed) moveFB = 1f;
            if (keyboard.sKey.isPressed) moveFB = -1f;
            if (keyboard.aKey.isPressed) moveLR = -1f;
            if (keyboard.dKey.isPressed) moveLR = 1f;
            if (keyboard.eKey.isPressed) moveUD = 1f; // Up
            if (keyboard.qKey.isPressed) moveUD = -1f; // Down

            Vector3 moveVector = new Vector3(moveLR, moveUD, moveFB);
            transform.Translate(moveVector * speed * Time.deltaTime);
        }
        else
        {
            // Release cursor when right click is let go
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}