using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    private int currentThreatLevel = 1;

    private float elapsedTime = 0f;

    private float nextThreatIncreaseTime;

    [SerializeField]
    private float threatIncreaseInterval = 30f;

    private void Start()
    {
        nextThreatIncreaseTime = threatIncreaseInterval;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= nextThreatIncreaseTime)
        {
            currentThreatLevel++;

            Debug.Log("Threat Level Increased to " + currentThreatLevel);

            nextThreatIncreaseTime += threatIncreaseInterval;
        }
    }
}