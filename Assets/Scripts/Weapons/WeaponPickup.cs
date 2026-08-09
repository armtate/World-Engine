using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Weapon weaponPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerWeaponController weaponController =
            other.GetComponent<PlayerWeaponController>();

        if (weaponController == null)
            return;

        weaponController.PickUpWeapon(weaponPrefab);

        Destroy(gameObject);
    }
}