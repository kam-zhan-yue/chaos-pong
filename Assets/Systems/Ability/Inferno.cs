using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inferno : Ability, IAbilitySpecial
{
    [SerializeField] private HitModifier hitModifier = new();
    [SerializeField] private PongModifier pongModifier = new();
    private const float HEIGHT_OFFSET = 0.1f;
    private IPaddle _paddle;
    private IPongFinder _pongFinder;
    private Player _player;
    private Animator _animator;
    private CinemachineStateDrivenCamera _stateCamera;
    private static readonly int ActivateTrigger = Animator.StringToHash("Activate");

    private void Awake()
    {
        Transform parent = transform.parent;
        _player = GetComponentInParent<Player>();
        _paddle = parent.GetComponentInChildren<IPaddle>();
        _pongFinder = parent.GetComponentInChildren<IPongFinder>();
        _animator = GetComponent<Animator>();
        _stateCamera = GetComponentInChildren<CinemachineStateDrivenCamera>();
        _stateCamera.enabled = false;
    }
    
    protected override bool Interactive()
    {
        return base.CanActivate() && _paddle.CanHit() && _player.State == CharacterState.Returning;
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
                FireAtTeam(gameManager.GetBlueTeam());
                break;
            case TeamSide.Red:
                FireAtTeam(gameManager.GetRedTeam());
                break;
        }
    }

    protected override void Deactivate()
    {
        
    }

    private void FireAtTeam(Team team)
    {
        if (team.Characters.Count == 0)
            return;
        Character random = team.Characters.GetRandomElement();

        if (_pongFinder.TryGetPong(out Pong pong))
        {
            pong.SetModifier(pongModifier);
            pong.Launch(random.transform.position, pong.transform.position.y + HEIGHT_OFFSET, teamSide:_player.TeamSide, hitModifier: hitModifier);
            //Set possession to the other team so that they can return it
            pong.ForceSetPossession(ChaosPongHelper.GetOppositeSide(_player.TeamSide));
        }
    }

    public void Activate(InputAction.CallbackContext callbackContext)
    {
        ProcessInput();
    }
}