using UnityEngine.InputSystem;

public interface IAbility
{
    public void Activate(InputAction.CallbackContext callbackContext);
    public bool Interactive();
    public float Cooldown();
}