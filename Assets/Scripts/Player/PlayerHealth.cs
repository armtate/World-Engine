using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public float HealthPercent
    {
        get
        {
            if (maxHealth <= 0)
                return 0f;

            return (float)currentHealth / maxHealth;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
            return;

        currentHealth -= damage;

        currentHealth =
            Mathf.Max(currentHealth, 0);

        Debug.Log(
            "Player HP: " +
            currentHealth +
            "/" +
            maxHealth
        );

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        PlayerItemInventory inventory =
            GetComponent<PlayerItemInventory>();

        if (inventory != null)
        {
            inventory.OnPlayerDamaged(this);
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0)
            return;

        if (currentHealth <= 0)
            return;

        currentHealth += amount;

        currentHealth =
            Mathf.Min(
                currentHealth,
                maxHealth
            );

        Debug.Log(
            "Player healed! HP: " +
            currentHealth +
            "/" +
            maxHealth
        );
    }

    private void Die()
    {
        Debug.Log("PLAYER DEAD");

        Time.timeScale = 0f;
    }
}