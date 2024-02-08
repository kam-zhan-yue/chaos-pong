using UnityEngine.InputSystem;

public interface IPaddle
{
    public void SetServe();
    public void Return(TeamSide teamSide);
}