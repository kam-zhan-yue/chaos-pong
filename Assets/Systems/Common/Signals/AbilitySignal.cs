using Signals;

public class AbilitySignal
{
    public Signal<bool> interactive = new();
    public Signal<float> cooldown = new();

    public void Update(IAbility ability)
    {
        interactive.Value = ability.Interactive();
        cooldown.Value = ability.Cooldown();
    }
}