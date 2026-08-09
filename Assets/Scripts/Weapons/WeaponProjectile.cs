using UnityEngine;

public class WeaponProjectile : MonoBehaviour
{
    private Vector2 direction;
    private float damage;
    private float speed;

    public void Initialize(Vector2 direction, float damage, float speed)
    {
        this.direction = direction.normalized;
        this.damage = damage;
        this.speed = speed;
    }

    private void Update()
    {
        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);
    }
}