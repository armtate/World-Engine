
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] private string weaponName = "Weapon";

    [SerializeField]
    private WeaponFamily weaponFamily = WeaponFamily.Rifle;

    [Header("Supported Attachment Slots")]
    [SerializeField]
    private AttachmentSlotType[] supportedSlots;

    [Header("Weapon Stats")]
    [SerializeField] private float baseDamage = 1f;
    [SerializeField] private float baseFireRate = 8f;
    [SerializeField] private float baseProjectileSpeed = 15f;

    [Header("Accuracy")]
    [SerializeField, Range(0f, 30f)]
    private float baseSpreadAngle = 3f;

    private float spreadMultiplier = 1f;

    public float SpreadAngle =>
        Mathf.Max(0f, baseSpreadAngle * spreadMultiplier);

    [Header("Rarity")]
    [SerializeField]
    private LootRarity rarity = LootRarity.Common;

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
    private int magazineFlatBonus;

    private PlayerItemInventory itemInventory;

    private readonly Dictionary<AttachmentSlotType, WeaponAttachment>
        equippedAttachments =
        new Dictionary<AttachmentSlotType, WeaponAttachment>();

    public string WeaponName => weaponName;
    public WeaponFamily WeaponFamily => weaponFamily;
    public int CurrentAmmo => currentAmmo;
    public int ReserveAmmo => reserveAmmo;
    public LootRarity Rarity => rarity;
    public bool IsReloading => isReloading;

    public float Damage
    {
        get
        {
            float itemDamageMultiplier = 1f;

            if (itemInventory != null)
            {
                itemDamageMultiplier =
                    itemInventory.GetWeaponDamageMultiplier();
            }

            return baseDamage *
                damageMultiplier *
                itemDamageMultiplier;
        }
    }

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
        if (firePoint == null)
            firePoint = transform.Find("FirePoint");

        itemInventory =
            GetComponentInParent<PlayerItemInventory>();

        RecalculateAttachmentStats();

        currentAmmo = MagazineSize;
    }

    private void Update()
    {
        CheckReload();
    }

    private void OnEnable()
    {
        CheckReload();

        if (itemInventory == null)
        {
            itemInventory =
                GetComponentInParent<PlayerItemInventory>();
        }
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

        if (FireRate <= 0f)
            return;

        float timeBetweenShots = 1f / FireRate;

        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + timeBetweenShots;

        // Only the original shot consumes ammo.
        currentAmmo--;

        // Fire the normal projectile.
        SpawnProjectile(direction);

        // Check for Rusty Screw activation.
        TryActivateRustyScrew(direction);
    }

    private void SpawnProjectile(Vector2 direction)
    {
        float randomAngle =
    Random.Range(-SpreadAngle, SpreadAngle);

        Vector2 spreadDirection =
            Quaternion.Euler(0f, 0f, randomAngle) * direction.normalized;

        Vector3 spawnPosition =
            firePoint != null
                ? firePoint.position
                : transform.position;

        GameObject projectile = Instantiate(
            projectilePrefab,
            spawnPosition,
            Quaternion.identity
        );

        WeaponProjectile projectileScript =
            projectile.GetComponent<WeaponProjectile>();

        if (projectileScript != null)
        {
            projectileScript.Initialize(
                spreadDirection,
                Damage,
                ProjectileSpeed
            );
        }

        Debug.Log(
            weaponName + " fired for " + Damage + " damage."
        );
    }

    private void TryActivateRustyScrew(Vector2 direction)
    {
        if (itemInventory == null)
            return;

        int stacks =
            itemInventory.GetStackCount(ItemID.RustyScrew);

        if (stacks <= 0)
            return;

        float activationChance =
            Mathf.Clamp01(stacks * .10f);

        if (Random.value >= activationChance)
            return;

        // Bonus shot does not consume ammo or trigger itself.
        SpawnProjectile(direction);

        Debug.Log(
            "Rusty Screw activated! | Stacks: " + stacks
        );
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

        reloadEndTime = Time.time + ReloadTime;
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
        int ammoNeeded = MagazineSize - currentAmmo;

        int ammoToLoad = Mathf.Min(
            ammoNeeded,
            reserveAmmo
        );

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;
    }

    public bool CanUseAttachment(WeaponAttachment attachment)
    {
        if (attachment == null)
            return false;

        if (!SupportsSlot(attachment.slotType))
            return false;

        if (!attachment.IsCompatibleWith(weaponFamily))
            return false;

        return true;
    }

    public bool ApplyAttachment(WeaponAttachment attachment)
    {
        if (!CanUseAttachment(attachment))
            return false;

        int oldMagazineSize = MagazineSize;

        equippedAttachments[attachment.slotType] = attachment;

        RecalculateAttachmentStats();
        spreadMultiplier = 1f;

        int magazineDifference =
            MagazineSize - oldMagazineSize;

        if (magazineDifference > 0)
        {
            currentAmmo += magazineDifference;
        }
        else if (currentAmmo > MagazineSize)
        {
            currentAmmo = MagazineSize;
        }

        return true;
    }

    public bool SupportsSlot(AttachmentSlotType slot)
    {
        if (supportedSlots == null)
            return false;

        foreach (AttachmentSlotType supportedSlot
                 in supportedSlots)
        {
            if (supportedSlot == slot)
                return true;
        }

        return false;
    }

    public WeaponAttachment GetAttachment(
        AttachmentSlotType slot)
    {
        equippedAttachments.TryGetValue(
            slot,
            out WeaponAttachment attachment
        );

        return attachment;
    }

    private void RecalculateAttachmentStats()
    {
        damageMultiplier = 1f;
        fireRateMultiplier = 1f;
        projectileSpeedMultiplier = 1f;
        reloadTimeMultiplier = 1f;

        magazineSizeMultiplier = 1f;
        magazineFlatBonus = 0;

        foreach (WeaponAttachment attachment
                 in equippedAttachments.Values)
        {
            if (attachment == null)
                continue;

            damageMultiplier *= attachment.damageMultiplier;
            fireRateMultiplier *= attachment.fireRateMultiplier;
            projectileSpeedMultiplier *=
                attachment.projectileSpeedMultiplier;
            reloadTimeMultiplier *=
                attachment.reloadTimeMultiplier;
            magazineSizeMultiplier *=
                attachment.magazineSizeMultiplier;

            magazineFlatBonus += attachment.magazineFlatBonus;
        }
    }
}
