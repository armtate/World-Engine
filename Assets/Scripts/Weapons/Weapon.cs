using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] protected float damage = 1f;
    [SerializeField] protected float fireRate = 5f;
    [SerializeField] protected float projectileSpeed = 10f;

    [Header("Projectile")]
    [SerializeField] protected GameObject projectilePrefab;

    private float nextFireTime;

    public void Fire(Vector2 direction)
    {
        if (Time.time < nextFireTime)
            return;

        if (projectilePrefab == null)
            return;

        nextFireTime = Time.time + (1f / fireRate);

        GameObject projectile = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        WeaponProjectile projectileScript =
            projectile.GetComponent<WeaponProjectile>();

        if (projectileScript != null)
        {
            projectileScript.Initialize(direction, damage, projectileSpeed);
        }
    }
}