using UnityEngine;

[CreateAssetMenu(
    fileName = "NewWeaponAttachment",
    menuName = "World Engine/Weapon Attachment"
)]
public class WeaponAttachment : ScriptableObject
{
    [Header("Identity")]
    public string attachmentName = "Attachment";

    public LootRarity rarity =
        LootRarity.Common;

    [TextArea(2, 5)]
    public string description =
        "Attachment description.";

    [Header("Compatibility")]
    public AttachmentSlotType slotType;

    public WeaponFamily[] allowedWeaponFamilies;

    [Header("Behavior")]
    public WeaponBehaviorModifier behaviorModifier =
        WeaponBehaviorModifier.None;

    [Header("Weapon Multipliers")]
    public float damageMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float projectileSpeedMultiplier = 1f;
    public float reloadTimeMultiplier = 1f;

    [Header("Magazine")]
    public float magazineSizeMultiplier = 1f;
    public int magazineFlatBonus = 0;

    public bool IsCompatibleWith(
        WeaponFamily weaponFamily)
    {
        if (allowedWeaponFamilies == null ||
            allowedWeaponFamilies.Length == 0)
        {
            return false;
        }

        foreach (
            WeaponFamily allowedFamily
            in allowedWeaponFamilies)
        {
            if (allowedFamily == weaponFamily)
            {
                return true;
            }
        }

        return false;
    }
}