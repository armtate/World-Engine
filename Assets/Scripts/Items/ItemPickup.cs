using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private SpriteRenderer highlightRenderer;

    [Header("Highlight")]
    [SerializeField]
    private Color highlightedColor =
        Color.yellow;

    private Color originalColor;

    public ItemData ItemData => itemData;

    public string InteractionText
    {
        get
        {
            if (itemData == null)
                return "Pick Up Item";

            return "Pick Up " +
                itemData.itemName;
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
        if (itemData == null)
            return;

        if (player == null)
            return;

        PlayerItemInventory inventory =
            player.GetComponent<PlayerItemInventory>();

        if (inventory == null)
        {
            Debug.LogError(
                "ItemPickup: Player is missing " +
                "PlayerItemInventory!"
            );

            return;
        }

        inventory.AddItem(itemData);

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