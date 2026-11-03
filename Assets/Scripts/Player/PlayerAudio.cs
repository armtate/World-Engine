
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Movement Sounds")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landingSound;

    [Header("Footstep Settings")]
    [SerializeField] private float footstepInterval = 0.3f;
    [SerializeField] private float minimumMoveSpeed = 0.1f;

    [Header("Sound Variation")]
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    private PlayerMovement movement;
    private float footstepTimer;
    private bool wasGrounded;
    private bool initialized;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    private void Update()
    {
        if (movement == null)
            return;

        bool grounded = movement.IsGrounded;

        if (!initialized)
        {
            wasGrounded = grounded;
            initialized = true;
        }

        if (grounded && !wasGrounded)
        {
            PlayLanding();
        }

        wasGrounded = grounded;

        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        bool isMoving =
            movement.IsGrounded &&
            movement.HorizontalSpeed > minimumMoveSpeed;

        if (!isMoving)
        {
            footstepTimer = 0f;
            return;
        }

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f)
        {
            PlayRandomFootstep();
            footstepTimer = footstepInterval;
        }
    }

    private void PlayRandomFootstep()
    {
        if (footstepSounds == null ||
            footstepSounds.Length == 0)
            return;

        int index = Random.Range(0, footstepSounds.Length);
        PlaySound(footstepSounds[index]);
    }

    public void PlayJump()
    {
        PlaySound(jumpSound);
    }

    public void PlayLanding()
    {
        PlaySound(landingSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null || audioSource == null)
            return;

        // A slight pitch variation prevents repetition.
        float pitch = Random.Range(minPitch, maxPitch);

        // PlayOneShot allows footsteps and landing
        // sounds to overlap.
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip);
    }
}
