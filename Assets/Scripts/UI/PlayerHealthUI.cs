using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth playerHealth;

    [SerializeField]
    private TMP_Text healthText;

    private void Awake()
    {
        if (healthText == null)
        {
            healthText =
                GetComponent<TMP_Text>();
        }
    }

    private void Start()
    {
        if (playerHealth == null)
        {
            playerHealth =
                FindAnyObjectByType<PlayerHealth>();
        }

        UpdateHealthText();
    }

    private void Update()
    {
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        if (
            playerHealth == null ||
            healthText == null
        )
        {
            return;
        }

        healthText.text =
            playerHealth.CurrentHealth +
            " / " +
            playerHealth.MaxHealth;
    }
}