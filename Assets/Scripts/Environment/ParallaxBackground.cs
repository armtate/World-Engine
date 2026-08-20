using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [Range(0f, 1f)]
    [SerializeField] private float parallaxStrength = 0.15f;

    [SerializeField] private bool followVertical = false;

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
    }

    private void LateUpdate()
    {
        if (cameraTransform == null)
            return;

        Vector3 cameraMovement =
            cameraTransform.position - cameraStartPosition;

        float xMovement =
            cameraMovement.x * parallaxStrength;

        float yMovement =
            followVertical
                ? cameraMovement.y * parallaxStrength
                : 0f;

        transform.position =
            startPosition +
            new Vector3(
                xMovement,
                yMovement,
                0f
            );
    }
}