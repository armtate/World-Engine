using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] private string weaponName = "Weapon";

    [Header("Weapon Stats")]
    [SerializeField] private float baseDamage = 1f;
    [SerializeField] private float baseFireRate = 8f;
    [SerializeField] private float baseProjectileSpeed = 15f;

    [Header("Rarity")]
    [SerializeField]
    private WeaponRarity rarity =
        WeaponRarity.Common;

    [Header("Ammo")]
    [SerializeField] private int baseMagazineSize = 30;
    [SerializeField] private int reserveAmmo = 120;
    [SerializeField] private float baseReloadTime = 1.5f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private int currentAmmo;
    private float nextFireTime;

    private bool isReloading;
    private float reloadEndTime;

    private float damageMultiplier = 1f;
    private float fireRateMultiplier = 1f;
    private float projectileSpeedMultiplier = 1f;
    private float reloadTimeMultiplier = 1f;

    private float magazineSizeMultiplier = 1f;
    private int magazineFlatBonus = 0;

    public string WeaponName => weaponName;

    public int CurrentAmmo => currentAmmo;
    public int ReserveAmmo => reserveAmmo;

    public WeaponRarity Rarity => rarity;

    public bool IsReloading => isReloading;

    public float Damage =>
        baseDamage * damageMultiplier;

    public float FireRate =>
        baseFireRate * fireRateMultiplier;

    public float ProjectileSpeed =>
        baseProjectileSpeed * projectileSpeedMultiplier;

    public float ReloadTime =>
        baseReloadTime * reloadTimeMultiplier;

    public int MagazineSize =>
        Mathf.Max(
            1,
            Mathf.RoundToInt(
                baseMagazineSize * magazineSizeMultiplier
            ) + magazineFlatBonus
        );

    private void Awake()
    {
        // Automatically find the FirePoint child.
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
        }

        currentAmmo = MagazineSize;
    }

    private void Update()
    {
        CheckReload();
    }

    private void OnEnable()
    {
        CheckReload();
    }

    public void Fire(Vector2 direction)
    {
        CheckReload();

        if (isReloading)
            return;

        if (projectilePrefab == null)
            return;

        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        float timeBetweenShots =
            1f / FireRate;

        if (Time.time < nextFireTime)
            return;

        nextFireTime =
            Time.time + timeBetweenShots;

        currentAmmo--;

        // Use the FirePoint if the weapon has one.
        // Otherwise, fall back to the weapon's position.
        Vector3 spawnPosition =
            firePoint != null
                ? firePoint.position
                : transform.position;

        GameObject projectile =
            Instantiate(
                projectilePrefab,
                spawnPosition,
                Quaternion.identity
            );

        WeaponProjectile projectileScript =
            projectile.GetComponent<WeaponProjectile>();

        if (projectileScript != null)
        {
            projectileScript.Initialize(
                direction,
                Damage,
                ProjectileSpeed
            );
        }
    }

    public void StartReload()
    {
        if (isReloading)
            return;

        if (currentAmmo >= MagazineSize)
            return;

        if (reserveAmmo <= 0)
            return;

        isReloading = true;

        reloadEndTime =
            Time.time + ReloadTime;
    }

    private void CheckReload()
    {
        if (!isReloading)
            return;

        if (Time.time < reloadEndTime)
            return;

        FinishReload();
    }

    private void FinishReload()
    {
        int ammoNeeded =
            MagazineSize - currentAmmo;

        int ammoToLoad =
            Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;
    }

    public void ApplyAttachment(
        WeaponAttachment attachment)
    {
        if (attachment == null)
            return;

        int oldMagazineSize =
            MagazineSize;

        damageMultiplier *=
            attachment.damageMultiplier;

        fireRateMultiplier *=
            attachment.fireRateMultiplier;

        projectileSpeedMultiplier *=
            attachment.projectileSpeedMultiplier;

        reloadTimeMultiplier *=
            attachment.reloadTimeMultiplier;

        magazineSizeMultiplier *=
            attachment.magazineSizeMultiplier;

        magazineFlatBonus +=
            attachment.magazineFlatBonus;

        int magazineIncrease =
            MagazineSize - oldMagazineSize;

        if (magazineIncrease > 0)
        {
            currentAmmo += magazineIncrease;
        }
    }
}