
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // MOVEMENT SETTINGS

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    // GROUND DETECTION

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;

    // JUMP ASSISTANCE

    [Header("Jump Assistance")]
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.05f;

    // STATE

    private bool isGrounded;
    private bool jumpReleased;

    // TIMERS

    private float coyoteTimer;
    private float jumpBufferTimer;

    // REFERENCES

    private Rigidbody2D rb;
    private PlayerControls controls;
    private PlayerAudio playerAudio;

    // INPUT

    private Vector2 moveInput;

    // PUBLIC ANIMATION DATA

    public bool IsGrounded => isGrounded;

    public float HorizontalSpeed =>
        Mathf.Abs(rb != null ? rb.linearVelocity.x : 0f);

    public float VerticalVelocity =>
        rb != null ? rb.linearVelocity.y : 0f;

    // INITIALIZATION

    private void Awake()
    {
        controls = new PlayerControls();

        rb = GetComponent<Rigidbody2D>();

        // Find the PlayerAudio component on the Player.
        playerAudio = GetComponent<PlayerAudio>();
    }

    private void OnEnable()
    {
        controls.Player.Jump.performed += OnJump;
        controls.Player.Jump.canceled += OnJumpReleased;

        controls.Enable();
    }

    // UNITY LOOP

    private void Update()
    {
        ReadInput();
        CheckGround();
        UpdateCoyoteTimer();
        UpdateJumpBuffer();
    }

    private void FixedUpdate()
    {
        // Horizontal movement.
        rb.linearVelocity = new Vector2(
            moveInput.x * moveSpeed,
            rb.linearVelocity.y
        );

        // Jumping with coyote time and jump buffering.
        if (jumpBufferTimer > 0f &&
            coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            jumpReleased = false;
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;

            // Play jump SFX only when a jump succeeds.
            if (playerAudio != null)
            {
                playerAudio.PlayJump();

                Debug.Log("JUMP SOUND TRIGGERED!");
            }
            else
            {
                Debug.LogWarning(
                    "PlayerAudio component not found on Player!"
                );
            }
        }

        // Shorter jump when the jump button is released.
        if (jumpReleased &&
            rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * 0.5f
            );

            jumpReleased = false;
        }
    }

    // INPUT

    private void ReadInput()
    {
        moveInput =
            controls.Player.Move.ReadValue<Vector2>();
    }

    // GROUND CHECK

    private void CheckGround()
    {
        if (groundCheck == null)
            return;

        isGrounded = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );
    }

    // COYOTE TIME

    private void UpdateCoyoteTimer()
    {
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    // JUMP BUFFER

    private void UpdateJumpBuffer()
    {
        jumpBufferTimer -= Time.deltaTime;
    }

    // INPUT CALLBACKS

    private void OnJump(
        InputAction.CallbackContext context)
    {
        jumpBufferTimer = jumpBufferTime;
    }

    private void OnJumpReleased(
        InputAction.CallbackContext context)
    {
        jumpReleased = true;
    }

    // CLEANUP

    private void OnDisable()
    {
        controls.Player.Jump.performed -= OnJump;
        controls.Player.Jump.canceled -= OnJumpReleased;

        controls.Disable();
    }

    // DEBUG

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawLine(
            groundCheck.position,
            groundCheck.position +
            Vector3.down * groundCheckDistance
        );
    }
}
