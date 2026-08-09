using UnityEngine;

[CreateAssetMenu(
    fileName = "NewWeaponAttachment",
    menuName = "World Engine/Weapon Attachment"
)]
public class WeaponAttachment : ScriptableObject
{
    [Header("Identity")]
    public string attachmentName = "Attachment";

    [Header("Weapon Multipliers")]
    public float damageMultiplier = 1f;
    public float fireRateMultiplier = 1f;
    public float projectileSpeedMultiplier = 1f;
    public float reloadTimeMultiplier = 1f;

    [Header("Magazine")]
    public float magazineSizeMultiplier = 1f;
    public int magazineFlatBonus = 0;
}