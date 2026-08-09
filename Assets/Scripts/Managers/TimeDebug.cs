using UnityEngine;

public class TimeDebug : MonoBehaviour
{
    private void Update()
    {
        Debug.Log(
            $"time={Time.time:F2} | " +
            $"unscaled={Time.unscaledTime:F2} | " +
            $"delta={Time.deltaTime:F4} | " +
            $"unscaledDelta={Time.unscaledDeltaTime:F4} | " +
            $"fixedDelta={Time.fixedDeltaTime} | " +
            $"maxDelta={Time.maximumDeltaTime} | " +
            $"scale={Time.timeScale}"
        );
    }
}