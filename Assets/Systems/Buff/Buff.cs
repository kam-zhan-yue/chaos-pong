public abstract class Buff
{
    private readonly float _duration;
    private float _durationTimer;
    public bool Expired { get; private set; }

    protected Buff(float duration)
    {
        _duration = duration;
        Expired = false;
    }
    
    public virtual void Apply()
    {
        _durationTimer = _duration;
        Expired = false;
        ApplyEffect();
    }

    protected abstract void ApplyEffect();
    protected abstract void RemoveEffect();

    public virtual void Tick(float deltaTime)
    {
        if (Expired)
            return;
        _durationTimer -= deltaTime;
        if (_durationTimer <= 0f)
        {
            Expire();
        }
    }

    protected virtual void Expire()
    {
        Expired = true;
        RemoveEffect();
    }
}