using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform cameraTransform;

    [Header("Horizontal Parallax")]
    [Range(0f, 1f)]
    [SerializeField] private float horizontalParallax = 0.15f;

    [Header("Vertical Parallax")]
    [SerializeField] private bool followVertical = true;

    [Range(0f, 1f)]
    [SerializeField] private float verticalParallax = 0.05f;

    private Vector3 startPosition;
    private Vector3 cameraStartPosition;

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        startPosition = transform.position;

        if (cameraTransform != null)
        {
            cameraStartPosition = cameraTransform.position;
        }
        else
        {
            Debug.LogWarning(
                "ParallaxBackground: No camera was assigned or found."
            );
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        Vector3 cameraMovement =
            cameraTransform.position - cameraStartPosition;

        float xMovement =
            cameraMovement.x * horizontalParallax;

        float yMovement = 0f;

        if (followVertical)
        {
            // Follow most of the camera's vertical movement,
            // while retaining a small parallax effect.
            yMovement =
                cameraMovement.y * (1f - verticalParallax);
        }

        transform.position =
            startPosition +
            new Vector3(
                xMovement,
                yMovement,
                0f
            );
    }
}