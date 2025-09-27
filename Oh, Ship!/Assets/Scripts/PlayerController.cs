using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private Transform cameraTransform = null;

    [Header("Gravedad")]
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Leemos WASD mediante Keyboard.current
        float forward = 0f;
        float right = 0f;

        if (Keyboard.current.wKey.isPressed) forward += 1f;
        if (Keyboard.current.sKey.isPressed) forward -= 1f;
        if (Keyboard.current.dKey.isPressed) right += 1f;
        if (Keyboard.current.aKey.isPressed) right -= 1f;

        Vector3 move;

        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = cameraTransform.right;
            camRight.y = 0f;
            camRight.Normalize();

            move = camForward * forward + camRight * right;
        }
        else
        {
            move = new Vector3(right, 0f, forward);
        }

        if (move.sqrMagnitude > 0.001f)
        {
            move = move.normalized * speed;

            Quaternion targetRot = Quaternion.LookRotation(new Vector3(move.x, 0f, move.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            controller.Move(move * Time.deltaTime);
        }

        // Gravedad
        if (!controller.isGrounded)
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity.y = Mathf.Min(verticalVelocity.y, -0.1f);
        }

        controller.Move(verticalVelocity * Time.deltaTime);
    }
}
