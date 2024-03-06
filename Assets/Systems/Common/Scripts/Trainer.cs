using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Trainer : Character
{
    private IPaddle _paddle;
    
    protected override void Awake()
    {
        base.Awake();
        _paddle = GetComponent<IPaddle>();
    }

    public override void Init(PlayerInfo info)
    {
        base.Init(info);
        _paddle.Init(info.teamSide);
    }
    
    public override void SetStart()
    {
        _paddle.SetStart();
        ServeAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTask ServeAsync(CancellationToken token)
    {
        await UniTask.WaitForSeconds(1f, cancellationToken:token);
        _paddle.Serve();
    }
}
