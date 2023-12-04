using UnityEngine.InputSystem;

public interface IPaddle
{
    public void SetServe();
    public void Serve();
    public void Return(InputAction.CallbackContext callbackContext);
}