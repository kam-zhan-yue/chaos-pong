public struct AbilityInfo
{
    public bool Interactive { get; }
    public float Duration { get; }
    public float Cooldown { get; }
    public float DurationTime { get; }
    public float CooldownTime { get; }

    public AbilityInfo(bool interactive, float duration, float cooldown, float durationTime, float cooldownTime)
    {
        Interactive = interactive;
        Duration = duration;
        Cooldown = cooldown;
        DurationTime = durationTime;
        CooldownTime = cooldownTime;
    }
}