
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyAI : MonoBehaviour
{
    [Header("Player Detection")]
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private float stoppingDistance = 1f;

    [Header("Patrol")]
    [SerializeField] private float turnPauseDuration = 0.5f;

    [Header("Wall Detection")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float wallCheckDistance = 0.7f;
    [SerializeField]
    private Vector2 wallCheckOffset =
        new Vector2(0f, 0.3f);

    [Header("Ledge Detection")]
    [SerializeField] private float ledgeCheckDistance = 0.8f;
    [SerializeField] private float ledgeCheckForward = 0.2f;

    private Transform player;
    private EnemyMovement movement;
    private Collider2D enemyCollider;

    private bool movingRight = true;
    private bool isTurning;
    private float turnTimer;
    private float playerSearchTimer;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        enemyCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        if (player == null)
        {
            playerSearchTimer -= Time.deltaTime;

            if (playerSearchTimer <= 0f)
            {
                FindPlayer();
                playerSearchTimer = 0.5f;
            }
        }

        // Finish an obstacle turn before doing anything else.
        if (isTurning)
        {
            movement.Stop();

            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0f)
            {
                movingRight = !movingRight;
                isTurning = false;
            }

            return;
        }

        if (player != null)
        {
            float distance = Vector2.Distance(
                transform.position,
                player.position
            );

            if (distance <= detectionRadius)
            {
                Chase();
                return;
            }
        }

        Patrol();
    }

    private void FindPlayer()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    private bool IsWallAhead(float direction)
    {
        Vector2 origin =
            (Vector2)transform.position + wallCheckOffset;

        return Physics2D.Raycast(
            origin,
            Vector2.right * direction,
            wallCheckDistance,
            groundLayer
        ).collider != null;
    }

    private bool IsLedgeAhead(float direction)
    {
        if (enemyCollider == null)
            return false;

        Bounds bounds = enemyCollider.bounds;

        Vector2 origin = new Vector2(
            bounds.center.x +
                direction * (bounds.extents.x + ledgeCheckForward),
            bounds.min.y + 0.1f
        );

        return Physics2D.Raycast(
            origin,
            Vector2.down,
            ledgeCheckDistance,
            groundLayer
        ).collider == null;
    }

    private bool IsGrounded()
    {
        if (enemyCollider == null)
            return false;

        Bounds bounds = enemyCollider.bounds;

        Vector2 origin = new Vector2(
            bounds.center.x,
            bounds.min.y + 0.1f
        );

        return Physics2D.Raycast(
            origin,
            Vector2.down,
            0.3f,
            groundLayer
        ).collider != null;
    }



    private void Patrol()
    {
        float direction = movingRight ? 1f : -1f;

        if (IsGrounded() &&
            (IsWallAhead(direction) || IsLedgeAhead(direction)))
        {
            BeginTurn();
            return;
        }

        movement.Move(direction);
    }



    private void Chase()
    {
        float horizontalDistance =
            player.position.x - transform.position.x;

        if (Mathf.Abs(horizontalDistance) <= stoppingDistance)
        {
            movement.Stop();
            return;
        }

        float direction = Mathf.Sign(horizontalDistance);

        if (IsGrounded() &&
            (IsWallAhead(direction) || IsLedgeAhead(direction)))
        {
            movement.Stop();
            return;
        }

        movement.Move(direction);
    }

    private void BeginTurn()
    {
        movement.Stop();
        isTurning = true;
        turnTimer = turnPauseDuration;
    }

    private void OnDrawGizmosSelected()
    {
        Collider2D col = GetComponent<Collider2D>();

        if (col == null)
            return;

        Bounds bounds = col.bounds;

        Gizmos.color = Color.yellow;

        Vector2 wallOrigin =
            (Vector2)transform.position + wallCheckOffset;

        Gizmos.DrawLine(
            wallOrigin,
            wallOrigin + Vector2.right * wallCheckDistance
        );

        Gizmos.DrawLine(
            wallOrigin,
            wallOrigin + Vector2.left * wallCheckDistance
        );

        Gizmos.color = Color.cyan;

        for (int i = -1; i <= 1; i += 2)
        {
            Vector2 ledgeOrigin = new Vector2(
                bounds.center.x +
                    i * (bounds.extents.x + ledgeCheckForward),
                bounds.min.y + 0.1f
            );

            Gizmos.DrawLine(
                ledgeOrigin,
                ledgeOrigin + Vector2.down * ledgeCheckDistance
            );
        }
    }
}
