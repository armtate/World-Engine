using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int score = 0;

    public void AddCoin()
    {
        score++;

        Debug.Log("Score: " + score);
    }
}