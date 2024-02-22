using UnityEngine.InputSystem;

public interface IAbility
{
    public void Activate(InputAction.CallbackContext callbackContext);
}