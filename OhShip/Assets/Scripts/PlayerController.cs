using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{


    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private Transform cameraTransform = null;

    [SerializeField] private ParticleSystem particles;

    [Header("Gravedad")]
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 verticalVelocity;

    private Vector2 moveInput; 
    private bool interacted; 

    private Animator animator;
    private AudioSource stepAudioSource;
    public Animator GiveAnimator() {
        return animator;
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = gameObject.GetComponentInChildren<Animator>();
        stepAudioSource = gameObject.GetComponentInChildren<AudioSource>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Interacted(InputAction.CallbackContext context)
    {
        interacted = context.performed;
    }

    public bool GetInteracted() {
        return interacted;
    }
    public float GetX() {
        return moveInput.x;
    }
    public float GetY() {
        return moveInput.y;
    }

    private void Update()
    {
        float forward = 0f;
        float right = 0f;

        if (Input.GetKeyDown(KeyCode.Escape)) { SceneManager.LoadScene(0); }
        if (Input.GetKeyDown(KeyCode.RightShift)) { SceneManager.LoadScene(1); }

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y); 
        controller.Move(move * speed * Time.deltaTime); 
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            this.transform.rotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0, moveInput.y));
            animator.SetBool("running", true);

            stepAudioSource.enabled = true;

            var em = particles.emission;
            em.enabled = true;
        }
        else
        {
            animator.SetBool("running", false);

            stepAudioSource.enabled = false;

            var em = particles.emission;
            em.enabled = false;
        }

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
