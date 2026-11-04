
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    [Header("Collision Settings")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask enemyLayer;

    private Vector2 direction;
    private float damage;
    private float speed;

    private ProjectileAudio projectileAudio;
    private bool hasHit;

    private void Awake()
    {
        projectileAudio = GetComponent<ProjectileAudio>();
    }

    public void Initialize(
        Vector2 direction,
        float damage,
        float speed)
    {
        this.direction = direction.normalized;
        this.damage = damage;
        this.speed = speed;
    }

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    private void Update()
    {
        if (hasHit)
            return;

        float distance = speed * Time.deltaTime;

        Vector2 currentPosition = transform.position;

        LayerMask collisionLayers =
            terrainLayer | enemyLayer;

        RaycastHit2D hit = Physics2D.Raycast(
            currentPosition,
            direction,
            distance,
            collisionLayers
        );

        if (hit.collider != null)
        {
            transform.position = hit.point;

            HandleHit(hit.collider);
            return;
        }

        transform.position +=
            (Vector3)(direction * distance);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleHit(other);
    }

    private void HandleHit(Collider2D other)
    {
        if (hasHit)
            return;

        EnemyHealth enemyHealth =
            other.GetComponentInParent<EnemyHealth>();

        if (enemyHealth != null)
        {
            hasHit = true;

            enemyHealth.TakeDamage(damage);

            if (projectileAudio != null)
                projectileAudio.PlayEnemyHit();

            Destroy(gameObject);
            return;
        }

        bool hitTerrain =
            (terrainLayer.value &
            (1 << other.gameObject.layer)) != 0;

        if (hitTerrain)
        {
            hasHit = true;

            if (projectileAudio != null)
                projectileAudio.PlayWallHit();

            Destroy(gameObject);
        }
    }
}
