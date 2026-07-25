using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private float patrolDistance = 3f;

    [SerializeField]
    private float patrolWaitTime = 1.5f;

    private Vector2 patrolStartPosition;
    private bool movingRight = true;
    private float waitTimer;
    private enum EnemyState
    {
        Patrol,
        Chase
    }

    private EnemyState currentState = EnemyState.Patrol;

    [SerializeField]
    private float detectionRadius = 1000f;

    private Transform player;
    private EnemyMovement movement;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        patrolStartPosition = transform.position;
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            currentState = EnemyState.Chase;
        }
        else
        {
            currentState = EnemyState.Patrol;
        }

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                float direction = Mathf.Sign(player.position.x - transform.position.x);
                movement.Move(direction);
                break;
        }
    }

    private void Patrol()
    {
        float moveDirection = movingRight ? 1f : -1f;

        movement.Move(moveDirection);

        float distanceFromStart = Mathf.Abs(transform.position.x - patrolStartPosition.x);

        if (distanceFromStart >= patrolDistance)
        {
            movement.Stop();

            waitTimer += Time.deltaTime;

            if (waitTimer >= patrolWaitTime)
            {
                movingRight = !movingRight;
                waitTimer = 0f;
            }
        }
    }
}