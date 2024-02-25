public class PlayerComputedSignal
{
    public AbilityComputedSignal primaryAbility = new();
    public AbilityComputedSignal secondaryAbility = new();
    public AbilityComputedSignal specialAbility = new();

    public void UpdateDeps(PlayerSignal playerSignal)
    {
        primaryAbility.UpdateDeps(playerSignal.primarySignal);
        secondaryAbility.UpdateDeps(playerSignal.secondarySignal);
        specialAbility.UpdateDeps(playerSignal.specialSignal);
    }
}