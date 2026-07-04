using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int coinsCollected = 0;
    private int score = 0;

    public void AddCoin()
    {
        coinsCollected++;

        score += 100;

        Debug.Log(
            "Coins: " + coinsCollected + 
            " | Score: " + score
            );
    }
}