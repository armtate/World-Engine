
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;

    private Rigidbody2D rb;
    private float moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            moveDirection * moveSpeed,
            rb.linearVelocity.y
        );
    }

    public void Move(float direction)
    {
        moveDirection = Mathf.Clamp(direction, -1f, 1f);
    }

    public void Stop()
    {
        moveDirection = 0f;
    }
}
