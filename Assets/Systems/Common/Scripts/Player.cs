using Cinemachine;
using Kuroneko.UtilityDelivery;
using SuperMaxim.Messaging;
using UnityEngine.InputSystem;

public class Player : Character
{
    private IPaddle _paddle;
    private IMovement _movement;
    private IAbilityPrimary _abilityPrimary;
    private IAbilitySecondary _abilitySecondary;
    private IAbilitySpecial _abilitySpecial;

    private PlayerControls _playerControls;
    private PlayerState _state = PlayerState.Idle;

    public IPaddle Paddle => _paddle;

    public PlayerSignal PlayerSignal { get; } = new();

    protected override void Awake()
    {
        base.Awake();
        _movement = GetComponent<IMovement>();
        _paddle = GetComponentInChildren<IPaddle>();
        _abilityPrimary = GetComponentInChildren<IAbilityPrimary>();
        _abilitySecondary = GetComponentInChildren<IAbilitySecondary>();
        _abilitySpecial = GetComponentInChildren<IAbilitySpecial>();
    }
    
    public override void Init(PlayerInfo info)
    {
        base.Init(info);
        InitControls();
        _paddle.Init(playerInfo.teamSide);
        _abilityPrimary?.Init(playerInfo);
        _abilitySecondary?.Init(playerInfo);
        _abilitySpecial?.Init(playerInfo);
        Payload();
    }

    private void Update()
    {
        PlayerSignal.primarySignal.Update(_abilityPrimary);
        PlayerSignal.secondarySignal.Update(_abilitySecondary);
        PlayerSignal.specialSignal.Update(_abilitySpecial);
    }

    private void Payload()
    {
        PlayerPayload payload = new PlayerPayload
        {
            PlayerInfo = playerInfo
        };
        Messenger.Default.Publish(payload);
    }

    private void InitControls()
    {
        _playerControls = new PlayerControls();
        _playerControls.bindingMask = ChaosPongHelper.GetBindingMask(playerInfo.controlScheme);
        if (playerInfo.controlScheme == ControlScheme.KeyboardSpecial)
        {
            if(_movement != null)
                _playerControls.Player.MoveSpecial.performed += _movement.Move;
            if(_paddle != null)
                _playerControls.Player.HitSpecial.performed += Hit;
        }
        else
        {
            if(_movement != null)
                _playerControls.Player.Move.performed += _movement.Move;
            if(_paddle != null)
                _playerControls.Player.Hit.performed += Hit;
            if (_abilityPrimary != null)
                _playerControls.Player.AbilityPrimary.performed += _abilityPrimary.Activate;
            if (_abilitySecondary != null)
                _playerControls.Player.AbilitySecondary.performed += _abilitySecondary.Activate;
            if (_abilitySpecial != null)
                _playerControls.Player.AbilitySpecial.performed += _abilitySpecial.Activate;
        }
        _playerControls.Enable();
    }

    private void Hit(InputAction.CallbackContext callbackContext)
    {
        switch (_state)
        {
            //Do nothing
            case PlayerState.Idle:
                break;
            //Throw the pong into the air and wait for next input
            case PlayerState.Starting:
                _paddle.Toss();
                _state = PlayerState.Serving;
                break;
            //Serve the pong
            case PlayerState.Serving:
                _movement.SetActive(true);
                _paddle.Serve();
                _state = PlayerState.Returning;
                break;
            //Return the pong
            case PlayerState.Returning:
                _paddle.Return();
                break;
        }
    }

    public override void SetStart()
    {
        _paddle.SetStart();
        _state = PlayerState.Starting;
        _movement.SetActive(false);
    }
    
    private void OnDestroy()
    {
        _playerControls.Dispose();
    }
}
