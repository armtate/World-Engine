using TMPro;
using UnityEngine;

public class ItemInfoUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text rarityText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text statsText;

    [Header("Comparison Colors")]
    [SerializeField] private Color betterColor = Color.green;
    [SerializeField] private Color worseColor = Color.red;
    [SerializeField] private Color equalColor = Color.white;
    [SerializeField] private Color currentValueColor = Color.gray;

    [Header("Rarity Colors")]
    [SerializeField] private Color commonColor = Color.white;
    [SerializeField] private Color uncommonColor = Color.green;
    [SerializeField] private Color rareColor = Color.blue;
    [SerializeField] private Color epicColor = Color.magenta;
    [SerializeField] private Color legendaryColor = Color.yellow;

    private void Awake()
    {
        Hide();
    }

    // =========================
    // WEAPONS
    // =========================

    public void ShowWeapon(
        Weapon pickupWeapon,
        Weapon currentWeapon)
    {
        if (panel == null || pickupWeapon == null)
            return;

        panel.SetActive(true);

        itemNameText.text =
            pickupWeapon.WeaponName;

        rarityText.text =
            pickupWeapon.Rarity
                .ToString()
                .ToUpper();

        rarityText.color =
            GetRarityColor(
                pickupWeapon.Rarity
            );

        descriptionText.text =
            "Weapon";

        if (currentWeapon == null)
        {
            statsText.text =
                BuildWeaponStatsOnly(
                    pickupWeapon
                );
        }
        else
        {
            statsText.text =
                BuildWeaponComparison(
                    pickupWeapon,
                    currentWeapon
                );
        }
    }

    private string BuildWeaponStatsOnly(
        Weapon weapon)
    {
        return
            "Damage        " +
            weapon.Damage.ToString("0.##") +

            "\nFire Rate     " +
            weapon.FireRate.ToString("0.##") +

            "\nMagazine      " +
            weapon.MagazineSize +

            "\nReload        " +
            weapon.ReloadTime.ToString("0.##") +
            "s";
    }

    private string BuildWeaponComparison(
        Weapon pickup,
        Weapon current)
    {
        return
            BuildStatLine(
                "Damage",
                pickup.Damage,
                current.Damage,
                true
            ) +

            "\n" +

            BuildStatLine(
                "Fire Rate",
                pickup.FireRate,
                current.FireRate,
                true
            ) +

            "\n" +

            BuildStatLine(
                "Magazine",
                pickup.MagazineSize,
                current.MagazineSize,
                true
            ) +

            "\n" +

            BuildStatLine(
                "Reload",
                pickup.ReloadTime,
                current.ReloadTime,
                false,
                "s"
            );
    }

    private string BuildStatLine(
        string statName,
        float newValue,
        float currentValue,
        bool higherIsBetter,
        string suffix = "")
    {
        string symbol;
        Color comparisonColor;

        if (Mathf.Approximately(
            newValue,
            currentValue))
        {
            symbol = "=";
            comparisonColor = equalColor;
        }
        else
        {
            bool improvement =
                higherIsBetter
                    ? newValue > currentValue
                    : newValue < currentValue;

            if (improvement)
            {
                symbol = "▲";
                comparisonColor = betterColor;
            }
            else
            {
                symbol = "▼";
                comparisonColor = worseColor;
            }
        }

        string comparisonHex =
            ColorUtility.ToHtmlStringRGB(
                comparisonColor
            );

        string currentValueHex =
            ColorUtility.ToHtmlStringRGB(
                currentValueColor
            );

        return
            statName.PadRight(13) +
            newValue.ToString("0.##") +
            suffix +
            "  " +

            "<color=#" +
            comparisonHex +
            ">" +
            symbol +
            "</color>" +

            "  <color=#" +
            currentValueHex +
            ">(" +
            currentValue.ToString("0.##") +
            suffix +
            ")</color>";
    }

    // =========================
    // REGULAR ITEMS
    // =========================

    public void ShowItem(
        ItemData item)
    {
        if (panel == null || item == null)
            return;

        panel.SetActive(true);

        itemNameText.text =
            item.itemName;

        rarityText.text =
            item.rarity
                .ToString()
                .ToUpper();

        rarityText.color =
            GetRarityColor(
                item.rarity
            );

        descriptionText.text =
            item.description;

        statsText.text =
            "";
    }

    // =========================
    // ATTACHMENTS
    // =========================

    public void ShowAttachment(
        WeaponAttachment attachment)
    {
        if (panel == null ||
            attachment == null)
            return;

        panel.SetActive(true);

        itemNameText.text =
            attachment.attachmentName;

        rarityText.text =
            attachment.rarity
                .ToString()
                .ToUpper();

        rarityText.color =
            GetRarityColor(
                attachment.rarity
            );

        descriptionText.text =
            attachment.description;

        statsText.text =
            BuildAttachmentEffects(
                attachment
            );
    }

    private string BuildAttachmentEffects(
        WeaponAttachment attachment)
    {
        string effects = "";

        if (!Mathf.Approximately(
            attachment.damageMultiplier,
            1f))
        {
            effects +=
                "Damage: ×" +
                attachment.damageMultiplier
                    .ToString("0.##");
        }

        if (!Mathf.Approximately(
            attachment.fireRateMultiplier,
            1f))
        {
            AddEffectLine(ref effects);

            effects +=
                "Fire Rate: ×" +
                attachment.fireRateMultiplier
                    .ToString("0.##");
        }

        if (!Mathf.Approximately(
            attachment.projectileSpeedMultiplier,
            1f))
        {
            AddEffectLine(ref effects);

            effects +=
                "Projectile Speed: ×" +
                attachment.projectileSpeedMultiplier
                    .ToString("0.##");
        }

        if (!Mathf.Approximately(
            attachment.reloadTimeMultiplier,
            1f))
        {
            AddEffectLine(ref effects);

            effects +=
                "Reload Time: ×" +
                attachment.reloadTimeMultiplier
                    .ToString("0.##");
        }

        if (!Mathf.Approximately(
            attachment.magazineSizeMultiplier,
            1f) ||
            attachment.magazineFlatBonus != 0)
        {
            AddEffectLine(ref effects);

            effects +=
                "Magazine: ×" +
                attachment.magazineSizeMultiplier
                    .ToString("0.##");

            if (attachment.magazineFlatBonus > 0)
            {
                effects +=
                    " +" +
                    attachment.magazineFlatBonus;
            }
            else if (
                attachment.magazineFlatBonus < 0)
            {
                effects +=
                    " " +
                    attachment.magazineFlatBonus;
            }
        }

        if (string.IsNullOrEmpty(effects))
        {
            effects =
                "No stat changes.";
        }

        return effects;
    }

    private void AddEffectLine(
        ref string effects)
    {
        if (!string.IsNullOrEmpty(effects))
        {
            effects += "\n";
        }
    }

    // =========================
    // RARITY
    // =========================

    private Color GetRarityColor(
        LootRarity rarity)
    {
        switch (rarity)
        {
            case LootRarity.Common:
                return commonColor;

            case LootRarity.Uncommon:
                return uncommonColor;

            case LootRarity.Rare:
                return rareColor;

            case LootRarity.Epic:
                return epicColor;

            case LootRarity.Legendary:
                return legendaryColor;

            default:
                return Color.white;
        }
    }

    // =========================
    // HIDE
    // =========================

    public void Hide()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}