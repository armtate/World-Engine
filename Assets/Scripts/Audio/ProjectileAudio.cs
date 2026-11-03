
using UnityEngine;

public class ProjectileAudio : MonoBehaviour
{
    [Header("Impact Sounds")]
    [SerializeField] private AudioClip enemyHitSound;
    [SerializeField] private AudioClip wallHitSound;

    [Header("Individual Volumes")]
    [SerializeField, Range(0f, 1f)]
    private float enemyHitVolume = 0.7f;

    [SerializeField, Range(0f, 1f)]
    private float wallHitVolume = 0.5f;

    [Header("Pitch Variation")]
    [SerializeField] private float minPitch = 0.97f;
    [SerializeField] private float maxPitch = 1.03f;

    public void PlayEnemyHit()
    {
        PlayImpact(enemyHitSound, enemyHitVolume);
    }

    public void PlayWallHit()
    {
        PlayImpact(wallHitSound, wallHitVolume);
    }

    private void PlayImpact(AudioClip clip, float volume)
    {
        if (clip == null)
            return;

        GameObject soundObject =
            new GameObject("Projectile Impact SFX");

        soundObject.transform.position =
            transform.position;

        AudioSource source =
            soundObject.AddComponent<AudioSource>();

        source.playOnAwake = false;
        source.spatialBlend = 0f;
        source.pitch = Random.Range(minPitch, maxPitch);
        source.volume = Mathf.Clamp01(volume);

        source.clip = clip;
        source.Play();

        float duration =
            clip.length / Mathf.Max(0.01f, source.pitch);

        Destroy(soundObject, duration + 0.1f);
    }
}
