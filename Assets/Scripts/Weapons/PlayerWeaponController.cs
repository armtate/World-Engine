using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Weapon currentWeapon;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (currentWeapon == null)
            return;

        if (Mouse.current == null)
            return;

        Vector2 mouseScreenPosition =
            Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition =
            mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        Vector2 direction =
            mouseWorldPosition - currentWeapon.transform.position;

        if (Mouse.current.leftButton.isPressed)
        {
            currentWeapon.Fire(direction);
        }

        if (Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame)
        {
            currentWeapon.StartReload();
        }
    }
}