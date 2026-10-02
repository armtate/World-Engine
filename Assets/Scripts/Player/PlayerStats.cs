using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int coinsCollected = 0;
    [SerializeField] private int score = 0;

    [SerializeField] private UIManager uiManager;

    public void AddCoin()
    {
        coinsCollected++;
        score += 100;

        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
        }
    }
}