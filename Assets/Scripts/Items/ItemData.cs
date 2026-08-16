using UnityEngine;

[CreateAssetMenu(
    fileName = "NewItem",
    menuName = "World Engine/Item"
)]
public class ItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemName = "Item";

    public LootRarity rarity =
        LootRarity.Common;

    [TextArea(2, 5)]
    public string description =
        "Item description.";
}