using UnityEngine;

public enum ItemID
{
    OverchargeCell,
    EmergencyPack,
    RustyScrew,
    HeatSink,
    CrackedCapacitor,
    BlackboxRecorder,
    GravityNail,
    LightInABottle,
    DeatomizingRay,
    ReactiveBarrier
}

[CreateAssetMenu(
    fileName = "NewItem",
    menuName = "World Engine/Item"
)]
public class ItemData : ScriptableObject
{
    [Header("Identity")]
    public ItemID itemID;

    public string itemName = "Item";

    public LootRarity rarity =
        LootRarity.Common;

    [TextArea(2, 5)]
    public string description =
        "Item description.";
}