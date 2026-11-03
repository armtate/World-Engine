
using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    [Header("Collision Settings")]
    [SerializeField] private LayerMask terrainLayer;

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

        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        // Check whether we hit an enemy.
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

        // Check whether we hit terrain.
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
