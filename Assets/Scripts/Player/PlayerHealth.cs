
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header("Death Settings")]
    [SerializeField] private float respawnDelay = 1f;

    [Header("Player Sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Hit Flash")]
    [SerializeField] private Color hitFlashColor = Color.red;
    [SerializeField] private float hitFlashDuration = 0.12f;

    [Header("Death Flash")]
    [SerializeField] private Color deathFlashColor = Color.red;
    [SerializeField] private float flashSpeed = 0.1f;

    [Header("Hit Audio")]
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField, Range(0f, 1f)]
    private float hitVolume = 0.7f;

    [Header("Death Audio")]
    [SerializeField] private AudioClip[] deathSounds;
    [SerializeField, Range(0f, 1f)]
    private float deathVolume = 0.8f;

    [Header("Pitch Variation")]
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    private bool isDead = false;
    private Color originalColor;
    private Coroutine hitFlashCoroutine;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public float HealthPercent
    {
        get
        {
            if (maxHealth <= 0)
                return 0f;

            return (float)currentHealth / maxHealth;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || isDead)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log(
            "PLAYER TAKING DAMAGE: " +
            gameObject.name +
            " | HP: " +
            currentHealth +
            "/" +
            maxHealth
        );

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        PlayRandomSound(hitSounds, hitVolume);

        if (spriteRenderer != null)
        {
            if (hitFlashCoroutine != null)
                StopCoroutine(hitFlashCoroutine);

            hitFlashCoroutine = StartCoroutine(HitFlash());
        }

        PlayerItemInventory inventory =
            GetComponent<PlayerItemInventory>();

        if (inventory != null)
        {
            inventory.OnPlayerDamaged(this);
        }
        else
        {
            Debug.LogWarning(
                "PlayerHealth could not find PlayerItemInventory on " +
                gameObject.name
            );
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || isDead)
            return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log(
            "PLAYER HEALED: " +
            gameObject.name +
            " | HP: " +
            currentHealth +
            "/" +
            maxHealth
        );
    }

    private IEnumerator HitFlash()
    {
        spriteRenderer.color = hitFlashColor;

        yield return new WaitForSecondsRealtime(
            hitFlashDuration
        );

        spriteRenderer.color = originalColor;
        hitFlashCoroutine = null;
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("PLAYER DEAD - RESTARTING");

        if (hitFlashCoroutine != null)
        {
            StopCoroutine(hitFlashCoroutine);
            hitFlashCoroutine = null;
        }

        PlayRandomSound(deathSounds, deathVolume);

        if (spriteRenderer != null)
            StartCoroutine(DeathFlash());

        StartCoroutine(RestartLevel());
    }

    private IEnumerator DeathFlash()
    {
        while (isDead)
        {
            spriteRenderer.color = deathFlashColor;

            yield return new WaitForSecondsRealtime(
                Mathf.Max(0.01f, flashSpeed)
            );

            spriteRenderer.color = originalColor;

            yield return new WaitForSecondsRealtime(
                Mathf.Max(0.01f, flashSpeed)
            );
        }
    }

    private void PlayRandomSound(
        AudioClip[] clips,
        float volume
    )
    {
        if (clips == null || clips.Length == 0)
            return;

        AudioClip clip = clips[
            Random.Range(0, clips.Length)
        ];

        if (clip == null)
            return;

        GameObject soundObject =
            new GameObject("Player SFX");

        AudioSource source =
            soundObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = volume;

        source.pitch = Random.Range(
            Mathf.Max(0.1f, Mathf.Min(minPitch, maxPitch)),
            Mathf.Max(0.1f, Mathf.Max(minPitch, maxPitch))
        );

        source.spatialBlend = 0f;

        source.Play();

        Destroy(
            soundObject,
            clip.length / source.pitch + 0.1f
        );
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSecondsRealtime(
            respawnDelay
        );

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
