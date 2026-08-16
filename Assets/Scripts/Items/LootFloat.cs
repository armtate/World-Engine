using UnityEngine;

public class LootFloat : MonoBehaviour
{
    [SerializeField] private Transform visual;
    [SerializeField] private float floatHeight = 0.15f;
    [SerializeField] private float floatSpeed = 2f;

    private Vector3 startLocalPosition;

    private void Awake()
    {
        if (visual == null)
        {
            visual = transform.Find("Visual");
        }

        if (visual != null)
        {
            startLocalPosition = visual.localPosition;
        }
    }

    private void Update()
    {
        if (visual == null)
            return;

        float offset =
            Mathf.Sin(Time.time * floatSpeed) *
            floatHeight;

        visual.localPosition =
            startLocalPosition +
            new Vector3(0f, offset, 0f);
    }
}