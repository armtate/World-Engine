
using UnityEngine;

public class PickupAudio : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Pickup Sounds")]
    [SerializeField] private AudioClip weaponPickupSound;
    [SerializeField] private AudioClip itemPickupSound;
    [SerializeField] private AudioClip attachmentPickupSound;

    [Header("Individual Volumes")]
    [SerializeField, Range(0f, 1f)]
    private float weaponPickupVolume = 0.7f;

    [SerializeField, Range(0f, 1f)]
    private float itemPickupVolume = 0.7f;

    [SerializeField, Range(0f, 1f)]
    private float attachmentPickupVolume = 0.7f;

    [Header("Pitch Variation")]
    [SerializeField] private float minPitch = 0.97f;
    [SerializeField] private float maxPitch = 1.03f;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    public void PlayWeaponPickup()
    {
        PlaySound(weaponPickupSound, weaponPickupVolume);
    }

    public void PlayItemPickup()
    {
        PlaySound(itemPickupSound, itemPickupVolume);
    }

    public void PlayAttachmentPickup()
    {
        PlaySound(
            attachmentPickupSound,
            attachmentPickupVolume
        );
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (clip == null || audioSource == null)
            return;

        audioSource.pitch = Random.Range(minPitch, maxPitch);

        audioSource.PlayOneShot(
            clip,
            Mathf.Clamp01(volume)
        );
    }
}
