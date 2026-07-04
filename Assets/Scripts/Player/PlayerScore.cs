using TMPro;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private int coinsCollected = 0;
    [SerializeField] private int score = 0;

    [SerializeField] private UIManager uiManager;

    public void AddCoin()
    {
        coinsCollected++;
        score += 100;

        uiManager.UpdateScore(score);
    }

    ScenesUsingAsset TMPro;
}