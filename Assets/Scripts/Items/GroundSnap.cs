using UnityEngine;

public class GroundSnap : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float rayDistance = 20f;
    [SerializeField] private float groundOffset = 0.05f;

    private void Start()
    {
        SnapToGround();
    }

    public void SnapToGround()
    {
        Vector2 origin =
            groundCheck != null
                ? groundCheck.position
                : transform.position;

        RaycastHit2D hit =
            Physics2D.Raycast(
                origin,
                Vector2.down,
                rayDistance,
                groundLayer
            );

        if (!hit.collider)
            return;

        float checkOffset =
            origin.y - transform.position.y;

        Vector3 position = transform.position;

        position.y =
            hit.point.y -
            checkOffset +
            groundOffset;

        transform.position = position;
    }
}