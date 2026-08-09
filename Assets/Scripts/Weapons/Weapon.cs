using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] protected float damage = 1f;
    [SerializeField] protected float fireRate = 5f;
    [SerializeField] protected float projectileSpeed = 10f;

    [Header("Projectile")]
    [SerializeField] protected GameObject projectilePrefab;

    private float fireTimer;

    private void Update()
    {
        if (fireTimer > 0f)
        {
            fireTimer -= Time.deltaTime;
        }
    }

    public void Fire(Vector2 direction)
    {
        Debug.Log("Fire() called");

        if (projectilePrefab == null)
        {
            Debug.LogError("Weapon has NO projectile prefab!");
            return;
        }

        if (fireTimer > 0f)
        {
            return;
        }

        fireTimer = 1f / fireRate;

        GameObject projectile = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        Debug.Log("Projectile created");

        WeaponProjectile projectileScript =
            projectile.GetComponent<WeaponProjectile>();

        if (projectileScript != null)
        {
            projectileScript.Initialize(
                direction,
                damage,
                projectileSpeed
            );
        }
        else
        {
            Debug.LogError("Projectile prefab has NO WeaponProjectile script!");
        }
    }
}