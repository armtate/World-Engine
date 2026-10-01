using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;

    [Header("Damage Number")]
    [SerializeField]
    private Vector3 damageNumberOffset =
        new Vector3(0f, -2f, 0f);

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        int actualDamage =
            Mathf.RoundToInt(damage);

        currentHealth -= actualDamage;

        SpawnDamageNumber(actualDamage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void SpawnDamageNumber(int damage)
    {
        if (DamageNumberSpawner.Instance == null)
            return;

        Vector3 spawnPosition =
            transform.position +
            damageNumberOffset;

        DamageNumberSpawner.Instance.Spawn(
            damage,
            spawnPosition
        );
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}