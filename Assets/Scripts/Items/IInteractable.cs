public interface IInteractable
{
    string InteractionText { get; }

    void Interact(PlayerWeaponController player);

    void Highlight();

    void Unhighlight();
}