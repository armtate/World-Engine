using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int health = 3;
    [SerializeField] private int coinsCollected = 0;
    [SerializeField] private int score = 0;

    [SerializeField] private UIManager uiManager;
 
    public void AddCoin()
    {
        coinsCollected++;
        score += 100;

        uiManager.UpdateScore(score);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log("Health: " + health);

        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void Heal(int amount)
    {
        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        Debug.Log("Health: " + health);
    }
}