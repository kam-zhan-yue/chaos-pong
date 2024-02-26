using UnityEngine.InputSystem;

public interface IAbility
{
    public void Init(PlayerInfo playerInfo);
    public void Activate(InputAction.CallbackContext callbackContext);
    public AbilityInfo GetInfo();
}