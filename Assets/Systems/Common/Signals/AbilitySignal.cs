using Signals;

public class AbilitySignal
{
    public readonly Signal<bool> interactive = new();
    public readonly Signal<float> duration = new();
    public readonly Signal<float> cooldown = new();
    public float durationTime;
    public float cooldownTime;

    public void Update(IAbility ability)
    {
        if (ability == null)
            return;
        AbilityInfo abilityInfo = ability.GetInfo();
        interactive.Value = abilityInfo.Interactive;
        duration.Value = abilityInfo.Duration;
        cooldown.Value = abilityInfo.Cooldown;
        durationTime = abilityInfo.DurationTime;
        cooldownTime = abilityInfo.CooldownTime;
    }
}