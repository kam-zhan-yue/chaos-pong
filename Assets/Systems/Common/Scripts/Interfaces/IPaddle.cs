using UnityEngine.InputSystem;

public interface IPaddle
{
    public void Init(TeamSide teamSide);
    public void SetStart();
    public void Toss();
    public void Serve();
    public void Return();
    public void SetHitModifier(HitModifier modifier);
}