using Signals;

public class AbilityComputedSignal
{
    public BoolToBoolSignal interactive = new BoolToBoolSignal(new Signal<bool>());
    public FloatToFloatSignal cooldown = new FloatToFloatSignal(new Signal<float>());

    public void UpdateDeps(AbilitySignal abilitySignal)
    {
        interactive.UpdateDeps(abilitySignal.interactive);
        cooldown.UpdateDeps(abilitySignal.cooldown);
    }
}