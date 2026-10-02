using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attack")]
    public int damage = 5;
    public float attackRange = 1.2f;
    public float attackRate = 1f;

    private float nextAttackTime;
    private PlayerHealth playerHealth;

    private void Start()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerHealth =
                player.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (playerHealth == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            playerHealth.transform.position
        );

        if (
            distance <= attackRange &&
            Time.time >= nextAttackTime
        )
        {
            playerHealth.TakeDamage(damage);

            nextAttackTime =
                Time.time + attackRate;
        }
    }
}