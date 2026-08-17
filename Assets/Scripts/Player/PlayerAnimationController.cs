using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement movement;

    private void Update()
    {
        if (animator == null ||
            movement == null)
        {
            return;
        }

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
}