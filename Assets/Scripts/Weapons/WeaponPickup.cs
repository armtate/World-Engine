using UnityEngine;

public class WeaponPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private Weapon weaponPrefab;
    [SerializeField] private SpriteRenderer highlightRenderer;

    [Header("Highlight")]
    [SerializeField] private Color highlightedColor = Color.yellow;

    private Color originalColor;

    public string InteractionText
    {
        get
        {
            if (weaponPrefab == null)
                return "Pick Up Weapon";

            return "Pick Up " + weaponPrefab.WeaponName;
        }
    }

    private void Awake()
    {
        if (highlightRenderer == null)
        {
            highlightRenderer = GetComponent<SpriteRenderer>();
        }

        if (highlightRenderer != null)
        {
            originalColor = highlightRenderer.color;
        }
    }

    public void Interact(PlayerWeaponController player)
    {
        if (weaponPrefab == null)
            return;

        player.PickUpWeapon(weaponPrefab);

        Destroy(gameObject);
    }

    public void Highlight()
    {
        if (highlightRenderer != null)
        {
            highlightRenderer.color = highlightedColor;
        }
    }

    public void Unhighlight()
    {
        if (highlightRenderer != null)
        {
            highlightRenderer.color = originalColor;
        }
    }
}