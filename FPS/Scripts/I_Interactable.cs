
public enum FPS_InteractionType
{
    Equippable,
    Button
}

public interface I_Interactable {
    
    void Interact();
    
    FPS_InteractionType GetInteraction();
}
