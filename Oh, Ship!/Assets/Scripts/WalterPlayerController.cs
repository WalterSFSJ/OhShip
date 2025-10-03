using UnityEngine;
using UnityEngine.InputSystem;

public class WalterPlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;


    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;


    void Awake()
    {
        controller = GetComponent<CharacterController>();

    }

    public void Move(InputAction.CallbackContext context)
    {
       moveInput = context.ReadValue<Vector2>();
    }
        
    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        controller.Move(move * speed * Time.deltaTime);
    }
}
