using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
}