using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats =
                other.GetComponent<PlayerStats>();

            stats.AddCoin();

            Destroy(gameObject);
        }
    }
}