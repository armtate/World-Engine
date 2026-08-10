using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attack")]
    public int damage = 1;
    public float attackRange = 1.2f;
    public float attackRate = 1f;

    private float nextAttackTime;
    private PlayerStats playerStats;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
        }
    }

    private void Update()
    {
        if (playerStats == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            playerStats.transform.position
        );

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            playerStats.TakeDamage(damage);
            nextAttackTime = Time.time + attackRate;
        }
    }
}