using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Weapon Setup")]
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Weapon startingWeapon;

    [Header("Facing")]
    [SerializeField] private SpriteRenderer playerSprite;

    private Weapon[] weaponSlots = new Weapon[2];
    private int activeSlot = 0;

    private Camera mainCamera;

    public Weapon CurrentWeapon => weaponSlots[activeSlot];

    private void Awake()
    {
        mainCamera = Camera.main;

        if (startingWeapon != null)
        {
            weaponSlots[0] = startingWeapon;
            EquipSlot(0);
        }
    }

    private void Update()
    {
        HandleWeaponSwitching();

        Weapon currentWeapon = CurrentWeapon;

        if (currentWeapon == null)
            return;

        if (Mouse.current == null)
            return;

        Vector2 mouseScreenPosition =
            Mouse.current.position.ReadValue();

        Vector3 mouseWorldPosition =
            mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        UpdateFacing(mouseWorldPosition);

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

    private void UpdateFacing(Vector3 mouseWorldPosition)
    {
        bool facingRight =
            mouseWorldPosition.x >= transform.position.x;

        if (playerSprite != null)
        {
            playerSprite.flipX = !facingRight;
        }

        if (weaponHolder != null)
        {
            Vector3 holderScale = weaponHolder.localScale;

            holderScale.x =
                Mathf.Abs(holderScale.x) *
                (facingRight ? 1f : -1f);

            weaponHolder.localScale = holderScale;
        }
    }

    private void HandleWeaponSwitching()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            EquipSlot(0);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            EquipSlot(1);
        }
    }

    private void EquipSlot(int slot)
    {
        if (slot < 0 || slot >= weaponSlots.Length)
            return;

        if (weaponSlots[slot] == null)
            return;

        activeSlot = slot;

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null)
            {
                weaponSlots[i].gameObject.SetActive(
                    i == activeSlot
                );
            }
        }
    }

    public void PickUpWeapon(Weapon weaponPrefab)
    {
        if (weaponPrefab == null)
            return;

        int emptySlot = FindEmptySlot();

        if (emptySlot != -1)
        {
            Weapon newWeapon =
                CreateWeapon(weaponPrefab);

            weaponSlots[emptySlot] = newWeapon;

            EquipSlot(emptySlot);
        }
        else
        {
            ReplaceCurrentWeapon(weaponPrefab);
        }
    }

    private int FindEmptySlot()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                return i;
            }
        }

        return -1;
    }

    private Weapon CreateWeapon(Weapon weaponPrefab)
    {
        Weapon newWeapon = Instantiate(
            weaponPrefab,
            weaponHolder
        );

        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;

        return newWeapon;
    }

    private void ReplaceCurrentWeapon(Weapon weaponPrefab)
    {
        if (weaponSlots[activeSlot] != null)
        {
            Destroy(
                weaponSlots[activeSlot].gameObject
            );
        }

        weaponSlots[activeSlot] =
            CreateWeapon(weaponPrefab);

        EquipSlot(activeSlot);
    }
}