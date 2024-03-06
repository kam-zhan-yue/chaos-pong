using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
    [SerializeField] protected float durationTime;

    private float _cooldownTimer;
    private float _durationTimer;
    private bool _cooldown = false;
    private bool _active = false;
    private bool _casting = false;

    public void Init(PlayerInfo info)
    {
        gameObject.SetLayerRecursively(ChaosPongHelper.GetTeamLayer(info.teamSide));
    }

    protected void ProcessInput()
    {
        if(Interactive())
        {
            AbilityAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }
    }

    private async UniTask AbilityAsync(CancellationToken token)
    {
        await CastAsync(token);
        await ActivateAsync(token);
        await CooldownAsync(token);
    }

    private async UniTask CastAsync(CancellationToken token)
    {
        StartCast();
        await UniTask.WaitForSeconds(castTime, true,  cancellationToken:token);
        EndCast();
    }

    private async UniTask ActivateAsync(CancellationToken token)
    {
        _active = true;
        Activate();
        _durationTimer = durationTime;
        while (_durationTimer > 0f)
        {
            _durationTimer -= Time.unscaledDeltaTime;
            await UniTask.NextFrame(cancellationToken:token);
        }
        _active = false;
        Deactivate();
    }

    private async UniTask CooldownAsync(CancellationToken token)
    {
        _cooldown = true;
        _cooldownTimer = cooldownTime;
        while (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.unscaledDeltaTime;
            await UniTask.NextFrame(cancellationToken:token);
        }
        _cooldown = false;
    }
    

    public AbilityInfo GetInfo()
    {
        return new AbilityInfo(Interactive(), _durationTimer, _cooldownTimer, durationTime, cooldownTime);
    }

    protected abstract bool Interactive();

    protected virtual bool CanActivate()
    {
        return !_cooldown && !_active && !_casting;
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

    protected virtual void StartCast()
    {
        _casting = true;
    }

    protected virtual void EndCast()
    {
        _casting = false;
    }
    
    protected abstract void Activate();
    protected abstract void Deactivate();
}