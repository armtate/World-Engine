
using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Bobbing Settings")]
    [SerializeField] private float bobHeight = 0.15f;
    [SerializeField] private float bobSpeed = 3f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField, Range(0f, 1f)]
    private float pickupVolume = 0.8f;

    [Header("Coin Combo Settings")]
    [SerializeField] private float comboResetTime = 1.5f;
    [SerializeField] private float pitchIncrease = 0.1f;
    [SerializeField] private float maxComboPitch = 1.5f;

    [Header("Random Pitch Variation")]
    [SerializeField] private float pitchVariation = 0.03f;

    private Vector3 startPosition;
    private float bobOffset;
    private bool collected;

    // Shared between all coins
    private static int comboCount = 0;
    private static float lastCoinTime = -100f;

    private void Start()
    {
        startPosition = transform.position;
        bobOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        float bob =
            Mathf.Sin(Time.time * bobSpeed + bobOffset)
            * bobHeight;

        transform.position =
            startPosition + Vector3.up * bob;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || !other.CompareTag("Player"))
            return;

        PlayerStats stats = other.GetComponent<PlayerStats>();

        if (stats == null)
            return;

        collected = true;
        stats.AddCoin();

        UpdateCombo();
        PlayPickupSound();

        Destroy(gameObject);
    }

    private void UpdateCombo()
    {
        if (Time.time - lastCoinTime > comboResetTime)
        {
            comboCount = 0;
        }

        comboCount++;
        lastCoinTime = Time.time;
    }

    private void PlayPickupSound()
    {
        if (pickupSound == null)
            return;

        GameObject soundObject =
            new GameObject("Coin Pickup SFX");

        soundObject.transform.position =
            transform.position;

        AudioSource source =
            soundObject.AddComponent<AudioSource>();

        source.clip = pickupSound;
        source.volume = pickupVolume;
        source.spatialBlend = 0f;

        float comboPitch =
            1f + (comboCount - 1) * pitchIncrease;

        comboPitch = Mathf.Min(
            comboPitch,
            maxComboPitch
        );

        float randomPitch =
            Random.Range(-pitchVariation, pitchVariation);

        source.pitch = Mathf.Clamp(
            comboPitch + randomPitch,
            0.1f,
            maxComboPitch
        );

        source.Play();

        Destroy(
            soundObject,
            pickupSound.length / source.pitch + 0.1f
        );
    }
}
