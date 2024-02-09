using UnityEngine.InputSystem;

public interface IMovement
{
    public void Move(InputAction.CallbackContext callbackContext);
    public void SetActive(bool active);
}