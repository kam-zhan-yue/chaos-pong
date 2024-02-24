using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] private float castTime;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float durationTime;

    private bool _cooldown = false;
    private bool _active = false;
    private bool _casting = false;
    private CoroutineHandle _castRoutine;

    protected void ProcessInput()
    {
        if(CanActivate())
        {
            Timing.KillCoroutines(_castRoutine);
            _castRoutine = Timing.RunCoroutine(CastRoutine().CancelWith(gameObject), Segment.RealtimeUpdate);
        }
    }

    protected virtual bool CanActivate()
    {
        return !_cooldown && !_active && !_casting;
    }

    private IEnumerator<float> CastRoutine()
    {
        Debug.Log("Casting!");
        _casting = true;
        yield return Timing.WaitForSeconds(castTime);
        _casting = false;
        Timing.RunCoroutine(ActivateRoutine().CancelWith(gameObject), Segment.RealtimeUpdate);
    }

    private IEnumerator<float> ActivateRoutine()
    {
        Debug.Log("Activating!");
        _active = true;
        Activate();
        yield return Timing.WaitForSeconds(durationTime);
        _active = false;
        Debug.Log("Deactivate!");
        Deactivate();
        Timing.RunCoroutine(CooldownRoutine().CancelWith(gameObject), Segment.RealtimeUpdate);
    }

    private IEnumerator<float> CooldownRoutine()
    {
        Debug.Log($"Cooldown! {cooldownTime}");
        _cooldown = true;
        yield return Timing.WaitForSeconds(cooldownTime);
        _cooldown = false;
        Debug.Log("Cooldown Ended!");
    }
    
    protected abstract void Activate();
    protected abstract void Deactivate();
}