using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private PlayerWeaponController weaponController;
    [SerializeField] private TMP_Text ammoText;

    private void Update()
    {
        if (weaponController == null || ammoText == null)
            return;

        Weapon weapon = weaponController.CurrentWeapon;

        if (weapon == null)
        {
            ammoText.text = "-- / --";
            return;
        }

        if (weapon.IsReloading)
        {
            ammoText.text = "RELOADING...";
        }
        else
        {
            ammoText.text =
                weapon.CurrentAmmo + " / " + weapon.ReserveAmmo;
        }
    }
}