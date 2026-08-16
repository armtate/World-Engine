using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private TMP_Text interactionText;
    [SerializeField] private ItemInfoUI itemInfoUI;

    private IInteractable currentInteractable;

    private void Update()
    {
        if (currentInteractable == null)
        {
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }

            return;
        }

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(true);

            interactionText.text =
                "[E] " +
                currentInteractable.InteractionText;
        }

        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            PlayerWeaponController weaponController =
                GetComponent<PlayerWeaponController>();

            if (weaponController != null)
            {
                currentInteractable.Interact(
                    weaponController
                );
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable =
            other.GetComponent<IInteractable>();

        if (interactable == null)
            return;

        if (currentInteractable != null)
        {
            currentInteractable.Unhighlight();
        }

        currentInteractable = interactable;
        currentInteractable.Highlight();

        ShowItemInfo(interactable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable =
            other.GetComponent<IInteractable>();

        if (interactable == null)
            return;

        if (interactable != currentInteractable)
            return;

        currentInteractable.Unhighlight();
        currentInteractable = null;

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }

        if (itemInfoUI != null)
        {
            itemInfoUI.Hide();
        }
    }

    private void ShowItemInfo(
    IInteractable interactable)
    {
        if (itemInfoUI == null)
            return;

        PlayerWeaponController controller =
            GetComponent<PlayerWeaponController>();

        WeaponPickup weaponPickup =
            interactable as WeaponPickup;

        if (weaponPickup != null)
        {
            if (controller == null)
                return;

            itemInfoUI.ShowWeapon(
                weaponPickup.WeaponPrefab,
                controller.CurrentWeapon
            );

            return;
        }

        ItemPickup itemPickup =
            interactable as ItemPickup;

        if (itemPickup != null)
        {
            itemInfoUI.ShowItem(
                itemPickup.ItemData
            );

            return;
        }

        WeaponAttachmentPickup attachmentPickup =
            interactable
                as WeaponAttachmentPickup;

        if (attachmentPickup != null)
        {
            itemInfoUI.ShowAttachment(
                attachmentPickup.Attachment
            );
        }
    }
}