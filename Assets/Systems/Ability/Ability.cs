using System.Collections.Generic;
using DG.Tweening;
using Kuroneko.UtilityDelivery;
using MEC;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] private float castTime;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float durationTime;

    private float _cooldownTimer;
    private float _durationTimer;
    private bool _cooldown = false;
    private bool _active = false;
    private bool _casting = false;
    private CoroutineHandle _castRoutine;

    public void Init(PlayerInfo info)
    {
        gameObject.SetLayerRecursively(ChaosPongHelper.GetTeamLayer(info.teamSide));
        Debug.Log($"Duration: {durationTime} Cooldown: {cooldownTime}");
    }

    protected void ProcessInput()
    {
        if(CanActivate())
        {
            Timing.KillCoroutines(_castRoutine);
            _castRoutine = Timing.RunCoroutine(CastRoutine().CancelWith(gameObject), Segment.RealtimeUpdate);
        }
    }

    public AbilityInfo GetInfo()
    {
        return new AbilityInfo(CanActivate(), _durationTimer, _cooldownTimer, durationTime, cooldownTime);
    }

    protected virtual bool CanActivate()
    {
        return !_cooldown && !_active && !_casting;
    }

    private IEnumerator<float> CastRoutine()
    {
        _casting = true;
        yield return Timing.WaitForSeconds(castTime);
        _casting = false;
        Timing.RunCoroutine(ActivateRoutine().CancelWith(gameObject), Segment.RealtimeUpdate);
    }

    private IEnumerator<float> ActivateRoutine()
    {
        _active = true;
        Activate();
        DurationTimer();
        yield return Timing.WaitForSeconds(durationTime);
        _active = false;
        Deactivate();
        Timing.RunCoroutine(CooldownRoutine().CancelWith(gameObject), Segment.RealtimeUpdate);
    }

    private IEnumerator<float> CooldownRoutine()
    {
        // Debug.Log($"Cooldown! {cooldownTime}");
        _cooldown = true;
        CountdownTimer();
        yield return Timing.WaitForSeconds(cooldownTime);
        _cooldown = false;
        // Debug.Log("Cooldown Ended!");
    }

    private void CountdownTimer()
    {
        _cooldownTimer = cooldownTime;
        DOTween.To(() => _cooldownTimer, x => _cooldownTimer = x, 0f, cooldownTime)
            .SetUpdate(UpdateType.Normal, true);
    }

    private void DurationTimer()
    {
        _durationTimer = durationTime;
        DOTween.To(() => _durationTimer, x => _durationTimer = x, 0f, durationTime)
            .SetUpdate(UpdateType.Normal, true);
    }
    
    protected abstract void Activate();
    protected abstract void Deactivate();
}