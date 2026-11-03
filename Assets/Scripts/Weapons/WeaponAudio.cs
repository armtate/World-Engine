
using UnityEngine;

public class WeaponAudio : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Weapon Sounds")]
    [SerializeField] private AudioClip gunshotSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip emptySound;

    [Header("Individual Volumes")]
    [SerializeField, Range(0f, 1f)]
    private float gunshotVolume = 0.7f;

    [SerializeField, Range(0f, 1f)]
    private float reloadVolume = 0.6f;

    [SerializeField, Range(0f, 1f)]
    private float emptyVolume = 0.5f;

    [Header("Gunshot Variation")]
    [SerializeField] private float minGunshotPitch = 0.97f;
    [SerializeField] private float maxGunshotPitch = 1.03f;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }

    public void PlayGunshot()
    {
        if (gunshotSound == null)
            return;

        // A new pitch for each gunshot.
        audioSource.pitch = Random.Range(
            minGunshotPitch,
            maxGunshotPitch
        );

        audioSource.PlayOneShot(
            gunshotSound,
            gunshotVolume
        );
    }

    public void PlayReload()
    {
        if (reloadSound == null)
            return;

        audioSource.pitch = 1f;
        audioSource.PlayOneShot(
            reloadSound,
            reloadVolume
        );
    }

    public void PlayEmpty()
    {
        if (emptySound == null)
            return;

        audioSource.pitch = 1f;
        audioSource.PlayOneShot(
            emptySound,
            emptyVolume
        );
    }
}
