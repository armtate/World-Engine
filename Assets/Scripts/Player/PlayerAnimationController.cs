using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Facing")]
    [SerializeField] private Camera mainCamera;

    private bool facingRight = true;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (animator == null ||
            movement == null)
        {
            return;
        }

        UpdateAnimations();
        UpdateFacing();
    }

    private void UpdateAnimations()
    {
        animator.SetBool(
            "IsGrounded",
            movement.IsGrounded
        );

        animator.SetFloat(
            "YVelocity",
            movement.VerticalVelocity
        );

        animator.SetFloat(
            "Speed",
            movement.HorizontalSpeed
        );
    }

    private void UpdateFacing()
    {
        if (spriteRenderer == null)
            return;

        if (mainCamera == null)
            return;

        if (Mouse.current == null)
            return;

        Vector2 mouseScreenPosition =
            Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition =
            mainCamera.ScreenToWorldPoint(
                mouseScreenPosition
            );

        float difference =
            mouseWorldPosition.x -
            transform.position.x;

        if (difference > 0.05f)
        {
            facingRight = true;
        }
        else if (difference < -0.05f)
        {
            facingRight = false;
        }

        spriteRenderer.flipX =
            !facingRight;
    }
}