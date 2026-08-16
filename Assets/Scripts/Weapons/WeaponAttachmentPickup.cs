using UnityEngine;

public class WeaponAttachmentPickup :
    MonoBehaviour,
    IInteractable
{
    [SerializeField]
    private WeaponAttachment attachment;

    [SerializeField]
    private SpriteRenderer highlightRenderer;

    [Header("Highlight")]
    [SerializeField]
    private Color highlightedColor = Color.yellow;

    private Color originalColor;

    public WeaponAttachment Attachment =>
        attachment;

    public string InteractionText
    {
        get
        {
            if (attachment == null)
                return "Pick Up Attachment";

            return "Attach " +
                   attachment.attachmentName;
        }
    }

    private void Awake()
    {
        if (highlightRenderer == null)
        {
            Transform visual =
                transform.Find("Visual");

            if (visual != null)
            {
                highlightRenderer =
                    visual.GetComponent<SpriteRenderer>();
            }

            if (highlightRenderer == null)
            {
                highlightRenderer =
                    GetComponent<SpriteRenderer>();
            }
        }

        if (highlightRenderer != null)
        {
            originalColor =
                highlightRenderer.color;
        }
    }

    public void Interact(
        PlayerWeaponController player)
    {
        if (attachment == null)
            return;

        Weapon weapon =
            player.CurrentWeapon;

        if (weapon == null)
            return;

        weapon.ApplyAttachment(
            attachment
        );

        Destroy(gameObject);
    }

    public void Highlight()
    {
        if (highlightRenderer != null)
        {
            highlightRenderer.color =
                highlightedColor;
        }
    }

    public void Unhighlight()
    {
        if (highlightRenderer != null)
        {
            highlightRenderer.color =
                originalColor;
        }
    }
}