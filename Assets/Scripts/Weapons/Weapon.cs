using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private float fireRate = 8f;
    [SerializeField] private float projectileSpeed = 15f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;

    private float nextFireTime = 0f;

    public void Fire(Vector2 direction)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab missing!");
            return;
        }

        Debug.Log(
     $"Time: {Time.time:F2} | Unscaled: {Time.unscaledTime:F2} | TimeScale: {Time.timeScale} | Next: {nextFireTime:F2}"
 );

        if (Time.time < nextFireTime)
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
            projectileScript.Initialize(
                direction,
                damage,
                projectileSpeed
            );
        }

        Debug.Log("SHOT CREATED");
    }
}