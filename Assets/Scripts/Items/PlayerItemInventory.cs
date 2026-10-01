using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInventory : MonoBehaviour
{
    private Dictionary<ItemID, int> itemStacks =
        new Dictionary<ItemID, int>();

    private float emergencyPackReadyTime;

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
            (overchargeStacks *
            bonusDamagePerStack);
    }

    public void OnPlayerDamaged(
        PlayerHealth playerHealth)
    {
        if (playerHealth == null)
            return;

        TryUseEmergencyPack(playerHealth);
    }

    private void TryUseEmergencyPack(
        PlayerHealth playerHealth)
    {
        int stacks =
            GetStackCount(
                ItemID.EmergencyPack
            );

        if (stacks <= 0)
            return;

        if (Time.time < emergencyPackReadyTime)
            return;

        float activationThreshold =
            0.40f +
            ((stacks - 1) * 0.05f);

        activationThreshold =
            Mathf.Clamp01(
                activationThreshold
            );

        if (
            playerHealth.HealthPercent >
            activationThreshold
        )
        {
            return;
        }

        float healingPercent =
            0.25f +
            ((stacks - 1) * 0.10f);

        int healingAmount =
            Mathf.CeilToInt(
                playerHealth.MaxHealth *
                healingPercent
            );

        float cooldown =
            30f -
            ((stacks - 1) * 3f);

        cooldown =
            Mathf.Max(
                cooldown,
                10f
            );

        playerHealth.Heal(
            healingAmount
        );

        emergencyPackReadyTime =
            Time.time + cooldown;

        Debug.Log(
            "Emergency Pack activated!" +
            " | Healed: " +
            healingAmount +
            " | Cooldown: " +
            cooldown +
            " seconds"
        );
    }
}