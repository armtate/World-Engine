using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private float fireRate = 8f;
    [SerializeField] private float projectileSpeed = 15f;

    [Header("Ammo")]
    [SerializeField] private int magazineSize = 30;
    [SerializeField] private int reserveAmmo = 120;
    [SerializeField] private float reloadTime = 1.5f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading;

    public int CurrentAmmo => currentAmmo;
    public int ReserveAmmo => reserveAmmo;
    public int MagazineSize => magazineSize;
    public bool IsReloading => isReloading;

    private void Awake()
    {
        currentAmmo = magazineSize;
    }

    public void Fire(Vector2 direction)
    {
        if (isReloading)
            return;

        if (projectilePrefab == null)
            return;

        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        float timeBetweenShots = 1f / fireRate;

        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + timeBetweenShots;

        currentAmmo--;

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
    }

    public void StartReload()
    {
        if (isReloading)
            return;

        if (currentAmmo >= magazineSize)
            return;

        if (reserveAmmo <= 0)
            return;

        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = magazineSize - currentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;
    }
}