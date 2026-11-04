
using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public static int ActiveEnemyCount { get; private set; }

    [Header("Health")]
    [SerializeField] private int maxHealth = 5;

    [Header("Damage Number")]
    [SerializeField]
    private Vector3 damageNumberOffset =
        new Vector3(0f, -2f, 0f);

    [Header("Hit Flash")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.1f;

    [Header("Random Enemy Colors")]
    [SerializeField]
    private Color[] possibleColors =
    {
        new Color(0.9f, 0.3f, 0.3f),
        new Color(0.3f, 0.8f, 0.4f),
        new Color(0.3f, 0.6f, 0.9f),
        new Color(0.8f, 0.4f, 0.9f),
        new Color(0.9f, 0.7f, 0.3f)
    };

    [Header("Enemy Audio")]
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip[] deathSounds;
    [SerializeField, Range(0f, 1f)]
    private float soundVolume = 0.8f;

    [Header("Pitch Variation")]
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    private int currentHealth;
    private Color originalColor;
    private Coroutine flashCoroutine;
    private bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            if (possibleColors != null && possibleColors.Length > 0)
            {
                spriteRenderer.color =
                    possibleColors[Random.Range(0, possibleColors.Length)];
            }

            originalColor = spriteRenderer.color;
        }
    }

    private void OnEnable()
    {
        ActiveEnemyCount++;
    }

    private void OnDisable()
    {
        ActiveEnemyCount--;
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        int actualDamage = Mathf.RoundToInt(damage);

        if (actualDamage <= 0)
            return;

        currentHealth -= actualDamage;

        SpawnDamageNumber(actualDamage);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        PlayRandomSound(hitSounds);

        if (spriteRenderer != null)
        {
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashOnHit());
        }
    }

    private IEnumerator FlashOnHit()
    {
        spriteRenderer.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }

    private void SpawnDamageNumber(int damage)
    {
        if (DamageNumberSpawner.Instance == null)
            return;

        Vector3 spawnPosition =
            transform.position + damageNumberOffset;

        DamageNumberSpawner.Instance.Spawn(
            damage,
            spawnPosition
        );
    }


    private void PlayRandomSound(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0)
            return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];

        if (clip == null)
            return;

        GameObject soundObject = new GameObject("Enemy SFX");
        soundObject.transform.position = transform.position;

        AudioSource source = soundObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.volume = soundVolume;
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


    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        PlayRandomSound(deathSounds);

        Destroy(gameObject);
    }
}
