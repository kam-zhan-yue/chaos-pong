using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blizzard : Ability, IAbilitySpecial
{
    [SerializeField] private float speedModifier;
    [SerializeField] private float pongTimeScale;
    private Player _player;
    private Pong[] _pongs = Array.Empty<Pong>();
    private Animator _animator;
    private CinemachineStateDrivenCamera _stateCamera;
    private static readonly int ActivateTrigger = Animator.StringToHash("Activate");
    
    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _animator = GetComponent<Animator>();
        _stateCamera = GetComponentInChildren<CinemachineStateDrivenCamera>();
        _stateCamera.enabled = false;
    }
    
    protected override bool Interactive()
    {
        return base.CanActivate() && _player.State == CharacterState.Returning;
    }

    protected override void StartCast()
    {
        base.StartCast();
        Time.timeScale = 0f;
        _stateCamera.enabled = true;
        _animator.SetTrigger(ActivateTrigger);
    }

    protected override void EndCast()
    {
        base.EndCast();
        Time.timeScale = 1f;
        _stateCamera.enabled = false;
    }

    protected override void Activate()
    {
        _pongs = FindObjectsOfType<Pong>();
        SetPongTimeScale(pongTimeScale);
        //Get the player's team side
        TeamSide playerSide = _player.TeamSide;
        //Get the opposing team
        TeamSide opposingSide = ChaosPongHelper.GetOppositeSide(playerSide);
        //Find a random player from the opposing team
        //Fire the pong at them at an extremely high speed with very little gravity
        IGameManager gameManager = ServiceLocator.Instance.Get<IGameManager>();
        switch (opposingSide)
        {
            case TeamSide.Blue:
                SlowTeam(gameManager.GetBlueTeam());
                break;
            case TeamSide.Red:
                SlowTeam(gameManager.GetRedTeam());
                break;
        }
    }

    private void SlowTeam(Team team)
    {
        for (int i = 0; i < team.CharacterCount; ++i)
        {
            SlowCharacter(team.Characters[i]);
        }
    }

    private void SlowCharacter(Character character)
    {
        if (character.TryGetComponent(out BuffController buffController) &&
            character.TryGetComponent(out IMovement movement))
        {
            MovementBuff movementBuff = new MovementBuff(movement, speedModifier, durationTime);
            buffController.ApplyBuff(movementBuff);
        }
    }

    protected override void Deactivate()
    {
        SetPongTimeScale(1f);
    }

    private void SetPongTimeScale(float timeScale)
    {
        for (int i = 0; i < _pongs.Length; ++i)
        {
            if(_pongs[i] != null)
                _pongs[i].SetTimeScale(timeScale);
        }
    }

    public void Activate(InputAction.CallbackContext callbackContext)
    {
        ProcessInput();
    }
}