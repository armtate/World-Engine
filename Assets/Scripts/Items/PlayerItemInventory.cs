using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInventory : MonoBehaviour
{
    private Dictionary<ItemID, int> itemStacks =
        new Dictionary<ItemID, int>();

    public void AddItem(ItemData item)
    {
        if (item == null)
            return;

        if (itemStacks.ContainsKey(item.itemID))
        {
            itemStacks[item.itemID]++;
        }
        else
        {
            itemStacks.Add(item.itemID, 1);
        }

        Debug.Log(
            "Collected " +
            item.itemName +
            " | Stacks: " +
            GetStackCount(item.itemID)
        );
    }

    public int GetStackCount(ItemID itemID)
    {
        if (itemStacks.TryGetValue(
            itemID,
            out int count))
        {
            return count;
        }

        return 0;
    }

    public bool HasItem(ItemID itemID)
    {
        return GetStackCount(itemID) > 0;
    }

    public float GetWeaponDamageMultiplier()
    {
        int overchargeStacks =
            GetStackCount(
                ItemID.OverchargeCell
            );

        float bonusDamagePerStack = 0.25f;

        return 1f +
            (overchargeStacks * bonusDamagePerStack);
    }
}