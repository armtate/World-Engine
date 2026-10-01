using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text damageText;

    [Header("Movement")]
    [SerializeField] private float lifetime = 0.8f;
    [SerializeField] private float floatSpeed = 0.5f;
    [SerializeField] private float horizontalSpread = 0.5f;

    [Header("Animation")]
    [SerializeField] private float startScale = 1.25f;
    [SerializeField] private float endScale = 0.85f;

    private float timer;
    private Vector3 movementDirection;
    private Color startingColor;

    private void Awake()
    {
        if (damageText == null)
        {
            damageText =
                GetComponent<TMP_Text>();
        }

        if (damageText != null)
        {
            startingColor =
                damageText.color;
        }

        movementDirection = new Vector3(
            Random.Range(
                -horizontalSpread,
                horizontalSpread
            ),
            floatSpeed,
            0f
        );
    }

    public void Setup(float damage)
    {
        if (damageText == null)
            return;

        damageText.text =
            Mathf.RoundToInt(damage).ToString();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float progress =
            Mathf.Clamp01(timer / lifetime);

        transform.position +=
            movementDirection *
            Time.deltaTime;

        float scale =
            Mathf.Lerp(
                startScale,
                endScale,
                progress
            );

        transform.localScale =
            Vector3.one * scale;

        if (damageText != null)
        {
            Color color =
                startingColor;

            color.a =
                Mathf.Lerp(
                    1f,
                    0f,
                    progress
                );

            damageText.color = color;
        }

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}